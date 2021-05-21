using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Messages;
using Database;

namespace TestProj
{
    public partial class FrmSWIFT : Form
    {
        BlockHeader BH = new BlockHeader();
        DBUtils dbu = new DBUtils();
        Util mUtil = new Util();
        Object MsgContainer;
        readonly bool DEBUG_ON = false;
        
        //private SqlConnection cnn = null;

        public FrmSWIFT() => InitializeComponent();
   
        private void mnuFileExit_Click(object sender, EventArgs e) => Application.Exit();
    
        private void mnuFileOpen_Click(object sender, EventArgs e)
        {
            string fileName = null;
            MsgContainer = null;
        
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse Text Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "All Files",
                Filter = "All Files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };
        
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog1.FileName;
                getData(fileName);
            }
        }

        private void mnuFileOpenDB_Click(object sender, EventArgs e)
        {
            //string connetionString;
            
            //connetionString = @"Data Source=DESKTOP-SRU018M;Initial Catalog=SWIFT;USER ID=user1;Password=U1234";
            //cnn = new SqlConnection(connetionString);
            try
            {
                //cnn.Open();
                dbu.DBOpen();
                btnSaveData.Enabled = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region DISPLAY_FUNCTIONS
        private void getData(string fileName)
        {
            clearData();
            if (BH.ParseFile(fileName) == true)
            {
                parseBlock4(BH.Block_4, BH.MessageType);

                fillMessagePage();
                fillBlock1Page();
                fillBlock2Page();
                fillBlock3Page();
                fillBlock4Page(BH.MessageType);
                fillBlock5Page();
            }
            fillErrorsPage();
        }
        
        private void clearData()
        {
            txtErrors.Text = "";
            txtScope.Text = "";
        }
    
        private void parseBlock4(string message, string messageType)
        {
            switch(messageType)
            {
                case "101":
                    MsgContainer = new MT101(message);
                    break;
                case "105":
                    MsgContainer = new MT105(message);
                    break;
                case "110":
                    MsgContainer = new MT110(message);
                    break;
                case "111":
                    MsgContainer = new MT111(message);
                    break;
                case "112":
                    MsgContainer = new MT112(message);
                    break;
                case "320":
                    MsgContainer = new MT320(message);
                    break;
                default:
                    break;
            }
        }
    
        private void fillMessagePage()
        {
            txtMessageBlock1.Text = BH.Block_1;
            txtMessageBlock2.Text = BH.Block_2;
            txtMessageBlock3.Text = BH.Block_3;
            txtMessageBlock4.Text = BH.Block_4;
            txtMessageBlock5.Text = BH.Block_5;
        }
    
        private void fillBlock1Page()
        {
            txtAppID.Text = BH.ApplicationID;
            txtServiceID.Text = BH.ServiceID;
            txtLTAddress.Text = BH.LTAddress;
            txtBICCode.Text = BH.BICCode;
            txtLTCode.Text = BH.LogicalTerminalCode;
            txtBICBranchCode.Text = BH.BICBranchCode;
            txtSessionNumber.Text = BH.SessionNumber;
            txtSequenceNumber.Text = BH.SequenceNumber;
        }
    
        private void fillBlock2Page()
        {
            if(BH.InputOutputID.Equals("I") == true)
            {
                lblBlock2IO.Text = "Inbound Message";
                pnlInbound.Visible = false;
                pnlInbound.Visible = true;
                
                txtInIOId.Text = BH.InputOutputID;
                txtInMessageType.Text = BH.MessageType;
                txtInDestinationAddress.Text = BH.DestinationAddress;
                txtInPriority.Text = BH.Priority;
                txtInDeliveryMonitoring.Text = BH.DeliveryMonitoring;
                txtInObsolescencePeriod.Text = BH.ObsolescencePeriod;
            }
            else
            {
                lblBlock2IO.Text = "Outbound Message";
                pnlInbound.Visible = false;
                pnlInbound.Visible = true;
                
                txtOutIOId.Text = BH.InputOutputID;
                txtOutMessageType.Text = BH.MessageType;
                txtOutInputTime.Text = BH.InputTime;
                txtOutMIR.Text = BH.MIR;
                txtOutMIRSenderDate.Text = BH.MIRSenderDate;
                txtOutMIRLTAddress.Text = BH.MIRLTAddress;
                txtOutMIRBICCode.Text = BH.MIRBICCode;
                txtOutMIRLogicalTerminalCode.Text = BH.MIRLTCode;
                txtOutMIRBICBranchCode.Text = BH.MIRBICBranchCode;
                txtOutMIRSessionNumber.Text = BH.MIRSessNum;
                txtOutMIRSequenceNumber.Text = BH.MIRSessNum;
                txtOutOutputDate.Text = BH.OutputDate;
                txtOutOutputTime.Text = BH.OutputTime;
                txtOutPriority.Text = BH.Priority;
            }
        }

        private void fillBlock3Page()
        {
            txtTAG103.Text = BH.TAG103_ServiceID;
            txtTAG113.Text = BH.TAG113_BankingPriority;
            txtTAG108.Text = BH.TAG108_MUR;
            txtTAG119.Text = BH.TAG119_ValidationFlag;
            txtTAG423.Text = BH.TAG423_BalanceCheckPoint;
            txtTAG106.Text = BH.TAG106_MIR;
            txtTAG424.Text = BH.TAG424_RelatedReference;
            txtTAG111.Text = BH.TAG111_ServiceTypeID;
            txtTAG121.Text = BH.TAG121_UniqueTranReference;
            txtTAG115.Text = BH.TAG115_AddresseeInfo;
            txtTAG165.Text = BH.TAG165_PaymentRIR;
            txtTAG433.Text = BH.TAG433_SanctionsSIR;
            txtTAG434.Text = BH.TAG434_PaymentCIR;
        }

        private void fillBlock4Page(string messageType)
        {
            switch (messageType)
            {
                case "101":
                    display_101_Data();
                    break;
                case "105":
                    display_105_Data();
                    break;
                case "111":
                    display_111_Data();
                    break;
                case "112":
                    display_112_Data();
                    break;
                case "320":
                    display_320_Data();
                    break;
                default:
                    break;
            }
        }

        private void fillBlock5Page()
        {
            txtChecksum.Text = BH.Checksum;
            txtTestTrain.Text = BH.TNGMessage;
            txtPDE.Text = BH.PDE;
            txtPDETime.Text = BH.PDETime;
            txtPDEMir.Text = BH.PDEMir;
            txtPDEMirDate.Text = BH.PDEMirDate;
            txtPDEMirLTId.Text = BH.PDEMirLTId;
            txtPDEMirSessNum.Text = BH.PDEMirSessionNum;
            txtPDEMirSeqNum.Text = BH.PDEMirSequenceNum;
            txtDelayedMessage.Text = BH.DLM;
            txtMessageReference.Text = BH.MRF;
            txtMRFDate.Text = BH.MRFDate;
            txtMRFTime.Text = BH.MRFTime;
            txtMRFMir.Text = BH.MRFMir;
            txtPDM.Text = BH.PDM;
            txtPDMTime.Text = BH.PDMTime;
            txtPDMMor.Text = BH.PDMMor;
            txtPDMMorDate.Text = BH.PDMMorDate;
            txtPDMMorLTId.Text = BH.PDMMorLTId;
            txtPDMMorSessNum.Text = BH.PDMMorSessionNum;
            txtPDMMorSeqNum.Text = BH.PDMMorSequenceNum;
            txtSYS.Text = BH.SYS;
            txtSYSTime.Text = BH.SYSTime;
            txtSYSMor.Text = BH.SYSMor;
            txtSYSMorDate.Text = BH.SYSMorDate;
            txtSYSMorLTId.Text = BH.SYSMorLTId;
            txtSYSMorSessNum.Text = BH.SYSMorSessionNum;
            txtSYSMorSeqNum.Text = BH.SYSMorSequenceNum;
        }

        private void display_101_Data()
        {
            int sections = ((MT101)MsgContainer).numOfSequences;
            MT101 fields = ((MT101)MsgContainer);
            List<TagData<string, string, string, string, int>> fieldData = new List<TagData<string, string, string, string, int>>();
            DataTable dt = new DataTable();

            dt.Columns.Add("Tag Name", typeof(string));
            dt.Columns.Add("Tag ID", typeof(string));
            dt.Columns.Add("Tag Value", typeof(string));
            dt.Columns.Add("Madatory", typeof(string));
            dt.Columns.Add("Present", typeof(int));

            for (int s = 0; s < sections; s++)
            {
                fieldData = fields[s];
                foreach (TagData<string, string, string, string, int> t in fieldData)
                {
                    dt.Rows.Add(new object[] { t.Name, t.Tag, t.Value, t.Mandatory, t.Present });
                }
            }
            dgView.DataSource = dt;
            foreach (DataGridViewRow row in dgView.Rows)
            {
                if (row.Cells["Present"].Value == null)
                {
                    continue;
                }
                if (row.Cells["Tag ID"].Value.ToString().Contains("15"))
                {
                    row.DefaultCellStyle.BackColor = Color.Yellow;
                }
                else if (row.Cells["Present"].Value.Equals(0))
                {
                    row.DefaultCellStyle.BackColor = Color.Gray;
                }
            }
            dgView.Columns["Present"].Visible = DEBUG_ON;

            txtScope.Text = ((MT101)MsgContainer).Scope;
        }

        private void display_105_Data()
        {
            int sections = ((MT105)MsgContainer).numOfSequences;
            MT105 fields = ((MT105)MsgContainer);
            List<TagData<string, string, string, string, int>> fieldData = new List<TagData<string, string, string, string, int>>();
            DataTable dt = new DataTable();

            dt.Columns.Add("Tag Name", typeof(string));
            dt.Columns.Add("Tag ID", typeof(string));
            dt.Columns.Add("Tag Value", typeof(string));
            dt.Columns.Add("Madatory", typeof(string));
            dt.Columns.Add("Present", typeof(int));

            for (int s = 0; s < sections; s++)
            {
                fieldData = fields[s];
                foreach (TagData<string, string, string, string, int> t in fieldData)
                {
                    dt.Rows.Add(new object[] { t.Name, t.Tag, t.Value, t.Mandatory, t.Present });
                }
            }
            dgView.DataSource = dt;
            foreach (DataGridViewRow row in dgView.Rows)
            {
                if (row.Cells["Present"].Value == null)
                {
                    continue;
                }
                if (row.Cells["Tag ID"].Value.ToString().Contains("15"))
                {
                    row.DefaultCellStyle.BackColor = Color.Yellow;
                }
                else if (row.Cells["Present"].Value.Equals(0))
                {
                    row.DefaultCellStyle.BackColor = Color.Gray;
                }
            }
            dgView.Columns["Present"].Visible = DEBUG_ON;

            txtScope.Text = ((MT105)MsgContainer).Scope;
        }

        private void display_110_Data()
        {
            int sections = ((MT110)MsgContainer).numOfSequences;
            MT110 fields = ((MT110)MsgContainer);
            List<TagData<string, string, string, string, int>> fieldData = new List<TagData<string, string, string, string, int>>();
            DataTable dt = new DataTable();

            dt.Columns.Add("Tag Name", typeof(string));
            dt.Columns.Add("Tag ID", typeof(string));
            dt.Columns.Add("Tag Value", typeof(string));
            dt.Columns.Add("Madatory", typeof(string));
            dt.Columns.Add("Present", typeof(int));

            for (int s = 0; s < sections; s++)
            {
                fieldData = fields[s];
                foreach (TagData<string, string, string, string, int> t in fieldData)
                {
                    dt.Rows.Add(new object[] { t.Name, t.Tag, t.Value, t.Mandatory, t.Present });
                }
            }
            dgView.DataSource = dt;
            foreach (DataGridViewRow row in dgView.Rows)
            {
                if (row.Cells["Present"].Value == null)
                {
                    continue;
                }
                if (row.Cells["Tag ID"].Value.ToString().Contains("15"))
                {
                    row.DefaultCellStyle.BackColor = Color.Yellow;
                }
                else if (row.Cells["Present"].Value.Equals(0))
                {
                    row.DefaultCellStyle.BackColor = Color.Gray;
                }
            }
            dgView.Columns["Present"].Visible = DEBUG_ON;

            txtScope.Text = ((MT110)MsgContainer).Scope;
        }

        private void display_111_Data()
        {
            int sections = ((MT111)MsgContainer).numOfSequences;
            MT111 fields = ((MT111)MsgContainer);
            List<TagData<string, string, string, string, int>> fieldData = new List<TagData<string, string, string, string, int>>();
            DataTable dt = new DataTable();

            dt.Columns.Add("Tag Name", typeof(string));
            dt.Columns.Add("Tag ID", typeof(string));
            dt.Columns.Add("Tag Value", typeof(string));
            dt.Columns.Add("Madatory", typeof(string));
            dt.Columns.Add("Present", typeof(int));

            for (int s = 0; s < sections; s++)
            {
                fieldData = fields[s];
                foreach (TagData<string, string, string, string, int> t in fieldData)
                {
                    dt.Rows.Add(new object[] { t.Name, t.Tag, t.Value, t.Mandatory, t.Present });
                }
            }
            dgView.DataSource = dt;
            foreach (DataGridViewRow row in dgView.Rows)
            {
                if (row.Cells["Present"].Value == null)
                {
                    continue;
                }
                if (row.Cells["Tag ID"].Value.ToString().Contains("15"))
                {
                    row.DefaultCellStyle.BackColor = Color.Yellow;
                }
                else if (row.Cells["Present"].Value.Equals(0))
                {
                    row.DefaultCellStyle.BackColor = Color.Gray;
                }
            }
            dgView.Columns["Present"].Visible = DEBUG_ON;

            txtScope.Text = ((MT111)MsgContainer).Scope;
        }

        private void display_112_Data()
        {
            int sections = ((MT112)MsgContainer).numOfSequences;
            MT112 fields = ((MT112)MsgContainer);
            List<TagData<string, string, string, string, int>> fieldData = new List<TagData<string, string, string, string, int>>();
            DataTable dt = new DataTable();

            dt.Columns.Add("Tag Name", typeof(string));
            dt.Columns.Add("Tag ID", typeof(string));
            dt.Columns.Add("Tag Value", typeof(string));
            dt.Columns.Add("Madatory", typeof(string));
            dt.Columns.Add("Present", typeof(int));
            
            for (int s = 0; s < sections; s++)
            {
                fieldData = fields[s];
                foreach (TagData<string, string, string, string, int> t in fieldData)
                {
                    dt.Rows.Add(new object[] { t.Name, t.Tag, t.Value, t.Mandatory, t.Present });
                }
            }
            dgView.DataSource = dt;
            foreach (DataGridViewRow row in dgView.Rows)
            {
                if (row.Cells["Present"].Value == null)
                {
                    continue;
                }
                if (row.Cells["Tag ID"].Value.ToString().Contains("15"))
                {
                    row.DefaultCellStyle.BackColor = Color.Yellow;
                }
                else if (row.Cells["Present"].Value.Equals(0))
                {
                    row.DefaultCellStyle.BackColor = Color.Gray;
                }
            }
            dgView.Columns["Present"].Visible = DEBUG_ON;
            
            txtScope.Text = ((MT112)MsgContainer).Scope;
        }

        private void display_320_Data()
        {
            int sections = ((MT320)MsgContainer).numOfSequences;
            MT320 fields = ((MT320)MsgContainer);
            List<TagData<string, string, string, string, int>> fieldData = new List<TagData<string, string, string, string, int>>();
            DataTable dt = new DataTable();

            dt.Columns.Add("Tag Name", typeof(string));
            dt.Columns.Add("Tag ID", typeof(string));
            dt.Columns.Add("Tag Value", typeof(string));
            dt.Columns.Add("Madatory", typeof(string));
            dt.Columns.Add("Present", typeof(int));

            for (int s = 0; s < sections; s++)
            {
                fieldData = fields[s];
                foreach (TagData<string, string, string, string, int> t in fieldData)
                {
                    dt.Rows.Add(new object[] { t.Name, t.Tag, t.Value, t.Mandatory, t.Present });
                }
            }
            dgView.DataSource = dt;
            foreach (DataGridViewRow row in dgView.Rows)
            {
                if (row.Cells["Present"].Value == null)
                {
                    continue;
                }
                if (row.Cells["Tag ID"].Value.ToString().Contains("15"))
                {
                    row.DefaultCellStyle.BackColor = Color.Yellow;
                }
                else if (row.Cells["Present"].Value.Equals(0))
                {
                    row.DefaultCellStyle.BackColor = Color.Gray;
                }
            }
            dgView.Columns["Present"].Visible = DEBUG_ON;
            txtScope.Text = ((MT320)MsgContainer).Scope;
        }

        private void fillErrorsPage()
        {
            List<string> mishaps = null;
            txtErrors.Text = "";
            foreach (string err in BH.Errors)
            {
                txtErrors.Text += err + "\r\n";
            }
         
            switch(BH.MessageType)
            {
                case "101":
                    mishaps = ((MT101)MsgContainer).Anomalies;
                    break;
                case "105":
                    mishaps = ((MT105)MsgContainer).Anomalies;
                    break;
                case "110":
                    mishaps = ((MT110)MsgContainer).Anomalies;
                    break;
                case "111":
                    mishaps = ((MT111)MsgContainer).Anomalies;
                    break;
                case "112":
                    mishaps = ((MT112)MsgContainer).Anomalies;
                    break;
                case "320":
                    mishaps = ((MT320)MsgContainer).Anomalies;
                    break;
                default:
                    break;
            }

            if (mishaps != null)
            {
                foreach (string err in mishaps)
                {
                    txtErrors.Text += err + "\r\n";
                }
            }
        }
        #endregion

        private void FrmSWIFT_FormClosing(object sender, FormClosingEventArgs e)
        {
            dbu.DBClose();
        }

        private void btnSaveData_Click(object sender, EventArgs e)
        {
            string mtype = BH.MessageType;

            try
            {
                switch (mtype)
                {
                    case "105":
                        ((MT105)MsgContainer).saveRecord(BH);
                        break;
                    case "110":
                        ((MT110)MsgContainer).saveRecord(BH);
                        break;
                    case "111":
                        ((MT111)MsgContainer).saveRecord(BH);
                        break;
                    case "112":
                        ((MT112)MsgContainer).saveRecord(BH);
                        break;
                    case "320":
                        ((MT320)MsgContainer).saveRecord(BH);
                        break;
                    default:
                        break;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            string mtype = BH.MessageType;

            try
            {
                switch (mtype)
                {
                    case "105":
                        ((MT105)MsgContainer).testFunctions();
                        break;
                    case "110":
                        ((MT110)MsgContainer).testFunctions();
                        break;
                    case "111":
                        ((MT111)MsgContainer).testFunctions();
                        break;
                    case "112":
                        ((MT112)MsgContainer).testFunctions();
                        break;
                    case "320":
                        ((MT320)MsgContainer).testFunctions();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
