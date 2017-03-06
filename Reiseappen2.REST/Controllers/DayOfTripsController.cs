using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Reiseappen2.DatabaseConnection;
using Reiseappen2.Model;
using System.Data.SqlClient;
using System.Configuration;

namespace Reiseappen2.REST.Controllers
{
    public class DayOfTripsController : ApiController
    {
        private Reiseappen2Context db = new Reiseappen2Context();

        // GET: api/DayOfTrips
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public IQueryable<DayOfTrip> GetDays()
        {
            return db.Days;
        }

        // GET: api/DayOfTrips/5
        [ResponseType(typeof(DayOfTrip))]
        public async Task<IHttpActionResult> GetDayOfTrip(int id)
        {
            DayOfTrip dayOfTrip = await db.Days.FindAsync(id);
            if (dayOfTrip == null)
            {
                return NotFound();
            }

            return Ok(dayOfTrip);
        }

        /// <summary>
        /// Gets the day of a trip specified by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>DayOfTrip list to the UI</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [HttpGet]
        [Route("api/dayoftrips/{name}")]
        [ResponseType(typeof(DayOfTrip))]
        public List<DayOfTrip> GetTripDaysFromName(string name)
        {
            string query = "SELECT TripDayId, Day, City, Hotel, Dinner, MoneySpent, AdditionalInfo, TripId FROM DayOfTrips WHERE TripId=(SELECT TripId FROM Trips WHERE Name = @name)";
            string tripName = name;
         

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Reiseappen2Conn"].ConnectionString))
            {
                return GetInfo(query, conn, tripName);
            }
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        //Suppresser denne da variabelen kommer fra en liste, ikke user input.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public List<DayOfTrip> GetInfo(string query, SqlConnection conn, string tripName)
        {
            List<DayOfTrip> result = new List<DayOfTrip>();
            using (SqlCommand Command = new SqlCommand(query, conn))
            {

                Command.Parameters.AddWithValue("name", tripName);
                try
                {
                    conn.Open();
                    SqlDataReader rdr = Command.ExecuteReader();
                    while (rdr.Read())
                    {
                        var Day = new DayOfTrip
                        {
                            TripDayId = rdr.GetInt32(0),
                            Day = rdr.GetInt32(1),
                            City = rdr.GetString(2),
                            Hotel = rdr.GetString(3),
                            Dinner = rdr.GetString(4),
                            MoneySpent = rdr.GetInt32(5),
                            AdditionalInfo = rdr.GetString(6),
                            TripId = rdr.GetInt32(7)

                        };
                        result.Add(Day);

                    }

                    return result;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
                return result;
            }
        }

        /// <summary>
        /// Gets the cities from trip for use in map and caluclating distance traveled.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [HttpGet]
        [Route("api/dayoftrips/city/{name}")]
        [ResponseType(typeof(DayOfTrip))]
        public List<string> GetCitiesFromTrip(string name)
        {
            string query = "SELECT City FROM DayOfTrips WHERE TripId=(SELECT TripId FROM Trips WHERE Name = @name)";
            //List<string> result = new List<string>();
            string tripName = name;


            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Reiseappen2Conn"].ConnectionString))
            {
                return GetCities(query, conn, tripName);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        //Suppresser denne da variabelen kommer fra en liste, ikke user input.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public List<string> GetCities(string query, SqlConnection conn, string tripName)
        {
            List<string> result = new List<string>();
            using (SqlCommand Command = new SqlCommand(query, conn))
            {

                Command.Parameters.AddWithValue("name", tripName);
                try
                {
                    conn.Open();
                    SqlDataReader rdr = Command.ExecuteReader();
                    while (rdr.Read())
                    {
                        result.Add(rdr.GetString(0));
                    }

                    return result;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
                return result;
            }
        }

        // PUT: api/DayOfTrips/5
        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("api/dayoftrips/{id}")]
        public async Task<IHttpActionResult> PutDayOfTrip(int id, DayOfTrip dayOfTrip)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dayOfTrip.TripDayId)
            {
                return BadRequest();
            }

            db.Entry(dayOfTrip).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DayOfTripExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/DayOfTrips
        [ResponseType(typeof(DayOfTrip))]
        public async Task<IHttpActionResult> PostDayOfTrip(DayOfTrip dayOfTrip)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Days.Add(dayOfTrip);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = dayOfTrip.TripDayId }, dayOfTrip);
        }

        // DELETE: api/DayOfTrips/5
        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("api/dayoftrips/{id}")]
        public async Task<IHttpActionResult> DeleteDayOfTrip(int id)
        {
            DayOfTrip dayOfTrip = await db.Days.FindAsync(id);
            if (dayOfTrip == null)
            {
                return NotFound();
            }

            db.Days.Remove(dayOfTrip);
            await db.SaveChangesAsync();

            return Ok(dayOfTrip);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DayOfTripExists(int id)
        {
            return db.Days.Count(e => e.TripDayId == id) > 0;
        }
    }
}