﻿using SelfMonitoringApp.Models;
using SelfMonitoringApp.Navigation;
using SelfMonitoringApp.Services;
using SelfMonitoringApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SelfMonitoringApp.ViewModels
{
    public class DataExplorerViewModel : NavigatableViewModelBase, INavigationViewModel
    {
        public const string NavigationNodeName = "data";
        public ObservableCollection<DaySummaryViewModel> DaySummaries { get; private set; }

        private DateTime _startDate = DateTime.Now;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate == value)
                    return;

                _startDate = value;
                RaiseAllPropertiesChanged();
            }
        }

        private DateTime _endDate = DateTime.Now.AddDays(1);
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate == value)
                    return;

                _endDate = value;
                NotifyPropertyChanged();
            }
        }

        private bool _loading;
        public bool Loading
        {
            get => _loading;
            set
            {
                if (_loading == value)
                    return;

                _loading = value;
                NotifyPropertyChanged();
            }
        }

        public Command LoadDataCommand { get; private set; }

        public DataExplorerViewModel(INavigationService navService) 
            : base(navService)
        {
            LoadDataCommand = new Command(async () => await LoadData());
            LoadData().SafeFireAndForget(false);

            navService.PageIsTop += NavService_PageIsTop;
        }

        private void NavService_PageIsTop(object sender, EventArgs e)
        {
            
        }

        private bool DateInRange(DateTime startDate, DateTime endDate, DateTime checkDate, DateTime filter) =>
            (checkDate >= startDate && checkDate <= endDate) && checkDate.Day == filter.Day &&
            checkDate.Month == filter.Month && checkDate.Year == filter.Year;

        public async Task<ObservableCollection<DaySummaryViewModel>> GetDateFilteredData(DateTime startDate, DateTime endDate)
        {
            var filteredData = new ObservableCollection<DaySummaryViewModel>();
            if (startDate <= endDate)
            {
                List<SleepModel> sleepData = null;
                List<MoodModel> moodData = null;
                List<MealModel> mealData = null; 
                List<SubstanceModel> substanceData = null;
                List<ActivityModel> activityData = null;
                List<Task> loadingTasks = new List<Task>()
                {
                    Task.Run( async ()=> sleepData = await App.Database.GetSleepsAsync()),
                    Task.Run( async ()=> moodData = await App.Database.GetMoodsAsync()),
                    Task.Run( async ()=> mealData = await App.Database.GetMealsAsync()),
                    Task.Run( async ()=> substanceData = await App.Database.GetSubstancesAsync()),
                    Task.Run( async ()=> activityData = await App.Database.GetActivitiesAsync())
                };

                await Task.WhenAll(loadingTasks);

                for (DateTime date = StartDate; date.Date <= EndDate.Date; date = date.AddDays(1))
                {
                    var sleeps = (sleepData.Where(sleep => DateInRange(startDate, endDate, sleep.RegisteredTime, date))).ToList();
                    var moods = (moodData.Where(mood => DateInRange(startDate, endDate, mood.RegisteredTime, date))).ToList();
                    var meals = (mealData.Where(meal => DateInRange(startDate, endDate, meal.RegisteredTime, date))).ToList();
                    var activities = (activityData.Where(activities => DateInRange(startDate, endDate, activities.RegisteredTime, date))).ToList();
                    var substances = (substanceData.Where(substances => DateInRange(startDate, endDate, substances.RegisteredTime, date))).ToList();

                    if (sleeps.Count != 0 || activities.Count != 0 || moods.Count != 0 || meals.Count != 0 || substances.Count != 0)
                        filteredData.Add(new DaySummaryViewModel(sleeps, activities, meals, moods, substances, date, _navigator));
                }
            }
            return filteredData;
        }

        public async Task LoadData()
        {
            Loading = true;

            var data = await GetDateFilteredData(StartDate, EndDate);
            Loading = false;

            // Need to give the filter grid a second to collapse so the cards render full
            await Task.Delay(50);

            DaySummaries = data;
            NotifyPropertyChanged(nameof(DaySummaries));
        }
    }
}
