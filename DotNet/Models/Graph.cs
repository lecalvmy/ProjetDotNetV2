﻿using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;

namespace DotNet.Models
{
    internal class Graph 
    {
        #region public field
        public string Name { get { return "HedgeVsPorfolio"; } }

        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public SimulationModel GraphSimulation;
        public Func<double, string> YFormatter { get; set; }
        #endregion
        #region Public constructor
        public Graph() {
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Price Option",
                    Values = new ChartValues<double>{}
                },
                new LineSeries
                {
                    Title = "Hedge portfolio",
                    Values = new ChartValues<double> { }
                }
            };
            Labels = new List<string> {};
        }
        #endregion
        #region Public methods

        public void setSimulation(SimulationModel simulation)
        {
            this.GraphSimulation = simulation;
            SeriesCollection[0].Values = new ChartValues<double> { };
            SeriesCollection[1].Values = new ChartValues<double> { };
            Labels = new List<string> { };

            for (int i = 1; i <= simulation.Balancement.Hedge.Count - 1; i++)
            {
                SeriesCollection[0].Values.Add(simulation.Balancement.PriceOption[i]);
                SeriesCollection[1].Values.Add(Convert.ToDouble(simulation.Balancement.Hedge[i]));
                Labels.Add(simulation.Balancement.Dates[i].ToShortDateString());
            }
            YFormatter = value => value.ToString("C");
        }
        #endregion

    }
}