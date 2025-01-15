using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelCoordinateChart
{
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
}
