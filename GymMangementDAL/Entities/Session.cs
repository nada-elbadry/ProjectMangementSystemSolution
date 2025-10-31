using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementDAL.Entities
{
    public class Session:BaseEntity
    {
        public string Description { get; set; } = null!;

        public int Capacity { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string TrainerName { get; set; } = null!;

        #region RelationShips

        #region Session - Category
        public Category Category { get; set; } = null!;

        public int CategoryId { get; set; }
        #endregion

        #region Session - Trainer

        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }= null!;
        #endregion

        #region Session -MemberSession
        public ICollection<MemberSession> SessionMembers { get; set; } = null!;
        #endregion
        #endregion

    }
}
