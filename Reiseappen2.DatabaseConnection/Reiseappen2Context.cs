﻿using Reiseappen2.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reiseappen2.DatabaseConnection
{
    public class Reiseappen2Context : DbContext
    {
        public Reiseappen2Context() : base("Reiseappen2Conn")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Trip> Trips { get; set; }

        public DbSet<DayOfTrip> Days { get; set; }
    }
}
