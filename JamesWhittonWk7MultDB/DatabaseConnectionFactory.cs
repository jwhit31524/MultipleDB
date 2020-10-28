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
    public class DatabaseConnectionFactory
    {
        public static DbConnection Build()
        {
            DbConnection cn = null;
            string dbConfig = ConfigurationManager.AppSettings["DbConfig"].ToUpper();
            string connString = ConfigurationManager.ConnectionStrings["FactoryDB"].ConnectionString;

            switch (dbConfig)
            {
                case "SQLSERVER":
                    cn = new SqlConnection(connString);
                    break;
                case "MSACCESS":
                    cn = new OleDbConnection(connString);
                    break;
                    //case "ORACLE":
                    //  cn = new OracleConnection(connString);
                    //break;
            }

            return cn;
        }

    }
}
