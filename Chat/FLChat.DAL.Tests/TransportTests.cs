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
    public class TransportTests : SqlDatabaseTestClass
    {

        public TransportTests() {
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
        public void Transport_GreetingMessage() {
            SqlDatabaseTestActions testActions = this.Transport_GreetingMessageData;
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
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction Transport_GreetingMessage_TestAction;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransportTests));
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition greet_empty1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition greet_sent2;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition greet_sent2_sender;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition greet_send2_toTransport;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition greet_disabled3;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition greet_enable4;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition greet5_insert_disabled;
            this.Transport_GreetingMessageData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            Transport_GreetingMessage_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            greet_empty1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            greet_sent2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            greet_sent2_sender = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            greet_send2_toTransport = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            greet_disabled3 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            greet_enable4 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            greet5_insert_disabled = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            // 
            // Transport_GreetingMessageData
            // 
            this.Transport_GreetingMessageData.PosttestAction = null;
            this.Transport_GreetingMessageData.PretestAction = null;
            this.Transport_GreetingMessageData.TestAction = Transport_GreetingMessage_TestAction;
            // 
            // Transport_GreetingMessage_TestAction
            // 
            Transport_GreetingMessage_TestAction.Conditions.Add(greet_empty1);
            Transport_GreetingMessage_TestAction.Conditions.Add(greet_sent2);
            Transport_GreetingMessage_TestAction.Conditions.Add(greet_sent2_sender);
            Transport_GreetingMessage_TestAction.Conditions.Add(greet_send2_toTransport);
            Transport_GreetingMessage_TestAction.Conditions.Add(greet_disabled3);
            Transport_GreetingMessage_TestAction.Conditions.Add(greet_enable4);
            Transport_GreetingMessage_TestAction.Conditions.Add(greet5_insert_disabled);
            resources.ApplyResources(Transport_GreetingMessage_TestAction, "Transport_GreetingMessage_TestAction");
            // 
            // greet_empty1
            // 
            greet_empty1.Enabled = true;
            greet_empty1.Name = "greet_empty1";
            greet_empty1.ResultSet = 1;
            // 
            // greet_sent2
            // 
            greet_sent2.Enabled = true;
            greet_sent2.Name = "greet_sent2";
            greet_sent2.ResultSet = 2;
            greet_sent2.RowCount = 1;
            // 
            // greet_sent2_sender
            // 
            greet_sent2_sender.ColumnNumber = 1;
            greet_sent2_sender.Enabled = true;
            greet_sent2_sender.ExpectedValue = "00000000-0000-0000-0000-000000000000";
            greet_sent2_sender.Name = "greet_sent2_sender";
            greet_sent2_sender.NullExpected = false;
            greet_sent2_sender.ResultSet = 2;
            greet_sent2_sender.RowNumber = 1;
            // 
            // greet_send2_toTransport
            // 
            greet_send2_toTransport.ColumnNumber = 2;
            greet_send2_toTransport.Enabled = true;
            greet_send2_toTransport.ExpectedValue = "-1";
            greet_send2_toTransport.Name = "greet_send2_toTransport";
            greet_send2_toTransport.NullExpected = false;
            greet_send2_toTransport.ResultSet = 2;
            greet_send2_toTransport.RowNumber = 1;
            // 
            // greet_disabled3
            // 
            greet_disabled3.Enabled = true;
            greet_disabled3.Name = "greet_disabled3";
            greet_disabled3.ResultSet = 3;
            greet_disabled3.RowCount = 1;
            // 
            // greet_enable4
            // 
            greet_enable4.Enabled = true;
            greet_enable4.Name = "greet_enable4";
            greet_enable4.ResultSet = 4;
            greet_enable4.RowCount = 2;
            // 
            // greet5_insert_disabled
            // 
            greet5_insert_disabled.Enabled = true;
            greet5_insert_disabled.Name = "greet5_insert_disabled";
            greet5_insert_disabled.ResultSet = 5;
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

        private SqlDatabaseTestActions Transport_GreetingMessageData;
    }
}
