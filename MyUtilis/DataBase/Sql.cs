using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUtilis.DataBase
{
    /// <summary>
    /// Sql class 
    /// </summary>
    public class SQL
    {

        /// <summary>
        /// Execute a commnad in SQLServer
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="Query">Query to execute</param>
        /// <returns></returns>
        public static DataSet Execute(string ConnectionString, string Query)
        {
            try
            {
                SqlConnection connect = new SqlConnection();
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataSet dataset;

                connect.ConnectionString = ConnectionString;
                connect.Open();
                command.Connection = connect;
                dataset = new DataSet();
                command.CommandText = Query;
                adapter.SelectCommand = command;
                adapter.Fill(dataset);

                connect.Close();

                return dataset;
            }
            catch (Exception ex)
            {
              //  Console.WriteLine(Utilis.Debug() + "SQL statement can not be executed: " + ex.Message.ToString().Trim());
                throw new ArgumentException("SQL statement can not be executed:" + "\n" + ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="ProcedureName"></param>
        /// <param name="Dt_Parameters"></param>
        /// <returns></returns>
        public static DataSet ExecuteProcedure(string ConnectionString, string ProcedureName, Dictionary<string, object> Dt_Parameters)
        {
            try
            {
                SqlConnection connect = new SqlConnection();
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();

                DataSet dataset;

                try
                {
                    connect.ConnectionString = ConnectionString;
                    connect.Open();
                    command.Connection = connect;
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("sql Server Connection problem." + ex.Message.ToString().Trim());
                }


                using (SqlCommand cmd = new SqlCommand(ProcedureName, connect))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (var param in Dt_Parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }

                    dataset = new DataSet();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dataset);

                    try
                    {
                        connect.Close();
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException(ex.Message);
                    }

                    return dataset;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("StoreProcedure: " + ex.Message);
            }
        }

    }
}
