using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE.Models
{
    public class DietPlan
    {
        [Key]
        public int Id { get; set; }

        public int DayNo { get; set; }
        public decimal Fats { get; set; }

        public decimal Carbohydrates{ get; set; }
        public decimal Proteins{ get; set; }
        public decimal WaterIntake{ get; set; }
        
        public decimal RequiredCalorieIntake{ get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserId { get; set; }


        public int ProfileId { get; set; }
      




    }
}
