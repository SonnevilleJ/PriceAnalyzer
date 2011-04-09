﻿using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace LineCharts
{
    public class DataCollection
    {
        private List<DataSeries> dataList;

        public DataCollection()
        {
            dataList = new List<DataSeries>();
        }

        public List<DataSeries> DataList
        {
            get { return dataList; }
            set { dataList = value; }
        }

        public void AddLines(ChartStyle cs)
        {
            int j = 0;
            foreach (DataSeries ds in DataList)
            {
                if (ds.Ticker == "Default Name")
                {
                    ds.Ticker = "DataSeries" + j.ToString();
                }
                ds.AddLinePattern();
                for (int i = 0; i < ds.LineSeries.Points.Count; i++)
                {
                    ds.LineSeries.Points[i] = cs.NormalizePoint(ds.LineSeries.Points[i]);
                    ds.Symbols.AddSymbol(cs.ChartCanvas, ds.LineSeries.Points[i]);
                }
                cs.ChartCanvas.Children.Add(ds.LineSeries);
                j++;
            }
        }
    }
}
