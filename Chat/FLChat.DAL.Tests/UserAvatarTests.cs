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
    public class UserAvatarTests : SqlDatabaseTestClass
    {

        public UserAvatarTests() {
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
        public void UserAvatar_UpdateFieldInUser() {
            SqlDatabaseTestActions testActions = this.UserAvatar_UpdateFieldInUserData;
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
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction UserAvatar_UpdateFieldInUser_TestAction;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserAvatarTests));
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition WithoutAvatar;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.NotEmptyResultSetCondition InsertAvatar;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.NotEmptyResultSetCondition UpdatedAvatar;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition UpdatedDateInUser;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition DeleteAvatar;
            this.UserAvatar_UpdateFieldInUserData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            UserAvatar_UpdateFieldInUser_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            WithoutAvatar = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            InsertAvatar = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.NotEmptyResultSetCondition();
            UpdatedAvatar = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.NotEmptyResultSetCondition();
            UpdatedDateInUser = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            DeleteAvatar = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            // 
            // UserAvatar_UpdateFieldInUserData
            // 
            this.UserAvatar_UpdateFieldInUserData.PosttestAction = null;
            this.UserAvatar_UpdateFieldInUserData.PretestAction = null;
            this.UserAvatar_UpdateFieldInUserData.TestAction = UserAvatar_UpdateFieldInUser_TestAction;
            // 
            // UserAvatar_UpdateFieldInUser_TestAction
            // 
            UserAvatar_UpdateFieldInUser_TestAction.Conditions.Add(WithoutAvatar);
            UserAvatar_UpdateFieldInUser_TestAction.Conditions.Add(InsertAvatar);
            UserAvatar_UpdateFieldInUser_TestAction.Conditions.Add(UpdatedAvatar);
            UserAvatar_UpdateFieldInUser_TestAction.Conditions.Add(UpdatedDateInUser);
            UserAvatar_UpdateFieldInUser_TestAction.Conditions.Add(DeleteAvatar);
            resources.ApplyResources(UserAvatar_UpdateFieldInUser_TestAction, "UserAvatar_UpdateFieldInUser_TestAction");
            // 
            // WithoutAvatar
            // 
            WithoutAvatar.ColumnNumber = 1;
            WithoutAvatar.Enabled = true;
            WithoutAvatar.ExpectedValue = null;
            WithoutAvatar.Name = "WithoutAvatar";
            WithoutAvatar.NullExpected = true;
            WithoutAvatar.ResultSet = 1;
            WithoutAvatar.RowNumber = 1;
            // 
            // InsertAvatar
            // 
            InsertAvatar.Enabled = true;
            InsertAvatar.Name = "InsertAvatar";
            InsertAvatar.ResultSet = 2;
            // 
            // UpdatedAvatar
            // 
            UpdatedAvatar.Enabled = true;
            UpdatedAvatar.Name = "UpdatedAvatar";
            UpdatedAvatar.ResultSet = 3;
            // 
            // UpdatedDateInUser
            // 
            UpdatedDateInUser.ColumnNumber = 2;
            UpdatedDateInUser.Enabled = true;
            UpdatedDateInUser.ExpectedValue = "1";
            UpdatedDateInUser.Name = "UpdatedDateInUser";
            UpdatedDateInUser.NullExpected = false;
            UpdatedDateInUser.ResultSet = 3;
            UpdatedDateInUser.RowNumber = 1;
            // 
            // DeleteAvatar
            // 
            DeleteAvatar.ColumnNumber = 1;
            DeleteAvatar.Enabled = true;
            DeleteAvatar.ExpectedValue = null;
            DeleteAvatar.Name = "DeleteAvatar";
            DeleteAvatar.NullExpected = true;
            DeleteAvatar.ResultSet = 4;
            DeleteAvatar.RowNumber = 1;
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

        private SqlDatabaseTestActions UserAvatar_UpdateFieldInUserData;
    }
}
