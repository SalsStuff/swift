using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace Messages
{
    // https://www2.swift.com/knowledgecentre/publications/us3ma_20190719/1.0?topic=mt320.htm
    public class MT320
    {
        #region SEQUENCE_VARIABLES
        // Sequence A - Mandatory
        // Sequence A General Information contains general information about the fixed loan/deposit as well as about the confirmation itself.
        List<TagData<string, string, string, string, int>> sequenceA = new List<TagData<string, string, string, string, int>>
        {
            // Tag, Name, Value, Mandatory
            new TagData<string, string, string, string, int>("15A", "New Sequence",              "" ,"M", 0),
            new TagData<string, string, string, string, int>("20",  "Sender's Reference",        "" ,"M", 0),
            new TagData<string, string, string, string, int>("21",  "Related Reference",         "" ,"O", 0),
            new TagData<string, string, string, string, int>("21N", "Contract Number Party A",   "" ,"O", 0),
            new TagData<string, string, string, string, int>("22A", "Type of Operation",         "", "M", 0),
            new TagData<string, string, string, string, int>("22B", "Type of Event",             "", "M", 0),
            new TagData<string, string, string, string, int>("22C", "Common Reference",          "", "M", 0),
            new TagData<string, string, string, string, int>("77D", "Terms and Conditions",      "", "O", 0),
            new TagData<string, string, string, string, int>("82A", "Party A",                   "", "M", 0),
            new TagData<string, string, string, string, int>("82D", "Party A",                   "", "M", 0),
            new TagData<string, string, string, string, int>("82J", "Party A",                   "", "M", 0),
            new TagData<string, string, string, string, int>("83A", "Fund or Instructing Party", "", "O", 0),
            new TagData<string, string, string, string, int>("83D", "Fund or Instructing Party", "", "O", 0),
            new TagData<string, string, string, string, int>("83J", "Fund or Instructing Party", "", "O", 0),
            new TagData<string, string, string, string, int>("87A", "Party B",                   "", "M", 0),
            new TagData<string, string, string, string, int>("87D", "Party B",                   "", "M", 0),
            new TagData<string, string, string, string, int>("87J", "Party B",                   "", "M", 0),
            new TagData<string, string, string, string, int>("94A", "Scope of Operation",        "", "O", 0)
        };

        // Sequence B - Mandatory
        // Sequence B Transaction Details contains information about the transaction.
        List<TagData<string, string, string, string, int>> sequenceB = new List<TagData<string, string, string, string, int>>
        {
            // Tag, Name, Value, Mandatory
            new TagData<string, string, string, string, int>("14D", "Day Count Fraction"                   , "" ,"M", 0),
            new TagData<string, string, string, string, int>("15B", "New Sequence"                         , "" ,"M", 0),
            new TagData<string, string, string, string, int>("17R", "Party A's Role"                       , "" ,"M", 0),
            new TagData<string, string, string, string, int>("30F", "Last Day of the First Interest Period", "" ,"O", 0),
            new TagData<string, string, string, string, int>("30P", "Maturity Date"                        , "", "M", 0),
            new TagData<string, string, string, string, int>("30T", "Trade Date"                           , "", "M", 0),
            new TagData<string, string, string, string, int>("30V", "Value Date"                           , "", "M", 0),
            new TagData<string, string, string, string, int>("30X", "Next Interest Due Date"               , "", "O", 0),
            new TagData<string, string, string, string, int>("32B", "Currency and Principal Amount"        , "", "M", 0),
            new TagData<string, string, string, string, int>("32H", "Amount to be Settled"                 , "", "O", 0),
            new TagData<string, string, string, string, int>("34E", "Currenct and Interest Amount"         , "", "M", 0),
            new TagData<string, string, string, string, int>("37G", "Interest Rate"                        , "", "M", 0),
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
            new TagData<string, string, string, string, int>("56A", "Intermediary"           , "", "O", 0),
            new TagData<string, string, string, string, int>("56D", "Intermediary"           , "", "O", 0),
            new TagData<string, string, string, string, int>("56J", "Intermediary"           , "", "O", 0),
            new TagData<string, string, string, string, int>("57A", "Receiving Agent"        , "", "M", 0),
            new TagData<string, string, string, string, int>("57D", "Receiving Agent"        , "", "M", 0),
            new TagData<string, string, string, string, int>("57J", "Receiving Agent"        , "", "M", 0),
            new TagData<string, string, string, string, int>("58A", "Beneficiary Institution", "", "O", 0),
            new TagData<string, string, string, string, int>("58D", "Beneficiary Institution", "", "O", 0),
            new TagData<string, string, string, string, int>("58J", "Beneficiary Institution", "", "O", 0),
            new TagData<string, string, string, string, int>("86A", "Intermediary 2"         , "", "O", 0),
            new TagData<string, string, string, string, int>("86D", "Intermediary 2"         , "", "O", 0),
            new TagData<string, string, string, string, int>("86J", "Intermediary 2"         , "", "O", 0)
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
            new TagData<string, string, string, string, int>("56A", "Intermediary"           , "", "O", 0),
            new TagData<string, string, string, string, int>("56D", "Intermediary"           , "", "O", 0),
            new TagData<string, string, string, string, int>("56J", "Intermediary"           , "", "O", 0),
            new TagData<string, string, string, string, int>("57A", "Receiving Agent"        , "", "M", 0),
            new TagData<string, string, string, string, int>("57D", "Receiving Agent"        , "", "M", 0),
            new TagData<string, string, string, string, int>("57J", "Receiving Agent"        , "", "M", 0),
            new TagData<string, string, string, string, int>("58A", "Beneficiary Institution", "", "O", 0),
            new TagData<string, string, string, string, int>("58D", "Beneficiary Institution", "", "O", 0),
            new TagData<string, string, string, string, int>("58J", "Beneficiary Institution", "", "O", 0),
            new TagData<string, string, string, string, int>("86A", "Intermediary 2"         , "", "O", 0),
            new TagData<string, string, string, string, int>("86D", "Intermediary 2"         , "", "O", 0),
            new TagData<string, string, string, string, int>("86J", "Intermediary 2"         , "", "O", 0)
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
            new TagData<string, string, string, string, int>("56A", "Intermediary"           , "", "O", 0),
            new TagData<string, string, string, string, int>("56D", "Intermediary"           , "", "O", 0),
            new TagData<string, string, string, string, int>("56J", "Intermediary"           , "", "O", 0),
            new TagData<string, string, string, string, int>("57A", "Receiving Agent"        , "", "M", 0),
            new TagData<string, string, string, string, int>("57D", "Receiving Agent"        , "", "M", 0),
            new TagData<string, string, string, string, int>("57J", "Receiving Agent"        , "", "M", 0),
            new TagData<string, string, string, string, int>("58A", "Beneficiary Institution", "", "O", 0),
            new TagData<string, string, string, string, int>("58D", "Beneficiary Institution", "", "O", 0),
            new TagData<string, string, string, string, int>("58J", "Beneficiary Institution", "", "O", 0),
            new TagData<string, string, string, string, int>("86A", "Intermediary 2"         , "", "O", 0),
            new TagData<string, string, string, string, int>("86D", "Intermediary 2"         , "", "O", 0),
            new TagData<string, string, string, string, int>("86J", "Intermediary 2"         , "", "O", 0)
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
            new TagData<string, string, string, string, int>("56A", "Intermediary"           , "", "O", 0),
            new TagData<string, string, string, string, int>("56D", "Intermediary"           , "", "O", 0),
            new TagData<string, string, string, string, int>("56J", "Intermediary"           , "", "O", 0),
            new TagData<string, string, string, string, int>("57A", "Receiving Agent"        , "", "M", 0),
            new TagData<string, string, string, string, int>("57D", "Receiving Agent"        , "", "M", 0),
            new TagData<string, string, string, string, int>("57J", "Receiving Agent"        , "", "M", 0),
            new TagData<string, string, string, string, int>("58A", "Beneficiary Institution", "", "O", 0),
            new TagData<string, string, string, string, int>("58D", "Beneficiary Institution", "", "O", 0),
            new TagData<string, string, string, string, int>("58J", "Beneficiary Institution", "", "O", 0),
            new TagData<string, string, string, string, int>("86A", "Intermediary 2"         , "", "O", 0),
            new TagData<string, string, string, string, int>("86D", "Intermediary 2"         , "", "O", 0),
            new TagData<string, string, string, string, int>("86J", "Intermediary 2"         , "", "O", 0)
        };

        // Sequence G - Optional
        // Sequence G Tax Information contains information about the tax regime.
        List<TagData<string, string, string, string, int>> sequenceG = new List<TagData<string, string, string, string, int>>
        {
            // Tag, Name, Value, Mandatory
            new TagData<string, string, string, string, int>("15G", "New Sequence"                                , "" ,"M", 0),
            new TagData<string, string, string, string, int>("33B", "Transaction Currency and Net Interest Amount", "" ,"M", 0),
            new TagData<string, string, string, string, int>("33E", "Reporting Currency and Tax Amount"           , "" ,"O", 0),
            new TagData<string, string, string, string, int>("36",  "Exchange Rate"                               , "" ,"O", 0),
            new TagData<string, string, string, string, int>("37L", "Tax Rate"                                    , "", "M", 0)
        };

        // Sequence H - Optional
        // Sequence H Additional Information provides information, which is not match-critical.
        List<TagData<string, string, string, string, int>> sequenceH = new List<TagData<string, string, string, string, int>>
        {
            // Tag, Name, Value, Mandatory
            new TagData<string, string, string, string, int>("15H", "New Sequence"                  , "" ,"M", 0),
            new TagData<string, string, string, string, int>("21G", "Broker's Reference"            , "" ,"O", 0),
            new TagData<string, string, string, string, int>("24D", "Dealing Method"                , "" ,"O", 0),
            new TagData<string, string, string, string, int>("26H", "Counterparty's Reference"      , "" ,"O", 0),
            new TagData<string, string, string, string, int>("29A", "Contract Information"          , "", "O", 0),
            new TagData<string, string, string, string, int>("34C", "Commission and Fees"           , "", "O", 0),  // repeating
            new TagData<string, string, string, string, int>("71F", "Broker's Commission"           , "", "O", 0),
            new TagData<string, string, string, string, int>("72",  "Sender to Receiver Information", "", "O", 0),
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
            new TagData<string, string, string, string, int>("88J", "Broker Identification"         , "", "O", 0)
        };

        // Sequence I - Optional
        // Sequence I Additional Amounts provides information on additional fees
        List<TagData<string, string, string, string, int>> sequenceI = new List<TagData<string, string, string, string, int>>
        {
            // Tag, Name, Value, Mandatory
            new TagData<string, string, string, string, int>("15I", "New Sequence"            , "" ,"M", 0),
            new TagData<string, string, string, string, int>("18A", "Number of Repetitions"   , "" ,"M", 0),
            new TagData<string, string, string, string, int>("30F", "Payment Date"            , "" ,"M", 0),  //   - Repeating fields
            new TagData<string, string, string, string, int>("32H", "Currency, Payment Amount", "" ,"M", 0),  //   /
            new TagData<string, string, string, string, int>("53A", "Delivery Agent"          , "", "O", 0),
            new TagData<string, string, string, string, int>("53D", "Delivery Agent"          , "", "O", 0),
            new TagData<string, string, string, string, int>("53J", "Delivery Agent"          , "", "O", 0),
            new TagData<string, string, string, string, int>("56A", "Intermediary"            , "", "O", 0),
            new TagData<string, string, string, string, int>("56D", "Intermediary"            , "", "O", 0),
            new TagData<string, string, string, string, int>("56J", "Intermediary"            , "", "O", 0),
            new TagData<string, string, string, string, int>("57A", "Receiving Agent"         , "", "M", 0),
            new TagData<string, string, string, string, int>("57D", "Receiving Agent"         , "", "M", 0),
            new TagData<string, string, string, string, int>("57J", "Receiving Agent"         , "", "M", 0),
            new TagData<string, string, string, string, int>("86A", "Intermediary 2"          , "", "O", 0),
            new TagData<string, string, string, string, int>("86D", "Intermediary 2"          , "", "O", 0),
            new TagData<string, string, string, string, int>("86J", "Intermediary 2"          , "", "O", 0)
        };
        #endregion

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

        private void InitializeMT320()
        {
            ResetVariables();
            DefineScope();
            Anomalies.Clear();
        }

        /// <summary>
        /// Definition of Message Scope
        /// This is the SWIFT definition of the message
        /// It can be found at https://www2.swift.com/knowledgecentre/publications/us3ma_20190719/1.0?topic=mt320-scope.htm
        /// </summary>
        private void DefineScope()
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
        private void ResetVariables()
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
            string input = message.Substring(3);
            // Clean up message
            input = input.Replace(System.Environment.NewLine, "");
            input = input.Replace("\r\n", "");
            string test = input.Substring(0, 1);
            if (test.Equals(":") == true)
                input = input.Substring(1);
            
            // Split the tags and values up
            string[] result = input.Split(':');
            result = result.Where((s, i) => i % 2 == 0)
                        .Zip(result.Where((s, i) => i % 2 == 1), (a, b) => a + ", " + b)
                        .ToArray();
            
            FillDataTags(result);

            IsMessageValid();
        }

        /// <summary>
        /// Fill in the class variables with the SWIFT message data
        /// </summary>
        /// <param name="tags"></param>
        private void FillDataTags(string[] tags)
        {
            string useSequence = "";
            
            foreach (string key in tags)
            {
                string[] keyPair = key.Split(',');
                if (keyPair[0].Contains("15") == true)
                {
                    useSequence = keyPair[0].Substring(2);
                    SetTagPresent(useSequence, keyPair[0], 1);
                }
                else
                {
                    SetTagValue(useSequence, keyPair[0], keyPair[1]);
                    SetTagPresent(useSequence, keyPair[0], 1);
                }
            }
        }

        /// <summary>
        /// Parse a pipe delimited SWIFT message
        /// </summary>
        /// <param name="message"></param>
        private void ParsePipeMsg(string message)
        {
        
        }

        #region GET SET Functions
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

        /// <summary>
        /// Get method to return the number of class/message data sequence
        /// </summary>
        public int numOfSequences { get; } = 9;

        /// <summary>
        /// Get method to return the SWIFT definition of message scope
        /// </summary>
        public string Scope { get; private set; } = "";

        /// <summary>
        /// Get method to read back any errors or warnings set during the parsing of the message
        /// </summary>
        public List<string> Anomalies { get; } = new List<string>();

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

        /// <summary>
        /// Get method the return the readable name of a SWIFT tag number
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string GetTagName(string sequence, string tag)
        {
            string tagName = null;
        
            switch(sequence)
            {
                case "A":
                    foreach(TagData<string, string, string, string, int> t in sequenceA)
                    {
                        if(tag.Equals(t.Tag) ==  true)
                        {
                            tagName = t.Name;
                            break;
                        }
                    }
                    break;
                case "B":
                    foreach (TagData<string, string, string, string, int> t in sequenceB)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            tagName = t.Name;
                            break;
                        }
                    }
                    break;
                case "C":
                    foreach (TagData<string, string, string, string, int> t in sequenceC)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            tagName = t.Name;
                            break;
                        }
                    }
                    break;
                case "D":
                    foreach (TagData<string, string, string, string, int> t in sequenceD)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            tagName = t.Name;
                            break;
                        }
                    }
                    break;
                case "E":
                    foreach (TagData<string, string, string, string, int> t in sequenceE)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            tagName = t.Name;
                            break;
                        }
                    }
                    break;
                case "F":
                    foreach (TagData<string, string, string, string, int> t in sequenceF)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            tagName = t.Name;
                            break;
                        }
                    }
                    break;
                case "G":
                    foreach (TagData<string, string, string, string, int> t in sequenceG)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            tagName = t.Name;
                            break;
                        }
                    }
                    break;
                case "H":
                    foreach (TagData<string, string, string, string, int> t in sequenceH)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            tagName = t.Name;
                            break;
                        }
                    }
                    break;
                case "I":
                    foreach (TagData<string, string, string, string, int> t in sequenceI)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            tagName = t.Name;
                            break;
                        }
                    }
                    break;
                default:
                    tagName = null;
                    break;
            }
        
            return tagName;
        }

        /// <summary>
        /// Get method to return the value of a tag
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string GetTagValue(string sequence, string tag)
        {
            string tagValue = null;
            
            switch (sequence)
            {
                case "A":
                    foreach (TagData<string, string, string, string, int> t in sequenceA)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            tagValue = t.Value;
                            break;
                        }
                    }
                    break;
                case "B":
                    foreach (TagData<string, string, string, string, int> t in sequenceB)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            tagValue = t.Value;
                            break;
                        }
                    }
                    break;
                case "C":
                    foreach (TagData<string, string, string, string, int> t in sequenceC)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            tagValue = t.Value;
                            break;
                        }
                    }
                    break;
                case "D":
                    foreach (TagData<string, string, string, string, int> t in sequenceD)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            tagValue = t.Value;
                            break;
                        }
                    }
                    break;
                case "E":
                    foreach (TagData<string, string, string, string, int> t in sequenceE)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            tagValue = t.Value;
                            break;
                        }
                    }
                    break;
                case "F":
                    foreach (TagData<string, string, string, string, int> t in sequenceF)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            tagValue = t.Value;
                            break;
                        }
                    }
                    break;
                case "G":
                    foreach (TagData<string, string, string, string, int> t in sequenceG)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            tagValue = t.Value;
                            break;
                        }
                    }
                    break;
                case "H":
                    foreach (TagData<string, string, string, string, int> t in sequenceH)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            tagValue = t.Value;
                            break;
                        }
                    }
                    break;
                case "I":
                    foreach (TagData<string, string, string, string, int> t in sequenceI)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            tagValue = t.Value;
                            break;
                        }
                    }
                    break;
                default:
                    tagValue = null;
                    break;
            }
            
            return tagValue;
        }

        /// <summary>
        /// Set method to set a tag value
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        public void SetTagValue(string sequence, string tag, string value)
        {
            switch (sequence)
            {
                case "A":
                    foreach (TagData<string, string, string, string, int> t in sequenceA)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            t.Value = value;
                            break;
                        }
                    }
                    break;
                case "B":
                    foreach (TagData<string, string, string, string, int> t in sequenceB)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            t.Value = value;
                            break;
                        }
                    }
                    break;
                case "C":
                    foreach (TagData<string, string, string, string, int> t in sequenceC)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            t.Value = value;
                            break;
                        }
                    }
                    break;
                case "D":
                    foreach (TagData<string, string, string, string, int> t in sequenceD)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            t.Value = value;
                            break;
                        }
                    }
                    break;
                case "E":
                    foreach (TagData<string, string, string, string, int> t in sequenceE)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            t.Value = value;
                            break;
                        }
                    }
                    break;
                case "F":
                    foreach (TagData<string, string, string, string, int> t in sequenceF)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            t.Value = value;
                            break;
                        }
                    }
                    break;
                case "G":
                    foreach (TagData<string, string, string, string, int> t in sequenceG)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            t.Value = value;
                            break;
                        }
                    }
                    break;
                case "H":
                    foreach (TagData<string, string, string, string, int> t in sequenceH)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            t.Value = value;
                            break;
                        }
                    }
                    break;
                case "I":
                    foreach (TagData<string, string, string, string, int> t in sequenceI)
                    {
                        if (tag.Equals(t.Tag) == true)
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

        /// <summary>
        /// Set method to set the present flag of a tag
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="tag"></param>
        /// <param name="present"></param>
        private void SetTagPresent(string sequence, string tag, int present)
        {
            switch (sequence)
            {
                case "A":
                    foreach (TagData<string, string, string, string, int> t in sequenceA)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            t.Present = present;
                            break;
                        }
                    }
                    break;
                case "B":
                    foreach (TagData<string, string, string, string, int> t in sequenceB)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            t.Present = present;
                            break;
                        }
                    }
                    break;
                case "C":
                    foreach (TagData<string, string, string, string, int> t in sequenceC)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            t.Present = present;
                            break;
                        }
                    }
                    break;
                case "D":
                    foreach (TagData<string, string, string, string, int> t in sequenceD)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            t.Present = present;
                            break;
                        }
                    }
                    break;
                case "E":
                    foreach (TagData<string, string, string, string, int> t in sequenceE)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            t.Present = present;
                            break;
                        }
                    }
                    break;
                case "F":
                    foreach (TagData<string, string, string, string, int> t in sequenceF)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            t.Present = present;
                            break;
                        }
                    }
                    break;
                case "G":
                    foreach (TagData<string, string, string, string, int> t in sequenceG)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            t.Present = present;
                            break;
                        }
                    }
                    break;
                case "H":
                    foreach (TagData<string, string, string, string, int> t in sequenceH)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            t.Present = present;
                            break;
                        }
                    }
                    break;
                case "I":
                    foreach (TagData<string, string, string, string, int> t in sequenceI)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            t.Present = present;
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Get method to determine if a tag is mandatory
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public bool IsTagMandatory(string sequence, string tag)
        {
            bool isMandatory = false;
            
            switch (sequence)
            {
                case "A":
                    foreach (TagData<string, string, string, string, int> t in sequenceA)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            if (string.Equals(t.Mandatory, "M") == true)
                            {
                                isMandatory = true;
                                break;
                            }
                        }
                    }
                    break;
                case "B":
                    foreach (TagData<string, string, string, string, int> t in sequenceB)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            if (string.Equals(t.Mandatory, "M") == true)
                            {
                                isMandatory = true;
                                break;
                            }
                        }
                    }
                    break;
                case "C":
                    foreach (TagData<string, string, string, string, int> t in sequenceC)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            if (string.Equals(t.Mandatory, "M") == true)
                            {
                                isMandatory = true;
                                break;
                            }
                        }
                    }
                    break;
                case "D":
                    foreach (TagData<string, string, string, string, int> t in sequenceD)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            if (string.Equals(t.Mandatory, "M") == true)
                            {
                                isMandatory = true;
                                break;
                            }
                        }
                    }
                    break;
                case "E":
                    foreach (TagData<string, string, string, string, int> t in sequenceE)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            if (string.Equals(t.Mandatory, "M") == true)
                            {
                                isMandatory = true;
                                break;
                            }
                        }
                    }
                    break;
                case "F":
                    foreach (TagData<string, string, string, string, int> t in sequenceF)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            if (string.Equals(t.Mandatory, "M") == true)
                            {
                                isMandatory = true;
                                break;
                            }
                        }
                    }
                    break;
                case "G":
                    foreach (TagData<string, string, string, string, int> t in sequenceG)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            if (string.Equals(t.Mandatory, "M") == true)
                            {
                                isMandatory = true;
                                break;
                            }
                        }
                    }
                    break;
                case "H":
                    foreach (TagData<string, string, string, string, int> t in sequenceH)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            if (string.Equals(t.Mandatory, "M") == true)
                            {
                                isMandatory = true;
                                break;
                            }
                        }
                    }
                    break;
                case "I":
                    foreach (TagData<string, string, string, string, int> t in sequenceI)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            if (string.Equals(t.Mandatory, "M") == true)
                            {
                                isMandatory = true;
                                break;
                            }
                        }
                    }
                    break;
                default:
                    isMandatory = false;
                    break;
            }

            return isMandatory;
        }

        /// <summary>
        /// Get method to determine if a tag is mandatory, optional, conditional or unknown
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string GetTagStatus(string sequence, string tag)
        {
           string status = "U";

            switch (sequence)
            {
                case "A":
                    foreach (TagData<string, string, string, string, int> t in sequenceA)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            status = t.Mandatory;
                        }
                    }
                    break;
                case "B":
                    foreach (TagData<string, string, string, string, int> t in sequenceB)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            status = t.Mandatory;
                        }
                    }
                    break;
                case "C":
                    foreach (TagData<string, string, string, string, int> t in sequenceC)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            status = t.Mandatory;
                        }
                    }
                    break;
                case "D":
                    foreach (TagData<string, string, string, string, int> t in sequenceD)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            status = t.Mandatory;
                        }
                    }
                    break;
                case "E":
                    foreach (TagData<string, string, string, string, int> t in sequenceE)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            status = t.Mandatory;
                        }
                    }
                    break;
                case "F":
                    foreach (TagData<string, string, string, string, int> t in sequenceF)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            status = t.Mandatory;
                        }
                    }
                    break;
                case "G":
                    foreach (TagData<string, string, string, string, int> t in sequenceG)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            status = t.Mandatory;
                        }
                    }
                    break;
                case "H":
                    foreach (TagData<string, string, string, string, int> t in sequenceH)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            status = t.Mandatory;
                        }
                    }
                    break;
                case "I":
                    foreach (TagData<string, string, string, string, int> t in sequenceI)
                    {
                        if (tag.Equals(t.Tag) == true)
                        {
                            status = t.Mandatory;
                        }
                    }
                    break;
                default:
                    break;
            }

            return status;
        }

        public string GeDetailtXML()
        {
            using (var sw = new StringWriter())
            {
                using (var xw = XmlWriter.Create(sw))
                {
                    // Build Xml with xw.
        
        
                }

                return sw.ToString();
            }
        }
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
                            if (Is_T77D_Valid(field) == false)
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
                            if (Is_T30F_Valid(field) == false)
                                validTag = false;
                            break;
                        case "30P":
                            if (Is_T30P_Valid(field) == false)
                                validTag = false;
                            break;
                        case "30T":
                            if (Is_T30T_Valid(field) == false)
                                validTag = false;
                            break;
                        case "30V":
                            if (Is_T30V_Valid(field) == false)
                                validTag = false;
                            break;
                        case "30X":
                            if (Is_T30X_Valid(field) == false)
                                validTag = false;
                            break;
                        case "32B":
                            if (Is_T32B_Valid(field) == false)
                                validTag = false;
                            break;
                            /*
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
                            */
                        default:
                            Anomalies.Add("ERROR : Unknown tag " + field.Tag + " in sequence A - can not validate.");
                            break;
                    }
                    break;
                case "C":
                    break;
                case "D":
                    break;
                case "E":
                    break;
                case "F":
                    break;
                case "G":
                    break;
                case "H":
                    break;
                case "I":
                    break;
                default:
                    break;
            }

            return validTag;
        }

        #region FIELD VALIDATIONS
        #region SEQUENCE A TAG VALIDATIONS
        /// <summary>
        /// Is_T15A_Valid
        /// Format
        ///     This is an empty field
        /// Presence
        ///     Mandatory in mandatory sequence A
        /// Definition    
        ///     This field specifies the start of mandatory sequence A General Information.
        /// Usage Rules
        ///     Only the field tag must be present, the field is empty.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool Is_T15A_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            // 15A is a mandatory field in a mandatory block. It must be present
            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("15A") == true)
                {
                    if (field.Present == 0)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                    }
                    if (field.Value.Equals("") == false)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + " - Only the field tag must be present, the field must be empty. It contains : " + field.Value);
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T15A_Valid");
                }
            }
            else
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T20_Valid
        /// Format
        ///     16x
        /// Presence
        ///     Mandatory in mandatory sequence A
        /// Definition
        ///     This field specifies the reference assigned by the Sender to unambiguously identify the message.
        /// Network Validated Rules    
        ///     This field must not start or end with a slash '/' and must not contain two consecutive slashes '//' (Error code(s): T26).
        /// Usage Rules
        ///     The reference assigned to a message is used for cross-referencing purposes in subsequent messages, such as following 
        ///     confirmation and statement messages as well as queries. It is therefore essential for the identification of the 
        ///     original message sent that the reference be unique.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool Is_T20_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 20 is a mandatory field in a mandatory block. It must be present
                if (field.Tag.Equals("20") == true)
                {
                    if (field.Present == 0)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                    }
                    field.Value = field.Value.Trim();
                    if(field.Value.Length > 16)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 16 characters.");
                    }
                    if (field.Value.Substring(0,1).Equals("/") == true)
                    {
                        valid = false;
                        Anomalies.Add("ERROR T26 - Tag " + field.Tag + "," + field.Name + ", starts with a '/'");
                    }
                    if (field.Value.Substring(field.Value.Length-1, 1).Equals("/") == true)
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
        ///     Conditional (see rule C1) in mandatory sequence A
        /// Definition
        ///     This field contains the identification of the message to which the current message is related.
        /// Network Validated Rules
        ///     This field must not start or end with a slash '/' and must not contain two consecutive slashes '//' (Error code(s): T26).
        /// Usage Rules
        ///     When used, this field must contain the Sender's reference (field 20) of the previous confirmation which is to be amended, 
        ///     cancelled, duplicated or the reference of the message to which the rollover or maturity applies.
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
            else
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T22A_Valid
        /// Format
        ///     Option A:  4!c
        /// Presence
        ///     Mandatory (referenced in rule C1) in mandatory sequence A
        /// Definition
        ///     This field specifies the function of the message.
        /// Codes
        ///     Function must contain one of the following codes (Error code(s): T36):
        ///     AMND        Amendment           Replaces a previously sent message with the amended one contained in this message.
        ///                                     Used to add settlement agents to an MT 320 previously sent or to correct error(s) in an MT 320 previously sent.
        ///     CANC        Cancellation        Cancels a previously sent message. 
        ///                                     Used to cancel an MT 320 previously sent or to cancel an MT 320 which contains erroneous information.
        ///     DUPL        Duplicate           Duplicates an already sent confirmation.
        ///     NEWT        New Confirmation    Used to send the MT 320 for the first time or to send a corrected MT 320 when the erroneous one was cancelled using an MT 320 with function CANC.
        /// Usage Rules
        ///     As the amend message replaces the previously sent confirmation, it must contain both the amended fields and the fields which are not changed.
        ///     When the cancel function is used, the message must reconfirm at least the mandatory fields of the original transaction.
        ///     See further guidelines under field 22B, Type of Event.
        ///     An amendment or cancellation always refers to the previous confirmation identified in field 21 of this message.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool Is_T22A_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            Util util = new Util();

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 22A is a mandatory field.
                if (field.Tag.Equals("22A") == true)
                {
                    if (field.Present == 0)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                    }
                    field.Value = field.Value.Trim();
                    if (util.IsAllUpper(field.Value) == false)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", must be alpha-numberic with using only capital letters");
                    }

                    if (field.Value.Replace(" ", "").Length != 4)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is not 4 characters long.");
                    }
                    if ( (field.Value.Contains("AMND") == true) || (field.Value.Contains("CANC") == true) ||
                         (field.Value.Contains("DUPL") == true) || (field.Value.Contains("NEWT") == true) )
                    {
                        // Do nothing for now.
                    }
                    else
                    {
                        valid = false;
                        Anomalies.Add("ERROR T36 - Tag " + field.Tag + "," + field.Name + ", field must contain one of the following: AMND, CANC, DUPL or NEWT");
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T22A_Valid");
                }
            }
            else
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T94A_Valid
        /// Format 
        ///     Option A:  4!C
        /// Presence
        ///     Optional (referenced in rules C2 and C8) in mandatory sequence A
        /// Definition
        ///     This field specifies the role of the Sender and the Receiver of the message in the conclusion of the confirmed trade.
        /// Codes
        ///     Scope must contain one of the following codes (Error code(s): T36 ):
        ///     AGNT            Sender/Receiver is sending/receiving the message on behalf of a third party.
        ///     BILA            Bilateral confirmation, that is, Sender and Receiver are the principals.
        ///     BROK            Confirmation is sent by a money broker.
        /// Usage Rules
        ///     The absence of this field means that the Sender and the Receiver are the principals.
        ///     AGNT is used when the confirmation has been sent or received on behalf of a separate legal party and that party has done the deal.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool Is_T94A_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            Util util = new Util();

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 94A is NOT a mandatory field.
                if (field.Tag.Equals("94A") == true)
                {
                    if (field.Present == 1)
                    {
                        if (util.IsAllUpper(field.Value) == false)
                        {
                            valid = false;
                            Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", must be alpha-numberic with using only capital letters");
                        }
                        field.Value = field.Value.Trim();
                        if (field.Value.Length != 4)
                        {
                            valid = false;
                            Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is not 4 characters long.");
                        }
                        if ((field.Value.Contains("AGNT") == true) || (field.Value.Contains("BILA") == true) || (field.Value.Contains("BROK") == true))
                        {
                            // Do nothing for now.
                        }
                        else
                        {
                            valid = false;
                            Anomalies.Add("ERROR T36 - Tag " + field.Tag + "," + field.Name + ", field must contain one of the following: AGNT, BILA or BROK");
                        }
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T94A_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T22B_Valid
        /// Format
        ///     Option B:  4!c
        /// Presence
        ///     Mandatory (referenced in rules C1, C3, C4, and C5) in mandatory sequence A
        /// Definition
        ///     This field specifies the event in the life of the loan/deposit.
        /// Codes
        ///     Type must contain one of the following codes (Error code(s): T93 ):
        ///     CONF            This is the first confirmation.
        ///     MATU            This is a confirmation of the liquidation of a fixed loan/ deposit.
        ///     ROLL            This is a confirmation of a mutually agreed rollover / renewal with / without change in the 
        ///                     principal amount and the interest to be settled or added / subtracted from that amount.
        /// Usage Rules
        ///     When the confirmation of a loan/deposit is sent for the first time CONF is used with field 22A = NEWT. 
        ///     To amend or cancel this confirmation CONF is still used and field 22A must contain either AMND or CANC.
        ///     
        ///     When the loan/deposit is renewed(rolled over) for the first time, ROLL is used with field 22A = NEWT.
        ///     To amend or cancel this rollover ROLL is still used and field 22A must contain either AMND or CANC.
        ///     
        ///     Any subsequent renewal must be confirmed the same way as the first rollover: ROLL is used with field 22A = NEWT.
        ///     To amend or cancel subsequent rollovers, ROLL is still used and field 22A must contain either AMND or CANC.
        ///     
        ///     When the loan/deposit is reaching its maturity without being rolled over, MATU is used with field 22A = NEWT.
        ///     To amend or cancel this maturity message, MATU is still used and field 22A must contain either AMND or CANC.
        ///     
        ///     As an overview: the right sequence of using the code words in the life cycle of the deal is:
        ///     1. Start of the loan/deposit:
        ///        NEWT/CONF(mandatory) followed by: AMND/CONF(optional) or CANC/CONF(optional)
        ///     2. Renewal of the loan/deposit(the following is repeated as often as needed during the life of the loan/deposit) :
        ///        NEWT/ROLL(mandatory) followed by: AMND/ROLL(optional) or CANC/ROLL(optional)
        ///     3. Termination of the loan/deposit:
        ///        NEWT/MATU(mandatory if maturity function is used) followed by: AMND/MATU(optional) or CANC/MATU(optional)
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool Is_T22B_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            Util util = new Util();

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 22B is a mandatory field.
                if (field.Tag.Equals("22B") == true)
                {
                    if (field.Present == 0)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                    }
                    field.Value = field.Value.Trim();
                    if (util.IsAllUpper(field.Value) == false)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", must be alpha-numberic with using only capital letters");
                    }

                    if (field.Value.Replace(" ", "").Length != 4)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is not 4 characters long.");
                    }
                    if ((field.Value.Contains("CONF") == true) || (field.Value.Contains("MATU") == true) || (field.Value.Contains("ROLL") == true))
                    {
                        // Do nothing for now.
                    }
                    else
                    {
                        valid = false;
                        Anomalies.Add("ERROR T36 - Tag " + field.Tag + "," + field.Name + ", field must contain one of the following: CONF, MATU or ROLL");
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T22B_Valid");
                }
            }
            else
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T22C_Valid
        /// Format
        ///     Option C:  4!a2!c4!n4!a2!c     (Length of 16)
        /// Presence
        ///     Mandatory in mandatory sequence A
        /// Definition
        ///     This field contains a reference common to both the Sender and the Receiver.
        /// Network Validated Rules
        ///     This field consists of two codes, a Sender's code and a Receiver's code, separated by a four-digit number.
        ///     The codes are made up from the party prefix and party suffix of the Sender's and Receiver's BICs, that is, their BIC without the country code(Error code(s): T95). 
        ///     These codes must appear in alphabetical order, with letters taking precedence over numbers(Error code(s): T96).
        ///     The four-digit number must consist of the rightmost non-zero digit of subfield Rate in field 37G in sequence B, preceded by the three digits to the left of it.
        ///     If there are no digits to the left of it, the space must be zero filled.The four digits must be '0000' if field 37G in sequence B has a value of zero
        ///     (Error code(s): T22).
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool Is_T22C_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            Util util = new Util();

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 22C is a mandatory field.
                if (field.Tag.Equals("22C") == true)
                {
                    if (field.Present == 0)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                    }
                    field.Value = field.Value.Trim();
                    if (util.IsAllUpper(field.Value) == false)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", must be alpha-numberic with using only capital letters");
                    }
                    if (field.Value.Replace(" ", "").Length != 16)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is not 16 characters long.");
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T22C_Valid");
                }
            }
            else
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T21N_Valid
        /// Format
        ///     Option N:  16x
        /// Presence
        ///     Conditional (see rule C2) in mandatory sequence A
        /// Definition
        ///     This field specifies the contract number of the transaction from party A's viewpoint.
        /// Usage Rules
        ///     This field must remain the same throughout the whole life of the transaction. It is used by party A to link the rollover or maturity to the original transaction.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool Is_T21N_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 21N is NOT a mandatory field.
                if (field.Tag.Equals("21N") == true)
                {
                    if (field.Present == 1)
                    {
                        field.Value = field.Value.Trim();
                        if (field.Value.Length != 16)
                        {
                            valid = false;
                            Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 16 characters.");
                        }
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T21N_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T82_Valid
        /// Format
        ///     Option A	    [/1!a] [/34x]           (Party Identifier)
        ///                     4!a2!a2!c[3!c]          (Identifier Code)
        ///     Option D        [/ 1!a][/ 34x]          (Party Identifier)
        ///                     4*35x                   (Name and Address)
        ///     Option J	    5*40x                   (Party Identification)
        /// Presence
        ///     Mandatory in mandatory sequence A
        /// Definition
        ///     This field identifies party A.
        /// Codes
        ///     In option J, Party Identification must be specified as a list of pairs (Code)(Value) and the following codes and format must be used (Error code(s): T78).
        ///     Note that optional codes are surrounded by square brackets('[' and ']') which are not part of the syntax.
        ///     The codes must be placed between slashes('/').
        ///     ABIC        4!a2!a2!c[3!c] or 4!a       Identifier Code or 'UKWN' if BIC not known
        ///     [ACCT]      34x                         Account number(optional)
        ///     [ADD1]      35x                         First line of the address(optional)
        ///     [ADD2]      35x                         Second line of the address(optional)
        ///     [CITY]      35x                         City, possibly followed by state and country(optional)
        ///     [CLRC]      35x                         Clearing code(optional)
        ///     [GBSC]      6!n                         UK domestic sort code(optional)
        ///     [LEIC]      18!c2!n                     Legal Entity Identifier(optional)
        ///     NAME        34x                         Party's name
        ///     [USCH]      6!n                         CHIPS UID(optional)
        ///     [USFW]      9!n                         FedWire Routing Number(optional)
        /// Network Validation Rules
        ///     Identifier Code must be a registered BIC (Error code(s): T27, T28, T29, T45).
        /// Usage Rules
        ///     For matching purposes, option A must be used when available.
        ///     Party A is either the sender :94A:BILA, or, the institution or corporate on whose behalf the message is sent :94A:AGNT, 
        ///     except when the Sender is a money broker :94A:BROK.
        ///     When the Sender is a fund manager, the fund manager is specified in this field.
        ///     See the chapter Scope for this MT.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool Is_T82_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 82A / 82D / 82J is a mandatory field.
                if ( (field.Tag.Equals("82A") == true) || (field.Tag.Equals("82D") == true) || (field.Tag.Equals("82J") == true) ) 
                {
                    if (field.Present == 0)
                    {
                            valid = false;
                            Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                    }
                    field.Value = field.Value.Trim();
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T82_Valid");
                }
            }
            else
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T87_Valid
        /// Format
        ///     Option A	    [/1!a] [/34x]           (Party Identifier)
        ///                     4!a2!a2!c[3!c]          (Identifier Code)
        ///     Option D        [/1!a][/34x]            (Party Identifier)
        ///                     4*35x                   (Name and Address)
        ///     Option J	    5*40x                   (Party Identification)
        /// Presence
        ///     Mandatory in mandatory sequence A
        /// Definition
        ///     This field identifies party B.
        /// Codes
        ///     In option J, Party Identification must be specified as a list of pairs (Code)(Value) and the following codes and format must be used (Error code(s): T78).
        ///     Note that optional codes are surrounded by square brackets('[' and ']') which are not part of the syntax.
        ///     The codes must be placed between slashes('/').
        ///     ABIC        4!a2!a2!c[3!c] or 4!a       Identifier Code or 'UKWN' if BIC not known
        ///     [ACCT]      34x                         Account number(optional)
        ///     [ADD1]      35x                         First line of the address(optional)
        ///     [ADD2]      35x                         Second line of the address(optional)
        ///     [CITY]      35x                         City, possibly followed by state and country(optional)
        ///     [CLRC]      35x                         Clearing code(optional)
        ///     [GBSC]      6!n                         UK domestic sort code(optional)
        ///     [LEIC]      18!c2!n                     Legal Entity Identifier(optional)
        ///     NAME        34x                         Party's name
        ///     [USCH]      6!n                         CHIPS UID(optional)
        ///     [USFW]      9!n                         FedWire Routing Number(optional)
        /// Network Validation Rules
        ///     Identifier Code must be a registered BIC (Error code(s): T27, T28, T29, T45).
        /// Usage Rules
        ///     For matching purposes, option A must be used when available.
        ///     Party B is either the sender :94A:BILA, or, the institution or corporate on whose behalf the message is sent :94A:AGNT, 
        ///     except when the Sender is a money broker :94A:BROK.
        ///     When the Sender is a fund manager, the fund manager is specified in this field.
        ///     See the chapter Scope for this MT.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool Is_T87_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 87A / 87D / 87J is a mandatory field.
                if ((field.Tag.Equals("87A") == true) || (field.Tag.Equals("87D") == true) || (field.Tag.Equals("87J") == true))
                {
                    if (field.Present == 0)
                    {
                            valid = false;
                            Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                    }
                    field.Value = field.Value.Trim();
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T87_Valid");
                }
            }
            else
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T83_Valid
        /// Format
        ///     Option A	    [/1!a] [/34x]           (Party Identifier)
        ///                     4!a2!a2!c[3!c]          (Identifier Code)
        ///     Option D        [/1!a][/34x]            (Party Identifier)
        ///                     4*35x                   (Name and Address)
        ///     Option J	    5*40x                   (Party Identification)
        /// Presence
        ///     Optional in mandatory sequence A
        /// Definition
        ///     This field identifies the account information for party A, the underlying fund or instructing institution.
        /// Codes
        ///     In option J, Party Identification must be specified as a list of pairs (Code)(Value) and the following codes and format must be used (Error code(s): T78).
        ///     Note that optional codes are surrounded by square brackets('[' and ']') which are not part of the syntax.
        ///     The codes must be placed between slashes('/').
        ///     ABIC        4!a2!a2!c[3!c] or 4!a       Identifier Code or 'UKWN' if BIC not known
        ///     [ACCT]      34x                         Account number(optional)
        ///     [ADD1]      35x                         First line of the address(optional)
        ///     [ADD2]      35x                         Second line of the address(optional)
        ///     [CITY]      35x                         City, possibly followed by state and country(optional)
        ///     [CLRC]      35x                         Clearing code(optional)
        ///     [GBSC]      6!n                         UK domestic sort code(optional)
        ///     [LEIC]      18!c2!n                     Legal Entity Identifier(optional)
        ///     NAME        34x                         Party's name
        ///     [USCH]      6!n                         CHIPS UID(optional)
        ///     [USFW]      9!n                         FedWire Routing Number(optional)
        /// Network Validation Rules
        ///     Identifier Code must be a registered BIC (Error code(s): T27, T28, T29, T45).
        /// Usage Rules
        ///     When the message is sent or received by a fund manager, this field specifies the fund. The fund manager is specified respectively in either field 82a or field 87a.
        ///     For matching purposes, option A must be used when available.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool Is_T83_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 83A / 83D / 83J is a mandatory field.
                if ((field.Tag.Equals("83A") == true) || (field.Tag.Equals("83D") == true) || (field.Tag.Equals("83J") == true))
                {
                    if (field.Present == 1)
                    {
                        // Add checks
                    }
                    field.Value = field.Value.Trim();
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T83_Valid");
                }
            }
            
            return valid;
        }

        /// <summary>
        /// Is_T77D_Valid
        /// Format
        ///     Option D:  6*35x    (Max length of 210)
        /// Presence
        ///     Optional in mandatory sequence A
        /// Definition
        ///     This field specifies the underlying legal agreement.
        /// Codes
        ///     Narrative may contain one of the following codes, placed between slashes ('/'):
        ///     FIDU                The trade is a fiduciary.
        ///     FLTR                The trade is a floating rate loan/deposit.
        ///     TBIL                The trade is the result of the issuance of a "Sterling Acceptance " or a "Treasury Bill" sent from the borrower to the lender 
        ///                         and confirmed by the lender. At maturity, the borrower will pay a pre-arranged amount back to the lender.
        ///     WITH                Withholding taxes apply.
        /// Usage Rules
        ///     This field may refer to master agreements; it may also refer to local regulations or specific conditions applicable to the trade.
        ///     If this field is not present, the deal conforms either to bilateral agreements or to usual banking practices.
        ///     The absence of the codes WITH and/or FIDU does not mean that withholding taxes do not apply or that the trade is the result of proprietary trading, 
        ///     it only means that the information is not relevant for the Receiver.
        ///     When the trade is a floating rate loan/deposit, the floating rate option follows the code FLTR. 
        ///     The opening confirmation contains a zero interest rate and interest amount.At maturity, the actual interest rate and amount are specified.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool Is_T77D_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 77D is NOT a mandatory field.
                if (field.Tag.Equals("77D") == true)
                {
                    if (field.Present == 1)
                    {
                        field.Value = field.Value.Trim();
                        if (field.Value.Length > 210)
                        {
                            valid = false;
                            Anomalies.Add("ERROR - Tag " + field.Tag + " value to long");
                        }
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T77D_Valid");
                }
            }
            
            return valid;
        }
        #endregion

        #region SEQUENCE B TAG VALIDATIONS
        /// <summary>
        /// Is_T14D_Valid
        /// Format
        ///     Option D :  7x
        /// Presence
        ///     Mandatory in mandatory sequence B
        /// Definition
        ///     This field specifies the number of days which are taken into account for the calculation of the interest.
        ///     This field specifies the Day Count Fraction as per ISDA definitions.
        /// Codes
        ///     One of the following codes must be used(Error code(s): T36) :
        ///     30E/360             30E/360 or Eurobond Basis
        ///     360/360             30/360, 360/360 or Bond Basis
        ///     ACT/360             Actual/360 (28-31/360)
        ///     ACT/365             Actual/365 or Actual/Actual(28-31/365-6)
        ///     AFI/365             Actual/365 (fixed) (28 - 31 / 365)
        ///     
        /// Example
        ///     February 2008:      
        ///     ACT / 360           29 / 360.          
        ///     ACT / 365           29 / 366.           
        ///     AFI / 365           29 / 365.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool Is_T14D_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            // 14D is a mandatory field in a mandatory block. It must be present
            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("14D") == true)
                {
                    if (field.Present == 0)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                    }
                    field.Value = field.Value.Trim();
                    if (field.Value.Length != 7)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + " - Incorrect field length : " + field.Value.Length);
                    }
                    if( (field.Value.Equals("30E/360") == false) && (field.Value.Equals("360/360") == false) && (field.Value.Equals("ACT/360") == false) &&
                        (field.Value.Equals("ACT/365") == false) && (field.Value.Equals("AFI/365") == false) )
                    {
                        valid = false;
                        Anomalies.Add("ERROR T36 - Tag " + field.Tag + " - Valid values are: 30E/360, 360/360, ACT/360, ACT/365 or AFI/365");
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T14D_Valid");
                }
            }
            else
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T15B_Valid
        /// Format
        ///     This is an empty field
        /// Presence
        ///     Mandatory in mandatory sequence B
        /// Definition    
        ///     This field specifies the start of mandatory sequence B Transaction Details.
        /// Usage Rules
        ///     Only the field tag must be present, the field is empty.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool Is_T15B_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            // 15B is a mandatory field in a mandatory block. It must be present
            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("15B") == true)
                {
                    if (field.Present == 0)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                    }
                    field.Value = field.Value.Trim();
                    if (field.Value.Equals("") == false)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + " - Only the field tag must be present, the field must be empty. It contains : " + field.Value);
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T15B_Valid");
                }
            }
            else
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T17R_Valid
        /// Format
        ///     Option R:  1!a
        /// Presence
        ///     Mandatory(referenced in rule C4) in mandatory sequence B
        /// Definition
        ///     This field specifies whether party A is the borrower or the lender.
        /// Codes
        ///     Indicator must contain one of the following codes(Error code(s): T67) :
        ///     B       Borrower: party A receives the principal amount and pays the interest.
        ///     L       Lender: party A pays the principal amount and receives the interest.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool Is_T17R_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            // 17R is a mandatory field in a mandatory block. It must be present
            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("17R") == true)
                {
                    if (field.Present == 0)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                    }
                    field.Value = field.Value.Trim();
                    if (field.Value.Length != 1)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + " - Incorrect field length : " + field.Value.Length);
                    }
                    if ((field.Value.Equals("B") == false) && (field.Value.Equals("L") == false))
                    {
                        valid = false;
                        Anomalies.Add("ERROR T67 - Tag " + field.Tag + " - Valid values are: B or L");
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T17R_Valid");
                }
            }
            else
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T30F_Valid
        /// Format
        ///     Option F:  8!n
        /// Presence
        ///     Conditional(see rule C5, also referenced in rule C6) in mandatory sequence B
        /// Definition
        ///     This field specifies the last day of the first/next interest period.
        /// Network Validated Rules
        ///     Date must be a valid date expressed as YYYYMMDD(Error code(s): T50).
        /// Usage Rules
        ///     This field should only be used when there is at least one interest payment before maturity.
        ///     In the first confirmation of a loan/deposit, this field contains the date of the first interest payment 
        ///     while in a rollover confirmation, this field specifies the next interest payment date.
        ///     The interest period is specified in field 38J.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool Is_T30F_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            Util util = new Util();

            // 30F is NOT a mandatory field in a mandatory block.
            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("30F") == true)
                {
                    field.Value = field.Value.Trim();
                    if (field.Value.Length != 8)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + " - Incorrect field length : " + field.Value.Length);
                    }
                    if ((util.IsDate(field.Value) == false))
                    {
                        valid = false;
                        Anomalies.Add("ERROR T50 - Tag " + field.Tag + " - is a date field. Must be in YYYYMMDD format");
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T30F_Valid");
                }
            }
            
            return valid;
        }

        /// <summary>
        /// Is_T30P_Valid
        /// Format
        ///     Option P:  8!n  (Date)
        /// Presence
        ///     Mandatory in mandatory sequence B
        /// Definition
        ///     This field specifies the latest agreed maturity date, that is, the date on which the principal is to be returned and the interest due.
        /// Network Validated Rules
        ///     Date must be a valid date expressed as YYYYMMDD(Error code(s): T50).
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool Is_T30P_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            Util util = new Util();

            // 30P is a mandatory field in a mandatory block.
            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("30P") == true)
                {
                    if (field.Present == 0)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                    }
                    field.Value = field.Value.Trim();
                    if (field.Value.Length != 8)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + " - Incorrect field length : " + field.Value.Length);
                    }
                    if ((util.IsDate(field.Value) == false))
                    {
                        valid = false;
                        Anomalies.Add("ERROR T50 - Tag " + field.Tag + " - is a date field. Must be in YYYYMMDD format");
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T30P_Valid");
                }
            }
            else
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T30T_Valid
        /// Format
        ///     Option T:  8!n  (Date)
        /// Presence
        ///     Mandatory in mandatory sequence B
        /// Definition
        ///     This field specifies the date the original deal or the rollover was agreed between party A and party B.
        /// Network Validated Rules
        ///     Date must be a valid date expressed as YYYYMMDD(Error code(s): T50).
        /// Usage Rules
        ///     The trade date remains the same when a confirmation is amended(corrected/completed) unilaterally.
        ///     When the terms of the deal are renegotiated on a bilateral basis, the trade date reflects the date of renegotiation in the amend message.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool Is_T30T_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            Util util = new Util();

            // 30T is a mandatory field in a mandatory block.
            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("30T") == true)
                {
                    if (field.Present == 0)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                    }
                    field.Value = field.Value.Trim();
                    if (field.Value.Length != 8)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + " - Incorrect field length : " + field.Value.Length);
                    }
                    if ((util.IsDate(field.Value) == false))
                    {
                        valid = false;
                        Anomalies.Add("ERROR T50 - Tag " + field.Tag + " - is a date field. Must be in YYYYMMDD format");
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T30T_Valid");
                }
            }
            else
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T30V_Valid
        /// Format
        ///     Option V:  8!n  (Date)
        /// Presence
        ///     Mandatory in mandatory sequence B
        /// Definition
        ///     This field specifies:
        ///         for a new confirmation(22B= CONF), the value date of the deposit;
        ///         for a rollover(22B= ROLL), the value date of the rollover, that is, the maturity date of the rolled over deposit;
        ///         for a maturity confirmation(22B= MATU), the value date of the original deposit for a non-rolled over deposit or the value date of the previous rollover.
        /// Network Validated Rules
        ///     Date must be a valid date expressed as YYYYMMDD(Error code(s): T50).
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool Is_T30V_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            Util util = new Util();

            // 30T is a mandatory field in a mandatory block.
            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("30V") == true)
                {
                    if (field.Present == 0)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                    }
                    field.Value = field.Value.Trim();
                    if (field.Value.Length != 8)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + " - Incorrect field length : " + field.Value.Length);
                    }
                    if ((util.IsDate(field.Value) == false))
                    {
                        valid = false;
                        Anomalies.Add("ERROR T50 - Tag " + field.Tag + " - is a date field. Must be in YYYYMMDD format");
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T30V_Valid");
                }
            }
            else
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T30X_Valid
        /// Format
        ///     Option X	8!n    (Date)
        /// Presence
        ///     Conditional(see rule C3) in mandatory sequence B
        /// Definition
        ///     This field specifies the date the next interest is due.
        /// Network Validated Rules
        ///     Date must be a valid date expressed as YYYYMMDD(Error code(s): T50).
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool Is_T30X_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            Util util = new Util();

            // 30X is NOT a mandatory field in a mandatory block.
            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("30X") == true)
                {
                    field.Value = field.Value.Trim();
                    if (field.Value.Length != 8)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + " - Incorrect field length : " + field.Value.Length);
                    }
                    if ((util.IsDate(field.Value) == false))
                    {
                        valid = false;
                        Anomalies.Add("ERROR T50 - Tag " + field.Tag + " - is a date field. Must be in YYYYMMDD format");
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T30X_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T32B_Valid
        /// Format
        ///     Option B	3!a15d     (Currency)(Amount)
        /// Presence
        ///     Mandatory(referenced in rule C9) in mandatory sequence B
        /// Definition
        ///     This field specifies the currency and contract amount, that is, the amount on which the interest specified in field 34E is calculated.
        ///     For a new confirmation(22B= CONF), this amount has to be settled at value date.
        /// Network Validated Rules
        ///     Currency must be a valid ISO 4217 currency code (Error code(s): T52).
        ///     The integer part of Amount must contain at least one digit.A decimal comma is mandatory and is included in the maximum length.
        ///     The number of digits following the comma must not exceed the maximum number allowed for the specified currency(Error code(s): C03, T40, T43).
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool Is_T32B_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            Util util = new Util();

            // 32B is a mandatory field in a mandatory block.
            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("32B") == true)
                {
                    if (field.Present == 0)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                    }
                    field.Value = field.Value.Trim();
                    if (field.Value.Length > 18)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + " - Incorrect field length : " + field.Value.Length);
                    }

                    string ccy = field.Value.Substring(0, 3);
                    if( util.IsValidCcy(ccy) == false)
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
        /// Is_T32H_Valid
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool Is_T32H_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            Util util = new Util();

            // 32H is NOT a mandatory field in a mandatory block.
            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("32H") == true)
                {
                    field.Value = field.Value.Trim();
                    if (field.Value.Length != 8)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + " - Incorrect field length : " + field.Value.Length);
                    }
                    if ((util.IsDate(field.Value) == false))
                    {
                        valid = false;
                        Anomalies.Add("ERROR T50 - Tag " + field.Tag + " - is a date field. Must be in YYYYMMDD format");
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T32H_Valid");
                }
            }

            return valid;
        }


        //private bool Is_T34E_Valid(TagData<string, string, string, string, int> field)
        //private bool Is_T37G_Valid(TagData<string, string, string, string, int> field)
        //private bool Is_T38J_Valid(TagData<string, string, string, string, int> field)
        //private bool Is_T39M_Valid(TagData<string, string, string, string, int> field)


        #endregion

        #endregion

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

            foreach (string sid in seqs)
            {
                if (sid.Equals("A") == true) seq = sequenceA;
                else if (sid.Equals("B") == true) seq = sequenceB;
                else if (sid.Equals("C") == true) seq = sequenceC;
                else if (sid.Equals("D") == true) seq = sequenceD;
                else if (sid.Equals("E") == true) seq = sequenceE;
                else if (sid.Equals("F") == true) seq = sequenceF;
                else if (sid.Equals("G") == true) seq = sequenceG;
                else if (sid.Equals("H") == true) seq = sequenceH;
                else if (sid.Equals("I") == true) seq = sequenceI;
                else seq = sequenceA;

                if (IsNewSequencePresent(seq) == true)
                {
                    foreach (TagData<string, string, string, string, int> t in seq)
                    {
                        validateField = true;
                        /* check for vairants */
                        if( (t.Tag.Equals("82A") && t.Present == 0) || (t.Tag.Equals("82D") && t.Present == 0) || (t.Tag.Equals("82J") && t.Present == 0) )
                        {
                            validateField = false;
                            T82present++;
                        }
                        if ((t.Tag.Equals("87A") && t.Present == 0) || (t.Tag.Equals("87D") && t.Present == 0) || (t.Tag.Equals("87J") && t.Present == 0))
                        {
                            validateField = false;
                            T87present++;
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

                if((sid.Equals("A") == true) && (T82present == 3))
                    Anomalies.Add("ERROR: Mandatory Tag 82 is not present in any variantion");
                else if ((sid.Equals("A") == true) && (T87present == 3))
                    Anomalies.Add("ERROR: Mandatory Tag 87 is not present in any variantion");
            }

            return allTagsValid;
        }

        private bool IsNewSequencePresent(List<TagData<string, string, string, string, int>> seq)
        {
            bool presentSequence = true;

            foreach (TagData<string, string, string, string, int> t in seq)
            {
                if (t.Tag.Contains("15") == true)
                {
                    if (t.Present == 0)
                        presentSequence = false;
                    break;
                }
            }

            return presentSequence;
        }

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
            string t21 = GetTagValue("A", "21");
            string t22A = GetTagValue("A", "22A");
            string t22B = GetTagValue("A", "22B");

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
            string t94A = GetTagValue("A", "94A");
            string t21N = GetTagValue("A", "21N");

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
            string t22B = GetTagValue("A", "22B");
            string t32H = GetTagValue("B", "32H");
            string t30X = GetTagValue("B", "30X");

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
            string t22B = GetTagValue("A", "22B");
            string t17R = GetTagValue("B", "17R");
            string t32H = GetTagValue("B", "32H");

            if(t22B.Equals("MATU") == true)
            {
                if (t17R.Equals("L") == false)
                {
                    valid = false;
                    Anomalies.Add("ERROR D57 : Validation Rule C4 failed : if tag 22B != MATU then tag 17R is not applicable");
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
        private bool Valid_VR_C5()
        {
            bool valid = true;


            return valid;
        }
        private bool Valid_VR_C6()
        {
            bool valid = true;


            return valid;
        }
        private bool Valid_VR_C7()
        {
            bool valid = true;


            return valid;
        }
        private bool Valid_VR_C8()
        {
            bool valid = true;


            return valid;
        }
        private bool Valid_VR_C9()
        {
            bool valid = true;


            return valid;
        }
        private bool Valid_VR_C10()
        {
            bool valid = true;


            return valid;
        }
        private bool Valid_VR_C11()
        {
            bool valid = true;


            return valid;
        }
        #endregion

    }

}


/**************************** method list ************************************
 * 
 * public MT320()												
 * public MT320(String msg)
 * 
 * private void InitializeMT320()
 * private void DefineScope()
 * private void ResetVariables()
 * private void ParseBlock4(string message)
 * private void ParsePipeMsg(string message)
 * private void FillDataTags(string[] tags)
 * **private void ParsePipeMsg(string message)
 * 
 * GET SET 
 * public List<TagData<string, string, string, string>> this[int index]
 * public List<TagData<string, string, string, string>> this[int index]
 * public string Scope
 * public List<string> Anomalies
 * public string GetTagName(string sequence, string tag)
 * public string GetTagValue(string sequence, string tag)
 * public void SetTagValue(string sequence, string tag, string value)
 * public bool IsTagMandatory(string sequence, string tag)
 * public string GetTagStatus(string sequence, string tag)
 * **public string GeDetailtXML()
 * 
 * VALIDATION
 * public bool IsMessageValid()
 * **public bool ValidateTag(string sequence, string tag)
 * **public bool ValidateTags()
 *
 * private bool Valid_VR_C1()
 * private bool Valid_VR_C2()
 * private bool Valid_VR_C3()
 * **private bool Valid_VR_C4()
 * **private bool Valid_VR_C5()
 * **private bool Valid_VR_C6()
 * **private bool Valid_VR_C7()
 * **private bool Valid_VR_C8()
 * **private bool Valid_VR_C9()
 * **private bool Valid_VR_C10()
 * **private bool Valid_VR_C11()
 * 
 ***/

/**************************** method needed ************************************
 * 
 * 1. validate message format
 * 2. generate xml message
 * 3. get xml message
 * 4. complete pipe delimited reader
 * 5. generate pipe delimited message
 * 
 ***/
