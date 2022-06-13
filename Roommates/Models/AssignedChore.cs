using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roommates.Models
{
    public class AssignedChore
    {
        public int Id { get; set; }
        public string ChoreName { get; set; }
        public int ChoreId { get; set; }
        public string RoommateName { get; set; }
        public int RoommateId{ get; set; }
    }
}
