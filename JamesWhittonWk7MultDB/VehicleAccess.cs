using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.OleDb;

namespace JamesWhittonWk7MultDB
{
    public class VehicleAccess : VehicleDB
    {
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // Remember to change the database connection in the app.config!!!!!!!!!!
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        static readonly string cnFactoryDB = ConfigurationManager.ConnectionStrings["FactoryDB"].ConnectionString;

        public static readonly string sqlSelect = @"SELECT 


VehicleNo,
VehicleTypeCode,
VehicleId,
VehicleColor,
VehicleAddDateTime
FROM VHT001_VEHICLE
WHERE VehicleNo = ?;";

        public static readonly string sqlInsert = @"
INSERT INTO VHT001_VEHICLE
(
VehicleTypeCode,
VehicleId,
VehicleColor,
VehicleAddDateTime
)
VALUES
(
?,
?,
?,
?
);
";

        public static readonly string sqlUpdate = @"
UPDATE VHT001_VEHICLE
SET
VehicleTypeCode = ?,
VehicleId = ?,
StudentMajorCode = ?,
VehicleColor = ?
VehicleAddDateTime = ?
WHERE VehicleNo = ?; 
";

        public static readonly string sqlDelete = @"
DELETE FROM VHT001_VEHICLE
WHERE VehicleNo = ?; 
";

        public override Vehicle Inquire(int VehicleNo)
        {
            Vehicle vehicle = null;

            try
            {

                using (OleDbConnection cn = new OleDbConnection(cnFactoryDB))
                {
                    using (OleDbCommand cm = new OleDbCommand(sqlSelect, cn))
                    {
                        var pm = new OleDbParameter("@VehicleNo", OleDbType.Integer, 4);
                        pm.Direction = ParameterDirection.Input;
                        pm.Value = VehicleNo;
                        cm.Parameters.Add(pm);

                        cn.Open();
                        using (OleDbDataReader dr = cm.ExecuteReader())
                        {
                            if (dr.Read())  // Read() returns true if there is a record to read; false otherwise
                            {
                                vehicle = new Vehicle
                                {
                                    VehicleNo = (int)dr["VehicleNo"],
                                    VehicleTypeCode = Byte.Parse(dr["VehicleTypeCode"].ToString()), //tinyint
                                    VehicleId = dr["VehicleId"] as string, //Varchar 50
                                    VehicleColor = dr["VehicleColor"] as string, //varchar15
                                    VehicleAddDateTime = (DateTime)dr["VehicleAddDateTime"],
                                };
                            }
                        }

                    }
                }
            }
            catch (Exception ex )
            {
                throw ex;
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

            try
            {

                using (OleDbConnection cn = new OleDbConnection(cnFactoryDB))
                {
                    using (OleDbCommand cm = new OleDbCommand(sqlInsert, cn))
                    {
                        OleDbParameter pm = new OleDbParameter("@VehicleTypeCode", OleDbType.VarChar, 50);
                        pm.Direction = ParameterDirection.Input;
                        pm.Value = vehicle.VehicleTypeCode;
                        cm.Parameters.Add(pm);

                        pm = new OleDbParameter("@VehicleId", OleDbType.VarChar, 50);
                        pm.Direction = ParameterDirection.Input;
                        pm.Value = vehicle.VehicleId;
                        cm.Parameters.Add(pm);

                        pm = new OleDbParameter("@VehicleColor", OleDbType.Char, 5);
                        pm.Direction = ParameterDirection.Input;
                        pm.Value = vehicle.VehicleColor;
                        cm.Parameters.Add(pm);

                        pm = new OleDbParameter("@VehicleAddDateTime", OleDbType.Date);
                        pm.Direction = ParameterDirection.Input;
                        pm.Precision = 3;   // total digits
                        pm.Scale = 2;       // digits to the right of the decimal point 
                        pm.Value = vehicle.VehicleAddDateTime;
                        cm.Parameters.Add(pm);

                        cn.Open();
                        cm.ExecuteNonQuery();

                        //obtain the key value assigned by the database 
                        using (OleDbCommand cmIdentity = new OleDbCommand("SELECT @@IDENTITY AS VehicleNo;", cn))
                        {
                            using (OleDbDataReader drIdentity = cmIdentity.ExecuteReader())
                            {
                                if (drIdentity.Read())
                                {
                                    returnValue = (int)drIdentity["VehicleNo"];
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception)
            {
                // TODO:  Log the exception 
                returnValue = -1;
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

            try
            {

                using (OleDbConnection cn = new OleDbConnection(cnFactoryDB))
                {
                    using (OleDbCommand cm = new OleDbCommand(sqlUpdate, cn))
                    {
                        // IMPORTANT!!!
                        // Parameters should be added in the order they appear in the SQL
                        // IMPORTANT!!!

                        OleDbParameter pm = new OleDbParameter("@VehicleTypeCode", OleDbType.VarChar, 50);
                        pm.Direction = ParameterDirection.Input;
                        pm.Value = vehicle.VehicleTypeCode;
                        cm.Parameters.Add(pm);

                        pm = new OleDbParameter("@VehicleID", OleDbType.VarChar, 50);
                        pm.Direction = ParameterDirection.Input;
                        pm.Value = vehicle.VehicleId;
                        cm.Parameters.Add(pm);

                        pm = new OleDbParameter("@VehicleColor", OleDbType.Char, 5);
                        pm.Direction = ParameterDirection.Input;
                        pm.Value = vehicle.VehicleColor;
                        cm.Parameters.Add(pm);

                        pm = new OleDbParameter("@VehicleAddDateTime", OleDbType.Date);
                        pm.Direction = ParameterDirection.Input;
                        pm.Precision = 3;   // total digits
                        pm.Scale = 2;       // digits to the right of the decimal point 
                        pm.Value = vehicle.VehicleAddDateTime;
                        cm.Parameters.Add(pm);

                        pm = new OleDbParameter("@VehicleNo", OleDbType.Integer, 4);
                        pm.Direction = ParameterDirection.Input;
                        pm.Value = vehicle.VehicleNo;
                        cm.Parameters.Add(pm);

                        cn.Open();
                        cm.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception)
            {
                // TODO: log the exception 
                returnValue = -1;
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

            try
            {

                using (OleDbConnection cn = new OleDbConnection(cnFactoryDB))
                {
                    using (OleDbCommand cm = new OleDbCommand(sqlDelete, cn))
                    {
                        OleDbParameter pm = new OleDbParameter("@VehicleNo", OleDbType.Integer, 4);
                        pm.Direction = ParameterDirection.Input;
                        pm.Value = vehicle.VehicleNo;
                        cm.Parameters.Add(pm);

                        cn.Open();
                        cm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                // TODO:  Log the exception 
                returnValue = -1;
            }

            return returnValue;
        }

    }
}

