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
    public class UserDefaultTransportViewTests : SqlDatabaseTestClass
    {

        public UserDefaultTransportViewTests() {
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
        public void UserDefaultTransportTypeView_DefNotNull() {
            SqlDatabaseTestActions testActions = this.UserDefaultTransportTypeView_DefNotNullData;
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
        public void UserDefaultTransportTypeView_DefNullAndHasFLChat() {
            SqlDatabaseTestActions testActions = this.UserDefaultTransportTypeView_DefNullAndHasFLChatData;
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
        public void UserDefaultTransportTypeView_DefNullAndDisabledFLChat() {
            SqlDatabaseTestActions testActions = this.UserDefaultTransportTypeView_DefNullAndDisabledFLChatData;
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
        public void UserDefaultTransportTypeView_DefIsDisabled() {
            SqlDatabaseTestActions testActions = this.UserDefaultTransportTypeView_DefIsDisabledData;
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
        public void UserDefaultTransportTypeView_WithoutEnabledTransports() {
            SqlDatabaseTestActions testActions = this.UserDefaultTransportTypeView_WithoutEnabledTransportsData;
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
        public void UserDefaultTransportTypeView_DisabledUser() {
            SqlDatabaseTestActions testActions = this.UserDefaultTransportTypeView_DisabledUserData;
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
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction UserDefaultTransportTypeView_DefNotNull_TestAction;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserDefaultTransportViewTests));
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition RowCount1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction UserDefaultTransportTypeView_DefNullAndHasFLChat_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition RowCountFLChat;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition TransportFLCHat;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction UserDefaultTransportTypeView_DefNullAndDisabledFLChat_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition RowCountFLChatDisabled;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction UserDefaultTransportTypeView_DefIsDisabled_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition RowCountDefDisabled;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction UserDefaultTransportTypeView_WithoutEnabledTransports_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition UserIsExists;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition emptyResultSetCondition1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction UserDefaultTransportTypeView_DisabledUser_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition RowCountDisabledUser;
            this.UserDefaultTransportTypeView_DefNotNullData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.UserDefaultTransportTypeView_DefNullAndHasFLChatData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.UserDefaultTransportTypeView_DefNullAndDisabledFLChatData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.UserDefaultTransportTypeView_DefIsDisabledData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.UserDefaultTransportTypeView_WithoutEnabledTransportsData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.UserDefaultTransportTypeView_DisabledUserData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            UserDefaultTransportTypeView_DefNotNull_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            RowCount1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            UserDefaultTransportTypeView_DefNullAndHasFLChat_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            RowCountFLChat = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            TransportFLCHat = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            UserDefaultTransportTypeView_DefNullAndDisabledFLChat_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            RowCountFLChatDisabled = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            UserDefaultTransportTypeView_DefIsDisabled_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            RowCountDefDisabled = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            UserDefaultTransportTypeView_WithoutEnabledTransports_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            UserIsExists = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            emptyResultSetCondition1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            UserDefaultTransportTypeView_DisabledUser_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            RowCountDisabledUser = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            // 
            // UserDefaultTransportTypeView_DefNotNull_TestAction
            // 
            UserDefaultTransportTypeView_DefNotNull_TestAction.Conditions.Add(RowCount1);
            resources.ApplyResources(UserDefaultTransportTypeView_DefNotNull_TestAction, "UserDefaultTransportTypeView_DefNotNull_TestAction");
            // 
            // RowCount1
            // 
            RowCount1.Enabled = true;
            RowCount1.Name = "RowCount1";
            RowCount1.ResultSet = 1;
            RowCount1.RowCount = 1;
            // 
            // UserDefaultTransportTypeView_DefNullAndHasFLChat_TestAction
            // 
            UserDefaultTransportTypeView_DefNullAndHasFLChat_TestAction.Conditions.Add(RowCountFLChat);
            UserDefaultTransportTypeView_DefNullAndHasFLChat_TestAction.Conditions.Add(TransportFLCHat);
            resources.ApplyResources(UserDefaultTransportTypeView_DefNullAndHasFLChat_TestAction, "UserDefaultTransportTypeView_DefNullAndHasFLChat_TestAction");
            // 
            // RowCountFLChat
            // 
            RowCountFLChat.Enabled = true;
            RowCountFLChat.Name = "RowCountFLChat";
            RowCountFLChat.ResultSet = 1;
            RowCountFLChat.RowCount = 1;
            // 
            // TransportFLCHat
            // 
            TransportFLCHat.ColumnNumber = 2;
            TransportFLCHat.Enabled = true;
            TransportFLCHat.ExpectedValue = "0";
            TransportFLCHat.Name = "TransportFLCHat";
            TransportFLCHat.NullExpected = false;
            TransportFLCHat.ResultSet = 1;
            TransportFLCHat.RowNumber = 1;
            // 
            // UserDefaultTransportTypeView_DefNullAndDisabledFLChat_TestAction
            // 
            UserDefaultTransportTypeView_DefNullAndDisabledFLChat_TestAction.Conditions.Add(RowCountFLChatDisabled);
            resources.ApplyResources(UserDefaultTransportTypeView_DefNullAndDisabledFLChat_TestAction, "UserDefaultTransportTypeView_DefNullAndDisabledFLChat_TestAction");
            // 
            // RowCountFLChatDisabled
            // 
            RowCountFLChatDisabled.Enabled = true;
            RowCountFLChatDisabled.Name = "RowCountFLChatDisabled";
            RowCountFLChatDisabled.ResultSet = 1;
            RowCountFLChatDisabled.RowCount = 1;
            // 
            // UserDefaultTransportTypeView_DefIsDisabled_TestAction
            // 
            UserDefaultTransportTypeView_DefIsDisabled_TestAction.Conditions.Add(RowCountDefDisabled);
            resources.ApplyResources(UserDefaultTransportTypeView_DefIsDisabled_TestAction, "UserDefaultTransportTypeView_DefIsDisabled_TestAction");
            // 
            // RowCountDefDisabled
            // 
            RowCountDefDisabled.Enabled = true;
            RowCountDefDisabled.Name = "RowCountDefDisabled";
            RowCountDefDisabled.ResultSet = 1;
            RowCountDefDisabled.RowCount = 1;
            // 
            // UserDefaultTransportTypeView_WithoutEnabledTransports_TestAction
            // 
            UserDefaultTransportTypeView_WithoutEnabledTransports_TestAction.Conditions.Add(UserIsExists);
            UserDefaultTransportTypeView_WithoutEnabledTransports_TestAction.Conditions.Add(emptyResultSetCondition1);
            resources.ApplyResources(UserDefaultTransportTypeView_WithoutEnabledTransports_TestAction, "UserDefaultTransportTypeView_WithoutEnabledTransports_TestAction");
            // 
            // UserIsExists
            // 
            UserIsExists.Enabled = true;
            UserIsExists.Name = "UserIsExists";
            UserIsExists.ResultSet = 1;
            UserIsExists.RowCount = 1;
            // 
            // emptyResultSetCondition1
            // 
            emptyResultSetCondition1.Enabled = true;
            emptyResultSetCondition1.Name = "emptyResultSetCondition1";
            emptyResultSetCondition1.ResultSet = 2;
            // 
            // UserDefaultTransportTypeView_DisabledUser_TestAction
            // 
            UserDefaultTransportTypeView_DisabledUser_TestAction.Conditions.Add(RowCountDisabledUser);
            resources.ApplyResources(UserDefaultTransportTypeView_DisabledUser_TestAction, "UserDefaultTransportTypeView_DisabledUser_TestAction");
            // 
            // RowCountDisabledUser
            // 
            RowCountDisabledUser.Enabled = true;
            RowCountDisabledUser.Name = "RowCountDisabledUser";
            RowCountDisabledUser.ResultSet = 1;
            RowCountDisabledUser.RowCount = 0;
            // 
            // UserDefaultTransportTypeView_DefNotNullData
            // 
            this.UserDefaultTransportTypeView_DefNotNullData.PosttestAction = null;
            this.UserDefaultTransportTypeView_DefNotNullData.PretestAction = null;
            this.UserDefaultTransportTypeView_DefNotNullData.TestAction = UserDefaultTransportTypeView_DefNotNull_TestAction;
            // 
            // UserDefaultTransportTypeView_DefNullAndHasFLChatData
            // 
            this.UserDefaultTransportTypeView_DefNullAndHasFLChatData.PosttestAction = null;
            this.UserDefaultTransportTypeView_DefNullAndHasFLChatData.PretestAction = null;
            this.UserDefaultTransportTypeView_DefNullAndHasFLChatData.TestAction = UserDefaultTransportTypeView_DefNullAndHasFLChat_TestAction;
            // 
            // UserDefaultTransportTypeView_DefNullAndDisabledFLChatData
            // 
            this.UserDefaultTransportTypeView_DefNullAndDisabledFLChatData.PosttestAction = null;
            this.UserDefaultTransportTypeView_DefNullAndDisabledFLChatData.PretestAction = null;
            this.UserDefaultTransportTypeView_DefNullAndDisabledFLChatData.TestAction = UserDefaultTransportTypeView_DefNullAndDisabledFLChat_TestAction;
            // 
            // UserDefaultTransportTypeView_DefIsDisabledData
            // 
            this.UserDefaultTransportTypeView_DefIsDisabledData.PosttestAction = null;
            this.UserDefaultTransportTypeView_DefIsDisabledData.PretestAction = null;
            this.UserDefaultTransportTypeView_DefIsDisabledData.TestAction = UserDefaultTransportTypeView_DefIsDisabled_TestAction;
            // 
            // UserDefaultTransportTypeView_WithoutEnabledTransportsData
            // 
            this.UserDefaultTransportTypeView_WithoutEnabledTransportsData.PosttestAction = null;
            this.UserDefaultTransportTypeView_WithoutEnabledTransportsData.PretestAction = null;
            this.UserDefaultTransportTypeView_WithoutEnabledTransportsData.TestAction = UserDefaultTransportTypeView_WithoutEnabledTransports_TestAction;
            // 
            // UserDefaultTransportTypeView_DisabledUserData
            // 
            this.UserDefaultTransportTypeView_DisabledUserData.PosttestAction = null;
            this.UserDefaultTransportTypeView_DisabledUserData.PretestAction = null;
            this.UserDefaultTransportTypeView_DisabledUserData.TestAction = UserDefaultTransportTypeView_DisabledUser_TestAction;
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

        private SqlDatabaseTestActions UserDefaultTransportTypeView_DefNotNullData;
        private SqlDatabaseTestActions UserDefaultTransportTypeView_DefNullAndHasFLChatData;
        private SqlDatabaseTestActions UserDefaultTransportTypeView_DefNullAndDisabledFLChatData;
        private SqlDatabaseTestActions UserDefaultTransportTypeView_DefIsDisabledData;
        private SqlDatabaseTestActions UserDefaultTransportTypeView_WithoutEnabledTransportsData;
        private SqlDatabaseTestActions UserDefaultTransportTypeView_DisabledUserData;
    }
}
