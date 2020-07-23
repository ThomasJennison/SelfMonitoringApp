﻿using Acr.UserDialogs;
using SelfMonitoringApp.Models;
using SelfMonitoringApp.Models.Base;
using SelfMonitoringApp.Services;
using SelfMonitoringApp.ViewModels.Base;

using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SelfMonitoringApp.ViewModels.Logs
{
    public class SubstanceViewModel: ViewModelBase, INavigationViewModel
    {
        private readonly SubstanceModel _substance;
        public const string NavigationNodeName = "substance";
        public event EventHandler ModelShed;

        #region Bindings
        //Commands
        public Command SaveLogCommand { get; private set; }

        //General Notify
        private DateTime _logTime;
        public DateTime LogTime
        {
            get => _logTime;
            set
            {
                if (_logTime == value)
                    return;

                _logTime = value;
                NotifyPropertyChanged();
            }
        }

        private TimeSpan _logTimeSpan;
        public TimeSpan StartTimeSpan
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

        public string ConsumptionMethod
        {
            get => _substance.ConsumptionMethod;
            set
            {
                if (_substance.ConsumptionMethod == value)
                    return;

                _substance.ConsumptionMethod = value;
                NotifyPropertyChanged();
            }
        }

        public string SubstanceName
        {
            get => _substance.SubstanceName;
            set
            {
                if (_substance.SubstanceName == value)
                    return;

                _substance.SubstanceName = value;
                NotifyPropertyChanged();
            }
        }

        public string Comment
        {
            get => _substance.Comment;
            set
            {
                if (_substance.Comment == value)
                    return;

                _substance.Comment = value;
                NotifyPropertyChanged();
            }
        }

        public double Ammount
        {
            get => _substance.Amount;
            set
            {
                if (_substance.Amount == value)
                    return;

                _substance.Amount = value;
                NotifyPropertyChanged();
            }
        }

        public string Unit
        {
            get => _substance.Unit;
            set
            {
                if (_substance.Unit == value)
                    return;

                _substance.Unit = value;
                NotifyPropertyChanged();
            }
        }

        public double Satisfaction
        {
            get => _substance.Satisfaction;
            set
            {
                if (_substance.Satisfaction == value)
                    return;

                _substance.Satisfaction = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        public SubstanceViewModel(IModel existingModel = null) 
        {
            if (existingModel is null)
            {
                _substance = new SubstanceModel();
                LogTime = DateTime.Now;
                StartTimeSpan = new TimeSpan
                (
                    hours   : LogTime.Hour, 
                    minutes : LogTime.Hour, 
                    seconds : LogTime.Second
                );
            }
            else
            {
                _substance = existingModel as SubstanceModel;
                LogTime = _substance.RegisteredTime;
                StartTimeSpan = new TimeSpan
                (
                    hours   : _substance.RegisteredTime.Hour, 
                    minutes : _substance.RegisteredTime.Minute, 
                    seconds : _substance.RegisteredTime.Second
                );
            }   
            SaveLogCommand = new Command(async ()=> await SaveAndPop());
        }

        public async Task SaveAndPop()
        {
            _substance.RegisteredTime = new DateTime
            (
                year   : LogTime.Year,
                month  : LogTime.Month,
                day    : LogTime.Day,
                hour   : StartTimeSpan.Hours,
                minute : StartTimeSpan.Minutes,
                second : StartTimeSpan.Seconds
            );

            await _database.AddOrModifyModelAsync(_substance);
            await _navigator.NavigateBack();
            ModelShed?.Invoke(this, new ModelShedEventArgs(_substance));
        }
    }
}