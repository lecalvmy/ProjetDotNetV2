/*
 AUTHORS
 MYLENE LE CALVEZ
 ALEXANDRE MAZARS
 DANIELLA TEUKENG MOBOU
 ALEXANDRE VOLCIC
 */
using DotNet.Models;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using Prism.Mvvm;
using System;
using System.Collections.Generic;

namespace DotNet.ViewModel
{
    internal class UniverseViewModel : BindableBase
    {
        #region Private Fields

        private GraphViewModel graphVM;
        private SimulationModel simulation;
        private InitializerViewModel initializer;
        private Universe underlyingUniverse;

        #endregion Private Fields

        #region Public Constructors

        public UniverseViewModel()
        {
            initializer = new InitializerViewModel();
            simulation = new SimulationModel(initializer.Option, initializer.TypeData, initializer.debutTest, initializer.PlageEstimation, initializer.PeriodeRebalancement);
            graphVM = new GraphViewModel();
            underlyingUniverse = new Universe(simulation, graphVM.Graph);
        }

        #endregion Public Constructors

        #region Public Properties
        public InitializerViewModel Initializer
        {
            get { return initializer; }
            set { initializer = value; }
        }

        public SimulationModel Simulation
        {
            get { return simulation; }
            set {
                simulation = value;
                RaisePropertyChanged(nameof(Simulation));
            }
        }

        public Universe UnderlyingUniverse
        {
            get { return underlyingUniverse; }
            set
            {
                underlyingUniverse = value;
                RaisePropertyChanged(nameof(UnderlyingUniverse));
            }
        }

        public GraphViewModel GraphVM
        {
            get { return graphVM; }
        }
        
        #endregion Public Properties

    }
}