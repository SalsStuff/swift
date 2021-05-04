using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;

namespace Messages
{
    //https://www2.swift.com/knowledgecentre/publications/us1m_20200724/2.0?topic=mt111-format-spec.htm
    public class MT111 : MTMessage
    {
        DBUtils dbu = new DBUtils();

        #region SEQUENCE_VARIABLES
        // Sequence A - Mandatory
        // Sequence A General Information contains information about the request of a stop payment of a cheque.
        List<TagData<string, string, string, string, int>> sequenceA = new List<TagData<string, string, string, string, int>>
        {
            // Tag, Name, Value, Mandatory
            new TagData<string, string, string, string, int>("20",  "Transaction Reference Number",     "" ,"M", 0),
            new TagData<string, string, string, string, int>("21",  "Cheque Number",                    "" ,"M", 0),
            new TagData<string, string, string, string, int>("30",  "Date of Issue",                    "" ,"M", 0),
            new TagData<string, string, string, string, int>("32A", "Amount",                           "", "M", 0),
            new TagData<string, string, string, string, int>("32B", "Amount",                           "", "M", 0),
            new TagData<string, string, string, string, int>("52A", "Drawer Bank",                      "", "O", 0),
            new TagData<string, string, string, string, int>("52B", "Drawer Bank",                      "", "O", 0),
            new TagData<string, string, string, string, int>("52D", "Drawer Bank",                      "", "O", 0),
            new TagData<string, string, string, string, int>("59",  "Payee",                            "", "O", 0),
            new TagData<string, string, string, string, int>("75",  "Queries",                          "", "O", 0)
        };
        #endregion

        #region MESSAGE SETUP
        /// <summary>
        /// Method Constructor
        /// </summary>
        public MT111()
        {
            InitializeMT111();
        }

        /// <summary>
        /// Method Constructor
        /// </summary>
        /// <param name="msg"></param>
        public MT111(String msg)
        {
            InitializeMT111();

            if (msg.Contains("{4:") == true)
                ParseBlock4(msg);
            if (msg.Contains("||") == true)
                ParsePipeMsg(msg);
        }

        /// <summary>
        /// Get method to return specified data sequence
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public List<TagData<string, string, string, string, int>> this[int index]
        {
            get
            {
                if (index == 0) { return sequenceA; }
                else { return null; }
            }
        }

        private void InitializeMT111()
        {
            numOfSequences = 1;
            ResetVariables();
            DefineScope();
            Anomalies.Clear();
        }

        /// <summary>
        /// Definition of Message Scope
        /// This is the SWIFT definition of the message
        /// It can be found at https://www2.swift.com/knowledgecentre/publications/us1m_20200724/2.0?topic=mt111-scope.htm
        /// </summary>
        protected override void DefineScope()
        {
            Scope = "MT 111 Scope:\r\n" +
                    "This single message type is sent by a drawer bank, or a bank acting on behalf of the drawer bank, to the bank on which a cheque has been drawn (the drawee bank).\r\n" +
                    "It is used to request stop payment of the cheque referred to in the message.\r\n";
        }

        /// <summary>
        /// Reset the class variables 
        /// </summary>
        protected override void ResetVariables()
        {
            foreach (TagData<string, string, string, string, int> t in sequenceA)
            {
                t.Value = "";
                t.Present = 0;
            }
        }

        /// <summary>
        /// Parse block 4 of the SWIFT message - the message itself
        /// </summary>
        /// <param name="message"></param>
        private void ParseBlock4(string message)
        {
            string[] result = ParseBlock4MessageString(message);

            FillDataTags(result);

            IsMessageValid();
        }

        protected override List<TagData<string, string, string, string, int>> getSequence(string seqId)
        {
            List<TagData<string, string, string, string, int>> sequence = null;

            switch (seqId)
            {
                case "A":
                    sequence = sequenceA;
                    break;
                default:
                    break;
            }
            return sequence;
        }
        #endregion

        #region GET SET FUNCTIONS
        /// <summary>
        /// Get / Set method to auto validate message.
        /// If set to true the message will be validated when read in.
        /// If set to false an explicit call will be needed after the message is read in.
        /// </summary>
        public bool AutoValidate { get; set; } = true;

        /// <summary>
        /// Get / Set method to always validate tag whether or not it is present in message.
        /// </summary>
        public bool AlwaysValidateTag { get; set; } = false;
        #endregion

        #region VALIDATION RULES
        public bool IsMessageValid()
        {
            bool validMessage = true;

            if (AutoValidate == true)
            {
                ValidateTags();

                if (Anomalies.Count > 0)
                    validMessage = false;
            }
            return validMessage;
        }

        /// <summary>
        /// Validate an individual MT320 tag
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="tag"></param>
        /// <param name="tagvalue"></param>
        /// <returns></returns>
        private bool ValidateTag(string sequence, TagData<string, string, string, string, int> field)
        {
            bool validTag = true;

            switch (sequence)
            {
                case "A":
                    switch (field.Tag)
                    {
                        case "20":
                            if (Is_T20_Valid(field) == false)
                                validTag = false;
                            break;
                        case "21":
                            if (Is_T21_Valid(field) == false)
                                validTag = false;
                            break;
                        case "30":
                            if (Is_T30_Valid(field) == false)
                                validTag = false;
                            break;
                        case "32A":
                        case "32B":
                            if (Is_T32_Valid(field) == false)
                                validTag = false;
                            break;
                        case "52A":
                        case "52B":
                        case "52D":
                            if (Is_T52_Valid(field) == false)
                                validTag = false;
                            break;
                        case "59":
                            if (Is_T59_Valid(field) == false)
                                validTag = false;
                            break;
                        case "75":
                            if (Is_T75_Valid(field) == false)
                                validTag = false;
                            break;
                        default:
                            Anomalies.Add("ERROR : Unknown tag " + field.Tag + " in sequence A - can not validate.");
                            break;
                    }
                    break;
                default:
                    Anomalies.Add("ERROR : Unknown sequence " + sequence + " passed to ValidateTag.");
                    break;
            }

            return validTag;
        }

        /// <summary>
        /// Validate all tags in message
        /// </summary>
        private bool ValidateTags()
        {
            bool allTagsValid = true;
            List<string> seqs = new List<string>() { "A" };
            List<TagData<string, string, string, string, int>> seq;
            bool validateField = true;
            int T32present = 0;
            int T52present = 0;

            foreach (string sid in seqs)
            {
                seq = getSequence(sid);

                if (IsNewSequencePresent(seq) == true)
                {
                    foreach (TagData<string, string, string, string, int> t in seq)
                    {
                        validateField = true;
                        /* check for vairants */
                        if (sid.Equals("A") == true)
                        {
                            if ((t.Tag.Equals("32A") && t.Present == 0) || (t.Tag.Equals("32B") && t.Present == 0))
                            {
                                validateField = false;
                                T32present++;
                            }
                            if ((t.Tag.Equals("52A") && t.Present == 0) || (t.Tag.Equals("52B") && t.Present == 0) || (t.Tag.Equals("52D") && t.Present == 0))
                            {
                                validateField = false;
                                T52present++;
                            }
                        }

                        if (validateField == true)
                        {
                            if (ValidateTag(sid, t) == false)
                            {
                                Anomalies.Add("Tag " + t.Tag + " : Failed validation.");
                                allTagsValid = false;
                            }
                        }
                    }
                }

                if ((sid.Equals("A") == true) && (T32present == 3))
                    Anomalies.Add("ERROR: Mandatory Tag 32 is not present in any variantion");
                if ((sid.Equals("A") == true) && (T52present == 3))
                    Anomalies.Add("ERROR: Mandatory Tag 52 is not present in any variantion");
            }

            return allTagsValid;
        }

        #region FIELD VALIDATIONS
        #region SEQUENCE A
        /// <summary>
        /// Is_T20_Valid
        /// Format
        ///     16x
        /// Presence
        ///     Mandatory 
        /// Definition
        ///     This field specifies the reference assigned by the Sender to unambiguously identify the message.
        /// Network Validated Rules    
        ///     This field must not start or end with a slash '/' and must not contain two consecutive slashes '//' (Error code(s): T26).
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool Is_T20_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 20 is a mandatory field it must be present
                if (field.Tag.Equals("20") == true)
                {
                    if (field.Present == 0)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                    }
                    field.Value = field.Value.Trim();
                    if (field.Value.Length > 16)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 16 characters.");
                    }
                    if (field.Value.Substring(0, 1).Equals("/") == true)
                    {
                        valid = false;
                        Anomalies.Add("ERROR T26 - Tag " + field.Tag + "," + field.Name + ", starts with a '/'");
                    }
                    if (field.Value.Substring(field.Value.Length - 1, 1).Equals("/") == true)
                    {
                        valid = false;
                        Anomalies.Add("ERROR T26 - Tag " + field.Tag + "," + field.Name + ", ends with a '/'");
                    }
                    if (field.Value.Contains("//") == true)
                    {
                        valid = false;
                        Anomalies.Add("ERROR T26 - Tag " + field.Tag + "," + field.Name + ", contains a '//'");
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T20_Valid");
                }
            }
            else
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T21_Valid
        /// Format
        ///     16x
        /// Presence
        ///     Mandatory
        /// Definition
        ///     This field contains the number of the cheque to which this message refers.
        /// Network Validated Rules
        ///     This field must not start or end with a slash '/' and must not contain two consecutive slashes '//' (Error code(s): T26).
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool Is_T21_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 21 is NOT a mandatory field.
                if (field.Tag.Equals("21") == true)
                {
                    if (field.Present == 1)
                    {
                        field.Value = field.Value.Trim();
                        if (field.Value.Length > 16)
                        {
                            valid = false;
                            Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 16 characters.");
                        }
                        if (field.Value.Substring(0, 1).Equals("/") == true)
                        {
                            valid = false;
                            Anomalies.Add("ERROR T26 - Tag " + field.Tag + "," + field.Name + ", starts with a '/'");
                        }
                        if (field.Value.Substring(field.Value.Length - 1, 1).Equals("/") == true)
                        {
                            valid = false;
                            Anomalies.Add("ERROR T26 - Tag " + field.Tag + "," + field.Name + ", ends with a '/'");
                        }
                        if (field.Value.Contains("//") == true)
                        {
                            valid = false;
                            Anomalies.Add("ERROR T26 - Tag " + field.Tag + "," + field.Name + ", contains a '//'");
                        }
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T21_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T30_Valid
        /// Presence
        ///     Mandatory
        /// Definition
        ///     This field contains the date on which the cheque was drawn.
        /// Network Validated Rules
        ///     Date must be a valid date expressed as YYMMDD (Error code(s): T50).
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool Is_T30_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            Util util = new Util();

            // 30T is a mandatory field in a mandatory block.
            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("30") == true)
                {
                    if (field.Present == 0)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                    }
                    field.Value = field.Value.Trim();
                    if (field.Value.Length != 6)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + " - Incorrect field length : " + field.Value.Length);
                    }
                    if ((util.IsDate(field.Value) == false))
                    {
                        valid = false;
                        Anomalies.Add("ERROR T50 - Tag " + field.Tag + " - is a date field. Must be in YYMMDD format");
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T30_Valid");
                }
            }
            else
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T32_Valid
        /// Format
        ///     Option A    6!n3!a15d   (date)(Currency)(Amount)
        ///     Option B	3!a15d      (Currency)(Amount)
        /// Presence
        ///     Mandatory
        /// Definition
        ///     This field identifies the currency and amount of the cheque; it may also specify the value date.
        /// Network Validated Rules
        ///     Date must be a valid date expressed as YYMMDD (Error code(s): T50).
        ///     Currency must be a valid ISO 4217 currency code(Error code(s): T52).
        ///     The integer part of Amount must contain at least one digit. A decimal comma is mandatory and is included in the maximum length. The number of digits following the comma must not exceed the maximum number allowed for the specified currency(Error code(s): C03, T40, T43).
        /// Usage Rules
        ///     When the message is in response to an MT 111 Request for Stop Payment of a Cheque, the contents of this field must be the same as field 32a of the MT 111.
        ///     If the request for stop payment has not been received via an MT 111, option A will be used when the drawer bank has previously credited the drawee bank with the cheque amount.It contains the value date, currency code and amount of the cheque.
        ///     In all other cases, option B must be used.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool Is_T32_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            Util util = new Util();
            string strValue = null;

            // 32B is a mandatory field in a mandatory block.
            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                strValue = field.Value;
                if (field.Tag.Equals("32B") == true)
                {
                    string date = strValue.Substring(0, 6);
                    DateTime result;
                    if (DateTime.TryParse(date, out result) == false)
                        Anomalies.Add("ERROR - Tag " + field.Tag + " - Incorrect date format ");

                    strValue = strValue.Substring(6, strValue.Length - 6);
                }

                if (field.Tag.Equals("32B") == true)
                {
                    if (field.Present == 0)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                    }
                    strValue = strValue.Trim();
                    if (field.Value.Length > 18)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + " - Incorrect field length : " + strValue.Length);
                    }

                    string ccy = strValue.Substring(0, 3);
                    if (util.IsValidCcy(ccy) == false)
                    {
                        valid = false;
                        Anomalies.Add("ERROR T52 - Tag " + field.Tag + " - Invalid currency : " + ccy);
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T32B_Valid");
                }
            }
            else
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T52_Valid
        /// Format
        ///     Option A	    [/1!a] [/34x]               (Party Identifier)
        ///                     4!a2!a2!c[3!c]              (Identifier Code)
        ///
        ///     Option B        [/ 1!a][/ 34x]              (Party Identifier)
        ///                     35x                         (Location)
        ///
        ///     Option D	    5*40x                       (Party Identification)
        ///                     4*35x                       (Name and Address)
        ///
        /// Presence
        ///     Optional 
        ///     
        /// Definition
        ///     This field identifies the drawer bank when other than the Sender.
        ///
        /// Codes
        ///     In option A, Party Identifier may be used to indicate a national clearing system code.
        ///     The following codes may be used, preceded by a double slash '//':
        ///
        ///     AT      5!n                         Austrian Bankleitzahl           
        ///     AU      6!n                         Australian Bank State Branch(BSB) Code
        ///     BL      8!n                         German Bankleitzahl
        ///     CC      9!n                         Canadian Payments Association Payment Routing Number
        ///     CN      12..14n                     China National Advanced Payment System(CNAPS) Code
        ///     ES      8..9n                       Spanish Domestic Interbanking Code
        ///     FW      without 9 digit code        Pay by Fedwire
        ///     GR      7!n                         HEBIC(Hellenic Bank Identification Code)
        ///     HK      3!n                         Bank Code of Hong Kong
        ///     IE      6!n                         Irish National Clearing Code(NSC)
        ///     IN      11!c                        Indian Financial System Code(IFSC)
        ///     IT      10!n                        Italian Domestic Identification Code
        ///     PL      8!n                         Polish National Clearing Code(KNR)
        ///     PT      8!n                         Portuguese National Clearing Code
        ///     SC      6!n                         UK Domestic Sort Code
        ///
        /// Codes       
        ///     In option B or D, Party Identifier may be used to indicate a national clearing system code.
        ///     The following codes may be used, preceded by a double slash '//':
        ///     
        ///     AT      5!n                         Austrian Bankleitzahl
        ///     AU      6!n                         Australian Bank State Branch(BSB) Code
        ///     BL      8!n                         German Bankleitzahl
        ///     CC      9!n                         Canadian Payments Association Payment Routing Number
        ///     CH      6!n                         CHIPS Universal Identifier
        ///     CN      12..14n                     China National Advanced Payment System(CNAPS) Code
        ///     CP      4!n                         CHIPS Participant Identifier
        ///     ES      8..9n                       Spanish Domestic Interbanking Code
        ///     FW      9!n                         Fedwire Routing Number
        ///     GR      7!n                         HEBIC(Hellenic Bank Identification Code)
        ///     HK      3!n                         Bank Code of Hong Kong
        ///     IE      6!n                         Irish National Clearing Code(NSC)
        ///     IN      11!c                        Indian Financial System Code(IFSC)
        ///     IT      10!n                        Italian Domestic Identification Code
        ///     PL      8!n                         Polish National Clearing Code(KNR)
        ///     PT      8!n                         Portuguese National Clearing Code
        ///     RU      9!n                         Russian Central Bank Identification Code
        ///     SC      6!n                         UK Domestic Sort Code
        ///     SW      3..5n                       Swiss Clearing Code(BC code)
        ///     SW      6!n                         Swiss Clearing Code(SIC code)
        ///     
        /// Network Validated Rules
        ///     Identifier Code must be a registered financial institution BIC (Error code(s): T27, T28, T29, T45).
        ///     Identifier Code must be a financial institution BIC.This error code applies to all types of BICs referenced in a FIN message including connected BICs, non-connected BICs, Masters, Synonyms, Live destinations and Test & Training destinations (Error code(s): C05).
        ///     
        /// Usage Rules
        ///     This field will be used when the drawer bank is a branch of the Receiver or a bank other than the Receiver of the message.
        ///     The coded information contained in field 52a must be meaningful to the Receiver of the message.
        ///     Option A is the preferred option.
        ///     Option D should only be used when the ordering financial institution has no BIC.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool Is_T52_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 52A / 52B / 52D is an optional field.
                field.Value = field.Value.Trim();
                if ((field.Tag.Equals("52A") == true) && (field.Present == 1))
                {
                    if (field.Value.Length > 48)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 48 characters.");
                    }
                }
                else if ((field.Tag.Equals("52B") == true) && (field.Present == 1))
                {
                    if (field.Value.Length > 72)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 72 characters.");
                    }
                }
                else if ((field.Tag.Equals("52D") == true) && (field.Present == 1))
                {
                    if (field.Value.Length > 177)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 177 characters.");
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T52_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T59_Valid
        /// Format
        ///     [/34x]          (Account)
        ///     4*35x           (Name and Address)
        /// Presence
        ///     Optional
        /// Definition
        ///     This field identifies the beneficiary of the cheque.
        /// Usage Rules
        ///     Account must not be used.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool Is_T59_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 21 is NOT a mandatory field.
                if (field.Tag.Equals("59") == true)
                {
                    if (field.Present == 1)
                    {
                        field.Value = field.Value.Trim();
                        if (field.Value.Length > 174)
                        {
                            valid = false;
                            Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 174 characters.");
                        }
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T59_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T75_Valid
        /// Format
        ///     6*35x           (Narrative)
        ///     
        /// In addition to narrative text, the following line formats may be used:
        ///     Line 1	            /2n/[supplement 1][supplement 2]                        (Query Number) (Narrative1) (Narrative2)
        ///     Lines 2-6	        [//continuation of supplementary information]           (Narrative)
        ///                                     or                                              or
        ///                         [/2n/[supplement 1] [supplement 2]]                     (Query Number)(Narrative1) (Narrative2)
        /// Presence
        ///     Optional
        /// Definition
        ///     This field may contain either the reason for stopping the payment of the cheque or a request for reimbursement authorisation.
        ///     
        /// Codes
        /// For frequently used answer texts, the following predefined Answer Numbers may be used:
        /// 3       We have been advised that the beneficiary did not receive payment/cheque.Please state if and when the transaction was effected.
        /// 18      Please authorise us to debit your account.
        /// 19      Please refund cover to credit of(1) ... (account/place).
        /// 20      Cheque/draft not debited as of closing balance of statement(1) ... (number) dated(2) ... (YYMMDD).
        /// 21      Cheque has been stolen/lost.
        /// 
        /// Usage Rules
        ///     Where a message contains more than one query, each query must appear on a separate line.
        ///     Numbers in brackets, for example, (1), mean that supplementary information is required.This supplementary information must be the first information following the code number.
        ///     When supplement 2 is used, that is, two different pieces of supplementary information are provided, the second piece of information should be preceded by a slash '/'.
        ///     
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool Is_T75_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 21 is NOT a mandatory field.
                if (field.Tag.Equals("75") == true)
                {
                    if (field.Present == 1)
                    {
                        field.Value = field.Value.Trim();
                        if (field.Value.Length > 210)
                        {
                            valid = false;
                            Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 210 characters.");
                        }
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T75_Valid");
                }
            }

            return valid;
        }
        #endregion
        #endregion
        #endregion

        #region TAG PARSING
        /// <summary>
        /// getT20_TransactionReferenceNumber
        /// 
        /// Returns the reference assigned by the Sender to unambiguously identify the message.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT20_TransactionReferenceNumber(List<TagData<string, string, string, string, int>> seq)
        {
            return getT20(seq);
        }

        /// <summary>
        /// getT21_RelatedReference
        /// 
        /// Returns the number of the cheque to which this message refers.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT21_ChequeNumber(List<TagData<string, string, string, string, int>> seq)
        {
            return getT21(seq);
        }

        /// <summary>
        /// getT30_DateOfIssue
        /// 
        /// Returns the date on which the cheque was drawn.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT30_DateOfIssue(List<TagData<string, string, string, string, int>> seq)
        {
            return getT30(seq);
        }

        /// <summary>
        /// getT32A_Date
        /// 
        /// Returns the date on which the cheque was drawn.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT32A_Date(List<TagData<string, string, string, string, int>> seq)
        {
            string date = null;
            string ccy = null;
            Nullable<double> amount = null;

            try
            {
                parseDateCcyAmt(seq, "32A", out date, out ccy, out amount);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return date;
        }

        /// <summary>
        /// getT32A_Currency
        /// 
        /// Returns the currency on which the cheque was drawn.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT32A_Currency(List<TagData<string, string, string, string, int>> seq)
        {
            string date = null;
            string ccy = null;
            Nullable<double> amount = null;

            try
            {
                parseDateCcyAmt(seq, "32A", out date, out ccy, out amount);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ccy;
        }

        /// <summary>
        /// getT32A_Amount
        /// 
        /// Returns the amount of the cheque drawn.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public Nullable<double> getT32A_Amount(List<TagData<string, string, string, string, int>> seq)
        {
            string date = null;
            string ccy = null;
            Nullable<double> amount = null;

            try
            {
                parseDateCcyAmt(seq, "32A", out date, out ccy, out amount);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return amount;
        }

        /// <summary>
        /// getT32B_Currency
        /// 
        /// Returns the currency on which the cheque was drawn.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT32B_Currency(List<TagData<string, string, string, string, int>> seq)
        {
            string ccy = null;
            Nullable<double> amount = null;

            try
            {
                parseCcyAmt(seq, "32B", out ccy, out amount);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ccy;
        }

        /// <summary>
        /// getT32B_Amount
        /// 
        /// Returns the amount of the cheque drawn.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public Nullable<double> getT32B_Amount(List<TagData<string, string, string, string, int>> seq)
        {
            string ccy = null;
            Nullable<double> amount = null;

            try
            {
                parseCcyAmt(seq, "32B", out ccy, out amount);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return amount;
        }

        /// <summary>
        /// getT52A_ID
        /// 
        /// Returns the id of the financial institution from which party A will transfer the funds.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT52A_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "52A");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT52A_Code
        /// 
        /// Returns the code of the financial institution from which party A will transfer the funds.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT52A_Code(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "52A");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[1];
        }

        /// <summary>
        /// getT52B_ID
        /// 
        /// Returns the id of the financial institution from which party A will transfer the funds.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT52B_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "52B");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT52B_Location
        /// 
        /// Returns the location of the financial institution from which party A will transfer the funds.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT52B_Location(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "52B");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[1];
        }

        /// <summary>
        /// getT52D_ID
        /// 
        /// Returns the id of the financial institution from which party A will transfer the funds.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT52D_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "52D");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT52D_NameAddr
        /// 
        /// Returns the name and address of the financial institution from which party A will transfer the funds.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT52D_NameAddr(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "52D");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[1];
        }

        /// <summary>
        /// getT59_PayeeAccount
        /// 
        /// Returns the identifies the beneficiary of the cheque.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT59_PayeeAccount(List<TagData<string, string, string, string, int>> seq)
        {
            string rawStr = GetTagValue(seq, "59");
            List<string> retLst = new List<string>();
            string[] stringSeparators = new string[] { "\r\n" };

            try
            {
                if (rawStr != null && rawStr.Length >= 1 && isTagPresentInSequence(seq, "59") == true)
                {
                    if (rawStr[0].Equals("/"))
                    {
                        string[] lines = rawStr.Split(stringSeparators, StringSplitOptions.None);
                        retLst.Add(lines[0].Substring(1));  // remoive the first '/'
                        retLst.Add(lines[1]);
                    }
                    else
                    {
                        retLst.Add(null);
                        retLst.Add(rawStr);
                    }
                }
                else
                {
                    retLst.Add(null);
                    retLst.Add(null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retLst[0];
        }

        /// <summary>
        /// getT59_PayeeNameAddr
        /// 
        /// Returns the identifies the beneficiary of the cheque.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT59_PayeeNameAddr(List<TagData<string, string, string, string, int>> seq)
        {
            string rawStr = GetTagValue(seq, "59");
            List<string> retLst = new List<string>();
            string[] stringSeparators = new string[] { "\r\n" };

            try
            {
                if (rawStr != null && rawStr.Length >= 1 && isTagPresentInSequence(seq, "59") == true)
                {
                    if (rawStr[0].Equals("/"))
                    {
                        string[] lines = rawStr.Split(stringSeparators, StringSplitOptions.None);
                        retLst.Add(lines[0].Substring(1));  // remoive the first '/'
                        retLst.Add(lines[1]);
                    }
                    else
                    {
                        retLst.Add(null);
                        retLst.Add(rawStr);
                    }
                }
                else
                {
                    retLst.Add(null);
                    retLst.Add(null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retLst[1];
        }

        /// <summary>
        /// getT75_Answers
        /// 
        /// Returns either the reason for stopping the payment of the cheque or a request for reimbursement authorisation.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT75_Queries(List<TagData<string, string, string, string, int>> seq)
        {
            return getT75(seq);
        }
        #endregion

        #region SAVE DATA
        public void saveRecord(BlockHeader headers)
        {
            string sqlCmd = null;
            long ref_id = -1;

            if (headers != null)
            {
                try
                {
                    sqlCmd = "Select max(reference_id) from dbo.MT111_Block1";
                    ref_id = dbu.getNewReferenceId(sqlCmd, -1);

                    dbu.DBBegin(ref_id.ToString());
                    saveBlock1(ref_id, headers);
                    saveBlock2(ref_id, headers);
                    saveBlock3(ref_id, headers);
                    saveBlock4(ref_id);
                    saveBlock5(ref_id, headers);
                    dbu.DBCommit(ref_id.ToString());
                }
                catch (Exception ex)
                {
                    dbu.DBRollback(ref_id.ToString());
                    throw ex;
                }
            }
        }

        private void saveBlock1(long refid, BlockHeader hdr)
        {
            string sqlCmd = null;

            if (hdr == null)
                return;

            try
            {
                sqlCmd = "INSERT INTO dbo.MT111_Block1 (reference_id, application_id, service_id, lt_address, bic_code, logical_terminal_code, bic_branch_code, session_number, sequence_number) ";
                sqlCmd += "VALUES('" + refid + "', '" + hdr.ApplicationID + "', '" + hdr.ServiceID + "', '" + hdr.LTAddress + "', '" + hdr.BICCode + "', '" + hdr.LogicalTerminalCode + "', '" + hdr.BICBranchCode + "', '" + hdr.SessionNumber + "', '" + hdr.SequenceNumber + "')";

                dbu.saveMTRecord(sqlCmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to insert MT111 Block1 record.\n" + ex.Message);
            }
        }

        private void saveBlock2(long refid, BlockHeader hdr)
        {
            string sqlCmd = null;

            if (hdr == null)
                return;

            try
            {
                sqlCmd = "INSERT INTO dbo.MT111_Block2 (reference_id, input_output_id, message_type, destination_address, priority, delivery_monitoring, ";
                sqlCmd += "obsolescence_period, input_time, mir, mir_sender_date, mir_lt_address, mir_bic_code,  mir_lt_code, mir_bic_branch_code, ";
                sqlCmd += "mir_session_number, mir_sequence_number, output_date, output_time) ";
                sqlCmd += "VALUES('" + refid + "', '" + hdr.InputOutputID + "', '" + hdr.MessageType + "', '" + hdr.DestinationAddress + "', '";
                sqlCmd += hdr.Priority + "', '" + hdr.DeliveryMonitoring + "', '" + hdr.ObsolescencePeriod + "', '" + hdr.InputTime + "', '";
                sqlCmd += hdr.MIR + "', '" + hdr.MIRSenderDate + "', '" + hdr.MIRLTAddress + "', '" + hdr.MIRBICCode + "',  '" + hdr.MIRLTCode + "', '";
                sqlCmd += hdr.BICBranchCode + "', '" + hdr.MIRSessNum + "', '" + hdr.MIRSeqNum + "', '" + hdr.OutputDate + "', '" + hdr.OutputTime + "')";

                dbu.saveMTRecord(sqlCmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to insert MT111 Block2 record.\n" + ex.Message);
            }
        }

        private void saveBlock3(long refid, BlockHeader hdr)
        {
            string sqlCmd = null;

            if (hdr == null)
                return;

            try
            {
                sqlCmd = "INSERT INTO dbo.MT111_Block3 (reference_id, tag103_service_id, tag113_banking_priority, tag108_mur, tag119_validation_flag, ";
                sqlCmd += "tag423_balance_check_point, tag106_mir, tag424_related_reference, tag111_service_type_id, tag121_unique_tran_reference, ";
                sqlCmd += "tag115_addressee_info, tag165_payment_rir, tag433_sanctions_sir, tag434_payment_cir) ";
                sqlCmd += "VALUES('" + refid + "', '" + hdr.TAG103_ServiceID + "', '" + hdr.TAG113_BankingPriority + "', '" + hdr.TAG108_MUR + "', '";
                sqlCmd += hdr.TAG119_ValidationFlag + "', '" + hdr.TAG423_BalanceCheckPoint + "', '" + hdr.TAG106_MIR + "', '" + hdr.TAG424_RelatedReference + "', '";
                sqlCmd += hdr.TAG111_ServiceTypeID + "', '" + hdr.TAG121_UniqueTranReference + "', '" + hdr.TAG115_AddresseeInfo + "', '" + hdr.TAG165_PaymentRIR + "', '";
                sqlCmd += hdr.TAG433_SanctionsSIR + "', '" + hdr.TAG434_PaymentCIR + "')";

                dbu.saveMTRecord(sqlCmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to insert MT111 Block3 record.\n" + ex.Message);
            }
        }

        private void saveBlock4(long refid)
        {
            string sqlCmd = null;

            try
            {
                sqlCmd = "INSERT INTO dbo.MT111_SequenceA (reference_id, transaction_reference_number_20, cheque_number_21, date_of_issue_30, date_32a, currency_32a, ";
                sqlCmd += "amount_32a, currency_32b, amount_32b, party_id_52a, party_code_52a, party_id_52b, party_location_52b, party_id_52d, party_name_addr_52d, ";
                sqlCmd += "payee_account_59, payee_name_addr_59, queries_75)";
                sqlCmd += "VALUES ('" + refid + "', '" +
                                    getT20_TransactionReferenceNumber(sequenceA) + "', '" +
                                    getT21_ChequeNumber(sequenceA) + "', '" +
                                    getT30_DateOfIssue(sequenceA) + "', '" +
                                    getT32A_Date(sequenceA) + "', '" +
                                    getT32A_Currency(sequenceA) + "', '" +
                                    getT32A_Amount(sequenceA) + "', '" +
                                    getT32B_Currency(sequenceA) + "', '" +
                                    getT32B_Amount(sequenceA) + "', '" +
                                    getT52A_ID(sequenceA) + "', '" +
                                    getT52A_Code(sequenceA) + "', '" +
                                    getT52B_ID(sequenceA) + "', '" +
                                    getT52B_Location(sequenceA) + "', '" +
                                    getT52D_ID(sequenceA) + "', '" +
                                    getT52D_NameAddr(sequenceA) + "', '" +
                                    getT59_PayeeAccount(sequenceA) + "', '" +
                                    getT59_PayeeNameAddr(sequenceA) + "', '" +
                                    getT75_Queries(sequenceA) + "')";
                dbu.saveMTRecord(sqlCmd);


            }
            catch (Exception ex)
            {
                throw new Exception("Failed to insert MT111 Block4 record.\n" + ex.Message);
            }
        }

        private void saveBlock5(long refid, BlockHeader hdr)
        {
            string sqlCmd = null;

            if (hdr == null)
                return;

            try
            {
                sqlCmd = "INSERT INTO dbo.MT111_Block5 (reference_id, checksum, tng_message, pde, pde_time, pde_mir, pde_mir_date, pde_mir_lt_id, ";
                sqlCmd += "pde_mir_session_number, pde_mir_sequence_number, dlm, mrf, mrf_date, mrf_time, mrf_mir, pdm, pdm_time,  pdm_mor, ";
                sqlCmd += "pdm_mor_date, pdm_mor_lt_id, pdm_mor_session_number, pdm_mor_sequence_number, sys, sys_time, sys_mor, sys_mor_date, ";
                sqlCmd += "sys_mor_lt_id, sys_mor_session_number, sys_mor_sequence_number)";
                sqlCmd += "VALUES('" + refid + "', '" + hdr.Checksum + "', '" + hdr.TNGMessage + "', '" + hdr.PDE + "', '" + hdr.PDETime + "', '";
                sqlCmd += hdr.PDEMir + "', '" + hdr.PDEMirDate + "', '" + hdr.PDEMirLTId + "', '" + hdr.PDEMirSessionNum + "', '" + hdr.PDEMirSequenceNum + "', '";
                sqlCmd += hdr.DLM + "', '" + hdr.MRF + "', '" + hdr.MRFDate + "', '" + hdr.MRFTime + "', '" + hdr.MRFMir + "', '" + hdr.PDM + "', '";
                sqlCmd += hdr.PDMTime + "', '" + hdr.PDMMor + "', '" + hdr.PDMMorDate + "', '" + hdr.PDMMorLTId + "', '" + hdr.PDMMorSessionNum + "', '";
                sqlCmd += hdr.PDMMorSequenceNum + "', '" + hdr.SYS + "', '" + hdr.SYSTime + "', '" + hdr.SYSMor + "', '" + hdr.SYSMorDate + "', '";
                sqlCmd += hdr.SYSMorLTId + "', '" + hdr.SYSMorSessionNum + "', '" + hdr.SYSMorSequenceNum + "')";

                dbu.saveMTRecord(sqlCmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to insert MT111 Block5 record.\n" + ex.Message);
            }
        }
        #endregion


        public void testFunctions()
        {


        }
    }
}
