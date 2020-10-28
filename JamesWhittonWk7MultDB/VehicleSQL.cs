using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace JamesWhittonWk7MultDB
{
    class VehicleSQL : VehicleDB
    {  // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // Remember to change the database connection in the app.config!!!!!!!!!!
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        static readonly string cnFactoryDB = ConfigurationManager.ConnectionStrings["old_FactoryDb"].ConnectionString;

        public static readonly string sqlSelect = @"SELECT 
VehicleNo,
VehicleTypeCode,
VehicleId,
VehicleColor,
VehicleAddDateTime
FROM VHT001_VEHICLE
WHERE VehicleNo = @VehicleNo;";

        private static readonly string sqlInsert = @"
DECLARE @RC INT = 0;                -- this value holds the return code, which will be 0 (success) or -1 (failure) 

BEGIN TRY


BEGIN TRANSACTION

SET @VehicleNo = (SELECT ISNULL(MAX(VehicleNo), 0) FROM VHT001_VEHICLE); 
SET @VehicleNo = @VehicleNo + 1; 

INSERT INTO VHT001_VEHICLE
(
VehicleNo,
VehicleTypeCode,
VehicleId,
VehicleColor,
VehicleAddDateTime
)
VALUES
(
@VehicleNo,
@VehicleTypeCode,
@VehicleId,
@VehicleColor,
@VehicleAddDateTime
);

COMMIT TRANSACTION 



END TRY

BEGIN CATCH 

IF @@TRANCOUNT > 0
   ROLLBACK TRANSACTION;

 -- there has been an error so set the return code to -1
SET @RC = -1;

-- Uncomment this to cause an error to be thrown in the calling (C#) code 
THROW;  

END CATCH
-- SELECT @RC;
";

        private static readonly string sqlUpdate = @"
DECLARE @RC INT = 0;                -- this value holds the return code, which will be 0 (success) or -1 (failure) 

BEGIN TRY

--SET @StudentMajorCode = UPPER(@StudentMajorCode); -- force this field to be uppercase 

UPDATE VHT001_VEHICLE
SET
VehicleTypeCode = @VehicleTypeCode,
VehicleId = @VehicleId,
VehicleColor = @VehicleColor,
VehicleAddDateTime = @VehicleAddDateTime
WHERE VehicleNo = @VehicleNo; 

END TRY

BEGIN CATCH 

 -- there has been an error so set the return code to -1
SET @RC = -1;

-- Uncomment this to cause an error to be thrown in the calling (C#) code 
 THROW;  

END CATCH
";

        private static readonly string sqlDelete = @"
DECLARE @RC INT = 0;                -- this value holds the return code, which will be 0 (success) or -1 (failure) 

BEGIN TRY

DELETE FROM VHT001_VEHICLE
WHERE VehicleNo = @VehicleNo; 

END TRY

BEGIN CATCH 

 -- there has been an error so set the return code to -1
SET @RC = -1;

-- Uncomment this to cause an error to be thrown in the calling (C#) code 
 THROW;  

END CATCH
";

        public override Vehicle Inquire(int VehicleNo)
        {
            Vehicle vehicle = null;

            using (SqlConnection cn = new SqlConnection(cnFactoryDB))
            {
                using (SqlCommand cm = new SqlCommand(sqlSelect, cn))
                {
                    SqlParameter pm = new SqlParameter("@VehicleNo", SqlDbType.Int, 4);
                    pm.Direction = ParameterDirection.Input;
                    pm.Value = VehicleNo;
                    cm.Parameters.Add(pm);

                    cn.Open();
                    using (DbDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.Read())  // Read() returns true if there is a record to read; false otherwise
                        {
                            var VehicleTypeCode = (byte)dr["VehicleTypeCode"];
                            if (VehicleTypeCode == 3)
                            {
                                vehicle = new Boat
                                {
                                    VehicleNo = (int)dr["VehicleNo"],
                                    VehicleId = dr["VehicleId"] as string, //Varchar 50
                                    VehicleColor = dr["VehicleColor"] as string, //varchar15
                                    VehicleAddDateTime = (DateTime)dr["VehicleAddDateTime"]

                                };
                            }
                            else if (VehicleTypeCode == 2)
                            {
                                vehicle = new Truck
                                {
                                    VehicleNo = (int)dr["VehicleNo"],
                                    VehicleId = dr["VehicleId"] as string, //Varchar 50
                                    VehicleColor = dr["VehicleColor"] as string, //varchar15
                                    VehicleAddDateTime = (DateTime)dr["VehicleAddDateTime"]

                                };
                            }

                            else if (VehicleTypeCode == 1)
                            {
                                vehicle = new Car
                                {
                                    VehicleNo = (int)dr["VehicleNo"],
                                    VehicleId = dr["VehicleId"] as string, //Varchar 50
                                    VehicleColor = dr["VehicleColor"] as string, //varchar15
                                    VehicleAddDateTime = (DateTime)dr["VehicleAddDateTime"]


                                };
                            }
                            /*
                            vehicle = new Vehicle
                            {
                                VehicleNo = (int)dr["VehicleNo"],
                                VehicleTypeCode = (byte)dr["VehicleTypeCode"], //tinyint
                                VehicleId = dr["VehicleId"] as string, //Varchar 50
                                VehicleColor = dr["VehicleColor"] as string, //varchar15
                                VehicleAddDateTime = (DateTime)dr["VehicleAddDateTime"],
                            };*/
                        }
                    }

                }
            }

            return vehicle;
        }

        /// <summary>
        /// Inserts a student into the database
        /// </summary>
        /// <param name="vehicle">A student object representing the student to be added to the database</param>
        /// <returns>The student id (greater than zero) upon success; otherwise -1.</returns>
        public override int Add(Vehicle vehicle)
        {
            int returnValue = 0;

            if (vehicle == null) throw new ArgumentNullException("Vehicle cannot be null");

            using (SqlConnection cn = new SqlConnection(cnFactoryDB))
            {
                using (SqlCommand cm = new SqlCommand(sqlInsert, cn))
                {
                    SqlParameter pm = new SqlParameter("@VehicleNo", SqlDbType.Int, 1);
                    pm.Direction = ParameterDirection.Output;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@VehicleTypeCode", SqlDbType.TinyInt, 3);
                    pm.Direction = ParameterDirection.Input;
                    pm.Value = vehicle.VehicleTypeCode;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@VehicleId", SqlDbType.VarChar, 50);
                    pm.Direction = ParameterDirection.Input;
                    pm.Value = vehicle.VehicleId;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@VehicleColor", SqlDbType.VarChar, 15);
                    pm.Direction = ParameterDirection.Input;
                    pm.Value = vehicle.VehicleColor;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@VehicleAddDateTime", SqlDbType.DateTime);
                    pm.Direction = ParameterDirection.Input;
                    pm.Precision = 3;   // total digits
                    pm.Scale = 2;       // digits to the right of the decimal point 
                    pm.Value = vehicle.VehicleAddDateTime;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@RC", SqlDbType.Int, 4);
                    pm.Direction = ParameterDirection.ReturnValue;
                    pm.Value = 0;
                    cm.Parameters.Add(pm);

                    cn.Open();
                    cm.ExecuteNonQuery();

                    returnValue = (int)cm.Parameters["@RC"].Value;
                    if (returnValue == 0)
                    {
                        returnValue = (int)cm.Parameters["@VehicleNo"].Value;
                    }
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Updates a student record in the database
        /// </summary>
        /// <param name="vehicle">A student object representing the student to be updated to the database</param>
        /// <returns>The student id (greater than zero) upon success; otherwise -1.</returns>
        public override int Update(Vehicle vehicle)
        {
            int returnValue = 0;

            if (vehicle == null) throw new ArgumentNullException("Vehicle cannot be null");

            using (SqlConnection cn = new SqlConnection(cnFactoryDB))
            {
                using (SqlCommand cm = new SqlCommand(sqlUpdate, cn))
                {
                    SqlParameter pm = new SqlParameter("@VehicleNo", SqlDbType.Int, 1);
                    pm.Direction = ParameterDirection.Output;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@VehicleTypeCode", SqlDbType.TinyInt, 3);
                    pm.Direction = ParameterDirection.Input;
                    pm.Value = vehicle.VehicleTypeCode;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@VehicleId", SqlDbType.VarChar, 50);
                    pm.Direction = ParameterDirection.Input;
                    pm.Value = vehicle.VehicleId;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@VehicleColor", SqlDbType.VarChar, 15);
                    pm.Direction = ParameterDirection.Input;
                    pm.Value = vehicle.VehicleColor;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@VehicleAddDateTime", SqlDbType.DateTime);
                    pm.Direction = ParameterDirection.Input;
                    pm.Precision = 3;   // total digits
                    pm.Scale = 2;       // digits to the right of the decimal point 
                    pm.Value = vehicle.VehicleAddDateTime;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@RC", SqlDbType.Int, 4);
                    pm.Direction = ParameterDirection.ReturnValue;
                    cm.Parameters.Add(pm);

                    cn.Open();
                    cm.ExecuteNonQuery();

                    returnValue = (int)cm.Parameters["@RC"].Value;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Updates a student record in the database
        /// </summary>
        /// <param name="vehicle">A student object representing the student to be updated to the database</param>
        /// <returns>The student id (greater than zero) upon success; otherwise -1.</returns>
        public override int Delete(Vehicle vehicle)
        {
            int returnValue = 0;

            if (vehicle == null) throw new ArgumentNullException("Vehicle cannot be null");

            using (SqlConnection cn = new SqlConnection(cnFactoryDB))
            {
                using (SqlCommand cm = new SqlCommand(sqlDelete, cn))
                {
                    SqlParameter pm = new SqlParameter("@VehicleNo", SqlDbType.Int, 4);
                    pm.Direction = ParameterDirection.Input;
                    pm.Value = vehicle.VehicleNo;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@RC", SqlDbType.Int, 4);
                    pm.Direction = ParameterDirection.ReturnValue;
                    cm.Parameters.Add(pm);

                    cn.Open();
                    cm.ExecuteNonQuery();

                    returnValue = (int)cm.Parameters["@RC"].Value;
                }
            }

            return returnValue;
        }

    }
}
