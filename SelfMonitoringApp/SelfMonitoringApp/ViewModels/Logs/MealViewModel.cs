﻿
using SelfMonitoringApp.Models;
using SelfMonitoringApp.Models.Base;
using SelfMonitoringApp.Services;
using SelfMonitoringApp.Services.Navigation;
using SelfMonitoringApp.ViewModels.Base;
    
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SelfMonitoringApp.ViewModels.Logs
{
    public class MealViewModel : ViewModelBase, INavigationViewModel
    {
        private readonly MealModel _mealModel;
        public const string NavigationNodeName = "meal";
        public event EventHandler ModelShed;

        #region Bindings       
        public Command SaveLogCommand { get; private set; }

        private DateTime _logDateTime;
        public DateTime LogDateTime
        {
            get => _logDateTime;
            set
            {
                if (_logDateTime == value)
                    return;

                _logDateTime = value;
                NotifyPropertyChanged();
            }
        }

        private TimeSpan _logTimeSpan;
        public TimeSpan LogTimeSpan
        {
            get => _logTimeSpan;
            set
            {
                if (_logTimeSpan == value)
                    return;

                _logTimeSpan = value;
                NotifyPropertyChanged();
            }
        }

        public string MealSize
        {
            get => _mealModel.MealSize;
            set
            {
                if (_mealModel.MealSize == value)
                    return;

                _mealModel.MealSize= value;
                NotifyPropertyChanged();
            }
        }

        public string MealType
        {
            get => _mealModel.MealType;
            set
            {
                if (_mealModel.MealType == value)
                    return;

                _mealModel.MealType = value;
                NotifyPropertyChanged();
            }
        }

        public string MealName
        {
            get => _mealModel.Description;
            set
            {
                if (_mealModel.Description == value)
                    return;

                _mealModel.Description = value;
                NotifyPropertyChanged();
            }
        }

        public double Satisfaction
        {
            get => _mealModel.Satisfaction;
            set
            {
                if (_mealModel.Satisfaction == value)
                    return;

                _mealModel.Satisfaction = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        public MealViewModel(IModel existingModel = null, INavigationService nav = null, IDatabaseService db = null)
            : base(nav, db)
        {
            if (existingModel is null) // Creating new log
            {
                _mealModel = new MealModel()
                {
                    Satisfaction = 5
                };
                LogDateTime = DateTime.Now;
                LogTimeSpan = new TimeSpan
                (
                    hours   : LogDateTime.Hour,
                    minutes : LogDateTime.Minute,
                    seconds : LogDateTime.Second
                );
            }
            else // Editing Log
            {
                _mealModel = existingModel as MealModel;
                LogDateTime = _mealModel.RegisteredTime;
                LogTimeSpan = new TimeSpan
                (
                    hours    : _mealModel.RegisteredTime.Hour,
                    minutes  : _mealModel.RegisteredTime.Minute,
                    seconds  : _mealModel.RegisteredTime.Second
                );
            }

            SaveLogCommand = new Command(async () => await SaveAndPop());
        }

        public async Task SaveAndPop()
        {
            _mealModel.RegisteredTime = new DateTime
            (
                year   : LogDateTime.Year,
                month  : LogDateTime.Month,
                day    : LogDateTime.Day,
                hour   : LogTimeSpan.Hours,
                minute : LogTimeSpan.Minutes,  
                second : LogTimeSpan.Seconds
            ) ;

            await _database.AddOrModifyModelAsync(_mealModel);
            await _navigator.NavigateBack();
            ModelShed?.Invoke(this, new ModelShedEventArgs(_mealModel));
        }
    }
}
