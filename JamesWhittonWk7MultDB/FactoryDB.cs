using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamesWhittonWk7MultDB
{
    public class FactoryDB : DbContext
    {
        public DbSet<CarVehicle> CarVehicle { get; set; }
    }
}
