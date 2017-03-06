using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reiseappen2.Model
{
    public class Trip
    {
        [Key]
        public int TripId { get; set; }

        public string Name { get; set; }

        //Suppresser da den ikke er i bruk, redd for å ødelegge modellen rett før innelevering.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<DayOfTrip> Days { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
