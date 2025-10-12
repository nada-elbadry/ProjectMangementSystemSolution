using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementDAL.Entities
{
    public class Membership: BaseEntity
    {
        //Start Date ==CreatedAt of BaseEntity
        public DateTime EndDate { get; set; }
        //Read Only Property
        public string Status
        {
            get
            {
                return EndDate >= DateTime.Now ? "Active" : "Expired";
            }
        }
        public Member Member { get; set; } = null!;
        public int MemberId { get; set; }

        public Plan Plan { get; set; } = null!;

        public int PlanId { get; set; }

    }
}
