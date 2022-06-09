using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE.Models
{
    public class DashboardVm
    {
        public DietPlan DietPlan { get; set; }
        public List<BMI> BMIList { get; set; }
    }
}
