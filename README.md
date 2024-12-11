# How to create Parallel coordinate chart in .NET MAUI
This article offers a comprehensive guide to creating a Parallel Coordinate Chart in .NET MAUI.

You can achieve the parallel coordinates chart using the [.NET MAUI Cartesian Chart](https://www.syncfusion.com/maui-controls/maui-cartesian-charts) by incorporating multiple spline or line series and utilizing axis crossing support to arrange the chart axes in parallel.

A **Parallel Coordinate Chart** for visualizing multidimensional data, where each axis represents a variable, and data points are displayed as lines connecting these parallel axes. This guide walks you through the steps to implement such a visualization effectively in .NET MAUI.

The following steps and code examples illustrate how to create the parallel coordinates chart using [SfCartesianChart](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.SfCartesianChart.html).

**Step 1:** Create a custom parallel coordinates chart by inheriting from the SfCartesianChart class and initializing the chart setup through the GenerateChart method.
 
**C#**
 ```csharp
public class ParallelCoordinateChart:SfCartesianChart
{
    public ParallelCoordinateChart()
    {
        GenerateChart();
    }
} 
 ```
 

**Step 2:**  This method manages the complete chart setup, including the configuration of axes, series, and visual styling elements such as color palettes.

**C#**
 
 ```csharp
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
 ```
 

**Step 3:** Generate the X-axis for the chart and create a list of Y-axes, each representing a different variable from the view model data source.

 **C#**
 ```csharp
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
 ```
 

**Step 4:** Generates and returns a list of series to visualize data points across the parallel axes, normalizes raw data values to a standardized scale, and maps car model names to their corresponding indices on the X-axis.

**C#**
 
 ```csharp
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
 ```
 
**Step 5:** This ViewModel class contains DataSource and Source. It generates a collection of Model objects with car details using GenerateData method, then wraps each Model in a ChartModel and adds it to DataSource.

**C#**
 
 ```csharp
public class ViewModel
{
    public ObservableCollection<ChartModel> DataSource { get; set; }

    public ObservableCollection<Model> Source { get; set; }

    public ViewModel()
    {
        Source = GenerateData();
        DataSource = new ObservableCollection<ChartModel>();
        foreach (var data in Source)
        {
            DataSource.Add(new ChartModel(new List<object> { data }));
        }
    }

    private ObservableCollection<Model> GenerateData()
    {
        var data = new ObservableCollection<Model>()
        {
            new Model { CarModel = "Car A", Horsepower = 220, Torque = 400, FuelEfficiency = 10, Price = 50000 },
            new Model { CarModel = "Car B", Horsepower = 150, Torque = 350, FuelEfficiency = 15, Price = 15000 },
            new Model { CarModel = "Car C", Horsepower = 200, Torque = 150, FuelEfficiency = 20, Price = 30000 },
            new Model { CarModel = "Car D", Horsepower = 180, Torque = 300, FuelEfficiency = 30, Price = 25000 },
            new Model { CarModel = "Car E", Horsepower = 120, Torque = 200, FuelEfficiency = 35, Price = 40000 }
        };

        return data;
    }
} 
 ```
 

**Step 6:** The Content page contains the ParallelCoordinateChart control, which renders the parallel coordinate chart on the page.

**XAML**
 
 ```xml
<ContentPage.Content>
    <local:ParallelCoordinateChart/>
</ContentPage.Content> 
 ```
 
**Output:**

![ParallelCoordinateChart1](https://github.com/user-attachments/assets/1625c2c0-7d14-420d-b591-5fb29426b90d)

