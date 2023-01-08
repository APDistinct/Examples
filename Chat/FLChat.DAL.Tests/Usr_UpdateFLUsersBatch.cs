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
    public class Usr_UpdateFLUsersBatch : SqlDatabaseTestClass
    {

        public Usr_UpdateFLUsersBatch() {
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
        public void FullNameTest() {
            SqlDatabaseTestActions testActions = this.FullNameTestData;
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
        public void RankCityTest() {
            SqlDatabaseTestActions testActions = this.RankCityTestData;
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
        public void OwnerTest() {
            SqlDatabaseTestActions testActions = this.OwnerTestData;
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
        public void DublicatePhone() {
            SqlDatabaseTestActions testActions = this.DublicatePhoneData;
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
        public void DublicateEmail() {
            SqlDatabaseTestActions testActions = this.DublicateEmailData;
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
        public void Import() {
            SqlDatabaseTestActions testActions = this.ImportData;
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
        public void ListImportDate() {
            SqlDatabaseTestActions testActions = this.ListImportDateData;
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
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction FullNameTest_TestAction;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Usr_UpdateFLUsersBatch));
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition FullName_Ins1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition FullName_Ins2;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition FullName_Ins3;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition FullName_Upd1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition FullName_Upd2;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition FullName_Upd3;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition FullName_UpdCnt;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction RankCityTest_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition RankCity_Different1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition RankCity_Different2;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition RankCity_UpdCnt;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction OwnerTest_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Onwer_Ins_UpdOwnerCnt;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Owner_Ins_MissedOwnerCnt;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition Owner_Ins_MissedOwner_RowCount;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Owner_Ins_MissedOwner_Value;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition Owner_Ins_Differ;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Owner_Upd1_UpdatedCount;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Owner_Upd1_UpdOwnerCnt;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Owner_Upd1_MissedOwnCnt;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition Owner_Upd1_MissedOwnerEmpty;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition Owner_Upd1_Differ;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Owner_Upd2_Updated;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Owner_Upd2_OwnerUpdated;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Owner_Upd2_MissedOwnerCnt;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition Owner_Upd2_MissedOwnTable;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition Owner_Upd2_Differ;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction DublicatePhone_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Phone_Ins_ClearedPhoneCnt;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition Phone_Ins_MissedPhoneTable;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Phone_Ins_FLUserNumber;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Phone_Ins_Phone;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Phone_Upd_UpdatedCount;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Phone_Upd_ClearedPhoneCnt;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition Phone_Upd_MissedPhoneRowCount;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Phone_Upd_FLUserNumber;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Phone_Upd_Phone;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition Phone_Ins_Different;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition Phone_Upd_Different;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction DublicateEmail_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Mail_Ins_ClearedMail;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition Mail_Ins_ClearedMailRowCount;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Mail_Ins_FLUserNumber;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Mail_Ins_EMail;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition Mail_Ins_Differ;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Mail_Upd_UpdatedCount;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Mail_Upd_ClearedMail;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition Mail_Upd_ClearedPhone_RowCount;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Mail_Upd_FLUserNumber;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Mail_Upd_Email;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition Mail_Upd_Differ;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction Import_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Import_Ins_Updated;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Import_Ins_Inserted;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition Import_Ins_Different;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Import_Upd_Updated;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Import_Upd_Inserted;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition Import_Upd_Different;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Import_Upd2_Updated;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition Import_Upd2_Inserted;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction ListImportDate_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition scalarValueCondition1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition scalarValueCondition2;
            this.FullNameTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.RankCityTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.OwnerTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.DublicatePhoneData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.DublicateEmailData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.ImportData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.ListImportDateData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            FullNameTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            FullName_Ins1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            FullName_Ins2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            FullName_Ins3 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            FullName_Upd1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            FullName_Upd2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            FullName_Upd3 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            FullName_UpdCnt = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            RankCityTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            RankCity_Different1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            RankCity_Different2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            RankCity_UpdCnt = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            OwnerTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            Onwer_Ins_UpdOwnerCnt = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Owner_Ins_MissedOwnerCnt = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Owner_Ins_MissedOwner_RowCount = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            Owner_Ins_MissedOwner_Value = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Owner_Ins_Differ = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            Owner_Upd1_UpdatedCount = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Owner_Upd1_UpdOwnerCnt = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Owner_Upd1_MissedOwnCnt = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Owner_Upd1_MissedOwnerEmpty = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            Owner_Upd1_Differ = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            Owner_Upd2_Updated = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Owner_Upd2_OwnerUpdated = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Owner_Upd2_MissedOwnerCnt = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Owner_Upd2_MissedOwnTable = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            Owner_Upd2_Differ = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            DublicatePhone_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            Phone_Ins_ClearedPhoneCnt = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Phone_Ins_MissedPhoneTable = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            Phone_Ins_FLUserNumber = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Phone_Ins_Phone = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Phone_Upd_UpdatedCount = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Phone_Upd_ClearedPhoneCnt = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Phone_Upd_MissedPhoneRowCount = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            Phone_Upd_FLUserNumber = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Phone_Upd_Phone = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Phone_Ins_Different = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            Phone_Upd_Different = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            DublicateEmail_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            Mail_Ins_ClearedMail = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Mail_Ins_ClearedMailRowCount = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            Mail_Ins_FLUserNumber = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Mail_Ins_EMail = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Mail_Ins_Differ = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            Mail_Upd_UpdatedCount = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Mail_Upd_ClearedMail = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Mail_Upd_ClearedPhone_RowCount = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            Mail_Upd_FLUserNumber = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Mail_Upd_Email = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Mail_Upd_Differ = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            Import_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            Import_Ins_Updated = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Import_Ins_Inserted = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Import_Ins_Different = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            Import_Upd_Updated = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Import_Upd_Inserted = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Import_Upd_Different = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            Import_Upd2_Updated = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            Import_Upd2_Inserted = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            ListImportDate_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            scalarValueCondition1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            scalarValueCondition2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            // 
            // FullNameTest_TestAction
            // 
            FullNameTest_TestAction.Conditions.Add(FullName_Ins1);
            FullNameTest_TestAction.Conditions.Add(FullName_Ins2);
            FullNameTest_TestAction.Conditions.Add(FullName_Ins3);
            FullNameTest_TestAction.Conditions.Add(FullName_Upd1);
            FullNameTest_TestAction.Conditions.Add(FullName_Upd2);
            FullNameTest_TestAction.Conditions.Add(FullName_Upd3);
            FullNameTest_TestAction.Conditions.Add(FullName_UpdCnt);
            resources.ApplyResources(FullNameTest_TestAction, "FullNameTest_TestAction");
            // 
            // FullName_Ins1
            // 
            FullName_Ins1.ColumnNumber = 2;
            FullName_Ins1.Enabled = true;
            FullName_Ins1.ExpectedValue = "S1 N1 P1";
            FullName_Ins1.Name = "FullName_Ins1";
            FullName_Ins1.NullExpected = false;
            FullName_Ins1.ResultSet = 5;
            FullName_Ins1.RowNumber = 1;
            // 
            // FullName_Ins2
            // 
            FullName_Ins2.ColumnNumber = 2;
            FullName_Ins2.Enabled = true;
            FullName_Ins2.ExpectedValue = "S2 N2";
            FullName_Ins2.Name = "FullName_Ins2";
            FullName_Ins2.NullExpected = false;
            FullName_Ins2.ResultSet = 5;
            FullName_Ins2.RowNumber = 2;
            // 
            // FullName_Ins3
            // 
            FullName_Ins3.ColumnNumber = 2;
            FullName_Ins3.Enabled = true;
            FullName_Ins3.ExpectedValue = "S3 P3";
            FullName_Ins3.Name = "FullName_Ins3";
            FullName_Ins3.NullExpected = false;
            FullName_Ins3.ResultSet = 5;
            FullName_Ins3.RowNumber = 3;
            // 
            // FullName_Upd1
            // 
            FullName_Upd1.ColumnNumber = 2;
            FullName_Upd1.Enabled = true;
            FullName_Upd1.ExpectedValue = "S1 N1 P1";
            FullName_Upd1.Name = "FullName_Upd1";
            FullName_Upd1.NullExpected = false;
            FullName_Upd1.ResultSet = 10;
            FullName_Upd1.RowNumber = 1;
            // 
            // FullName_Upd2
            // 
            FullName_Upd2.ColumnNumber = 2;
            FullName_Upd2.Enabled = true;
            FullName_Upd2.ExpectedValue = "S2 P2";
            FullName_Upd2.Name = "FullName_Upd2";
            FullName_Upd2.NullExpected = false;
            FullName_Upd2.ResultSet = 10;
            FullName_Upd2.RowNumber = 2;
            // 
            // FullName_Upd3
            // 
            FullName_Upd3.ColumnNumber = 2;
            FullName_Upd3.Enabled = true;
            FullName_Upd3.ExpectedValue = "S3 N3";
            FullName_Upd3.Name = "FullName_Upd3";
            FullName_Upd3.NullExpected = false;
            FullName_Upd3.ResultSet = 10;
            FullName_Upd3.RowNumber = 3;
            // 
            // FullName_UpdCnt
            // 
            FullName_UpdCnt.ColumnNumber = 1;
            FullName_UpdCnt.Enabled = true;
            FullName_UpdCnt.ExpectedValue = "2";
            FullName_UpdCnt.Name = "FullName_UpdCnt";
            FullName_UpdCnt.NullExpected = false;
            FullName_UpdCnt.ResultSet = 6;
            FullName_UpdCnt.RowNumber = 1;
            // 
            // RankCityTest_TestAction
            // 
            RankCityTest_TestAction.Conditions.Add(RankCity_Different1);
            RankCityTest_TestAction.Conditions.Add(RankCity_Different2);
            RankCityTest_TestAction.Conditions.Add(RankCity_UpdCnt);
            resources.ApplyResources(RankCityTest_TestAction, "RankCityTest_TestAction");
            // 
            // RankCity_Different1
            // 
            RankCity_Different1.Enabled = true;
            RankCity_Different1.Name = "RankCity_Different1";
            RankCity_Different1.ResultSet = 5;
            // 
            // RankCity_Different2
            // 
            RankCity_Different2.Enabled = true;
            RankCity_Different2.Name = "RankCity_Different2";
            RankCity_Different2.ResultSet = 10;
            // 
            // RankCity_UpdCnt
            // 
            RankCity_UpdCnt.ColumnNumber = 1;
            RankCity_UpdCnt.Enabled = true;
            RankCity_UpdCnt.ExpectedValue = "1";
            RankCity_UpdCnt.Name = "RankCity_UpdCnt";
            RankCity_UpdCnt.NullExpected = false;
            RankCity_UpdCnt.ResultSet = 6;
            RankCity_UpdCnt.RowNumber = 1;
            // 
            // OwnerTest_TestAction
            // 
            OwnerTest_TestAction.Conditions.Add(Onwer_Ins_UpdOwnerCnt);
            OwnerTest_TestAction.Conditions.Add(Owner_Ins_MissedOwnerCnt);
            OwnerTest_TestAction.Conditions.Add(Owner_Ins_MissedOwner_RowCount);
            OwnerTest_TestAction.Conditions.Add(Owner_Ins_MissedOwner_Value);
            OwnerTest_TestAction.Conditions.Add(Owner_Ins_Differ);
            OwnerTest_TestAction.Conditions.Add(Owner_Upd1_UpdatedCount);
            OwnerTest_TestAction.Conditions.Add(Owner_Upd1_UpdOwnerCnt);
            OwnerTest_TestAction.Conditions.Add(Owner_Upd1_MissedOwnCnt);
            OwnerTest_TestAction.Conditions.Add(Owner_Upd1_MissedOwnerEmpty);
            OwnerTest_TestAction.Conditions.Add(Owner_Upd1_Differ);
            OwnerTest_TestAction.Conditions.Add(Owner_Upd2_Updated);
            OwnerTest_TestAction.Conditions.Add(Owner_Upd2_OwnerUpdated);
            OwnerTest_TestAction.Conditions.Add(Owner_Upd2_MissedOwnerCnt);
            OwnerTest_TestAction.Conditions.Add(Owner_Upd2_MissedOwnTable);
            OwnerTest_TestAction.Conditions.Add(Owner_Upd2_Differ);
            resources.ApplyResources(OwnerTest_TestAction, "OwnerTest_TestAction");
            // 
            // Onwer_Ins_UpdOwnerCnt
            // 
            Onwer_Ins_UpdOwnerCnt.ColumnNumber = 5;
            Onwer_Ins_UpdOwnerCnt.Enabled = true;
            Onwer_Ins_UpdOwnerCnt.ExpectedValue = "2";
            Onwer_Ins_UpdOwnerCnt.Name = "Onwer_Ins_UpdOwnerCnt";
            Onwer_Ins_UpdOwnerCnt.NullExpected = false;
            Onwer_Ins_UpdOwnerCnt.ResultSet = 1;
            Onwer_Ins_UpdOwnerCnt.RowNumber = 1;
            // 
            // Owner_Ins_MissedOwnerCnt
            // 
            Owner_Ins_MissedOwnerCnt.ColumnNumber = 6;
            Owner_Ins_MissedOwnerCnt.Enabled = true;
            Owner_Ins_MissedOwnerCnt.ExpectedValue = "1";
            Owner_Ins_MissedOwnerCnt.Name = "Owner_Ins_MissedOwnerCnt";
            Owner_Ins_MissedOwnerCnt.NullExpected = false;
            Owner_Ins_MissedOwnerCnt.ResultSet = 1;
            Owner_Ins_MissedOwnerCnt.RowNumber = 1;
            // 
            // Owner_Ins_MissedOwner_RowCount
            // 
            Owner_Ins_MissedOwner_RowCount.Enabled = true;
            Owner_Ins_MissedOwner_RowCount.Name = "Owner_Ins_MissedOwner_RowCount";
            Owner_Ins_MissedOwner_RowCount.ResultSet = 4;
            Owner_Ins_MissedOwner_RowCount.RowCount = 1;
            // 
            // Owner_Ins_MissedOwner_Value
            // 
            Owner_Ins_MissedOwner_Value.ColumnNumber = 1;
            Owner_Ins_MissedOwner_Value.Enabled = true;
            Owner_Ins_MissedOwner_Value.ExpectedValue = "-1";
            Owner_Ins_MissedOwner_Value.Name = "Owner_Ins_MissedOwner_Value";
            Owner_Ins_MissedOwner_Value.NullExpected = false;
            Owner_Ins_MissedOwner_Value.ResultSet = 4;
            Owner_Ins_MissedOwner_Value.RowNumber = 1;
            // 
            // Owner_Ins_Differ
            // 
            Owner_Ins_Differ.Enabled = true;
            Owner_Ins_Differ.Name = "Owner_Ins_Differ";
            Owner_Ins_Differ.ResultSet = 5;
            // 
            // Owner_Upd1_UpdatedCount
            // 
            Owner_Upd1_UpdatedCount.ColumnNumber = 1;
            Owner_Upd1_UpdatedCount.Enabled = true;
            Owner_Upd1_UpdatedCount.ExpectedValue = "3";
            Owner_Upd1_UpdatedCount.Name = "Owner_Upd1_UpdatedCount";
            Owner_Upd1_UpdatedCount.NullExpected = false;
            Owner_Upd1_UpdatedCount.ResultSet = 6;
            Owner_Upd1_UpdatedCount.RowNumber = 1;
            // 
            // Owner_Upd1_UpdOwnerCnt
            // 
            Owner_Upd1_UpdOwnerCnt.ColumnNumber = 5;
            Owner_Upd1_UpdOwnerCnt.Enabled = true;
            Owner_Upd1_UpdOwnerCnt.ExpectedValue = "2";
            Owner_Upd1_UpdOwnerCnt.Name = "Owner_Upd1_UpdOwnerCnt";
            Owner_Upd1_UpdOwnerCnt.NullExpected = false;
            Owner_Upd1_UpdOwnerCnt.ResultSet = 6;
            Owner_Upd1_UpdOwnerCnt.RowNumber = 1;
            // 
            // Owner_Upd1_MissedOwnCnt
            // 
            Owner_Upd1_MissedOwnCnt.ColumnNumber = 6;
            Owner_Upd1_MissedOwnCnt.Enabled = true;
            Owner_Upd1_MissedOwnCnt.ExpectedValue = "0";
            Owner_Upd1_MissedOwnCnt.Name = "Owner_Upd1_MissedOwnCnt";
            Owner_Upd1_MissedOwnCnt.NullExpected = false;
            Owner_Upd1_MissedOwnCnt.ResultSet = 6;
            Owner_Upd1_MissedOwnCnt.RowNumber = 1;
            // 
            // Owner_Upd1_MissedOwnerEmpty
            // 
            Owner_Upd1_MissedOwnerEmpty.Enabled = true;
            Owner_Upd1_MissedOwnerEmpty.Name = "Owner_Upd1_MissedOwnerEmpty";
            Owner_Upd1_MissedOwnerEmpty.ResultSet = 9;
            // 
            // Owner_Upd1_Differ
            // 
            Owner_Upd1_Differ.Enabled = true;
            Owner_Upd1_Differ.Name = "Owner_Upd1_Differ";
            Owner_Upd1_Differ.ResultSet = 10;
            // 
            // Owner_Upd2_Updated
            // 
            Owner_Upd2_Updated.ColumnNumber = 1;
            Owner_Upd2_Updated.Enabled = true;
            Owner_Upd2_Updated.ExpectedValue = "1";
            Owner_Upd2_Updated.Name = "Owner_Upd2_Updated";
            Owner_Upd2_Updated.NullExpected = false;
            Owner_Upd2_Updated.ResultSet = 11;
            Owner_Upd2_Updated.RowNumber = 1;
            // 
            // Owner_Upd2_OwnerUpdated
            // 
            Owner_Upd2_OwnerUpdated.ColumnNumber = 5;
            Owner_Upd2_OwnerUpdated.Enabled = true;
            Owner_Upd2_OwnerUpdated.ExpectedValue = "1";
            Owner_Upd2_OwnerUpdated.Name = "Owner_Upd2_OwnerUpdated";
            Owner_Upd2_OwnerUpdated.NullExpected = false;
            Owner_Upd2_OwnerUpdated.ResultSet = 11;
            Owner_Upd2_OwnerUpdated.RowNumber = 1;
            // 
            // Owner_Upd2_MissedOwnerCnt
            // 
            Owner_Upd2_MissedOwnerCnt.ColumnNumber = 6;
            Owner_Upd2_MissedOwnerCnt.Enabled = true;
            Owner_Upd2_MissedOwnerCnt.ExpectedValue = "0";
            Owner_Upd2_MissedOwnerCnt.Name = "Owner_Upd2_MissedOwnerCnt";
            Owner_Upd2_MissedOwnerCnt.NullExpected = false;
            Owner_Upd2_MissedOwnerCnt.ResultSet = 11;
            Owner_Upd2_MissedOwnerCnt.RowNumber = 1;
            // 
            // Owner_Upd2_MissedOwnTable
            // 
            Owner_Upd2_MissedOwnTable.Enabled = true;
            Owner_Upd2_MissedOwnTable.Name = "Owner_Upd2_MissedOwnTable";
            Owner_Upd2_MissedOwnTable.ResultSet = 14;
            // 
            // Owner_Upd2_Differ
            // 
            Owner_Upd2_Differ.Enabled = true;
            Owner_Upd2_Differ.Name = "Owner_Upd2_Differ";
            Owner_Upd2_Differ.ResultSet = 15;
            // 
            // DublicatePhone_TestAction
            // 
            DublicatePhone_TestAction.Conditions.Add(Phone_Ins_ClearedPhoneCnt);
            DublicatePhone_TestAction.Conditions.Add(Phone_Ins_MissedPhoneTable);
            DublicatePhone_TestAction.Conditions.Add(Phone_Ins_FLUserNumber);
            DublicatePhone_TestAction.Conditions.Add(Phone_Ins_Phone);
            DublicatePhone_TestAction.Conditions.Add(Phone_Upd_UpdatedCount);
            DublicatePhone_TestAction.Conditions.Add(Phone_Upd_ClearedPhoneCnt);
            DublicatePhone_TestAction.Conditions.Add(Phone_Upd_MissedPhoneRowCount);
            DublicatePhone_TestAction.Conditions.Add(Phone_Upd_FLUserNumber);
            DublicatePhone_TestAction.Conditions.Add(Phone_Upd_Phone);
            DublicatePhone_TestAction.Conditions.Add(Phone_Ins_Different);
            DublicatePhone_TestAction.Conditions.Add(Phone_Upd_Different);
            resources.ApplyResources(DublicatePhone_TestAction, "DublicatePhone_TestAction");
            // 
            // Phone_Ins_ClearedPhoneCnt
            // 
            Phone_Ins_ClearedPhoneCnt.ColumnNumber = 3;
            Phone_Ins_ClearedPhoneCnt.Enabled = true;
            Phone_Ins_ClearedPhoneCnt.ExpectedValue = "1";
            Phone_Ins_ClearedPhoneCnt.Name = "Phone_Ins_ClearedPhoneCnt";
            Phone_Ins_ClearedPhoneCnt.NullExpected = false;
            Phone_Ins_ClearedPhoneCnt.ResultSet = 1;
            Phone_Ins_ClearedPhoneCnt.RowNumber = 1;
            // 
            // Phone_Ins_MissedPhoneTable
            // 
            Phone_Ins_MissedPhoneTable.Enabled = true;
            Phone_Ins_MissedPhoneTable.Name = "Phone_Ins_MissedPhoneTable";
            Phone_Ins_MissedPhoneTable.ResultSet = 2;
            Phone_Ins_MissedPhoneTable.RowCount = 1;
            // 
            // Phone_Ins_FLUserNumber
            // 
            Phone_Ins_FLUserNumber.ColumnNumber = 1;
            Phone_Ins_FLUserNumber.Enabled = true;
            Phone_Ins_FLUserNumber.ExpectedValue = "-3";
            Phone_Ins_FLUserNumber.Name = "Phone_Ins_FLUserNumber";
            Phone_Ins_FLUserNumber.NullExpected = false;
            Phone_Ins_FLUserNumber.ResultSet = 2;
            Phone_Ins_FLUserNumber.RowNumber = 1;
            // 
            // Phone_Ins_Phone
            // 
            Phone_Ins_Phone.ColumnNumber = 2;
            Phone_Ins_Phone.Enabled = true;
            Phone_Ins_Phone.ExpectedValue = "-2mobi";
            Phone_Ins_Phone.Name = "Phone_Ins_Phone";
            Phone_Ins_Phone.NullExpected = false;
            Phone_Ins_Phone.ResultSet = 2;
            Phone_Ins_Phone.RowNumber = 1;
            // 
            // Phone_Upd_UpdatedCount
            // 
            Phone_Upd_UpdatedCount.ColumnNumber = 1;
            Phone_Upd_UpdatedCount.Enabled = true;
            Phone_Upd_UpdatedCount.ExpectedValue = "2";
            Phone_Upd_UpdatedCount.Name = "Phone_Upd_UpdatedCount";
            Phone_Upd_UpdatedCount.NullExpected = false;
            Phone_Upd_UpdatedCount.ResultSet = 6;
            Phone_Upd_UpdatedCount.RowNumber = 1;
            // 
            // Phone_Upd_ClearedPhoneCnt
            // 
            Phone_Upd_ClearedPhoneCnt.ColumnNumber = 3;
            Phone_Upd_ClearedPhoneCnt.Enabled = true;
            Phone_Upd_ClearedPhoneCnt.ExpectedValue = "1";
            Phone_Upd_ClearedPhoneCnt.Name = "Phone_Upd_ClearedPhoneCnt";
            Phone_Upd_ClearedPhoneCnt.NullExpected = false;
            Phone_Upd_ClearedPhoneCnt.ResultSet = 6;
            Phone_Upd_ClearedPhoneCnt.RowNumber = 1;
            // 
            // Phone_Upd_MissedPhoneRowCount
            // 
            Phone_Upd_MissedPhoneRowCount.Enabled = true;
            Phone_Upd_MissedPhoneRowCount.Name = "Phone_Upd_MissedPhoneRowCount";
            Phone_Upd_MissedPhoneRowCount.ResultSet = 7;
            Phone_Upd_MissedPhoneRowCount.RowCount = 1;
            // 
            // Phone_Upd_FLUserNumber
            // 
            Phone_Upd_FLUserNumber.ColumnNumber = 1;
            Phone_Upd_FLUserNumber.Enabled = true;
            Phone_Upd_FLUserNumber.ExpectedValue = "-2";
            Phone_Upd_FLUserNumber.Name = "Phone_Upd_FLUserNumber";
            Phone_Upd_FLUserNumber.NullExpected = false;
            Phone_Upd_FLUserNumber.ResultSet = 7;
            Phone_Upd_FLUserNumber.RowNumber = 1;
            // 
            // Phone_Upd_Phone
            // 
            Phone_Upd_Phone.ColumnNumber = 2;
            Phone_Upd_Phone.Enabled = true;
            Phone_Upd_Phone.ExpectedValue = "-1mobi";
            Phone_Upd_Phone.Name = "Phone_Upd_Phone";
            Phone_Upd_Phone.NullExpected = false;
            Phone_Upd_Phone.ResultSet = 7;
            Phone_Upd_Phone.RowNumber = 1;
            // 
            // Phone_Ins_Different
            // 
            Phone_Ins_Different.Enabled = true;
            Phone_Ins_Different.Name = "Phone_Ins_Different";
            Phone_Ins_Different.ResultSet = 5;
            // 
            // Phone_Upd_Different
            // 
            Phone_Upd_Different.Enabled = true;
            Phone_Upd_Different.Name = "Phone_Upd_Different";
            Phone_Upd_Different.ResultSet = 5;
            // 
            // DublicateEmail_TestAction
            // 
            DublicateEmail_TestAction.Conditions.Add(Mail_Ins_ClearedMail);
            DublicateEmail_TestAction.Conditions.Add(Mail_Ins_ClearedMailRowCount);
            DublicateEmail_TestAction.Conditions.Add(Mail_Ins_FLUserNumber);
            DublicateEmail_TestAction.Conditions.Add(Mail_Ins_EMail);
            DublicateEmail_TestAction.Conditions.Add(Mail_Ins_Differ);
            DublicateEmail_TestAction.Conditions.Add(Mail_Upd_UpdatedCount);
            DublicateEmail_TestAction.Conditions.Add(Mail_Upd_ClearedMail);
            DublicateEmail_TestAction.Conditions.Add(Mail_Upd_ClearedPhone_RowCount);
            DublicateEmail_TestAction.Conditions.Add(Mail_Upd_FLUserNumber);
            DublicateEmail_TestAction.Conditions.Add(Mail_Upd_Email);
            DublicateEmail_TestAction.Conditions.Add(Mail_Upd_Differ);
            resources.ApplyResources(DublicateEmail_TestAction, "DublicateEmail_TestAction");
            // 
            // Mail_Ins_ClearedMail
            // 
            Mail_Ins_ClearedMail.ColumnNumber = 4;
            Mail_Ins_ClearedMail.Enabled = true;
            Mail_Ins_ClearedMail.ExpectedValue = "1";
            Mail_Ins_ClearedMail.Name = "Mail_Ins_ClearedMail";
            Mail_Ins_ClearedMail.NullExpected = false;
            Mail_Ins_ClearedMail.ResultSet = 1;
            Mail_Ins_ClearedMail.RowNumber = 1;
            // 
            // Mail_Ins_ClearedMailRowCount
            // 
            Mail_Ins_ClearedMailRowCount.Enabled = true;
            Mail_Ins_ClearedMailRowCount.Name = "Mail_Ins_ClearedMailRowCount";
            Mail_Ins_ClearedMailRowCount.ResultSet = 3;
            Mail_Ins_ClearedMailRowCount.RowCount = 1;
            // 
            // Mail_Ins_FLUserNumber
            // 
            Mail_Ins_FLUserNumber.ColumnNumber = 1;
            Mail_Ins_FLUserNumber.Enabled = true;
            Mail_Ins_FLUserNumber.ExpectedValue = "-3";
            Mail_Ins_FLUserNumber.Name = "Mail_Ins_FLUserNumber";
            Mail_Ins_FLUserNumber.NullExpected = false;
            Mail_Ins_FLUserNumber.ResultSet = 3;
            Mail_Ins_FLUserNumber.RowNumber = 1;
            // 
            // Mail_Ins_EMail
            // 
            Mail_Ins_EMail.ColumnNumber = 2;
            Mail_Ins_EMail.Enabled = true;
            Mail_Ins_EMail.ExpectedValue = "-2mobi";
            Mail_Ins_EMail.Name = "Mail_Ins_EMail";
            Mail_Ins_EMail.NullExpected = false;
            Mail_Ins_EMail.ResultSet = 3;
            Mail_Ins_EMail.RowNumber = 1;
            // 
            // Mail_Ins_Differ
            // 
            Mail_Ins_Differ.Enabled = true;
            Mail_Ins_Differ.Name = "Mail_Ins_Differ";
            Mail_Ins_Differ.ResultSet = 5;
            // 
            // Mail_Upd_UpdatedCount
            // 
            Mail_Upd_UpdatedCount.ColumnNumber = 1;
            Mail_Upd_UpdatedCount.Enabled = true;
            Mail_Upd_UpdatedCount.ExpectedValue = "2";
            Mail_Upd_UpdatedCount.Name = "Mail_Upd_UpdatedCount";
            Mail_Upd_UpdatedCount.NullExpected = false;
            Mail_Upd_UpdatedCount.ResultSet = 6;
            Mail_Upd_UpdatedCount.RowNumber = 1;
            // 
            // Mail_Upd_ClearedMail
            // 
            Mail_Upd_ClearedMail.ColumnNumber = 4;
            Mail_Upd_ClearedMail.Enabled = true;
            Mail_Upd_ClearedMail.ExpectedValue = "1";
            Mail_Upd_ClearedMail.Name = "Mail_Upd_ClearedMail";
            Mail_Upd_ClearedMail.NullExpected = false;
            Mail_Upd_ClearedMail.ResultSet = 6;
            Mail_Upd_ClearedMail.RowNumber = 1;
            // 
            // Mail_Upd_ClearedPhone_RowCount
            // 
            Mail_Upd_ClearedPhone_RowCount.Enabled = true;
            Mail_Upd_ClearedPhone_RowCount.Name = "Mail_Upd_ClearedPhone_RowCount";
            Mail_Upd_ClearedPhone_RowCount.ResultSet = 8;
            Mail_Upd_ClearedPhone_RowCount.RowCount = 1;
            // 
            // Mail_Upd_FLUserNumber
            // 
            Mail_Upd_FLUserNumber.ColumnNumber = 1;
            Mail_Upd_FLUserNumber.Enabled = true;
            Mail_Upd_FLUserNumber.ExpectedValue = "-2";
            Mail_Upd_FLUserNumber.Name = "Mail_Upd_FLUserNumber";
            Mail_Upd_FLUserNumber.NullExpected = false;
            Mail_Upd_FLUserNumber.ResultSet = 8;
            Mail_Upd_FLUserNumber.RowNumber = 1;
            // 
            // Mail_Upd_Email
            // 
            Mail_Upd_Email.ColumnNumber = 2;
            Mail_Upd_Email.Enabled = true;
            Mail_Upd_Email.ExpectedValue = "-1mobi";
            Mail_Upd_Email.Name = "Mail_Upd_Email";
            Mail_Upd_Email.NullExpected = false;
            Mail_Upd_Email.ResultSet = 8;
            Mail_Upd_Email.RowNumber = 1;
            // 
            // Mail_Upd_Differ
            // 
            Mail_Upd_Differ.Enabled = true;
            Mail_Upd_Differ.Name = "Mail_Upd_Differ";
            Mail_Upd_Differ.ResultSet = 10;
            // 
            // Import_TestAction
            // 
            Import_TestAction.Conditions.Add(Import_Ins_Updated);
            Import_TestAction.Conditions.Add(Import_Ins_Inserted);
            Import_TestAction.Conditions.Add(Import_Ins_Different);
            Import_TestAction.Conditions.Add(Import_Upd_Updated);
            Import_TestAction.Conditions.Add(Import_Upd_Inserted);
            Import_TestAction.Conditions.Add(Import_Upd_Different);
            Import_TestAction.Conditions.Add(Import_Upd2_Updated);
            Import_TestAction.Conditions.Add(Import_Upd2_Inserted);
            resources.ApplyResources(Import_TestAction, "Import_TestAction");
            // 
            // Import_Ins_Updated
            // 
            Import_Ins_Updated.ColumnNumber = 1;
            Import_Ins_Updated.Enabled = true;
            Import_Ins_Updated.ExpectedValue = "0";
            Import_Ins_Updated.Name = "Import_Ins_Updated";
            Import_Ins_Updated.NullExpected = false;
            Import_Ins_Updated.ResultSet = 1;
            Import_Ins_Updated.RowNumber = 1;
            // 
            // Import_Ins_Inserted
            // 
            Import_Ins_Inserted.ColumnNumber = 2;
            Import_Ins_Inserted.Enabled = true;
            Import_Ins_Inserted.ExpectedValue = "1";
            Import_Ins_Inserted.Name = "Import_Ins_Inserted";
            Import_Ins_Inserted.NullExpected = false;
            Import_Ins_Inserted.ResultSet = 1;
            Import_Ins_Inserted.RowNumber = 1;
            // 
            // Import_Ins_Different
            // 
            Import_Ins_Different.Enabled = true;
            Import_Ins_Different.Name = "Import_Ins_Different";
            Import_Ins_Different.ResultSet = 5;
            // 
            // Import_Upd_Updated
            // 
            Import_Upd_Updated.ColumnNumber = 1;
            Import_Upd_Updated.Enabled = true;
            Import_Upd_Updated.ExpectedValue = "1";
            Import_Upd_Updated.Name = "Import_Upd_Updated";
            Import_Upd_Updated.NullExpected = false;
            Import_Upd_Updated.ResultSet = 6;
            Import_Upd_Updated.RowNumber = 1;
            // 
            // Import_Upd_Inserted
            // 
            Import_Upd_Inserted.ColumnNumber = 2;
            Import_Upd_Inserted.Enabled = true;
            Import_Upd_Inserted.ExpectedValue = "0";
            Import_Upd_Inserted.Name = "Import_Upd_Inserted";
            Import_Upd_Inserted.NullExpected = false;
            Import_Upd_Inserted.ResultSet = 6;
            Import_Upd_Inserted.RowNumber = 1;
            // 
            // Import_Upd_Different
            // 
            Import_Upd_Different.Enabled = true;
            Import_Upd_Different.Name = "Import_Upd_Different";
            Import_Upd_Different.ResultSet = 10;
            // 
            // Import_Upd2_Updated
            // 
            Import_Upd2_Updated.ColumnNumber = 1;
            Import_Upd2_Updated.Enabled = true;
            Import_Upd2_Updated.ExpectedValue = "0";
            Import_Upd2_Updated.Name = "Import_Upd2_Updated";
            Import_Upd2_Updated.NullExpected = false;
            Import_Upd2_Updated.ResultSet = 11;
            Import_Upd2_Updated.RowNumber = 1;
            // 
            // Import_Upd2_Inserted
            // 
            Import_Upd2_Inserted.ColumnNumber = 2;
            Import_Upd2_Inserted.Enabled = true;
            Import_Upd2_Inserted.ExpectedValue = "0";
            Import_Upd2_Inserted.Name = "Import_Upd2_Inserted";
            Import_Upd2_Inserted.NullExpected = false;
            Import_Upd2_Inserted.ResultSet = 11;
            Import_Upd2_Inserted.RowNumber = 1;
            // 
            // FullNameTestData
            // 
            this.FullNameTestData.PosttestAction = null;
            this.FullNameTestData.PretestAction = null;
            this.FullNameTestData.TestAction = FullNameTest_TestAction;
            // 
            // RankCityTestData
            // 
            this.RankCityTestData.PosttestAction = null;
            this.RankCityTestData.PretestAction = null;
            this.RankCityTestData.TestAction = RankCityTest_TestAction;
            // 
            // OwnerTestData
            // 
            this.OwnerTestData.PosttestAction = null;
            this.OwnerTestData.PretestAction = null;
            this.OwnerTestData.TestAction = OwnerTest_TestAction;
            // 
            // DublicatePhoneData
            // 
            this.DublicatePhoneData.PosttestAction = null;
            this.DublicatePhoneData.PretestAction = null;
            this.DublicatePhoneData.TestAction = DublicatePhone_TestAction;
            // 
            // DublicateEmailData
            // 
            this.DublicateEmailData.PosttestAction = null;
            this.DublicateEmailData.PretestAction = null;
            this.DublicateEmailData.TestAction = DublicateEmail_TestAction;
            // 
            // ImportData
            // 
            this.ImportData.PosttestAction = null;
            this.ImportData.PretestAction = null;
            this.ImportData.TestAction = Import_TestAction;
            // 
            // ListImportDateData
            // 
            this.ListImportDateData.PosttestAction = null;
            this.ListImportDateData.PretestAction = null;
            this.ListImportDateData.TestAction = ListImportDate_TestAction;
            // 
            // ListImportDate_TestAction
            // 
            ListImportDate_TestAction.Conditions.Add(scalarValueCondition1);
            ListImportDate_TestAction.Conditions.Add(scalarValueCondition2);
            resources.ApplyResources(ListImportDate_TestAction, "ListImportDate_TestAction");
            // 
            // scalarValueCondition1
            // 
            scalarValueCondition1.ColumnNumber = 1;
            scalarValueCondition1.Enabled = true;
            scalarValueCondition1.ExpectedValue = null;
            scalarValueCondition1.Name = "scalarValueCondition1";
            scalarValueCondition1.NullExpected = true;
            scalarValueCondition1.ResultSet = 5;
            scalarValueCondition1.RowNumber = 1;
            // 
            // scalarValueCondition2
            // 
            scalarValueCondition2.ColumnNumber = 1;
            scalarValueCondition2.Enabled = true;
            scalarValueCondition2.ExpectedValue = "1";
            scalarValueCondition2.Name = "scalarValueCondition2";
            scalarValueCondition2.NullExpected = false;
            scalarValueCondition2.ResultSet = 6;
            scalarValueCondition2.RowNumber = 1;
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

        private SqlDatabaseTestActions FullNameTestData;
        private SqlDatabaseTestActions RankCityTestData;
        private SqlDatabaseTestActions OwnerTestData;
        private SqlDatabaseTestActions DublicatePhoneData;
        private SqlDatabaseTestActions DublicateEmailData;
        private SqlDatabaseTestActions ImportData;
        private SqlDatabaseTestActions ListImportDateData;
    }
}
