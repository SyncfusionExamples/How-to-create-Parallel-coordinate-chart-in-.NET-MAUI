using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelCoordinateChart
{
    public class ChartModel
    {
        private List<object> variables;

        public List<object> Variable
        {
            get => variables;
            set => variables = value;
        }

        public ChartModel(List<object> values)
        {
            variables = values;
        }
    }
}
