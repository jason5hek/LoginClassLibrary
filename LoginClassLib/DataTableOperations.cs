using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace LoginClassLib
{   
    /// <summary>
    /// Performs various Data operations to interogate databases for functions of the program
    /// </summary>
    class DataTableOperations: IDisposable
    {
        static SqlConnection connection;
        static DataTable datatable;
        private bool _disposed;
        static string _select;
        
        public  DataTable Datatable
        {
            get { return DataTableOperations.datatable; }
            set { DataTableOperations.datatable = value; }
        }



        /// <summary>
        /// Basic Constructor sets _disposed to false to ensure disposal.
        /// </summary>
        public DataTableOperations(){  _disposed = false;}
        /// <summary>
        /// Over ridden constructor this sets the connection for other operations.
        /// </summary>
        /// <param name="strconnection">The string of the connection for the SqlConnection object's connection string</param>
        public DataTableOperations (string strconnection)
        {
            SqlConnection c = new SqlConnection(strconnection);
            connection = c;
            _disposed = false;
        }
        /// <summary>
        /// Overridden Constructor this defines the connection object, initialises a SqlDataAdapter and populates the class' Datatable.
        /// </summary>
        /// <param name="strconnection">The connection string</param>
        /// <param name="strSelect">The select for the DataAdapter</param>
        /// <param name="tablename">The name of the table to output</param>
        public DataTableOperations (string strconnection, string strSelect,string tablename)
        {
            SqlConnection c = new SqlConnection(strconnection += ";Asynchronous Processing=true;");
            connection = c;
            SqlDataAdapter da = new SqlDataAdapter(strSelect, connection);
            DataSet ds = new DataSet();
            
            connection.Open();

            try
            {
                da.Fill(ds, tablename);
            }
            catch (Exception)
            {
                
                throw;
            }
            connection.Close();
            Datatable = ds.Tables[tablename];
            _disposed = false; _select = strSelect;
        }
        
        /// <summary>
        /// Creates and executes a command object for inserting values to a table
        /// </summary>
        /// <param name="insert">The insert SQL string</param>
        public void InsertToTable(string insert)
        {
            SqlCommand cmnd = new SqlCommand(insert, connection);
            connection.Open();
            cmnd.BeginExecuteNonQuery();
        }
        /// <summary>
        /// Builds the insert string for the command.  Parses the fields and values arrays.
        /// </summary>
        /// <param name="tablename">Name of the table to insert to</param>
        /// <param name="fields">An array of field names</param>
        /// <param name="values">An array of values</param>
        /// <returns></returns>
        public string InsertString(string tablename, string[] fields, string[] values)
        {

            if (fields.Length != values.Length)
            {
                throw new Exception(string.Format("Unmatched fields({0}) and values ({1}) were passed please check the application."));
            }
            StringBuilder build = new StringBuilder();
            build.AppendLine(string.Format("INSERT INTO {0}", tablename));
            build.AppendLine("(");
            for (int i = 0; i < fields.Length -1; i++)
            {
                try
                {
                    build.Append(string.Format("{0}, ", fields[i]));
                }
                catch (IndexOutOfRangeException)
                {
                    
                    throw;
                }
            }
            build.Remove(build.Length-2, 2);
            build.Append(")");
            build.AppendLine("VALUES (");
            for (int i = 0; i < fields.Length - 1; i++)
            {
                try
                {
                    build.Append(string.Format("'{0}', ", values[i]));
                }
                catch (IndexOutOfRangeException)
                {

                    throw;
                }
            }
            build.Remove(build.Length-2, 2);
            build.Append(")");
            return build.ToString();
        }
        public bool editrow (string filter,string field, object value  )
        {
            DataRow[] edit = datatable.Select(filter);
            edit[0][field] = value;
            return true;
        }
        public bool editrow(string filter, int field, object value)
        {
            DataRow[] edit = datatable.Select(filter);
            edit[0][field] = value;
            return true;
        }
        public bool editrow(string filter,Tuple<int,object>[] values)
        {
            DataRow[] edit = datatable.Select(filter);
            foreach (Tuple<int,object> item in values)
            {
                edit[0][item.Item1] = item.Item2;
            }
            return true;
        }
        public bool editrow(string filter, Tuple<string, object>[] values)
        {
            DataRow[] edit = datatable.Select(filter);
            foreach (Tuple<string, object> item in values)
            {
                edit[0][item.Item1] = item.Item2;
            }
            return true;
        }

        public bool updatesource()
        {
            DataTable changes = datatable.GetChanges();
            SqlDataAdapter update = new SqlDataAdapter(_select, connection);

            if (connection.State == ConnectionState.Closed) connection.Open();
            SqlCommandBuilder cmndbuilder = new SqlCommandBuilder(update);

            if(changes != null)update.Update(changes);

            if (connection.State == ConnectionState.Open) connection.Close();
            return true;

        }
        /*
         * #########################################          Dispose section         #########################################
         */
        #region Disposing
        public void Dispose()
        {
            Dispose(true);

        }
        /// <summary>
        /// Disposes the SqlConnection and DataTable objects
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            // If you need thread safety, use a lock around these 
            // operations, as well as in your methods that use the resource.
            if (!_disposed)
            {
                if (disposing)
                {
                    //if (obj != null) obj.Dispose();
                    if (connection != null) connection.Dispose();
                    if (datatable != null) datatable.Dispose();

                }

                // Indicate that the instance has been disposed.
                //obj = null;
                connection = null;
                datatable = null;


                _disposed = true;
            }
        }

        #endregion Disposing

       
    }
}
