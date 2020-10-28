using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;


namespace JamesWhittonWk7MultDB
{
   public class VehicleDAL
    {
        public static Vehicle Inquire(int VehicleNo)
        {
            Vehicle vehicle = null;

            using (DbConnection cn = DatabaseConnectionFactory.Build())
            {
                using (DbCommand cm = DatabaseCommandFactory.Build(VehicleDB.Action.INQUIRE, cn))
                {
                    DbParameter pm = DatabaseParameterFactory.Build("VehicleNo",
                        System.Data.DbType.Int32,
                        4,
                        System.Data.ParameterDirection.Input,
                        VehicleNo);
                    cm.Parameters.Add(pm);

                    cn.Open();
                    using (DbDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.Read())  // Read() returns true if there is a record to read; false otherwise
                        {
                            vehicle = new Vehicle
                            {
                                VehicleNo = (int)dr["VehicleNo"],
                                VehicleTypeCode = (byte)dr["VehicleTypeCode"], //tinyint
                                VehicleId = dr["VehicleId"] as string, //Varchar 50
                                VehicleColor = dr["VehicleColor"] as string, //varchar15
                                VehicleAddDateTime = (DateTime)dr["VehicleAddDateTime"],
                            };
                        }
                    }
                }
            }

            return vehicle;
        }

    }
}
