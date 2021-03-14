using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

/*
 * This class is used to do unit testing of all the contracts defined in the DBDataHelper class.
 * The class derives from the DALHelper.Tests.Base class and uses the setup and teardown methods
 * defined in the base class.
 */

namespace DALHelper.MSTests
{
    [TestClass]
    public class DBDataHelperMSTestsUnitMethods : DBDataHelperMSTestsUnitBase
    {
        #region Unit Tests Testing Contracts

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecSQL_NullSQLStatementProvided_ThrowArgumentNullException()
        {            
            try
            {
                dbdatahelperObject.ExecSQL(null, SQLTextType.Query);
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExecSQL_ZeroLengthSQLStatementProvided_ThrowArgumentException()
        {
            try
            {
                dbdatahelperObject.ExecSQL("", SQLTextType.Query);
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetRowsAffected_NullSQLStatementProvided_ThrowArgumentNullException()
        {
            try
            {
                dbdatahelperObject.GetRowsAffected(null, SQLTextType.Query);
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetRowsAffected_ZeroLengthSQLStatementProvided_ThrowArgumentException()
        {
            try
            {
                dbdatahelperObject.GetRowsAffected("", SQLTextType.Query);
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetDataTable_NullSQLStatementProvided_ThrowArgumentNullException()
        {
            try
            {
                dbdatahelperObject.GetDataTable(null, SQLTextType.Query);
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetDataTable_ZeroLengthSQLStatementProvided_ThrowArgumentException()
        {
            try
            {
                dbdatahelperObject.GetDataTable("", SQLTextType.Query);
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetDataView_NullSQLStatementProvided_ThrowArgumentNullException()
        {
            try
            {
                dbdatahelperObject.GetDataView(null, SQLTextType.Query);
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetDataView_ZeroLengthSQLStatementProvided_ThrowArgumentException()
        {
            try
            {
                dbdatahelperObject.GetDataView("", SQLTextType.Query);
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetDataReader_NullSQLStatementProvided_ThrowArgumentNullException()
        {
            try
            {
                dbdatahelperObject.GetDataTable(null, SQLTextType.Query);
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetDataReader_ZeroLengthSQLStatementProvided_ThrowArgumentException()
        {
            try
            {
                dbdatahelperObject.GetDataTable("", SQLTextType.Query);
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetDataSet_NullSQLStatementProvided_ThrowArgumentNullException()
        {
            try
            {
                dbdatahelperObject.GetDataTable(null, SQLTextType.Query);
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetDataSet_ZeroLengthSQLStatementProvided_ThrowArgumentException()
        {
            try
            {
                dbdatahelperObject.GetDataTable("", SQLTextType.Query);
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
