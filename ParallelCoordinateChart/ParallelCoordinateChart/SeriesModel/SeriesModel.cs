
using Syncfusion.Maui.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelCoordinateChart
{
    internal class SeriesModel
    {
        private double xValue;
        private double yValue;
        public double XValues
        {
            get => xValue;
            set => xValue = value;
        }

        public double YValues
        {
            get => yValue;
            set => yValue = value;
        }

        public SeriesModel(double x, double y)
        {
            XValues = x;
            YValues = y;
        }
    }
}
