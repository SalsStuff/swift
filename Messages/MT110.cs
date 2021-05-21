using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;

namespace Messages
{
    //https://www2.swift.com/knowledgecentre/publications/us1m_20200724/2.0?topic=mt110-format-spec.htm
    public class MT110 : MTMessage
    {
        DBUtils dbu = new DBUtils();

        #region SEQUENCE_VARIABLES
        // Sequence A - Mandatory
        // Sequence A General Information contains information about the request of a stop payment of a cheque.
        List<TagData<string, string, string, string, int>> sequenceA = new List<TagData<string, string, string, string, int>>
        {
            // Tag, Name, Value, Mandatory
            new TagData<string, string, string, string, int>("20",  "Senders Reference",                "" ,"M", 0),
            new TagData<string, string, string, string, int>("53A", "Senders Correspondent",            "", "O", 0),
            new TagData<string, string, string, string, int>("53B", "Senders Correspondent",            "", "O", 0),
            new TagData<string, string, string, string, int>("53D", "Senders Correspondent",            "", "O", 0),
            new TagData<string, string, string, string, int>("54A", "Receivers Correspondent",          "", "O", 0),
            new TagData<string, string, string, string, int>("54B", "Receivers Correspondent",          "", "O", 0),
            new TagData<string, string, string, string, int>("54D", "Receivers Correspondent",          "", "O", 0),
            new TagData<string, string, string, string, int>("72",  "Sender To Receiver Information",   "", "O", 0),
            new TagData<string, string, string, string, int>("21",  "Cheque Number",                    "" ,"M", 0),
            new TagData<string, string, string, string, int>("30",  "Date of Issue",                    "" ,"M", 0),
            new TagData<string, string, string, string, int>("32A", "Amount",                           "", "M", 0),
            new TagData<string, string, string, string, int>("32B", "Amount",                           "", "M", 0),
            new TagData<string, string, string, string, int>("50A", "Payer",                            "", "O", 0),
            new TagData<string, string, string, string, int>("50F", "Payer",                            "", "O", 0),
            new TagData<string, string, string, string, int>("50K", "Payer",                            "", "O", 0),
            new TagData<string, string, string, string, int>("52A", "Drawer Bank",                      "", "O", 0),
            new TagData<string, string, string, string, int>("52B", "Drawer Bank",                      "", "O", 0),
            new TagData<string, string, string, string, int>("52D", "Drawer Bank",                      "", "O", 0),
            new TagData<string, string, string, string, int>("59",  "Payee",                            "", "M", 0),
            new TagData<string, string, string, string, int>("59F", "Payee",                            "", "M", 0)
        };
        #endregion

        #region MESSAGE SETUP
        /// <summary>
        /// Method Constructor
        /// </summary>
        public MT110()
        {
            InitializeMT110();
        }

        /// <summary>
        /// Method Constructor
        /// </summary>
        /// <param name="msg"></param>
        public MT110(String msg)
        {
            InitializeMT110();

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

        private void InitializeMT110()
        {
            numOfSequences = 1;
            ResetVariables();
            DefineScope();
            Anomalies.Clear();
        }

        /// <summary>
        /// Definition of Message Scope
        /// This is the SWIFT definition of the message
        /// It can be found at https://www2.swift.com/knowledgecentre/publications/us1m_20200724/2.0?topic=mt110-scope.htm
        /// </summary>
        protected override void DefineScope()
        {
            Scope = "MT 110 Scope:\r\n" +
                    "This multiple message is sent by a drawer bank, or a bank acting on behalf of the drawer bank to the bank on which a/several cheque(s) has been drawn (the drawee bank).\r\n" +
                    "It is used to advise the drawee bank, or confirm to an enquiring bank, the details concerning the cheque(s) referred to in the message.\r\n";
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

        /// <summary>
        /// Set method to set a MT320 tag value
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        protected override void SetTagValue(List<TagData<string, string, string, string, int>> sequence, string tag, string value)
        {
            switch (whichSequence(sequence))
            {
                case "A":
                    foreach (TagData<string, string, string, string, int> t in sequence)
                    {
                        if (tag.Equals("21") == true && t.Tag.Equals("21") && !t.Value.Equals(""))
                        {
                            sequence.Add(new TagData<string, string, string, string, int>("21", "Cheque Number", value, "O", 1));
                            break;
                        }
                        else if(tag.Equals("30") == true && t.Tag.Equals("30") && !t.Value.Equals(""))
                        {
                            sequence.Add(new TagData<string, string, string, string, int>("30", "Date of Issue", value, "M", 1));
                            break;
                        }
                        else if (tag.Equals("32A") == true && t.Tag.Equals("32A") && !t.Value.Equals(""))
                        {
                            sequence.Add(new TagData<string, string, string, string, int>("32A", "Amount", value, "M", 1));
                            break;
                        }
                        else if (tag.Equals("32B") == true && t.Tag.Equals("32B") && !t.Value.Equals(""))
                        {
                            sequence.Add(new TagData<string, string, string, string, int>("32B", "Amount", value, "M", 1));
                            break;
                        }
                        else if (tag.Equals("50A") == true && t.Tag.Equals("50A") && !t.Value.Equals(""))
                        {
                            sequence.Add(new TagData<string, string, string, string, int>("50A", "Payer", value, "O", 1));
                            break;
                        }
                        else if (tag.Equals("50F") == true && t.Tag.Equals("50F") && !t.Value.Equals(""))
                        {
                            sequence.Add(new TagData<string, string, string, string, int>("50F", "Payer", value, "O", 1));
                            break;
                        }
                        else if (tag.Equals("50K") == true && t.Tag.Equals("50K") && !t.Value.Equals(""))
                        {
                            sequence.Add(new TagData<string, string, string, string, int>("50K", "Payer", value, "O", 1));
                            break;
                        }
                        else if (tag.Equals("52A") == true && t.Tag.Equals("52A") && !t.Value.Equals(""))
                        {
                            sequence.Add(new TagData<string, string, string, string, int>("52A", "Drawer Bank", value, "O", 1));
                            break;
                        }
                        else if (tag.Equals("52B") == true && t.Tag.Equals("52B") && !t.Value.Equals(""))
                        {
                            sequence.Add(new TagData<string, string, string, string, int>("52B", "Drawer Bank", value, "O", 1));
                            break;
                        }
                        else if (tag.Equals("52D") == true && t.Tag.Equals("52D") && !t.Value.Equals(""))
                        {
                            sequence.Add(new TagData<string, string, string, string, int>("52D", "Drawer Bank", value, "O", 1));
                            break;
                        }
                        else if (tag.Equals("59") == true && t.Tag.Equals("59") && !t.Value.Equals(""))
                        {
                            sequence.Add(new TagData<string, string, string, string, int>("59", "Payee", value, "M", 1));
                            break;
                        }
                        else if (tag.Equals("59F") == true && t.Tag.Equals("59F") && !t.Value.Equals(""))
                        {
                            sequence.Add(new TagData<string, string, string, string, int>("59F", "Payee", value, "M", 1));
                            break;
                        }
                        else if (tag.Equals(t.Tag) == true)
                        {
                            t.Value = value;
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region GET SET FUNCTIONS
        /// <summary>
        /// Get / Set method to auto validate message.
        /// If set to true the message will be validated when read in.
        /// If set to false an explicit call will be needed after the message is read in.
        /// </summary>
        public bool AutoValidate { get; set; } = true;
        #endregion

        #region VALIDATION RULES
        public bool IsMessageValid()
        {
            bool validMessage = true;

            if (AutoValidate == true)
            {
                ValidateTags();

                Valid_VR_C1();
                Valid_VR_C2();

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
                            if (Is_T32A_Valid(field) == false)
                                validTag = false;
                            break;
                        case "32B":
                            if (Is_T32B_Valid(field) == false)
                                validTag = false;
                            break;
                        case "50A":
                        case "50F":
                        case "50K":
                            if (Is_T50_Valid(field) == false)
                                validTag = false;
                            break;
                        case "52A":
                        case "52B":
                        case "52D":
                            if (Is_T52_Valid(field) == false)
                                validTag = false;
                            break;
                        case "53A":
                        case "53B":
                        case "53D":
                            if (Is_T53_Valid(field) == false)
                                validTag = false;
                            break;
                        case "54A":
                        case "54B":
                        case "54D":
                            if (Is_T54_Valid(field) == false)
                                validTag = false;
                            break;
                        case "59":
                        case "59F":
                            if (Is_T59_Valid(field) == false)
                                validTag = false;
                            break;
                        case "72":
                            if (Is_T72_Valid(field) == false)
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
            int T59present = 0;

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
                            if ((t.Tag.Equals("59") && t.Present == 0) || (t.Tag.Equals("59F") && t.Present == 0))
                            {
                                validateField = false;
                                T59present++;
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

                if ((sid.Equals("A") == true) && (T32present == 2))
                    Anomalies.Add("ERROR: Mandatory Tag 32 is not present in any variantion");
                if ((sid.Equals("A") == true) && (T59present == 2))
                    Anomalies.Add("ERROR: Mandatory Tag 59 is not present in any variantion");
            }

            return allTagsValid;
        }

        #region Network Validated Rules
        /// <summary>
        /// Validation rule C1 
        /// The repetitive sequence must not be present more than ten times (Error code(s): T10).
        /// </summary>
        /// <returns></returns>
        private bool Valid_VR_C1()
        {
            bool valid = true;
            int count21 = 0;

            foreach (TagData<string, string, string, string, int> t in sequenceA)
            {
                if (t.Tag.Equals("21") == true)
                    count21++;
            }
            
            if (count21 > 10)
            { 
                    valid = false;
                    Anomalies.Add("ERROR T10 : There are more than 10 repetitive sections in this message.");
            }

            return valid;
        }

        /// <summary>
        /// Validation Rule C2
        /// The currency code in the amount field 32a must be the same for all occurrences of this field in the message (Error code(s): C02).
        /// </summary>
        /// <returns></returns>
        private bool Valid_VR_C2()
        {
            bool valid = true;
            List<string> currencies = new List<string>();
            List<string> filteredCcys = new List<string>();

            foreach (TagData<string, string, string, string, int> t in sequenceA)
            {
                if (t.Tag.Equals("32A") == true && t.Present == 1)
                {
                    currencies.Add(t.Value.Substring(6, 3));
                }
                else if (t.Tag.Equals("32B") == true && t.Present == 1)
                {
                    currencies.Add(t.Value.Substring(0, 3));
                }
            }

            filteredCcys = currencies.Distinct().ToList();
            if (filteredCcys.Count() > 1)
            {
                valid = false;
                Anomalies.Add("ERROR C02 : There are multiple currencies in this message.");
            }

            return valid;
        }
        #endregion

        #region FIELD VALIDATIONS
        #region SEQUENCE A

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
        public string getT20_SendersReference(List<TagData<string, string, string, string, int>> seq)
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
            List<string> data = getT32A(seq);

            return data[0];
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
            List<string> data = getT32A(seq);

            return data[1];
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
            List<string> data = getT32A(seq);
            Nullable<double> amount = null;

            if (data[2] != null)
                amount = Convert.ToDouble(data[2]);

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
            List<string> data = getT32B(seq);

            return data[0];
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
            List<string> data = getT32B(seq);
            Nullable<double> amount = null;

            if (data[2] != null)
                amount = Convert.ToDouble(data[2]);

            return amount;
        }

        /// <summary>
        /// getT50A_Account
        /// 
        /// Returns the account of the payer
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT50A_Account(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = getT50A(seq);

            return lst[0];
        }

        /// <summary>
        /// getT50A_Code
        /// 
        /// Returns the identifier code of the payer
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT50A_Code(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = getT50A(seq);

            return lst[1];
        }

        /// <summary>
        /// getT50F_Id
        /// 
        /// Returns the party identifier of the payer
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT50F_Id(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = getT50F(seq);

            return lst[0];
        }

        /// <summary>
        /// getT50F_Code
        /// 
        /// Returns the name and address of the payer
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT50F_NameAddr(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = getT50F(seq);

            return lst[1];
        }

        /// <summary>
        /// getT50K_Account
        /// 
        /// Returns the account of the payer
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT50K_Account(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = getT50K(seq);

            return lst[0];
        }

        /// <summary>
        /// getT50K_NameAddr
        /// 
        /// Returns the account of the payer
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT50K_NameAddr(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = getT50K(seq);

            return lst[1];
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
            List<string> lst = getT52A(seq);

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
            List<string> lst = getT52A(seq);

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
            List<string> lst = getT52B(seq);

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
            List<string> lst = getT52B(seq);

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
            List<string> lst = getT52D(seq);

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
            List<string> lst = getT52D(seq);

            return lst[1];
        }

        /// <summary>
        /// getT53A_ID
        /// 
        /// Returns the id of the branch of the sender
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT53A_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = getT53A(seq);
            
            return lst[0];
        }

        /// <summary>
        /// getT53A_Code
        /// 
        /// Returns the code of the branch of the sender
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT53A_Code(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = getT53A(seq);

            return lst[1];
        }

        /// <summary>
        /// getT53B_ID
        /// 
        /// Returns the id of the branch of sender
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT53B_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = getT53B(seq);

            return lst[0];
        }

        /// <summary>
        /// getT53B_Location
        /// 
        /// Returns the location of the branch of the sender
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT53B_Location(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = getT53B(seq);

            return lst[1];
        }

        /// <summary>
        /// getT53D_ID
        /// 
        /// Returns the id of the branch of sender
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT53D_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = getT53D(seq);

            return lst[0];
        }

        /// <summary>
        /// getT53D_NameAddr
        /// 
        /// Returns the name and address of the branch of party A with whom the deal was done.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT53D_NameAddr(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = getT53D(seq);

            return lst[1];
        }

        /// <summary>
        /// getT54A_ID
        /// 
        /// Returns the id of the branch of the receiver
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT54A_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = getT54A(seq);

            return lst[0];
        }

        /// <summary>
        /// getT54A_Code
        /// 
        /// Returns the code of the branch of the receiver
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT54A_Code(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = getT54A(seq);

            return lst[1];
        }

        /// <summary>
        /// getT54B_ID
        /// 
        /// Returns the id of the branch of receiver
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT54B_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = getT54B(seq);

            return lst[0];
        }

        /// <summary>
        /// getT54B_Location
        /// 
        /// Returns the location of the branch of the receiver
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT54B_Location(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = getT54B(seq);

            return lst[1];
        }

        /// <summary>
        /// getT54D_ID
        /// 
        /// Returns the id of the branch of receiver
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT54D_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = getT54D(seq);

            return lst[0];
        }

        /// <summary>
        /// getT54D_NameAddr
        /// 
        /// Returns the name and address of the branch of receiver with whom the deal was done.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT54D_NameAddr(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = getT54D(seq);

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
            List<string> retLst = getT59(seq);

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
            List<string> retLst = getT59(seq);

            return retLst[1];
        }

        /// <summary>
        /// getT59F_PayeeAccount
        /// 
        /// Returns the identifies the beneficiary of the cheque.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT59F_PayeeAccount(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> retLst = getT59F(seq);

            return retLst[0];
        }

        /// <summary>
        /// getT59F_PayeeNameAddr
        /// 
        /// Returns the identifies the beneficiary of the cheque.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT59F_PayeeNameAddr(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> retLst = getT59F(seq);

            return retLst[1];
        }

        /// <summary>
        /// getT72_SenderReceiverInfo
        /// 
        /// Returns either the additional information for the Receiver or other party specified.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT72_SenderReceiverInfo(List<TagData<string, string, string, string, int>> seq)
        {
            return getT72(seq);
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
                    sqlCmd = "Select max(reference_id) from dbo.MT110_Block1";
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
                sqlCmd = "INSERT INTO dbo.MT110_Block1 (reference_id, application_id, service_id, lt_address, bic_code, logical_terminal_code, bic_branch_code, session_number, sequence_number) ";
                sqlCmd += "VALUES('" + refid + "', '" + hdr.ApplicationID + "', '" + hdr.ServiceID + "', '" + hdr.LTAddress + "', '" + hdr.BICCode + "', '" + hdr.LogicalTerminalCode + "', '" + hdr.BICBranchCode + "', '" + hdr.SessionNumber + "', '" + hdr.SequenceNumber + "')";

                dbu.saveMTRecord(sqlCmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to insert MT110 Block1 record.\n" + ex.Message);
            }
        }

        private void saveBlock2(long refid, BlockHeader hdr)
        {
            string sqlCmd = null;

            if (hdr == null)
                return;

            try
            {
                sqlCmd = "INSERT INTO dbo.MT110_Block2 (reference_id, input_output_id, message_type, destination_address, priority, delivery_monitoring, ";
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
                sqlCmd = "INSERT INTO dbo.MT110_Block3 (reference_id, tag103_service_id, tag113_banking_priority, tag108_mur, tag119_validation_flag, ";
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
                throw new Exception("Failed to insert MT110 Block3 record.\n" + ex.Message);
            }
        }

        private void saveBlock4(long refid)
        {
            string sqlCmd = null;
            List<string> inData = new List<string>();
            int i = 0;

            try
            {
                createSaveData(refid, out inData);

                for (i = 0; i < inData.Count(); i++)
                {
                    sqlCmd = null;
                    sqlCmd = "INSERT INTO dbo.MT110_SequenceA (reference_id, senders_reference_20, senders_correspondent_party_id_53a, senders_correspondent_code_53a, senders_correspondent_party_id_53b, ";
                    sqlCmd += "senders_correspondent_loc_53b, senders_correspondent_party_id_53d, senders_correspondent_name_addr_53d, senders_correspondent_party_id_54a, ";
                    sqlCmd += "senders_correspondent_code_54a, senders_correspondent_party_id_54b, senders_correspondent_loc_54b, senders_correspondent_party_id_54d, ";
                    sqlCmd += "senders_correspondent_name_addr_54d, sender_receiver_info_72, cheque_number_21, date_of_issue_30, date_32a, currency_32a, amount_32a, currency_32b, ";
                    sqlCmd += "amount_32b, payer_account_50a, payer_code_50a, payer_party_id_50f, payer_name_addr_50f, payer_account_50k, payer_name_addr_50k, ";
                    sqlCmd += "party_id_52a, party_code_52a, party_id_52b, party_location_52b, party_id_52d, party_name_addr_52d, payee_account_59, payee_name_addr_59, ";
                    sqlCmd += "payee_account_59f, payee_name_addr_59f)";
                    sqlCmd += "VALUES ('" + inData[i];
                    dbu.saveMTRecord(sqlCmd);
                }


            }
            catch (Exception ex)
            {
                throw new Exception("Failed to insert MT110 Block4 record.\n" + ex.Message);
            }
        }

        private void saveBlock5(long refid, BlockHeader hdr)
        {
            string sqlCmd = null;

            if (hdr == null)
                return;

            try
            {
                sqlCmd = "INSERT INTO dbo.MT110_Block5 (reference_id, checksum, tng_message, pde, pde_time, pde_mir, pde_mir_date, pde_mir_lt_id, ";
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
                throw new Exception("Failed to insert MT110 Block5 record.\n" + ex.Message);
            }
        }

        private void createSaveData(long refId, out List<string> data)
        { 
            string header = null;
            string trailer = null;
            data = new List<string>();

            header = "'" + refId + "', '" + getT20_SendersReference(sequenceA) + "', '" + getT53A_ID(sequenceA) + "', '" +
                     getT53A_Code(sequenceA) + "', '" + getT53B_ID(sequenceA) + "', '" + getT53B_Location(sequenceA) + "', '" +
                     getT53D_ID(sequenceA) + "', '" + getT53D_NameAddr(sequenceA) + "', '" + getT54A_ID(sequenceA) + "', '" +
                     getT54A_Code(sequenceA) + "', '" + getT54B_ID(sequenceA) + "', '" + getT54B_Location(sequenceA) + "', '" +
                     getT54D_ID(sequenceA) + "', '" + getT54D_NameAddr(sequenceA) + "', '" + getT72_SenderReceiverInfo(sequenceA) + "', '";

            foreach (TagData<string, string, string, string, int> t in sequenceA)
            {
                switch(t.Tag)
                {
                    case "21":
                        trailer += getT21_ChequeNumber(sequenceA) + "', '";
                        break;
                    case "30":
                        trailer += getT30_DateOfIssue(sequenceA) + "', '";
                        break;
                    case "32A":
                        trailer += getT32A_Date(sequenceA) + "', '" + getT32A_Currency(sequenceA) + "', '" + getT32A_Amount(sequenceA) + "', '";
                        break;
                    case "32B":
                        trailer += getT32A_Currency(sequenceA) + "', '" + getT32A_Amount(sequenceA) + "', '";
                        break;
                    case "50A":
                        trailer += getT50A_Account(sequenceA) + "', '" + getT50A_Code(sequenceA) + "', '";
                        break;
                    case "50F":
                        trailer += getT50F_Id(sequenceA) + "', '" + getT50F_NameAddr(sequenceA) + "', '";
                        break;
                    case "50K":
                        trailer += getT50K_Account(sequenceA) + "', '" + getT50K_NameAddr(sequenceA) + "', '";
                        break;
                    case "52A":
                        trailer += getT52A_ID(sequenceA) + "', '" + getT52A_Code(sequenceA) + "', '";
                        break;
                    case "52B":
                        trailer += getT52B_ID(sequenceA) + "', '" + getT52B_Location(sequenceA) + "', '";
                        break;
                    case "52D":
                        trailer += getT52D_ID(sequenceA) + "', '" + getT52D_NameAddr(sequenceA) + "', '";
                        break;
                    case "59":
                        trailer += getT59_PayeeAccount(sequenceA) + "', '" + getT59_PayeeNameAddr(sequenceA) + "'";
                        data.Add(header + trailer);
                        trailer = null;
                        break;
                    case "59F":
                        trailer += getT59F_PayeeAccount(sequenceA) + "', '" + getT59F_PayeeNameAddr(sequenceA) + "'";
                        data.Add(header + trailer);
                        trailer = null;
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion


        public void testFunctions()
        {
            string ss = null;
            Nullable<double> dd = null;

            ss = getT32A_Date(sequenceA);
            ss = getT32A_Currency(sequenceA);
            dd = getT32A_Amount(sequenceA);
        }
    }
}
