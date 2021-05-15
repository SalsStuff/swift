using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Messages
{
    // Validation Rules are from : https://www.paiementor.com/swift-mt-message-structure-blocks-1-to-5/
    // Also here : https://www2.swift.com/knowledgecentre/products/Standards%20MT
    public class BlockHeader
    {
        #region DEFINES
        private static readonly int MAX_SWIFT_MSG_LEN = 4096;
        private static readonly int MIN_BLOCK_1_LEN = 29;
        private static readonly int MAX_BLOCK_1_LEN = 31;
        private static readonly int MIN_BLOCK_2i_LEN = 25;
        private static readonly int MAX_BLOCK_2i_LEN = 27;
        private static readonly int MIN_BLOCK_2o_LEN = 29;
        private static readonly int MAX_BLOCK_2o_LEN = 53;
        private static readonly int MIN_BLOCK_3_LEN = 6;
        private static readonly int MAX_BLOCK_3_LEN = 325;
        private static readonly int MIN_BLOCK_5_LEN = 24;
        private static readonly int MAX_BLOCK_5_LEN = 192;
        private static readonly int MAX_BLOCK_5_SECTIONS = 7;
        #endregion
        
        #region VARIABLES
        protected List<string> errors = new List<string>();
        #endregion
    
        public BlockHeader()
        {
           
        }
    
        private void InitializeVariables()
        {
            // BASIC HEADER member variables
            ApplicationID = null;
            ServiceID = null;
            LTAddress = null;
            BICCode = null;
            LogicalTerminalCode = null;
            BICBranchCode = null;
            SessionNumber = null;
            SequenceNumber = null;
            
            // APPLICATION HEADER member variables
            InputOutputID = null;
            MessageType = null;
            DestinationAddress = null;
            Priority = null;
            DeliveryMonitoring = null;
            ObsolescencePeriod = null;
            InputTime = null;
            MIR = null;
            MIRSenderDate = null;
            MIRLTAddress = null;
            MIRBICCode = null;
            MIRLTCode = null;
            MIRBICBranchCode = null;
            MIRSessNum = null;
            MIRSeqNum = null;
            OutputDate = null;
            OutputTime = null;

            // USER HEADER member variables
            TAG103_ServiceID = null;
            TAG113_BankingPriority = null;
            TAG108_MUR = null;
            TAG119_ValidationFlag = null;
            TAG423_BalanceCheckPoint = null;
            TAG106_MIR = null;
            TAG424_RelatedReference = null;
            TAG111_ServiceTypeID = null;
            TAG121_UniqueTranReference = null;
            TAG115_AddresseeInfo = null;
            TAG165_PaymentRIR = null;
            TAG433_SanctionsSIR = null;
            TAG434_PaymentCIR = null;

            // TRAILER HEADER member variables
            Checksum = null;
            TNGMessage = null;
            PDE = null;
            PDETime = null;
            PDEMir = null;
            PDEMirDate = null;
            PDEMirLTId = null;
            PDEMirSessionNum = null;
            PDEMirSequenceNum = null;
            DLM = null;
            MRF = null;
            MRFDate = null;
            MRFTime = null;
            MRFMir = null;
            PDM = null;
            PDMTime = null;
            PDMMor = null;
            PDMMorDate = null;
            PDMMorLTId = null;
            PDMMorSessionNum = null;
            PDMMorSequenceNum = null;
            SYS = null;
            SYSTime = null;
            SYSMor = null;
            SYSMorDate = null;
            SYSMorLTId = null;
            SYSMorSessionNum = null;
            SYSMorSequenceNum = null;
        }
    
        private void ParseBlocks(string msg)
        {
            int start = 0;
            int i = 0;
            string[] tempStrs = { "", "", "", "", "" };
            string bStr = "";
            bool msgEnd = false;

            foreach (char c in msg)
            {
                if( c.Equals('{') == true)
                {
                    start++;
                }
            
                if( c.Equals('}') == true)
                {
                    start--;
                }

                if (start == 0)
                {
                    start = 0;
                    tempStrs[i] = bStr + c;

                    if (tempStrs[i].Substring(tempStrs[i].Length - 2, 2).Equals("-}") == true)
                        msgEnd = true;
                    bStr = "";
                    i++;
                }
                else
                {
                    bStr += c;
                }

                if (msgEnd == true)
                    break;
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
                   Block_1 = tempStrs[i];
                }
                else if (temp.Equals("{2:") == true)
                {
                    Block_2 = tempStrs[i];
                }
                else if (temp.Equals("{3:") == true)
                {
                    Block_3 = tempStrs[i];
                }
                else if (temp.Equals("{4:") == true)
                {
                    Block_4 = tempStrs[i];
                }
                else if (temp.Equals("{5:") == true)
                {
                    Block_5 = tempStrs[i];
                }
            }
        }
    
        public bool ParseFile(string fileName)
        {
            string msgFile = File.ReadAllText(fileName);
        
            errors.Clear();
            InitializeVariables();
            if (ValidMsgLength(msgFile) == false)
                return false;
        
            ParseBlocks(msgFile);
        
            if(ValidateBlocks() == false)
                return false;

            return true;
        }

        #region Get_Set
        public string Block_1 { get; private set; } = null;

        public string Block_2 { get; private set; } = null;
        
        public string Block_3 { get; private set; } = null;
        
        public string Block_4 { get; private set; } = null;
        
        public string Block_5 { get; private set; } = null;
        
        public List<string> Errors
        {
            get { return errors; }
        }

        #region BASIC_HEADER_GETS
        public string ApplicationID { get; private set; } = null;

        public string ServiceID { get; private set; } = null;

        public string LTAddress { get; private set; } = null;

        public string BICCode { get; private set; } = null;

        public string LogicalTerminalCode { get; private set; } = null;

        public string BICBranchCode { get; private set; } = null;

        public string SessionNumber { get; private set; } = null;

        public string SequenceNumber { get; private set; } = null;

        #endregion

        #region APPLICATION_HEADER_GETS
        public string InputOutputID { get; private set; } = null;

        public string MessageType { get; private set; } = null;

        public string DestinationAddress { get; private set; } = null;

        public string Priority { get; private set; } = null;

        public string DeliveryMonitoring { get; private set; } = null;

        public string ObsolescencePeriod { get; private set; } = null;

        public string InputTime { get; private set; } = null;

        public string MIR { get; private set; } = null;

        public string MIRSenderDate { get; private set; } = null;

        public string MIRLTAddress { get; private set; } = null;

        public string MIRBICCode { get; private set; } = null;

        public string MIRLTCode { get; private set; } = null;

        public string MIRBICBranchCode { get; private set; } = null;

        public string MIRSessNum { get; private set; } = null;

        public string MIRSeqNum { get; private set; } = null;

        public string OutputDate { get; private set; } = null;

        public string OutputTime { get; private set; } = null;
        #endregion

        #region USER HEADER GETS
        public string TAG103_ServiceID { get; private set; } = null;
        public string TAG113_BankingPriority { get; private set; } = null;
        public string TAG108_MUR { get; private set; } = null;
        public string TAG119_ValidationFlag { get; private set; } = null;
        public string TAG423_BalanceCheckPoint { get; private set; } = null;
        public string TAG106_MIR { get; private set; } = null;
        public string TAG424_RelatedReference { get; private set; } = null;
        public string TAG111_ServiceTypeID { get; private set; } = null;
        public string TAG121_UniqueTranReference { get; private set; } = null;
        public string TAG115_AddresseeInfo { get; private set; } = null;
        public string TAG165_PaymentRIR { get; private set; } = null;
        public string TAG433_SanctionsSIR { get; private set; } = null;
        public string TAG434_PaymentCIR { get; private set; } = null;
        #endregion

        #region TRAILER_HEADER_GETS
        public string Checksum { get; private set; } = null;

        public string TNGMessage { get; private set; } = null;

        public string PDE { get; private set; } = null;

        public string PDETime { get; private set; } = null;

        public string PDEMir { get; private set; } = null;

        public string PDEMirDate { get; private set; } = null;

        public string PDEMirLTId { get; private set; } = null;

        public string PDEMirSessionNum { get; private set; } = null;

        public string PDEMirSequenceNum { get; private set; } = null;

        public string DLM { get; private set; } = null;

        public string MRF { get; private set; } = null;

        public string MRFDate { get; private set; } = null;

        public string MRFTime { get; private set; } = null;

        public string MRFMir { get; private set; } = null;

        public string PDM { get; private set; } = null;

        public string PDMTime { get; private set; } = null;

        public string PDMMor { get; private set; } = null;

        public string PDMMorDate { get; private set; } = null;

        public string PDMMorLTId { get; private set; } = null;

        public string PDMMorSessionNum { get; private set; } = null;

        public string PDMMorSequenceNum { get; private set; } = null;

        public string SYS { get; private set; } = null;

        public string SYSTime { get; private set; } = null;

        public string SYSMor { get; private set; } = null;

        public string SYSMorDate { get; private set; } = null;

        public string SYSMorLTId { get; private set; } = null;

        public string SYSMorSessionNum { get; private set; } = null;

        public string SYSMorSequenceNum { get; private set; } = null;
        #endregion
        #endregion
    
        #region FIELD_VALIDATION
        private bool ValidMsgLength(string message)
        {
            if (message.Length > MAX_SWIFT_MSG_LEN)
            {
                errors.Add("Message exceeds maximum SWIFT Character length of " + MAX_SWIFT_MSG_LEN);
                return false;
            }
            else
                return true;
        }
    
        private bool ValidateBlocks()
        {
            bool isValid = true;
            if (ValidBlock1() == false)
            {
                isValid = false;
            }
            
            if (ValidBlock2() == false)
            {
                isValid =  false;
            }
            
            if (ValidBlock3() == false)
            {
                isValid =  false;
            }
            
            //         if (validBlock4() == false)
            //         {
            //            isValid =  false;
            //         }
            
            if (ValidBlock5() == false)
            {
                isValid =  false;
            }
            
            return isValid;
        }
    
        protected bool IsValidBICCode(string BICCode)
        {
            return true;
        }
        
        protected bool IsValidLogicalTerminal(string LT)
        {
            if (!Char.IsUpper(LT[0]))
                return false;
            else
                return true;
        }
        
        protected bool IsValidBICBranchCode(string BBC)
        {
            return true;
        }
        
        protected bool IsValidSessionNumber(string sessNum)
        {
            int sn = -1;
        
            sn = Int32.Parse(sessNum);
            if (sn < 0 || sn > 9999)
                return false;
        
            return true;
        }
    
        protected bool IsValidSequenceNumber(string seqNum)
        {
            int sn = -1;
        
            sn = Int32.Parse(seqNum);
            if (sn < 0 || sn > 999999)
                return false;
        
            return true;
        }
    
        private bool IsValidMessageType(string messageType)
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
        private bool ValidBlock1()
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
            ApplicationID = temp.Substring(0, 1);
            if ((ApplicationID.Equals("F") == false) && (ApplicationID.Equals("A") == false) && (ApplicationID.Equals("L") == false))
            {
                errors.Add("BLOCK 1: Invalid APPLICATION IDENTIFIER : " + ApplicationID + " Expecting : F, A or L");
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
            ServiceID = temp.Substring(0, 2);
            if ((ServiceID.Equals("01") == false) && (ServiceID.Equals("21") == false))
            {
                errors.Add("BLOCK 1: Invalid SERVICE IDENTIFIER : " + ServiceID + " Expecting : 01 or 21");
                valid = false;
            }
            
            // Check the LOGICAL TERMINAL ADDRESS = 12x
            // This is a mandatory field.
            // The Logical Termial (LT) Address is a 12-character FIN address. 
            // It is the address of the sending LT for input messages or of the receiving LT for output messages, and includes the Branch Code. 
            // It consists of: - the BIC 8 CODE (8 characters) - the Logical Terminal Code (1 upper case alphabetic character) - the BIC Branch Code (3 characters)
            temp = temp.Substring(2);
            LTAddress = temp.Substring(0, 12);
            BICCode = LTAddress.Substring(0, 8);
            LogicalTerminalCode = LTAddress.Substring(8, 1);
            BICBranchCode = LTAddress.Substring(9, 3);
            if (IsValidBICCode(BICCode) == false)
            {
                errors.Add("BLOCK 1: Invalid BIC Code : " + BICCode);
                valid = false;
            }
            if (IsValidLogicalTerminal(LogicalTerminalCode) == false)
            {
                errors.Add("BLOCK 1: Invalid Logical Terminal : " + LogicalTerminalCode);
                valid = false;
            }
            if (IsValidBICBranchCode(BICBranchCode) == false)
            {
                errors.Add("BLOCK 1: Invalid BIC Branch Code : " + BICBranchCode);
                valid = false;
            }
            
            // Check the SESSION NUMBER = 4n
            // This is a mandatory field.
            // The Session Number identifies the session in which the message was transmitted. 
            // It is a four digits number that is automatically generated by the user's computer and padded with zeros.
            temp = temp.Substring(12);
            SessionNumber = temp.Substring(0, 4);
            if (IsValidSessionNumber(SessionNumber) == false)
            {
                errors.Add("BLOCK 1: Invalid SESSION NUMBER : " + SessionNumber);
                valid = false;
            }
            
            // Check the INPUT SEQUENCE NUMBER = 6n 
            // This is a mandatory field.
            // The sequence number always consists of six digits. 
            // It is the Input Sequence Number (ISN) of the sender's current input session or the Output Sequence Number (OSN) of the receiver's current output session. 
            // It is automatically generated by the user's computer and padded with zeros.
            temp = temp.Substring(4);
            SequenceNumber = temp.Substring(0, 6);
            if (IsValidSequenceNumber(SequenceNumber) == false)
            {
                errors.Add("BLOCK 1: Invalid INPUT SEQUENCE NUMBER : " + SequenceNumber);
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
        
        private bool ValidBlock2()
        {
            string temp = Block_2;
            string sub = null;
            bool valid = true;
            int offset = 0;
            
            // Check the length first.
            // If this is wrong don't bother checking anything else.
            // Block 2 is optional therefore it is a valid message if it is not present
            if (Block_2 == null)
            {
                errors.Add("Block 2 -- Optional -- : NOT present");
                valid = true;
                return valid;
            }
            else if (((Block_2.Length < MIN_BLOCK_2i_LEN) || (Block_2.Length > MAX_BLOCK_2i_LEN)) &&
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
            InputOutputID = temp.Substring(0, 1);
            if( (InputOutputID.Equals("I") == false) && (InputOutputID.Equals("O") == false) )
            {
                errors.Add("BLOCK 2: Invalid INPUT OUTPUT IDENTIFIER : " + InputOutputID + " Expecting : I or O");
                valid = false;
            }
            
            // Check SWIFT MESSAGE TYPE
            // The Message Type consists of 3 digits which define the MT number of the message being input
            // NO CHECK NEEDED.
            // Move forward fitr IOId
            temp = temp.Substring(1);
            // Move forward for SWIFT message type.
            MessageType = temp.Substring(0,3);
            if( IsValidMessageType(MessageType) == false )
            {
                errors.Add("BLOCK 2: Invalid SWIFT Message Type : " + MessageType);
                valid = false;
            }
            temp = temp.Substring(3);
            
            if (InputOutputID.Equals("I") == true)
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
                DestinationAddress = temp.Substring(0,12);
                temp = temp.Substring(12);
                
                // Check the PRIORITY = 1a
                // This character, used within FIN Application Headers only, defines the priority with which a message is delivered. 
                // The possible values are:
                // S = System
                // U = Urgent
                // N = Normal
                Priority = temp.Substring(0, 1);
                if ((Priority.Equals("S") == false) && (Priority.Equals("U") == false) && (Priority.Equals("N") == false))
                {
                    errors.Add("BLOCK 2: PRIORITY not set: ");
                    valid = false;
                    offset = 0;
                    Priority = "";
                }
                else
                {
                    offset = 1;
                }
                
                // Check DELIVERY MONITORING = 1x
                // Delivery monitoring options apply only to FIN user-to-user messages. The chosen option is expressed as a single digit:
                // 1 = Non - Delivery Warning
                // 2 = Delivery Notification
                // 3 = Non - Delivery Warning and Delivery Notification
                // If the message has priority 'U', the user must request delivery monitoring option '1' or '3'.
                // If the message has priority 'N', the user can request delivery monitoring option '2' or, by leaving the option blank, no delivery monitoring.
                temp = temp.Substring(offset);
                DeliveryMonitoring = temp.Substring(0, 1);
                if(Priority.Equals("S") )
                {
                    if( (DeliveryMonitoring.Equals("1") == false) && (DeliveryMonitoring.Equals("1") == false) && (DeliveryMonitoring.Equals("3") == false) && (DeliveryMonitoring.Equals("") == false) )
                    {
                        errors.Add("BLOCK 2: Invalid DELIVERY MONITORING - not set" + DeliveryMonitoring + " PRIORITY : " + Priority + " DELIVERY MONITORING MUST BE : 1, 2, 3 or blank");
                        valid = false;
                    }
                    else
                    {
                        offset = 1;
                    }
                }
                else if (Priority.Equals("U"))
                {
                    if ((DeliveryMonitoring.Equals("1") == false) && (DeliveryMonitoring.Equals("3") == false))
                    {
                        errors.Add("BLOCK 2: Invalid DELIVERY MONITORING - " + DeliveryMonitoring + " PRIORITY : " + Priority + " DELIVERY MONITORING MUST BE : 1 or 3");
                        valid = false;
                    }
                    else
                    {
                        offset = 1;
                    }
                }
                else if (Priority.Equals("N"))
                {
                    if ((DeliveryMonitoring.Equals("2") == false) && (DeliveryMonitoring.Equals("") == false))
                    {
                        errors.Add("BLOCK 2: Invalid DELIVERY MONITORING - " + DeliveryMonitoring + " PRIORITY : " + Priority + " DELIVERY MONITORING MUST BE : 2 or blank");
                        valid = false;
                    }
                    else
                    {
                        offset = 1;
                    }
                }
                else
                {
                    offset = 0;
                    DeliveryMonitoring = "";
                }
                
                // Check OBSOLESCENCE PERIOD = 3n
                // The obsolescence period defines the period of time after which a Delayed Message (DLM) trailer is added to a FIN user-to-user message 
                // when the message is delivered. For urgent priority messages, it is also the period of time after which, if the message remains undelivered, 
                // a Non-Delivery Warning is generated. 
                // The values for the obsolescence period are: 
                // 003 (15 minutes) for 'U' priority, and 
                // 020 (100 minutes) for 'N' priority.
                temp = temp.Substring(offset);
                ObsolescencePeriod = temp.Substring(0, 3);
                if (Priority.Equals("U"))
                {
                    if (ObsolescencePeriod.Equals("003") == false)
                    {
                        errors.Add("BLOCK 2: Invalid  OBSOLESCENCE PERIOD - " + ObsolescencePeriod + " PRIORITY : " + Priority + " DELIVERY MONITORING MUST BE : 003");
                        valid = false;
                    }
                }
                else if (Priority.Equals("N"))
                {
                    if (ObsolescencePeriod.Equals("020") == false)
                    {
                        errors.Add("BLOCK 2: Invalid  OBSOLESCENCE PERIOD - " + ObsolescencePeriod + " PRIORITY : " + Priority + " DELIVERY MONITORING MUST BE : 020");
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
            else if (InputOutputID.Equals("O") == true)
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
                InputTime = temp.Substring(0,4);
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
                MIR = temp.Substring(0, 28);
                temp = temp.Substring(28);
                
                MIRSenderDate = MIR.Substring(0, 6);
                
                MIRLTAddress = MIR.Substring(6, 12);
                MIRBICCode = MIRLTAddress.Substring(0, 8);
                MIRLTCode = MIRLTAddress.Substring(8, 1); ;
                MIRBICBranchCode = MIRLTAddress.Substring(9, 3);
                
                MIRSessNum = MIR.Substring(18, 4);
                MIRSeqNum = MIR.Substring(22, 6);
                
                // The OUTPUT DATE - YYMMDD
                // The output date, local to the receiver
                // This is a mandatory field
                OutputDate = temp.Substring(0, 6);
                temp = temp.Substring(6);
                
                // The OUTPUT TIME - HHMM
                // The output time, local to the receiver
                // This is a mandatory field
                OutputTime = temp.Substring(0, 4);
                temp = temp.Substring(4);
                
                // Check the PRIORITY = 1a
                // This character, used within FIN Application Headers only, defines the priority with which a message is delivered. 
                // The possible values are:
                // S = System
                // U = Urgent
                // N = Normal
                Priority = temp.Substring(0, 1);
                if ((Priority.Equals("S") == false) && (Priority.Equals("U") == false) && (Priority.Equals("N") == false))
                {
                    errors.Add("BLOCK 2: PRIORITY -- OPTIONAL -- : not present");
                    valid = false;
                    offset = 0;
                    Priority = "";
                }
                else
                {
                    offset = 1;
                }
                
                // Check END OF BLOCK INDICATOR = } 
                // This is a mandatory field.
                // The character } indicates the end of a block.
                temp = temp.Substring(offset);
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
    
        private bool ValidBlock3()
        {
            string temp = Block_3;
            bool valid = true;
            string[] sections = new string[MAX_BLOCK_5_SECTIONS];

            // Check the length first.
            // If this is wrong don't bother checking anything else.
            // Block 3 is optional therefore it is a valid message if it is not present
            if (Block_3 == null)
            {
                errors.Add("Block 3 -- Optional -- : NOT present");
                valid = true;
                return valid;
            }
            else if ((Block_3.Length < MIN_BLOCK_3_LEN) || (Block_3.Length > MAX_BLOCK_3_LEN))
            {
                errors.Add("Invalid length for block 3 : " + Block_3.Length);
                valid = false;
                return valid;
            }

            // Check START OF BLOCK INDICATOR = { 
            // This is a mandatory field.
            // The character { indicates the beginning of a block.
            if (temp[0] != '{')
            {
                errors.Add("BLOCK 3: Message missing START OF BLOCK INDICATOR ({)");
                valid = false;
            }

            temp = temp.Substring(1);
            // Check BLOCK IDENTIFIER = 3c
            // This is a mandatory field.
            // 1 to 3 alphanumeric characters used to define block contents. 
            // This block identiifer must be 1.
            string sub = temp.Substring(0, temp.IndexOf(':'));
            if (string.Equals(sub, "3") == false)
            {
                errors.Add("BLOCK 3: Invalid BLOCK IDENTIFIER : " + sub + " Expecting : 3");
                valid = false;
            }

            // We found the colon in the last step now get rid of it
            // This is a mandatory field.
            temp = temp.Substring(2);

            SeparateSections(temp, out sections);

            // Check the CHK - Checksum - CHK:12!h
            // Checksum calculated for all message types
            // This is a mandatory field.
            foreach (string sect in sections)
            {
                if (sect == null)
                    continue;

                if (sect.Contains("103:") == true)
                    TAG103_ServiceID = sect.Substring(5, sect.Length - 6);
                else if (sect.Contains("113:") == true)
                    TAG113_BankingPriority = sect.Substring(5, sect.Length - 6);
                else if (sect.Contains("108:") == true)
                    TAG108_MUR = sect.Substring(5, sect.Length - 6);
                else if (sect.Contains("119:") == true)
                    TAG119_ValidationFlag = sect.Substring(5, sect.Length-6);
                else if (sect.Contains("432:") == true)
                    TAG433_SanctionsSIR = sect.Substring(5, sect.Length - 6);
                else if (sect.Contains("106:") == true)
                    TAG433_SanctionsSIR = sect.Substring(5, sect.Length - 6);
                else if (sect.Contains("424:") == true)
                    TAG424_RelatedReference = sect.Substring(5, sect.Length - 6);
                else if (sect.Contains("111:") == true)
                    TAG111_ServiceTypeID = sect.Substring(5, sect.Length - 6);
                else if (sect.Contains("121:") == true)
                    TAG121_UniqueTranReference = sect.Substring(5, sect.Length - 6);
                else if (sect.Contains("115:") == true)
                    TAG115_AddresseeInfo = sect.Substring(5, sect.Length - 6);
                else if (sect.Contains("165:") == true)
                    TAG165_PaymentRIR = sect.Substring(5, sect.Length - 6);
                else if (sect.Contains("433:") == true)
                    TAG433_SanctionsSIR = sect.Substring(5, sect.Length - 6);
                else if (sect.Contains("434:") == true)
                    TAG434_PaymentCIR = sect.Substring(5, sect.Length - 6);
            }

            return valid;
        }
    
        private void ValidateBlock4()
        {
            string temp = Block_4;
            
            // Block 4 is optional therefore it is a valid message if it is not present
            // Just check for presence here each message class will validate it's own message.
            if (temp == null)
            {
                errors.Add("Block 4 -- Optional -- : NOT present");
            }
        }
    
        private void SeparateSections(string input, out string[] output)
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
    
        private bool ValidBlock5()
        {
            string temp = Block_5;
            bool valid = true;
            string[] sections = new string[MAX_BLOCK_5_SECTIONS];
            
            // Check the length first.
            // If this is wrong don't bother checking anything else.
            // Block 5 is optional therefore it is a valid message if it is not present
            if(Block_5 == null)
            {
                errors.Add("Block 5 -- Optional -- : NOT present");
                valid = true;
                return valid;
            }
            else if ((Block_5.Length < MIN_BLOCK_5_LEN) || (Block_5.Length > MAX_BLOCK_5_LEN))
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
            string sub = temp.Substring(0, temp.IndexOf(':'));
            if (string.Equals(sub, "5") == false)
            {
                errors.Add("BLOCK 5: Invalid BLOCK IDENTIFIER : " + sub + " Expecting : 5");
                valid = false;
            }
            
            // We found the colon in the last step now get rid of it
            // This is a mandatory field.
            temp = temp.Substring(2);
            
            SeparateSections(temp, out sections);
            
            // Check the CHK - Checksum - CHK:12!h
            // Checksum calculated for all message types
            // This is a mandatory field.
            foreach(string sect in sections)
            {
                if (sect == null)
                    continue;
            
                if (sect.Contains("CHK:") == true)
                    Checksum = sect.Substring(5, sect.Length - 6);
                else if (sect.Contains("TNG:") == true)
                    TNGMessage = "YES";
                else if (sect.Contains("PDE:") == true)
                {
                    PDE = sect;
                    PDETime = sect.Substring(5, 4);
                    PDEMir = sect.Substring(9, 27);
                    PDEMirDate = sect.Substring(9, 6);
                    PDEMirLTId = sect.Substring(15, 12);
                    PDEMirSessionNum = sect.Substring(27, 4);
                    PDEMirSequenceNum = sect.Substring(31, 6);
                }
                else if (sect.Contains("DLM:") == true)
                    DLM = "YES";
                else if (sect.Contains("MRF:") == true)
                {
                    MRF = sect;
                    MRFDate = sect.Substring(6, 6);
                    MRFTime = sect.Substring(7, 4);
                    MRFMir = sect.Substring(11, 28);
                }
                else if (sect.Contains("PDM:") == true)
                {
                    PDM = sect;
                    PDMTime = sect.Substring(5, 4);
                    PDMMor = sect.Substring(9, 28);
                    PDMMorDate = sect.Substring(9, 6);
                    PDMMorLTId = sect.Substring(15, 12);
                    PDMMorSessionNum = sect.Substring(27, 4);
                    PDMMorSequenceNum = sect.Substring(31, 6);
                }
                else if (sect.Contains("SYS:") == true)
                {
                    SYS = sect;
                    SYSTime = sect.Substring(5, 4);
                    SYSMor = sect.Substring(9, 28);
                    SYSMorDate = sect.Substring(9, 6);
                    SYSMorLTId = sect.Substring(15, 12);
                    SYSMorSessionNum = sect.Substring(27, 4);
                    SYSMorSequenceNum = sect.Substring(31, 6);
                }
            }
            
            return valid;
        }
        #endregion
    }
}
