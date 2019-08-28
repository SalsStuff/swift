using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Messages
{
   // Validation Rules are from : https://www.paiementor.com/swift-mt-message-structure-blocks-1-to-5/
   public class BlockHeader
   {
      #region DEFINES
      static private int MAX_SWIFT_MSG_LEN = 4096;
      static private int MIN_BLOCK_1_LEN = 29;
      static private int MAX_BLOCK_1_LEN = 31;
      static private int MIN_BLOCK_2i_LEN = 25;
      static private int MAX_BLOCK_2i_LEN = 27;
      static private int MIN_BLOCK_2o_LEN = 29;
      static private int MAX_BLOCK_2o_LEN = 53;
      static private int MIN_BLOCK_3_LEN = 29;
      static private int MAX_BLOCK_3_LEN = 31;
      static private int MIN_BLOCK_5_LEN = 24;
      static private int MAX_BLOCK_5_LEN = 192;
      static private int MAX_BLOCK_5_SECTIONS = 7;

      #endregion


      #region VARIABLES
      private string block_1 = null;
      private string block_2 = null;
      private string block_3 = null;
      private string block_4 = null;
      private string block_5 = null;

      protected List<string> errors = new List<string>();

      // BASIC HEADER member variables
      private string m_ApplicationID_m = null;
      private string m_ServiceID_m = null;
      private string m_LTAddress_m = null;
      private string m_BICCode = null;
      private string m_LogicalTerminalCode = null;
      private string m_BICBranchCode = null;
      private string m_SessionNumber_m = null;
      private string m_SequenceNumber_m = null;

      // APPLICATION HEADER member variables
      private string m_IOID_m = null;
      private string m_MessageType_m = null;
      private string m_DestinationAddress_m = null;
      private string m_Priority_o = null;
      private string m_DeliveryMonitoring_o = null;
      private string m_ObsolescencePeriod_o = null;
      private string m_InputTime_m = null;
      private string m_MIR_m = null;
      private string m_MIRSenderDate = null;
      private string m_MIRLTAddress = null;
      private string m_MIRBICCode = null;
      private string m_MIRLTCode = null;
      private string m_MIRBICBranchCode = null;
      private string m_MIRSessNum = null;
      private string m_MIRSeqNum = null;
      private string m_OutputDate_m = null;
      private string m_OutputTime_m = null;

      // TRAILER HEADER member variables
      private string m_Checksum_m = null;
      private string m_TNGMessage_o = null;
      private string m_PDE_o = null;
      private string m_PDE_time = null;
      private string m_PDE_mir = null;
      private string m_PDE_mirDate = null;
      private string m_PDE_mirLTId = null;
      private string m_PDE_mirSessionNum = null;
      private string m_PDE_mirSequenceNum = null;
      private string m_DLM_o = null;
      private string m_MRF_o = null;
      private string m_MRF_date = null;
      private string m_MRF_time = null;
      private string m_MRF_mir = null;
      private string m_PDM_o = null;
      private string m_PDM_time = null;
      private string m_PDM_mor = null;
      private string m_PDM_morDate = null;
      private string m_PDM_morLTId = null;
      private string m_PDM_morSessionNum = null;
      private string m_PDM_morSequenceNum = null;
      private string m_SYS_o = null;
      private string m_SYS_time = null;
      private string m_SYS_mor = null;
      private string m_SYS_morDate = null;
      private string m_SYS_morLTId = null;
      private string m_SYS_morSessionNum = null;
      private string m_SYS_morSequenceNum = null;

      #endregion

      public BlockHeader()
      {
         
      }

      private void initializeVariables()
      {
         // BASIC HEADER member variables
         m_ApplicationID_m = null;
         m_ServiceID_m = null;
         m_LTAddress_m = null;
         m_BICCode = null;
         m_LogicalTerminalCode = null;
         m_BICBranchCode = null;
         m_SessionNumber_m = null;
         m_SequenceNumber_m = null;

         // APPLICATION HEADER member variables
         m_IOID_m = null;
         m_MessageType_m = null;
         m_DestinationAddress_m = null;
         m_Priority_o = null;
         m_DeliveryMonitoring_o = null;
         m_ObsolescencePeriod_o = null;
         m_InputTime_m = null;
         m_MIR_m = null;
         m_MIRSenderDate = null;
         m_MIRLTAddress = null;
         m_MIRBICCode = null;
         m_MIRLTCode = null;
         m_MIRBICBranchCode = null;
         m_MIRSessNum = null;
         m_MIRSeqNum = null;
         m_OutputDate_m = null;
         m_OutputTime_m = null;

         // TRAILER HEADER member variables
         m_Checksum_m = null;
         m_TNGMessage_o = null;
         m_PDE_o = null;
         m_PDE_time = null;
         m_PDE_mir = null;
         m_PDE_mirDate = null;
         m_PDE_mirLTId = null;
         m_PDE_mirSessionNum = null;
         m_PDE_mirSequenceNum = null;
         m_DLM_o = null;
         m_MRF_o = null;
         m_MRF_date = null;
         m_MRF_time = null;
         m_MRF_mir = null;
         m_PDM_o = null;
         m_PDM_time = null;
         m_PDM_mor = null;
         m_PDM_morDate = null;
         m_PDM_morLTId = null;
         m_PDM_morSessionNum = null;
         m_PDM_morSequenceNum = null;
         m_SYS_o = null;
         m_SYS_time = null;
         m_SYS_mor = null;
         m_SYS_morDate = null;
         m_SYS_morLTId = null;
         m_SYS_morSessionNum = null;
         m_SYS_morSequenceNum = null;
      }

      private void parseBlocks(string msg)
      {
         int start = 0;
         //int end = 0;
         int i = 0;
         string[] tempStrs = { "", "", "", "", "" };
         string bStr = "";


         foreach (char c in msg)
         {
            if( c.Equals('{') == true)
            {
               start++;
            }

            if( c.Equals('}') == true)
            {
               //end++;
               start--;
            }

            //if (start - end == 0)
            if(start == 0)
            {
               //start = end = 0;
               start = 0;
               tempStrs[i] = bStr + c;
               bStr = "";
               i++;
            }
            else
            {
               bStr += c;
            }
         }

         int length = tempStrs.Length;

         for (i = 0; i < length; i++)
         {
            string temp = null;

            if (tempStrs[i].Length >= 3)
               temp = tempStrs[i].Substring(0, 3);
            else
               continue;

            if (temp.Equals("{1:") == true)
            {
               block_1 = tempStrs[i];
            }
            else if (temp.Equals("{2:") == true)
            {
               block_2 = tempStrs[i];
            }
            else if (temp.Equals("{3:") == true)
            {
               block_3 = tempStrs[i];
            }
            else if (temp.Equals("{4:") == true)
            {
               block_4 = tempStrs[i];
            }
            else if (temp.Equals("{5:") == true)
            {
               block_5 = tempStrs[i];
            }
         }

      }

      public void parseFile(string fileName)
      {
         string msgFile = File.ReadAllText(fileName);

         errors.Clear();
         initializeVariables();
         if (validMsgLength(msgFile) == false)
            return;

         parseBlocks(msgFile);

         validateBlocks();
      }

      #region Get_Set

      public string Block_1
      {
         get { return block_1; }
      }

      public string Block_2
      {
         get { return block_2; }
      }

      public string Block_3
      {
         get { return block_3; }
      }

      public string Block_4
      {
         get { return block_4; }
      }

      public string Block_5
      {
         get { return block_5; }
      }

      public List<string> Errors
      {
         get { return errors; }
      }

      #region BASIC_HEADER_GETS
      public string ApplicationID
      {
         get { return m_ApplicationID_m;  }
      }

      public string ServiceID
      {
         get { return m_ServiceID_m; }
      }

      public string LTAddress
      {
         get { return m_LTAddress_m; }
      }

      public string BICCode
      {
         get { return m_BICCode; }
      }

      public string LogicalTerminalCode
      {
         get { return m_LogicalTerminalCode; }
      }
      
      public string BICBranchCode
      {
         get { return m_BICBranchCode; }
      }
      
      public string SessionNumber
      {
         get { return m_SessionNumber_m; }
      }

      public string SequenceNumber
      {
         get { return m_SequenceNumber_m; }
      }

      #endregion

      #region APPLICATION_HEADER_GETS
      public string InputOutputID
      {
         get { return m_IOID_m; }
      }

      public string MessageType
      {
         get { return m_MessageType_m; }
      }

      public string DestinationAddress
      {
         get { return m_DestinationAddress_m; }
      }

      public string Priority
      {
         get { return m_Priority_o; }
      }

      public string DeliveryMonitoring
      {
         get { return m_DeliveryMonitoring_o; }
      }

      public string ObsolescencePeriod
      {
         get { return m_ObsolescencePeriod_o; }
      }

      public string InputTime
      {
         get { return m_InputTime_m; }
      }

      public string MIR
      {
         get { return m_MIR_m; }
      }

      public string MIRSenderDate
      {
         get { return m_MIRSenderDate; }
      }

      public string MIRLTAddress
      {
         get { return m_MIRLTAddress; }
      }

      public string MIRBICCode
      {
         get { return m_MIRBICCode; }
      }

      public string MIRLTCode
      {
         get { return m_MIRLTCode; }
      }

      public string MIRBICBranchCode
      {
         get { return m_MIRBICBranchCode; }
      }

      public string MIRSessNum
      {
         get { return m_MIRSessNum; }
      }

      private string MIRSeqNum
      {
         get { return m_MIRSeqNum; }
      }

      public string OutputDate
      {
         get { return m_OutputDate_m; }
      }

      public string OutputTime
      {
         get { return m_OutputTime_m; }
      }

      #endregion

      #region TRAILER_HEADER_GETS
      public string Checksum
      {
         get { return m_Checksum_m; }
      }

      public string TNGMessage
      {
         get { return m_TNGMessage_o; }
      }

      public string PDE
      {
         get { return m_PDE_o; }
      }

      public string PDETime
      {
         get { return m_PDE_time; }
      }

      public string PDEMir
      {
         get { return m_PDE_mir; }
      }

      public string PDEMirDate
      {
         get { return m_PDE_mirDate; }
      }

      public string PDEMirLTId
      {
         get { return m_PDE_mirLTId; }
      }

      public string PDEMirSessionNum
      {
         get { return m_PDE_mirSessionNum; }
      }

      public string PDEMirSequenceNum
      {
         get { return m_PDE_mirSequenceNum; }
      }

      public string DLM
      {
         get { return m_DLM_o; }
      }

      public string MRF
      {
         get { return m_MRF_o; }
      }

      public string MRFDate
      {
         get { return m_MRF_date; }
      }

      public string MRFTime
      {
         get { return m_MRF_time; }
      }

      public string MRFMir
      {
         get { return m_MRF_mir; }
      }

      public string PDM
      {
         get { return m_PDM_o; }
      }

      public string PDMTime
      {
         get { return m_PDM_time; }
      }

      public string PDMMor
      {
         get { return m_PDM_mor; }
      }

      public string PDMMorDate
      {
         get { return m_PDM_morDate; }
      }

      public string PDMMorLTId
      {
         get { return m_PDM_morLTId; }
      }

      public string PDMMorSessionNum
      {
         get { return m_PDM_morSessionNum; }
      }

      public string PDMMorSequenceNum
      {
         get { return m_PDM_morSequenceNum; }
      }

      public string SYS
      {
         get { return m_SYS_o; }
      }

      public string SYSTime
      {
         get { return m_SYS_time; }
      }

      public string SYSMor
      {
         get { return m_SYS_mor; }
      }

      public string SYSMorDate
      {
         get { return m_SYS_morDate; }
      }

      public string SYSMorLTId
      {
         get { return m_SYS_morLTId; }
      }

      public string SYSMorSessionNum
      {
         get { return m_SYS_morSessionNum; }
      }

      public string SYSMorSequenceNum
      {
         get { return m_SYS_morSequenceNum; }
      }

      #endregion

      #endregion


      #region FIELD_VALIDATION
      private bool validMsgLength(string message)
      {
         if (message.Length > MAX_SWIFT_MSG_LEN)
         {
            errors.Add("Message exceeds maximum SWIFT Character length of " + MAX_SWIFT_MSG_LEN);
            return false;
         }
         else
            return true;
      }

      private bool validateBlocks()
      {
         bool isValid = true;
         if (validBlock1() == false)
         {
            isValid = false;
         }

         if (validBlock2() == false)
         {
            isValid =  false;
         }

         if (validBlock3() == false)
         {
            isValid =  false;
         }

//         if (validBlock4() == false)
//         {
//            isValid =  false;
//         }

         if (validBlock5() == false)
         {
            isValid =  false;
         }
         

         return isValid;
      }

      protected bool isValidBICCode(string BICCode)
      {
         return true;
      }

      protected bool isValidLogicalTerminal(string LT)
      {
         if (!Char.IsUpper(LT[0]))
            return false;
         else
            return true;
      }

      protected bool isValidBICBranchCode(string BBC)
      {
         return true;
      }

      protected bool isValidSessionNumber(string sessNum)
      {
         int sn = -1;

         sn = Int32.Parse(sessNum);
         if (sn < 0 || sn > 9999)
            return false;

         return true;
      }

      protected bool isValidSequenceNumber(string seqNum)
      {
         int sn = -1;

         sn = Int32.Parse(seqNum);
         if (sn < 0 || sn > 999999)
            return false;

         return true;
      }

      private bool isValidMessageType(string messageType)
      {
         bool valid = false;

         // https://www.paiementor.com/list-of-all-swift-messages-types/
         switch (messageType)
         {
            // Category 1 messages: Customer Payments and Cheques
            case "101":
            case "102":
            case "103":
            case "104":
            case "105":
            case "106":
            case "107":
            case "110":
            case "111":
            case "112":
            case "121":
            case "190":
            case "191":
            case "192":
            case "195":
            case "196":
            case "198":
            case "199":
               valid = true;
               break;

            // Category 2 Messages: Financial Institution Transfers
            case "200":
            case "201":
            case "202":
            case "202COV":
            case "203":
            case "204":
            case "205":
            case "205COV":
            case "206":
            case "207":
            case "210":
            case "256":
            case "290":
            case "291":
            case "292":
            case "295":
            case "296":
            case "298":
            case "299":
               valid = true;
               break;

            // Category 3 Messages: Treasury markets – foreign exchange, money markets and derivatives
            case "300":
            case "303":
            case "304":
            case "305":
            case "306":
            case "307":
            case "308":
            case "320":
            case "321":
            case "330":
            case "340":
            case "341":
            case "350":
            case "360":
            case "361":
            case "362":
            case "364":
            case "365":
            case "380":
            case "381":
            case "390":
            case "391":
            case "392":
            case "395":
            case "396":
            case "398":
            case "399":
               valid = true;
               break;

            // Category 4 Messages: Collections and Cash Letters
            // Category 5 Messages: Securities Markets
            // Category 6 Messages: Treasury markets – precious markets and syndications
            // Category 7 Messages: Documentary Credits and Guarantees / Standby Letters of Credit
            // Category 8 Messages: Travellers Cheques
            // Category 9 Messages: Cash Management and Customer Status
            // Category 0 Messages: FIN System Messages

            default:
               valid = false;
               break;
         }

         return valid;
      }

      #endregion


      #region Block Validations

      private bool validBlock1()
      {
         bool valid = true;
         string temp = Block_1;
         string sub = null;

         // Check the length first.
         // If this is wrong don't bother checking anything else.
         if ((Block_1.Length < MIN_BLOCK_1_LEN) || (Block_1.Length > MAX_BLOCK_1_LEN))
         {
            errors.Add("Invalid length for block 1 : " + Block_1.Length + " Expecting length between : " + MIN_BLOCK_1_LEN + " and " + MAX_BLOCK_1_LEN);
            valid = false;
            return valid;
         }

         // Check START OF BLOCK INDICATOR = { 
         // This is a mandatory field.
         // The character { indicates the beginning of a block.
         if (temp[0] != '{')
         {
            errors.Add("BLOCK 1: Message missing START OF BLOCK INDICATOR ({)");
            valid = false;
         }

         temp = temp.Substring(1);
         // Check BLOCK IDENTIFIER = 3c
         // This is a mandatory field.
         // 1 to 3 alphanumeric characters used to define block contents. 
         // This block identiifer must be 1.
         sub = temp.Substring(0, temp.IndexOf(':'));
         if (string.Equals(sub, "1") == false)
         {
            errors.Add("BLOCK 1: Invalid BLOCK IDENTIFIER : " + sub + " Expecting : 1");
            valid = false;
         }

         // We found the colon in the last step now get rid of it
         // This is a mandatory field.
         temp = temp.Substring(2);

         // Check the APPLICATION IDENTIFIER = 1a
         // This is a mandatory field.
         // The Application Identifier identifies the application within which the message is being sent or received. 
         // The available options are: 
         // F = FIN All user-to-user, FIN system and FIN service messages 
         // A = GPA (General Purpose Application) Most GPA system and service messages 
         // L = GPA Certain GPA service messages, for example, LOGIN, LAKs, ABORT These values are automatically assigned by the SWIFT system and the user's CBT
         m_ApplicationID_m = temp.Substring(0, 1);
         if ((m_ApplicationID_m.Equals("F") == false) && (m_ApplicationID_m.Equals("A") == false) && (m_ApplicationID_m.Equals("L") == false))
         {
            errors.Add("BLOCK 1: Invalid APPLICATION IDENTIFIER : " + m_ApplicationID_m + " Expecting : F, A or L");
            valid = false;
         }

         // Check SERVICE IDENTIFIER = 2n
         // This is a mandatory field.
         // The Service Identifier consists of two numeric characters. 
         // It identifies the type of data that is being sent or received and, in doing so, whether the message which follows is one of the following: 
         // a user-to-user message, 
         // a system message, 
         // a service message, 
         // for example, a session control command, such as SELECT, or a logical acknowledgment, such as ACK/SAK/UAK. 
         // Possible values are 01 = FIN/GPA or 21 = ACK/NAK.
         temp = temp.Substring(1);
         m_ServiceID_m = temp.Substring(0, 2);
         if ((m_ServiceID_m.Equals("01") == false) && (m_ServiceID_m.Equals("21") == false))
         {
            errors.Add("BLOCK 1: Invalid SERVICE IDENTIFIER : " + m_ServiceID_m + " Expecting : 01 or 21");
            valid = false;
         }

         // Check the LOGICAL TERMINAL ADDRESS = 12x
         // This is a mandatory field.
         // The Logical Termial (LT) Address is a 12-character FIN address. 
         // It is the address of the sending LT for input messages or of the receiving LT for output messages, and includes the Branch Code. 
         // It consists of: - the BIC 8 CODE (8 characters) - the Logical Terminal Code (1 upper case alphabetic character) - the BIC Branch Code (3 characters)
         temp = temp.Substring(2);
         m_LTAddress_m = temp.Substring(0, 12);
         m_BICCode = m_LTAddress_m.Substring(0, 8);
         m_LogicalTerminalCode = m_LTAddress_m.Substring(8, 1);
         m_BICBranchCode = m_LTAddress_m.Substring(9, 3);
         if (isValidBICCode(m_BICCode) == false)
         {
            errors.Add("BLOCK 1: Invalid BIC Code : " + m_BICCode);
            valid = false;
         }
         if (isValidLogicalTerminal(m_LogicalTerminalCode) == false)
         {
            errors.Add("BLOCK 1: Invalid Logical Terminal : " + m_LogicalTerminalCode);
            valid = false;
         }
         if (isValidBICBranchCode(m_BICBranchCode) == false)
         {
            errors.Add("BLOCK 1: Invalid BIC Branch Code : " + m_BICBranchCode);
            valid = false;
         }

         // Check the SESSION NUMBER = 4n
         // This is a mandatory field.
         // The Session Number identifies the session in which the message was transmitted. 
         // It is a four digits number that is automatically generated by the user's computer and padded with zeros.
         temp = temp.Substring(12);
         m_SessionNumber_m = temp.Substring(0, 4);
         if (isValidSessionNumber(m_SessionNumber_m) == false)
         {
            errors.Add("BLOCK 1: Invalid SESSION NUMBER : " + m_SessionNumber_m);
            valid = false;
         }

         // Check the INPUT SEQUENCE NUMBER = 6n 
         // This is a mandatory field.
         // The sequence number always consists of six digits. 
         // It is the Input Sequence Number (ISN) of the sender's current input session or the Output Sequence Number (OSN) of the receiver's current output session. 
         // It is automatically generated by the user's computer and padded with zeros.
         temp = temp.Substring(4);
         m_SequenceNumber_m = temp.Substring(0, 6);
         if (isValidSequenceNumber(m_SequenceNumber_m) == false)
         {
            errors.Add("BLOCK 1: Invalid INPUT SEQUENCE NUMBER : " + m_SequenceNumber_m);
            valid = false;
         }

         // Check END OF BLOCK INDICATOR = { 
         // This is a mandatory field.
         // The character } indicates the end of a block.
         temp = temp.Substring(6);
         if (temp[0] != '}')
         {
            errors.Add("BLOCK 1: Message missing START OF BLOCK INDICATOR ({)");
            valid = false;
         }

         return valid;
      }

      private bool validBlock2()
      {
         string temp = Block_2;
         string sub = null;
         bool valid = true;

         // Check the length first.
         // If this is wrong don't bother checking anything else.
         if (((Block_2.Length < MIN_BLOCK_2i_LEN) || (Block_2.Length > MAX_BLOCK_2i_LEN)) &&
            ((Block_2.Length < MIN_BLOCK_2o_LEN) || (Block_2.Length > MAX_BLOCK_2o_LEN)) )
         {
            errors.Add("Invalid length for block 2 : " + Block_2.Length);
            valid = false;
            return valid;
         }

         // Check START OF BLOCK INDICATOR = { 
         // This is a mandatory field.
         // The character { indicates the beginning of a block.
         if (temp[0] != '{')
         {
            errors.Add("BLOCK 2: Message missing START OF BLOCK INDICATOR ({)");
            valid = false;
         }

         temp = temp.Substring(1);
         // Check BLOCK IDENTIFIER = 3c
         // This is a mandatory field.
         // 1 to 3 alphanumeric characters used to define block contents. 
         // This block identiifer must be 1.
         sub = temp.Substring(0, temp.IndexOf(':'));
         if (string.Equals(sub, "2") == false)
         {
            errors.Add("BLOCK 2: Invalid BLOCK IDENTIFIER : " + sub + " Expecting : 2");
            valid = false;
         }

         // We found the colon in the last step now get rid of it
         // This is a mandatory field.
         temp = temp.Substring(2);

         // Check the INPUT OUTPUT IDENTIFIER
         // This is a mandatory field.
         // For an input message, the Input/Output Identifier consists of the single letter 'I'
         // For an input message, the Input/Output Identifier consists of the single letter 'O'
         m_IOID_m = temp.Substring(0, 1);
         if( (m_IOID_m.Equals("I") == false) && (m_IOID_m.Equals("O") == false) )
         {
            errors.Add("BLOCK 2: Invalid INPUT OUTPUT IDENTIFIER : " + m_IOID_m + " Expecting : I or O");
            valid = false;
         }

         // Check SWIFT MESSAGE TYPE
         // The Message Type consists of 3 digits which define the MT number of the message being input
         // NO CHECK NEEDED.
         // Move forward fitr IOId
         temp = temp.Substring(1);
         // Move forward for SWIFT message type.
         m_MessageType_m = temp.Substring(0,3);
         if( isValidMessageType(m_MessageType_m) == false )
         {
            errors.Add("BLOCK 2: Invalid SWIFT Message Type : " + m_MessageType_m);
            valid = false;
         }
         temp = temp.Substring(3);
         
         if (m_IOID_m.Equals("I") == true)
         {
            // We now know it is an INPUT message check the length
            if ( (Block_2.Length < MIN_BLOCK_2i_LEN) || (Block_2.Length > MAX_BLOCK_2i_LEN) )
            {
               errors.Add("Invalid length for block 2 INPUT Message: " + Block_2.Length + " Expection length between : " + MIN_BLOCK_2i_LEN + " and " + MAX_BLOCK_2i_LEN);
               valid = false;
               return valid;
            }

            // The DESTINATION ADDRESS = 12x
            // This address is the 12-character SWIFT address of the receiver of the message. 
            // It defines the destination to which the message should be sent.
            // NO CHECK needed
            m_DestinationAddress_m = temp.Substring(0,12);
            temp = temp.Substring(12);

            // Check the PRIORITY = 1a
            // This character, used within FIN Application Headers only, defines the priority with which a message is delivered. 
            // The possible values are:
            // S = System
            // U = Urgent
            // N = Normal
            m_Priority_o = temp.Substring(0, 1);
            if ((m_Priority_o.Equals("S") == false) && (m_Priority_o.Equals("U") == false) && (m_Priority_o.Equals("N") == false))
            {
               errors.Add("BLOCK 2: Invalid PRIORITY : " + m_Priority_o + " Expecting : S, U or N");
               valid = false;
            }

            // Check DELIVERY MONITORING = 1x
            // Delivery monitoring options apply only to FIN user-to-user messages. The chosen option is expressed as a single digit:
            // 1 = Non - Delivery Warning
            // 2 = Delivery Notification
            // 3 = Non - Delivery Warning and Delivery Notification
            // If the message has priority 'U', the user must request delivery monitoring option '1' or '3'.
            // If the message has priority 'N', the user can request delivery monitoring option '2' or, by leaving the option blank, no delivery monitoring.
            temp = temp.Substring(1);
            m_DeliveryMonitoring_o = temp.Substring(0, 1);
            if(m_Priority_o.Equals("S") )
            {
               if( (m_DeliveryMonitoring_o.Equals("1") == false) && (m_DeliveryMonitoring_o.Equals("1") == false) && (m_DeliveryMonitoring_o.Equals("3") == false) && (m_DeliveryMonitoring_o.Equals("") == false) )
               {
                  errors.Add("BLOCK 2: Invalid DELIVERY MONITORING - " + m_DeliveryMonitoring_o + " PRIORITY : " + m_Priority_o + " DELIVERY MONITORING MUST BE : 1, 2, 3 or blank");
                  valid = false;
               }
            }
            else if (m_Priority_o.Equals("U"))
            {
               if ((m_DeliveryMonitoring_o.Equals("1") == false) && (m_DeliveryMonitoring_o.Equals("3") == false))
               {
                  errors.Add("BLOCK 2: Invalid DELIVERY MONITORING - " + m_DeliveryMonitoring_o + " PRIORITY : " + m_Priority_o + " DELIVERY MONITORING MUST BE : 1 or 3");
                  valid = false;
               }
            }
            else if (m_Priority_o.Equals("N"))
            {
               if ((m_DeliveryMonitoring_o.Equals("2") == false) && (m_DeliveryMonitoring_o.Equals("") == false))
               {
                  errors.Add("BLOCK 2: Invalid DELIVERY MONITORING - " + m_DeliveryMonitoring_o + " PRIORITY : " + m_Priority_o + " DELIVERY MONITORING MUST BE : 2 or blank");
                  valid = false;
               }
            }

            // Check OBSOLESCENCE PERIOD = 3n
            // The obsolescence period defines the period of time after which a Delayed Message (DLM) trailer is added to a FIN user-to-user message 
            // when the message is delivered. For urgent priority messages, it is also the period of time after which, if the message remains undelivered, 
            // a Non-Delivery Warning is generated. 
            // The values for the obsolescence period are: 
            // 003 (15 minutes) for 'U' priority, and 
            // 020 (100 minutes) for 'N' priority.
            temp = temp.Substring(1);
            m_ObsolescencePeriod_o = temp.Substring(0, 3);
            if (m_Priority_o.Equals("U"))
            {
               if (m_ObsolescencePeriod_o.Equals("003") == false)
               {
                  errors.Add("BLOCK 2: Invalid  OBSOLESCENCE PERIOD - " + m_ObsolescencePeriod_o + " PRIORITY : " + m_Priority_o + " DELIVERY MONITORING MUST BE : 003");
                  valid = false;
               }
            }
            else if (m_Priority_o.Equals("N"))
            {
               if (m_ObsolescencePeriod_o.Equals("020") == false)
               {
                  errors.Add("BLOCK 2: Invalid  OBSOLESCENCE PERIOD - " + m_ObsolescencePeriod_o + " PRIORITY : " + m_Priority_o + " DELIVERY MONITORING MUST BE : 020");
                  valid = false;
               }
            }

            // Check END OF BLOCK INDICATOR = } 
            // This is a mandatory field.
            // The character } indicates the end of a block.
            temp = temp.Substring(3);
            if (temp[0] != '}')
            {
               errors.Add("BLOCK 2: Message missing END OF BLOCK INDICATOR (})");
               valid = false;
            }

            return valid;
         }
         else if (m_IOID_m.Equals("O") == true)
         {
            // We now know it is an OUTPUT message check the length
            if ((Block_2.Length < MIN_BLOCK_2o_LEN) || (Block_2.Length > MAX_BLOCK_2o_LEN))
            {
               errors.Add("Invalid length for block 2 OUTPUT Message: " + Block_2.Length + " Expection length between : " + MIN_BLOCK_2o_LEN + " and " + MAX_BLOCK_2o_LEN);
               valid = false;
               return valid;
            }

            // The INPUT TIME = HHMM
            // The Input Time local to the sender of the message.
            // The hour (HH) and minute (MM) on which the sender sent the message to SWIFT.
            // This is a mandatory field.
            m_InputTime_m = temp.Substring(0,4);
            temp = temp.Substring(4);

            // The MIR - Message Input Reference
            // The MIR consists of four elements:
            // 1.Sender's Date - Date when the Sender sent the message
            // 2.The Logical Termial (LT)Address is a 12 - character FIN address.
            //   It is the address of the sending LT for this message and includes the Branch Code. It consists of:
            //       -the Sender BIC 8 CODE(8 characters)
            //       - the Logical Terminal Code(1 upper case alphabetic character)
            //       - the Sender BIC Branch Code(3 characters). It defines the sender of the message to the SWIFT network.
            // 3.Session number - As appropriate, the current application session number based on the Login.See block 1, field 4
            // 4.Sequence number - See block 1, field 5
            m_MIR_m = temp.Substring(0, 28);
            temp = temp.Substring(28);

            m_MIRSenderDate = m_MIR_m.Substring(0, 6);

            m_MIRLTAddress = m_MIR_m.Substring(6, 12);
            m_MIRBICCode = m_MIRLTAddress.Substring(0, 8);
            m_MIRLTCode = m_MIRLTAddress.Substring(8, 1); ;
            m_MIRBICBranchCode = m_MIRLTAddress.Substring(9, 3);

            m_MIRSessNum = m_MIR_m.Substring(18, 4);
            m_MIRSeqNum = m_MIR_m.Substring(22, 6);

            // The OUTPUT DATE - YYMMDD
            // The output date, local to the receiver
            // This is a mandatory field
            m_OutputDate_m = temp.Substring(0, 6);
            temp = temp.Substring(6);

            // The OUTPUT TIME - HHMM
            // The output time, local to the receiver
            // This is a mandatory field
            m_OutputTime_m = temp.Substring(0, 4);
            temp = temp.Substring(4);

            // Check the PRIORITY = 1a
            // This character, used within FIN Application Headers only, defines the priority with which a message is delivered. 
            // The possible values are:
            // S = System
            // U = Urgent
            // N = Normal
            m_Priority_o = temp.Substring(0, 1);
            if ((m_Priority_o.Equals("S") == false) && (m_Priority_o.Equals("U") == false) && (m_Priority_o.Equals("N") == false))
            {
               errors.Add("BLOCK 2: Invalid PRIORITY : " + m_Priority_o + " Expecting : S, U or N");
               valid = false;
            }

            // Check END OF BLOCK INDICATOR = } 
            // This is a mandatory field.
            // The character } indicates the end of a block.
            temp = temp.Substring(1);
            if (temp[0] != '}')
            {
               errors.Add("BLOCK 2: Message missing END OF BLOCK INDICATOR (})");
               valid = false;
            }
         }
         else
         {
            // Some how we corrupted the valid data we just had.
            errors.Add("BLOCK 2: Data corruption");
            valid = false;
         }

         return valid;
      }

      private bool validBlock3()
      {
         string temp = Block_3;
         string sub = null;
         bool valid = true;


         return valid;
      }

      private void validateBlock4()
      {

      }

      private void separateSections(string input, out string[] output)
      {
         int start = 0;
         int i = 0;
         string bStr = "";
         output = new string[MAX_BLOCK_5_SECTIONS];

         foreach (char c in input)
         {
            if (c.Equals('{') == true)
            {
               start++;
            }

            if (c.Equals('}') == true)
            {
               start--;
            }

            if (start == 0)
            {
               start = 0;
               output[i] = bStr + c;
               bStr = "";
               i++;
            }
            else
            {
               bStr += c;
            }
         }
      }

      private bool validBlock5()
      {
         string temp = Block_5;
         string sub = null;
         bool valid = true;
         string[] sections = new string[MAX_BLOCK_5_SECTIONS];

         // Check the length first.
         // If this is wrong don't bother checking anything else.
         if ((Block_5.Length < MIN_BLOCK_5_LEN) || (Block_5.Length > MAX_BLOCK_5_LEN))
         {
            errors.Add("Invalid length for block 5 : " + Block_5.Length);
            valid = false;
            return valid;
         }

         // Check START OF BLOCK INDICATOR = { 
         // This is a mandatory field.
         // The character { indicates the beginning of a block.
         if (temp[0] != '{')
         {
            errors.Add("BLOCK 5: Message missing START OF BLOCK INDICATOR ({)");
            valid = false;
         }

         temp = temp.Substring(1);
         // Check BLOCK IDENTIFIER = 3c
         // This is a mandatory field.
         // 1 to 3 alphanumeric characters used to define block contents. 
         // This block identiifer must be 1.
         sub = temp.Substring(0, temp.IndexOf(':'));
         if (string.Equals(sub, "5") == false)
         {
            errors.Add("BLOCK 5: Invalid BLOCK IDENTIFIER : " + sub + " Expecting : 5");
            valid = false;
         }

         // We found the colon in the last step now get rid of it
         // This is a mandatory field.
         temp = temp.Substring(2);

         separateSections(temp, out sections);

         // Check the CHK - Checksum - CHK:12!h
         // Checksum calculated for all message types
         // This is a mandatory field.
         foreach(string sect in sections)
         {
            if (sect == null)
               continue;

            if (sect.Contains("CHK:") == true)
               m_Checksum_m = sect.Substring(5, sect.Length - 6);
            else if (sect.Contains("TNG:") == true)
               m_TNGMessage_o = "YES";
            else if (sect.Contains("PDE:") == true)
            {
               m_PDE_o = sect;
               m_PDE_time = sect.Substring(5, 4);
               m_PDE_mir = sect.Substring(9, 27);
               m_PDE_mirDate = sect.Substring(9, 6);
               m_PDE_mirLTId = sect.Substring(15, 12);
               m_PDE_mirSessionNum = sect.Substring(27, 4);
               m_PDE_mirSequenceNum = sect.Substring(31, 6);
            }
            else if (sect.Contains("DLM:") == true)
               m_DLM_o = "YES";
            else if (sect.Contains("MRF:") == true)
            {
               m_MRF_o = sect;
               m_MRF_date = sect.Substring(6, 6);
               m_MRF_time = sect.Substring(7, 4);
               m_MRF_mir = sect.Substring(11, 28);
            }
            else if (sect.Contains("PDM:") == true)
            {
               m_PDM_o = sect;
               m_PDM_time = sect.Substring(5, 4);
               m_PDM_mor = sect.Substring(9, 28);
               m_PDM_morDate = sect.Substring(9, 6);
               m_PDM_morLTId = sect.Substring(15, 12);
               m_PDM_morSessionNum = sect.Substring(27, 4);
               m_PDM_morSequenceNum = sect.Substring(31, 6);
            }
            else if (sect.Contains("SYS:") == true)
            {
               m_SYS_o = sect;
               m_SYS_time = sect.Substring(5, 4);
               m_SYS_mor = sect.Substring(9, 28);
               m_SYS_morDate = sect.Substring(9, 6);
               m_SYS_morLTId = sect.Substring(15, 12);
               m_SYS_morSessionNum = sect.Substring(27, 4);
               m_SYS_morSequenceNum = sect.Substring(31, 6);
            }
         }

         return valid;
      }

      #endregion
   }
}
