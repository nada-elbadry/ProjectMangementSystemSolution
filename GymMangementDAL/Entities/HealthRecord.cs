using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementDAL.Entities
{
    //1-1 Relationship with Member [shared PK]
    public class HealthRecord :BaseEntity
    {
        public decimal Height { get; set; }

        public decimal Weight { get; set; }

        public string BloodType { get; set; } = null!;

        public string? Note { get; set; }


    }
}
