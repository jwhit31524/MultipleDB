using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamesWhittonWk7MultDB
{
    public class DatabaseParameterFactory
    {
        public static DbParameter Build(string parameterName,
        DbType parameterDbType,
        int parameterSize,
        ParameterDirection direction,
        object value)
        {
            DbParameter pm = null;
            string dbConfig = ConfigurationManager.AppSettings["DbConfig"].ToUpper();

            switch (dbConfig)
            {
                case "SQLSERVER":
                    SqlDbType sqlDbType = SqlDbType.Int;
                    if (parameterDbType == DbType.Int32)
                        sqlDbType = SqlDbType.Int;
                    pm = new SqlParameter($"@{parameterName}", sqlDbType, parameterSize);
                    pm.Direction = direction;   // TODO:  is direction correct?
                    pm.Value = value;
                    break;
                case "MSACCESS":
                    OleDbType oleDbType = OleDbType.BigInt;
                    if (parameterDbType == DbType.Int32)
                        oleDbType = OleDbType.Integer;
                    pm = new OleDbParameter(parameterName, oleDbType, parameterSize);
                    pm.Direction = direction;
                    pm.Value = value;
                    break;
                    //case "ORACLE":
                    //  cn = new OracleConnection(connString);
                    //break;
            }

            return pm;
        }
    }
}
