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
    public class BroadcastProhibitionTests : SqlDatabaseTestClass
    {

        public BroadcastProhibitionTests() {
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
        public void BroadcastProhibition_Structure() {
            SqlDatabaseTestActions testActions = this.BroadcastProhibition_StructureData;
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
        public void BroadcastProhibition_StructureUpwards() {
            SqlDatabaseTestActions testActions = this.BroadcastProhibition_StructureUpwardsData;
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
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction BroadcastProhibition_Structure_TestAction;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BroadcastProhibitionTests));
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition BroadcastProhibition_Structure_Count;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition BroadcastProhibition_Structure_Empty;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction BroadcastProhibition_StructureUpwards_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition upwards_1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition upwards_2;
            this.BroadcastProhibition_StructureData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.BroadcastProhibition_StructureUpwardsData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            BroadcastProhibition_Structure_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            BroadcastProhibition_Structure_Count = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            BroadcastProhibition_Structure_Empty = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            BroadcastProhibition_StructureUpwards_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            upwards_1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            upwards_2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            // 
            // BroadcastProhibition_Structure_TestAction
            // 
            BroadcastProhibition_Structure_TestAction.Conditions.Add(BroadcastProhibition_Structure_Count);
            BroadcastProhibition_Structure_TestAction.Conditions.Add(BroadcastProhibition_Structure_Empty);
            resources.ApplyResources(BroadcastProhibition_Structure_TestAction, "BroadcastProhibition_Structure_TestAction");
            // 
            // BroadcastProhibition_Structure_Count
            // 
            BroadcastProhibition_Structure_Count.Enabled = true;
            BroadcastProhibition_Structure_Count.Name = "BroadcastProhibition_Structure_Count";
            BroadcastProhibition_Structure_Count.ResultSet = 1;
            BroadcastProhibition_Structure_Count.RowCount = 2;
            // 
            // BroadcastProhibition_Structure_Empty
            // 
            BroadcastProhibition_Structure_Empty.Enabled = true;
            BroadcastProhibition_Structure_Empty.Name = "BroadcastProhibition_Structure_Empty";
            BroadcastProhibition_Structure_Empty.ResultSet = 2;
            // 
            // BroadcastProhibition_StructureData
            // 
            this.BroadcastProhibition_StructureData.PosttestAction = null;
            this.BroadcastProhibition_StructureData.PretestAction = null;
            this.BroadcastProhibition_StructureData.TestAction = BroadcastProhibition_Structure_TestAction;
            // 
            // BroadcastProhibition_StructureUpwardsData
            // 
            this.BroadcastProhibition_StructureUpwardsData.PosttestAction = null;
            this.BroadcastProhibition_StructureUpwardsData.PretestAction = null;
            this.BroadcastProhibition_StructureUpwardsData.TestAction = BroadcastProhibition_StructureUpwards_TestAction;
            // 
            // BroadcastProhibition_StructureUpwards_TestAction
            // 
            BroadcastProhibition_StructureUpwards_TestAction.Conditions.Add(upwards_1);
            BroadcastProhibition_StructureUpwards_TestAction.Conditions.Add(upwards_2);
            resources.ApplyResources(BroadcastProhibition_StructureUpwards_TestAction, "BroadcastProhibition_StructureUpwards_TestAction");
            // 
            // upwards_1
            // 
            upwards_1.Enabled = true;
            upwards_1.Name = "upwards_1";
            upwards_1.ResultSet = 1;
            upwards_1.RowCount = 2;
            // 
            // upwards_2
            // 
            upwards_2.Enabled = true;
            upwards_2.Name = "upwards_2";
            upwards_2.ResultSet = 2;
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

        private SqlDatabaseTestActions BroadcastProhibition_StructureData;
        private SqlDatabaseTestActions BroadcastProhibition_StructureUpwardsData;
    }
}
