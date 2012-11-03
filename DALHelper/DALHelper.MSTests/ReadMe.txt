The unit test project contains 2 types of tests: 
1. Unit tests
2. Integrated tests

1. Unit Tests:
The unit tests test the DALHelper's DBDataHelper class's contracts for failure when the contracts are violated.
There are two physical files in the DALHelper.MSTests project to test that:
	a) DBDataHelper.MSTests.Constructor.cs - Tests the constructor level contracts. 
	                                         Does not require test initialization or teardown.
	b) DBDataHelper.MSTests.Unit.Methods.cs - Tests all contracts at the method level. 
											  Requires test initialization and teardown.

2. Integrated Tests:
The integrated tests test whether the DBDataHelper class actually functions as desired and brings in and
writes data to the external database.

NOTE: There is DBDataHelper.MSTests.Base.cs available that does test initialization and teardown to help 
unit and integrated tests. 

Inheritance Hierarchy:

1. DBDataHelperMSTestsUnitConstructor
2. DBDataHelperMSTestsUnitBase
3. DBDataHelperMSTestsUnitMethods: DBDataHelperMSTestsUnitBase
4. DBDataHelperMSTestsIntegratedMethods: DBDataHelperMSTestsUnitBase

About the database:
