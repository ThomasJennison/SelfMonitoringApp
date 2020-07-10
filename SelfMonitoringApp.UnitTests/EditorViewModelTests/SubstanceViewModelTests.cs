using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SelfMonitoringApp.Models;
using SelfMonitoringApp.Navigation;
using SelfMonitoringApp.Services;
using SelfMonitoringApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SelfMonitoringApp.UnitTests
{
    [TestClass]
    public class SubstanceViewModelTests
    {
        private SubstanceViewModel _newModel;
        private SubstanceViewModel _existingModel;

        private INavigationService _navService;
        private IDatabaseService _dbService;

        [TestInitialize]
        public void Init()
        {
            _newModel = new SubstanceViewModel();
            _existingModel = new SubstanceViewModel(LogSamples.TestSubstance);
        }

        [TestMethod]
        public async Task TestSavingLog()
        {
            await _newModel.SaveAndPop();
            await _existingModel.SaveAndPop();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _navService = null;
            _newModel = null;
            _existingModel = null;
        }
    }
}
