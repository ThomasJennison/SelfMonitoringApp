﻿using SelfMonitoringApp.Services;
using SelfMonitoringApp.Services.Navigation;
using Splat;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SelfMonitoringApp.ViewModels.Base
{
    public class ViewModelBase : PropertyChangedBase
    {
        protected readonly INavigationService _navigator;
        protected readonly IDatabaseService _database;

        public Command BackCommand { get; private set; }
        public Command HomeCommand { get; private set; }

        public string FullNavigationPath { get; set; }

        public ViewModelBase(INavigationService nav = null, IDatabaseService db = null)
        {
            _navigator  = nav ?? (INavigationService)Locator.Current.GetService(typeof(INavigationService));
            _database   = db ?? (IDatabaseService)Locator.Current.GetService(typeof(IDatabaseService));

            BackCommand = new Command(async () => await _navigator.NavigateBack());
            HomeCommand = new Command(async () => await _navigator.NavigateBackToRoot());
        }

        public Task BeforeFirstShown()
        {
            return Task.CompletedTask;
        }

        public Task AfterDismissed()
        {
            return Task.CompletedTask;
        }

    }
}
