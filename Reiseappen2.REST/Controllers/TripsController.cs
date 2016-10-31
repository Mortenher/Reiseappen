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
using Reiseappen2.DataModel;
using Reiseappen2.DatabaseConnection;

namespace Reiseappen2.REST.Controllers
{
    public class TripsController : ApiController
    {
        private Reiseappen2Context db = new Reiseappen2Context();

        // GET: api/Trips
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

        // PUT: api/Trips/5
        [ResponseType(typeof(void))]
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
        
        [ResponseType(typeof(Trip))]
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