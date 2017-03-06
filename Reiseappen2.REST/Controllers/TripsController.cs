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
    public class TripsController : ApiController
    {
        private Reiseappen2Context db = new Reiseappen2Context();

        // GET: api/Trips
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public IQueryable<Trip> GetTrips()
        {
            return db.Trips;
        }

        // GET: api/Trips/5
        [ResponseType(typeof(Trip))]
        public async Task<IHttpActionResult> GetTrip(int id)
        {
            Trip trip = await db.Trips.FindAsync(id);
            if (trip == null)
            {
                return NotFound();
            }

            return Ok(trip);
        }
        /// <summary>
        /// Gets the name of the trip.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [Route("api/trips/name")]
        [ResponseType(typeof(Trip))]
        public List<string> GetTripName()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Reiseappen2Conn"].ConnectionString))
            {
                List<string> result = new List<string>();
                string Query = "SELECT Name FROM Trips";
                SqlCommand command = new SqlCommand(Query, conn);
                try
                {
                    conn.Open();
                    command.CommandType = CommandType.Text;
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                      
                           result.Add(reader.GetString(0));
                        }
                       
                        reader.Close();
                    }
                    
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.Write(e);
                }
                return result;
            }
        }

        /// <summary>
        /// Gets the name of the trip identifier from name selected in UI.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [HttpGet]
        [Route("api/trips/{name}")]
        [ResponseType(typeof(Trip))]
        public Trip GetTripIdFromName(string name)
        {
            Trip trip = new Trip();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Reiseappen2Conn"].ConnectionString))
            {
                
                string Query = "SELECT TripId FROM Trips Where Name LIKE @Name";
                SqlCommand Command = new SqlCommand(Query, conn);
                try
                {
                    conn.Open();
                    Command.Parameters.AddWithValue("Name", name);
                    SqlDataReader reader = Command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            trip.TripId = reader.GetInt32(0);
                            return trip; 
                        }

                        reader.Close();
                    }

                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.Write(e);
                }
                return trip;
            }
        }

        // PUT: api/Trips/5
        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("api/trips/{id}")]
        public async Task<IHttpActionResult> PutTrip(int id, Trip trip)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != trip.TripId)
            {
                return BadRequest();
            }

            db.Entry(trip).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripExists(id))
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

        // POST: api/Trips
        [ResponseType(typeof(Trip))]
        public async Task<IHttpActionResult> PostTrip(Trip trip)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Trips.Add(trip);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = trip.TripId }, trip);
        }

        // DELETE: api/Trips/5

        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("api/trips/{id}")]
        public async Task<IHttpActionResult> DeleteTrip(int id)
        {
            Trip trip = await db.Trips.FindAsync(id);
            if (trip == null)
            {
                return NotFound();
            }

            db.Trips.Remove(trip);
            await db.SaveChangesAsync();

            return Ok(trip);
        }  

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TripExists(int id)
        {
            return db.Trips.Count(e => e.TripId == id) > 0;
        }
    }
}