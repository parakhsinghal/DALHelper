using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;

namespace DALHelper
{
    /// <summary>
    /// The class houses the methods that can be used to pull and push data into 
    /// a SQL Server database.
    /// </summary>
    public class DBDataHelper : IDisposable
    {
        #region Private properties

        /// <summary>
        /// The bool type disposed field is used to denote whether the user handled 
        /// the object of DBDataHelper class for disposal, or whether the Garbage Collector
        /// handled the disposal.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// The Command property of is used internally by methods in the library.
        /// </summary>
        private SqlCommand Command { get; set; }

        /// <summary>
        /// The Connection property is used internally by methods in the library.
        /// </summary>
        private SqlConnection Connection { get; set; }

        #endregion

        #region Public properties
        /// <summary>
        /// DataTable that can be publicly read, but only privately set.
        /// Used in methods that return data table.
        /// /// </summary>
        public DataTable DataTable { get; private set; }

        /// <summary>
        /// DataSet that can be publicly read, but only privately set.
        /// Used in methods that return dataset.
        /// </summary>
        public DataSet DataSet { get; private set; }

        /// <summary>
        /// Gets and sets the connection that is used to connect to the database. 
        /// Implementation of dependency injection via property injection.
        /// </summary>
        public static string ConnectionString { get; set; }

        #endregion

        #region Private Methods

        /// <summary>
        /// ConnectToDB method is used to connect to the database with the help 
        /// of the ConnectionString property.
        /// </summary>
        /// <returns>Returns a open connection to the database.</returns>
        private SqlConnection ConnectToDB()
        {
            #region Contract
            Contract.Ensures(Contract.Result<SqlConnection>() != null);
            Contract.EndContractBlock();
            #endregion

            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            return Connection;
        }

        /// <summary>
        /// The GetCommand method is used by public facing method(s) of the helper library.
        /// </summary>
        /// <param name="sqlText">The SQL query/stored Procedure passed in to be executed.</param>
        /// <param name="sqlTextType">The enumerated value used to denote whether the passed in sql text is a 
        /// sql query or a stored procedure.</param>
        /// <param name="parameterCollection">The collection of parameters that the sql query/stored procedure
        /// requires in order to successfully execute.</param>
        /// <returns></returns>
        private void InitializeCommand(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc, List<SqlParameter> parameterCollection = null)
        {
            #region Contract
            Contract.Requires<ArgumentNullException>(sqlText != null, "The sql query text cannot be null. Please provide a valid sql query text.");
            Contract.Requires<ArgumentException>(sqlText.Length > 0, "Please provide a valid sql query text.");
            Contract.Ensures(Command != null);
            Contract.EndContractBlock();
            #endregion

            //Pseudocode
            //1. Add the sql query/stored procedure and connection to db.
            //2. See what command type we need to set - text for query and stored proc for sp.
            //3. Add the parameterCollection after checking whether it is empty or not.
            //4. Return the command.

            Command = new SqlCommand(sqlText, ConnectToDB());
            switch (sqlTextType)
            {
                case SQLTextType.Query:
                    Command.CommandType = CommandType.Text;
                    break;

                default:
                    Command.CommandType = CommandType.StoredProcedure;
                    break;
            }

            if (parameterCollection != null && parameterCollection.Count > 0)
            {
                foreach (var parameter in parameterCollection)
                {
                    Command.Parameters.Add(parameter);
                }
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            #region Contract
            Contract.Invariant(!String.IsNullOrEmpty(ConnectionString), "Please initialize the connection string before using the helper library");
            Contract.Invariant(ConnectionString.ToLower().Contains("data source"), "Please pass in a valid connection string. Data Source is missing from the connection string.");
            #endregion
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Executes a given SQL query or stored procedure.
        /// </summary>
        /// <param name="sqlText">The SQL query/stored procedure passed in to be executed.</param>
        /// <param name="sqlTextType">The enumerated value used to denote whether the passed in sql text is a 
        /// sql query or a stored procedure.</param>
        /// <param name="parameterCollection">The collection of parameters that the sql query/ stored procedure
        /// requires in order to successfully execute.</param>
        public void ExecSQL(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc, List<SqlParameter> parameterCollection = null)
        {
            #region Contract
            Contract.Requires<ArgumentNullException>(sqlText != null, "The sql query text cannot be null. Please provide a valid sql query text.");
            Contract.Requires<ArgumentException>(sqlText.Length > 0, "Please provide a valid sql query text.");
            Contract.EndContractBlock();
            #endregion

            InitializeCommand(sqlText, sqlTextType, parameterCollection);
            Command.ExecuteNonQuery();
        }

        /// <summary>
        /// Executes a given SQL query or stored procedure.
        /// </summary>
        /// <param name="sqlText">The SQL query/stored procedure passed in to be executed.</param>
        /// <param name="sqlTextType">The enumerated value used to denote whether the passed in sql text is a 
        /// sql query or a stored procedure.</param>
        public void ExecSQL(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc)
        {
            #region Contract
            Contract.Requires<ArgumentNullException>(sqlText != null, "The sql query text cannot be null. Please provide a valid sql query text.");
            Contract.Requires<ArgumentException>(sqlText.Length > 0, "Please provide a valid sql query text.");
            Contract.EndContractBlock();
            #endregion

            ExecSQL(sqlText, sqlTextType, null);
        }

        /// <summary>
        /// Executes a given SQL query/stored procedure and returns the total number of rows affected.
        /// </summary>
        /// <param name="sqlText">The SQL query/stored procedure passed in to be executed.</param>
        /// <param name="sqlTextType">The enumerated value used to denote whether the passed in sql text is a 
        /// sql query or a stored procedure.</param>
        /// <param name="parameterCollection">The collection of parameters that the sql query/ stored procedure
        /// requires in order to successfully execute.</param>
        /// <returns>Returns the total number of rows affected.</returns>
        public int GetRowsAffected(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc, List<SqlParameter> parameterCollection = null)
        {
            #region Contract
            Contract.Requires<ArgumentNullException>(sqlText != null, "The sql query text cannot be null. Please provide a valid sql query text.");
            Contract.Requires<ArgumentException>(sqlText.Length > 0, "Please provide a valid sql query text.");
            Contract.EndContractBlock();
            #endregion

            InitializeCommand(sqlText, sqlTextType, parameterCollection);
            return Command.ExecuteNonQuery();
        }

        /// <summary>
        /// Executes a given SQL query/stored procedure and returns the total number of rows affected.
        /// </summary>
        /// <param name="sqlText">The SQL query/stored procedure passed in to be executed.</param>
        /// <param name="sqlTextType">The enumerated value used to denote whether the passed in sql text is a 
        /// sql query or a stored procedure.</param>
        /// <returns>Returns the total number of rows affected.</returns>
        public int GetRowsAffected(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc)
        {
            #region Contract
            Contract.Requires<ArgumentNullException>(sqlText != null, "The sql query text cannot be null. Please provide a valid sql query text.");
            Contract.Requires<ArgumentException>(sqlText.Length > 0, "Please provide a valid sql query text.");
            Contract.EndContractBlock();
            #endregion

            return GetRowsAffected(sqlText, sqlTextType, null);
        }

        /// <summary>
        /// Executes a given SQL query/stored procedure and returns the result set in a data table.
        /// </summary>
        /// <param name="sqlText">The SQL query/stored procedure passed in to be executed.</param>
        /// <param name="sqlTextType">The enumerated value used to denote whether the passed in sql text is a 
        /// sql query or a stored procedure.</param>
        /// <param name="parameterCollection">The collection of parameters that the sql query/ stored procedure
        /// requires in order to successfully execute.</param>
        /// <returns>Returns a data table containing the result set.</returns>
        public DataTable GetDataTable(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc, List<SqlParameter> parameterCollection = null)
        {
            #region Contract
            Contract.Requires<ArgumentNullException>(sqlText != null, "The sql query text cannot be null. Please provide a valid sql query text.");
            Contract.Requires<ArgumentException>(sqlText.Length > 0, "Please provide a valid sql query text.");
            Contract.EndContractBlock();
            #endregion

            InitializeCommand(sqlText, sqlTextType, parameterCollection);
            using (DataTable = new DataTable())
            {
                DataTable.Load(Command.ExecuteReader(CommandBehavior.CloseConnection));
                return DataTable;
            }
        }

        /// <summary>
        /// Executes a given SQL query/stored procedure and returns the result set in a data table.
        /// </summary>
        /// <param name="sqlText">The SQL query/stored procedure passed in to be executed.</param>
        /// <param name="sqlTextType">The enumerated value used to denote whether the passed in sql text is a 
        /// sql query or a stored procedure.</param>
        /// <returns>Returns a data table containing the result set.</returns>
        public DataTable GetDataTable(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc)
        {
            #region Contract
            Contract.Requires<ArgumentNullException>(sqlText != null, "The sql query text cannot be null. Please provide a valid sql query text.");
            Contract.Requires<ArgumentException>(sqlText.Length > 0, "Please provide a valid sql query text.");
            Contract.EndContractBlock();
            #endregion

            return GetDataTable(sqlText, sqlTextType, null);
        }

        /// <summary>
        /// Executes a given SQL query/stored procedure and returns the result set in a data view.
        /// </summary>
        /// <param name="sqlText">The SQL query/stored procedure passed in to be executed.</param>
        /// <param name="sqlTextType">The enumerated value used to denote whether the passed in sql text is a 
        /// sql query or a stored procedure.</param>
        /// <param name="parameterCollection">The collection of parameters that the sql query/ stored procedure
        /// requires in order to successfully execute.</param>
        /// <returns>Returns a data view containing the result set.</returns>
        public DataView GetDataView(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc, List<SqlParameter> parameterCollection = null)
        {
            #region Contract
            Contract.Requires<ArgumentNullException>(sqlText != null, "The sql query text cannot be null. Please provide a valid sql query text.");
            Contract.Requires<ArgumentException>(sqlText.Length > 0, "Please provide a valid sql query text.");
            Contract.EndContractBlock();
            #endregion

            return GetDataTable(sqlText, sqlTextType, parameterCollection).AsDataView();
        }

        /// <summary>
        /// Executes a given SQL query/stored procedure and returns the result set in a data view.
        /// </summary>
        /// <param name="sqlText">The SQL query/stored procedure passed in to be executed.</param>
        /// <param name="sqlTextType">The enumerated value used to denote whether the passed in sql text is a 
        /// sql query or a stored procedure.</param>
        /// <returns>Returns a data view containing the result set.</returns>
        public DataView GetDataView(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc)
        {
            #region Contract
            Contract.Requires<ArgumentNullException>(sqlText != null, "The sql query text cannot be null. Please provide a valid sql query text.");
            Contract.Requires<ArgumentException>(sqlText.Length > 0, "Please provide a valid sql query text.");
            Contract.EndContractBlock();
            #endregion

            return GetDataView(sqlText, sqlTextType, null);
        }

        /// <summary>
        /// Executes a given SQL query/stored procedure and returns the result set(s) in a data set.
        /// </summary>
        /// <param name="sqlText">The SQL query/stored procedure passed in to be executed.</param>
        /// <param name="sqlTextType">The enumerated value used to denote whether the passed in sql text is a 
        /// sql query or a stored procedure.</param>
        /// <param name="parameterCollection">The collection of parameters that the sql query/ stored procedure
        /// requires in order to successfully execute.</param>
        /// <returns>Returns a data set containing the result set(s).</returns>
        public DataSet GetDataSet(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc, List<SqlParameter> parameterCollection = null)
        {
            #region Contract
            Contract.Requires<ArgumentNullException>(sqlText != null, "The sql query text cannot be null. Please provide a valid sql query text.");
            Contract.Requires<ArgumentException>(sqlText.Length > 0, "Please provide a valid sql query text.");
            Contract.EndContractBlock();
            #endregion

            InitializeCommand(sqlText, sqlTextType, parameterCollection);
            using (SqlDataAdapter adapter = new SqlDataAdapter(Command))
            {
                DataSet = new DataSet();
                adapter.Fill(DataSet);
                return DataSet;
            }
        }

        /// <summary>
        /// Executes a given SQL query/stored procedure and returns the result set(s) in a data set.
        /// </summary>
        /// <param name="sqlText">The SQL query/stored procedure passed in to be executed.</param>
        /// <param name="sqlTextType">The enumerated value used to denote whether the passed in sql text is a 
        /// sql query or a stored procedure.</param>
        /// <returns>Returns a data set containing the result set(s).</returns>
        public DataSet GetDataSet(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc)
        {
            #region Contract
            Contract.Requires<ArgumentNullException>(sqlText != null, "The sql query text cannot be null. Please provide a valid sql query text.");
            Contract.Requires<ArgumentException>(sqlText.Length > 0, "Please provide a valid sql query text.");
            Contract.EndContractBlock();
            #endregion

            return GetDataSet(sqlText, sqlTextType, null);
        }

        #endregion

        #region IDisposable implementation

        /// <summary>
        /// Cleanas up the managed resources used by the library, namely 
        /// the connection to the database and the command.
        /// </summary>
        public void Dispose()
        {
            //Call the helper method to clean up the managed resources.
            CleanUp(true);

            //Because the user explicitly took care of the
            //disposal, suppress the finalization of this object.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Executes the clean up procedure for public method Dispose and 
        /// for finalizer of the class. If passed true as the parameter, then performs 
        /// the clean up of both managed and unmanaged resources. If passed false, then
        /// it means that garbage collector called up the finalizer and only the unmanaged
        /// resources need clean up.
        /// </summary>
        /// <param name="disposing"></param>
        private void CleanUp(bool disposing)
        {
            if (!this.disposed)
            {
                //Use this block only for managed resources.
                //Dispose the managed resources if called by user.                
                if (disposing)
                {
                    //Dispose the connection to the database.
                    Connection.Dispose();

                    //Dispose the command used internally by the public methods.
                    Command.Dispose();
                }

                //Clean up any unmanaged resources here, if any.
            }

            disposed = true;
        }

        /// <summary>
        /// The finalizer that will be activated by the garbage collector
        /// in the event the user fails to properly dispose off the object.
        /// </summary>
        ~DBDataHelper()
        {
            //Call the helper method with false as the passed parameter
            //to signify that GC triggered the cleanup.
            CleanUp(false);
        }

        #endregion
    }
}
