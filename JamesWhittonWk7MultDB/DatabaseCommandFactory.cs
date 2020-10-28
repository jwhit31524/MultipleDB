using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamesWhittonWk7MultDB
{
    public class DatabaseCommandFactory
    {
        public static DbCommand Build(VehicleDB.Action action, DbConnection cn)
        {
            DbCommand cm = null;
            string sql = null;

            string dbConfig = ConfigurationManager.AppSettings["DbConfig"].ToUpper();

            switch (dbConfig)
            {
                case "SQLSERVER":
                    switch (action)
                    {
                        case VehicleDB.Action.INQUIRE:
                            sql = VehicleSQL.sqlSelect;
                            break;
                    }
                    cm = new SqlCommand(sql, (SqlConnection)cn);
                    break;
                case "MSACCESS":
                    switch (action)
                    {
                        case VehicleDB.Action.INQUIRE:
                            sql = VehicleAccess.sqlSelect;
                            break;
                    }
                    cm = new OleDbCommand(sql, (OleDbConnection)cn);
                    break;
            }
            return cm;
        }

    }
}
