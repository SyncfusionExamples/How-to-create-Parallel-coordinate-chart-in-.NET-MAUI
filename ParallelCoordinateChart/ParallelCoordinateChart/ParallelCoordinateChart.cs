using Syncfusion.Maui.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelCoordinateChart
{
    public class ParallelCoordinateChart:SfCartesianChart
    {
        public ParallelCoordinateChart()
        {
            GenerateChart();
        }

        private void GenerateChart()
        {
            var xAxes = GenerateXAxes();
            var yAxes = GenerateYAxesList();
            this.XAxes.Add(xAxes);
            foreach(var yaxis in yAxes)
            {
                this.YAxes.Add(yaxis);
            }

            var paletteBrushes = new List<Brush>
            {
                new SolidColorBrush(Color.FromArgb("#800080")), 
                new SolidColorBrush(Color.FromArgb("#6495ED")),
                new SolidColorBrush(Color.FromArgb("#32CD32")),
                new SolidColorBrush(Color.FromArgb("#FFD700")),
                new SolidColorBrush(Color.FromArgb("#FF6347")),
                new SolidColorBrush(Color.FromArgb("#8A2BE2")), 
            };
            this.PaletteBrushes = paletteBrushes;

            var serieslist = GenerateSeries(yAxes);
            foreach(var series in serieslist)
            {
                this.Series.Add(series);
            }
        }

        private NumericalAxis GenerateXAxes()
        {
            var xAxes = new NumericalAxis()
            {
                Minimum = 0,
                Maximum = 4,
                Interval = 1,
                ShowMajorGridLines = false,
                PlotOffsetStart = 20,
                PlotOffsetEnd = 50,
                CrossesAt = double.MaxValue,
            };
            
            xAxes.LabelCreated += (s, e) =>
            {
                e.Label = e.Position switch
                {
                    0 => "Model",
                    1 => "Horsepower",
                    2 => "Torque",
                    3 => "FuelEfficiency",
                    4 => "Price",
                    _ => string.Empty
                };
            };

            return xAxes;
        }

        private List<NumericalAxis> GenerateYAxesList()
        {
            var viewmodelData = new ViewModel().Source;
            var firstItem = viewmodelData.First();
            var properties = firstItem.GetType().GetProperties();
            var list = new List<NumericalAxis>();
            foreach ( var property in properties)
            {
                if(property.Name == "CarModel")
                {
                    var yaxes = new NumericalAxis()
                    {
                        Minimum = 0,
                        Maximum = 4,
                        Interval = 1,
                        CrossesAt = 0,
                        ShowMajorGridLines = false,
                        LabelsPosition = AxisElementPosition.Inside,
                    };
                    yaxes.LabelCreated += (s, e) =>
                    {
                        e.Label = e.Position switch
                        {
                            0 => "CarA",
                            1 => "CarB",
                            2 => "CarC",
                            3 => "CarD",
                            4 => "CarE",
                            _ => string.Empty
                        };
                    };
                    list.Add(yaxes);
                }
                else if(property.Name == "Horsepower")
                {
                    var yaxes = new NumericalAxis()
                    {
                        Minimum = 100,
                        Maximum = 240,
                        Interval = 20,
                        CrossesAt = 1,
                        ShowMajorGridLines = false,
                        LabelsPosition = AxisElementPosition.Inside,
                    };
                    list.Add(yaxes);
                }
                else if (property.Name == "Torque")
                {
                    var yaxes = new NumericalAxis()
                    {
                        Minimum = 100,
                        Maximum = 500,
                        Interval = 50,
                        CrossesAt = 2,
                        ShowMajorGridLines = false,
                        LabelsPosition = AxisElementPosition.Inside,
                    };
                    list.Add(yaxes);
                }
                else if (property.Name == "FuelEfficiency")
                {
                    var yaxes = new NumericalAxis()
                    {
                        Minimum = 5,
                        Maximum = 40,
                        Interval = 5,
                        CrossesAt = 3,
                        ShowMajorGridLines = false,
                        LabelsPosition = AxisElementPosition.Inside,
                    };
                    list.Add(yaxes);
                }
                else if(property.Name == "Price")
                {
                    var yaxes = new NumericalAxis()
                    {
                        Minimum = 10000,
                        Maximum = 50000,
                        Interval = 5000,
                        CrossesAt = 4,
                        ShowMajorGridLines = false,
                        LabelsPosition = AxisElementPosition.Inside,
                    };
                    list.Add(yaxes);
                }
            }
            return list;
        }

        private List<SplineSeries> GenerateSeries(List<NumericalAxis> yAxes)
        {
            var viewModel = new ViewModel();
            var seriesList = new List<SplineSeries>();
            foreach (var chartModel in viewModel.DataSource)
            {
                var itemSource = new ObservableCollection<SeriesModel>();

                for (int axisIndex = 0; axisIndex < yAxes.Count; axisIndex++)
                {
                    double yValue = 0;
                    var model = chartModel.Variable[0] as Model;

                    switch (axisIndex)
                    {
                        case 0:
                            yValue = Normalize(CarModelIndex(model.CarModel), 0, 4);
                            break;

                        case 1:
                            yValue = Normalize(model.Horsepower, 100, 240);
                            break;

                        case 2:
                            yValue = Normalize(model.Torque, 100, 500);
                            break;

                        case 3:
                            yValue = Normalize(model.FuelEfficiency, 5, 40);
                            break;

                        case 4:
                            yValue = Normalize(model.Price, 10000, 50000);
                            break;
                    }
                    itemSource.Add(new SeriesModel(axisIndex, yValue));
                }
                var series = new SplineSeries()
                {
                    ItemsSource = itemSource,
                    XBindingPath = nameof(SeriesModel.XValues),
                    YBindingPath = nameof(SeriesModel.YValues),
                };

                seriesList.Add(series);
            }
            return seriesList;
        }

        private double Normalize(double value, double min, double max)
        {
            double diff = max - min;
            return ((value - min) / diff) * 4;
        }

        private int CarModelIndex(string carModel)
        {
            return carModel switch
            {
                "Car A" => 0,
                "Car B" => 1,
                "Car C" => 2,
                "Car D" => 3,
                "Car E" => 4,
                _ => -1
            };
        }
    }
}
