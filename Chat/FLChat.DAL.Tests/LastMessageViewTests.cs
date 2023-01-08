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
    public class LastMessageViewTests : SqlDatabaseTestClass
    {

        public LastMessageViewTests() {
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
        public void LastMessageViewTest() {
            SqlDatabaseTestActions testActions = this.LastMessageViewTestData;
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

        #region Designer support code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction LastMessageViewTest_TestAction;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LastMessageViewTests));
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition emptyForNewCreatedUsers;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition checkMsg1U1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition checkMsg1U2;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition checkMsg2U1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition checkMsg2U2;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition checkMsg3U1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition checkMsg3U2;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition emptyDublicates;
            this.LastMessageViewTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            LastMessageViewTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            emptyForNewCreatedUsers = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            checkMsg1U1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            checkMsg1U2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            checkMsg2U1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            checkMsg2U2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            checkMsg3U1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            checkMsg3U2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            emptyDublicates = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            // 
            // LastMessageViewTestData
            // 
            this.LastMessageViewTestData.PosttestAction = null;
            this.LastMessageViewTestData.PretestAction = null;
            this.LastMessageViewTestData.TestAction = LastMessageViewTest_TestAction;
            // 
            // LastMessageViewTest_TestAction
            // 
            LastMessageViewTest_TestAction.Conditions.Add(emptyForNewCreatedUsers);
            LastMessageViewTest_TestAction.Conditions.Add(checkMsg1U1);
            LastMessageViewTest_TestAction.Conditions.Add(checkMsg1U2);
            LastMessageViewTest_TestAction.Conditions.Add(checkMsg2U1);
            LastMessageViewTest_TestAction.Conditions.Add(checkMsg2U2);
            LastMessageViewTest_TestAction.Conditions.Add(checkMsg3U1);
            LastMessageViewTest_TestAction.Conditions.Add(checkMsg3U2);
            LastMessageViewTest_TestAction.Conditions.Add(emptyDublicates);
            resources.ApplyResources(LastMessageViewTest_TestAction, "LastMessageViewTest_TestAction");
            // 
            // emptyForNewCreatedUsers
            // 
            emptyForNewCreatedUsers.Enabled = true;
            emptyForNewCreatedUsers.Name = "emptyForNewCreatedUsers";
            emptyForNewCreatedUsers.ResultSet = 1;
            // 
            // checkMsg1U1
            // 
            checkMsg1U1.ColumnNumber = 1;
            checkMsg1U1.Enabled = true;
            checkMsg1U1.ExpectedValue = "1";
            checkMsg1U1.Name = "checkMsg1U1";
            checkMsg1U1.NullExpected = false;
            checkMsg1U1.ResultSet = 2;
            checkMsg1U1.RowNumber = 1;
            // 
            // checkMsg1U2
            // 
            checkMsg1U2.ColumnNumber = 1;
            checkMsg1U2.Enabled = true;
            checkMsg1U2.ExpectedValue = "1";
            checkMsg1U2.Name = "checkMsg1U2";
            checkMsg1U2.NullExpected = false;
            checkMsg1U2.ResultSet = 3;
            checkMsg1U2.RowNumber = 1;
            // 
            // checkMsg2U1
            // 
            checkMsg2U1.ColumnNumber = 1;
            checkMsg2U1.Enabled = true;
            checkMsg2U1.ExpectedValue = "1";
            checkMsg2U1.Name = "checkMsg2U1";
            checkMsg2U1.NullExpected = false;
            checkMsg2U1.ResultSet = 4;
            checkMsg2U1.RowNumber = 1;
            // 
            // checkMsg2U2
            // 
            checkMsg2U2.ColumnNumber = 1;
            checkMsg2U2.Enabled = true;
            checkMsg2U2.ExpectedValue = "1";
            checkMsg2U2.Name = "checkMsg2U2";
            checkMsg2U2.NullExpected = false;
            checkMsg2U2.ResultSet = 5;
            checkMsg2U2.RowNumber = 1;
            // 
            // checkMsg3U1
            // 
            checkMsg3U1.ColumnNumber = 1;
            checkMsg3U1.Enabled = true;
            checkMsg3U1.ExpectedValue = "1";
            checkMsg3U1.Name = "checkMsg3U1";
            checkMsg3U1.NullExpected = false;
            checkMsg3U1.ResultSet = 6;
            checkMsg3U1.RowNumber = 1;
            // 
            // checkMsg3U2
            // 
            checkMsg3U2.ColumnNumber = 1;
            checkMsg3U2.Enabled = true;
            checkMsg3U2.ExpectedValue = "1";
            checkMsg3U2.Name = "checkMsg3U2";
            checkMsg3U2.NullExpected = false;
            checkMsg3U2.ResultSet = 7;
            checkMsg3U2.RowNumber = 1;
            // 
            // emptyDublicates
            // 
            emptyDublicates.Enabled = true;
            emptyDublicates.Name = "emptyDublicates";
            emptyDublicates.ResultSet = 8;
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

        private SqlDatabaseTestActions LastMessageViewTestData;
    }
}
