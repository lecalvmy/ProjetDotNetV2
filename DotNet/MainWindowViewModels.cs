﻿using PricingLibrary.FinancialProducts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using DotNet.Models;
using PricingLibrary.Utilities.MarketDataFeed;
using System.Windows;
using DotNet.Visualization;
using DotNet.ViewModel;

namespace DotNet
{
    internal class MainWindowViewModels : BindableBase
    {
        #region Private fields
        private Graph selectedClasses;
        private UniverseViewModel universeVM;
        private bool tickerStarted;
        #endregion

        #region public fields
        public ObservableCollection<IOption> AvailableOptions { get; private set; }
        public ObservableCollection<IDataFeedProvider> AvailableData { get; private set; }
        public static Graph graphTest { get; set; }
        public Window win;

        #endregion


        #region Public Constructors
        public MainWindowViewModels()
        {
            StartCommand = new DelegateCommand(StartTicker, CanStartTicker);
            universeVM = new UniverseViewModel();
            VanillaCall vanillaCall = new VanillaCall("Vanilla Call", new Share("AIRBUS GROUP SE", "AIR FP    "), UniverseVM.Initializer.Maturity, UniverseVM.Initializer.Strike);
            BasketOption basketOption = new BasketOption("Basket Option", new Share[] { new Share("CREDIT AGRICOLE SA", "ACA FP    "), new Share("AIR LIQUIDE SA", "AI FP     "), new Share("AIRBUS GROUP SE", "AIR FP    ") }, new double[] { 0.3, 0.2, 0.5 }, UniverseVM.Initializer.Maturity, UniverseVM.Initializer.Strike);
            List<IOption> myOptionsList = new List<IOption>() { vanillaCall, basketOption };
            AvailableOptions = new ObservableCollection<IOption>(myOptionsList);

            IDataFeedProvider type1 = new SimulatedDataFeedProvider();
            IDataFeedProvider type2 = new HistoricalDataFeedProvider();
            List<IDataFeedProvider> mydataList = new List<IDataFeedProvider>() { type1,type2 };
            AvailableData = new ObservableCollection<IDataFeedProvider>(mydataList);
            graphTest = GraphTest;
           win = new GraphVisualization();
            /* win.Show();*/
        }
        #endregion

        public UniverseViewModel UniverseVM { get { return universeVM; } }

        public SimulationModel Simulation
        {
            get { return universeVM.Simulation; }
        }

        public Graph GraphTest
        {
            get { return universeVM.GraphVM.Graph; }
        }

        public Graph SelectedClasses
        {
            get { return selectedClasses; }
            set { SetProperty(ref selectedClasses, value); }
        }
        public Window Win
        {
            get { return win; }
            set
            {
                win = value;
                RaisePropertyChanged(nameof(win));
            }
        }

        public DelegateCommand StartCommand { get; private set; }

        public bool TickerStarted
        {
            get { return tickerStarted; }
            set
            {
                SetProperty(ref tickerStarted, value);
                StartCommand.RaiseCanExecuteChanged();

            }

        }
        private bool CanStartTicker()
        {             
            return !TickerStarted;
        }
        private void StartTicker()
        {
            universeVM.Simulation = new SimulationModel(universeVM.Initializer.Option, universeVM.Initializer.TypeData, UniverseVM.Initializer.DebutTest, UniverseVM.Initializer.PlageEstimation, UniverseVM.Initializer.PeriodeRebalancement);
            
            universeVM.UnderlyingUniverse = new Universe(universeVM.Simulation, universeVM.GraphVM.Graph);
            if (win != null)
            {
                win.Close();
            }
            graphTest = universeVM.UnderlyingUniverse.Graph;
            win = new GraphVisualization();
            win.Show();
            TickerStarted = false;
        }

    }
}
