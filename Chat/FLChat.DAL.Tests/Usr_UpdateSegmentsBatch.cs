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
    public class Usr_UpdateSegmentsBatch : SqlDatabaseTestClass
    {

        public Usr_UpdateSegmentsBatch() {
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
        public void UpdateSegmentsBatch() {
            SqlDatabaseTestActions testActions = this.UpdateSegmentsBatchData;
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
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction UpdateSegmentsBatch_TestAction;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Usr_UpdateSegmentsBatch));
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition update1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition update2;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition update3;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition update4;
            this.UpdateSegmentsBatchData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            UpdateSegmentsBatch_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            update1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            update2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            update3 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            update4 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            // 
            // UpdateSegmentsBatchData
            // 
            this.UpdateSegmentsBatchData.PosttestAction = null;
            this.UpdateSegmentsBatchData.PretestAction = null;
            this.UpdateSegmentsBatchData.TestAction = UpdateSegmentsBatch_TestAction;
            // 
            // UpdateSegmentsBatch_TestAction
            // 
            UpdateSegmentsBatch_TestAction.Conditions.Add(update1);
            UpdateSegmentsBatch_TestAction.Conditions.Add(update2);
            UpdateSegmentsBatch_TestAction.Conditions.Add(update3);
            UpdateSegmentsBatch_TestAction.Conditions.Add(update4);
            resources.ApplyResources(UpdateSegmentsBatch_TestAction, "UpdateSegmentsBatch_TestAction");
            // 
            // update1
            // 
            update1.ColumnNumber = 1;
            update1.Enabled = true;
            update1.ExpectedValue = "1";
            update1.Name = "update1";
            update1.NullExpected = false;
            update1.ResultSet = 1;
            update1.RowNumber = 1;
            // 
            // update2
            // 
            update2.ColumnNumber = 1;
            update2.Enabled = true;
            update2.ExpectedValue = "1";
            update2.Name = "update2";
            update2.NullExpected = false;
            update2.ResultSet = 2;
            update2.RowNumber = 1;
            // 
            // update3
            // 
            update3.ColumnNumber = 1;
            update3.Enabled = true;
            update3.ExpectedValue = "1";
            update3.Name = "update3";
            update3.NullExpected = false;
            update3.ResultSet = 3;
            update3.RowNumber = 1;
            // 
            // update4
            // 
            update4.ColumnNumber = 1;
            update4.Enabled = true;
            update4.ExpectedValue = "1";
            update4.Name = "update4";
            update4.NullExpected = false;
            update4.ResultSet = 4;
            update4.RowNumber = 1;
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

        private SqlDatabaseTestActions UpdateSegmentsBatchData;
    }
}
