/*
 AUTHORS
 MYLENE LE CALVEZ
 ALEXANDRE MAZARS
 DANIELLA TEUKENG MOBOU
 ALEXANDRE VOLCIC
 */
using PricingLibrary.FinancialProducts;
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
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace DotNet
{
    internal class MainWindowViewModels : BindableBase
    {
        #region Private fields
        private Graph selectedClasses;
        private UniverseViewModel universeVM;
        private bool tickerStarted;
        private IOption option;
        #endregion

        #region public fields
        public ObservableCollection<string> AvailableOptions { get; private set; }
        public ObservableCollection<ComponentInfo> AvailableShares { get; private set; }
        public string selectedOptions { get; set;}
        public ObservableCollection<IDataFeedProvider> AvailableData { get; private set; }
        public static Graph graphTest { get; set; }
        public Window win;
        public DelegateCommand StartCommand { get; private set; }
        #endregion


        #region Public Constructors
        public MainWindowViewModels()
        {
            selectedOptions = "vanillaCall";
           
            StartCommand = new DelegateCommand(StartTicker, CanStartTicker);
            universeVM = new UniverseViewModel();

            AvailableOptions = new ObservableCollection<string>()
            {
                "vanillaCall",
                "basketOption",
            };

            AvailableData = new ObservableCollection<IDataFeedProvider>()
            {
                new SimulatedDataFeedProvider(),
                new HistoricalDataFeedProvider(),
                new SemiHistoricDataFeedProvider(),
            };


            AvailableShares = new ObservableCollection<ComponentInfo>()
            {
                new ComponentInfo() {Name = "AIRBUS GROUP SE", Id = "AIR FP    ", IsSelected = true},
                new ComponentInfo() {Name = "CREDIT AGRICOLE SA", Id = "ACA FP    ", IsSelected = false},
                new ComponentInfo() {Name = "AIR LIQUIDE SA", Id = "AI FP     ", IsSelected = false},
                new ComponentInfo() {Name = "ACCOR SA", Id = "AC FP     ", IsSelected = false},
                new ComponentInfo() {Name = "ALSTOM", Id = "ALO FP    ", IsSelected = false},
                new ComponentInfo() {Name = "DANONE", Id = "BN FP     ", IsSelected = false},
                new ComponentInfo() {Name = "BNP PARIBAS", Id = "BNP FP    ", IsSelected = false},
                new ComponentInfo() {Name = "CARREFOUR SA", Id = "CA FP     ", IsSelected = false},
                new ComponentInfo() {Name = "CAP GEMINI", Id = "CAP FP    ", IsSelected = false},
                new ComponentInfo() {Name = "AXA SA", Id = "CS FP     ", IsSelected = false},
                new ComponentInfo() {Name = "EDF", Id = "EDF FP    ", IsSelected = false},
                new ComponentInfo() {Name = "ESSILOR INTERNATIONAL", Id = "EI FP     ", IsSelected = false},
                new ComponentInfo() {Name = "BOUYGUES SA", Id = "EN FP     ", IsSelected = false},
                new ComponentInfo() {Name = "SOCIETE GENERALE SA", Id = "GLE FP    ", IsSelected = false},


            };
            graphTest = GraphTest;
            win = new GraphVisualization();
        }
        #endregion
        #region Set Get
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
        #endregion

        #region public method
        public bool TickerStarted
        {
            get { return tickerStarted; }
            set
            {
                SetProperty(ref tickerStarted, value);
                StartCommand.RaiseCanExecuteChanged();

            }

        }
        #endregion

        #region Private Methods
        private bool CanStartTicker()
        {
            return !TickerStarted;
        }
        private void StartTicker()
        {
            List<Share> selectedShares =  new List<Share>();
            foreach (var comp in AvailableShares)
            {
                if (comp.IsSelected)
                {
                    selectedShares.Add(new Share(comp.Name, comp.Id));
                }
            }
            int length = selectedShares.Count;
            Share[] sharesTab = new Share[length];
            for (int i = 0; i < sharesTab.Length; i++) sharesTab[i] = selectedShares[i];

            if ((selectedOptions == "vanillaCall" || UniverseVM.Initializer.Option is VanillaCall) && length == 1)
            {
                VanillaCall vanillaCall = new VanillaCall(UniverseVM.Initializer.NameOption, sharesTab[0], UniverseVM.Initializer.Maturity, UniverseVM.Initializer.Strike);
                universeVM.Simulation = new SimulationModel(vanillaCall, universeVM.Initializer.TypeData, UniverseVM.Initializer.DebutTest, UniverseVM.Initializer.PlageEstimation, UniverseVM.Initializer.PeriodeRebalancement);
            }
            else if (selectedOptions == "basketOption" && length > 1)
            {
                Random aleatoire = new Random();
                double[] weights = new double[length];

                for (int i = 0; i < length; i++)
                {
                    weights[i] = (double) 1/length;
                }
                BasketOption basketOption = new BasketOption(UniverseVM.Initializer.NameOption, sharesTab, weights, UniverseVM.Initializer.Maturity, UniverseVM.Initializer.Strike);
                universeVM.Simulation = new SimulationModel(basketOption, universeVM.Initializer.TypeData, UniverseVM.Initializer.DebutTest, UniverseVM.Initializer.PlageEstimation, UniverseVM.Initializer.PeriodeRebalancement);
            }
            else
            {
                throw new InvalidOperationException("Vanilla Call supports only one share, and basket options supports at least two shares");
            }
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
<<<<<<< HEAD
        #endregion
=======
>>>>>>> 5809c44713676fd037550476c154fb7cd5733afb
    }
}
