using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUtilis.DataBase
{
    class AccessDb
    { /// <summary>
      /// Path of attach .accbd
      /// </summary>
        public string DataSourcePath { get; set; }


        /// <summary>
        /// Class to agilizate access methods
        /// </summary>
        /// <param name="mdb">Source of attachment .accdb</param>
        /// <param name="prov">Provider of access</param>
        public AccessDb(string mdb,Providers prov)
        {
            try
            {
                string conn = "";
                if (prov == Providers.AceOLEDB12)
                {
                     conn = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}", mdb);
                }
                if(prov == Providers.JetOLEDB4)
                {
                    conn = string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}", mdb);
                }
                DataSourcePath = conn;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="statement">Sets te SQL Statemente or Stored preacedure</param>
        /// <returns></returns>
        public DataSet ExecuteSQL(string statement)
        {
            try
            {
                OleDbConnection connect = new OleDbConnection(DataSourcePath);
                OleDbCommand command = new OleDbCommand();
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter();
                DataSet dataset = new DataSet();

                connect.Open();

                command.Connection = connect;
                command.CommandText = statement;
                dataAdapter.SelectCommand = command;
                dataAdapter.Fill(dataset);

                connect.Close();

                return dataset;
            }
            catch (Exception ex)
            {

                throw new ArgumentException(ex.Message);
            }
        }

       

    }//CLASS
    enum Providers
    {
        AceOLEDB12 = 1,
        JetOLEDB4 = 2,
    }
}//NAMESPACE
