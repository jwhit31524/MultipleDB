using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamesWhittonWk7MultDB
{
    public class Vehicle
    {
        public int VehicleNo { get; set; }
        public byte VehicleTypeCode { get; set; }
        public string VehicleId { get; set; }
        public string VehicleColor { get; set; }
        public DateTime? VehicleAddDateTime { get; set; }

        // Audit information (not intended to be modified directly) 
        public DateTime AddDateTime { get; set; }
        public Vehicle()
        {
            VehicleNo = 0;
            VehicleTypeCode = 1;
            VehicleId = "";
            VehicleColor = "";
            VehicleAddDateTime = DateTime.Now;
          }

        public override string ToString()
        {
            return $"VehicleNo: {VehicleNo}\nVehicleTypeCode: {VehicleTypeCode}\nVehicleId: {VehicleId}\nVehicleColor: {VehicleColor}\nVehicleAddDateTime: {VehicleAddDateTime}";
        }
    }

    public class Boat : Vehicle
    {
        public byte VehicleTypeCode { get { return 3; } }

        public int WakeWidth { get; internal set; }
    }

    public class Car : Vehicle
    {
        public byte VehicleTypeCode { get { return 1; } }

        public string CarType { get; internal set; }
    }

    public class Truck : Vehicle
    {
        public byte VehicleTypeCode { get { return 2; } }

        public int BedLength { get; internal set; }
    }
}