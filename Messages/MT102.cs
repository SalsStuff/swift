using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;

namespace Messages
{
    class MT102SequenceA
    {
        // Sequence A - Mandatory
        // Sequence A General Information contains general information about the fixed loan/deposit as well as about the confirmation itself.
        public List<TagData<string, string, string, string, int>> seq = new List<TagData<string, string, string, string, int>>
        {
            // Tag, Name, Value, Mandatory
            new TagData<string, string, string, string, int>("20",  "File Reference",               "" ,"M", 0),
            new TagData<string, string, string, string, int>("23",  "Bank Operation Code",          "" ,"M", 0),
            new TagData<string, string, string, string, int>("51A", "Sending Institution",          "", "O", 0),
            new TagData<string, string, string, string, int>("50A", "Ordering Customer",            "", "O", 0),
            new TagData<string, string, string, string, int>("50F", "Ordering Customer",            "", "O", 0),
            new TagData<string, string, string, string, int>("50K", "Ordering Customer",            "", "O", 0),
            new TagData<string, string, string, string, int>("52A", "Ordering Institution",         "" ,"O", 0),
            new TagData<string, string, string, string, int>("52B", "Ordering Institution",         "" ,"O", 0),
            new TagData<string, string, string, string, int>("52C", "Ordering Institution",         "" ,"O", 0),
            new TagData<string, string, string, string, int>("26T",  "Transaction Type Code",       "", "O", 0),
            new TagData<string, string, string, string, int>("77B", "Regulatory Reporting",         "", "O", 0),
            new TagData<string, string, string, string, int>("71A", "Details of Charges",           "", "O", 0),
            new TagData<string, string, string, string, int>("36",  "Exchange Rate",                "", "O", 0)
        };
    }

    class MT102SequenceB
    {
        // Sequence B - Mandatory
        // Sequence B Transaction Details contains information about the transaction.
        public List<TagData<string, string, string, string, int>> seq = new List<TagData<string, string, string, string, int>>
        {
            // Tag, Name, Value, Mandatory
            new TagData<string, string, string, string, int>("21",  "Transaction Reference"                , "" ,"M", 0),
            new TagData<string, string, string, string, int>("32B", "Currency Transaction Amount"          , "", "M", 0),
            new TagData<string, string, string, string, int>("50A", "Ordering Customer"                    , "", "O", 0),
            new TagData<string, string, string, string, int>("50F", "Ordering Customer"                    , "", "O", 0),
            new TagData<string, string, string, string, int>("50K", "Ordering Customer"                    , "", "O", 0),
            new TagData<string, string, string, string, int>("52A", "Ordering Institution"                 , "" ,"O", 0),
            new TagData<string, string, string, string, int>("52B", "Ordering Institution"                 , "" ,"O", 0),
            new TagData<string, string, string, string, int>("52C", "Ordering Institution"                 , "" ,"O", 0),
            new TagData<string, string, string, string, int>("57A", "Account with Institution"             , "", "O", 0),
            new TagData<string, string, string, string, int>("57C", "Account with Institution"             , "", "O", 0),
            new TagData<string, string, string, string, int>("59",  "Beneficiary Customer"                 , "", "M", 0),
            new TagData<string, string, string, string, int>("59A", "Beneficiary Customer"                 , "", "M", 0),
            new TagData<string, string, string, string, int>("59F", "Beneficiary Customer"                 , "", "M", 0),
            new TagData<string, string, string, string, int>("70",  "Remittance Information"               , "", "O", 0),
            new TagData<string, string, string, string, int>("26T", "Transaction Type Code"                , "" ,"O", 0),
            new TagData<string, string, string, string, int>("77B", "Regulatory Reporting"                 , "", "O", 0),
            new TagData<string, string, string, string, int>("33B", "Currency/Instructed Amount"           , "", "O", 0),
            new TagData<string, string, string, string, int>("71A", "Details of Charges"                   , "", "O", 0),
            new TagData<string, string, string, string, int>("71F", "Sender's Charges"                     , "", "O", 0),
            new TagData<string, string, string, string, int>("71G", "Receiver's Charges"                   , "", "O", 0),
            new TagData<string, string, string, string, int>("36",  "Exchange Rate"                        , "", "O", 0)
        };
    }

    class MT102SequenceC
    {
        // Sequence C - Mandatory
        // Sequence C Transaction Details contains information about the transaction.
        public List<TagData<string, string, string, string, int>> seq = new List<TagData<string, string, string, string, int>>
        {
            // Tag, Name, Value, Mandatory
            new TagData<string, string, string, string, int>("32A", "Value Date, Currency Code, Amount"    , "", "M", 0),
            new TagData<string, string, string, string, int>("19",  "Sum of Amounts"                       , "", "O", 0),
            new TagData<string, string, string, string, int>("71G", "Sum of Receiver's Charges"            , "", "O", 0),
            new TagData<string, string, string, string, int>("13C", "Time Indication"                      , "", "O", 0),
            new TagData<string, string, string, string, int>("53A", "Sender's Correspondent"               , "", "O", 0),
            new TagData<string, string, string, string, int>("53C", "Sender's Correspondent"               , "", "O", 0),
            new TagData<string, string, string, string, int>("54A", "Receiver's Correspondent"             , "", "O", 0),
            new TagData<string, string, string, string, int>("72",  "Sender to Receiver Information"       , "", "O", 0)
        };
    }

    //https://www2.swift.com/knowledgecentre/publications/us1m_20200724/2.0?topic=mt102-format-spec.htm
    class MT102 : MTMessage
    {
        DBUtils dbu = new DBUtils();

        #region SEQUENCE_VARIABLES
        MT102SequenceA sequenceA = new MT102SequenceA();
        //SequenceB sequenceB = new SequenceB();
        List<MT102SequenceB> BLst = new List<MT102SequenceB>();
        MT102SequenceC sequenceC = new MT102SequenceC();
        #endregion

        #region MESSAGE SETUP
        /// <summary>
        /// Method Constructor
        /// </summary>
        MT102()
        {
            InitializeMT102();
        }

        /// <summary>
        /// Method Constructor
        /// </summary>
        /// <param name="msg"></param>
        public MT102(String msg)
        {
            InitializeMT102();

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
                if (index == BLst.Count()+1) { return sequenceC.seq;  }
                else { return null; }
            }
        }

        private void InitializeMT102()
        {
            numOfSequences = 3;
            ResetVariables();
            DefineScope();
            Anomalies.Clear();
        }

        /// <summary>
        /// Definition of Message Scope
        /// This is the SWIFT definition of the message
        /// It can be found at https://www2.swift.com/knowledgecentre/publications/us1m_20200724/2.0?topic=mt102-scope.htm
        /// </summary>
        protected override void DefineScope()
        {
            Scope = "MT 102 Scope:\r\n" +
                    "This message is sent by or on behalf of the financial institution of the ordering customer(s) to another financial institution for payment to the beneficiary customer." +
                    "It requests the Receiver to credit the beneficiary customer(s) directly or indirectly through a clearing mechanism or another financial institution, or to issue a cheque to the beneficiary." +
                    "This message is used to convey multiple payment instructions between financial institutions for clean payments. Its use is subject to bilateral / multilateral agreements between Sender and Receiver." +
                    "Amongst other things, these bilateral agreements cover the transaction amount limits, the currencies accepted and their settlement.The multiple payments checklist included below is recommended as a guide for institutions in the setup of their agreements.";
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

            foreach (TagData<string, string, string, string, int> t in sequenceC.seq)
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
                case "C":
                    sequence = sequenceC.seq;
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
                else if (keyPair[0].Equals("21") == true)
                {
                    Bidx++;
                    BLst.Add(new MT102SequenceB());
                    useSequence = BLst[Bidx].seq;

                    numOfSequences = Bidx + 2;
                    SetTagValue(useSequence, keyPair[0], keyPair[1]);
                    SetTagPresent(useSequence, keyPair[0], 1);
                }
                else if (keyPair[0].Equals("32A") == true)
                {
                    useSequence = sequenceC.seq;
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
                else if (t.Tag.Equals("32A") == true)
                {
                    seq = "C";
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
                        if (tag.Equals("71F") == true && t.Tag.Equals("71F") && !t.Value.Equals(""))
                        {
                            idx = sequence.FindLastIndex((delegate (TagData<string, string, string, string, int> t1) { return t1.Tag == "71F"; }));
                            sequence.Insert(idx + 1, new TagData<string, string, string, string, int>("71F", "Sender's Charges", value, "O", 1));
                            break;
                        }
                        else if (tag.Equals(t.Tag) == true)
                        {
                            t.Value = value;
                            break;
                        }
                    }
                    break;
                case "C":
                    foreach (TagData<string, string, string, string, int> t in sequence)
                    {
                        if (tag.Equals("13C") == true && t.Tag.Equals("13C") && !t.Value.Equals(""))
                        {
                            idx = sequence.FindLastIndex((delegate (TagData<string, string, string, string, int> t1) { return t1.Tag == "13C"; }));
                            sequence.Insert(idx + 1, new TagData<string, string, string, string, int>("13C", "Time Indication", value, "O", 1));
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
                        case "23":
                            if (Is_T23_Valid(field) == false)
                                validTag = false;
                            break;
                        case "26T":
                            if (Is_T26T_Valid(field) == false)
                                validTag = false;
                            break;
                        case "36":
                            if (Is_T36_Valid(field) == false)
                                validTag = false;
                            break;
                        case "50A":
                        case "50F":
                        case "50K":
                            if (Is_T50_Valid(field) == false)
                                validTag = false;
                            break;
                        case "51A":
                            if (Is_T51A_Valid(field) == false)
                                validTag = false;
                            break;
                        case "52A":
                        case "52B":
                        case "52C":
                            if (Is_T52_Valid(field) == false)
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
                        case "26T":
                            if (Is_T26T_Valid(field) == false)
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
                        case "50A":
                        case "50F":
                        case "50K":
                            if (Is_T50_Valid(field) == false)
                                validTag = false;
                            break;
                        case "52A":
                        case "52B":
                        case "52C":
                            if (Is_T52_Valid(field) == false)
                                validTag = false;
                            break;
                        case "57A":
                        case "57C":
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
                        case "71F":
                            if (Is_T71F_Valid(field) == false)
                                validTag = false;
                            break;
                        case "71G":
                            if (Is_T71G_Valid(field) == false)
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
                case "C":
                    switch (field.Tag)
                    {
                        case "19":
                            if (Is_T19_Valid(field) == false)
                                validTag = false;
                            break;
                        case "13C":
                            if (Is_T13C_Valid(field) == false)
                                validTag = false;
                            break;
                        case "32A":
                            if (Is_T32A_Valid(field) == false)
                                validTag = false;
                            break;
                        case "53A":
                        case "53C":
                            if (Is_T53_Valid(field) == false)
                                validTag = false;
                            break;
                        case "54A":
                            if (Is_T54_Valid(field) == false)
                                validTag = false;
                            break;
                        case "71G":
                            if (Is_T71G_Valid(field) == false)
                                validTag = false;
                            break;
                        case "72":
                            if (Is_T72_Valid(field) == false)
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
            List<string> seqs = new List<string>() { "A", "B", "C" };
            List<TagData<string, string, string, string, int>> seq;
            bool validateField = true;
            int T59present = 0;
            int end = 1;
            int i = 0;

            foreach (string sid in seqs)
            {
                if (sid.Equals("B") == true)
                    end = BLst.Count();
                else if (sid.Equals("C") == true)
                    end = 1;

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
    }
}
