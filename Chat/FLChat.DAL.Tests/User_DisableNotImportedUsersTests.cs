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
    public class User_DisableNotImportedUsersTests : SqlDatabaseTestClass
    {

        public User_DisableNotImportedUsersTests() {
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
        public void User_DisableNotImportedUsers() {
            SqlDatabaseTestActions testActions = this.User_DisableNotImportedUsersData;
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
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction User_DisableNotImportedUsers_TestAction;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(User_DisableNotImportedUsersTests));
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition AbsenceCount;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition ActiveUsers;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition DisabledUsers;
            this.User_DisableNotImportedUsersData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            User_DisableNotImportedUsers_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            AbsenceCount = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            ActiveUsers = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            DisabledUsers = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            // 
            // User_DisableNotImportedUsersData
            // 
            this.User_DisableNotImportedUsersData.PosttestAction = null;
            this.User_DisableNotImportedUsersData.PretestAction = null;
            this.User_DisableNotImportedUsersData.TestAction = User_DisableNotImportedUsers_TestAction;
            // 
            // User_DisableNotImportedUsers_TestAction
            // 
            User_DisableNotImportedUsers_TestAction.Conditions.Add(AbsenceCount);
            User_DisableNotImportedUsers_TestAction.Conditions.Add(ActiveUsers);
            User_DisableNotImportedUsers_TestAction.Conditions.Add(DisabledUsers);
            resources.ApplyResources(User_DisableNotImportedUsers_TestAction, "User_DisableNotImportedUsers_TestAction");
            // 
            // AbsenceCount
            // 
            AbsenceCount.ColumnNumber = 1;
            AbsenceCount.Enabled = true;
            AbsenceCount.ExpectedValue = "2";
            AbsenceCount.Name = "AbsenceCount";
            AbsenceCount.NullExpected = false;
            AbsenceCount.ResultSet = 1;
            AbsenceCount.RowNumber = 1;
            // 
            // ActiveUsers
            // 
            ActiveUsers.Enabled = true;
            ActiveUsers.Name = "ActiveUsers";
            ActiveUsers.ResultSet = 2;
            // 
            // DisabledUsers
            // 
            DisabledUsers.Enabled = true;
            DisabledUsers.Name = "DisabledUsers";
            DisabledUsers.ResultSet = 3;
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

        private SqlDatabaseTestActions User_DisableNotImportedUsersData;
    }
}
