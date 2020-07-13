﻿using SelfMonitoringApp.Models;
using SelfMonitoringApp.Models.Base;
using SelfMonitoringApp.ViewModels.Base;

using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SelfMonitoringApp.ViewModels
{
    public class SleepViewModel : ViewModelBase, INavigationViewModel
    {
        private readonly SleepModel _sleepModel;
        private readonly bool _editing;

        public const string NavigationNodeName = "sleep";
        public event EventHandler ModelShed;
        public Command SaveLogCommand { get; private set; }

        private TimeSpan _sleepStart;
        public TimeSpan SleepStart
        {
            get => _sleepStart;
            set
            {
                if (_sleepStart == value)
                    return;

                _sleepStart = value;
                TotalSleep = GetSleep();
                NotifyPropertyChanged();
            }
        }

        public double TotalSleep
        {
            get => _sleepModel.TotalSleep;
            set
            {
                if (_sleepModel.TotalSleep == value)
                    return;

                _sleepModel.TotalSleep = value;
                NotifyPropertyChanged();
            }
        }

        private TimeSpan _sleepEnd;
        public TimeSpan SleepEnd
        {
            get => _sleepEnd;
            set
            {
                if (_sleepEnd == value)
                    return;

                _sleepEnd = value;
                TotalSleep = GetSleep();
                NotifyPropertyChanged();
            }
        }

        private DateTime _sleepEndDate = DateTime.Now;
        public DateTime SleepEndDate
        {
            get => _sleepEndDate;
            set
            {
                if (_sleepEndDate == value)
                    return;

                _sleepEndDate = value;
                TotalSleep = GetSleep();
                NotifyPropertyChanged();
            }
        }

        private DateTime _sleepStartDate = DateTime.Now.AddDays(-1);
        public DateTime SleepStartDate
        {
            get => _sleepStartDate;
            set
            {
                if (_sleepStartDate == value)
                    return;

                _sleepStartDate = value;
                TotalSleep = GetSleep();
                NotifyPropertyChanged();
            }
        }

        public bool RememberedDream
        {
            get => _sleepModel.RememberedDream;
            set
            {
                if (_sleepModel.RememberedDream == value)
                    return;

                _sleepModel.RememberedDream = value;
                NotifyPropertyChanged();
            }
        }

        public bool Nightmare
        {
            get => _sleepModel.Nightmare;
            set
            {
                if (_sleepModel.Nightmare == value)
                    return;

                _sleepModel.Nightmare = value;
                NotifyPropertyChanged();
            }
        }

        public string DreamLog
        {
            get => _sleepModel.DreamLog;
            set
            {
                if (_sleepModel.DreamLog == value)
                    return;

                _sleepModel.DreamLog = value;
                NotifyPropertyChanged();
            }
        }

        public double RestRating
        {
            get => _sleepModel.RestRating;
            set
            {
                if (_sleepModel.RestRating == value)
                    return;

                _sleepModel.RestRating = value;
                NotifyPropertyChanged();
            }
        }

        public SleepViewModel(IModel existingModel = null)
        {
            if (existingModel is null)
                _sleepModel = new SleepModel();
            else
            {
                _editing = true;
                _sleepModel = existingModel as SleepModel;
            }
            SaveLogCommand = new Command(async () => await SaveAndPop());
        }

        public IModel RegisterAndGetModel()
        {
            if (_editing)
            {
                _sleepModel.RegisteredTime = DateTime.Now;
            }

            return _sleepModel;
        }

        public double GetSleep()
        {
            var startDateTime = new DateTime(
                year: SleepStartDate.Year, 
                month: SleepStartDate.Month, 
                day: SleepStartDate.Day,
                hour: SleepStart.Hours,
                minute: SleepStart.Minutes,
                second: SleepStart.Seconds
                );

            var endDateTime = new DateTime(
                year: SleepEndDate.Year,
                month: SleepEndDate.Month,
                day: SleepEndDate.Day,
                hour: SleepEnd.Hours,
                minute: SleepEnd.Minutes,
                second: SleepEnd.Seconds
                );

            _sleepModel.SleepEndDate = endDateTime;
            _sleepModel.SleepStartDate = startDateTime;

            return endDateTime.Subtract(startDateTime).TotalHours;
        }

        public async Task SaveAndPop()
        {
            var model = RegisterAndGetModel();
            await _database.AddOrModifyModelAsync(RegisterAndGetModel());
            await _navigator.NavigateBack();
            ModelShed?.Invoke(this, new ModelShedEventArgs(model));
        }
    }
}