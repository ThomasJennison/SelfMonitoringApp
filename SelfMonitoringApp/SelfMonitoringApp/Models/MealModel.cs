﻿using System;

namespace SelfMonitoringApp.Models
{
    public class MealModel : LogModelBase, IModel
    {
        /// <summary>
        /// User description of their meals size
        /// </summary>
        public string MealSize { get; set; } = string.Empty;
        
        /// <summary>
        /// User description of meal type (snack, dinner, etc)
        /// </summary>
        public string MealType { get; set; } = string.Empty;

        /// <summary>
        /// What the user ate
        /// </summary>
        public string Description { get; set; } = string.Empty;

        public double Satisfaction { get; set; } 

        public MealModel():base (ModelType.Meal) { }
    }
}