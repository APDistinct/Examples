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
    public class MergeUsersTests : SqlDatabaseTestClass
    {

        public MergeUsersTests() {
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
        public void Merge_MasterWithotTransport() {
            SqlDatabaseTestActions testActions = this.Merge_MasterWithotTransportData;
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
        public void Merge_MasterHasTransport() {
            SqlDatabaseTestActions testActions = this.Merge_MasterHasTransportData;
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
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction Merge_MasterWithotTransport_TestAction;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MergeUsersTests));
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition CheckOutput;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition CheckMsgFromMaster;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition CheckMsgFromDonor;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition CheckMsgToMaster;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition CheckMsgToDonor;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.NotEmptyResultSetCondition CheckMasterEnabled;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.NotEmptyResultSetCondition CheckDonorDisabled;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition CheckTransports;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction Merge_MasterHasTransport_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition M1_Output;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition M1_Output_Value;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition M1_Messages;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition M1_DonorMessages;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition M1_UsersEnableFLag;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition M1_Transports;
            this.Merge_MasterWithotTransportData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.Merge_MasterHasTransportData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            Merge_MasterWithotTransport_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            CheckOutput = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            CheckMsgFromMaster = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            CheckMsgFromDonor = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            CheckMsgToMaster = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            CheckMsgToDonor = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            CheckMasterEnabled = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.NotEmptyResultSetCondition();
            CheckDonorDisabled = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.NotEmptyResultSetCondition();
            CheckTransports = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            Merge_MasterHasTransport_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            M1_Output = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            M1_Output_Value = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            M1_Messages = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            M1_DonorMessages = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            M1_UsersEnableFLag = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            M1_Transports = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            // 
            // Merge_MasterWithotTransport_TestAction
            // 
            Merge_MasterWithotTransport_TestAction.Conditions.Add(CheckOutput);
            Merge_MasterWithotTransport_TestAction.Conditions.Add(CheckMsgFromMaster);
            Merge_MasterWithotTransport_TestAction.Conditions.Add(CheckMsgFromDonor);
            Merge_MasterWithotTransport_TestAction.Conditions.Add(CheckMsgToMaster);
            Merge_MasterWithotTransport_TestAction.Conditions.Add(CheckMsgToDonor);
            Merge_MasterWithotTransport_TestAction.Conditions.Add(CheckMasterEnabled);
            Merge_MasterWithotTransport_TestAction.Conditions.Add(CheckDonorDisabled);
            Merge_MasterWithotTransport_TestAction.Conditions.Add(CheckTransports);
            resources.ApplyResources(Merge_MasterWithotTransport_TestAction, "Merge_MasterWithotTransport_TestAction");
            // 
            // CheckOutput
            // 
            CheckOutput.ColumnNumber = 1;
            CheckOutput.Enabled = true;
            CheckOutput.ExpectedValue = "1";
            CheckOutput.Name = "CheckOutput";
            CheckOutput.NullExpected = false;
            CheckOutput.ResultSet = 1;
            CheckOutput.RowNumber = 1;
            // 
            // CheckMsgFromMaster
            // 
            CheckMsgFromMaster.ColumnNumber = 1;
            CheckMsgFromMaster.Enabled = true;
            CheckMsgFromMaster.ExpectedValue = "1";
            CheckMsgFromMaster.Name = "CheckMsgFromMaster";
            CheckMsgFromMaster.NullExpected = false;
            CheckMsgFromMaster.ResultSet = 2;
            CheckMsgFromMaster.RowNumber = 1;
            // 
            // CheckMsgFromDonor
            // 
            CheckMsgFromDonor.Enabled = true;
            CheckMsgFromDonor.Name = "CheckMsgFromDonor";
            CheckMsgFromDonor.ResultSet = 3;
            // 
            // CheckMsgToMaster
            // 
            CheckMsgToMaster.ColumnNumber = 1;
            CheckMsgToMaster.Enabled = true;
            CheckMsgToMaster.ExpectedValue = "1";
            CheckMsgToMaster.Name = "CheckMsgToMaster";
            CheckMsgToMaster.NullExpected = false;
            CheckMsgToMaster.ResultSet = 4;
            CheckMsgToMaster.RowNumber = 1;
            // 
            // CheckMsgToDonor
            // 
            CheckMsgToDonor.Enabled = true;
            CheckMsgToDonor.Name = "CheckMsgToDonor";
            CheckMsgToDonor.ResultSet = 5;
            // 
            // CheckMasterEnabled
            // 
            CheckMasterEnabled.Enabled = true;
            CheckMasterEnabled.Name = "CheckMasterEnabled";
            CheckMasterEnabled.ResultSet = 6;
            // 
            // CheckDonorDisabled
            // 
            CheckDonorDisabled.Enabled = true;
            CheckDonorDisabled.Name = "CheckDonorDisabled";
            CheckDonorDisabled.ResultSet = 7;
            // 
            // CheckTransports
            // 
            CheckTransports.Enabled = true;
            CheckTransports.Name = "CheckTransports";
            CheckTransports.ResultSet = 8;
            CheckTransports.RowCount = 1;
            // 
            // Merge_MasterHasTransport_TestAction
            // 
            Merge_MasterHasTransport_TestAction.Conditions.Add(M1_Output);
            Merge_MasterHasTransport_TestAction.Conditions.Add(M1_Output_Value);
            Merge_MasterHasTransport_TestAction.Conditions.Add(M1_Messages);
            Merge_MasterHasTransport_TestAction.Conditions.Add(M1_DonorMessages);
            Merge_MasterHasTransport_TestAction.Conditions.Add(M1_UsersEnableFLag);
            Merge_MasterHasTransport_TestAction.Conditions.Add(M1_Transports);
            resources.ApplyResources(Merge_MasterHasTransport_TestAction, "Merge_MasterHasTransport_TestAction");
            // 
            // M1_Output
            // 
            M1_Output.Enabled = true;
            M1_Output.Name = "M1_Output";
            M1_Output.ResultSet = 1;
            M1_Output.RowCount = 1;
            // 
            // M1_Output_Value
            // 
            M1_Output_Value.ColumnNumber = 1;
            M1_Output_Value.Enabled = true;
            M1_Output_Value.ExpectedValue = "1";
            M1_Output_Value.Name = "M1_Output_Value";
            M1_Output_Value.NullExpected = false;
            M1_Output_Value.ResultSet = 1;
            M1_Output_Value.RowNumber = 1;
            // 
            // M1_Messages
            // 
            M1_Messages.Enabled = true;
            M1_Messages.Name = "M1_Messages";
            M1_Messages.ResultSet = 2;
            M1_Messages.RowCount = 2;
            // 
            // M1_DonorMessages
            // 
            M1_DonorMessages.Enabled = true;
            M1_DonorMessages.Name = "M1_DonorMessages";
            M1_DonorMessages.ResultSet = 3;
            // 
            // M1_UsersEnableFLag
            // 
            M1_UsersEnableFLag.Enabled = true;
            M1_UsersEnableFLag.Name = "M1_UsersEnableFLag";
            M1_UsersEnableFLag.ResultSet = 4;
            M1_UsersEnableFLag.RowCount = 2;
            // 
            // M1_Transports
            // 
            M1_Transports.Enabled = true;
            M1_Transports.Name = "M1_Transports";
            M1_Transports.ResultSet = 5;
            M1_Transports.RowCount = 1;
            // 
            // Merge_MasterWithotTransportData
            // 
            this.Merge_MasterWithotTransportData.PosttestAction = null;
            this.Merge_MasterWithotTransportData.PretestAction = null;
            this.Merge_MasterWithotTransportData.TestAction = Merge_MasterWithotTransport_TestAction;
            // 
            // Merge_MasterHasTransportData
            // 
            this.Merge_MasterHasTransportData.PosttestAction = null;
            this.Merge_MasterHasTransportData.PretestAction = null;
            this.Merge_MasterHasTransportData.TestAction = Merge_MasterHasTransport_TestAction;
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

        private SqlDatabaseTestActions Merge_MasterWithotTransportData;
        private SqlDatabaseTestActions Merge_MasterHasTransportData;
    }
}
