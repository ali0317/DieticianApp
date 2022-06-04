using AdminLTE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE.Models
{
    public class DietPlanListVM
    {
        public int Id { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public string Place { get; set; }
        public string Occupation { get; set; }
        public string Hypertension { get; set; }
        public string Diabetes { get; set; }
        public string LIverKidneyDiseases { get; set; }
        public string OtherDiseases { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public int WaterIntake { get; set; }
        public string CalorieIntake { get; set; }
        public string ReasonForDietPlan { get; set; }

        public int DayNo { get; set; }
        public decimal Fats { get; set; }

        public decimal Carbohydrates { get; set; }
        public decimal Proteins { get; set; }
        public decimal RequiredwaterIntake { get; set; }

        public decimal RequiredCalorieIntake { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ProfileId { get; set; }
        public decimal BMI { get; set; }

    }
}
