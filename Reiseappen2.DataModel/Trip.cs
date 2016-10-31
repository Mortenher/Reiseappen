using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Reiseappen2.DataModel
{
    public class Trip
    {
        [Key]
        public int TripId { get; set; }

        public string Name { get; set; }
        
        public ICollection<DayOfTrip> Days { get; set; }
    }
}
