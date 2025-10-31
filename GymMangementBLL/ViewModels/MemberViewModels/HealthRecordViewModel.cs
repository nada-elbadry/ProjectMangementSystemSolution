using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementBLL.ViewModels.MemberViewModels
{
    internal class HealthRecordViewModel
    {
        [Required (ErrorMessage ="Heigh Is Required")]
        [Range(0.1 , 300 , ErrorMessage ="Height Must Be Greater Than 0 and less Than 300 cm")]
        
        public decimal Height { get; set; }

        [Required(ErrorMessage = "Weight Is Required")]
        [Range(0.1, 500, ErrorMessage = "Weight Must Be Greater Than 0 and less Than 500 kg")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "Blood Type Is Required")]
        [StringLength(3,ErrorMessage ="Blood Type Must Be 3 char or Less")]
        public string BloodType { get; set; } = null!;

        public string? Note { get; set; }
    }
}
