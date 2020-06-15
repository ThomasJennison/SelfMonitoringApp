﻿using SelfMonitoringApp.Navigation;
using SelfMonitoringApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SelfMonitoringApp.ViewModels
{
    public class DataExplorerViewModel : NavigatableViewModelBase, INavigationViewModel
    {
        public const string NavigationNodeName = "data";

        public DataExplorerViewModel(INavigationService navService) 
            : base(navService)
        {
                
        }

        public ObservableCollection<DaySummaryViewModel> DaySummaries { get; private set; }
    }
}
