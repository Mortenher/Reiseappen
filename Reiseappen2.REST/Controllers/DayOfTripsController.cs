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
    public class DayOfTripsController : ApiController
    {
        private Reiseappen2Context db = new Reiseappen2Context();

        // GET: api/DayOfTrips
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

        // PUT: api/DayOfTrips/5
        [ResponseType(typeof(void))]
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
        [ResponseType(typeof(DayOfTrip))]
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