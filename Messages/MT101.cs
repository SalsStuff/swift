using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;

namespace Messages
{
    class MT101SequenceA
    {
        // Sequence A - Mandatory
        // Sequence A General Information contains general information about the fixed loan/deposit as well as about the confirmation itself.
        public List<TagData<string, string, string, string, int>> seq = new List<TagData<string, string, string, string, int>>
        {
            // Tag, Name, Value, Mandatory
            new TagData<string, string, string, string, int>("20",  "Sender's Reference",           "" ,"M", 0),
            new TagData<string, string, string, string, int>("21R", "Customer Specified Reference", "" ,"O", 0),
            new TagData<string, string, string, string, int>("28D", "Message Index/Total",          "", "M", 0),
            new TagData<string, string, string, string, int>("50C", "Instructing Party",            "", "O", 0),
            new TagData<string, string, string, string, int>("50L", "Instructing Party",            "", "O", 0),
            new TagData<string, string, string, string, int>("50F", "Ordering Customer",            "", "O", 0),
            new TagData<string, string, string, string, int>("50G", "Ordering Customer",            "", "O", 0),
            new TagData<string, string, string, string, int>("50H", "Ordering Customer",            "", "O", 0),
            new TagData<string, string, string, string, int>("52A", "Account Serving Institution",  "" ,"O", 0),
            new TagData<string, string, string, string, int>("52C", "Account Serving Institution",  "" ,"O", 0),
            new TagData<string, string, string, string, int>("51A", "Sending Institution",          "", "O", 0),
            new TagData<string, string, string, string, int>("30",  "Requersted Execution Date",    "", "M", 0),
            new TagData<string, string, string, string, int>("25",  "Authorisation",                "", "O", 0)
        };
    }

    class MT101SequenceB
    {
        // Sequence B - Mandatory
        // Sequence B Transaction Details contains information about the transaction.
        public List<TagData<string, string, string, string, int>> seq = new List<TagData<string, string, string, string, int>>
        {
            // Tag, Name, Value, Mandatory
            new TagData<string, string, string, string, int>("21",  "Transaction Reference"                , "" ,"M", 0),
            new TagData<string, string, string, string, int>("21F", "F/X Deal Reference"                   , "" ,"O", 0),
            new TagData<string, string, string, string, int>("23E", "Instruction Code"                     , "", "O", 0),
            new TagData<string, string, string, string, int>("32B", "Currency Transaction Amount"          , "", "M", 0),
            new TagData<string, string, string, string, int>("50C", "Instructing Party"                    , "", "O", 0),
            new TagData<string, string, string, string, int>("50L", "Instructing Party"                    , "", "O", 0),
            new TagData<string, string, string, string, int>("50F", "Ordering Customer"                    , "", "O", 0),
            new TagData<string, string, string, string, int>("50G", "Ordering Customer"                    , "", "O", 0),
            new TagData<string, string, string, string, int>("50H", "Ordering Customer"                    , "", "O", 0),
            new TagData<string, string, string, string, int>("52A", "Account Serving Institution"          , "" ,"O", 0),
            new TagData<string, string, string, string, int>("52C", "Account Serving Institution"          , "" ,"O", 0),
            new TagData<string, string, string, string, int>("56A", "Intermediary"                         , "", "O", 0),
            new TagData<string, string, string, string, int>("56C", "Intermediary"                         , "", "O", 0),
            new TagData<string, string, string, string, int>("56D", "Intermediary"                         , "", "O", 0),
            new TagData<string, string, string, string, int>("57A", "Account with Institution"             , "", "O", 0),
            new TagData<string, string, string, string, int>("57C", "Account with Institution"             , "", "O", 0),
            new TagData<string, string, string, string, int>("57D", "Account with Institution"             , "", "O", 0),
            new TagData<string, string, string, string, int>("59",  "Beneficiary"                          , "", "M", 0),
            new TagData<string, string, string, string, int>("59A", "Beneficiary"                          , "", "M", 0),
            new TagData<string, string, string, string, int>("59F", "Beneficiary"                          , "", "M", 0),
            new TagData<string, string, string, string, int>("70",  "Remittance Information"               , "", "O", 0),
            new TagData<string, string, string, string, int>("77B", "Regulatory Reporting"                 , "", "O", 0),
            new TagData<string, string, string, string, int>("33B", "Currency/Original Ordered Amount"     , "", "O", 0),
            new TagData<string, string, string, string, int>("71A", "Details of Charges"                   , "", "M", 0),
            new TagData<string, string, string, string, int>("25A", "Charges Account"                      , "", "O", 0),
            new TagData<string, string, string, string, int>("36",  "Exchange Rate"                        , "", "O", 0)
        };
    }


    //https://www2.swift.com/knowledgecentre/publications/us1m_20200724/2.0?topic=mt101-format-spec.htm
    public class MT101 : MTMessage
    {
        DBUtils dbu = new DBUtils();

        #region SEQUENCE_VARIABLES
        MT101SequenceA sequenceA = new MT101SequenceA();
        List<MT101SequenceB> BLst = new List<MT101SequenceB>();
        #endregion

        #region MESSAGE SETUP
        /// <summary>
        /// Method Constructor
        /// </summary>
        MT101()
        {
            InitializeMT101();
        }

        /// <summary>
        /// Method Constructor
        /// </summary>
        /// <param name="msg"></param>
        public MT101(String msg)
        {
            InitializeMT101();

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
                if (index == 0) { return sequenceA.seq; }
                if (index >= 1 && index <= BLst.Count()) { return BLst[index - 1].seq; }
                else { return null; }
            }
        }

        private void InitializeMT101()
        {
            //BLst.Add(new SequenceB());

            numOfSequences = 2;
            ResetVariables();
            DefineScope();
            Anomalies.Clear();
        }

        /// <summary>
        /// Definition of Message Scope
        /// This is the SWIFT definition of the message
        /// It can be found at https://www2.swift.com/knowledgecentre/publications/us1m_20200724/2.0?topic=mt101-scope.htm
        /// </summary>
        protected override void DefineScope()
        {
            Scope = "MT 101 Scope:\r\n" +
                    "This message is:\r\n" +
                    "   Sent by a financial institution on behalf of a non-financial institution account owner, to an account servicing financial institution or to a forwarding financial institution for further transmission to the account servicing institution.\r\n" +
                    "   Sent by a non - financial institution account owner, or a party authorised by the account owner, to an account servicing financial institution or to a forwarding financial institution for further transmission to the account servicing institution.\r\n" +
                    "\r\n" +
                    "It is used to move funds from the ordering customer's account(s) serviced at the receiving financial institution or at the account servicing institution, or from an account(s) owned by the ordering customer which the instructing customer has explicit authority to debit, for example, a subsidiary account.\r\n" +
                    "The MT 101 can be used to order the movement of funds:\r\n" +
                    "   between ordering customer accounts, or\r\n" +
                    "   in favour of a third party, either domestically or internationally.\r\n";
        }

        /// <summary>
        /// Reset the class variables 
        /// </summary>
        protected override void ResetVariables()
        {
            foreach (TagData<string, string, string, string, int> t in sequenceA.seq)
            {
                t.Value = "";
                t.Present = 0;
            }

            BLst.Clear();
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

        protected override List<TagData<string, string, string, string, int>> getSequence(string seqId, int idx)
        {
            List<TagData<string, string, string, string, int>> sequence = null;

            switch (seqId)
            {
                case "A":
                    sequence = sequenceA.seq;
                    break;
                case "B":
                    sequence = BLst[idx].seq;
                    break;
                default:
                    break;
            }
            return sequence;
        }


        /// <summary>
        /// Fill in the class variables with the SWIFT message data
        /// </summary>
        /// <param name="tags"></param>
        protected override void FillDataTags(string[] tags)
        {
            List<TagData<string, string, string, string, int>> useSequence = null;
            int Bidx = -1;


            foreach (string key in tags)
            {
                string[] keyPair = key.Split('~');
                if (keyPair[0].Equals("20") == true)
                {
                    useSequence = sequenceA.seq;
                    SetTagValue(useSequence, keyPair[0], keyPair[1]);
                    SetTagPresent(useSequence, keyPair[0], 1);
                }
                if (keyPair[0].Equals("21") == true)
                {
                    Bidx++;
                    BLst.Add(new MT101SequenceB());
                    useSequence = BLst[Bidx].seq;

                    numOfSequences = Bidx + 2;
                    SetTagValue(useSequence, keyPair[0], keyPair[1]);
                    SetTagPresent(useSequence, keyPair[0], 1);
                }
                else
                {
                    SetTagValue(useSequence, keyPair[0], keyPair[1]);
                    SetTagPresent(useSequence, keyPair[0], 1);
                }
            }
        }

        protected override string whichSequence(List<TagData<string, string, string, string, int>> sequence)
        {
            string seq = null;

            foreach (TagData<string, string, string, string, int> t in sequence)
            {
                if (t.Tag.Equals("20") == true)
                {
                    seq = "A";
                    break;
                }
                else if (t.Tag.Equals("21") == true)
                {
                    seq = "B";
                    break;
                }
            }

            return seq;
        }

        /// <summary>
        /// Set method to set a MT320 tag value
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        protected override void SetTagValue(List<TagData<string, string, string, string, int>> sequence, string tag, string value)
        {
            int idx = -1;

            switch (whichSequence(sequence))
            {
                case "A":
                    foreach (TagData<string, string, string, string, int> t in sequence)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            t.Value = value;
                            break;
                        }
                    }
                    break;
                case "B":
                    foreach (TagData<string, string, string, string, int> t in sequence)
                    {
                        if (tag.Equals("23E") == true && t.Tag.Equals("23E") && !t.Value.Equals(""))
                        {
                            idx = sequence.FindLastIndex((delegate (TagData<string, string, string, string, int> t1) { return t1.Tag == "23E"; }));
                            sequence.Insert(idx + 1, new TagData<string, string, string, string, int>("23E", "Instruction Code", value, "O", 1));
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
                        case "21R":
                            if (Is_T21R_Valid(field) == false)
                                validTag = false;
                            break;
                        case "25":
                            if (Is_T25_Valid(field) == false)
                                validTag = false;
                            break;
                        case "28D":
                            if (Is_T28D_Valid(field) == false)
                                validTag = false;
                            break;
                        case "30":
                            if (Is_T30_Valid(field) == false)
                                validTag = false;
                            break;
                        case "50C":
                        case "50F":
                        case "50G":
                        case "50H":
                        case "50L":
                            if (Is_T50_Valid(field) == false)
                                validTag = false;
                            break;
                        case "51A":
                            if (Is_T51A_Valid(field) == false)
                                validTag = false;
                            break;
                        case "52A":
                        case "52C":
                            if (Is_T52_Valid(field) == false)
                                validTag = false;
                            break;
                        default:
                            Anomalies.Add("ERROR : Unknown tag " + field.Tag + " in sequence A - can not validate.");
                            break;
                    }
                    break;
                case "B":
                    switch (field.Tag)
                    {
                        case "21":
                            if (Is_T21_Valid(field) == false)
                                validTag = false;
                            break;
                        case "21F":
                            if (Is_T21F_Valid(field) == false)
                                validTag = false;
                            break;
                        case "23E":
                            if (Is_T23E_Valid(field) == false)
                                validTag = false;
                            break;
                        case "25A":
                            if (Is_T25_Valid(field) == false)
                                validTag = false;
                            break;
                        case "32B":
                            if (Is_T32B_Valid(field) == false)
                                validTag = false;
                            break;
                        case "33B":
                            if (Is_T33B_Valid(field) == false)
                                validTag = false;
                            break;
                        case "36":
                            if (Is_T36_Valid(field) == false)
                                validTag = false;
                            break;
                        case "50C":
                        case "50F":
                        case "50G":
                        case "50H":
                        case "50L":
                            if (Is_T50_Valid(field) == false)
                                validTag = false;
                            break;
                        case "52A":
                        case "52C":
                            if (Is_T52_Valid(field) == false)
                                validTag = false;
                            break;
                        case "56A":
                        case "56C":
                        case "56D":
                            if (Is_T56_Valid(field) == false)
                                validTag = false;
                            break;
                        case "57A":
                        case "57C":
                        case "57D":
                            if (Is_T57_Valid(field) == false)
                                validTag = false;
                            break;
                        case "59":
                        case "59A":
                        case "59F":
                            if (Is_T59_Valid(field) == false)
                                validTag = false;
                            break;
                        case "70":
                            if (Is_T70_Valid(field) == false)
                                validTag = false;
                            break;
                        case "71A":
                            if (Is_T71A_Valid(field) == false)
                                validTag = false;
                            break;
                        case "77B":
                            if (Is_T77_Valid(field) == false)
                                validTag = false;
                            break;
                        default:
                            Anomalies.Add("ERROR : Unknown tag " + field.Tag + " in sequence B - can not validate.");
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
            List<string> seqs = new List<string>() { "A", "B" };
            List<TagData<string, string, string, string, int>> seq;
            bool validateField = true;
            int T59present = 0;
            int end = 1;
            int i = 0;

            foreach (string sid in seqs)
            {
                if (sid.Equals("B") == true)
                    end = BLst.Count();

                for (i = 0; i < end; i++)
                {
                    seq = getSequence(sid, i);
                    foreach (TagData<string, string, string, string, int> t in seq)
                    {
                        validateField = true;
                        /* check for vairants */
                        if (sid.Equals("B") == true)
                        {
                            if ((t.Tag.Equals("59") && t.Present == 0) || (t.Tag.Equals("59A") && t.Present == 0) || (t.Tag.Equals("59F") && t.Present == 0))
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
 
                if ((sid.Equals("B") == true) && (T59present == 3))
                    Anomalies.Add("ERROR: Mandatory Tag 59 is not present in any variantion");
            }

            return allTagsValid;
        }
        #endregion

        #region TAG PARSING
        /// <summary>
        /// getT20_SendersReference
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
        /// getT21_TransactionReference
        /// 
        /// Returns the unique reference for the individual transaction contained in a particular occurrence of sequence B.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT21_TransactionReference(List<TagData<string, string, string, string, int>> seq)
        {
            return getT21(seq);
        }

        /// <summary>
        /// getT21F_FXDealReference
        /// 
        /// Returns the foreign exchange contract reference between the ordering customer and the account servicing financial institution.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT21F_FXDealReference(List<TagData<string, string, string, string, int>> seq)
        {
            return getT21F(seq);
        }

        /// <summary>
        /// getT21R_CustomerSpecifiedReference
        /// 
        /// Returns the reference to the entire message assigned by either the, instructing party, when present or ordering customer, when the instructing party is not present.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT21R_CustomerSpecifiedReference(List<TagData<string, string, string, string, int>> seq)
        {
            return getT21R(seq);
        }

        /// <summary>
        /// getT23E_InstructionCode
        /// 
        /// Returns the specified instruction code
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT23E_InstructionCode(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> values = getT23E(seq);

            return values[0];
        }

        /// <summary>
        /// getT23E_Information
        /// 
        /// Returns the additional information associated with the specified instruction code
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT23E_Information(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> values = getT23E(seq);

            return values[1];
        }

        /// <summary>
        /// getT25_Authorisation
        /// 
        /// Returns the additional security provisions, for example, a digital signature, 
        /// between the ordering customer/instructing party and the account servicing financial institution.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT25_Authorisation(List<TagData<string, string, string, string, int>> seq)
        {
            return getT25(seq);
        }

        /// <summary>
        /// getT25A_ChargesAccount
        /// 
        /// Returns the ordering customer's account number to which applicable transaction charges should be separately applied.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT25A_ChargesAccount(List<TagData<string, string, string, string, int>> seq)
        {
            return getT25A(seq);
        }

        /// <summary>
        /// getT28D_MessageIndex
        /// 
        /// Returns the sequence number of the chained messages
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public int getT28D_MessageIndex(List<TagData<string, string, string, string, int>> seq)
        {
            List<int> values = getT28D(seq);

            return (values[0]);
        }

        /// <summary>
        /// getT28D_TotalMessage
        /// 
        /// Returns the total number of chained messages
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public int getT28D_TotalMessage(List<TagData<string, string, string, string, int>> seq)
        {
            List<int> values = getT28D(seq);

            return (values[1]);
        }

        /// <summary>
        /// getT30_RequestedExecutionDate
        /// 
        /// Returns the date on which the cheque was drawn.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT30_RequestedExecutionDate(List<TagData<string, string, string, string, int>> seq)
        {
            return getT30(seq);
        }

        /// <summary>
        /// getT32B_Currency
        /// 
        /// Returns the currency of the subsequent transfer to be executed by the Receiver.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT32B_Currency(List<TagData<string, string, string, string, int>> seq)
        {
            List<string > data = getT32B(seq);

            return data[0];
        }

        /// <summary>
        /// getT32B_TransactionAmount
        /// 
        /// Returns the amount of the subsequent transfer to be executed by the Receiver.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public Nullable<double> getT32B_TransactionAmount(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT32B(seq);
            Nullable<double> amount = null;

            if (data[1] != null)
                amount = Convert.ToDouble(data[1]);

            return amount;
        }

        /// <summary>
        /// getT33B_Currency
        /// 
        /// Returns the original currency as specified by the ordering customer.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT33B_Currency(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT33B(seq);

            return data[0];
        }

        /// <summary>
        /// getT33B_OrderedAmount
        /// 
        /// Returns the amount as specified by the ordering customer.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public Nullable<double> getT33B_OrderedAmount(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT33B(seq);
            Nullable<double> amount = null;

            if (data[1] != null)
                amount = Convert.ToDouble(data[1]);

            return amount;
        }

        /// <summary>
        /// getT36_ExchangeRate
        /// 
        /// Returns the exchange rate applied by the ordering customer/instructing party 
        /// when converting the original ordered amount to the transaction amount.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public Nullable<double> getT36_ExchangeRate(List<TagData<string, string, string, string, int>> seq)
        {
            return getT36(seq);
        }

        /// <summary>
        /// getT50C_InstructingPartyIdentifierCode
        /// 
        /// Returns the BIC code of the customer which is authorised by the 
        /// account owner/account servicing institution to order all the transactions in the message.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT50C_InstructingPartyIdentifierCode(List<TagData<string, string, string, string, int>> seq)
        {
            return getT50C(seq);
        }

        /// <summary>
        /// getT50F_OrderingCustomerPartyIdentifier
        /// 
        /// Returns the account owner whose account is to be debited with all transactions in sequence B.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT50F_OrderingCustomerPartyIdentifier(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data =  getT50F(seq);

            return data[0];
        }

        /// <summary>
        /// getT50F_OrderingCustomerNameAddress
        /// 
        /// Returns the account owner whose account is to be debited with all transactions in sequence B.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT50F_OrderingCustomerNameAddress(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT50F(seq);

            return data[1];
        }

        /// <summary>
        /// getT50G_OrderingCustomerAccount
        /// 
        /// Returns the account owner whose account is to be debited with all transactions in sequence B.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT50G_OrderingCustomerAccount(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT50G(seq);

            return data[0];
        }

        /// <summary>
        /// getT50G_OrderingCustomerIdentifierCode
        /// 
        /// Returns the account owner whose account is to be debited with all transactions in sequence B.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT50G_OrderingCustomerIdentifierCode(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT50G(seq);

            return data[1];
        }

        /// <summary>
        /// getT50H_OrderingCustomerAccount
        /// 
        /// Returns the account owner whose account is to be debited with all transactions in sequence B.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT50H_OrderingCustomerAccount(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT50H(seq);

            return data[0];
        }

        /// <summary>
        /// getT50H_OrderingCustomerNameAddress
        /// 
        /// Returns the account owner whose account is to be debited with all transactions in sequence B.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT50H_OrderingCustomerNameAddress(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT50H(seq);

            return data[1];
        }

        /// <summary>
        /// getT50L_InstructingPartyPartyIdentifier
        /// 
        /// Returns the party identifier of the customer which is 
        /// authorised by the account owner/account servicing institution 
        /// to order all the transactions in the message.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT50L_InstructingPartyPartyIdentifier(List<TagData<string, string, string, string, int>> seq)
        {
            return getT50L(seq);
        }

        /// <summary>
        /// getT51A_SendingInstitutionPartyIdentifier
        /// 
        /// Returns the identity of the Sender of the message.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT51A_SendingInstitutionPartyIdentifier(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT51A(seq);

            return data[0];
        }

        /// <summary>
        /// getT51A_SendingInstitutionIdentifierCode
        /// 
        /// Returns the account servicing institution - when other than the Receiver - 
        /// which services the account of the account owner to be debited.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT51A_SendingInstitutionIdentifierCode(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT51A(seq);

            return data[1];
        }

        /// <summary>
        /// getT52A_AcctServiceInstPartyIdentifier
        /// 
        /// Returns the account servicing institution - when other than the Receiver - 
        /// which services the account of the account owner to be debited.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT52A_AcctServiceInstPartyIdentifier(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT52A(seq);

            return data[0];
        }

        /// <summary>
        /// getT52A_AcctServiceInstIdentifierCode
        /// 
        /// Returns the account servicing institution - when other than the Receiver - 
        /// which services the account of the account owner to be debited.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT52A_AcctServiceInstIdentifierCode(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT52A(seq);

            return data[1];
        }

        /// <summary>
        /// getT52A_AcctServiceInstPartyIdentifier
        /// 
        /// Returns the account servicing institution - when other than the Receiver - 
        /// which services the account of the account owner to be debited.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT52C_AcctServiceInstPartyIdentifier(List<TagData<string, string, string, string, int>> seq)
        {
            return getT52C(seq);
        }

        /// <summary>
        /// getT56A_IntermediaryPartyIdentifier
        /// 
        /// Returns the financial institution through which the transaction must pass to reach the account with institution.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT56A_IntermediaryPartyIdentifier(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT56A(seq);

            return data[0];
        }

        /// <summary>
        /// getT56A_IntermediaryIdentifierCode
        /// 
        /// Returns the financial institution through which the transaction must pass to reach the account with institution.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT56A_IntermediaryIdentifierCode(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT56A(seq);

            return data[1];
        }

        /// <summary>
        /// getT56C_IntermediaryPartyIdentifier
        /// 
        /// Returns the financial institution through which the transaction must pass to reach the account with institution.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT56C_IntermediaryPartyIdentifier(List<TagData<string, string, string, string, int>> seq)
        {
            return getT56C(seq);
        }

        /// <summary>
        /// getT56D_PartyIdentifier
        /// 
        /// Returns the financial institution through which the transaction must pass to reach the account with institution.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT56D_PartyIdentifier(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT56D(seq);

            return data[0];
        }

        /// <summary>
        /// getT56D_IntermediaryNameAddress
        /// 
        /// Returns the financial institution through which the transaction must pass to reach the account with institution.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT56D_IntermediaryNameAddress(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT56D(seq);

            return data[1];
        }

        /// <summary>
        /// getT57A_AccountWithPartyIdentifier
        /// 
        /// Returns the financial institution which services the account for the beneficiary customer.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT57A_AccountWithPartyIdentifier(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT57A(seq);

            return data[0];
        }

        /// <summary>
        /// getT57A_AccountWithIdentifierCode
        /// 
        /// Returns the financial institution which services the account for the beneficiary customer.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT57A_AccountWithIdentifierCode(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT57A(seq);

            return data[1];
        }

        /// <summary>
        /// getT57C_AccountWithPartyIdentifier
        /// 
        /// Returns the financial institution which services the account for the beneficiary customer.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT57C_AccountWithPartyIdentifier(List<TagData<string, string, string, string, int>> seq)
        {
            return getT56C(seq);
        }

        /// <summary>
        /// getT57D_PartyIdentifier
        /// 
        /// Returns the financial institution which services the account for the beneficiary customer.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT57D_PartyIdentifier(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT56D(seq);

            return data[0];
        }

        /// <summary>
        /// getT57D_AccountWithNameAddress
        /// 
        /// Returns the financial institution which services the account for the beneficiary customer.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT57D_AccountWithNameAddress(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT56D(seq);

            return data[1];
        }

        /// <summary>
        /// getT59_BeneficiaryAccount
        /// 
        /// Returns the beneficiary of the subsequent operation from the particular occurrence of sequence B.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT59_BeneficiaryAccount(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT59(seq);

            return data[0];
        }

        /// <summary>
        /// getT59_BeneficiaryNameAddress
        /// 
        /// Returns the beneficiary of the subsequent operation from the particular occurrence of sequence B.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT59_BeneficiaryNameAddress(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT59(seq);

            return data[1];
        }

        /// <summary>
        /// getT59A_BeneficiaryAccount
        /// 
        /// Returns the beneficiary of the subsequent operation from the particular occurrence of sequence B.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT59A_BeneficiaryAccount(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT59A(seq);

            return data[0];
        }

        /// <summary>
        /// getT59A_BeneficiaryIdentifierCode
        /// 
        /// Returns the beneficiary of the subsequent operation from the particular occurrence of sequence B.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT59A_BeneficiaryIdentifierCode(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT59A(seq);

            return data[1];
        }

        /// <summary>
        /// getT59F_BeneficiaryAccount
        /// 
        /// Returns the beneficiary of the subsequent operation from the particular occurrence of sequence B.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT59F_BeneficiaryAccount(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT59F(seq);

            return data[0];
        }

        /// <summary>
        /// getT59F_BeneficiaryNameAddress
        /// 
        /// Returns the beneficiary of the subsequent operation from the particular occurrence of sequence B.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT59F_BeneficiaryNameAddress(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT59F(seq);

            return data[1];
        }

        /// <summary>
        /// getT70_RemittanceInformation
        /// 
        /// Returns the details of the individual transactions which are to be transmitted to the beneficiary customer.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT70_RemittanceInformation(List<TagData<string, string, string, string, int>> seq)
        {
            return getT70(seq);
        }

        /// <summary>
        /// getT71A_DetailsOfCharges
        /// 
        /// Returns which party will bear the applicable charges for the subsequent transfer of funds.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT71A_DetailsOfCharges(List<TagData<string, string, string, string, int>> seq)
        {
            return getT71A(seq);
        }

        /// <summary>
        /// getT77B_RegulatoryReporting
        /// 
        /// Returns the codes for the statutory and/or regulatory information required 
        /// by the authorities in the country of the Receiver or the Sender/originating customer.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT77B_RegulatoryReporting(List<TagData<string, string, string, string, int>> seq)
        {
            return getT77B(seq);
        }

        #endregion

        public void testFunctions()
        {
            List<string> ls = new List<string>();
            List<int> li = new List<int>();
            string ss = null;

            ls = getT23E(BLst[0].seq);
            ss = getT25A(BLst[0].seq);
            li = getT28D(BLst[0].seq);
        }
    }
}
