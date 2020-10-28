//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace JamesWhittonWk7MultDB
{
    public abstract class VehicleDB
    {
        public enum Action
        {
            INQUIRE,
            UPDATE,
            DELETE,
            INSERT
        }
        public abstract Vehicle Inquire(int VehicleNo);
        public abstract int Add(Vehicle vehicle);
        public abstract int Update(Vehicle vehicle);
        public abstract int Delete(Vehicle vehicle);
 

    }
}