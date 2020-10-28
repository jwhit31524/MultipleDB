using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace JamesWhittonWk7MultDB
{
    class Program
    {  // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // Remember to change the database connection in the app.config!!!!!!!!!!
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        static void Main(string[] args)
        {
            int VehicleNo = 0;
            int returnCode = 0;
           VehicleDB vehicleDb = null;

            string dbConfig = ConfigurationManager.AppSettings["DbConfig"].ToUpper();
            switch (dbConfig)
            {
                case "SQLSERVER":
                    vehicleDb = new VehicleSQL();
                    break;
                case "MSACCESS":
                    vehicleDb = new VehicleAccess();
                    break;
            }

            Vehicle vehicle = new Vehicle
            {
                VehicleTypeCode = 2,
                VehicleId = "111",
                VehicleColor = "Green",
                VehicleAddDateTime = DateTime.Now
            };

            // Note that this will add a new record every time the program is executed 
            VehicleNo = vehicleDb.Add(vehicle);

            if (VehicleNo > 0)
            {
                vehicle = vehicleDb.Inquire(VehicleNo);
                Console.WriteLine(vehicle.ToString());

                vehicle.VehicleTypeCode = 2;
                vehicle.VehicleColor = "Black";

                returnCode = vehicleDb.Update(vehicle);
                if (returnCode == 0)
                {
                    Console.WriteLine("\nAfter Updating");
                    Console.WriteLine(vehicle.ToString());

                    VehicleNo = vehicle.VehicleNo; // save the Id 
                    Console.WriteLine($"\nDelete VehicleNo: {VehicleNo} from the database");
                    returnCode = vehicleDb.Delete(vehicle);

                    
                   Console.ReadLine();

                    if (returnCode == 0)
                    {
                        vehicle = vehicleDb.Inquire(vehicle.VehicleNo);
                        if (vehicle == null)
                            Console.WriteLine($"VehicleNo: {VehicleNo} Not found in the database");
                        else
                            Console.WriteLine($"Hmmm... delete returned success yet the student record {VehicleNo} still exists...");
                    }
                    else
                        Console.WriteLine($"There was an error deleting the vehicle from the database");
                }
                else
                    Console.WriteLine($"There was an error updating the vehicle in the database");
            }
            else
                Console.WriteLine($"There was an error adding the vehicle to the database");

            Console.ReadLine();
        }
    }
}