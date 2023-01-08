using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.DAL.Tests
{
    [TestClass()]
    public class SegmentTests : SqlDatabaseTestClass
    {

        public SegmentTests() {
            InitializeComponent();
        }

        [TestInitialize()]
        public void TestInitialize() {
            base.InitializeTest();
        }
        [TestCleanup()]
        public void TestCleanup() {
            base.CleanupTest();
        }

        [TestMethod()]
        public void Segment_UpdateMembersTest() {
            SqlDatabaseTestActions testActions = this.Segment_UpdateMembersTestData;
            // Execute the pre-test script
            // 
            System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
            SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
            // Execute the test script
            // 
            System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
            SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
            // Execute the post-test script
            // 
            System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
            SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
        }
        [TestMethod()]
        public void Segment_GetMembers_Test() {
            SqlDatabaseTestActions testActions = this.Segment_GetMembers_TestData;
            // Execute the pre-test script
            // 
            System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
            SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
            try {
                // Execute the test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
                SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
            } finally {
                // Execute the post-test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
                SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
            }
        }



        #region Designer support code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction Segment_UpdateMembersTest_TestAction;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SegmentTests));
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition SgmUpdateMembers_1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition SgmUpdateMembers_2;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition SgmUpdateMembers_3;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition SgmUpdateMembers_4;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition SgmUpdateMembers_5;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction Segment_GetMembers_Test_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition GetMembers_RowCount1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition GetMembers_RowCount2;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition GetMembers_CheckUser1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition GetMembers_CheckUser2;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition GetMembers_RowCount5;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition GetMembers_RowCount6;
            this.Segment_UpdateMembersTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.Segment_GetMembers_TestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            Segment_UpdateMembersTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            SgmUpdateMembers_1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            SgmUpdateMembers_2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            SgmUpdateMembers_3 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            SgmUpdateMembers_4 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            SgmUpdateMembers_5 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Segment_GetMembers_Test_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            GetMembers_RowCount1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            GetMembers_RowCount2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            GetMembers_CheckUser1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            GetMembers_CheckUser2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            GetMembers_RowCount5 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            GetMembers_RowCount6 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            // 
            // Segment_UpdateMembersTest_TestAction
            // 
            Segment_UpdateMembersTest_TestAction.Conditions.Add(SgmUpdateMembers_1);
            Segment_UpdateMembersTest_TestAction.Conditions.Add(SgmUpdateMembers_2);
            Segment_UpdateMembersTest_TestAction.Conditions.Add(SgmUpdateMembers_3);
            Segment_UpdateMembersTest_TestAction.Conditions.Add(SgmUpdateMembers_4);
            Segment_UpdateMembersTest_TestAction.Conditions.Add(SgmUpdateMembers_5);
            resources.ApplyResources(Segment_UpdateMembersTest_TestAction, "Segment_UpdateMembersTest_TestAction");
            // 
            // SgmUpdateMembers_1
            // 
            SgmUpdateMembers_1.ColumnNumber = 1;
            SgmUpdateMembers_1.Enabled = true;
            SgmUpdateMembers_1.ExpectedValue = "3";
            SgmUpdateMembers_1.Name = "SgmUpdateMembers_1";
            SgmUpdateMembers_1.NullExpected = false;
            SgmUpdateMembers_1.ResultSet = 1;
            SgmUpdateMembers_1.RowNumber = 1;
            // 
            // SgmUpdateMembers_2
            // 
            SgmUpdateMembers_2.ColumnNumber = 2;
            SgmUpdateMembers_2.Enabled = true;
            SgmUpdateMembers_2.ExpectedValue = "3";
            SgmUpdateMembers_2.Name = "SgmUpdateMembers_2";
            SgmUpdateMembers_2.NullExpected = false;
            SgmUpdateMembers_2.ResultSet = 1;
            SgmUpdateMembers_2.RowNumber = 1;
            // 
            // SgmUpdateMembers_3
            // 
            SgmUpdateMembers_3.ColumnNumber = 1;
            SgmUpdateMembers_3.Enabled = true;
            SgmUpdateMembers_3.ExpectedValue = "2";
            SgmUpdateMembers_3.Name = "SgmUpdateMembers_3";
            SgmUpdateMembers_3.NullExpected = false;
            SgmUpdateMembers_3.ResultSet = 2;
            SgmUpdateMembers_3.RowNumber = 1;
            // 
            // SgmUpdateMembers_4
            // 
            SgmUpdateMembers_4.ColumnNumber = 2;
            SgmUpdateMembers_4.Enabled = true;
            SgmUpdateMembers_4.ExpectedValue = "2";
            SgmUpdateMembers_4.Name = "SgmUpdateMembers_4";
            SgmUpdateMembers_4.NullExpected = false;
            SgmUpdateMembers_4.ResultSet = 2;
            SgmUpdateMembers_4.RowNumber = 1;
            // 
            // SgmUpdateMembers_5
            // 
            SgmUpdateMembers_5.ColumnNumber = 1;
            SgmUpdateMembers_5.Enabled = true;
            SgmUpdateMembers_5.ExpectedValue = "0";
            SgmUpdateMembers_5.Name = "SgmUpdateMembers_5";
            SgmUpdateMembers_5.NullExpected = false;
            SgmUpdateMembers_5.ResultSet = 3;
            SgmUpdateMembers_5.RowNumber = 1;
            // 
            // Segment_UpdateMembersTestData
            // 
            this.Segment_UpdateMembersTestData.PosttestAction = null;
            this.Segment_UpdateMembersTestData.PretestAction = null;
            this.Segment_UpdateMembersTestData.TestAction = Segment_UpdateMembersTest_TestAction;
            // 
            // Segment_GetMembers_TestData
            // 
            this.Segment_GetMembers_TestData.PosttestAction = null;
            this.Segment_GetMembers_TestData.PretestAction = null;
            this.Segment_GetMembers_TestData.TestAction = Segment_GetMembers_Test_TestAction;
            // 
            // Segment_GetMembers_Test_TestAction
            // 
            Segment_GetMembers_Test_TestAction.Conditions.Add(GetMembers_RowCount1);
            Segment_GetMembers_Test_TestAction.Conditions.Add(GetMembers_RowCount2);
            Segment_GetMembers_Test_TestAction.Conditions.Add(GetMembers_CheckUser1);
            Segment_GetMembers_Test_TestAction.Conditions.Add(GetMembers_CheckUser2);
            Segment_GetMembers_Test_TestAction.Conditions.Add(GetMembers_RowCount5);
            Segment_GetMembers_Test_TestAction.Conditions.Add(GetMembers_RowCount6);
            resources.ApplyResources(Segment_GetMembers_Test_TestAction, "Segment_GetMembers_Test_TestAction");
            // 
            // GetMembers_RowCount1
            // 
            GetMembers_RowCount1.Enabled = true;
            GetMembers_RowCount1.Name = "GetMembers_RowCount1";
            GetMembers_RowCount1.ResultSet = 1;
            GetMembers_RowCount1.RowCount = 1;
            // 
            // GetMembers_RowCount2
            // 
            GetMembers_RowCount2.Enabled = true;
            GetMembers_RowCount2.Name = "GetMembers_RowCount2";
            GetMembers_RowCount2.ResultSet = 2;
            GetMembers_RowCount2.RowCount = 1;
            // 
            // GetMembers_CheckUser1
            // 
            GetMembers_CheckUser1.ColumnNumber = 1;
            GetMembers_CheckUser1.Enabled = true;
            GetMembers_CheckUser1.ExpectedValue = "1";
            GetMembers_CheckUser1.Name = "GetMembers_CheckUser1";
            GetMembers_CheckUser1.NullExpected = false;
            GetMembers_CheckUser1.ResultSet = 3;
            GetMembers_CheckUser1.RowNumber = 1;
            // 
            // GetMembers_CheckUser2
            // 
            GetMembers_CheckUser2.ColumnNumber = 1;
            GetMembers_CheckUser2.Enabled = true;
            GetMembers_CheckUser2.ExpectedValue = "1";
            GetMembers_CheckUser2.Name = "GetMembers_CheckUser2";
            GetMembers_CheckUser2.NullExpected = false;
            GetMembers_CheckUser2.ResultSet = 4;
            GetMembers_CheckUser2.RowNumber = 1;
            // 
            // GetMembers_RowCount5
            // 
            GetMembers_RowCount5.Enabled = true;
            GetMembers_RowCount5.Name = "GetMembers_RowCount5";
            GetMembers_RowCount5.ResultSet = 5;
            GetMembers_RowCount5.RowCount = 1;
            // 
            // GetMembers_RowCount6
            // 
            GetMembers_RowCount6.Enabled = true;
            GetMembers_RowCount6.Name = "GetMembers_RowCount6";
            GetMembers_RowCount6.ResultSet = 6;
            GetMembers_RowCount6.RowCount = 0;
        }

        #endregion


        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        #endregion

        private SqlDatabaseTestActions Segment_UpdateMembersTestData;
        private SqlDatabaseTestActions Segment_GetMembers_TestData;
    }
}
