using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Reiseappen2.Model
{
    public class DayOfTrip
    {
        [Key]
        public int TripDayId { get; set; }

        public int Day { get; set; }
        public string City { get; set; }
        public string Hotel { get; set; }
        public string Dinner { get; set; }
        public int MoneySpent { get; set; }
        
        public string AdditionalInfo { get; set; }

        [ForeignKey("Trip")]
        public int TripId { get; set; }

        public Trip Trip { get; set; }

        public override string ToString()
        {
            return $"Day: {Day} - {City}/{Hotel}. You had {Dinner} for dinner. You spent {MoneySpent}$. {AdditionalInfo}";
        }


       
    }
}
