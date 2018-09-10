using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.ViewModel
{
    class InitializerViewModel
    {
        #region public properties
        public IOption option { get; set; }
        public DateTime debutTest { get; set; }
        public DateTime maturity { get; set; }
        public int plageEstimation { get; set; }
        public double strike { get; set; }
        public IDataFeedProvider typeData { get; set; }
        public int periodeRebalancement { get; set; }
        public string nameOption { get; set; }
        public InitializerViewModel()
        {
            debutTest = DateTime.Now;
            maturity = new DateTime(2019, 6, 7);
            plageEstimation = 15;
            typeData = new SimulatedDataFeedProvider();
            strike = 8;
            periodeRebalancement = 5;
            nameOption = "Vanilla Call";
            option = new VanillaCall(nameOption, new Share("Axa", "1"), maturity, strike);
        }

        public IOption Option
        {
            get { return option; }
            set { option = value; }
        }
        #endregion
        public DateTime DebutTest
        {
            get { return debutTest; }
            set { debutTest = value; }
        }
        public DateTime Maturity
        {
            get { return maturity; }
            set { maturity = value; }
        }

        public int PlageEstimation
        {
            get { return plageEstimation; }
            set { plageEstimation = value; }
        }

        public double Strike
        {
            get { return strike; }
            set { strike = value; }
        }
        public string NameOption
        {
            get { return nameOption; }
            set { nameOption = value; }
        }
        public IDataFeedProvider TypeData
        {
            get { return typeData; }
            set { typeData = value; }
        }

        public int PeriodeRebalancement
        {
            get { return periodeRebalancement; }
            set { periodeRebalancement = value; }
        }
    }
}
