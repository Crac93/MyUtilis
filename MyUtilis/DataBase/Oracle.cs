using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUtilis.DataBase
{
    /// <summary>
    /// ORcle classs
    /// </summary>
    public class Oracle
    {
        /// <summary>
        /// Excuete dataset
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="Query"></param>
        /// <returns></returns>
        public static DataSet Execute(string ConnectionString, string Query)
        {
            try
            {
                OracleConnection connect = new OracleConnection();
                OracleCommand command = new OracleCommand();
                OracleDataAdapter adapter = new OracleDataAdapter();
                DataSet dataset;

                try
                {
                    connect.ConnectionString = ConnectionString;
                    connect.Open();
                    command.Connection = connect;
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Oracle Connection problem." + ex.Message.ToString().Trim());
                }

                dataset = new DataSet();
                command.CommandText = Query;
                adapter.SelectCommand = command;
                adapter.Fill(dataset);

                try
                {
                    connect.Close();
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Oracle Disconnection problem." + ex.Message.ToString().Trim());
                }

                return dataset;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Oracle statement can not be executed:" + "\n" + ex.Message);
            }
        }

        /// <summary>
        /// Exucte Procedure
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="ProcedureName"></param>
        /// <param name="Dt_Parameters"></param>
        /// <returns></returns>
        public static DataSet ExecuteProcedure(string ConnectionString, string ProcedureName, DataTable Dt_Parameters)
        {
            try
            {

                OracleConnection connect = new OracleConnection();
                OracleCommand command = new OracleCommand();
                OracleDataAdapter adapter = new OracleDataAdapter();
                DataSet dataset;

                try
                {
                    connect.ConnectionString = ConnectionString;
                    connect.Open();
                    command.Connection = connect;
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Oracle Connection problem." + ex.Message.ToString().Trim());
                }


                using (OracleCommand cmd = new OracleCommand(ProcedureName, connect))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (DataRow Row in Dt_Parameters.Rows)
                    {
                        string Parameter_Name = Row["ParameterName"].ToString();
                        var Parameter_Value = Row["ParameterValue"];
                        OracleDbType Parameter_DbType = (OracleDbType)Row["ParameterDataType"];
                        ParameterDirection Parameter_Direction = (ParameterDirection)Row["ParameterDirections"];

                        if ((ParameterDirection)Row[3] == ParameterDirection.Input)
                        {
                            cmd.Parameters.Add(Parameter_Name, Parameter_DbType).Value = Parameter_Value;
                        }
                        else
                        {
                            cmd.Parameters.Add(Parameter_Name, Parameter_DbType).Direction = Parameter_Direction;
                        }
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
