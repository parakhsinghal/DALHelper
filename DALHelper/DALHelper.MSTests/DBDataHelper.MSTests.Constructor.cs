using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

/*This class is used to do unit testing of constructor of the DBDataHelper class. 
 * It is a special case, and different from the rest of the unit tests because it 
 * deliberately tries not to initialize the connection string and test whether the 
 * contracts breaks. This class   DOES   NOT   USES   the DBDataHelperMSTestsUnitBase class for any test initialization or teardown.
 */

namespace DALHelper.MSTests
{
    [TestClass]
    public class DBDataHelperMSTestsUnitConstructor
    {
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void DBDataHelper_InitializeConstructorWithoutInitializingConnectionString_ThrowException()
        {
            try
            {
                DBDataHelper dbdatahelperObject = new DBDataHelper();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
