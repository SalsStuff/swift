using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace Messages
{
   public class MT320
   {
      #region SECTION_VARIABLES
      int m_MT320_SECTION = 9;

      List<TagData<string, string, string, string>> sectionA = new List<TagData<string, string, string, string>>
      {
         // Tag, Name, Value, Mandatory
         new TagData<string, string, string, string>("15A", "New Sequence",              "" ,"M"),
         new TagData<string, string, string, string>("20",  "Sender's Reference",        "" ,"M"),
         new TagData<string, string, string, string>("21",  "Related Reference",         "" ,"M"),
         new TagData<string, string, string, string>("21N", "Contract Number Party A",   "" ,"O"),
         new TagData<string, string, string, string>("22A", "Type of Operation",         "", "M"),
         new TagData<string, string, string, string>("22B", "Type of Event",             "", "M"),
         new TagData<string, string, string, string>("22C", "Common Reference",          "", "M"),
         new TagData<string, string, string, string>("77D", "Terms and Conditions",      "", "O"),
         new TagData<string, string, string, string>("82A", "Party A",                   "", "M"),
         new TagData<string, string, string, string>("82D", "Party A",                   "", "M"),
         new TagData<string, string, string, string>("82J", "Party A",                   "", "M"),
         new TagData<string, string, string, string>("83A", "Fund or Instructing Party", "", "O"),
         new TagData<string, string, string, string>("83D", "Fund or Instructing Party", "", "O"),
         new TagData<string, string, string, string>("83J", "Fund or Instructing Party", "", "O"),
         new TagData<string, string, string, string>("87A", "Party B",                   "", "M"),
         new TagData<string, string, string, string>("87D", "Party B",                   "", "M"),
         new TagData<string, string, string, string>("87J", "Party B",                   "", "M"),
         new TagData<string, string, string, string>("94A", "Scope of Operation",        "", "O")
      };

      List<TagData<string, string, string, string>> sectionB = new List<TagData<string, string, string, string>>
      {
         // Tag, Name, Value, Mandatory
         new TagData<string, string, string, string>("14D", "Day Count Fraction"                   , "" ,"M"),
         new TagData<string, string, string, string>("15B", "New Sequence"                         , "" ,"M"),
         new TagData<string, string, string, string>("17R", "Party A's Role"                       , "" ,"M"),
         new TagData<string, string, string, string>("30F", "Last Day of the First Interest Period", "" ,"O"),
         new TagData<string, string, string, string>("30P", "Maturity Date"                        , "", "M"),
         new TagData<string, string, string, string>("30T", "Trade Date"                           , "", "M"),
         new TagData<string, string, string, string>("30V", "Value Date"                           , "", "M"),
         new TagData<string, string, string, string>("30X", "Next Interest Due Date"               , "", "O"),
         new TagData<string, string, string, string>("32B", "Currency and Principal Amount"        , "", "M"),
         new TagData<string, string, string, string>("32H", "Amount to be Settled"                 , "", "O"),
         new TagData<string, string, string, string>("34E", "Currenct and Interest Amount"         , "", "M"),
         new TagData<string, string, string, string>("37G", "Interest Rate"                        , "", "M"),
         new TagData<string, string, string, string>("38J", "Number of Days"                       , "", "O")
         //new TagData<string, string, string, string>("39M", "Payment Clearing Center"              , "", "O")
      };

      List<TagData<string, string, string, string>> sectionC = new List<TagData<string, string, string, string>>
      {
         // Tag, Name, Value, Mandatory
         new TagData<string, string, string, string>("15C", "New Sequence"           , "" ,"M"),
         new TagData<string, string, string, string>("53A", "Delivery Agent"         , "" ,"O"),
         new TagData<string, string, string, string>("53D", "Delivery Agent"         , "" ,"O"),
         new TagData<string, string, string, string>("53J", "Delivery Agent"         , "" ,"O"),
         new TagData<string, string, string, string>("56A", "Intermediary"           , "", "O"),
         new TagData<string, string, string, string>("56D", "Intermediary"           , "", "O"),
         new TagData<string, string, string, string>("56J", "Intermediary"           , "", "O"),
         new TagData<string, string, string, string>("57A", "Receiving Agent"        , "", "M"),
         new TagData<string, string, string, string>("57D", "Receiving Agent"        , "", "M"),
         new TagData<string, string, string, string>("57J", "Receiving Agent"        , "", "M"),
         new TagData<string, string, string, string>("58A", "Beneficiary Institution", "", "O"),
         new TagData<string, string, string, string>("58D", "Beneficiary Institution", "", "O"),
         new TagData<string, string, string, string>("58J", "Beneficiary Institution", "", "O"),
         new TagData<string, string, string, string>("86A", "Intermediary 2"         , "", "O"),
         new TagData<string, string, string, string>("86D", "Intermediary 2"         , "", "O"),
         new TagData<string, string, string, string>("86J", "Intermediary 2"         , "", "O")
      };

      List<TagData<string, string, string, string>> sectionD = new List<TagData<string, string, string, string>>
      {
         // Tag, Name, Value, Mandatory
         new TagData<string, string, string, string>("15D", "New Sequence"           , "" ,"M"),
         new TagData<string, string, string, string>("53A", "Delivery Agent"         , "" ,"O"),
         new TagData<string, string, string, string>("53D", "Delivery Agent"         , "" ,"O"),
         new TagData<string, string, string, string>("53J", "Delivery Agent"         , "" ,"O"),
         new TagData<string, string, string, string>("56A", "Intermediary"           , "", "O"),
         new TagData<string, string, string, string>("56D", "Intermediary"           , "", "O"),
         new TagData<string, string, string, string>("56J", "Intermediary"           , "", "O"),
         new TagData<string, string, string, string>("57A", "Receiving Agent"        , "", "M"),
         new TagData<string, string, string, string>("57D", "Receiving Agent"        , "", "M"),
         new TagData<string, string, string, string>("57J", "Receiving Agent"        , "", "M"),
         new TagData<string, string, string, string>("58A", "Beneficiary Institution", "", "O"),
         new TagData<string, string, string, string>("58D", "Beneficiary Institution", "", "O"),
         new TagData<string, string, string, string>("58J", "Beneficiary Institution", "", "O"),
         new TagData<string, string, string, string>("86A", "Intermediary 2"         , "", "O"),
         new TagData<string, string, string, string>("86D", "Intermediary 2"         , "", "O"),
         new TagData<string, string, string, string>("86J", "Intermediary 2"         , "", "O")
      };

      List<TagData<string, string, string, string>> sectionE = new List<TagData<string, string, string, string>>
      {
         // Tag, Name, Value, Mandatory
         new TagData<string, string, string, string>("15E", "New Sequence"           , "" ,"M"),
         new TagData<string, string, string, string>("53A", "Delivery Agent"         , "" ,"O"),
         new TagData<string, string, string, string>("53D", "Delivery Agent"         , "" ,"O"),
         new TagData<string, string, string, string>("53J", "Delivery Agent"         , "" ,"O"),
         new TagData<string, string, string, string>("56A", "Intermediary"           , "", "O"),
         new TagData<string, string, string, string>("56D", "Intermediary"           , "", "O"),
         new TagData<string, string, string, string>("56J", "Intermediary"           , "", "O"),
         new TagData<string, string, string, string>("57A", "Receiving Agent"        , "", "M"),
         new TagData<string, string, string, string>("57D", "Receiving Agent"        , "", "M"),
         new TagData<string, string, string, string>("57J", "Receiving Agent"        , "", "M"),
         new TagData<string, string, string, string>("58A", "Beneficiary Institution", "", "O"),
         new TagData<string, string, string, string>("58D", "Beneficiary Institution", "", "O"),
         new TagData<string, string, string, string>("58J", "Beneficiary Institution", "", "O"),
         new TagData<string, string, string, string>("86A", "Intermediary 2"         , "", "O"),
         new TagData<string, string, string, string>("86D", "Intermediary 2"         , "", "O"),
         new TagData<string, string, string, string>("86J", "Intermediary 2"         , "", "O")
      };

      List<TagData<string, string, string, string>> sectionF = new List<TagData<string, string, string, string>>
      {
         // Tag, Name, Value, Mandatory
         new TagData<string, string, string, string>("15F", "New Sequence"           , "" ,"M"),
         new TagData<string, string, string, string>("53A", "Delivery Agent"         , "" ,"O"),
         new TagData<string, string, string, string>("53D", "Delivery Agent"         , "" ,"O"),
         new TagData<string, string, string, string>("53J", "Delivery Agent"         , "" ,"O"),
         new TagData<string, string, string, string>("56A", "Intermediary"           , "", "O"),
         new TagData<string, string, string, string>("56D", "Intermediary"           , "", "O"),
         new TagData<string, string, string, string>("56J", "Intermediary"           , "", "O"),
         new TagData<string, string, string, string>("57A", "Receiving Agent"        , "", "M"),
         new TagData<string, string, string, string>("57D", "Receiving Agent"        , "", "M"),
         new TagData<string, string, string, string>("57J", "Receiving Agent"        , "", "M"),
         new TagData<string, string, string, string>("58A", "Beneficiary Institution", "", "O"),
         new TagData<string, string, string, string>("58D", "Beneficiary Institution", "", "O"),
         new TagData<string, string, string, string>("58J", "Beneficiary Institution", "", "O"),
         new TagData<string, string, string, string>("86A", "Intermediary 2"         , "", "O"),
         new TagData<string, string, string, string>("86D", "Intermediary 2"         , "", "O"),
         new TagData<string, string, string, string>("86J", "Intermediary 2"         , "", "O")
      };

      List<TagData<string, string, string, string>> sectionG = new List<TagData<string, string, string, string>>
      {
         // Tag, Name, Value, Mandatory
         new TagData<string, string, string, string>("15G", "New Sequence"                                , "" ,"M"),
         new TagData<string, string, string, string>("33B", "Transaction Currency and Net Interest Amount", "" ,"M"),
         new TagData<string, string, string, string>("33E", "Reporting Currency and Tax Amount"           , "" ,"O"),
         new TagData<string, string, string, string>("36",  "Exchange Rate"                               , "" ,"O"),
         new TagData<string, string, string, string>("37L", "Tax Rate"                                    , "", "M")
      };

      List<TagData<string, string, string, string>> sectionH = new List<TagData<string, string, string, string>>
      {
         // Tag, Name, Value, Mandatory
         new TagData<string, string, string, string>("15H", "New Sequence"                  , "" ,"M"),
         new TagData<string, string, string, string>("21G", "Broker's Reference"            , "" ,"O"),
         new TagData<string, string, string, string>("24D", "Dealing Method"                , "" ,"O"),
         new TagData<string, string, string, string>("26H", "Counterparty's Reference"      , "" ,"O"),
         new TagData<string, string, string, string>("29A", "Contract Information"          , "", "O"),
         //new TagData<string, string, string, string>("34C", "Commission and Fees"           , "", "O"),
         new TagData<string, string, string, string>("71F", "Broker's Commission"           , "", "O"),
         new TagData<string, string, string, string>("72",  "Sender to Receiver Information", "", "O"),
         new TagData<string, string, string, string>("84A", "Dealing Branch Party A"        , "", "O"),
         new TagData<string, string, string, string>("84B", "Dealing Branch Party A"        , "", "O"),
         new TagData<string, string, string, string>("84D", "Dealing Branch Party A"        , "", "O"),
         new TagData<string, string, string, string>("84J", "Dealing Branch Party A"        , "", "O"),
         new TagData<string, string, string, string>("85A", "Dealing Branch Party B"        , "", "O"),
         new TagData<string, string, string, string>("85B", "Dealing Branch Party B"        , "", "O"),
         new TagData<string, string, string, string>("85D", "Dealing Branch Party B"        , "", "O"),
         new TagData<string, string, string, string>("85J", "Dealing Branch Party B"        , "", "O"),
         new TagData<string, string, string, string>("88A", "Broker Identification"         , "", "O"),
         new TagData<string, string, string, string>("88D", "Broker Identification"         , "", "O"),
         new TagData<string, string, string, string>("88J", "Broker Identification"         , "", "O")
      };

      List<TagData<string, string, string, string>> sectionI = new List<TagData<string, string, string, string>>
      {
         // Tag, Name, Value, Mandatory
         new TagData<string, string, string, string>("15I", "New Sequence"            , "" ,"M"),
         new TagData<string, string, string, string>("18A", "Number of Repetitions"   , "" ,"M"),
         new TagData<string, string, string, string>("30F", "Payment Date"            , "" ,"M"),
         new TagData<string, string, string, string>("32H", "Currency, Payment Amount", "" ,"M"),
         new TagData<string, string, string, string>("53A", "Delivery Agent"          , "", "O"),
         new TagData<string, string, string, string>("53D", "Delivery Agent"          , "", "O"),
         new TagData<string, string, string, string>("53J", "Delivery Agent"          , "", "O"),
         new TagData<string, string, string, string>("56A", "Intermediary"            , "", "O"),
         new TagData<string, string, string, string>("56D", "Intermediary"            , "", "O"),
         new TagData<string, string, string, string>("56J", "Intermediary"            , "", "O"),
         new TagData<string, string, string, string>("57A", "Receiving Agent"         , "", "M"),
         new TagData<string, string, string, string>("57D", "Receiving Agent"         , "", "M"),
         new TagData<string, string, string, string>("57J", "Receiving Agent"         , "", "M"),
         new TagData<string, string, string, string>("86A", "Intermediary 2"          , "", "O"),
         new TagData<string, string, string, string>("86D", "Intermediary 2"          , "", "O"),
         new TagData<string, string, string, string>("86J", "Intermediary 2"          , "", "O")
      };

      #endregion

      public MT320()
      {
         resetVariables();
      }

      public MT320(String msg)
      {
         resetVariables();

         if (msg.Contains("{4:") == true)
            parseBlock4(msg);
         if (msg.Contains("||") == true)
            parsePipeMsg(msg);
      }

      private void resetVariables()
      {
         foreach (TagData<string, string, string, string> t in sectionA)
         {
            t.Value = "";
         }
         foreach (TagData<string, string, string, string> t in sectionB)
         {
            t.Value = "";
         }
         foreach (TagData<string, string, string, string> t in sectionC)
         {
            t.Value = "";
         }
         foreach (TagData<string, string, string, string> t in sectionD)
         {
            t.Value = "";
         }
         foreach (TagData<string, string, string, string> t in sectionE)
         {
            t.Value = "";
         }
         foreach (TagData<string, string, string, string> t in sectionF)
         {
            t.Value = "";
         }
         foreach (TagData<string, string, string, string> t in sectionG)
         {
            t.Value = "";
         }
         foreach (TagData<string, string, string, string> t in sectionH)
         {
            t.Value = "";
         }
         foreach (TagData<string, string, string, string> t in sectionI)
         {
            t.Value = "";
         }
      }

      private void parseBlock4(string message)
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

         fillDataTags(result);
      }

      private void fillDataTags(string[] tags)
      {
         string useSection = "";

         foreach (string key in tags)
         {
            string[] keyPair = key.Split(',');
            if (keyPair[0].Contains("15") == true)
               useSection = keyPair[0].Substring(2);
            else
               setTagValue(useSection, keyPair[0], keyPair[1]);
         }
      }

      private void parsePipeMsg(string message)
      {

      }

      public List<TagData<string, string, string, string>> this[int index]
      {
         get
         {
            if (index == 0) { return sectionA; }
            else if (index == 1) { return sectionB; }
            else if (index == 2) { return sectionC; }
            else if (index == 3) { return sectionD; }
            else if (index == 4) { return sectionE; }
            else if (index == 5) { return sectionF; }
            else if (index == 6) { return sectionG; }
            else if (index == 7) { return sectionH; }
            else if (index == 8) { return sectionI; }
            else { return null; }
         }
      }

      public int NumOfSections
      {
         get { return m_MT320_SECTION; }
      }

      public string getTagName(string section, string tag)
      {
         string tagName = null;

         switch(section)
         {
            case "A":
               foreach(TagData<string, string, string, string> t in sectionA)
               {
                  if(tag.Equals(t.Tag) ==  true)
                  {
                     tagName = t.Name;
                     break;
                  }
               }
               break;
            case "B":
               foreach (TagData<string, string, string, string> t in sectionB)
               {
                  if (tag.Equals(t.Tag) == true)
                  {
                     tagName = t.Name;
                     break;
                  }
               }
               break;
            case "C":
               foreach (TagData<string, string, string, string> t in sectionC)
               {
                  if (tag.Equals(t.Tag) == true)
                  {
                     tagName = t.Name;
                     break;
                  }
               }
               break;
            case "D":
               foreach (TagData<string, string, string, string> t in sectionD)
               {
                  if (tag.Equals(t.Tag) == true)
                  {
                     tagName = t.Name;
                     break;
                  }
               }
               break;
            case "E":
               foreach (TagData<string, string, string, string> t in sectionE)
               {
                  if (tag.Equals(t.Tag) == true)
                  {
                     tagName = t.Name;
                     break;
                  }
               }
               break;
            case "F":
               foreach (TagData<string, string, string, string> t in sectionF)
               {
                  if (tag.Equals(t.Tag) == true)
                  {
                     tagName = t.Name;
                     break;
                  }
               }
               break;
            case "G":
               foreach (TagData<string, string, string, string> t in sectionG)
               {
                  if (tag.Equals(t.Tag) == true)
                  {
                     tagName = t.Name;
                     break;
                  }
               }
               break;
            case "H":
               foreach (TagData<string, string, string, string> t in sectionH)
               {
                  if (tag.Equals(t.Tag) == true)
                  {
                     tagName = t.Name;
                     break;
                  }
               }
               break;
            case "I":
               foreach (TagData<string, string, string, string> t in sectionI)
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

      public string getTagValue(string section, string tag)
      {
         string tagValue = null;

         switch (section)
         {
            case "A":
               foreach (TagData<string, string, string, string> t in sectionA)
               {
                  if (tag.Equals(t.Tag) == true)
                  {
                     tagValue = t.Value;
                     break;
                  }
               }
               break;
            case "B":
               foreach (TagData<string, string, string, string> t in sectionB)
               {
                  if (tag.Equals(t.Tag) == true)
                  {
                     tagValue = t.Value;
                     break;
                  }
               }
               break;
            case "C":
               foreach (TagData<string, string, string, string> t in sectionC)
               {
                  if (tag.Equals(t.Tag) == true)
                  {
                     tagValue = t.Value;
                     break;
                  }
               }
               break;
            case "D":
               foreach (TagData<string, string, string, string> t in sectionD)
               {
                  if (tag.Equals(t.Tag) == true)
                  {
                     tagValue = t.Value;
                     break;
                  }
               }
               break;
            case "E":
               foreach (TagData<string, string, string, string> t in sectionE)
               {
                  if (tag.Equals(t.Tag) == true)
                  {
                     tagValue = t.Value;
                     break;
                  }
               }
               break;
            case "F":
               foreach (TagData<string, string, string, string> t in sectionF)
               {
                  if (tag.Equals(t.Tag) == true)
                  {
                     tagValue = t.Value;
                     break;
                  }
               }
               break;
            case "G":
               foreach (TagData<string, string, string, string> t in sectionG)
               {
                  if (tag.Equals(t.Tag) == true)
                  {
                     tagValue = t.Value;
                     break;
                  }
               }
               break;
            case "H":
               foreach (TagData<string, string, string, string> t in sectionH)
               {
                  if (tag.Equals(t.Tag) == true)
                  {
                     tagValue = t.Value;
                     break;
                  }
               }
               break;
            case "I":
               foreach (TagData<string, string, string, string> t in sectionI)
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

      public void setTagValue(string section, string tag, string value)
      {
         switch (section)
         {
            case "A":
               foreach (TagData<string, string, string, string> t in sectionA)
               {
                  if (tag.Equals(t.Tag) == true)
                  {
                     t.Value = value;
                     break;
                  }
               }
               break;
            case "B":
               foreach (TagData<string, string, string, string> t in sectionB)
               {
                  if (tag.Equals(t.Tag) == true)
                  {
                     t.Value = value;
                     break;
                  }
               }
               break;
            case "C":
               foreach (TagData<string, string, string, string> t in sectionC)
               {
                  if (tag.Equals(t.Tag) == true)
                  {
                     t.Value = value;
                     break;
                  }
               }
               break;
            case "D":
               foreach (TagData<string, string, string, string> t in sectionD)
               {
                  if (tag.Equals(t.Tag) == true)
                  {
                     t.Value = value;
                     break;
                  }
               }
               break;
            case "E":
               foreach (TagData<string, string, string, string> t in sectionE)
               {
                  if (tag.Equals(t.Tag) == true)
                  {
                     t.Value = value;
                     break;
                  }
               }
               break;
            case "F":
               foreach (TagData<string, string, string, string> t in sectionF)
               {
                  if (tag.Equals(t.Tag) == true)
                  {
                     t.Value = value;
                     break;
                  }
               }
               break;
            case "G":
               foreach (TagData<string, string, string, string> t in sectionG)
               {
                  if (tag.Equals(t.Tag) == true)
                  {
                     t.Value = value;
                     break;
                  }
               }
               break;
            case "H":
               foreach (TagData<string, string, string, string> t in sectionH)
               {
                  if (tag.Equals(t.Tag) == true)
                  {
                     t.Value = value;
                     break;
                  }
               }
               break;
            case "I":
               foreach (TagData<string, string, string, string> t in sectionI)
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

      public bool isTagMandatory(string section, string tag)
      {
         bool isMandatory = false;

         switch (section)
         {
            case "A":
               foreach (TagData<string, string, string, string> t in sectionA)
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
               foreach (TagData<string, string, string, string> t in sectionB)
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
               foreach (TagData<string, string, string, string> t in sectionC)
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
               foreach (TagData<string, string, string, string> t in sectionD)
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
               foreach (TagData<string, string, string, string> t in sectionE)
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
               foreach (TagData<string, string, string, string> t in sectionF)
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
               foreach (TagData<string, string, string, string> t in sectionG)
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
               foreach (TagData<string, string, string, string> t in sectionH)
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
               foreach (TagData<string, string, string, string> t in sectionI)
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

      public string geDetailtXML()
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

   }

}
