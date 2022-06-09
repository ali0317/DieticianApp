using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE.Models
{
    public class BMI
    {

        [Key]
        public int Id { get; set; }

        public int Age { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public string Gender { get; set; }

        public DateTime CreatedDate { get; set; }
        public string UserId { get; set; }
        public decimal BMIValue { get; set; }





    }
}
