using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

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
            if (ConnectionString == string.Empty || ConnectionString == null || ConnectionString.Length <= 0)
            {
                throw new ArgumentNullException("The connection string to the database cannot be null.");
            }
            else if (ConnectionString.ToLowerInvariant().Contains("data source") != true)
            {
                throw new ArgumentException("The connection string is not having a valid data source. Please pass in a valid connection string.");
            }

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
            //Pseudocode
            //1. Add the sql query/stored procedure and connection to db.
            //2. See what command type we need to set - text for query and stored proc for sp.
            //3. Add the parameterCollection after checking whether it is empty or not.
            //4. Return the command.

            if (sqlText == null || sqlText == string.Empty)
            {
                throw new ArgumentNullException("The provided SQL text should be non-null and non-empty.");
            }

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

        #endregion

        #region Non-Async Public Methods

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
            if (sqlText == null || sqlText == string.Empty)
            {
                throw new ArgumentNullException("The provided SQL text should be non-null and non-empty.");
            }

            InitializeCommand(sqlText, sqlTextType, parameterCollection);
            Command.ExecuteNonQuery();
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
            if (sqlText == null || sqlText == string.Empty)
            {
                throw new ArgumentNullException("The provided SQL text should be non-null and non-empty.");
            }

            InitializeCommand(sqlText, sqlTextType, parameterCollection);
            return Command.ExecuteNonQuery();
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
            if (sqlText == null || sqlText == string.Empty)
            {
                throw new ArgumentNullException("The provided SQL text should be non-null and non-empty.");
            }

            InitializeCommand(sqlText, sqlTextType, parameterCollection);
            using (DataTable = new DataTable())
            {
                DataTable.Load(Command.ExecuteReader(CommandBehavior.CloseConnection));
                return DataTable;
            }
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
            if (sqlText == null || sqlText == string.Empty)
            {
                throw new ArgumentNullException("The provided SQL text should be non-null and non-empty.");
            }

            return GetDataTable(sqlText, sqlTextType, parameterCollection).DefaultView;
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
            if (sqlText == null || sqlText == string.Empty)
            {
                throw new ArgumentNullException("The provided SQL text should be non-null and non-empty.");
            }

            InitializeCommand(sqlText, sqlTextType, parameterCollection);
            using (SqlDataAdapter adapter = new SqlDataAdapter(Command))
            {
                DataSet = new DataSet();
                adapter.Fill(DataSet);
                return DataSet;
            }
        }

        #endregion

        #region Async Public Methods

        /// <summary>
        /// Executes a given SQL query or stored procedure.
        /// </summary>
        /// <param name="sqlText">The SQL query/stored procedure passed in to be executed.</param>
        /// <param name="sqlTextType">The enumerated value used to denote whether the passed in sql text is a 
        /// sql query or a stored procedure.</param>
        /// <param name="parameterCollection">The collection of parameters that the sql query/ stored procedure
        /// requires in order to successfully execute.</param>
        public async Task ExecSQLAsync(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc, List<SqlParameter> parameterCollection = null)
        {
            if (sqlText == null || sqlText == string.Empty)
            {
                throw new ArgumentNullException("The provided SQL text should be non-null and non-empty.");
            }

            InitializeCommand(sqlText, sqlTextType, parameterCollection);
            await Command.ExecuteNonQueryAsync();
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
        public async Task<int> GetRowsAffectedAsync(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc, List<SqlParameter> parameterCollection = null)
        {
            if (sqlText == null || sqlText == string.Empty)
            {
                throw new ArgumentNullException("The provided SQL text should be non-null and non-empty.");
            }

            InitializeCommand(sqlText, sqlTextType, parameterCollection);
            return await Command.ExecuteNonQueryAsync();
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
