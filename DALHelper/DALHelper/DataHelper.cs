using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using DALHelper.Enums;

namespace DALHelper
{
    public class DBDataHelper : IDisposable
    {
        protected bool disposed = false;
        public DataTable DataTable { get; private set; }
        public DataSet DataSet { get; private set; }
        public static string ConnectionString { get; set; }
        private SqlCommand Command { get; set; }
        private SqlConnection Connection { get; set; }

        #region Private Methods

        /// <summary>
        /// The GetCommand method is used by public facing method of the helper class.
        /// </summary>
        /// <param name="sqlText">The SQL Query/Stored Procedure passed in to be executed.</param>
        /// <param name="parameterCollection">The collection of parameters that the sql query/ stored procedure
        /// requires in order to successfully execute.</param>
        /// <param name="sqlTextType">The enumerated value used to denote whether the passed in sql text is a 
        /// sql query or a stored procedure.</param>
        /// <returns></returns>
        private void InitializeCommand(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc, SqlParameterCollection parameterCollection = null)
        {
            #region Contract
            Contract.Requires<ArgumentNullException>(sqlText != null, "The sql query text cannot be null. Please provide a valid sql query text.");
            Contract.Requires<ArgumentException>(sqlText.Length > 0, "Please provide a valid sql query text.");
            Contract.Ensures(Command != null);            
            Contract.EndContractBlock();
            #endregion

            //Pseudocode
            //1. Add the sql query/ stored procedure and connection to db.
            //2. See what command type we need to set - text for query and stored proc for sp.
            //3. Add the parameterCollection after checking whether it is empty or not.
            //4. Return the command.

            Command = new SqlCommand(sqlText, ConnectDB());
            switch (sqlTextType)
            {
                //SQL Query
                case SQLTextType.Query:
                    Command.CommandType = CommandType.Text;
                    break;

                //Stored Procedure
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

        /// <summary>
        /// ConnectDB method is used to connect to the database provided by the connection string.
        /// </summary>
        /// <returns>Returns a open connection to the database.</returns>
        private SqlConnection ConnectDB()
        {
            #region Contract
            Contract.Ensures(Contract.Result<SqlConnection>() != null);
            Contract.EndContractBlock();
            #endregion

            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            return Connection;
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            #region Contract
            Contract.Invariant(!String.IsNullOrEmpty(ConnectionString),"Please initialize the connection string before using the helper class");            
            Contract.Invariant(ConnectionString.ToLower().Contains("data source"), "Please pass in a valid connection string. Data Source is missing from the connection string.");
            Contract.Invariant(ConnectionString.ToLower().Contains("initial catalog"), "Please pass in a valid connection string. Initial Catalog is missing from the connection string.");
            Contract.Invariant(ConnectionString.ToLower().Contains("data provider"), "Please pass in a valid connection string. Data Provider is missing from the connection string.");
            #endregion
        }

        #endregion

        #region Public Methods        

        public void ExecSQL(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc, SqlParameterCollection parameterCollection = null)
        {
            #region Contract
            Contract.Requires<ArgumentNullException>(sqlText != null, "The sql query text cannot be null. Please provide a valid sql query text.");
            Contract.Requires<ArgumentException>(sqlText.Length > 0, "Please provide a valid sql query text.");
            Contract.EndContractBlock();
            #endregion

            InitializeCommand(sqlText, sqlTextType, parameterCollection);
            Command.ExecuteNonQuery();
        }

        public void ExecSQL(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc)
        {
            #region Contract
            Contract.Requires<ArgumentNullException>(sqlText != null, "The sql query text cannot be null. Please provide a valid sql query text.");
            Contract.Requires<ArgumentException>(sqlText.Length > 0, "Please provide a valid sql query text.");
            Contract.EndContractBlock();
            #endregion

            ExecSQL(sqlText, sqlTextType, null);
        }

        public int GetRowsAffected(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc, SqlParameterCollection parameterCollection = null)
        {
            #region Contract
            Contract.Requires<ArgumentNullException>(sqlText != null, "The sql query text cannot be null. Please provide a valid sql query text.");
            Contract.Requires<ArgumentException>(sqlText.Length > 0, "Please provide a valid sql query text.");
            Contract.EndContractBlock();
            #endregion

            InitializeCommand(sqlText, sqlTextType, parameterCollection);
            return Command.ExecuteNonQuery();
        }

        public int GetRowsAffected(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc)
        {
            #region Contract
            Contract.Requires<ArgumentNullException>(sqlText != null, "The sql query text cannot be null. Please provide a valid sql query text.");
            Contract.Requires<ArgumentException>(sqlText.Length > 0, "Please provide a valid sql query text.");
            Contract.EndContractBlock();
            #endregion

            return GetRowsAffected(sqlText, sqlTextType, null);
        }

        public DataTable GetDataTable(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc, SqlParameterCollection parameterCollection = null)
        {
            #region Contract
            Contract.Requires<ArgumentNullException>(sqlText != null, "The sql query text cannot be null. Please provide a valid sql query text.");
            Contract.Requires<ArgumentException>(sqlText.Length > 0, "Please provide a valid sql query text.");
            Contract.EndContractBlock();
            #endregion

            InitializeCommand(sqlText, sqlTextType, parameterCollection);
            using (DataTable = new DataTable())
            {
                DataTable.Load(Command.ExecuteReader());
                return DataTable;
            }
        }

        public DataTable GetDataTable(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc)
        {
            #region Contract
            Contract.Requires<ArgumentNullException>(sqlText != null, "The sql query text cannot be null. Please provide a valid sql query text.");
            Contract.Requires<ArgumentException>(sqlText.Length > 0, "Please provide a valid sql query text.");
            Contract.EndContractBlock();
            #endregion

            return GetDataTable(sqlText, sqlTextType, null);
        }

        public SqlDataReader GetDataReader(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc, SqlParameterCollection parameterCollection = null)
        {
            #region Contract
            Contract.Requires<ArgumentNullException>(sqlText != null, "The sql query text cannot be null. Please provide a valid sql query text.");
            Contract.Requires<ArgumentException>(sqlText.Length > 0, "Please provide a valid sql query text.");
            Contract.EndContractBlock();
            #endregion

            InitializeCommand(sqlText, sqlTextType, parameterCollection);
            return Command.ExecuteReader();
        }

        public SqlDataReader GetDataReader(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc)
        {
            #region Contract
            Contract.Requires<ArgumentNullException>(sqlText != null, "The sql query text cannot be null. Please provide a valid sql query text.");
            Contract.Requires<ArgumentException>(sqlText.Length > 0, "Please provide a valid sql query text.");
            Contract.EndContractBlock();
            #endregion

            return GetDataReader(sqlText, sqlTextType, null);
        }

        public DataSet GetDataSet(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc, SqlParameterCollection parameterCollection = null)
        {
            #region Contract
            Contract.Requires<ArgumentNullException>(sqlText != null, "The sql query text cannot be null. Please provide a valid sql query text.");
            Contract.Requires<ArgumentException>(sqlText.Length > 0, "Please provide a valid sql query text.");
            Contract.EndContractBlock();
            #endregion

            InitializeCommand(sqlText, sqlTextType, parameterCollection);
            SqlDataAdapter da = new SqlDataAdapter(Command);
            DataSet = new DataSet();
            da.Fill(DataSet);
            return DataSet;
        }

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            //block will get executed when called by GC. 
            //All the unmanaged resources should be disposed here.
            if (!disposed)
            {
                // Dispose the managed resources if called manually
                //Use this block only for managed resources.
                if (disposing)
                {
                    if (Connection != null)
                    {
                        Connection.Dispose();
                        Connection = null;
                    }
                    if (Command != null)
                    {
                        Command.Dispose();
                        Command = null;
                    }
                }
                disposed = true;
            }
        }

        #endregion
    }
}
