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
    public class UserTests : SqlDatabaseTestClass
    {

        public UserTests() {
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
        public void User_OnDisable() {
            SqlDatabaseTestActions testActions = this.User_OnDisableData;
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
        public void User_GetChilds() {
            SqlDatabaseTestActions testActions = this.User_GetChildsData;
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
        public void Trigger_User_ManageSmsTransport() {
            SqlDatabaseTestActions testActions = this.Trigger_User_ManageSmsTransportData;
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
        public void User_GetParents() {
            SqlDatabaseTestActions testActions = this.User_GetParentsData;
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
        public void User_GetSelection() {
            SqlDatabaseTestActions testActions = this.User_GetSelectionData;
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
        public void User_GetSelection_Segment() {
            SqlDatabaseTestActions testActions = this.User_GetSelection_SegmentData;
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
        public void User_GetChildsMulti() {
            SqlDatabaseTestActions testActions = this.User_GetChildsMultiData;
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
        public void User_GetSelection_w_BroadcastProhibition() {
            SqlDatabaseTestActions testActions = this.User_GetSelection_w_BroadcastProhibitionData;
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
        public void User_GetChildsExt() {
            SqlDatabaseTestActions testActions = this.User_GetChildsExtData;
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
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction User_OnDisable_TestAction;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserTests));
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition CounfOfAuthTokens;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction User_GetChilds_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition ChildCount_woDeepLimit;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition ChildCount_Deep1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition ChildCount_Deep2;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition ChildCount_Deep3;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition CheckDeepValue1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition CheckDeepValue2;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition CheckDeepValue3;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Childs_Owner1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Childs_Owner2;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Childs_Owner3;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction Trigger_User_ManageSmsTransport_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition SmsTransport1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition SmsTransport2;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition SmsTransport3;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction User_GetParents_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition ParentsCount;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition ParentsCount_Limit;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Parents_u1_name;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Parents_u1_deep;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Parents_u2_name;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Parents_u2_deep;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction User_GetSelection_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition User_GetSelection1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition User_GetSelection2;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition User_GetSelection3;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition User_GetSelection4;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition User_GetSelection5;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition User_GetSelection6;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction User_GetSelection_Segment_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition User_GetSelection_Segment1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition User_GetSelection_Segment2;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition User_GetSelection_Segment3;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction User_GetChildsMulti_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition GetChildsMulti1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction User_GetSelection_w_BroadcastProhibition_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition User_GetSelection_w_BroadcastProhibition1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction User_GetChildsExt_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition childExt1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition childExt2;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition childExt3;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition childExt4;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition childExt5;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition childExt6;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition childExt7;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition childExt8;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition childExt9;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition childExt10;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition childExt11;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition childExt12;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition childExt13;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition childExt14;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition childExt15;
            this.User_OnDisableData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.User_GetChildsData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.Trigger_User_ManageSmsTransportData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.User_GetParentsData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.User_GetSelectionData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.User_GetSelection_SegmentData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.User_GetChildsMultiData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.User_GetSelection_w_BroadcastProhibitionData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.User_GetChildsExtData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            User_OnDisable_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            CounfOfAuthTokens = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            User_GetChilds_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            ChildCount_woDeepLimit = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            ChildCount_Deep1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            ChildCount_Deep2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            ChildCount_Deep3 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            CheckDeepValue1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            CheckDeepValue2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            CheckDeepValue3 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Childs_Owner1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Childs_Owner2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Childs_Owner3 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Trigger_User_ManageSmsTransport_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            SmsTransport1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            SmsTransport2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            SmsTransport3 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            User_GetParents_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            ParentsCount = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            ParentsCount_Limit = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            Parents_u1_name = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Parents_u1_deep = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Parents_u2_name = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Parents_u2_deep = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            User_GetSelection_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            User_GetSelection1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            User_GetSelection2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            User_GetSelection3 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            User_GetSelection4 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            User_GetSelection5 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            User_GetSelection6 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            User_GetSelection_Segment_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            User_GetSelection_Segment1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            User_GetSelection_Segment2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            User_GetSelection_Segment3 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            User_GetChildsMulti_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            GetChildsMulti1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            User_GetSelection_w_BroadcastProhibition_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            User_GetSelection_w_BroadcastProhibition1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            User_GetChildsExt_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            childExt1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            childExt2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            childExt3 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            childExt4 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            childExt5 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            childExt6 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            childExt7 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            childExt8 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            childExt9 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            childExt10 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            childExt11 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            childExt12 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            childExt13 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            childExt14 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            childExt15 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            // 
            // User_OnDisable_TestAction
            // 
            User_OnDisable_TestAction.Conditions.Add(CounfOfAuthTokens);
            resources.ApplyResources(User_OnDisable_TestAction, "User_OnDisable_TestAction");
            // 
            // CounfOfAuthTokens
            // 
            CounfOfAuthTokens.Enabled = true;
            CounfOfAuthTokens.Name = "CounfOfAuthTokens";
            CounfOfAuthTokens.ResultSet = 1;
            CounfOfAuthTokens.RowCount = 0;
            // 
            // User_GetChilds_TestAction
            // 
            User_GetChilds_TestAction.Conditions.Add(ChildCount_woDeepLimit);
            User_GetChilds_TestAction.Conditions.Add(ChildCount_Deep1);
            User_GetChilds_TestAction.Conditions.Add(ChildCount_Deep2);
            User_GetChilds_TestAction.Conditions.Add(ChildCount_Deep3);
            User_GetChilds_TestAction.Conditions.Add(CheckDeepValue1);
            User_GetChilds_TestAction.Conditions.Add(CheckDeepValue2);
            User_GetChilds_TestAction.Conditions.Add(CheckDeepValue3);
            User_GetChilds_TestAction.Conditions.Add(Childs_Owner1);
            User_GetChilds_TestAction.Conditions.Add(Childs_Owner2);
            User_GetChilds_TestAction.Conditions.Add(Childs_Owner3);
            resources.ApplyResources(User_GetChilds_TestAction, "User_GetChilds_TestAction");
            // 
            // ChildCount_woDeepLimit
            // 
            ChildCount_woDeepLimit.Enabled = true;
            ChildCount_woDeepLimit.Name = "ChildCount_woDeepLimit";
            ChildCount_woDeepLimit.ResultSet = 1;
            ChildCount_woDeepLimit.RowCount = 5;
            // 
            // ChildCount_Deep1
            // 
            ChildCount_Deep1.Enabled = true;
            ChildCount_Deep1.Name = "ChildCount_Deep1";
            ChildCount_Deep1.ResultSet = 2;
            ChildCount_Deep1.RowCount = 2;
            // 
            // ChildCount_Deep2
            // 
            ChildCount_Deep2.Enabled = true;
            ChildCount_Deep2.Name = "ChildCount_Deep2";
            ChildCount_Deep2.ResultSet = 3;
            ChildCount_Deep2.RowCount = 4;
            // 
            // ChildCount_Deep3
            // 
            ChildCount_Deep3.Enabled = true;
            ChildCount_Deep3.Name = "ChildCount_Deep3";
            ChildCount_Deep3.ResultSet = 4;
            ChildCount_Deep3.RowCount = 5;
            // 
            // CheckDeepValue1
            // 
            CheckDeepValue1.ColumnNumber = 1;
            CheckDeepValue1.Enabled = true;
            CheckDeepValue1.ExpectedValue = "1";
            CheckDeepValue1.Name = "CheckDeepValue1";
            CheckDeepValue1.NullExpected = false;
            CheckDeepValue1.ResultSet = 5;
            CheckDeepValue1.RowNumber = 1;
            // 
            // CheckDeepValue2
            // 
            CheckDeepValue2.ColumnNumber = 1;
            CheckDeepValue2.Enabled = true;
            CheckDeepValue2.ExpectedValue = "2";
            CheckDeepValue2.Name = "CheckDeepValue2";
            CheckDeepValue2.NullExpected = false;
            CheckDeepValue2.ResultSet = 5;
            CheckDeepValue2.RowNumber = 2;
            // 
            // CheckDeepValue3
            // 
            CheckDeepValue3.ColumnNumber = 1;
            CheckDeepValue3.Enabled = true;
            CheckDeepValue3.ExpectedValue = "3";
            CheckDeepValue3.Name = "CheckDeepValue3";
            CheckDeepValue3.NullExpected = false;
            CheckDeepValue3.ResultSet = 5;
            CheckDeepValue3.RowNumber = 3;
            // 
            // Childs_Owner1
            // 
            Childs_Owner1.ColumnNumber = 1;
            Childs_Owner1.Enabled = true;
            Childs_Owner1.ExpectedValue = "1";
            Childs_Owner1.Name = "Childs_Owner1";
            Childs_Owner1.NullExpected = false;
            Childs_Owner1.ResultSet = 6;
            Childs_Owner1.RowNumber = 1;
            // 
            // Childs_Owner2
            // 
            Childs_Owner2.ColumnNumber = 1;
            Childs_Owner2.Enabled = true;
            Childs_Owner2.ExpectedValue = "2";
            Childs_Owner2.Name = "Childs_Owner2";
            Childs_Owner2.NullExpected = false;
            Childs_Owner2.ResultSet = 7;
            Childs_Owner2.RowNumber = 1;
            // 
            // Childs_Owner3
            // 
            Childs_Owner3.ColumnNumber = 1;
            Childs_Owner3.Enabled = true;
            Childs_Owner3.ExpectedValue = "3";
            Childs_Owner3.Name = "Childs_Owner3";
            Childs_Owner3.NullExpected = false;
            Childs_Owner3.ResultSet = 8;
            Childs_Owner3.RowNumber = 1;
            // 
            // Trigger_User_ManageSmsTransport_TestAction
            // 
            Trigger_User_ManageSmsTransport_TestAction.Conditions.Add(SmsTransport1);
            Trigger_User_ManageSmsTransport_TestAction.Conditions.Add(SmsTransport2);
            Trigger_User_ManageSmsTransport_TestAction.Conditions.Add(SmsTransport3);
            resources.ApplyResources(Trigger_User_ManageSmsTransport_TestAction, "Trigger_User_ManageSmsTransport_TestAction");
            // 
            // SmsTransport1
            // 
            SmsTransport1.ColumnNumber = 1;
            SmsTransport1.Enabled = true;
            SmsTransport1.ExpectedValue = "True";
            SmsTransport1.Name = "SmsTransport1";
            SmsTransport1.NullExpected = false;
            SmsTransport1.ResultSet = 1;
            SmsTransport1.RowNumber = 1;
            // 
            // SmsTransport2
            // 
            SmsTransport2.ColumnNumber = 1;
            SmsTransport2.Enabled = true;
            SmsTransport2.ExpectedValue = "False";
            SmsTransport2.Name = "SmsTransport2";
            SmsTransport2.NullExpected = false;
            SmsTransport2.ResultSet = 2;
            SmsTransport2.RowNumber = 1;
            // 
            // SmsTransport3
            // 
            SmsTransport3.ColumnNumber = 1;
            SmsTransport3.Enabled = true;
            SmsTransport3.ExpectedValue = "True";
            SmsTransport3.Name = "SmsTransport3";
            SmsTransport3.NullExpected = false;
            SmsTransport3.ResultSet = 3;
            SmsTransport3.RowNumber = 1;
            // 
            // User_GetParents_TestAction
            // 
            User_GetParents_TestAction.Conditions.Add(ParentsCount);
            User_GetParents_TestAction.Conditions.Add(ParentsCount_Limit);
            User_GetParents_TestAction.Conditions.Add(Parents_u1_name);
            User_GetParents_TestAction.Conditions.Add(Parents_u1_deep);
            User_GetParents_TestAction.Conditions.Add(Parents_u2_name);
            User_GetParents_TestAction.Conditions.Add(Parents_u2_deep);
            resources.ApplyResources(User_GetParents_TestAction, "User_GetParents_TestAction");
            // 
            // ParentsCount
            // 
            ParentsCount.Enabled = true;
            ParentsCount.Name = "ParentsCount";
            ParentsCount.ResultSet = 1;
            ParentsCount.RowCount = 2;
            // 
            // ParentsCount_Limit
            // 
            ParentsCount_Limit.Enabled = true;
            ParentsCount_Limit.Name = "ParentsCount_Limit";
            ParentsCount_Limit.ResultSet = 2;
            ParentsCount_Limit.RowCount = 1;
            // 
            // Parents_u1_name
            // 
            Parents_u1_name.ColumnNumber = 1;
            Parents_u1_name.Enabled = true;
            Parents_u1_name.ExpectedValue = "u1";
            Parents_u1_name.Name = "Parents_u1_name";
            Parents_u1_name.NullExpected = false;
            Parents_u1_name.ResultSet = 1;
            Parents_u1_name.RowNumber = 1;
            // 
            // Parents_u1_deep
            // 
            Parents_u1_deep.ColumnNumber = 2;
            Parents_u1_deep.Enabled = true;
            Parents_u1_deep.ExpectedValue = "-1";
            Parents_u1_deep.Name = "Parents_u1_deep";
            Parents_u1_deep.NullExpected = false;
            Parents_u1_deep.ResultSet = 1;
            Parents_u1_deep.RowNumber = 1;
            // 
            // Parents_u2_name
            // 
            Parents_u2_name.ColumnNumber = 1;
            Parents_u2_name.Enabled = true;
            Parents_u2_name.ExpectedValue = "u2";
            Parents_u2_name.Name = "Parents_u2_name";
            Parents_u2_name.NullExpected = false;
            Parents_u2_name.ResultSet = 1;
            Parents_u2_name.RowNumber = 2;
            // 
            // Parents_u2_deep
            // 
            Parents_u2_deep.ColumnNumber = 2;
            Parents_u2_deep.Enabled = true;
            Parents_u2_deep.ExpectedValue = "-2";
            Parents_u2_deep.Name = "Parents_u2_deep";
            Parents_u2_deep.NullExpected = false;
            Parents_u2_deep.ResultSet = 1;
            Parents_u2_deep.RowNumber = 2;
            // 
            // User_GetSelection_TestAction
            // 
            User_GetSelection_TestAction.Conditions.Add(User_GetSelection1);
            User_GetSelection_TestAction.Conditions.Add(User_GetSelection2);
            User_GetSelection_TestAction.Conditions.Add(User_GetSelection3);
            User_GetSelection_TestAction.Conditions.Add(User_GetSelection4);
            User_GetSelection_TestAction.Conditions.Add(User_GetSelection5);
            User_GetSelection_TestAction.Conditions.Add(User_GetSelection6);
            resources.ApplyResources(User_GetSelection_TestAction, "User_GetSelection_TestAction");
            // 
            // User_GetSelection1
            // 
            User_GetSelection1.ColumnNumber = 1;
            User_GetSelection1.Enabled = true;
            User_GetSelection1.ExpectedValue = "1";
            User_GetSelection1.Name = "User_GetSelection1";
            User_GetSelection1.NullExpected = false;
            User_GetSelection1.ResultSet = 1;
            User_GetSelection1.RowNumber = 1;
            // 
            // User_GetSelection2
            // 
            User_GetSelection2.ColumnNumber = 1;
            User_GetSelection2.Enabled = true;
            User_GetSelection2.ExpectedValue = "2";
            User_GetSelection2.Name = "User_GetSelection2";
            User_GetSelection2.NullExpected = false;
            User_GetSelection2.ResultSet = 2;
            User_GetSelection2.RowNumber = 1;
            // 
            // User_GetSelection3
            // 
            User_GetSelection3.ColumnNumber = 1;
            User_GetSelection3.Enabled = true;
            User_GetSelection3.ExpectedValue = "3";
            User_GetSelection3.Name = "User_GetSelection3";
            User_GetSelection3.NullExpected = false;
            User_GetSelection3.ResultSet = 3;
            User_GetSelection3.RowNumber = 1;
            // 
            // User_GetSelection4
            // 
            User_GetSelection4.ColumnNumber = 1;
            User_GetSelection4.Enabled = true;
            User_GetSelection4.ExpectedValue = "4";
            User_GetSelection4.Name = "User_GetSelection4";
            User_GetSelection4.NullExpected = false;
            User_GetSelection4.ResultSet = 4;
            User_GetSelection4.RowNumber = 1;
            // 
            // User_GetSelection5
            // 
            User_GetSelection5.ColumnNumber = 1;
            User_GetSelection5.Enabled = true;
            User_GetSelection5.ExpectedValue = "5";
            User_GetSelection5.Name = "User_GetSelection5";
            User_GetSelection5.NullExpected = false;
            User_GetSelection5.ResultSet = 5;
            User_GetSelection5.RowNumber = 1;
            // 
            // User_GetSelection6
            // 
            User_GetSelection6.ColumnNumber = 1;
            User_GetSelection6.Enabled = true;
            User_GetSelection6.ExpectedValue = "6";
            User_GetSelection6.Name = "User_GetSelection6";
            User_GetSelection6.NullExpected = false;
            User_GetSelection6.ResultSet = 6;
            User_GetSelection6.RowNumber = 1;
            // 
            // User_GetSelection_Segment_TestAction
            // 
            User_GetSelection_Segment_TestAction.Conditions.Add(User_GetSelection_Segment1);
            User_GetSelection_Segment_TestAction.Conditions.Add(User_GetSelection_Segment2);
            User_GetSelection_Segment_TestAction.Conditions.Add(User_GetSelection_Segment3);
            resources.ApplyResources(User_GetSelection_Segment_TestAction, "User_GetSelection_Segment_TestAction");
            // 
            // User_GetSelection_Segment1
            // 
            User_GetSelection_Segment1.ColumnNumber = 1;
            User_GetSelection_Segment1.Enabled = true;
            User_GetSelection_Segment1.ExpectedValue = "1";
            User_GetSelection_Segment1.Name = "User_GetSelection_Segment1";
            User_GetSelection_Segment1.NullExpected = false;
            User_GetSelection_Segment1.ResultSet = 1;
            User_GetSelection_Segment1.RowNumber = 1;
            // 
            // User_GetSelection_Segment2
            // 
            User_GetSelection_Segment2.ColumnNumber = 1;
            User_GetSelection_Segment2.Enabled = true;
            User_GetSelection_Segment2.ExpectedValue = "2";
            User_GetSelection_Segment2.Name = "User_GetSelection_Segment2";
            User_GetSelection_Segment2.NullExpected = false;
            User_GetSelection_Segment2.ResultSet = 2;
            User_GetSelection_Segment2.RowNumber = 1;
            // 
            // User_GetSelection_Segment3
            // 
            User_GetSelection_Segment3.ColumnNumber = 1;
            User_GetSelection_Segment3.Enabled = true;
            User_GetSelection_Segment3.ExpectedValue = "3";
            User_GetSelection_Segment3.Name = "User_GetSelection_Segment3";
            User_GetSelection_Segment3.NullExpected = false;
            User_GetSelection_Segment3.ResultSet = 3;
            User_GetSelection_Segment3.RowNumber = 1;
            // 
            // User_GetChildsMulti_TestAction
            // 
            User_GetChildsMulti_TestAction.Conditions.Add(GetChildsMulti1);
            resources.ApplyResources(User_GetChildsMulti_TestAction, "User_GetChildsMulti_TestAction");
            // 
            // GetChildsMulti1
            // 
            GetChildsMulti1.ColumnNumber = 1;
            GetChildsMulti1.Enabled = true;
            GetChildsMulti1.ExpectedValue = "1";
            GetChildsMulti1.Name = "GetChildsMulti1";
            GetChildsMulti1.NullExpected = false;
            GetChildsMulti1.ResultSet = 1;
            GetChildsMulti1.RowNumber = 1;
            // 
            // User_GetSelection_w_BroadcastProhibition_TestAction
            // 
            User_GetSelection_w_BroadcastProhibition_TestAction.Conditions.Add(User_GetSelection_w_BroadcastProhibition1);
            resources.ApplyResources(User_GetSelection_w_BroadcastProhibition_TestAction, "User_GetSelection_w_BroadcastProhibition_TestAction");
            // 
            // User_GetSelection_w_BroadcastProhibition1
            // 
            User_GetSelection_w_BroadcastProhibition1.ColumnNumber = 1;
            User_GetSelection_w_BroadcastProhibition1.Enabled = true;
            User_GetSelection_w_BroadcastProhibition1.ExpectedValue = "1";
            User_GetSelection_w_BroadcastProhibition1.Name = "User_GetSelection_w_BroadcastProhibition1";
            User_GetSelection_w_BroadcastProhibition1.NullExpected = false;
            User_GetSelection_w_BroadcastProhibition1.ResultSet = 1;
            User_GetSelection_w_BroadcastProhibition1.RowNumber = 1;
            // 
            // User_GetChildsExt_TestAction
            // 
            User_GetChildsExt_TestAction.Conditions.Add(childExt1);
            User_GetChildsExt_TestAction.Conditions.Add(childExt2);
            User_GetChildsExt_TestAction.Conditions.Add(childExt3);
            User_GetChildsExt_TestAction.Conditions.Add(childExt4);
            User_GetChildsExt_TestAction.Conditions.Add(childExt5);
            User_GetChildsExt_TestAction.Conditions.Add(childExt6);
            User_GetChildsExt_TestAction.Conditions.Add(childExt7);
            User_GetChildsExt_TestAction.Conditions.Add(childExt8);
            User_GetChildsExt_TestAction.Conditions.Add(childExt9);
            User_GetChildsExt_TestAction.Conditions.Add(childExt10);
            User_GetChildsExt_TestAction.Conditions.Add(childExt11);
            User_GetChildsExt_TestAction.Conditions.Add(childExt12);
            User_GetChildsExt_TestAction.Conditions.Add(childExt13);
            User_GetChildsExt_TestAction.Conditions.Add(childExt14);
            User_GetChildsExt_TestAction.Conditions.Add(childExt15);
            resources.ApplyResources(User_GetChildsExt_TestAction, "User_GetChildsExt_TestAction");
            // 
            // childExt1
            // 
            childExt1.Enabled = true;
            childExt1.Name = "childExt1";
            childExt1.ResultSet = 1;
            childExt1.RowCount = 5;
            // 
            // childExt2
            // 
            childExt2.Enabled = true;
            childExt2.Name = "childExt2";
            childExt2.ResultSet = 2;
            childExt2.RowCount = 2;
            // 
            // childExt3
            // 
            childExt3.Enabled = true;
            childExt3.Name = "childExt3";
            childExt3.ResultSet = 3;
            childExt3.RowCount = 4;
            // 
            // childExt4
            // 
            childExt4.Enabled = true;
            childExt4.Name = "childExt4";
            childExt4.ResultSet = 4;
            childExt4.RowCount = 5;
            // 
            // childExt5
            // 
            childExt5.ColumnNumber = 1;
            childExt5.Enabled = true;
            childExt5.ExpectedValue = "1";
            childExt5.Name = "childExt5";
            childExt5.NullExpected = false;
            childExt5.ResultSet = 5;
            childExt5.RowNumber = 1;
            // 
            // childExt6
            // 
            childExt6.ColumnNumber = 1;
            childExt6.Enabled = true;
            childExt6.ExpectedValue = "2";
            childExt6.Name = "childExt6";
            childExt6.NullExpected = false;
            childExt6.ResultSet = 5;
            childExt6.RowNumber = 2;
            // 
            // childExt7
            // 
            childExt7.ColumnNumber = 1;
            childExt7.Enabled = true;
            childExt7.ExpectedValue = "3";
            childExt7.Name = "childExt7";
            childExt7.NullExpected = false;
            childExt7.ResultSet = 5;
            childExt7.RowNumber = 3;
            // 
            // childExt8
            // 
            childExt8.ColumnNumber = 1;
            childExt8.Enabled = true;
            childExt8.ExpectedValue = "1";
            childExt8.Name = "childExt8";
            childExt8.NullExpected = false;
            childExt8.ResultSet = 6;
            childExt8.RowNumber = 1;
            // 
            // childExt9
            // 
            childExt9.ColumnNumber = 1;
            childExt9.Enabled = true;
            childExt9.ExpectedValue = "2";
            childExt9.Name = "childExt9";
            childExt9.NullExpected = false;
            childExt9.ResultSet = 7;
            childExt9.RowNumber = 1;
            // 
            // childExt10
            // 
            childExt10.ColumnNumber = 1;
            childExt10.Enabled = true;
            childExt10.ExpectedValue = "3";
            childExt10.Name = "childExt10";
            childExt10.NullExpected = false;
            childExt10.ResultSet = 8;
            childExt10.RowNumber = 1;
            // 
            // childExt11
            // 
            childExt11.ColumnNumber = 1;
            childExt11.Enabled = false;
            childExt11.ExpectedValue = "True";
            childExt11.Name = "childExt11";
            childExt11.NullExpected = false;
            childExt11.ResultSet = 9;
            childExt11.RowNumber = 1;
            // 
            // childExt12
            // 
            childExt12.ColumnNumber = 1;
            childExt12.Enabled = false;
            childExt12.ExpectedValue = "False";
            childExt12.Name = "childExt12";
            childExt12.NullExpected = false;
            childExt12.ResultSet = 10;
            childExt12.RowNumber = 1;
            // 
            // childExt13
            // 
            childExt13.Enabled = false;
            childExt13.Name = "childExt13";
            childExt13.ResultSet = 11;
            // 
            // childExt14
            // 
            childExt14.Enabled = false;
            childExt14.Name = "childExt14";
            childExt14.ResultSet = 9;
            childExt14.RowCount = 1;
            // 
            // childExt15
            // 
            childExt15.Enabled = false;
            childExt15.Name = "childExt15";
            childExt15.ResultSet = 10;
            childExt15.RowCount = 1;
            // 
            // User_OnDisableData
            // 
            this.User_OnDisableData.PosttestAction = null;
            this.User_OnDisableData.PretestAction = null;
            this.User_OnDisableData.TestAction = User_OnDisable_TestAction;
            // 
            // User_GetChildsData
            // 
            this.User_GetChildsData.PosttestAction = null;
            this.User_GetChildsData.PretestAction = null;
            this.User_GetChildsData.TestAction = User_GetChilds_TestAction;
            // 
            // Trigger_User_ManageSmsTransportData
            // 
            this.Trigger_User_ManageSmsTransportData.PosttestAction = null;
            this.Trigger_User_ManageSmsTransportData.PretestAction = null;
            this.Trigger_User_ManageSmsTransportData.TestAction = Trigger_User_ManageSmsTransport_TestAction;
            // 
            // User_GetParentsData
            // 
            this.User_GetParentsData.PosttestAction = null;
            this.User_GetParentsData.PretestAction = null;
            this.User_GetParentsData.TestAction = User_GetParents_TestAction;
            // 
            // User_GetSelectionData
            // 
            this.User_GetSelectionData.PosttestAction = null;
            this.User_GetSelectionData.PretestAction = null;
            this.User_GetSelectionData.TestAction = User_GetSelection_TestAction;
            // 
            // User_GetSelection_SegmentData
            // 
            this.User_GetSelection_SegmentData.PosttestAction = null;
            this.User_GetSelection_SegmentData.PretestAction = null;
            this.User_GetSelection_SegmentData.TestAction = User_GetSelection_Segment_TestAction;
            // 
            // User_GetChildsMultiData
            // 
            this.User_GetChildsMultiData.PosttestAction = null;
            this.User_GetChildsMultiData.PretestAction = null;
            this.User_GetChildsMultiData.TestAction = User_GetChildsMulti_TestAction;
            // 
            // User_GetSelection_w_BroadcastProhibitionData
            // 
            this.User_GetSelection_w_BroadcastProhibitionData.PosttestAction = null;
            this.User_GetSelection_w_BroadcastProhibitionData.PretestAction = null;
            this.User_GetSelection_w_BroadcastProhibitionData.TestAction = User_GetSelection_w_BroadcastProhibition_TestAction;
            // 
            // User_GetChildsExtData
            // 
            this.User_GetChildsExtData.PosttestAction = null;
            this.User_GetChildsExtData.PretestAction = null;
            this.User_GetChildsExtData.TestAction = User_GetChildsExt_TestAction;
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

        private SqlDatabaseTestActions User_OnDisableData;
        private SqlDatabaseTestActions User_GetChildsData;
        private SqlDatabaseTestActions Trigger_User_ManageSmsTransportData;
        private SqlDatabaseTestActions User_GetParentsData;
        private SqlDatabaseTestActions User_GetSelectionData;
        private SqlDatabaseTestActions User_GetSelection_SegmentData;
        private SqlDatabaseTestActions User_GetChildsMultiData;
        private SqlDatabaseTestActions User_GetSelection_w_BroadcastProhibitionData;
        private SqlDatabaseTestActions User_GetChildsExtData;
    }
}
