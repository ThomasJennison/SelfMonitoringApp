﻿using SelfMonitoringApp.Models;
using SelfMonitoringApp.Navigation;
using SelfMonitoringApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SelfMonitoringApp.ViewModels
{
    public class MealViewModel: NavigatableViewModelBase, INavigationViewModel
    {
        private readonly MealModel _mealModel;
        public const string NavigationNodeName = "meal";

        public string MealSize
        {
            get => _mealModel.MealSize;
            set
            {
                if (_mealModel.MealSize == value)
                    return;

                _mealModel.MealSize = value;
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

        public string Description
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

        public MealViewModel(INavigationService navService, IModel existingMeal = null)
            :base(navService)
        {
            if (existingMeal is null)
                _mealModel= new MealModel();
            else
                _mealModel = existingMeal as MealModel;
        }

        public IModel RegisterAndGetModel()
        {
            _mealModel.RegisteredTime = DateTime.Now;
            return _mealModel;
        }
    }
}
