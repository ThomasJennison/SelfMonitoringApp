﻿using SelfMonitoringApp.Models;
using SQLite;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SelfMonitoringApp.Services
{
    public class Database  
    {
        public const string _databaseFilename = "UserLogs.db3";

        private const SQLiteOpenFlags _openFlags = 
            SQLiteOpenFlags.ReadWrite |
            SQLiteOpenFlags.Create | 
            SQLiteOpenFlags.SharedCache;

        private static string _filePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                return Path.Combine(basePath, _databaseFilename);
            }
        }

        private SQLiteAsyncConnection _database;
        private static bool _initialized;

        public Database(){}

        public async Task InitializeAsync()
        {
            _database = new SQLiteAsyncConnection(_filePath, _openFlags);

            var tables = new Type[]
            {
                typeof(MoodModel),
                typeof(MealModel),
                typeof(SleepModel),
                typeof(SubstanceModel),
                typeof(ActivityModel)
            };

            if (!_initialized)
            {
                await _database.CreateTablesAsync(CreateFlags.None, tables).ConfigureAwait(false);
                _initialized = true;
            }
        }

        public Task ClearSpecificDatabase(ModelType modelType) => 
            modelType switch
        {
            ModelType.Activity => _database.DeleteAllAsync<ActivityModel>(),
            ModelType.Meal => _database.DeleteAllAsync<MealModel>(),
            ModelType.Mood => _database.DeleteAllAsync<MoodModel>(),
            ModelType.Substance => _database.DeleteAllAsync<SubstanceModel>(),
            ModelType.Sleep => _database.DeleteAllAsync<SleepModel>(),
            _ => throw new ArgumentException("Model type does not exist in db")
        };

        #region Get Items
        public Task<List<MoodModel>> GetMoodsAsync() => _database.Table<MoodModel>().ToListAsync();
        public Task<List<MealModel>> GetMealsAsync() => _database.Table<MealModel>().ToListAsync();
        public Task<List<SleepModel>> GetSleepsAsync() => _database.Table<SleepModel>().ToListAsync();
        public Task<List<SubstanceModel>> GetSubstancesAsync() => _database.Table<SubstanceModel>().ToListAsync();
        public Task<List<ActivityModel>> GetActivitiesAsync() => _database.Table<ActivityModel>().ToListAsync();
        #endregion

        public Task<int> AddOrModifyModelAsync(IModel model)
        {
            try
            {
                if (model.ID != 0)
                    return _database.UpdateAsync(model);
                else
                    return _database.InsertAsync(model);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw;
            }
        }

        public Task DeleteLog(IModel model) => _database.DeleteAsync(model);

    }
}