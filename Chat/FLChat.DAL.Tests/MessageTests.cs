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
    public class MessageTests : SqlDatabaseTestClass
    {

        public MessageTests() {
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
        public void Message_InsertToFLChatUser() {
            SqlDatabaseTestActions testActions = this.Message_InsertToFLChatUserData;
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
        public void Message_InsertMany() {
            SqlDatabaseTestActions testActions = this.Message_InsertManyData;
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
        [TestMethod()]
        public void MessageToUser_OnUpdate_ProduceEvents() {
            SqlDatabaseTestActions testActions = this.MessageToUser_OnUpdate_ProduceEventsData;
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
        [TestMethod()]
        public void MessageToUser_OnUpdate_ProduceEvents_PreventEarlyEvents() {
            SqlDatabaseTestActions testActions = this.MessageToUser_OnUpdate_ProduceEvents_PreventEarlyEventsData;
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
        [TestMethod()]
        public void MessageToUser_OnUpdate_ProduceEvents_OnDeletedEvents() {
            SqlDatabaseTestActions testActions = this.MessageToUser_OnUpdate_ProduceEvents_OnDeletedEventsData;
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
        [TestMethod()]
        public void MessageToUser_OnUpdate_PreventRollbackFlags() {
            SqlDatabaseTestActions testActions = this.MessageToUser_OnUpdate_PreventRollbackFlagsData;
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
        [TestMethod()]
        public void Message__MessageToUser_OnInsert_WebChat() {
            SqlDatabaseTestActions testActions = this.Message__MessageToUser_OnInsert_WebChatData;
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
        [TestMethod()]
        public void Message_OnInsert_LastUsedTransport() {
            SqlDatabaseTestActions testActions = this.Message_OnInsert_LastUsedTransportData;
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
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction Message_InsertToFLChatUser_PretestAction;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageTests));
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction Message_InsertToFLChatUser_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition RowCount;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition EventType;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition TotalRowCount;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition CausedByTransportCheck;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction Message_InsertMany_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition CountOfEventType;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition CheckEventType;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition CheckEventTransport;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition TotalEventCount;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction Message_InsertMany_PretestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition RowCount1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction MessageToUser_OnUpdate_ProduceEvents_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition MsgUpdateCheck1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition MsgUpdateCheck2;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition MsgUpdateCheck3;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition MsgUpdateCheck4;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition MsgUpdateCheck5;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition MsgUpdateCheck6;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition MsgUpdateCheck7;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition MsgUpdateCheck8;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition MsgUpdateCheck9;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction MessageToUser_OnUpdate_ProduceEvents_PreventEarlyEvents_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition EarlyEvent_Read;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition EarlyEvent_Deliver;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction MessageToUser_OnUpdate_ProduceEvents_OnDeletedEvents_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition EventCountForDeleted;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction MessageToUser_OnUpdate_PreventRollbackFlags_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition scalarValueCondition1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition scalarValueCondition2;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition scalarValueCondition3;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition scalarValueCondition4;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction Message__MessageToUser_OnInsert_WebChat_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition TotalCountOfMsg;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction Message_OnInsert_LastUsedTransport_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition LastUsedTransport_Empty1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition LastUsedTransport_Value1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition LastUsedTransport_Value2;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition TotalCountOfWebChat;
            this.Message_InsertToFLChatUserData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.Message_InsertManyData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.MessageToUser_OnUpdate_ProduceEventsData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.MessageToUser_OnUpdate_ProduceEvents_PreventEarlyEventsData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.MessageToUser_OnUpdate_ProduceEvents_OnDeletedEventsData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.MessageToUser_OnUpdate_PreventRollbackFlagsData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.Message__MessageToUser_OnInsert_WebChatData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.Message_OnInsert_LastUsedTransportData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            Message_InsertToFLChatUser_PretestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            Message_InsertToFLChatUser_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            RowCount = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            EventType = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            TotalRowCount = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            CausedByTransportCheck = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Message_InsertMany_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            CountOfEventType = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            CheckEventType = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            CheckEventTransport = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            TotalEventCount = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            Message_InsertMany_PretestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            RowCount1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            MessageToUser_OnUpdate_ProduceEvents_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            MsgUpdateCheck1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            MsgUpdateCheck2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            MsgUpdateCheck3 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            MsgUpdateCheck4 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            MsgUpdateCheck5 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            MsgUpdateCheck6 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            MsgUpdateCheck7 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            MsgUpdateCheck8 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            MsgUpdateCheck9 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            MessageToUser_OnUpdate_ProduceEvents_PreventEarlyEvents_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            EarlyEvent_Read = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            EarlyEvent_Deliver = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            MessageToUser_OnUpdate_ProduceEvents_OnDeletedEvents_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            EventCountForDeleted = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            MessageToUser_OnUpdate_PreventRollbackFlags_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            scalarValueCondition1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            scalarValueCondition2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            scalarValueCondition3 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            scalarValueCondition4 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Message__MessageToUser_OnInsert_WebChat_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            TotalCountOfMsg = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            Message_OnInsert_LastUsedTransport_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            LastUsedTransport_Empty1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            LastUsedTransport_Value1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            LastUsedTransport_Value2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            TotalCountOfWebChat = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            // 
            // Message_InsertToFLChatUser_PretestAction
            // 
            resources.ApplyResources(Message_InsertToFLChatUser_PretestAction, "Message_InsertToFLChatUser_PretestAction");
            // 
            // Message_InsertToFLChatUser_TestAction
            // 
            Message_InsertToFLChatUser_TestAction.Conditions.Add(RowCount);
            Message_InsertToFLChatUser_TestAction.Conditions.Add(EventType);
            Message_InsertToFLChatUser_TestAction.Conditions.Add(TotalRowCount);
            Message_InsertToFLChatUser_TestAction.Conditions.Add(CausedByTransportCheck);
            resources.ApplyResources(Message_InsertToFLChatUser_TestAction, "Message_InsertToFLChatUser_TestAction");
            // 
            // RowCount
            // 
            RowCount.Enabled = true;
            RowCount.Name = "RowCount";
            RowCount.ResultSet = 1;
            RowCount.RowCount = 1;
            // 
            // EventType
            // 
            EventType.ColumnNumber = 1;
            EventType.Enabled = true;
            EventType.ExpectedValue = "10";
            EventType.Name = "EventType";
            EventType.NullExpected = false;
            EventType.ResultSet = 1;
            EventType.RowNumber = 1;
            // 
            // TotalRowCount
            // 
            TotalRowCount.Enabled = true;
            TotalRowCount.Name = "TotalRowCount";
            TotalRowCount.ResultSet = 2;
            TotalRowCount.RowCount = 1;
            // 
            // CausedByTransportCheck
            // 
            CausedByTransportCheck.ColumnNumber = 2;
            CausedByTransportCheck.Enabled = true;
            CausedByTransportCheck.ExpectedValue = "0";
            CausedByTransportCheck.Name = "CausedByTransportCheck";
            CausedByTransportCheck.NullExpected = false;
            CausedByTransportCheck.ResultSet = 1;
            CausedByTransportCheck.RowNumber = 1;
            // 
            // Message_InsertMany_TestAction
            // 
            Message_InsertMany_TestAction.Conditions.Add(CountOfEventType);
            Message_InsertMany_TestAction.Conditions.Add(CheckEventType);
            Message_InsertMany_TestAction.Conditions.Add(CheckEventTransport);
            Message_InsertMany_TestAction.Conditions.Add(TotalEventCount);
            resources.ApplyResources(Message_InsertMany_TestAction, "Message_InsertMany_TestAction");
            // 
            // CountOfEventType
            // 
            CountOfEventType.Enabled = true;
            CountOfEventType.Name = "CountOfEventType";
            CountOfEventType.ResultSet = 1;
            CountOfEventType.RowCount = 1;
            // 
            // CheckEventType
            // 
            CheckEventType.ColumnNumber = 1;
            CheckEventType.Enabled = true;
            CheckEventType.ExpectedValue = "10";
            CheckEventType.Name = "CheckEventType";
            CheckEventType.NullExpected = false;
            CheckEventType.ResultSet = 1;
            CheckEventType.RowNumber = 1;
            // 
            // CheckEventTransport
            // 
            CheckEventTransport.ColumnNumber = 2;
            CheckEventTransport.Enabled = true;
            CheckEventTransport.ExpectedValue = "0";
            CheckEventTransport.Name = "CheckEventTransport";
            CheckEventTransport.NullExpected = false;
            CheckEventTransport.ResultSet = 1;
            CheckEventTransport.RowNumber = 1;
            // 
            // TotalEventCount
            // 
            TotalEventCount.Enabled = true;
            TotalEventCount.Name = "TotalEventCount";
            TotalEventCount.ResultSet = 2;
            TotalEventCount.RowCount = 2;
            // 
            // Message_InsertMany_PretestAction
            // 
            Message_InsertMany_PretestAction.Conditions.Add(RowCount1);
            resources.ApplyResources(Message_InsertMany_PretestAction, "Message_InsertMany_PretestAction");
            // 
            // RowCount1
            // 
            RowCount1.Enabled = true;
            RowCount1.Name = "RowCount1";
            RowCount1.ResultSet = 1;
            RowCount1.RowCount = 5;
            // 
            // MessageToUser_OnUpdate_ProduceEvents_TestAction
            // 
            MessageToUser_OnUpdate_ProduceEvents_TestAction.Conditions.Add(MsgUpdateCheck1);
            MessageToUser_OnUpdate_ProduceEvents_TestAction.Conditions.Add(MsgUpdateCheck2);
            MessageToUser_OnUpdate_ProduceEvents_TestAction.Conditions.Add(MsgUpdateCheck3);
            MessageToUser_OnUpdate_ProduceEvents_TestAction.Conditions.Add(MsgUpdateCheck4);
            MessageToUser_OnUpdate_ProduceEvents_TestAction.Conditions.Add(MsgUpdateCheck5);
            MessageToUser_OnUpdate_ProduceEvents_TestAction.Conditions.Add(MsgUpdateCheck6);
            MessageToUser_OnUpdate_ProduceEvents_TestAction.Conditions.Add(MsgUpdateCheck7);
            MessageToUser_OnUpdate_ProduceEvents_TestAction.Conditions.Add(MsgUpdateCheck8);
            MessageToUser_OnUpdate_ProduceEvents_TestAction.Conditions.Add(MsgUpdateCheck9);
            resources.ApplyResources(MessageToUser_OnUpdate_ProduceEvents_TestAction, "MessageToUser_OnUpdate_ProduceEvents_TestAction");
            // 
            // MsgUpdateCheck1
            // 
            MsgUpdateCheck1.Enabled = true;
            MsgUpdateCheck1.Name = "MsgUpdateCheck1";
            MsgUpdateCheck1.ResultSet = 1;
            // 
            // MsgUpdateCheck2
            // 
            MsgUpdateCheck2.Enabled = true;
            MsgUpdateCheck2.Name = "MsgUpdateCheck2";
            MsgUpdateCheck2.ResultSet = 2;
            MsgUpdateCheck2.RowCount = 1;
            // 
            // MsgUpdateCheck3
            // 
            MsgUpdateCheck3.ColumnNumber = 1;
            MsgUpdateCheck3.Enabled = true;
            MsgUpdateCheck3.ExpectedValue = "1";
            MsgUpdateCheck3.Name = "MsgUpdateCheck3";
            MsgUpdateCheck3.NullExpected = false;
            MsgUpdateCheck3.ResultSet = 3;
            MsgUpdateCheck3.RowNumber = 1;
            // 
            // MsgUpdateCheck4
            // 
            MsgUpdateCheck4.Enabled = true;
            MsgUpdateCheck4.Name = "MsgUpdateCheck4";
            MsgUpdateCheck4.ResultSet = 4;
            MsgUpdateCheck4.RowCount = 2;
            // 
            // MsgUpdateCheck5
            // 
            MsgUpdateCheck5.ColumnNumber = 1;
            MsgUpdateCheck5.Enabled = true;
            MsgUpdateCheck5.ExpectedValue = "2";
            MsgUpdateCheck5.Name = "MsgUpdateCheck5";
            MsgUpdateCheck5.NullExpected = false;
            MsgUpdateCheck5.ResultSet = 5;
            MsgUpdateCheck5.RowNumber = 1;
            // 
            // MsgUpdateCheck6
            // 
            MsgUpdateCheck6.Enabled = true;
            MsgUpdateCheck6.Name = "MsgUpdateCheck6";
            MsgUpdateCheck6.ResultSet = 6;
            MsgUpdateCheck6.RowCount = 3;
            // 
            // MsgUpdateCheck7
            // 
            MsgUpdateCheck7.ColumnNumber = 1;
            MsgUpdateCheck7.Enabled = true;
            MsgUpdateCheck7.ExpectedValue = "3";
            MsgUpdateCheck7.Name = "MsgUpdateCheck7";
            MsgUpdateCheck7.NullExpected = false;
            MsgUpdateCheck7.ResultSet = 7;
            MsgUpdateCheck7.RowNumber = 1;
            // 
            // MsgUpdateCheck8
            // 
            MsgUpdateCheck8.Enabled = true;
            MsgUpdateCheck8.Name = "MsgUpdateCheck8";
            MsgUpdateCheck8.ResultSet = 8;
            MsgUpdateCheck8.RowCount = 4;
            // 
            // MsgUpdateCheck9
            // 
            MsgUpdateCheck9.ColumnNumber = 1;
            MsgUpdateCheck9.Enabled = true;
            MsgUpdateCheck9.ExpectedValue = "5";
            MsgUpdateCheck9.Name = "MsgUpdateCheck9";
            MsgUpdateCheck9.NullExpected = false;
            MsgUpdateCheck9.ResultSet = 9;
            MsgUpdateCheck9.RowNumber = 1;
            // 
            // MessageToUser_OnUpdate_ProduceEvents_PreventEarlyEvents_TestAction
            // 
            MessageToUser_OnUpdate_ProduceEvents_PreventEarlyEvents_TestAction.Conditions.Add(EarlyEvent_Read);
            MessageToUser_OnUpdate_ProduceEvents_PreventEarlyEvents_TestAction.Conditions.Add(EarlyEvent_Deliver);
            resources.ApplyResources(MessageToUser_OnUpdate_ProduceEvents_PreventEarlyEvents_TestAction, "MessageToUser_OnUpdate_ProduceEvents_PreventEarlyEvents_TestAction");
            // 
            // EarlyEvent_Read
            // 
            EarlyEvent_Read.Enabled = true;
            EarlyEvent_Read.Name = "EarlyEvent_Read";
            EarlyEvent_Read.ResultSet = 1;
            EarlyEvent_Read.RowCount = 1;
            // 
            // EarlyEvent_Deliver
            // 
            EarlyEvent_Deliver.Enabled = true;
            EarlyEvent_Deliver.Name = "EarlyEvent_Deliver";
            EarlyEvent_Deliver.ResultSet = 2;
            EarlyEvent_Deliver.RowCount = 0;
            // 
            // MessageToUser_OnUpdate_ProduceEvents_OnDeletedEvents_TestAction
            // 
            MessageToUser_OnUpdate_ProduceEvents_OnDeletedEvents_TestAction.Conditions.Add(EventCountForDeleted);
            resources.ApplyResources(MessageToUser_OnUpdate_ProduceEvents_OnDeletedEvents_TestAction, "MessageToUser_OnUpdate_ProduceEvents_OnDeletedEvents_TestAction");
            // 
            // EventCountForDeleted
            // 
            EventCountForDeleted.Enabled = true;
            EventCountForDeleted.Name = "EventCountForDeleted";
            EventCountForDeleted.ResultSet = 1;
            EventCountForDeleted.RowCount = 0;
            // 
            // MessageToUser_OnUpdate_PreventRollbackFlags_TestAction
            // 
            MessageToUser_OnUpdate_PreventRollbackFlags_TestAction.Conditions.Add(scalarValueCondition1);
            MessageToUser_OnUpdate_PreventRollbackFlags_TestAction.Conditions.Add(scalarValueCondition2);
            MessageToUser_OnUpdate_PreventRollbackFlags_TestAction.Conditions.Add(scalarValueCondition3);
            MessageToUser_OnUpdate_PreventRollbackFlags_TestAction.Conditions.Add(scalarValueCondition4);
            resources.ApplyResources(MessageToUser_OnUpdate_PreventRollbackFlags_TestAction, "MessageToUser_OnUpdate_PreventRollbackFlags_TestAction");
            // 
            // scalarValueCondition1
            // 
            scalarValueCondition1.ColumnNumber = 1;
            scalarValueCondition1.Enabled = true;
            scalarValueCondition1.ExpectedValue = "51001";
            scalarValueCondition1.Name = "scalarValueCondition1";
            scalarValueCondition1.NullExpected = false;
            scalarValueCondition1.ResultSet = 1;
            scalarValueCondition1.RowNumber = 1;
            // 
            // scalarValueCondition2
            // 
            scalarValueCondition2.ColumnNumber = 1;
            scalarValueCondition2.Enabled = true;
            scalarValueCondition2.ExpectedValue = "51001";
            scalarValueCondition2.Name = "scalarValueCondition2";
            scalarValueCondition2.NullExpected = false;
            scalarValueCondition2.ResultSet = 2;
            scalarValueCondition2.RowNumber = 1;
            // 
            // scalarValueCondition3
            // 
            scalarValueCondition3.ColumnNumber = 1;
            scalarValueCondition3.Enabled = true;
            scalarValueCondition3.ExpectedValue = "51001";
            scalarValueCondition3.Name = "scalarValueCondition3";
            scalarValueCondition3.NullExpected = false;
            scalarValueCondition3.ResultSet = 3;
            scalarValueCondition3.RowNumber = 1;
            // 
            // scalarValueCondition4
            // 
            scalarValueCondition4.ColumnNumber = 1;
            scalarValueCondition4.Enabled = true;
            scalarValueCondition4.ExpectedValue = "51001";
            scalarValueCondition4.Name = "scalarValueCondition4";
            scalarValueCondition4.NullExpected = false;
            scalarValueCondition4.ResultSet = 4;
            scalarValueCondition4.RowNumber = 1;
            // 
            // Message__MessageToUser_OnInsert_WebChat_TestAction
            // 
            Message__MessageToUser_OnInsert_WebChat_TestAction.Conditions.Add(TotalCountOfMsg);
            Message__MessageToUser_OnInsert_WebChat_TestAction.Conditions.Add(TotalCountOfWebChat);
            resources.ApplyResources(Message__MessageToUser_OnInsert_WebChat_TestAction, "Message__MessageToUser_OnInsert_WebChat_TestAction");
            // 
            // TotalCountOfMsg
            // 
            TotalCountOfMsg.Enabled = true;
            TotalCountOfMsg.Name = "TotalCountOfMsg";
            TotalCountOfMsg.ResultSet = 2;
            TotalCountOfMsg.RowCount = 1;
            // 
            // Message_OnInsert_LastUsedTransport_TestAction
            // 
            Message_OnInsert_LastUsedTransport_TestAction.Conditions.Add(LastUsedTransport_Empty1);
            Message_OnInsert_LastUsedTransport_TestAction.Conditions.Add(LastUsedTransport_Value1);
            Message_OnInsert_LastUsedTransport_TestAction.Conditions.Add(LastUsedTransport_Value2);
            resources.ApplyResources(Message_OnInsert_LastUsedTransport_TestAction, "Message_OnInsert_LastUsedTransport_TestAction");
            // 
            // LastUsedTransport_Empty1
            // 
            LastUsedTransport_Empty1.Enabled = true;
            LastUsedTransport_Empty1.Name = "LastUsedTransport_Empty1";
            LastUsedTransport_Empty1.ResultSet = 1;
            // 
            // LastUsedTransport_Value1
            // 
            LastUsedTransport_Value1.ColumnNumber = 1;
            LastUsedTransport_Value1.Enabled = true;
            LastUsedTransport_Value1.ExpectedValue = "1";
            LastUsedTransport_Value1.Name = "LastUsedTransport_Value1";
            LastUsedTransport_Value1.NullExpected = false;
            LastUsedTransport_Value1.ResultSet = 2;
            LastUsedTransport_Value1.RowNumber = 1;
            // 
            // LastUsedTransport_Value2
            // 
            LastUsedTransport_Value2.ColumnNumber = 1;
            LastUsedTransport_Value2.Enabled = true;
            LastUsedTransport_Value2.ExpectedValue = "2";
            LastUsedTransport_Value2.Name = "LastUsedTransport_Value2";
            LastUsedTransport_Value2.NullExpected = false;
            LastUsedTransport_Value2.ResultSet = 3;
            LastUsedTransport_Value2.RowNumber = 1;
            // 
            // Message_InsertToFLChatUserData
            // 
            this.Message_InsertToFLChatUserData.PosttestAction = null;
            this.Message_InsertToFLChatUserData.PretestAction = Message_InsertToFLChatUser_PretestAction;
            this.Message_InsertToFLChatUserData.TestAction = Message_InsertToFLChatUser_TestAction;
            // 
            // Message_InsertManyData
            // 
            this.Message_InsertManyData.PosttestAction = null;
            this.Message_InsertManyData.PretestAction = Message_InsertMany_PretestAction;
            this.Message_InsertManyData.TestAction = Message_InsertMany_TestAction;
            // 
            // MessageToUser_OnUpdate_ProduceEventsData
            // 
            this.MessageToUser_OnUpdate_ProduceEventsData.PosttestAction = null;
            this.MessageToUser_OnUpdate_ProduceEventsData.PretestAction = null;
            this.MessageToUser_OnUpdate_ProduceEventsData.TestAction = MessageToUser_OnUpdate_ProduceEvents_TestAction;
            // 
            // MessageToUser_OnUpdate_ProduceEvents_PreventEarlyEventsData
            // 
            this.MessageToUser_OnUpdate_ProduceEvents_PreventEarlyEventsData.PosttestAction = null;
            this.MessageToUser_OnUpdate_ProduceEvents_PreventEarlyEventsData.PretestAction = null;
            this.MessageToUser_OnUpdate_ProduceEvents_PreventEarlyEventsData.TestAction = MessageToUser_OnUpdate_ProduceEvents_PreventEarlyEvents_TestAction;
            // 
            // MessageToUser_OnUpdate_ProduceEvents_OnDeletedEventsData
            // 
            this.MessageToUser_OnUpdate_ProduceEvents_OnDeletedEventsData.PosttestAction = null;
            this.MessageToUser_OnUpdate_ProduceEvents_OnDeletedEventsData.PretestAction = null;
            this.MessageToUser_OnUpdate_ProduceEvents_OnDeletedEventsData.TestAction = MessageToUser_OnUpdate_ProduceEvents_OnDeletedEvents_TestAction;
            // 
            // MessageToUser_OnUpdate_PreventRollbackFlagsData
            // 
            this.MessageToUser_OnUpdate_PreventRollbackFlagsData.PosttestAction = null;
            this.MessageToUser_OnUpdate_PreventRollbackFlagsData.PretestAction = null;
            this.MessageToUser_OnUpdate_PreventRollbackFlagsData.TestAction = MessageToUser_OnUpdate_PreventRollbackFlags_TestAction;
            // 
            // Message__MessageToUser_OnInsert_WebChatData
            // 
            this.Message__MessageToUser_OnInsert_WebChatData.PosttestAction = null;
            this.Message__MessageToUser_OnInsert_WebChatData.PretestAction = null;
            this.Message__MessageToUser_OnInsert_WebChatData.TestAction = Message__MessageToUser_OnInsert_WebChat_TestAction;
            // 
            // Message_OnInsert_LastUsedTransportData
            // 
            this.Message_OnInsert_LastUsedTransportData.PosttestAction = null;
            this.Message_OnInsert_LastUsedTransportData.PretestAction = null;
            this.Message_OnInsert_LastUsedTransportData.TestAction = Message_OnInsert_LastUsedTransport_TestAction;
            // 
            // TotalCountOfWebChat
            // 
            TotalCountOfWebChat.Enabled = true;
            TotalCountOfWebChat.Name = "TotalCountOfWebChat";
            TotalCountOfWebChat.ResultSet = 1;
            TotalCountOfWebChat.RowCount = 0;
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

        private SqlDatabaseTestActions Message_InsertToFLChatUserData;
        private SqlDatabaseTestActions Message_InsertManyData;
        private SqlDatabaseTestActions MessageToUser_OnUpdate_ProduceEventsData;
        private SqlDatabaseTestActions MessageToUser_OnUpdate_ProduceEvents_PreventEarlyEventsData;
        private SqlDatabaseTestActions MessageToUser_OnUpdate_ProduceEvents_OnDeletedEventsData;
        private SqlDatabaseTestActions MessageToUser_OnUpdate_PreventRollbackFlagsData;
        private SqlDatabaseTestActions Message__MessageToUser_OnInsert_WebChatData;
        private SqlDatabaseTestActions Message_OnInsert_LastUsedTransportData;
    }
}
