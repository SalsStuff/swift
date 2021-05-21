using System;
using System.Collections.Generic;
using System.Linq;
using Database;

namespace Messages
{
    // https://www2.swift.com/knowledgecentre/publications/us3ma_20190719/1.0?topic=mt320.htm
    public class MT320 : MTMessage
    {
        DBUtils dbu = new DBUtils();
        
        #region SEQUENCE_VARIABLES
        // Sequence A - Mandatory
        // Sequence A General Information contains general information about the fixed loan/deposit as well as about the confirmation itself.
        List<TagData<string, string, string, string, int>> sequenceA = new List<TagData<string, string, string, string, int>>
        {
            // Tag, Name, Value, Mandatory
            new TagData<string, string, string, string, int>("15A", "New Sequence",              "" ,"M", 0),
            new TagData<string, string, string, string, int>("20",  "Sender's Reference",        "" ,"M", 0),
            new TagData<string, string, string, string, int>("21",  "Related Reference",         "" ,"O", 0),
            new TagData<string, string, string, string, int>("22A", "Type of Operation",         "", "M", 0),
            new TagData<string, string, string, string, int>("94A", "Scope of Operation",        "", "O", 0),
            new TagData<string, string, string, string, int>("22B", "Type of Event",             "", "M", 0),
            new TagData<string, string, string, string, int>("22C", "Common Reference",          "", "M", 0),
            new TagData<string, string, string, string, int>("21N", "Contract Number Party A",   "" ,"O", 0),
            new TagData<string, string, string, string, int>("82A", "Party A",                   "", "M", 0),
            new TagData<string, string, string, string, int>("82D", "Party A",                   "", "M", 0),
            new TagData<string, string, string, string, int>("82J", "Party A",                   "", "M", 0),
            new TagData<string, string, string, string, int>("87A", "Party B",                   "", "M", 0),
            new TagData<string, string, string, string, int>("87D", "Party B",                   "", "M", 0),
            new TagData<string, string, string, string, int>("87J", "Party B",                   "", "M", 0),
            new TagData<string, string, string, string, int>("83A", "Fund or Instructing Party", "", "O", 0),
            new TagData<string, string, string, string, int>("83D", "Fund or Instructing Party", "", "O", 0),
            new TagData<string, string, string, string, int>("83J", "Fund or Instructing Party", "", "O", 0),
            new TagData<string, string, string, string, int>("77D", "Terms and Conditions",      "", "O", 0)
        };

        // Sequence B - Mandatory
        // Sequence B Transaction Details contains information about the transaction.
        List<TagData<string, string, string, string, int>> sequenceB = new List<TagData<string, string, string, string, int>>
        {
            // Tag, Name, Value, Mandatory
            new TagData<string, string, string, string, int>("15B", "New Sequence"                         , "" ,"M", 0),
            new TagData<string, string, string, string, int>("17R", "Party A's Role"                       , "" ,"M", 0),
            new TagData<string, string, string, string, int>("30T", "Trade Date"                           , "", "M", 0),
            new TagData<string, string, string, string, int>("30V", "Value Date"                           , "", "M", 0),
            new TagData<string, string, string, string, int>("30P", "Maturity Date"                        , "", "M", 0),
            new TagData<string, string, string, string, int>("32B", "Currency and Principal Amount"        , "", "M", 0),
            new TagData<string, string, string, string, int>("32H", "Amount to be Settled"                 , "", "O", 0),
            new TagData<string, string, string, string, int>("30X", "Next Interest Due Date"               , "", "O", 0),
            new TagData<string, string, string, string, int>("34E", "Currenct and Interest Amount"         , "", "M", 0),
            new TagData<string, string, string, string, int>("37G", "Interest Rate"                        , "", "M", 0),
            new TagData<string, string, string, string, int>("14D", "Day Count Fraction"                   , "" ,"M", 0),
            new TagData<string, string, string, string, int>("30F", "Last Day of the First Interest Period", "" ,"O", 0),
            new TagData<string, string, string, string, int>("38J", "Number of Days"                       , "", "O", 0),
            new TagData<string, string, string, string, int>("39M", "Payment Clearing Center"              , "", "O", 0)
        };

        // Sequence C - Mandatory
        // Sequence C Settlement Instructions for Amounts Payable by Party A provides the instructions for the amounts payable by party A.
        List<TagData<string, string, string, string, int>> sequenceC = new List<TagData<string, string, string, string, int>>
        {
            // Tag, Name, Value, Mandatory
            new TagData<string, string, string, string, int>("15C", "New Sequence"           , "" ,"M", 0),
            new TagData<string, string, string, string, int>("53A", "Delivery Agent"         , "" ,"O", 0),
            new TagData<string, string, string, string, int>("53D", "Delivery Agent"         , "" ,"O", 0),
            new TagData<string, string, string, string, int>("53J", "Delivery Agent"         , "" ,"O", 0),
            new TagData<string, string, string, string, int>("86A", "Intermediary 2"         , "", "O", 0),
            new TagData<string, string, string, string, int>("86D", "Intermediary 2"         , "", "O", 0),
            new TagData<string, string, string, string, int>("86J", "Intermediary 2"         , "", "O", 0),
            new TagData<string, string, string, string, int>("56A", "Intermediary"           , "", "O", 0),
            new TagData<string, string, string, string, int>("56D", "Intermediary"           , "", "O", 0),
            new TagData<string, string, string, string, int>("56J", "Intermediary"           , "", "O", 0),
            new TagData<string, string, string, string, int>("57A", "Receiving Agent"        , "", "M", 0),
            new TagData<string, string, string, string, int>("57D", "Receiving Agent"        , "", "M", 0),
            new TagData<string, string, string, string, int>("57J", "Receiving Agent"        , "", "M", 0),
            new TagData<string, string, string, string, int>("58A", "Beneficiary Institution", "", "O", 0),
            new TagData<string, string, string, string, int>("58D", "Beneficiary Institution", "", "O", 0),
            new TagData<string, string, string, string, int>("58J", "Beneficiary Institution", "", "O", 0)
        };

        // Sequence D - Mandatory
        // Sequence D Settlement Instructions for Amounts Payable by Party B provides the instructions for the amounts payable by party B.
        List<TagData<string, string, string, string, int>> sequenceD = new List<TagData<string, string, string, string, int>>
        {
            // Tag, Name, Value, Mandatory
            new TagData<string, string, string, string, int>("15D", "New Sequence"           , "" ,"M", 0),
            new TagData<string, string, string, string, int>("53A", "Delivery Agent"         , "" ,"O", 0),
            new TagData<string, string, string, string, int>("53D", "Delivery Agent"         , "" ,"O", 0),
            new TagData<string, string, string, string, int>("53J", "Delivery Agent"         , "" ,"O", 0),
            new TagData<string, string, string, string, int>("86A", "Intermediary 2"         , "", "O", 0),
            new TagData<string, string, string, string, int>("86D", "Intermediary 2"         , "", "O", 0),
            new TagData<string, string, string, string, int>("86J", "Intermediary 2"         , "", "O", 0),
            new TagData<string, string, string, string, int>("56A", "Intermediary"           , "", "O", 0),
            new TagData<string, string, string, string, int>("56D", "Intermediary"           , "", "O", 0),
            new TagData<string, string, string, string, int>("56J", "Intermediary"           , "", "O", 0),
            new TagData<string, string, string, string, int>("57A", "Receiving Agent"        , "", "M", 0),
            new TagData<string, string, string, string, int>("57D", "Receiving Agent"        , "", "M", 0),
            new TagData<string, string, string, string, int>("57J", "Receiving Agent"        , "", "M", 0),
            new TagData<string, string, string, string, int>("58A", "Beneficiary Institution", "", "O", 0),
            new TagData<string, string, string, string, int>("58D", "Beneficiary Institution", "", "O", 0),
            new TagData<string, string, string, string, int>("58J", "Beneficiary Institution", "", "O", 0)
        };

        // Sequence E - Optional
        // Sequence E Settlement Instructions for Interests Payable by Party A provides the instructions for the interest payable by party A.
        List<TagData<string, string, string, string, int>> sequenceE = new List<TagData<string, string, string, string, int>>
        {
            // Tag, Name, Value, Mandatory
            new TagData<string, string, string, string, int>("15E", "New Sequence"           , "" ,"M", 0),
            new TagData<string, string, string, string, int>("53A", "Delivery Agent"         , "" ,"O", 0),
            new TagData<string, string, string, string, int>("53D", "Delivery Agent"         , "" ,"O", 0),
            new TagData<string, string, string, string, int>("53J", "Delivery Agent"         , "" ,"O", 0),
            new TagData<string, string, string, string, int>("86A", "Intermediary 2"         , "", "O", 0),
            new TagData<string, string, string, string, int>("86D", "Intermediary 2"         , "", "O", 0),
            new TagData<string, string, string, string, int>("86J", "Intermediary 2"         , "", "O", 0),
            new TagData<string, string, string, string, int>("56A", "Intermediary"           , "", "O", 0),
            new TagData<string, string, string, string, int>("56D", "Intermediary"           , "", "O", 0),
            new TagData<string, string, string, string, int>("56J", "Intermediary"           , "", "O", 0),
            new TagData<string, string, string, string, int>("57A", "Receiving Agent"        , "", "M", 0),
            new TagData<string, string, string, string, int>("57D", "Receiving Agent"        , "", "M", 0),
            new TagData<string, string, string, string, int>("57J", "Receiving Agent"        , "", "M", 0),
            new TagData<string, string, string, string, int>("58A", "Beneficiary Institution", "", "O", 0),
            new TagData<string, string, string, string, int>("58D", "Beneficiary Institution", "", "O", 0),
            new TagData<string, string, string, string, int>("58J", "Beneficiary Institution", "", "O", 0)
        };

        // Sequence F - Optional
        // Sequence F Settlement Instructions for Interests Payable by Party B provides the instructions for the interest payable by party B.
        List<TagData<string, string, string, string, int>> sequenceF = new List<TagData<string, string, string, string, int>>
        {
            // Tag, Name, Value, Mandatory
            new TagData<string, string, string, string, int>("15F", "New Sequence"           , "" ,"M", 0),
            new TagData<string, string, string, string, int>("53A", "Delivery Agent"         , "" ,"O", 0),
            new TagData<string, string, string, string, int>("53D", "Delivery Agent"         , "" ,"O", 0),
            new TagData<string, string, string, string, int>("53J", "Delivery Agent"         , "" ,"O", 0),
            new TagData<string, string, string, string, int>("86A", "Intermediary 2"         , "", "O", 0),
            new TagData<string, string, string, string, int>("86D", "Intermediary 2"         , "", "O", 0),
            new TagData<string, string, string, string, int>("86J", "Intermediary 2"         , "", "O", 0),
            new TagData<string, string, string, string, int>("56A", "Intermediary"           , "", "O", 0),
            new TagData<string, string, string, string, int>("56D", "Intermediary"           , "", "O", 0),
            new TagData<string, string, string, string, int>("56J", "Intermediary"           , "", "O", 0),
            new TagData<string, string, string, string, int>("57A", "Receiving Agent"        , "", "M", 0),
            new TagData<string, string, string, string, int>("57D", "Receiving Agent"        , "", "M", 0),
            new TagData<string, string, string, string, int>("57J", "Receiving Agent"        , "", "M", 0),
            new TagData<string, string, string, string, int>("58A", "Beneficiary Institution", "", "O", 0),
            new TagData<string, string, string, string, int>("58D", "Beneficiary Institution", "", "O", 0),
            new TagData<string, string, string, string, int>("58J", "Beneficiary Institution", "", "O", 0)
        };

        // Sequence G - Optional
        // Sequence G Tax Information contains information about the tax regime.
        List<TagData<string, string, string, string, int>> sequenceG = new List<TagData<string, string, string, string, int>>
        {
            // Tag, Name, Value, Mandatory
            new TagData<string, string, string, string, int>("15G", "New Sequence"                                , "" ,"M", 0),
            new TagData<string, string, string, string, int>("37L", "Tax Rate"                                    , "", "M", 0),
            new TagData<string, string, string, string, int>("33B", "Transaction Currency and Net Interest Amount", "" ,"M", 0),
            new TagData<string, string, string, string, int>("36",  "Exchange Rate"                               , "" ,"O", 0),
            new TagData<string, string, string, string, int>("33E", "Reporting Currency and Tax Amount"           , "" ,"O", 0)
        };

        // Sequence H - Optional
        // Sequence H Additional Information provides information, which is not match-critical.
        List<TagData<string, string, string, string, int>> sequenceH = new List<TagData<string, string, string, string, int>>
        {
            // Tag, Name, Value, Mandatory
            new TagData<string, string, string, string, int>("15H", "New Sequence"                  , "" ,"M", 0),
            new TagData<string, string, string, string, int>("29A", "Contract Information"          , "", "O", 0),
            new TagData<string, string, string, string, int>("24D", "Dealing Method"                , "" ,"O", 0),
            new TagData<string, string, string, string, int>("84A", "Dealing Branch Party A"        , "", "O", 0),
            new TagData<string, string, string, string, int>("84B", "Dealing Branch Party A"        , "", "O", 0),
            new TagData<string, string, string, string, int>("84D", "Dealing Branch Party A"        , "", "O", 0),
            new TagData<string, string, string, string, int>("84J", "Dealing Branch Party A"        , "", "O", 0),
            new TagData<string, string, string, string, int>("85A", "Dealing Branch Party B"        , "", "O", 0),
            new TagData<string, string, string, string, int>("85B", "Dealing Branch Party B"        , "", "O", 0),
            new TagData<string, string, string, string, int>("85D", "Dealing Branch Party B"        , "", "O", 0),
            new TagData<string, string, string, string, int>("85J", "Dealing Branch Party B"        , "", "O", 0),
            new TagData<string, string, string, string, int>("88A", "Broker Identification"         , "", "O", 0),
            new TagData<string, string, string, string, int>("88D", "Broker Identification"         , "", "O", 0),
            new TagData<string, string, string, string, int>("88J", "Broker Identification"         , "", "O", 0),
            new TagData<string, string, string, string, int>("71F", "Broker's Commission"           , "", "O", 0),
            new TagData<string, string, string, string, int>("26H", "Counterparty's Reference"      , "" ,"O", 0),
            new TagData<string, string, string, string, int>("21G", "Broker's Reference"            , "" ,"O", 0),
            new TagData<string, string, string, string, int>("34C", "Commission and Fees"           , "", "O", 0),  // repeating
            new TagData<string, string, string, string, int>("72",  "Sender to Receiver Information", "", "O", 0)
        };

        // Sequence I - Optional
        // Sequence I Additional Amounts provides information on additional fees
        List<TagData<string, string, string, string, int>> sequenceI = new List<TagData<string, string, string, string, int>>
        {
            // Tag, Name, Value, Mandatory
            new TagData<string, string, string, string, int>("15I", "New Sequence"            , "" ,"M", 0),
            new TagData<string, string, string, string, int>("18A", "Number of Repetitions"   , "" ,"M", 0),
            new TagData<string, string, string, string, int>("30F", "Payment Date"            , "" ,"M", 0),  //   - Repeating fields 
            new TagData<string, string, string, string, int>("32H", "Currency, Payment Amount", "" ,"M", 0),  //   - Repeating fields
            new TagData<string, string, string, string, int>("53A", "Delivery Agent"          , "", "O", 0),
            new TagData<string, string, string, string, int>("53D", "Delivery Agent"          , "", "O", 0),
            new TagData<string, string, string, string, int>("53J", "Delivery Agent"          , "", "O", 0),
            new TagData<string, string, string, string, int>("86A", "Intermediary 2"          , "", "O", 0),
            new TagData<string, string, string, string, int>("86D", "Intermediary 2"          , "", "O", 0),
            new TagData<string, string, string, string, int>("86J", "Intermediary 2"          , "", "O", 0),
            new TagData<string, string, string, string, int>("56A", "Intermediary"            , "", "O", 0),
            new TagData<string, string, string, string, int>("56D", "Intermediary"            , "", "O", 0),
            new TagData<string, string, string, string, int>("56J", "Intermediary"            , "", "O", 0),
            new TagData<string, string, string, string, int>("57A", "Receiving Agent"         , "", "M", 0),
            new TagData<string, string, string, string, int>("57D", "Receiving Agent"         , "", "M", 0),
            new TagData<string, string, string, string, int>("57J", "Receiving Agent"         , "", "M", 0)
        };

        #endregion

        #region MESSAGE SETUP
        /// <summary>
        /// Method Constructor
        /// </summary>
        public MT320()
        {
            InitializeMT320();
        }

        /// <summary>
        /// Method Constructor
        /// </summary>
        /// <param name="msg"></param>
        public MT320(String msg)
        {
            InitializeMT320();

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
                else if (index == 1) { return sequenceB; }
                else if (index == 2) { return sequenceC; }
                else if (index == 3) { return sequenceD; }
                else if (index == 4) { return sequenceE; }
                else if (index == 5) { return sequenceF; }
                else if (index == 6) { return sequenceG; }
                else if (index == 7) { return sequenceH; }
                else if (index == 8) { return sequenceI; }
                else { return null; }
            }
        }

        private void InitializeMT320()
        {
            numOfSequences = 9;
            ResetVariables();
            DefineScope();
            Anomalies.Clear();
        }

        /// <summary>
        /// Definition of Message Scope
        /// This is the SWIFT definition of the message
        /// It can be found at https://www2.swift.com/knowledgecentre/publications/us3ma_20190719/1.0?topic=mt320-scope.htm
        /// </summary>
        protected override void DefineScope()
        {
            Scope = "MT 320 Scope:\r\n" + 
                    "This message is exchanged to confirm a fixed term loan/ deposit contract.\r\n" +
                    "The message is used to confirm / notify the details of:\r\n" +
                    "     a new contract between the parties\r\n" +
                    "     an amendment to a previously agreed contract\r\n" +
                    "     the cancellation of a confirmation\r\n" +
                    "     a rolled over/ renewed contract between the parties\r\n" +
                    "     the maturity of a contract.\r\n" +
                    "This message is exchanged by or on behalf of the institutions or corporates, party A and party B, who have agreed to a fixed term loan/ deposit contract.\r\n" +
                    "A money broker may also send this message to the two parties (party A and party B) for which he arranged the deal.\r\n" +
                    "If there are two money brokers involved in arranging a deal between party A and party B, this message can also be exchanged between these money brokers.\r\n" +
                    "Party A and party B are the legal entities which have agreed to the transaction.\r\n" +
                    "Party A is either:\r\n" +
                    "     the Sender, or\r\n" +
                    "     the institution/ corporate on behalf of which the message is sent, or\r\n" +
                    "     one of the institutions for which the broker arranged the deal and to whom it is sending the confirmation, or\r\n" +
                    "     when a money broker confirms to another money broker, the party for which the sending broker arranged the deal.\r\n" +
                    " Party B is either:\r\n" +
                    "     the Receiver, or\r\n" +
                    "     the institution / corporate on behalf of which the message is received, or\r\n" +
                    "     the other institution for which the broker arranged the deal, that is, party A's counterparty, or\r\n" +
                    "     when a money broker confirms to another money broker, party A's counterparty.\r\n";
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
            foreach (TagData<string, string, string, string, int> t in sequenceB)
            {
                t.Value = "";
                t.Present = 0;
            }
            foreach (TagData<string, string, string, string, int> t in sequenceC)
            {
                t.Value = "";
                t.Present = 0;
            }
            foreach (TagData<string, string, string, string, int> t in sequenceD)
            {
                t.Value = "";
                t.Present = 0;
            }
            foreach (TagData<string, string, string, string, int> t in sequenceE)
            {
                t.Value = "";
                t.Present = 0;
            }
            foreach (TagData<string, string, string, string, int> t in sequenceF)
            {
                t.Value = "";
                t.Present = 0;
            }
            foreach (TagData<string, string, string, string, int> t in sequenceG)
            {
                t.Value = "";
                t.Present = 0;
            }
            foreach (TagData<string, string, string, string, int> t in sequenceH)
            {
                t.Value = "";
                t.Present = 0;
            }
            foreach (TagData<string, string, string, string, int> t in sequenceI)
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
                case "B":
                    sequence = sequenceB;
                    break;
                case "C":
                    sequence = sequenceC;
                    break;
                case "D":
                    sequence = sequenceD;
                    break;
                case "E":
                    sequence = sequenceE;
                    break;
                case "F":
                    sequence = sequenceF;
                    break;
                case "G":
                    sequence = sequenceG;
                    break;
                case "H":
                    sequence = sequenceH;
                    break;
                case "I":
                    sequence = sequenceI;
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
            int idx = 0;

            switch (whichSequence(sequence))
            {
                case "A":
                case "B":
                case "C":
                case "D":
                case "E":
                case "F":
                case "G":
                    foreach (TagData<string, string, string, string, int> t in sequence)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            t.Value = value;
                            break;
                        }
                    }
                    break;
                case "H":
                    foreach (TagData<string, string, string, string, int> t in sequence)
                    {
                        if (tag.Equals("34C") == true && t.Tag.Equals("34C") && !t.Value.Equals(""))
                        {
                            idx = sequence.FindLastIndex((delegate (TagData<string, string, string, string, int> t1) { return t1.Tag == "34C"; }));
                            sequence.Insert(idx + 1, new TagData<string, string, string, string, int>("34C", "Commission and Fees", value, "O", 1));
                            break;
                        }
                        else if (tag.Equals(t.Tag) == true)
                        {
                            t.Value = value;
                            break;
                        }
                    }
                    break;
                case "I":
                    foreach (TagData<string, string, string, string, int> t in sequence)
                    {
                        if (tag.Equals("30F") == true && t.Tag.Equals("30F") && !t.Value.Equals(""))
                        {
                            idx = sequence.FindLastIndex((delegate (TagData<string, string, string, string, int> t1) { return t1.Tag == "32H"; }));
                            sequence.Insert(idx + 1, new TagData<string, string, string, string, int>("30F", "Commission and Fees", value, "O", 1));
                            break;
                        }
                        else if (tag.Equals("32H") == true && t.Tag.Equals("32H") && !t.Value.Equals(""))
                        {
                            idx = sequence.FindLastIndex((delegate (TagData<string, string, string, string, int> t1) { return t1.Tag == "30F"; }));
                            sequence.Insert(idx + 1, new TagData<string, string, string, string, int>("32H", "Commission and Fees", value, "O", 1));
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

        #region GET SET Functions
        /// <summary>
        /// Get / Set method to auto validate message.
        /// If set to true the message will be validated when read in.
        /// If set to false an explicit call will be needed after the message is read in.
        /// </summary>
        public bool AutoValidate { get; set; } = true;
        #endregion

        #region Validation Rules
        public bool IsMessageValid()
        {
            bool validMessage = true;

            if (AutoValidate == true)
            {
                ValidateTags();

                Valid_VR_C1();
                Valid_VR_C2();
                Valid_VR_C3();
                Valid_VR_C4();
                Valid_VR_C5();
                Valid_VR_C6();
                Valid_VR_C7();
                Valid_VR_C8();
                Valid_VR_C9();
                Valid_VR_C10();
                Valid_VR_C11();

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

            switch(sequence)
            {
                case "A":
                    switch(field.Tag)
                    {
                        case "15A":
                            if (Is_T15A_Valid(field) == false)
                                validTag = false;
                            break;
                        case "20":
                            if (Is_T20_Valid(field) == false)
                                validTag = false;
                            break;
                        case "21":
                            if (Is_T21_Valid(field) == false)
                                validTag = false;
                            break;
                        case "21N":
                            if (Is_T21N_Valid(field) == false)
                                validTag = false;
                            break;
                        case "22A":
                            if (Is_T22A_Valid(field) == false)
                                validTag = false;
                            break;
                        case "22B":
                            if (Is_T22B_Valid(field) == false)
                                validTag = false;
                            break;
                        case "22C":
                            if (Is_T22C_Valid(field) == false)
                                validTag = false;
                            break;
                        case "77D":
                            if (Is_T77_Valid(field) == false)
                                validTag = false;
                            break;
                        case "82A":
                        case "82D":
                        case "82J":
                            if (Is_T82_Valid(field) == false)
                                validTag = false;
                            break;
                        case "83A":
                        case "83D":
                        case "83J":
                            if (Is_T83_Valid(field) == false)
                                validTag = false;
                            break;
                        case "87A":
                        case "87D":
                        case "87J":
                            if (Is_T87_Valid(field) == false)
                                validTag = false;
                            break;
                        case "94A":
                            if (Is_T94A_Valid(field) == false)
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
                        case "14D":
                            if (Is_T14D_Valid(field) == false)
                                validTag = false;
                            break;
                        case "15B":
                            if (Is_T15B_Valid(field) == false)
                                validTag = false;
                            break;
                        case "17R":
                            if (Is_T17R_Valid(field) == false)
                                validTag = false;
                            break;
                        case "30F":
                        case "30P":
                        case "30T":
                        case "30V":
                        case "30X":
                            if (Is_T30_Valid(field) == false)
                                validTag = false;
                            break;
                        case "32B":
                            if (Is_T32B_Valid(field) == false)
                                validTag = false;
                            break;
                        case "32H":
                            if (Is_T32H_Valid(field) == false)
                                validTag = false;
                            break;
                        case "34E":
                            if (Is_T34E_Valid(field) == false)
                                validTag = false;
                            break;
                        case "37G":
                            if (Is_T37G_Valid(field) == false)
                                validTag = false;
                            break;
                        case "38J":
                            if (Is_T38J_Valid(field) == false)
                                validTag = false;
                            break;
                        case "39M":
                            if (Is_T39M_Valid(field) == false)
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
                        case "15C":
                            if (Is_T15C_Valid(field) == false)
                                validTag = false;
                            break;
                        case "53A":
                        case "53D":
                        case "53J":
                            if (Is_T53_Valid(field) == false)
                                validTag = false;
                            break;
                        case "56A":
                        case "56D":
                        case "56J":
                            if (Is_T56_Valid(field) == false)
                                validTag = false;
                            break;
                        case "57A":
                        case "57D":
                        case "57J":
                            if (Is_T57_Valid(field) == false)
                                validTag = false;
                            break;
                        case "58A":
                        case "58D":
                        case "58J":
                            if (Is_T58_Valid(field) == false)
                                validTag = false;
                            break;
                        case "86A":
                        case "86D":
                        case "86J":
                            if (Is_T86_Valid(field) == false)
                                validTag = false;
                            break;
                        default:
                            Anomalies.Add("ERROR : Unknown tag " + field.Tag + " in sequence C - can not validate.");
                            break;
                    }
                    break;
                case "D":
                    switch (field.Tag)
                    {
                        case "15D":
                            if (Is_T15D_Valid(field) == false)
                                validTag = false;
                            break;
                        case "53A":
                        case "53D":
                        case "53J":
                            if (Is_T53_Valid(field) == false)
                                validTag = false;
                            break;
                        case "56A":
                        case "56D":
                        case "56J":
                            if (Is_T56_Valid(field) == false)
                                validTag = false;
                            break;
                        case "57A":
                        case "57D":
                        case "57J":
                            if (Is_T57_Valid(field) == false)
                                validTag = false;
                            break;
                        case "58A":
                        case "58D":
                        case "58J":
                            if (Is_T58_Valid(field) == false)
                                validTag = false;
                            break;
                        case "86A":
                        case "86D":
                        case "86J":
                            if (Is_T86_Valid(field) == false)
                                validTag = false;
                            break;
                        default:
                            Anomalies.Add("ERROR : Unknown tag " + field.Tag + " in sequence D - can not validate.");
                            break;
                    }
                    break;
                case "E":
                    switch (field.Tag)
                    {
                        case "15E":
                            if (Is_T15E_Valid(field) == false)
                                validTag = false;
                            break;
                        case "53A":
                        case "53D":
                        case "53J":
                            if (Is_T53_Valid(field) == false)
                                validTag = false;
                            break;
                        case "56A":
                        case "56D":
                        case "56J":
                            if (Is_T56_Valid(field) == false)
                                validTag = false;
                            break;
                        case "57A":
                        case "57D":
                        case "57J":
                            if (Is_T57_Valid(field) == false)
                                validTag = false;
                            break;
                        case "58A":
                        case "58D":
                        case "58J":
                            if (Is_T58_Valid(field) == false)
                                validTag = false;
                            break;
                        case "86A":
                        case "86D":
                        case "86J":
                            if (Is_T86_Valid(field) == false)
                                validTag = false;
                            break;
                        default:
                            Anomalies.Add("ERROR : Unknown tag " + field.Tag + " in sequence E - can not validate.");
                            break;
                    }
                    break;
                case "F":
                    switch (field.Tag)
                    {
                        case "15F":
                            if (Is_T15F_Valid(field) == false)
                                validTag = false;
                            break;
                        case "53A":
                        case "53D":
                        case "53J":
                            if (Is_T53_Valid(field) == false)
                                validTag = false;
                            break;
                        case "56A":
                        case "56D":
                        case "56J":
                            if (Is_T56_Valid(field) == false)
                                validTag = false;
                            break;
                        case "57A":
                        case "57D":
                        case "57J":
                            if (Is_T57_Valid(field) == false)
                                validTag = false;
                            break;
                        case "58A":
                        case "58D":
                        case "58J":
                            if (Is_T58_Valid(field) == false)
                                validTag = false;
                            break;
                        case "86A":
                        case "86D":
                        case "86J":
                            if (Is_T86_Valid(field) == false)
                                validTag = false;
                            break;
                        default:
                            Anomalies.Add("ERROR : Unknown tag " + field.Tag + " in sequence F - can not validate.");
                            break;
                    }
                    break;
                case "G":
                    switch (field.Tag)
                    {
                        case "15G":
                            if (Is_T15G_Valid(field) == false)
                                validTag = false;
                            break;
                        case "33B":
                            if (Is_T33B_Valid(field) == false)
                                validTag = false;
                            break;
                        case "33E":
                            if (Is_T33E_Valid(field) == false)
                                validTag = false;
                            break;
                        case "36":
                            if (Is_T36_Valid(field) == false)
                                validTag = false;
                            break;
                        case "37L":
                            if (Is_T37L_Valid(field) == false)
                                validTag = false;
                            break;
                        default:
                            Anomalies.Add("ERROR : Unknown tag " + field.Tag + " in sequence G - can not validate.");
                            break;
                    }
                    break;
                case "H":
                    switch (field.Tag)
                    {
                        case "15H":
                            if (Is_T15H_Valid(field) == false)
                                validTag = false;
                            break;
                        case "21G":
                            if (Is_T21G_Valid(field) == false)
                                validTag = false;
                            break;
                        case "24D":
                            if (Is_T24D_Valid(field) == false)
                                validTag = false;
                            break;
                        case "26H":
                            if (Is_T26H_Valid(field) == false)
                                validTag = false;
                            break;
                        case "29A":
                            if (Is_T29A_Valid(field) == false)
                                validTag = false;
                            break;
                        case "34C":
                            if (Is_T34C_Valid(field) == false)
                                validTag = false;
                            break;
                        case "71F":
                            if (Is_T71F_Valid(field) == false)
                                validTag = false;
                            break;
                        case "72":
                            if (Is_T72_Valid(field) == false)
                                validTag = false;
                            break;
                        case "84A":
                        case "84B":
                        case "84D":
                        case "84J":
                            if (Is_T84_Valid(field) == false)
                                validTag = false;
                            break;
                        case "85A":
                        case "85B":
                        case "85D":
                        case "85J":
                            if (Is_T85_Valid(field) == false)
                                validTag = false;
                            break;
                        case "88A":
                        case "88D":
                        case "88J":
                            if (Is_T88_Valid(field) == false)
                                validTag = false;
                            break;
                        default:
                            Anomalies.Add("ERROR : Unknown tag " + field.Tag + " in sequence H - can not validate.");
                            break;
                    }
                    break;
                case "I":
                    switch (field.Tag)
                    {
                        case "15I":
                            if (Is_T15I_Valid(field) == false)
                                validTag = false;
                            break;
                        case "18A":
                            if (Is_T18A_Valid(field) == false)
                                validTag = false;
                            break;
                        case "30F":
                            if (Is_T30_Valid(field) == false)
                                validTag = false;
                            break;
                        case "32H":
                            if (Is_T32H_Valid(field) == false)
                                validTag = false;
                            break;
                        case "53A":
                        case "53D":
                        case "53J":
                            if (Is_T53_Valid(field) == false)
                                validTag = false;
                            break;
                        case "56A":
                        case "56D":
                        case "56J":
                            if (Is_T56_Valid(field) == false)
                                validTag = false;
                            break;
                        case "57A":
                        case "57D":
                        case "57J":
                            if (Is_T57_Valid(field) == false)
                                validTag = false;
                            break;
                        case "86A":
                        case "86D":
                        case "86J":
                            if (Is_T86_Valid(field) == false)
                                validTag = false;
                            break;
                        default:
                            Anomalies.Add("ERROR : Unknown tag " + field.Tag + " in sequence I - can not validate.");
                            break;
                    }
                    break;
                default:
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
            List<string> seqs = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I" };
            List<TagData<string, string, string, string, int>> seq;
            bool validateField = true;
            int T82present = 0;
            int T87present = 0;
            int T57present = 0;

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
                            if ((t.Tag.Equals("82A") && t.Present == 0) || (t.Tag.Equals("82D") && t.Present == 0) || (t.Tag.Equals("82J") && t.Present == 0))
                            {
                                validateField = false;
                                T82present++;
                            }
                            if ((t.Tag.Equals("87A") && t.Present == 0) || (t.Tag.Equals("87D") && t.Present == 0) || (t.Tag.Equals("87J") && t.Present == 0))
                            {
                                validateField = false;
                                T87present++;
                            }
                        }

                        if(sid.Equals("C") == true || sid.Equals("D") == true || sid.Equals("E") == true || sid.Equals("F") == true || sid.Equals("I") == true)
                        {
                            if ((t.Tag.Equals("57A") && t.Present == 0) || (t.Tag.Equals("57D") && t.Present == 0) || (t.Tag.Equals("57J") && t.Present == 0))
                            {
                                validateField = false;
                                T57present++;
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

                if ((sid.Equals("A") == true) && (T82present == 3))
                    Anomalies.Add("ERROR: Mandatory Tag 82 is not present in any variantion");
                else if ((sid.Equals("A") == true) && (T87present == 3))
                    Anomalies.Add("ERROR: Mandatory Tag 87 is not present in any variantion");
                else if ((sid.Equals("C") == true || sid.Equals("D") == true || sid.Equals("E") == true || sid.Equals("F") == true || sid.Equals("I") == true)
                    && T57present == 3)
                    Anomalies.Add("ERROR: Mandatory Tag 57 is not present in any variantion in sequence " + sid);
            }

            return allTagsValid;
        }

        #region FIELD VALIDATIONS
        #region SEQUENCE A TAG VALIDATIONS
        protected override bool Is_T21_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
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

        #endregion

        #region SEQUENCE B TAG VALIDATIONS
        
        #endregion

        #region SEQUENCE C TAG VALIDATIONS
        
        #endregion

        #region SEQUENCE D TAG VALIDATIONS
        
        #endregion

        #region SEQUENCE E TAG VALIDATIONS
        
        #endregion

        #region SEQUENCE F TAG VALIDATIONS
        
        #endregion

        #region SEQUENCE G TAG VALIDATIONS

        #endregion

        #region SEQUENCE H TAG VALIDATIONS
        
        #endregion

        #region SEQUENCE I TAG VALIDATIONS
        
        #endregion
        #endregion

        #region Network Validated Rules
        /// <summary>
        /// Validation rule C1 
        /// In sequence A, the presence of field 21 depends on the value of fields 22B and 22A as follows (Error code(s): D70):
        /// ====================================================================================================================
        ///    Sequence A                      Sequence A                      Sequence A
        /// if field 22B is ...             and if field 22A is ...         then field 21 is ...
        /// --------------------------------------------------------------------------------------------------------------------
        ///     CONF                              NEWT                          Optional
        ///     CONF                         Not equal to NEWT                  Mandatory
        ///  Not equal to CONF                 Any value                        Mandatory
        /// </summary>
        /// <returns></returns>
        private bool Valid_VR_C1()
        {
            bool valid = true;
            string t21 = GetTagValue(sequenceA, "21");
            string t22A = GetTagValue(sequenceA, "22A");
            string t22B = GetTagValue(sequenceA, "22B");

            if (t22B.Equals("CONF") == true)
            {
                if (t22A.Equals("NEWT") == false)
                {
                    if (t21.Equals("") == true)
                    {
                        valid = false;
                        Anomalies.Add("ERROR D70 : Validation Rule C1 failed: tag 21 must be present when 22B = CONF and 22A != NEWT");
                    }
                }
            }
            else
            {
                if (t21.Equals("") == true)
                {
                    valid = false;
                    Anomalies.Add("ERROR D70 : Validation Rule C1 failed: tag 21 must be present when 22B != CONF");
                }
            }

            return valid;
        }

        /// <summary>
        /// Validation Rule C2
        /// In sequence A, if field 94A is present and contains AGNT, then field 21N is mandatory, otherwise field 21N is optional (Error code(s): D72):
        /// ==============================================================================================================================================
        ///        Sequence A                   Sequence A
        ///    if field 94A is ...            then field 21N is ...             
        /// ------------------------------------------------------------------------
        ///         AGNT                        Mandatory
        ///         BILA                        Optional
        ///         BROK                        Optional
        ///      Not present                    Optional
        /// </summary>
        /// <returns></returns>
        private bool Valid_VR_C2()
        {
            bool valid = true;
            string t94A = GetTagValue(sequenceA, "94A");
            string t21N = GetTagValue(sequenceA, "21N");

            if (t94A.Equals("AGNT") == true)
            {
                if (t21N.Equals("") == true)
                {
                    valid = false;
                    Anomalies.Add("ERROR D72 : Validation Rule C2 failed: tag 21N must be present when 94A = AGNT ");
                }
            }

            return valid;
        }

        /// <summary>
        /// Validation Rule C3
        /// In sequence B, the presence of fields 32H and 30X depends on the value of field 22B in sequence A as follows (Error code(s): D56 ):
        /// ======================================================================================================================================
        ///    Sequence A                   Sequence B                  Sequence B
        ///  if field 22B is ...         then field 32H is ...        and field 30X is ...
        /// -----------------------------------------------------------------------------------------
        ///     CONF                        Not allowed                 Mandatory
        ///     MATU                        Mandatory                   Not allowed
        ///     ROLL                        Mandatory                   Mandatory
        /// </summary>
        /// <returns></returns>
        private bool Valid_VR_C3()
        {
            bool valid = true;
            string t22B = GetTagValue(sequenceA, "22B");
            string t32H = GetTagValue(sequenceB, "32H");
            string t30X = GetTagValue(sequenceB, "30X");

            if(t22B.Equals("CONF") == true)
            {
                if(t32H.Equals("") == false)
                {
                    valid = false;
                    Anomalies.Add("ERROR D56 : Validation Rule C3 failed : if tag 22B = CONF then tag 32H is not allowed");
                }
                if (t30X.Equals("") == true)
                {
                    valid = false;
                    Anomalies.Add("ERROR D56 : Validation Rule C3 failed : if tag 22B = CONF then tag 33X is mandatory");
                }
            }

            if (t22B.Equals("MATU") == true)
            {
                if (t32H.Equals("") == true)
                {
                    valid = false;
                    Anomalies.Add("ERROR D56 : Validation Rule C3 failed : if tag 22B = MATU then tag 32H is mandatory");
                }
                if (t30X.Equals("") == false)
                {
                    valid = false;
                    Anomalies.Add("ERROR D56 : Validation Rule C3 failed : if tag 22B = MATU then tag 33X is not allowed");
                }
            }

            if (t22B.Equals("ROLL") == true)
            {
                if (t32H.Equals("") == true)
                {
                    valid = false;
                    Anomalies.Add("ERROR D56 : Validation Rule C3 failed : if tag 22B = MATU then tag 32H is mandatory");
                }
                if (t30X.Equals("") == true)
                {
                    valid = false;
                    Anomalies.Add("ERROR D56 : Validation Rule C3 failed : if tag 22B = MATU then tag 33X is mandatory");
                }
            }

            return valid;
        }

        /// <summary>
        /// Validation Rule C4
        /// In sequence B, the values allowed for subfield 3 of field 32H (if present) depend on the values of fields 22B in sequence A and 17R in sequence B as follows (Error code(s): D57):
        /// ===================================================================================================================================================================================
        ///    Sequence A                   Sequence B                  Sequence B
        ///  if field 22B is ...         and field 17R is ...         then subfield 3 of field 32H must be ...
        /// ----------------------------------------------------------------------------------------------------------------- 
        ///     MATU                            L                           Negative or zero(*)
        ///     MATU                            B                           Positive or zero(*)
        ///     Not equal to MATU               Not applicable              Not applicable
        /// -----------------------------------------------------------------------------------------------------------------
        /// (*) The presence of the letter N(sign) in subfield 1 of field 32H specifies a negative amount.
        /// The absence of the letter N(sign) in subfield 1 of field 32H specifies a positive amount.
        /// If the value in subfield 3 of field 32H is zero, then the letter N(sign) in subfield 1 of field 32H is not allowed(Error code(s): T14 ).
        /// </summary>
        /// <returns></returns>
        private bool Valid_VR_C4()
        {
            bool valid = true;
            string t22B = GetTagValue(sequenceA, "22B");
            string t17R = GetTagValue(sequenceB, "17R");
            string t32H = GetTagValue(sequenceB, "32H");
            double num = 0.0;

            if(t22B.Equals("MATU") == true)
            {
                if (t17R.Equals("L") == true)
                {
                    Double.TryParse(t32H.Substring(0, t32H.Length), out num);
                    if (num > 0.0)
                    {
                        valid = false;
                        Anomalies.Add("ERROR D57 : Validation Rule C4 failed : if tag 22B == MATU and tag 17R == L then tag 32H must be <= 0.0 ");
                    }
                }

                if (t17R.Equals("B") == true)
                {
                    Double.TryParse(t32H.Substring(0, t32H.Length), out num);
                    if (num < 0.0)
                    {
                        valid = false;
                        Anomalies.Add("ERROR D57 : Validation Rule C4 failed : if tag 22B == MATU and tag 17R == B then tag 32H must be >= 0.0 ");
                    }
                }
            }
            else
            {
                if(t17R.Equals("") == false)
                {
                    valid = false;
                    Anomalies.Add("ERROR D57 : Validation Rule C4 failed : if tag 22B != MATU then tag 17R is not applicable");
                }

                if (t32H.Equals("") == false)
                {
                    valid = false;
                    Anomalies.Add("ERROR D57 : Validation Rule C4 failed : if tag 22B != MATU then tag 32H is not applicable");
                }
            }
            return valid;
        }

        /// <summary>
        /// Validation Rule C5
        /// In sequence A, if field 22B contains MATU, then field 30F in sequence B is not allowed, otherwise field 30F is optional (Error code(s): D69):
        /// ===================================================================================================================================================================================
        ///    Sequence A                   Sequence B        
        ///  if field 22B is ...         then field 30F is ...
        /// ----------------------------------------------------------------------------------------------------------------- 
        ///     MATU                            Not Allowed
        ///     Not equal to MATU               Optional
        /// -----------------------------------------------------------------------------------------------------------------
        /// </summary>
        /// <returns></returns>
        private bool Valid_VR_C5()
        {
            bool valid = true;
            string t22B = GetTagValue(sequenceA, "22B");
            string t30F = GetTagValue(sequenceB, "30F");
            
            if (t22B.Equals("MATU") == true)
            {
                if (!t30F.Equals("") == false)
                {
                    valid = false;
                    Anomalies.Add("ERROR D69 : Validation Rule C5 failed : if tag 22B = MATU then tag 30F is not allowed");
                }
            }
            
            return valid;
        }

        /// <summary>
        /// Validation Rule C6
        /// In sequence B, if field 30F is present then field 38J is mandatory, otherwise field 38J is not allowed (Error code(s): D60):
        /// ===================================================================================================================================================================================
        ///    Sequence B                      Sequence B        
        ///  if field 30F is ...            then field 38J is ...
        /// ----------------------------------------------------------------------------------------------------------------- 
        ///     Present                         Mandatory
        ///     Not Present                     Not Allowed
        /// -----------------------------------------------------------------------------------------------------------------
        /// </summary>
        /// <returns></returns>
        private bool Valid_VR_C6()
        {
            bool valid = true;
            string t30F = GetTagValue(sequenceB, "30F");
            string t38J = GetTagValue(sequenceB, "38J");

            if (!t30F.Equals("") == true)
            {
                if (t38J.Equals("") == true)
                {
                    valid = false;
                    Anomalies.Add("ERROR D60 : Validation Rule C6 failed : if tag 30F is present then field 38J is mandatory, otherwise field 38J is not allowed");
                }
            }
            else
            {
                if (!t38J.Equals("") == true)
                {
                    valid = false;
                    Anomalies.Add("ERROR D60 : Validation Rule C6 failed : if tag 30F is present then field 38J is mandatory, otherwise field 38J is not allowed");
                }
            }

            return valid;
        }

        /// <summary>
        /// Validation Rule C7
        /// In sequences C, D, E (if present), F (if present) and I (if present), if field 56a is not present, then field 86a in the same 
        /// sequence C, D, E, F or I is not allowed, otherwise field 86a is optional (Error code(s): E35):
        /// ===================================================================================================================================================================================
        ///    Sequence C                      Sequence C               Sequence D              Sequence E          Sequence F          Sequence I
        ///  if field 56a is ...            then field 86a is ...     then field 86a is ... then field 86a is ... then field 86a is ... then field 86a is ...               
        /// ---------------------------------------------------------------------------------------------------------------------------------------------------
        ///     Not Present                    Not Allowed              Not Allowed             Not Allowed         Not Allowed             Not Allowed
        ///     Present                         Optional                 Optional                Optional            Optional                Optional
        /// ---------------------------------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        /// <returns></returns>
        private bool Valid_VR_C7()
        {
            bool valid = true;
            string t56A = GetTagValue(sequenceC, "56A");
            string t56D = GetTagValue(sequenceC, "56D");
            string t56J = GetTagValue(sequenceC, "56J");
            string t86A = GetTagValue(sequenceC, "86A");
            string t86D = GetTagValue(sequenceC, "86D");
            string t86J = GetTagValue(sequenceC, "86J");

            if (t56A.Equals("") == true && t86A.Equals("") == false)
                valid = false;
            if (t56D.Equals("") == true && t86D.Equals("") == false)
                valid = false;
            if (t56J.Equals("") == true && t86J.Equals("") == false)
                valid = false;

            if (valid == false) 
                Anomalies.Add("ERROR E35 : In sequences C, D, E, F and I (if present), if field 56a is not present, then field 86a in the same sequence C, D, E, F or I is not allowed, otherwise field 86a is optional");

            return valid;
        }

        /// <summary>
        /// Validation Rule C8
        /// The presence of sequence H and the presence of fields 88a and 71F in sequence H, depends on the value of field 94A in sequence A as follows (Error code(s): D74):
        /// ===================================================================================================================================================================================
        ///     Sequence A          Then sequence H is ...         Sequence H                 Sequence H
        /// if field 94A is ...                                 and field 88a is ...        and field 71F is ...
        /// ---------------------------------------------------------------------------------------------------------------------------------------------------
        ///     Not present             Optional                    Optional                    Not allowed
        ///         AGNT                Optional                    Optional                    Not allowed
        ///         BILA                Optional                    Optional                    Not allowed
        ///         BROK                Mandatory                   Mandatory                   Optional
        /// ---------------------------------------------------------------------------------------------------------------------------------------------------        
        /// </summary>
        /// <returns></returns>
        private bool Valid_VR_C8()
        {
            bool valid = true;
            string t94A = GetTagValue(sequenceA, "94A");
            string t15H = GetTagValue(sequenceH, "15H");
            string t88A = GetTagValue(sequenceH, "88A");
            string t88D = GetTagValue(sequenceH, "88D");
            string t88J = GetTagValue(sequenceH, "88J");
            string t71F = GetTagValue(sequenceH, "71F");

            if (t94A.Equals("") == true && t71F.Equals("") == false)
                valid = false;
            if (t94A.Equals("AGNT") == true && t71F.Equals("") == false)
                valid = false;
            if (t94A.Equals("BILA") == true && t71F.Equals("") == false)
                valid = false;
            if (t94A.Equals("BROK") == true && t15H.Equals("") == true)
                valid = false;
            if (t94A.Equals("BROK") == true && (t88A.Equals("") == true || t88D.Equals("") == true || t88J.Equals("") == true))
                valid = false;

            if( valid == false)
                Anomalies.Add("ERROR D74 : The presence of sequence H and the presence of fields 88a and 71F in sequence H, depends on the value of field 94A in sequence A");

            return valid;
        }

        /// <summary>
        /// Validation Rule C9
        /// The currency code in the amount fields 32B, 32H and 34E in sequence B, and field 71F in sequence H must be the same (Error code(s): C02).
        /// </summary>
        /// <returns></returns>
        private bool Valid_VR_C9()
        {
            bool valid = true;
            string t32B_ccy = GetTagValue(sequenceB, "32B").Substring(0, 3);
            string t34E_ccy = GetTagValue(sequenceB, "34E").Substring(0, 1);
            string t32H_ccy = GetTagValue(sequenceB, "32H"); 
            string t71F_ccy = GetTagValue(sequenceH, "71F");

            if (!t32H_ccy.Equals(""))
            {
                if (t32H_ccy.Equals("N"))
                    t32H_ccy = GetTagValue(sequenceB, "32H").Substring(1, 4);
                else
                    t32H_ccy = GetTagValue(sequenceB, "32H").Substring(0, 3);
            }

            if (!t71F_ccy.Equals(""))
            {
                if (t71F_ccy.Equals("N"))
                    t71F_ccy = GetTagValue(sequenceH, "71F").Substring(1, 4);
                else
                    t71F_ccy = GetTagValue(sequenceH, "71F").Substring(0, 3);
            }

            if (t34E_ccy.Equals("N"))
                t34E_ccy = GetTagValue(sequenceB, "34E").Substring(1, 3);
            else
                t34E_ccy = GetTagValue(sequenceB, "34E").Substring(0, 3);

            if (CheckStringsForEquality(t32B_ccy, t32H_ccy, t34E_ccy, t71F_ccy) == false)
            {
                valid = false;
                Anomalies.Add("ERROR C02 - The currency code in the amount fields 32B, 32H and 34E in sequence B, and field 71F in sequence H must be the same");
            }

            return valid;
        }

        /// <summary>
        /// Validation Rule C10
        /// In sequence H, if field 15H is present, then at least one of the other fields of sequence H must be present (Error code(s): C98).
        /// </summary>
        /// <returns></returns>
        private bool Valid_VR_C10()
        {
            bool valid = false;

            if(isTagPresentInSequence(sequenceH, "15H") == true)
            {
                foreach (TagData<string, string, string, string, int> t in sequenceH)
                {
                    if (!t.Tag.Equals("15H") && isTagPresentInSequence(sequenceH, t.Tag) == true)
                    {
                        valid = true;
                        break;
                    }
                }
            }

            if(valid == false)
                Anomalies.Add("ERROR C98 - In sequence H, if field 15H is present, then at least one of the other fields of sequence H must be present");

            return valid;
        }

        /// <summary>
        /// Validation Rule C11
        /// In all optional sequences, the fields with status M must be present if the sequence is present, and are otherwise not allowed (Error code(s): C32)
        /// </summary>
        /// <returns></returns>
        private bool Valid_VR_C11()
        {
            bool validE = false;
            bool validF = false;
            bool validG = false;
            bool validH = false;
            bool validI = false;

            if (isTagPresentInSequence(sequenceE, "15E") == true)
            {
                foreach (TagData<string, string, string, string, int> t in sequenceE)
                {
                    if (isTagPresentInSequence(sequenceE, t.Value) == true)
                    {
                        validE = true;
                        break;
                    }
                }
            }

            if (isTagPresentInSequence(sequenceF, "15F") == true)
            {
                foreach (TagData<string, string, string, string, int> t in sequenceF)
                {
                    if (isTagPresentInSequence(sequenceF, t.Value) == true)
                    {
                        validF = true;
                        break;
                    }
                }
            }

            if (isTagPresentInSequence(sequenceG, "15G") == true)
            {
                foreach (TagData<string, string, string, string, int> t in sequenceG)
                {
                    if (isTagPresentInSequence(sequenceG, t.Value) == true)
                    {
                        validG = true;
                        break;
                    }
                }
            }

            if (isTagPresentInSequence(sequenceH, "15H") == true)
            {
                foreach (TagData<string, string, string, string, int> t in sequenceH)
                {
                    if (isTagPresentInSequence(sequenceH, t.Value) == true)
                    {
                        validH = true;
                        break;
                    }
                }
            }

            if (isTagPresentInSequence(sequenceI, "15I") == true)
            {
                foreach (TagData<string, string, string, string, int> t in sequenceI)
                {
                    if (isTagPresentInSequence(sequenceI, t.Value) == true)
                    {
                        validI = true;
                        break;
                    }
                }
            }

            return validE && validF && validG && validH && validI;
        }
        #endregion

        #endregion

        #region TAG PARSING
                
        /// <summary>
        /// parseCommissionAndFees
        /// 
        /// Parses repetitive field 34C in sequence H and returns :
        ///     Commission/Fee Type
        ///     Commission/Fee Currency
        ///     Commission/Fee Amount
        /// each in their own list
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="option"></param>
        /// <param name="type"></param>
        /// <param name="ccy"></param>
        /// <param name="amount"></param>
        private void parseCommissionAndFees(List<TagData<string, string, string, string, int>> seq, string option, out List<string> type, out List<string> ccy, out List<Nullable<double>> amount)
        {
            string rawStr = null;
            double multiplier = 1.0;
            string amtStr = null;

            type = new List<string>();
            ccy = new List<string>();
            amount = new List<Nullable<double>>();

            try
            {
                foreach (TagData<string, string, string, string, int> t in seq)
                {
                    if (t.Tag.Equals("34C") == true)
                    {
                        rawStr = t.Value;
                        if (rawStr != null && rawStr.Length >= 8 && isTagPresentInSequence(seq, option) == true)
                        {
                            type.Add(rawStr.Substring(0, 4));
                            rawStr = rawStr.Substring(5, rawStr.Length - 5);   // remove the type and '/' character
                            if (rawStr.Substring(0, 1).Equals("N") && (Char.IsLetter(rawStr[1]) == true && Char.IsLetter(rawStr[2]) == true && Char.IsLetter(rawStr[3]) == true))
                            {
                                multiplier = -1.0;
                                ccy.Add(rawStr.Substring(1, 3));
                                amtStr = rawStr.Substring(4, rawStr.Length - 4);
                                amtStr = amtStr.Replace(",", ".");
                                amount.Add(Convert.ToDouble(amtStr) * multiplier);
                            }
                            else
                            {
                                multiplier = 1.0;
                                ccy.Add(rawStr.Substring(0, 3));
                                amtStr = rawStr.Substring(3, rawStr.Length - 3);
                                amtStr = amtStr.Replace(",", ".");
                                amount.Add(Convert.ToDouble(amtStr) * multiplier);
                            }
                        }
                        else
                        {
                            type = null;
                            ccy = null;
                            amount = null;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Invalid Tag Data: Tag " + option + ".\n" + ex.Message);
            }
        }

        /// <summary>
        /// parseCommissionAndFees
        /// 
        /// Parses repetitive fields 30F and 32H in sequence I and returns :
        ///     Payment Date
        ///     Payment Currency
        ///     Payment Amount
        /// each in their own list
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="option"></param>
        /// <param name="type"></param>
        /// <param name="ccy"></param>
        /// <param name="amount"></param>
        private void parseAdditionalAmounts(List<TagData<string, string, string, string, int>> seq, out List<string> date, out List<string> ccy, out List<Nullable<double>> amount)
        {
            string rawStr = null;
            double multiplier = 1.0;
            string amtStr = null;

            date = new List<string>();
            ccy = new List<string>();
            amount = new List<Nullable<double>>();

            try
            {
                foreach (TagData<string, string, string, string, int> t in seq)
                {
                    if (t.Tag.Equals("30F") == true && isTagPresentInSequence(seq, "30F") == true)
                    {
                        date.Add(t.Value);
                    }

                    if (t.Tag.Equals("32H") == true)
                    {
                        rawStr = t.Value;
                        if (rawStr != null && rawStr.Length >= 8 && isTagPresentInSequence(seq, "32H") == true)
                        {
                            if (rawStr.Substring(0, 1).Equals("N") && (Char.IsLetter(rawStr[1]) == true && Char.IsLetter(rawStr[2]) == true && Char.IsLetter(rawStr[3]) == true))
                            {
                                multiplier = -1.0;
                                ccy.Add(rawStr.Substring(1, 3));
                                amtStr = rawStr.Substring(4, rawStr.Length - 4);
                                amtStr = amtStr.Replace(",", ".");
                                amount.Add(Convert.ToDouble(amtStr) * multiplier);
                            }
                            else
                            {
                                multiplier = 1.0;
                                ccy.Add(rawStr.Substring(0, 3));
                                amtStr = rawStr.Substring(3, rawStr.Length - 3);
                                amtStr = amtStr.Replace(",", ".");
                                amount.Add(Convert.ToDouble(amtStr) * multiplier);
                            }
                        }
                        else
                        {
                            date = null;
                            ccy = null;
                            amount = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid Tag Data - Aditional Amounts .\n" + ex.Message);
            }
        }

        /// <summary>
        /// parseRate
        /// 
        /// Returns the rate for Tags 36, 37G and 37L
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        private Nullable<double> parseRate(List<TagData<string, string, string, string, int>> seq, string option)
        {
            string rawStr = GetTagValue(seq, option);
            string rateStr = null;
            Nullable<double> rate = 0.0;
            double multiplier = 1.0;

            try
            {
                if (rawStr != null && rawStr.Length >= 1 && isTagPresentInSequence(seq, option) == true)
                {
                    if (rawStr.Substring(0, 1).Equals("N"))
                    {
                        multiplier = -1.0;
                        rateStr = rawStr.Substring(1, rawStr.Length - 1);
                        rateStr = rateStr.Replace(",", ".");
                        rate = Convert.ToDouble(rateStr) * multiplier;
                    }
                    else
                    {
                        multiplier = 1.0;
                        rateStr = rawStr.Substring(0, rawStr.Length);
                        rateStr = rateStr.Replace(",", ".");
                        rate = Convert.ToDouble(rateStr) * multiplier;
                    }
                }
                else
                {
                    rate = 0.0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid Tag Data: Tag " + option + ".\n" + ex.Message);
            }

            return rate;
        }

        /// <summary>
        /// parseT38J
        /// 
        /// Returns the Indicator and Number for Tag 38J
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="indicator"></param>
        /// <param name="number"></param>
        private void parseT38J(List<TagData<string, string, string, string, int>> seq, out string indicator, out Nullable<int> number)
        {
            string rawStr = GetTagValue(seq, "38J");
            indicator = null;
            number = null;
            
            if (rawStr != null && rawStr.Length >= 1 && isTagPresentInSequence(seq, "38J") == true)
            {
                try
                {
                    indicator = rawStr.Substring(0, 1);
                    number = Convert.ToInt32(rawStr.Substring(1, 3));
                }
                catch(Exception ex)
                {
                    throw new Exception("Invalid Tag Data: Tag 38J.\n" + ex.Message);
                }
            }
        }

        // =============================================================================================================================

        /// <summary>
        /// getT14D_DayCountFraction
        /// 
        /// Returns the number of days which are taken into account for the calculation of the interest.
        /// This field specifies the Day Count Fraction as per ISDA definitions.
        /// One of the following codes must be used :
        ///     30E/360         30E/360 or Eurobond Basis
        ///     360/360         30/360, 360/360 or Bond Basis
        ///     ACT/360         Actual/360 (28-31/360)
        ///     ACT/365         Actual/365 or Actual/Actual(28-31/365-6)
        ///     AFI/365         Actual/365 (fixed) (28 - 31 / 365)
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT14D_DayCountFraction(List<TagData<string, string, string, string, int>> seq)
        {
            return getT14D(seq);
        }

        /// <summary>
        /// getT17R_PartyARole
        /// 
        /// Specifies whether party A is the borrower or the lender
        /// B   Borrower: party A receives the principal amount and pays the interest.
        /// L   Lender: party A pays the principal amount and receives the interest.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT17R_PartyARole(List<TagData<string, string, string, string, int>> seq)
        {
            return GetTagValue(seq, "17R");
        }

        /// <summary>
        /// getT18A_repetitions
        /// 
        /// Returns the number of times fields 30F Payment Date and 32H Currency, Payment Amount are present in this sequence.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public Nullable<int> getT18A_repetitions(List<TagData<string, string, string, string, int>> seq)
        {
            return getT18A(seq);
        }

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
        /// getT21_RelatedReference
        /// 
        /// Returns the identification of the message to which the current message is related.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT21_RelatedReference(List<TagData<string, string, string, string, int>> seq)
        {
            return getT21(seq);
        }

        /// <summary>
        /// getT21G_BrokersReference
        /// 
        /// Returns the broker's reference of the trade.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT21G_BrokersReference(List<TagData<string, string, string, string, int>> seq)
        {
            return getT21G(seq);
        }

        /// <summary>
        /// getT21N_PartyAContractNum
        /// 
        /// Returns the contract number of the transaction from party A's viewpoint.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT21N_PartyAContractNum(List<TagData<string, string, string, string, int>> seq)
        {
            return getT21N(seq);
        }

        /// <summary>
        /// getT22A_OperationType
        /// 
        /// Returns the function of the message.
        /// Function must contain one of the following codes :
        /// AMND    Amendment           Replaces a previously sent message with the amended one contained in this message.
        ///                             Used to add settlement agents to an MT 320 previously sent or to correct error(s) in an MT 320 previously sent.
        /// CANC    Cancellation        Cancels a previously sent message.
        ///                             Used to cancel an MT 320 previously sent or to cancel an MT 320 which contains erroneous information.                   
        /// DUPL    Duplicate           Duplicates an already sent confirmation.
        /// NEWT    New Confirmation    Used to send the MT 320 for the first time or to send a corrected MT 320 when the erroneous one was cancelled using an MT 320 with function CANC.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT22A_OperationType(List<TagData<string, string, string, string, int>> seq)
        {
            return getT22A(seq);
        }

        /// <summary>
        /// getT22B_EventType
        /// 
        /// Returns the event in the life of the loan/deposit.
        /// Type must contain one of the following codes (Error code(s): T93):
        /// CONF    This is the first confirmation.     
        /// MATU    This is a confirmation of the liquidation of a fixed loan/ deposit.
        /// ROLL    This is a confirmation of a mutually agreed rollover / renewal with / without change in the principal amount and the interest to be settled or added / subtracted from that amount.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT22B_EventType(List<TagData<string, string, string, string, int>> seq)
        {
            return getT22B(seq);
        }

        /// <summary>
        /// getT22C_CommonReference
        /// 
        /// Returns the reference common to both the Sender and the Receiver.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT22C_CommonReference(List<TagData<string, string, string, string, int>> seq)
        {
            return getT22C(seq);
        }

        /// <summary>
        /// getT24D_DealingMethod
        /// 
        /// Returns how the deal was agreed.
        /// BROK        Deal made via a money broker
        /// ELEC        Deal made via an electronic system(Reuters, EBS etc.)
        /// PHON        Deal agreed on the phone
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT24D_DealingMethod(List<TagData<string, string, string, string, int>> seq)
        {
            string method = null;

            method = GetTagValue(seq, "24D");

            if (method.Length >= 4)
                method = method.Substring(0, 4);
            else
                method = null;

            return method;
        }

        /// <summary>
        /// getT24D_DealingMethodInfo
        /// 
        /// Returns any additional information on how the deal was agreed.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT24D_DealingMethodInfo(List<TagData<string, string, string, string, int>> seq)
        {
            return getT24D(seq);
        }

        /// <summary>
        /// getT26H_CounterpartyReference
        /// 
        /// Returns the counterparty's reference, if known.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT26H_CounterpartyReference(List<TagData<string, string, string, string, int>> seq)
        {
            return getT26H(seq);
        }

        /// <summary>
        /// getT29A_ContractInfo
        /// 
        /// Returns the name and/or telephone number of the person the Receiver may contact for any queries concerning this transaction.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT29A_ContractInfo(List<TagData<string, string, string, string, int>> seq)
        {
            return getT29A(seq);
        }

        /// <summary>
        /// getT30F_LastDayOfFirstInterestPeriod
        /// 
        /// Returns the last day of the first/next interest period.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT30F_LastDayOfFirstInterestPeriod(List<TagData<string, string, string, string, int>> seq)
        {
            return getT30F(seq);
        }

        /// <summary>
        /// getT30F32H_AdditionalAmounts
        /// 
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="date"></param>
        /// <param name="ccy"></param>
        /// <param name="amount"></param>
        public void getT30F32H_AdditionalAmounts(List<TagData<string, string, string, string, int>> seq, out List<string> date, out List<string> ccy, out List<Nullable<double>> amount)
        {
            try
            {
                parseAdditionalAmounts(seq, out date, out ccy, out amount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// getT30P_MaturityDate
        /// 
        /// Returns the latest agreed maturity date, that is, the date on which the principal is to be returned and the interest due.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT30P_MaturityDate(List<TagData<string, string, string, string, int>> seq)
        {
            return getT30P(seq);
        }

        /// <summary>
        /// getT30T_TradeDate
        /// 
        /// Returns the date the original deal or the rollover was agreed between party A and party B.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT30T_TradeDate(List<TagData<string, string, string, string, int>> seq)
        {
            return getT30T(seq);
        }

        /// <summary>
        /// getT30V_ValueDate
        /// 
        /// Returns :
        ///     For a new confirmation (22B=CONF), the value date of the deposit;
        ///     For a rollover(22B= ROLL), the value date of the rollover, that is, the maturity date of the rolled over deposit;
        ///     For a maturity confirmation(22B= MATU), the value date of the original deposit for a non-rolled over deposit or the value date of the previous rollover.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT30V_ValueDate(List<TagData<string, string, string, string, int>> seq)
        {
            return getT30V(seq);
        }

        /// <summary>
        /// getT30X_NetInterestDueDate
        /// 
        /// Returns the date the next interest is due.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT30X_NetInterestDueDate(List<TagData<string, string, string, string, int>> seq)
        {
            return getT30X(seq);
        }

        /// <summary>
        /// get32B_Currency
        /// 
        /// Returns the currency on which the interest specified in field 34E is calculated. 
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
            catch(Exception ex)
            {
                throw ex;
            }

            return ccy;
        }

        /// <summary>
        /// get32B_Amount
        /// 
        /// Returns the contract amount, that is, the amount on which the interest specified in field 34E is calculated. 
        /// For a new confirmation (22B=CONF), this amount has to be settled at value date.
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
        /// get32H_Currency
        /// 
        /// Returns the currency for Tag 32H
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT32H_Currency(List<TagData<string, string, string, string, int>> seq)
        {
            string ccy = null;
            Nullable<double> amount = null;

            try
            {
                parseCcyAmt(seq, "32H", out ccy, out amount);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ccy;
        }

        /// <summary>
        /// get32H_Amount
        /// 
        /// Returns :
        /// For a rollover confirmation (22B=ROLL), this field specifies the difference between the previous and the new principal amount, with interest included when interest is settled through the same cash flow.
        /// For a maturity confirmation(22B= MATU), this field specifies the amount with optional interest to be paid by the borrower at maturity date.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public Nullable<double> getT32H_Amount(List<TagData<string, string, string, string, int>> seq)
        {
            string ccy = null;
            Nullable<double> amount = null;

            try
            {
                parseCcyAmt(seq, "32H", out ccy, out amount);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return amount;
        }

        /// <summary>
        /// getT33B_Currency
        /// 
        /// Returns the currency for Tag 33B
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT33B_Currency(List<TagData<string, string, string, string, int>> seq)
        {
            string ccy = null;
            Nullable<double> amount = null;

            try
            {
                parseCcyAmt(seq, "33B", out ccy, out amount);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ccy;
        }

        /// <summary>
        /// getT33B_Amount
        /// 
        /// Returns the net interest amount (after deductions of taxes).
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public Nullable<double> getT33B_Amount(List<TagData<string, string, string, string, int>> seq)
        {
            string ccy = null;
            Nullable<double> amount = null;

            try
            {
                parseCcyAmt(seq, "33B", out ccy, out amount);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return amount;
        }

        /// <summary>
        /// getT33E_Currency
        /// 
        /// Returns the reporting currency.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT33E_Currency(List<TagData<string, string, string, string, int>> seq)
        {
            string ccy = null;
            Nullable<double> amount = null;

            try
            {
                parseCcyAmt(seq, "33E", out ccy, out amount);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ccy;
        }

        /// <summary>
        /// getT33E_Amount
        /// 
        /// Returns the tax amount.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public Nullable<double> getT33E_Amount(List<TagData<string, string, string, string, int>> seq)
        {
            string ccy = null;
            Nullable<double> amount = null;

            try
            {
                parseCcyAmt(seq, "33E", out ccy, out amount);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return amount;
        }

        /// <summary>
        /// getT34C_CommissionAndFees
        /// 
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="type"></param>
        /// <param name="ccy"></param>
        /// <param name="amount"></param>
        public void getT34C_CommissionAndFees(List<TagData<string, string, string, string, int>> seq, out List<string>type, out List<string>ccy, out List<Nullable<double>>amount)
        {
            try
            {
                parseCommissionAndFees(seq, "34C", out type, out ccy, out amount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// getT34E_Currency
        /// 
        /// Returns the currency for Tag 34E
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT34E_Currency(List<TagData<string, string, string, string, int>> seq)
        {
            string ccy = null;
            Nullable<double> amount = null;

            try
            {
                parseCcyAmt(seq, "34E", out ccy, out amount);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ccy;
        }

        /// <summary>
        /// getT34E_Amount
        /// 
        /// Returns :
        ///     For a new confirmation (22B=CONF), the first interest amount;
        ///     For a rollover confirmation(22B= ROLL), the next interest amount;
        ///     For a maturity confirmation(22B= MATU), the final interest amount to be settled at maturity.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public Nullable<double> getT34E_Amount(List<TagData<string, string, string, string, int>> seq)
        {
            string ccy = null;
            Nullable<double> amount = null;

            try
            {
                parseCcyAmt(seq, "34E", out ccy, out amount);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return amount;
        }

        /// <summary>
        /// get36_ExchangeRate
        /// 
        /// Returns the exchange rate between the transaction currency and the reporting currency.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public Nullable<double> getT36_ExchangeRate(List<TagData<string, string, string, string, int>> seq)
        {
            return getDouble(seq, "36");
        }

        /// <summary>
        /// get37G_InterestRate
        /// 
        /// Returns the interest rate.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public Nullable<double> getT37G_InterestRate(List<TagData<string, string, string, string, int>> seq)
        {
            return getDouble(seq, "37G");
        }

        /// <summary>
        /// get37L_TaxRate
        /// 
        /// Returns the tax percentage.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public Nullable<double> getT37L_TaxRate(List<TagData<string, string, string, string, int>> seq)
        {
            return getDouble(seq, "37L");
        }

        /// <summary>
        /// getT38J_Indicator
        /// 
        /// Returns the Indicator value for Tag 38G
        /// Indicator must contain one of the following codes :
        ///     D       Days
        ///     M       Months
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT38J_Indicator(List<TagData<string, string, string, string, int>> seq)
        {
            string indicator = null;
            Nullable<int> number = null;

            try
            {
                parseT38J(seq, out indicator, out number);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return indicator;
        }

        /// <summary>
        /// getT38J_Number
        /// 
        /// Returns the number of days or months between interest payments starting from the date specified in field 30F.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public Nullable<int> getT38J_Number(List<TagData<string, string, string, string, int>> seq)
        {
            string indicator = null;
            Nullable<int> number = null;

            try
            {
                parseT38J(seq, out indicator, out number);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return number;
        }

        /// <summary>
        /// getT39M_PaymentClearingCenter
        /// 
        /// Returns the place of clearing for offshore currency trades.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT39M_PaymentClearingCenter(List<TagData<string, string, string, string, int>> seq)
        {
            return GetTagValue(seq, "39M");
        }

        /// <summary>
        /// getT53A_ID
        /// 
        /// Returns the id of the financial institution from which party A will transfer the funds.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT53A_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "53A");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT53A_Code
        /// 
        /// Returns the code of the financial institution from which party A will transfer the funds.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT53A_Code(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "53A");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[1];
        }

        /// <summary>
        /// getT53D_ID
        /// 
        /// Returns the id of the financial institution from which party A will transfer the funds.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT53D_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "53D");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT53D_NameAddr
        /// 
        /// Returns the name and address of the financial institution from which party A will transfer the funds.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT53D_NameAddr(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "53D");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[1];
        }

        /// <summary>
        /// getT53J_ID
        /// 
        /// Returns the id of the financial institution from which party A will transfer the funds.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT53J_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "53J");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT56A_ID
        /// 
        /// Returns the id of the first intermediary institution for the transfer of the funds.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT56A_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "56A");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT56A_Code
        /// 
        /// Returns the code of the first intermediary institution for the transfer of the funds.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT56A_Code(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "56A");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[1];
        }

        /// <summary>
        /// getT56D_ID
        /// 
        /// Returns the id of the first intermediary institution for the transfer of the funds.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT56D_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "56D");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT56D_NameAddr
        /// 
        /// Returns the name and address of the first intermediary institution for the transfer of the funds.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT56D_NameAddr(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "56D");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[1];
        }

        /// <summary>
        /// getT56J_ID
        /// 
        /// Returns the id of the first intermediary institution for the transfer of the funds.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT56J_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "56J");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT57A_ID
        /// 
        /// Returns the id of the financial institution and account where party B will receive the payment.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT57A_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "57A");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT57A_Code
        /// 
        /// Returns the code of the financial institution and account where party B will receive the payment.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT57A_Code(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "57A");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[1];
        }

        /// <summary>
        /// getT57D_ID
        /// 
        /// Returns the id of the financial institution and account where party B will receive the payment.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT57D_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "57D");

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT57D_NameAddr
        /// 
        /// Returns the name and address of the financial institution and account where party B will receive the payment.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT57D_NameAddr(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "57D");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[1];
        }

        /// <summary>
        /// getT57J_ID
        /// 
        /// Returns the id of the financial institution and account where party B will receive the payment.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT57J_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "57J");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT58A_ID
        /// 
        /// Returns the id of the institution in favour of which the payment is made when different from Party B.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT58A_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "58A");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT58A_Code
        /// 
        /// Returns the code of the institution in favour of which the payment is made when different from Party B.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT58A_Code(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "58A");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[1];
        }

        /// <summary>
        /// getT58D_ID
        /// 
        /// Returns the id of the institution in favour of which the payment is made when different from Party B.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT58D_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "58D");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT58D_NameAddr
        /// 
        /// Returns the name and address of the institution in favour of which the payment is made when different from Party B.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT58D_NameAddr(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "58D");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[1];
        }

        /// <summary>
        /// getT58J_ID
        /// 
        /// Returns the id of the institution in favour of which the payment is made when different from Party B.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT58J_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "58J");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// get71F_Currency
        /// 
        /// Returns the currency of the brokerage fee for a broker confirmation.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT71F_Currency(List<TagData<string, string, string, string, int>> seq)
        {
            string ccy = null;
            Nullable<double> amount = null;

            try
            {
                parseCcyAmt(seq, "71F", out ccy, out amount);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ccy;
        }

        /// <summary>
        /// get71F_Amount
        /// 
        /// Returns the brokerage fee for a broker confirmation.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public Nullable<double> getT71F_Amount(List<TagData<string, string, string, string, int>> seq)
        {
            string ccy = null;
            Nullable<double> amount = null;

            try
            {
                parseCcyAmt(seq, "71F", out ccy, out amount);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return amount;
        }

        /// <summary>
        /// getT72_Sender2ReceiverInfo
        /// 
        /// Returns the additional information for the Receiver and applies to the whole messages.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT72_Sender2ReceiverInfo(List<TagData<string, string, string, string, int>> seq)
        {
            return getT72(seq);
        }

        /// <summary>
        /// getT77D_Terms
        /// 
        /// Returns the underlying legal agreement.
        /// Narrative may contain one of the following codes, placed between slashes ('/'):
        ///     FIDU        The trade is a fiduciary.
        ///     FLTR        The trade is a floating rate loan/deposit.
        ///     TBIL        The trade is the result of the issuance of a "Sterling Acceptance " or a "Treasury Bill" sent from the borrower to the lender and confirmed by the lender. At maturity, the borrower will pay a pre-arranged amount back to the lender.
        ///     WITH        Withholding taxes apply.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT77D_Terms(List<TagData<string, string, string, string, int>> seq)
        {
            return getT77D(seq);
        }

        /// <summary>
        /// getT82A_ID
        /// 
        /// Returns the Party A Identifier of Tag 82A for a given sequence
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT82A_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "82A");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT82A_Code
        /// 
        /// Returns the Party A Identifier Code of Tag 82A for a given sequence
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT82A_Code(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "82A");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[1];
        }

        /// <summary>
        /// getT82D_ID
        /// 
        /// Returns the Party A Identifier of Tag 82D for a given sequence
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT82D_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "82D");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT82D_NameAddr
        /// 
        /// Returns the Party A name and address of Tag 82D for a given sequence
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT82D_NameAddr(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "82D");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[1];
        }

        /// <summary>
        /// getT82J_ID
        /// 
        /// Returns the Party A Identifier of Tag 82J for a given sequence
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT82J_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "82J");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT83A_ID
        /// 
        /// Returns the id of the account information for party A, the underlying fund or instructing institution.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT83A_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "83A");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT83A_Code
        /// 
        /// Returns the code of the account information for party A, the underlying fund or instructing institution.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT83A_Code(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "83A");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[1];
        }

        /// <summary>
        /// getT83D_ID
        /// 
        /// Returns the id of the account information for party A, the underlying fund or instructing institution.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT83D_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "83D");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT83D_NameAddr
        /// 
        /// Returns the name and address of the account information for party A, the underlying fund or instructing institution.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT83D_NameAddr(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "83D");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[1];
        }

        /// <summary>
        /// getT83J_ID
        /// 
        /// Returns the id of the account information for party A, the underlying fund or instructing institution.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT83J_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "83J");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT84A_ID
        /// 
        /// Returns the id of the branch of party A with whom the deal was done.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT84A_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();
            try
            {
                lst = parsePartyAgent(seq, "84A");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT84A_Code
        /// 
        /// Returns the code of the branch of party A with whom the deal was done.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT84A_Code(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "84A");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[1];
        }

        /// <summary>
        /// getT84B_ID
        /// 
        /// Returns the id of the branch of party A with whom the deal was done.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT84B_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "84B");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT84B_Location
        /// 
        /// Returns the location of the branch of party A with whom the deal was done.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT84B_Location(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "84B");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[1];
        }

        /// <summary>
        /// getT84D_ID
        /// 
        /// Returns the id of the branch of party A with whom the deal was done.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT84D_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "84D");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT84D_NameAddr
        /// 
        /// Returns the name and address of the branch of party A with whom the deal was done.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT84D_NameAddr(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "84D");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[1];
        }

        /// <summary>
        /// getT84J_ID
        /// 
        /// Returns the id of the branch of party A with whom the deal was done.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT84J_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "84J");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT85A_ID
        /// 
        /// Returns the id of the branch of party B with whom the deal was done.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT85A_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "85A");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT85A_Code
        /// 
        /// Returns the code of the branch of party B with whom the deal was done.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT85A_Code(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "85A");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[1];
        }

        /// <summary>
        /// getT85B_ID
        /// 
        /// Returns the id of the branch of party B with whom the deal was done.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT85B_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "85B");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT85B_Location
        /// 
        /// Returns the location of the branch of party B with whom the deal was done.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT85B_Location(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "85B");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[1];
        }

        /// <summary>
        /// getT85D_ID
        /// 
        /// Returns the id of the branch of party B with whom the deal was done.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT85D_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "84D");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT85D_NameAddr
        /// 
        /// Returns the name and address of the branch of party B with whom the deal was done.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT85D_NameAddr(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "85D");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[1];
        }

        /// <summary>
        /// getT85J_ID
        /// 
        /// Returns the id of the branch of party B with whom the deal was done.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT85J_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "85J");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT86A_ID
        /// 
        /// Returns the id of the second intermediary institution for the transfer of the funds.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT86A_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "86A");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT86A_Code
        /// 
        /// Returns the code of the second intermediary institution for the transfer of the funds.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT86A_Code(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "86A");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[1];
        }

        /// <summary>
        /// getT86D_ID
        /// 
        /// Returns the id of the second intermediary institution for the transfer of the funds.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT86D_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "86D");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT86D_NameAddr
        /// 
        /// Returns the name and address of the second intermediary institution for the transfer of the funds.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT86D_NameAddr(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "86D");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[1];
        }

        /// <summary>
        /// getT86J_ID
        /// 
        /// Returns the id of the second intermediary institution for the transfer of the funds.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT86J_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "86J");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT87A_ID
        /// 
        /// Returns the Party B Identifier of Tag 87A for a given sequence
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT87A_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "87A");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT87A_Code
        /// 
        /// Returns the Party B Identifier Code of Tag 87A for a given sequence
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT87A_Code(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "87A");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[1];
        }

        /// <summary>
        /// getT87D_ID
        /// 
        /// Returns the Party B Identifier of Tag 87D for a given sequence
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT87D_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "87D");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT87D_NameAddr
        /// 
        /// Returns the Party B name and address of Tag 87D for a given sequence
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT87D_NameAddr(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "87D");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[1];
        }

        /// <summary>
        /// getT87J_ID
        /// 
        /// Returns the Party B Identifier of Tag 87J for a given sequence
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT87J_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "87J");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT88A_ID
        /// 
        /// This field specifies the broker which arranged the deal between party A and party B or, when two money brokers are involved, 
        /// between party A and the other money broker.
        /// Returns the Party B Identifier of Tag 87A for a given sequence
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT88A_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "88A");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT88A_Code
        /// 
        /// This field specifies the broker which arranged the deal between party A and party B or, when two money brokers are involved, 
        /// between party A and the other money broker.
        /// Returns the Party B Identifier Code of Tag 87A for a given sequence
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT88A_Code(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "88A");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[1];
        }

        /// <summary>
        /// getT88D_ID
        /// 
        /// This field specifies the broker which arranged the deal between party A and party B or, when two money brokers are involved, 
        /// between party A and the other money broker.
        /// Returns the Party B Identifier of Tag 87D for a given sequence
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT88D_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "88D");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT88D_NameAddr
        /// 
        /// This field specifies the broker which arranged the deal between party A and party B or, when two money brokers are involved, 
        /// between party A and the other money broker.
        /// Returns the Party B name and address of Tag 87D for a given sequence
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT88D_NameAddr(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "88D");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[1];
        }

        /// <summary>
        /// getT88J_ID
        /// 
        /// This field specifies the broker which arranged the deal between party A and party B or, when two money brokers are involved, 
        /// between party A and the other money broker.
        /// Returns the Party B Identifier of Tag 87J for a given sequence
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT88J_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = new List<string>();

            try
            {
                lst = parsePartyAgent(seq, "88J");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst[0];
        }

        /// <summary>
        /// getT94A_Scope
        /// 
        /// Returns the role of the Sender and the Receiver of the message in the conclusion of the confirmed trade.
        /// Scope must contain one of the following codes :
        ///     AGNT        Sender/Receiver is sending/receiving the message on behalf of a third party.
        ///     BILA        Bilateral confirmation, that is, Sender and Receiver are the principals.
        ///     BROK        Confirmation is sent by a money broker.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT94A_Scope(List<TagData<string, string, string, string, int>> seq)
        {
            return getT94A(seq);
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
                    sqlCmd = "Select max(reference_id) from dbo.MT320_Block1";
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
                sqlCmd = "INSERT INTO dbo.MT320_Block1 (reference_id, application_id, service_id, lt_address, bic_code, logical_terminal_code, bic_branch_code, session_number, sequence_number) ";
                sqlCmd += "VALUES('" + refid + "', '" + hdr.ApplicationID + "', '" + hdr.ServiceID + "', '" + hdr.LTAddress + "', '" + hdr.BICCode + "', '" + hdr.LogicalTerminalCode + "', '" + hdr.BICBranchCode + "', '" + hdr.SessionNumber + "', '" + hdr.SequenceNumber + "')";

                dbu.saveMTRecord(sqlCmd);
            }
            catch(Exception ex)
            {
                throw new Exception("Failed to insert MT320 Block1 record.\n" + ex.Message);
            }
        }

        private void saveBlock2(long refid, BlockHeader hdr)
        {
            string sqlCmd = null;

            if (hdr == null)
                return;

            try
            {
                sqlCmd = "INSERT INTO dbo.MT320_Block2 (reference_id, input_output_id, message_type, destination_address, priority, delivery_monitoring, ";
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
                throw new Exception("Failed to insert MT320 Block2 record.\n" + ex.Message);
            }
        }

        private void saveBlock3(long refid, BlockHeader hdr)
        {
            string sqlCmd = null;

            if (hdr == null)
                return;

            try
            {
                sqlCmd = "INSERT INTO dbo.MT320_Block3 (reference_id, tag103_service_id, tag113_banking_priority, tag108_mur, tag119_validation_flag, ";
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
                throw new Exception("Failed to insert MT320 Block3 record.\n" + ex.Message);
            }
        }

        private void saveBlock4(long refid)
        {
            string sqlCmd = null;

            try
            {
                sqlCmd = "INSERT INTO dbo.MT320_SequenceA (reference_id, senders_ref_20, related_ref_21, operation_type_22a, operation_scope_94a, event_type_22b, ";
                sqlCmd += "common_ref_22c, party_a_contact_num_21n, party_a_id_82a, party_a_code_82a, party_a_id_82d, party_a_addr_82d, party_a_id_82j, party_b_id_87a, ";
                sqlCmd += "party_b_code_87a, party_b_id_87d, party_b_addr_87d, party_b_id_87j, fund_party_id_83a, fund_party_code_83a, fund_party_id_83d, ";
                sqlCmd += "fund_party_addr_83d, fund_party_id_83j, terms_77d)";
                sqlCmd += "VALUES ('" + refid + "', '" +
                                    getT20_SendersReference(sequenceA) + "', '" +
                                    getT21_RelatedReference(sequenceA) + "', '" +
                                    getT22A_OperationType(sequenceA) + "', '" +
                                    getT94A_Scope(sequenceA) + "', '" +
                                    getT22B_EventType(sequenceA) + "', '" +
                                    getT22C_CommonReference(sequenceA) + "', '" +
                                    getT21N_PartyAContractNum(sequenceA) + "', '" +
                                    getT82A_ID(sequenceA) + "', '" +
                                    getT82A_Code(sequenceA) + "', '" +
                                    getT82D_ID(sequenceA) + "', '" +
                                    getT82D_NameAddr(sequenceA) + "', '" +
                                    getT82J_ID(sequenceA) + "', '" +
                                    getT87A_ID(sequenceA) + "', '" +
                                    getT87A_Code(sequenceA) + "', '" +
                                    getT87D_ID(sequenceA) + "', '" +
                                    getT87D_NameAddr(sequenceA) + "', '" +
                                    getT87J_ID(sequenceA) + "', '" +
                                    getT83A_ID(sequenceA) + "', '" +
                                    getT83A_Code(sequenceA) + "', '" +
                                    getT83D_ID(sequenceA) + "', '" +
                                    getT83D_NameAddr(sequenceA) + "', '" +
                                    getT83J_ID(sequenceA) + "', '" +
                                    getT77D_Terms(sequenceA) + "')";
                dbu.saveMTRecord(sqlCmd);

                sqlCmd = "INSERT INTO dbo.MT320_SequenceB (reference_id, party_a_role_17r, trade_date_30t, value_date_30v, maturity_date_30p, principal_currency_32b, ";
                sqlCmd += "principal_amount_32b, settle_amount_ccy_32h, settle_amount_32h, next_interest_due_date_30x, interest_currency_34e, interest_amount_34e, ";
                sqlCmd += "interest_rate_37g, day_count_fraction_14d, last_day_first_interest_period_30f, number_of_days_indicator_38j, number_of_days_38j, payment_clearing_center_39m)";
                sqlCmd += "VALUES ('" + refid + "', '" +
                                    getT17R_PartyARole(sequenceB) + "', '" +
                                    getT30T_TradeDate(sequenceB) + "', '" +
                                    getT30V_ValueDate(sequenceB) + "', '" +
                                    getT30P_MaturityDate(sequenceB) + "', '" +
                                    getT32B_Currency(sequenceB) + "', '" +
                                    getT32B_Amount(sequenceB) + "', '" +
                                    getT32H_Currency(sequenceB) + "', '" +
                                    getT32H_Amount(sequenceB) + "', '" +
                                    getT30X_NetInterestDueDate(sequenceB) + "', '" +
                                    getT34E_Currency(sequenceB) + "', '" +
                                    getT34E_Amount(sequenceB) + "', '" +
                                    getT37G_InterestRate(sequenceB) + "', '" +
                                    getT14D_DayCountFraction(sequenceB) + "', '" +
                                    getT30F_LastDayOfFirstInterestPeriod(sequenceB) + "', '" +
                                    getT38J_Indicator(sequenceB) + "', '" +
                                    getT38J_Number(sequenceB) + "', '" +
                                    getT39M_PaymentClearingCenter(sequenceB) + "')";
                dbu.saveMTRecord(sqlCmd);

                sqlCmd = "INSERT INTO dbo.MT320_SequenceC (reference_id, delevery_agent_id_53a, delevery_agent_code_53a, delevery_agent_id_53d, delevery_agent_addr_53d, ";
                sqlCmd += "delevery_agent_id_53j, intermediary_2_id_86a, intermediary_2_code_86a, intermediary_2_id_86d, intermediary_2_addr_86d, intermediary_2_id_86j, ";
                sqlCmd += "intermediary_id_56a, intermediary_code_56a, intermediary_id_56d, intermediary_addr_56d, intermediary_id_56j, receiving_agent_id_57a, ";
                sqlCmd += "receiving_agent_code_57a, receiving_agent_id_57d, receiving_agent_addr_57d, receiving_agent_id_57j, beneficiary_inst_id_58a, beneficiary_inst_code_58a, ";
                sqlCmd += "beneficiary_inst_id_58d, beneficiary_inst_addr_58d, beneficiary_inst_id_58j)";
                sqlCmd += "VALUES ('" + refid + "', '" +
                                    getT53A_ID(sequenceC) + "', '" +
                                    getT53A_Code(sequenceC) + "', '" +
                                    getT53D_ID(sequenceC) + "', '" +
                                    getT53D_NameAddr(sequenceC) + "', '" +
                                    getT53J_ID(sequenceC) + "', '" +
                                    getT86A_ID(sequenceC) + "', '" +
                                    getT86A_Code(sequenceC) + "', '" +
                                    getT86D_ID(sequenceC) + "', '" +
                                    getT86D_NameAddr(sequenceC) + "', '" +
                                    getT86J_ID(sequenceC) + "', '" +
                                    getT56A_ID(sequenceC) + "', '" +
                                    getT56A_Code(sequenceC) + "', '" +
                                    getT56D_ID(sequenceC) + "', '" +
                                    getT56D_NameAddr(sequenceC) + "', '" +
                                    getT56J_ID(sequenceC) + "', '" +
                                    getT57A_ID(sequenceC) + "', '" +
                                    getT57A_Code(sequenceC) + "', '" +
                                    getT57D_ID(sequenceC) + "', '" +
                                    getT57D_NameAddr(sequenceC) + "', '" +
                                    getT57J_ID(sequenceC) + "', '" +
                                    getT58A_ID(sequenceC) + "', '" +
                                    getT58A_Code(sequenceC) + "', '" +
                                    getT58D_ID(sequenceC) + "', '" +
                                    getT58D_NameAddr(sequenceC) + "', '" +
                                    getT58J_ID(sequenceC) + "')";
                dbu.saveMTRecord(sqlCmd);

                sqlCmd = "INSERT INTO dbo.MT320_SequenceD (reference_id, delevery_agent_id_53a, delevery_agent_code_53a, delevery_agent_id_53d, delevery_agent_addr_53d, ";
                sqlCmd += "delevery_agent_id_53j, intermediary_2_id_86a, intermediary_2_code_86a, intermediary_2_id_86d, intermediary_2_addr_86d, intermediary_2_id_86j, ";
                sqlCmd += "intermediary_id_56a, intermediary_code_56a, intermediary_id_56d, intermediary_addr_56d, intermediary_id_56j, receiving_agent_id_57a, ";
                sqlCmd += "receiving_agent_code_57a, receiving_agent_id_57d, receiving_agent_addr_57d, receiving_agent_id_57j, beneficiary_inst_id_58a, beneficiary_inst_code_58a, ";
                sqlCmd += "beneficiary_inst_id_58d, beneficiary_inst_addr_58d, beneficiary_inst_id_58j)";
                sqlCmd += "VALUES ('" + refid + "', '" +
                                    getT53A_ID(sequenceD) + "', '" +
                                    getT53A_Code(sequenceD) + "', '" +
                                    getT53D_ID(sequenceD) + "', '" +
                                    getT53D_NameAddr(sequenceD) + "', '" +
                                    getT53J_ID(sequenceD) + "', '" +
                                    getT86A_ID(sequenceD) + "', '" +
                                    getT86A_Code(sequenceD) + "', '" +
                                    getT86D_ID(sequenceD) + "', '" +
                                    getT86D_NameAddr(sequenceD) + "', '" +
                                    getT86J_ID(sequenceD) + "', '" +
                                    getT56A_ID(sequenceD) + "', '" +
                                    getT56A_Code(sequenceD) + "', '" +
                                    getT56D_ID(sequenceD) + "', '" +
                                    getT56D_NameAddr(sequenceD) + "', '" +
                                    getT56J_ID(sequenceD) + "', '" +
                                    getT57A_ID(sequenceD) + "', '" +
                                    getT57A_Code(sequenceD) + "', '" +
                                    getT57D_ID(sequenceD) + "', '" +
                                    getT57D_NameAddr(sequenceD) + "', '" +
                                    getT57J_ID(sequenceD) + "', '" +
                                    getT58A_ID(sequenceD) + "', '" +
                                    getT58A_Code(sequenceD) + "', '" +
                                    getT58D_ID(sequenceD) + "', '" +
                                    getT58D_NameAddr(sequenceD) + "', '" +
                                    getT58J_ID(sequenceD) + "')";
                dbu.saveMTRecord(sqlCmd);

                sqlCmd = "INSERT INTO dbo.MT320_SequenceE (reference_id, delevery_agent_id_53a, delevery_agent_code_53a, delevery_agent_id_53d, delevery_agent_addr_53d, ";
                sqlCmd += "delevery_agent_id_53j, intermediary_2_id_86a, intermediary_2_code_86a, intermediary_2_id_86d, intermediary_2_addr_86d, intermediary_2_id_86j, ";
                sqlCmd += "intermediary_id_56a, intermediary_code_56a, intermediary_id_56d, intermediary_addr_56d, intermediary_id_56j, receiving_agent_id_57a, ";
                sqlCmd += "receiving_agent_code_57a, receiving_agent_id_57d, receiving_agent_addr_57d, receiving_agent_id_57j, beneficiary_inst_id_58a, beneficiary_inst_code_58a, ";
                sqlCmd += "beneficiary_inst_id_58d, beneficiary_inst_addr_58d, beneficiary_inst_id_58j)";
                sqlCmd += "VALUES ('" + refid + "', '" +
                                    getT53A_ID(sequenceE) + "', '" +
                                    getT53A_Code(sequenceE) + "', '" +
                                    getT53D_ID(sequenceE) + "', '" +
                                    getT53D_NameAddr(sequenceE) + "', '" +
                                    getT53J_ID(sequenceE) + "', '" +
                                    getT86A_ID(sequenceE) + "', '" +
                                    getT86A_Code(sequenceE) + "', '" +
                                    getT86D_ID(sequenceE) + "', '" +
                                    getT86D_NameAddr(sequenceE) + "', '" +
                                    getT86J_ID(sequenceE) + "', '" +
                                    getT56A_ID(sequenceE) + "', '" +
                                    getT56A_Code(sequenceE) + "', '" +
                                    getT56D_ID(sequenceE) + "', '" +
                                    getT56D_NameAddr(sequenceE) + "', '" +
                                    getT56J_ID(sequenceE) + "', '" +
                                    getT57A_ID(sequenceE) + "', '" +
                                    getT57A_Code(sequenceE) + "', '" +
                                    getT57D_ID(sequenceE) + "', '" +
                                    getT57D_NameAddr(sequenceE) + "', '" +
                                    getT57J_ID(sequenceE) + "', '" +
                                    getT58A_ID(sequenceE) + "', '" +
                                    getT58A_Code(sequenceE) + "', '" +
                                    getT58D_ID(sequenceE) + "', '" +
                                    getT58D_NameAddr(sequenceE) + "', '" +
                                    getT58J_ID(sequenceE) + "')";
                dbu.saveMTRecord(sqlCmd);

                sqlCmd = "INSERT INTO dbo.MT320_SequenceF (reference_id, delevery_agent_id_53a, delevery_agent_code_53a, delevery_agent_id_53d, delevery_agent_addr_53d, ";
                sqlCmd += "delevery_agent_id_53j, intermediary_2_id_86a, intermediary_2_code_86a, intermediary_2_id_86d, intermediary_2_addr_86d, intermediary_2_id_86j, ";
                sqlCmd += "intermediary_id_56a, intermediary_code_56a, intermediary_id_56d, intermediary_addr_56d, intermediary_id_56j, receiving_agent_id_57a, ";
                sqlCmd += "receiving_agent_code_57a, receiving_agent_id_57d, receiving_agent_addr_57d, receiving_agent_id_57j, beneficiary_inst_id_58a, beneficiary_inst_code_58a, ";
                sqlCmd += "beneficiary_inst_id_58d, beneficiary_inst_addr_58d, beneficiary_inst_id_58j)";
                sqlCmd += "VALUES ('" + refid + "', '" +
                                    getT53A_ID(sequenceF) + "', '" +
                                    getT53A_Code(sequenceF) + "', '" +
                                    getT53D_ID(sequenceF) + "', '" +
                                    getT53D_NameAddr(sequenceF) + "', '" +
                                    getT53J_ID(sequenceF) + "', '" +
                                    getT86A_ID(sequenceF) + "', '" +
                                    getT86A_Code(sequenceF) + "', '" +
                                    getT86D_ID(sequenceF) + "', '" +
                                    getT86D_NameAddr(sequenceF) + "', '" +
                                    getT86J_ID(sequenceF) + "', '" +
                                    getT56A_ID(sequenceF) + "', '" +
                                    getT56A_Code(sequenceF) + "', '" +
                                    getT56D_ID(sequenceF) + "', '" +
                                    getT56D_NameAddr(sequenceF) + "', '" +
                                    getT56J_ID(sequenceF) + "', '" +
                                    getT57A_ID(sequenceF) + "', '" +
                                    getT57A_Code(sequenceF) + "', '" +
                                    getT57D_ID(sequenceF) + "', '" +
                                    getT57D_NameAddr(sequenceF) + "', '" +
                                    getT57J_ID(sequenceF) + "', '" +
                                    getT58A_ID(sequenceF) + "', '" +
                                    getT58A_Code(sequenceF) + "', '" +
                                    getT58D_ID(sequenceF) + "', '" +
                                    getT58D_NameAddr(sequenceF) + "', '" +
                                    getT58J_ID(sequenceF) + "')";
                dbu.saveMTRecord(sqlCmd);

                sqlCmd = "INSERT INTO dbo.MT320_SequenceG (reference_id, tax_rate_37l, transaction_currency_33b, transaction_net_interest_amount_33b, exchange_rate, ";
                sqlCmd += "reporting_currency_33e, reporting_tax_amount_33e)";
                sqlCmd += "VALUES ('" + refid + "', '" +
                                    getT37L_TaxRate(sequenceF) + "', '" +
                                    getT33B_Currency(sequenceF) + "', '" +
                                    getT33B_Amount(sequenceF) + "', '" +
                                    getT36_ExchangeRate(sequenceF) + "', '" +
                                    getT33E_Currency(sequenceF) + "', '" +
                                    getT33E_Amount(sequenceF) + "')";
                dbu.saveMTRecord(sqlCmd);

                sqlCmd = "INSERT INTO dbo.MT320_SequenceH (reference_id, contact_information_29a, dealing_method_24d, dealing_method_information_24d, dealing_branch_party_a_id_84a, ";
                sqlCmd += "dealing_branch_party_a_code_84a, dealing_branch_party_a_id_84b, dealing_branch_party_a_loc_84b, dealing_branch_party_a_id_84d, dealing_branch_party_a_addr_84d, ";
                sqlCmd += "dealing_branch_party_a_id_84j, dealing_branch_party_b_id_85a, dealing_branch_party_b_code_85a, dealing_branch_party_b_id_85b, dealing_branch_party_b_loc_85b, ";
                sqlCmd += "dealing_branch_party_b_id_85d, dealing_branch_party_b_addr_85d, dealing_branch_party_b_id_85j, broker_identification_id_88a, broker_identification_code_88a, ";
                sqlCmd += "broker_identification_id_88d, broker_identification_addr_88d, broker_identification_id_88j, send_receive_information_72)";
                sqlCmd += "VALUES ('" + refid + "', '" +
                                    getT29A_ContractInfo(sequenceH) + "', '" +
                                    getT24D_DealingMethod(sequenceH) + "', '" +
                                    getT24D_DealingMethodInfo(sequenceH) + "', '" +
                                    getT84A_ID(sequenceH) + "', '" +
                                    getT84A_Code(sequenceH) + "', '" +
                                    getT84B_ID(sequenceH) + "', '" +
                                    getT84B_Location(sequenceH) + "', '" +
                                    getT84D_ID(sequenceH) + "', '" +
                                    getT84D_NameAddr(sequenceH) + "', '" +
                                    getT84J_ID(sequenceH) + "', '" +
                                    getT85A_ID(sequenceH) + "', '" +
                                    getT85A_Code(sequenceH) + "', '" +
                                    getT85B_ID(sequenceH) + "', '" +
                                    getT85B_Location(sequenceH) + "', '" +
                                    getT85D_ID(sequenceH) + "', '" +
                                    getT85D_NameAddr(sequenceH) + "', '" +
                                    getT85J_ID(sequenceH) + "', '" +
                                    getT88A_ID(sequenceH) + "', '" +
                                    getT88A_Code(sequenceH) + "', '" +
                                    getT88D_ID(sequenceH) + "', '" +
                                    getT88D_NameAddr(sequenceH) + "', '" +
                                    getT88J_ID(sequenceH) + "', '" +
                                    getT72_Sender2ReceiverInfo(sequenceH) + "')";
                dbu.saveMTRecord(sqlCmd);

                List<string> feeType = new List<string>();
                List<string> feeCcy = new List<string>();
                List<Nullable<double>> feeAmt = new List<Nullable<double>>();
                getT34C_CommissionAndFees(sequenceH, out feeType, out feeCcy, out feeAmt);

                for(int i = 0; i < feeType.Count(); i++)
                {
                    sqlCmd = "INSERT INTO dbo.MT320_CommissionFees (reference_id, commission_type_34c, currency_percent_34c, amount_rate_34c)";
                    sqlCmd += "VALUES ('" + refid + "', '" +
                                            feeType[i] + "', '" +
                                            feeCcy[i] + "', '" +
                                            feeAmt[i] + "')";
                    dbu.saveMTRecord(sqlCmd);
                }

                sqlCmd = "INSERT INTO dbo.MT320_SequenceI (reference_id, number_of_repetitions_18a, delevery_agent_id_53a, delevery_agent_code_53a, delevery_agent_id_53d, ";
                sqlCmd += "delevery_agent_addr_53d, delevery_agent_id_53j, intermediary_2_id_86a, intermediary_2_code_86a, intermediary_2_id_86d, intermediary_2_addr_86d, ";
                sqlCmd += "intermediary_2_id_86j, intermediary_id_56a, intermediary_code_56a, intermediary_id_56d, intermediary_addr_56d, intermediary_id_56j, ";
                sqlCmd += "receiving_agent_id_57a, receiving_agent_code_57a, receiving_agent_id_57d, receiving_agent_addr_57d, receiving_agent_id_57j)";
                sqlCmd += "VALUES ('" + refid + "', '" +
                                    getT18A_repetitions(sequenceI) + "', '" +
                                    getT53A_ID(sequenceI) + "', '" +
                                    getT53A_Code(sequenceI) + "', '" +
                                    getT53D_ID(sequenceI) + "', '" +
                                    getT53D_NameAddr(sequenceI) + "', '" +
                                    getT53J_ID(sequenceI) + "', '" +
                                    getT86A_ID(sequenceI) + "', '" +
                                    getT86A_Code(sequenceI) + "', '" +
                                    getT86D_ID(sequenceI) + "', '" +
                                    getT86D_NameAddr(sequenceI) + "', '" +
                                    getT86J_ID(sequenceI) + "', '" +
                                    getT56A_ID(sequenceI) + "', '" +
                                    getT56A_Code(sequenceI) + "', '" +
                                    getT56D_ID(sequenceI) + "', '" +
                                    getT56D_NameAddr(sequenceI) + "', '" +
                                    getT56J_ID(sequenceI) + "', '" +
                                    getT57A_ID(sequenceI) + "', '" +
                                    getT57A_Code(sequenceI) + "', '" +
                                    getT57D_ID(sequenceI) + "', '" +
                                    getT57D_NameAddr(sequenceI) + "', '" +
                                    getT57J_ID(sequenceI) + "')";
                dbu.saveMTRecord(sqlCmd);

                List<string> date = new List<string>();
                List<string> ccy = new List<string>();
                List<Nullable<double>> amt = new List<Nullable<double>>();
                getT30F32H_AdditionalAmounts(sequenceI, out date, out ccy, out amt);

                for (int i = 0; i < date.Count(); i++)
                {
                    sqlCmd = "INSERT INTO dbo.MT320_AdditionalAmounts (reference_id, payment_date_30F, currency_32h, payment_amount_32h)";
                    sqlCmd += "VALUES ('" + refid + "', '" +
                                        date[i] + "', '" +
                                        ccy[i] + "', '" +
                                        amt[i] + "')";
                    dbu.saveMTRecord(sqlCmd);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to insert MT320 Block4 record.\n" + ex.Message);
            }
        }

        private void saveBlock5(long refid, BlockHeader hdr)
        {
            string sqlCmd = null;

            if (hdr == null)
                return;

            try
            {
                sqlCmd = "INSERT INTO dbo.MT320_Block5 (reference_id, checksum, tng_message, pde, pde_time, pde_mir, pde_mir_date, pde_mir_lt_id, ";
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
                throw new Exception("Failed to insert MT320 Block5 record.\n" + ex.Message);
            }
        }
        #endregion


        public void testFunctions()
        {
            string ss = null;
            Nullable<double> dd = 0.0;
            Nullable<int> ii = 0;

            ii = getT18A_repetitions(sequenceI);
            ss = getT24D_DealingMethod(sequenceH);
            dd = getT32B_Amount(sequenceB);
            ss = getT32H_Currency(sequenceB);
            dd = getT32H_Amount(sequenceB);
            ss = getT34E_Currency(sequenceB);
            dd = getT34E_Amount(sequenceB);

            dd = getT37G_InterestRate(sequenceB);
            ss = getT38J_Indicator(sequenceB);
            ii = getT38J_Number(sequenceB);

            List<string> date = new List<string> ();
            List<string> ccy = new List<string>();
            List<Nullable<double>> amt = new List<Nullable<double>>();
            getT30F32H_AdditionalAmounts(sequenceI, out date, out ccy, out amt);

            if (date.Count() != ccy.Count())
                ss = "Oops!";
        }
    }
}


/**************************** method needed ************************************
 * 
 * 1. validate message format
 * 2. generate xml message
 * 3. get xml message
 * 4. complete pipe delimited reader
 * 5. generate pipe delimited message
 * 
 ***/
