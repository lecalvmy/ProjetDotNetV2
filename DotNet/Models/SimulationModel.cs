/*
 AUTHORS
 MYLENE LE CALVEZ
 ALEXANDRE MAZARS
 DANIELLA TEUKENG MOBOU
 ALEXANDRE VOLCIC
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;

namespace DotNet.Models
{
    class SimulationModel
    {
        #region private field
        private IOption option;
        private IDataFeedProvider dataFeedProvider;

        private DateTime dateDebut;

        private int plageEstimation;
        private Balancement balancement;
        
        #endregion

        #region Public Constructor

        public SimulationModel(IOption option, IDataFeedProvider dataFeedProvider, DateTime dateDebut, int plageEstimation, int periodeRebalancement)
        {
            this.option = option;
            if (this.option == null)
            {
                MessageBox.Show("Option should not be null");
            }
            this.dataFeedProvider = dataFeedProvider;
            if (this.dataFeedProvider == null){MessageBox.Show("dataFeed should not be null");}
            if (dateDebut == null) { MessageBox.Show("Beginning date should not be null"); }
            if (dateDebut.DayOfWeek.ToString() == "Saturday" || dateDebut.DayOfWeek.ToString() == "Sunday")
                { MessageBox.Show("Beginning date is not a business day"); }
            this.dateDebut = dateDebut;
            if (plageEstimation < 2) { MessageBox.Show("plage estimation should be at least 2"); }
                this.plageEstimation = plageEstimation;
            if (periodeRebalancement <= 0) { MessageBox.Show("Rebalancement period should be positive"); }
            if (option.Strike <= 0) { MessageBox.Show("Strike should be positive"); }
            this.balancement = new Balancement(dataFeedProvider, option, dateDebut, plageEstimation, periodeRebalancement);
        }
        #endregion
        public IOption Option
        {
            get { return option; }
            set { option = value; }
        }

        public IDataFeedProvider DataFeedProvider
        {
            get { return dataFeedProvider; }
            set { dataFeedProvider = value; }
        }

        public double Strike
        {
            get { return option.Strike; }
        }

        public DateTime DateDebut
        {
            get { return dateDebut; }
            set { dateDebut = value; }
        }

        public DateTime DateMaturite
        {
            get { return option.Maturity; }
        }

        public int PlageEstimation
        {
            get { return plageEstimation; }
            set { plageEstimation = value; }
        }

        public double PayOffaMaturite
        {
            get { return balancement.Payoff; }
        }

        public double HedgeMaturity
        {
            get { return Convert.ToDouble(balancement.Hedge.Last()); }
        }
        public double PriceDebut
        {
            get { return balancement.PriceOption[1]; }
        }
        public Balancement Balancement
        {
            get { return balancement; }
        }
        /*
        public List<double> ComparaisonOptionCouverture()
        {
            var rebalancements = GetRebalancement();
            List<double> comparaisons = new List<double>();

            double optionInitiale = rebalancements[0].prixOption();

            foreach (RebalancementModel rebalancement in rebalancements)
            {

            }
            //comparaisons.Add(Math.Abs(rebalancement.prixOption() - rebalancement.ValeurPortefeuille) / optionInitiale);
            //Console.WriteLine(Math.Abs(rebalancement.prixOption() - rebalancement.ValeurPortefeuille) / optionInitiale);

            return comparaisons;
        }*/
    }
}
