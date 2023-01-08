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
    public class MessageToSegmentTests : SqlDatabaseTestClass
    {

        public MessageToSegmentTests() {
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
        public void MessageToSegment_ProduceMessageToUsers() {
            SqlDatabaseTestActions testActions = this.MessageToSegment_ProduceMessageToUsersData;
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
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction MessageToSegment_ProduceMessageToUsers_TestAction;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageToSegmentTests));
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition MessageCount;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition User_r1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition User_r2;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Transport_r1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Transport_r2;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition IsSent_r1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition IsSent_r2;
            this.MessageToSegment_ProduceMessageToUsersData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            MessageToSegment_ProduceMessageToUsers_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            MessageCount = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            User_r1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            User_r2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Transport_r1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Transport_r2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            IsSent_r1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            IsSent_r2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            // 
            // MessageToSegment_ProduceMessageToUsers_TestAction
            // 
            MessageToSegment_ProduceMessageToUsers_TestAction.Conditions.Add(MessageCount);
            MessageToSegment_ProduceMessageToUsers_TestAction.Conditions.Add(User_r1);
            MessageToSegment_ProduceMessageToUsers_TestAction.Conditions.Add(User_r2);
            MessageToSegment_ProduceMessageToUsers_TestAction.Conditions.Add(Transport_r1);
            MessageToSegment_ProduceMessageToUsers_TestAction.Conditions.Add(Transport_r2);
            MessageToSegment_ProduceMessageToUsers_TestAction.Conditions.Add(IsSent_r1);
            MessageToSegment_ProduceMessageToUsers_TestAction.Conditions.Add(IsSent_r2);
            resources.ApplyResources(MessageToSegment_ProduceMessageToUsers_TestAction, "MessageToSegment_ProduceMessageToUsers_TestAction");
            // 
            // MessageCount
            // 
            MessageCount.Enabled = true;
            MessageCount.Name = "MessageCount";
            MessageCount.ResultSet = 3;
            MessageCount.RowCount = 2;
            // 
            // User_r1
            // 
            User_r1.ColumnNumber = 1;
            User_r1.Enabled = true;
            User_r1.ExpectedValue = "u11";
            User_r1.Name = "User_r1";
            User_r1.NullExpected = false;
            User_r1.ResultSet = 3;
            User_r1.RowNumber = 1;
            // 
            // User_r2
            // 
            User_r2.ColumnNumber = 1;
            User_r2.Enabled = true;
            User_r2.ExpectedValue = "u1";
            User_r2.Name = "User_r2";
            User_r2.NullExpected = false;
            User_r2.ResultSet = 3;
            User_r2.RowNumber = 2;
            // 
            // Transport_r1
            // 
            Transport_r1.ColumnNumber = 2;
            Transport_r1.Enabled = true;
            Transport_r1.ExpectedValue = "-1";
            Transport_r1.Name = "Transport_r1";
            Transport_r1.NullExpected = false;
            Transport_r1.ResultSet = 3;
            Transport_r1.RowNumber = 1;
            // 
            // Transport_r2
            // 
            Transport_r2.ColumnNumber = 2;
            Transport_r2.Enabled = true;
            Transport_r2.ExpectedValue = "0";
            Transport_r2.Name = "Transport_r2";
            Transport_r2.NullExpected = false;
            Transport_r2.ResultSet = 3;
            Transport_r2.RowNumber = 2;
            // 
            // IsSent_r1
            // 
            IsSent_r1.ColumnNumber = 3;
            IsSent_r1.Enabled = true;
            IsSent_r1.ExpectedValue = "False";
            IsSent_r1.Name = "IsSent_r1";
            IsSent_r1.NullExpected = false;
            IsSent_r1.ResultSet = 3;
            IsSent_r1.RowNumber = 1;
            // 
            // IsSent_r2
            // 
            IsSent_r2.ColumnNumber = 3;
            IsSent_r2.Enabled = true;
            IsSent_r2.ExpectedValue = "True";
            IsSent_r2.Name = "IsSent_r2";
            IsSent_r2.NullExpected = false;
            IsSent_r2.ResultSet = 3;
            IsSent_r2.RowNumber = 2;
            // 
            // MessageToSegment_ProduceMessageToUsersData
            // 
            this.MessageToSegment_ProduceMessageToUsersData.PosttestAction = null;
            this.MessageToSegment_ProduceMessageToUsersData.PretestAction = null;
            this.MessageToSegment_ProduceMessageToUsersData.TestAction = MessageToSegment_ProduceMessageToUsers_TestAction;
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

        private SqlDatabaseTestActions MessageToSegment_ProduceMessageToUsersData;
    }
}
