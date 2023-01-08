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
    public class AuthTokenTests : SqlDatabaseTestClass
    {

        public AuthTokenTests() {
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
        public void AuthToken_OnInsertNewbe() {
            SqlDatabaseTestActions testActions = this.AuthToken_OnInsertNewbeData;
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
        public void AuthToken_InsertVerification() {
            SqlDatabaseTestActions testActions = this.AuthToken_InsertVerificationData;
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
        public void AuthToken_OnInserNewbe_CheckEnableTransports() {
            SqlDatabaseTestActions testActions = this.AuthToken_OnInserNewbe_CheckEnableTransportsData;
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
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction AuthToken_OnInsertNewbe_TestAction;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuthTokenTests));
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition SignUpDate_BeforeAuth;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Transport_BeforeAuth;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition SignUpDate_AfterAuth;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Transport_AfterAuth;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition SingUp_Third;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Transport_Third;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction AuthToken_InsertVerification_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition WasThrows;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction AuthToken_OnInserNewbe_CheckEnableTransports_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition HasNotEnableTransportsBefore;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition FLChatIsEnable;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition TestIsDisable;
            this.AuthToken_OnInsertNewbeData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.AuthToken_InsertVerificationData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.AuthToken_OnInserNewbe_CheckEnableTransportsData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            AuthToken_OnInsertNewbe_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            SignUpDate_BeforeAuth = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Transport_BeforeAuth = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            SignUpDate_AfterAuth = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Transport_AfterAuth = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            SingUp_Third = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Transport_Third = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            AuthToken_InsertVerification_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            WasThrows = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            AuthToken_OnInserNewbe_CheckEnableTransports_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            HasNotEnableTransportsBefore = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            FLChatIsEnable = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            TestIsDisable = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            // 
            // AuthToken_OnInsertNewbe_TestAction
            // 
            AuthToken_OnInsertNewbe_TestAction.Conditions.Add(SignUpDate_BeforeAuth);
            AuthToken_OnInsertNewbe_TestAction.Conditions.Add(Transport_BeforeAuth);
            AuthToken_OnInsertNewbe_TestAction.Conditions.Add(SignUpDate_AfterAuth);
            AuthToken_OnInsertNewbe_TestAction.Conditions.Add(Transport_AfterAuth);
            AuthToken_OnInsertNewbe_TestAction.Conditions.Add(SingUp_Third);
            AuthToken_OnInsertNewbe_TestAction.Conditions.Add(Transport_Third);
            resources.ApplyResources(AuthToken_OnInsertNewbe_TestAction, "AuthToken_OnInsertNewbe_TestAction");
            // 
            // SignUpDate_BeforeAuth
            // 
            SignUpDate_BeforeAuth.ColumnNumber = 1;
            SignUpDate_BeforeAuth.Enabled = true;
            SignUpDate_BeforeAuth.ExpectedValue = null;
            SignUpDate_BeforeAuth.Name = "SignUpDate_BeforeAuth";
            SignUpDate_BeforeAuth.NullExpected = true;
            SignUpDate_BeforeAuth.ResultSet = 1;
            SignUpDate_BeforeAuth.RowNumber = 1;
            // 
            // Transport_BeforeAuth
            // 
            Transport_BeforeAuth.ColumnNumber = 2;
            Transport_BeforeAuth.Enabled = true;
            Transport_BeforeAuth.ExpectedValue = null;
            Transport_BeforeAuth.Name = "Transport_BeforeAuth";
            Transport_BeforeAuth.NullExpected = true;
            Transport_BeforeAuth.ResultSet = 1;
            Transport_BeforeAuth.RowNumber = 1;
            // 
            // SignUpDate_AfterAuth
            // 
            SignUpDate_AfterAuth.ColumnNumber = 1;
            SignUpDate_AfterAuth.Enabled = true;
            SignUpDate_AfterAuth.ExpectedValue = "0";
            SignUpDate_AfterAuth.Name = "SignUpDate_AfterAuth";
            SignUpDate_AfterAuth.NullExpected = false;
            SignUpDate_AfterAuth.ResultSet = 2;
            SignUpDate_AfterAuth.RowNumber = 1;
            // 
            // Transport_AfterAuth
            // 
            Transport_AfterAuth.ColumnNumber = 2;
            Transport_AfterAuth.Enabled = true;
            Transport_AfterAuth.ExpectedValue = "0";
            Transport_AfterAuth.Name = "Transport_AfterAuth";
            Transport_AfterAuth.NullExpected = false;
            Transport_AfterAuth.ResultSet = 2;
            Transport_AfterAuth.RowNumber = 1;
            // 
            // SingUp_Third
            // 
            SingUp_Third.ColumnNumber = 1;
            SingUp_Third.Enabled = true;
            SingUp_Third.ExpectedValue = "0";
            SingUp_Third.Name = "SingUp_Third";
            SingUp_Third.NullExpected = false;
            SingUp_Third.ResultSet = 3;
            SingUp_Third.RowNumber = 1;
            // 
            // Transport_Third
            // 
            Transport_Third.ColumnNumber = 2;
            Transport_Third.Enabled = true;
            Transport_Third.ExpectedValue = "0";
            Transport_Third.Name = "Transport_Third";
            Transport_Third.NullExpected = false;
            Transport_Third.ResultSet = 3;
            Transport_Third.RowNumber = 1;
            // 
            // AuthToken_InsertVerification_TestAction
            // 
            AuthToken_InsertVerification_TestAction.Conditions.Add(WasThrows);
            resources.ApplyResources(AuthToken_InsertVerification_TestAction, "AuthToken_InsertVerification_TestAction");
            // 
            // WasThrows
            // 
            WasThrows.ColumnNumber = 1;
            WasThrows.Enabled = true;
            WasThrows.ExpectedValue = "1";
            WasThrows.Name = "WasThrows";
            WasThrows.NullExpected = false;
            WasThrows.ResultSet = 1;
            WasThrows.RowNumber = 1;
            // 
            // AuthToken_OnInsertNewbeData
            // 
            this.AuthToken_OnInsertNewbeData.PosttestAction = null;
            this.AuthToken_OnInsertNewbeData.PretestAction = null;
            this.AuthToken_OnInsertNewbeData.TestAction = AuthToken_OnInsertNewbe_TestAction;
            // 
            // AuthToken_InsertVerificationData
            // 
            this.AuthToken_InsertVerificationData.PosttestAction = null;
            this.AuthToken_InsertVerificationData.PretestAction = null;
            this.AuthToken_InsertVerificationData.TestAction = AuthToken_InsertVerification_TestAction;
            // 
            // AuthToken_OnInserNewbe_CheckEnableTransportsData
            // 
            this.AuthToken_OnInserNewbe_CheckEnableTransportsData.PosttestAction = null;
            this.AuthToken_OnInserNewbe_CheckEnableTransportsData.PretestAction = null;
            this.AuthToken_OnInserNewbe_CheckEnableTransportsData.TestAction = AuthToken_OnInserNewbe_CheckEnableTransports_TestAction;
            // 
            // AuthToken_OnInserNewbe_CheckEnableTransports_TestAction
            // 
            AuthToken_OnInserNewbe_CheckEnableTransports_TestAction.Conditions.Add(HasNotEnableTransportsBefore);
            AuthToken_OnInserNewbe_CheckEnableTransports_TestAction.Conditions.Add(FLChatIsEnable);
            AuthToken_OnInserNewbe_CheckEnableTransports_TestAction.Conditions.Add(TestIsDisable);
            resources.ApplyResources(AuthToken_OnInserNewbe_CheckEnableTransports_TestAction, "AuthToken_OnInserNewbe_CheckEnableTransports_TestAction");
            // 
            // HasNotEnableTransportsBefore
            // 
            HasNotEnableTransportsBefore.Enabled = true;
            HasNotEnableTransportsBefore.Name = "HasNotEnableTransportsBefore";
            HasNotEnableTransportsBefore.ResultSet = 1;
            // 
            // FLChatIsEnable
            // 
            FLChatIsEnable.ColumnNumber = 1;
            FLChatIsEnable.Enabled = true;
            FLChatIsEnable.ExpectedValue = "True";
            FLChatIsEnable.Name = "FLChatIsEnable";
            FLChatIsEnable.NullExpected = false;
            FLChatIsEnable.ResultSet = 2;
            FLChatIsEnable.RowNumber = 1;
            // 
            // TestIsDisable
            // 
            TestIsDisable.ColumnNumber = 1;
            TestIsDisable.Enabled = true;
            TestIsDisable.ExpectedValue = "False";
            TestIsDisable.Name = "TestIsDisable";
            TestIsDisable.NullExpected = false;
            TestIsDisable.ResultSet = 3;
            TestIsDisable.RowNumber = 1;
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

        private SqlDatabaseTestActions AuthToken_OnInsertNewbeData;
        private SqlDatabaseTestActions AuthToken_InsertVerificationData;
        private SqlDatabaseTestActions AuthToken_OnInserNewbe_CheckEnableTransportsData;
    }
}
