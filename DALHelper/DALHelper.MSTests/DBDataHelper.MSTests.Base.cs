﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DALHelper;

/*This base class does the work of setting up the connection string and 
 * initializing an object of the DBDataHelper class. The connection string
 * and the object can then be used by the derived DBDataHelperMSTestsUnitMethods 
 * and DBDataHelperMSTestsIntegratedMethods classes.
 */

namespace DALHelper.MSTests
{
    [TestClass]
    public abstract class DBDataHelperMSTestsUnitBase
    {
        protected DBDataHelper dbdatahelperObject;

        #region Test Initialization and Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            DBDataHelper.ConnectionString = @"Data Source = .\SQLEXPRESS;";

            dbdatahelperObject = new DBDataHelper();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            DBDataHelper.ConnectionString = null;
            dbdatahelperObject = null;
        }

        #endregion
    }
}
