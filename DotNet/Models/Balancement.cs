using PricingLibrary.Computations;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Models
{
    class Balancement
    {
        #region public fields
        //public Estimateur estimateur; 
        //public DataFeed data;
        public Portfolio portfolio;
        public Pricer pricer;

        public double volatilite;
        public double[,] matrixCorrelation;
        #endregion
        #region Private fields
        private List<decimal> hedge;
        private List<double> priceOption;
        private List<DateTime> dates;
        private double payoff;
        #endregion

        #region Public constructor
        public Balancement(IDataFeedProvider dataFeedProvider, IOption option, DateTime dateDebut, int plageEstimation, int periodeRebalancement)
        {
            var priceList = dataFeedProvider.GetDataFeed(option, dateDebut);
            volatilite = Estimateur.Volatilite(priceList, plageEstimation, dateDebut.AddDays(plageEstimation), option, dataFeedProvider);
            Console.WriteLine("Volatilite: " + volatilite);
            matrixCorrelation = Estimateur.getCorrMatrix(priceList, plageEstimation, dateDebut.AddDays(plageEstimation));
            Estimateur.dispMatrix(matrixCorrelation);

            payoff = payOffaMaturite(option, priceList);
            pricer = new Pricer();
            hedge = new List<decimal>();
            priceOption = new List<double>();
            dates = new List<DateTime>();

            PricingResults priceDelta = PriceResults(option, priceList[0].PriceList, dateDebut, dataFeedProvider);
            Dictionary<string, double> compo = new Dictionary<string, double> { };
            int i = 0;

            foreach (string id in option.UnderlyingShareIds)
            {
                compo[id] = priceDelta.Deltas[i];
                i += 1;
            }

            DateTime oldBalancement = dateDebut;

            portfolio = new Portfolio(Convert.ToDecimal(priceDelta.Price), compo, priceList[0].PriceList);
            priceOption.Add(priceDelta.Price);
            hedge.Add(portfolio.portfolioValue);
            dates.Add(priceList[0].Date);
            //Traitement des données 

            foreach (DataFeed priceAsset_t in priceList.Skip(1))
            {

                if (DayCount.CountBusinessDays(oldBalancement, priceAsset_t.Date) >= periodeRebalancement || priceAsset_t == priceList.Last())
                {

                    priceDelta = PriceResults(option, priceAsset_t.PriceList, dateDebut, dataFeedProvider);
                    priceOption.Add(priceDelta.Price);
                    //updateCompo
                    portfolio.UpdatePortfolioValue(priceAsset_t, dataFeedProvider.NumberOfDaysPerYear, oldBalancement);
                    i = 0;
                    foreach (string id in option.UnderlyingShareIds)
                    {
                        compo[id] = priceDelta.Deltas[i];
                        i += 1;
                    }
                    hedge.Add(portfolio.portfolioValue);
                    dates.Add(priceAsset_t.Date);
                    portfolio.UpdateLiquidity(priceAsset_t);
                    portfolio.UpdateCompo(compo);
                    oldBalancement = priceAsset_t.Date;
                }
            }
        }
        #endregion
        #region Public methods
        public PricingResults PriceResults(IOption option,Dictionary<string, decimal> priceMarket, DateTime dateDebut, IDataFeedProvider dataFeedProvider)
        {
            Pricer pricer = new Pricer();
            int length = priceMarket.Count;
            double[] spot = new double[length];
            for (int i = 0; i < spot.Length; i++) spot[i] = (double) priceMarket.ElementAt(i).Value;
            if(option is VanillaCall)
            {
                VanillaCall optionVanilla = (VanillaCall)option;
                PricingResults priceDelta = pricer.PriceCall(optionVanilla, dateDebut, dataFeedProvider.NumberOfDaysPerYear, spot[0], volatilite);
                return (priceDelta);
            }
            else
            {
                BasketOption optionBasket = (BasketOption)option;
                double[] volatilities = new double[optionBasket.Weights.Length];
                for (int i = 0; i < optionBasket.Weights.Length; i++) volatilities[i] = volatilite;
                PricingResults priceDelta = pricer.PriceBasket(optionBasket, dateDebut, dataFeedProvider.NumberOfDaysPerYear, spot, volatilities, matrixCorrelation);
                return (priceDelta);
            }
            
        }
        #endregion

        public double payOffaMaturite(IOption option, List<DataFeed> priceList)
        {
            return option.GetPayoff(priceList.Last().PriceList);
        }

        public void Method()
        {
            throw new System.NotImplementedException();
        }

        public double Payoff
        {
            get { return this.payoff; }
        }
        public List<decimal> Hedge
        {
            get { return this.hedge; }
        }
        public List<double> PriceOption
        {
            get { return this.priceOption; }
        }
        public List<DateTime> Dates
        {
            get { return this.dates; }
        }
    }
}
