using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementBLL.ViewModels.PlanViewModels
{
    internal class UpdatePlanViewModel
    {
        [Required(ErrorMessage = "Plan Name Id is required")]
        [StringLength(50, ErrorMessage = "Plan Name must be less than 51 char ")]
        public string PlanName { get; set; } = null!;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(200 ,MinimumLength =50,ErrorMessage = "Description must be between 5 and 200 char ")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Duration is required")]
        [Range(1, 365, ErrorMessage = "Duration Days must be between 1 and 365 days")]
        public int DurationDays { get; set; }
        [Range(0.1, 10000, ErrorMessage = "Price must be between 0.1 and 10000")]
        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }
    }
}
