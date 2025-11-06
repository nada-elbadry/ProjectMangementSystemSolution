using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementBLL.ViewModels.TrainerViewModel
{
    public class TrainerViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Specialties { get; set; } = null!; // Yoga, CrossFit, etc.

        public string? Gender { get; set; }

        public string? DateOfBirth { get; set; }

        public string? HireDate { get; set; }

        public string? Address { get; set; }

  
    }
}
