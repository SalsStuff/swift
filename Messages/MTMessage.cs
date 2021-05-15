using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace Messages
{
    public class MTMessage
    {
        #region MEMBER VARIABLES
        /// <summary>
        /// Get method to return the SWIFT definition of message scope
        /// </summary>
        public string Scope { get; protected set; } = "";

        public Object DB_Conn { get; set; } = null;

        /// <summary>
        /// Get method to return the number of class/message data sequence
        /// </summary>
        public int numOfSequences { get; set; } = 0;

        /// <summary>
        /// Get method to read back any errors or warnings set during the parsing of the message
        /// </summary>
        public List<string> Anomalies { get; } = new List<string>();

        /// <summary>
        /// Get / Set method to always validate tag whether or not it is present in message.
        /// </summary>
        public bool AlwaysValidateTag { get; set; } = false;
        #endregion

        protected virtual string whichSequence(List<TagData<string, string, string, string, int>> sequence)
        {
            string seq = null;

            if (numOfSequences == 1)
            {
                seq = "A";
            }
            else
            {
                foreach (TagData<string, string, string, string, int> t in sequence)
                {
                    if (t.Tag.Contains("15") == true)
                    {
                        seq = t.Tag.Substring(2, 1);
                        break;
                    }
                }
            }

            return seq;
        }

        /// <summary>
        /// Get method the return the readable name of a SWIFT tag number
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        protected string GetTagName(List<TagData<string, string, string, string, int>> sequence, string tag)
        {
            string tagName = null;

            foreach (TagData<string, string, string, string, int> t in sequence)
            {
                if (tag.Equals(t.Tag) == true)
                {
                    tagName = t.Name;
                    break;
                }
            }

            return tagName;
        }

        /// <summary>
        /// Get method to return the value of a tag
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        protected string GetTagValue(List<TagData<string, string, string, string, int>> sequence, string tag)
        {
            string tagValue = null;

            foreach (TagData<string, string, string, string, int> t in sequence)
            {
                if (tag.Equals(t.Tag) == true)
                {
                    tagValue = t.Value;
                    break;
                }
            }

            return tagValue;
        }

        /// <summary>
        /// Set method to set a tag value
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        protected virtual void SetTagValue(List<TagData<string, string, string, string, int>> sequence, string tag, string value)
        {
            foreach (TagData<string, string, string, string, int> t in sequence)
            {
                if (tag.Equals(t.Tag) == true)
                {
                    t.Value = value;
                    break;
                }
            }
        }

        /// <summary>
        /// Set method to set the present flag of a tag
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="tag"></param>
        /// <param name="present"></param>
        protected void SetTagPresent(List<TagData<string, string, string, string, int>> sequence, string tag, int present)
        {
            foreach (TagData<string, string, string, string, int> t in sequence)
            {
                if (tag.Equals(t.Tag) == true)
                {
                    t.Present = present;
                    break;
                }
            }
        }

        protected bool isTagPresentInSequence(List<TagData<string, string, string, string, int>> sequence, string tag)
        {
            bool present = false;

            foreach (TagData<string, string, string, string, int> t in sequence)
            {
                if (tag.Equals(t.Tag) == true)
                {
                    present = Convert.ToBoolean(t.Present);
                    break;
                }
            }

            return present;
        }

        protected bool isAnyTagPresentInSequence(List<TagData<string, string, string, string, int>> sequence)
        {
            bool present = false;

            foreach (TagData<string, string, string, string, int> t in sequence)
            {
                if (t.Tag.StartsWith("15") == true)
                {
                    continue;
                }
                else if (Convert.ToBoolean(t.Present) == true)
                {
                    present = true;
                    break;
                }
            }

            return present;
        }

        protected bool IsNewSequencePresent(List<TagData<string, string, string, string, int>> seq)
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
        /// Get method to determine if a tag is mandatory
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        protected bool IsTagMandatory(List<TagData<string, string, string, string, int>> sequence, string tag)
        {
            bool isMandatory = false;

            foreach (TagData<string, string, string, string, int> t in sequence)
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
    
            return isMandatory;
        }

        /// <summary>
        /// Get method to determine if a tag is mandatory, optional, conditional or unknown
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        protected string GetTagStatus(List<TagData<string, string, string, string, int>> sequence, string tag)
        {
            string status = "U";

            foreach (TagData<string, string, string, string, int> t in sequence)
            {
                if (tag.Equals(t.Tag) == true)
                {
                    status = t.Mandatory;
                }
            }

            return status;
        }

        protected virtual void DefineScope()
        {
            Scope = "Scope not defined for this message.\r\n";
        }

        protected virtual void ResetVariables()
        {

        }

        protected bool CheckStringsForEquality(params string[] strings)
        {
            string target = strings.FirstOrDefault(s => !string.IsNullOrEmpty(s));

            if (target == null)
                return false;

            return strings.All(s => string.IsNullOrEmpty(s) || s == target);
        }

        protected virtual string[] ParseBlock4MessageString(string message)
        {
            string input = message.Substring(3);
            input = input.Substring(input.IndexOf(':') + 1);

            // Remove the End of Block flag
            input = input.Substring(0, input.Length - 2);
            // Split the tags and values up
            string[] result = input.Split(':');
            result = result.Where((s, i) => i % 2 == 0)
                        .Zip(result.Where((s, i) => i % 2 == 1), (a, b) => a + "~" + b)
                        .ToArray();

            return result;
        }

        protected virtual List<TagData<string, string, string, string, int>> getSequence(string seqId)
        {
            List<TagData<string, string, string, string, int>> sequence = null;

            switch (seqId)
            {
                default:
                    break;
            }
            return sequence;
        }

        protected virtual List<TagData<string, string, string, string, int>> getSequence(string seqId, int idx)
        {
            List<TagData<string, string, string, string, int>> sequence = null;

            switch (seqId)
            {
                default:
                    break;
            }
            return sequence;
        }

        /// <summary>
        /// Fill in the class variables with the SWIFT message data
        /// </summary>
        /// <param name="tags"></param>
        protected virtual void FillDataTags(string[] tags)
        {
            List<TagData<string, string, string, string, int>> useSequence = null;

            if (numOfSequences == 1)
                useSequence = getSequence("A");

            foreach (string key in tags)
            {
                string[] keyPair = key.Split('~');
                if (keyPair[0].Contains("15") == true)
                {
                    useSequence = getSequence(keyPair[0].Substring(2));
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
        /// parseCcyAmt
        /// 
        /// Parses Tags 32B, 32H and 34E
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="option"></param>
        /// <param name="ccy"></param>
        /// <param name="amount"></param>
        protected void parseCcyAmt(List<TagData<string, string, string, string, int>> seq, string option, out string ccy, out Nullable<double> amount)
        {
            string rawStr = GetTagValue(seq, option);
            double multiplier = 1.0;
            string amtStr = null;

            try
            {
                if (rawStr != null && rawStr.Length >= 1 && isTagPresentInSequence(seq, option) == true)
                {
                    if (rawStr.Substring(0, 1).Equals("N") && (Char.IsLetter(rawStr[1]) == true && Char.IsLetter(rawStr[2]) == true && Char.IsLetter(rawStr[3]) == true))
                    {
                        multiplier = -1.0;
                        ccy = rawStr.Substring(1, 3);
                        amtStr = rawStr.Substring(4, rawStr.Length - 4);
                        amtStr = amtStr.Replace(",", ".");
                        amount = Convert.ToDouble(amtStr) * multiplier;
                    }
                    else
                    {
                        multiplier = 1.0;
                        ccy = rawStr.Substring(0, 3);
                        amtStr = rawStr.Substring(3, rawStr.Length - 3);
                        amtStr = amtStr.Replace(",", ".");
                        amount = Convert.ToDouble(amtStr) * multiplier;
                    }
                }
                else
                {
                    ccy = null;
                    amount = null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid Tag Data: Tag " + option + ".\n" + ex.Message);
            }
        }

        /// <summary>
        /// parseDateCcyAmt
        /// 
        /// Parses Tags 32A
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="option"></param>
        /// <param name="date"></param>
        /// <param name="ccy"></param>
        /// <param name="amount"></param>
        protected void parseDateCcyAmt(List<TagData<string, string, string, string, int>> seq, string option, out string date, out string ccy, out Nullable<double> amount)
        {
            string rawStr = GetTagValue(seq, option);
            double multiplier = 1.0;
            string amtStr = null;

            try
            {
                if (rawStr != null && rawStr.Length >= 1 && isTagPresentInSequence(seq, option) == true)
                {
                    date = rawStr.Substring(0, 6);
                    rawStr = rawStr.Substring(6, rawStr.Length - 6);
                    if (rawStr.Substring(0, 1).Equals("N") && (Char.IsLetter(rawStr[1]) == true && Char.IsLetter(rawStr[2]) == true && Char.IsLetter(rawStr[3]) == true))
                    {
                        multiplier = -1.0;
                        ccy = rawStr.Substring(1, 3);
                        amtStr = rawStr.Substring(4, rawStr.Length - 4);
                        amtStr = amtStr.Replace(",", ".");
                        amount = Convert.ToDouble(amtStr) * multiplier;
                    }
                    else
                    {
                        multiplier = 1.0;
                        ccy = rawStr.Substring(0, 3);
                        amtStr = rawStr.Substring(3, rawStr.Length - 3);
                        amtStr = amtStr.Replace(",", ".");
                        amount = Convert.ToDouble(amtStr) * multiplier;
                    }
                }
                else
                {
                    date = null;
                    ccy = null;
                    amount = null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid Tag Data: Tag " + option + ".\n" + ex.Message);
            }
        }

        /// <summary>
        /// parsePartyAgent
        /// 
        /// Parses Tags 52A/B/D, 53A/D/J, 56A/D/J, 57A/D/J, 58A/D/J, 82A/D/J, 83A/D/J, 84A/B/D/J, 85A/B/D/J, 
        /// 86A/D/J, 87A/D/J and 88A/D/J of a given 
        /// sequence and returns the id and code in a list
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        protected List<string> parsePartyAgent(List<TagData<string, string, string, string, int>> seq, string option)
        {
            string rawStr = GetTagValue(seq, option);
            List<string> retLst = new List<string>();
            string[] stringSeparators = new string[] { "\r\n" };

            try
            {
                if (rawStr != null && rawStr.Length >= 1 && isTagPresentInSequence(seq, option) == true)
                {
                    if (option.Contains("A") == true || option.Contains("B") == true || option.Contains("D") == true || option.Contains("K") == true)
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
                    else if (option.Contains("J") == true)
                    {
                        retLst.Add(rawStr);
                    }
                    else if (option.Contains("F") == true)
                    {
                        string[] lines = rawStr.Split(stringSeparators, 2, StringSplitOptions.None);
                        retLst.Add(lines[0]);
                        retLst.Add(lines[1]);
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
                throw new Exception("Invalid Tag Data: Tag " + option + ".\n" + ex.Message);
            }

            return retLst;
        }

        /// <summary>
        /// parseAcctNameAddr
        /// 
        /// Returns the account, name and address of the party
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        protected List<string> parseAcctNameAddr(List<TagData<string, string, string, string, int>> seq, string option)
        {
            string rawStr = GetTagValue(seq, option);
            List<string> retLst = new List<string>();
            string[] stringSeparators = new string[] { "\r\n" };

            try
            {
                if (rawStr != null && rawStr.Length >= 1 && isTagPresentInSequence(seq, option) == true)
                {
                    if (rawStr[0].Equals("/"))
                    {
                        string[] lines = rawStr.Split(stringSeparators, 2, StringSplitOptions.None);
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

            return retLst;
        }

        /// <summary>
        /// getDouble
        /// 
        /// Returns the value in tag 36
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public Nullable<double> getDouble(List<TagData<string, string, string, string, int>> seq, string option)
        {
            Nullable<double> number = null;
            double num;

            if (double.TryParse(GetTagValue(seq, option), out num) == true)
                number = num;
            else
                number = null;

            return number;
        }

        /// <summary>
        /// Parse a pipe delimited SWIFT message
        /// </summary>
        /// <param name="message"></param>
        protected virtual void ParsePipeMsg(string message)
        {

        }

        protected virtual string GeDetailtXML()
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

        #region GET TAG FUNCTIONS
        /// <summary>
        /// getT14D
        /// 
        /// Returns the value of tag 14D
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT14D(List<TagData<string, string, string, string, int>> seq)
        {
            return GetTagValue(seq, "14D");
        }

        /// <summary>
        /// getT17R
        /// 
        /// Returns the value in tag 17R
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT17R(List<TagData<string, string, string, string, int>> seq)
        {
            return GetTagValue(seq, "17R");
        }

        /// <summary>
        /// getInt
        /// 
        /// Returns the value in tag 18A
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public Nullable<int> getInt(List<TagData<string, string, string, string, int>> seq, string option)
        {
            Nullable<int> number = null;
            int num;

            if (int.TryParse(GetTagValue(seq, option), out num) == true)
                number = num;
            else
                number = null;

            return number;
        }

        /// <summary>
        /// getT20
        /// 
        /// Returns the value in tag 20.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT20(List<TagData<string, string, string, string, int>> seq)
        {
            return GetTagValue(seq, "20");
        }

        /// <summary>
        /// getT21
        /// 
        /// Returns the value in tag 21.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT21(List<TagData<string, string, string, string, int>> seq)
        {
            return GetTagValue(seq, "21");
        }

        /// <summary>
        /// getT21G
        /// 
        /// Returns the value in tag 21G.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT21G(List<TagData<string, string, string, string, int>> seq)
        {
            return GetTagValue(seq, "21G");
        }

        /// <summary>
        /// getT21N
        /// 
        /// Returns the value of tag 21N.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT21N(List<TagData<string, string, string, string, int>> seq)
        {
            return GetTagValue(seq, "21N");
        }

        /// <summary>
        /// getT22A
        /// 
        /// Returns the value of tag 22A
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT22A(List<TagData<string, string, string, string, int>> seq)
        {
            return GetTagValue(seq, "22A");
        }

        /// <summary>
        /// getT22B
        /// 
        /// Returns the value of Tag 22B
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT22B(List<TagData<string, string, string, string, int>> seq)
        {
            return GetTagValue(seq, "22B");
        }

        /// <summary>
        /// getT22C
        /// 
        /// Returns the value 0f tag 22c.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT22C(List<TagData<string, string, string, string, int>> seq)
        {
            return GetTagValue(seq, "22C");
        }

        /// <summary>
        /// getT26H
        /// 
        /// Returns the value of tag 26H
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT26H(List<TagData<string, string, string, string, int>> seq)
        {
            return GetTagValue(seq, "26H");
        }

        /// <summary>
        /// getT29A
        /// 
        /// Returns the value 0f 29A
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT29A(List<TagData<string, string, string, string, int>> seq)
        {
            return GetTagValue(seq, "29A");
        }

        /// <summary>
        /// getT30
        /// 
        /// Returns the value of tag 30
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT30(List<TagData<string, string, string, string, int>> seq)
        {
            return GetTagValue(seq, "30");
        }

        /// <summary>
        /// getT30F
        /// 
        /// Returns the value of tag 30F
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT30F(List<TagData<string, string, string, string, int>> seq)
        {
            return GetTagValue(seq, "30F");
        }

        /// <summary>
        /// getT30P
        /// 
        /// Returns the value of tag 30P
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT30P(List<TagData<string, string, string, string, int>> seq)
        {
            return GetTagValue(seq, "30P");
        }

        /// <summary>
        /// getT30T
        /// 
        /// Returns the value of 30T
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT30T(List<TagData<string, string, string, string, int>> seq)
        {
            return GetTagValue(seq, "30T");
        }

        /// <summary>
        /// getT30V
        /// 
        /// Returns the value of tag 30V
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT30V(List<TagData<string, string, string, string, int>> seq)
        {
            return GetTagValue(seq, "30V");
        }

        /// <summary>
        /// getT30X
        /// 
        /// Returns the value of tag 30X
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT30X(List<TagData<string, string, string, string, int>> seq)
        {
            return GetTagValue(seq, "30X");
        }

        /// <summary>
        /// getT72
        /// 
        /// Returns the value in tag 72
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT72(List<TagData<string, string, string, string, int>> seq)
        {
            return GetTagValue(seq, "72");
        }

        /// <summary>
        /// getT76
        /// 
        /// Returns the value of tag 76
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT76(List<TagData<string, string, string, string, int>> seq)
        {
            return GetTagValue(seq, "76");
        }

        /// <summary>
        /// getT75
        /// 
        /// Returns the value of tag 75
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT75(List<TagData<string, string, string, string, int>> seq)
        {
            return GetTagValue(seq, "75");
        }

        /// <summary>
        /// getT77D
        /// 
        /// Returns the value of tag 77D
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT77D(List<TagData<string, string, string, string, int>> seq)
        {
            return GetTagValue(seq, "77D");
        }

        /// <summary>
        /// getT94A
        /// 
        /// Returns the value of tag 94A
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT94A(List<TagData<string, string, string, string, int>> seq)
        {
            return GetTagValue(seq, "94A");
        }

        #endregion

        #region VALIDATE TAG FUNCTIONS
        /// <summary>
        /// Is_T12_Valid
        /// 
        /// Validate tag 12
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T12_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            Util util = new Util();
            int val = 0;

            // 30T is a mandatory field in a mandatory block.
            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("12") == true)
                {
                    if (field.Present == 0)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                    }
                    field.Value = field.Value.Trim();
                    if (field.Value.Length != 3)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + " - Incorrect field length : " + field.Value.Length);
                    }
                    int.TryParse(field.Value, out val);
                    if (val < 100 || val > 999)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + " - Incorrect field value : must be between 100 - 999 inclusive");
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T12_Valid");
                }
            }
            else
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T14D_Valid
        /// 
        /// Validate tag 14D
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T14D_Valid(TagData<string, string, string, string, int> field)
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
                    if ((field.Value.Equals("30E/360") == false) && (field.Value.Equals("360/360") == false) && (field.Value.Equals("ACT/360") == false) &&
                        (field.Value.Equals("ACT/365") == false) && (field.Value.Equals("AFI/365") == false))
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
        /// Is_T15A_Valid
        /// 
        /// Validate tag 15A
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T15A_Valid(TagData<string, string, string, string, int> field)
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
        /// Is_T15B_Valid
        /// 
        /// Validate tag 15B
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T15B_Valid(TagData<string, string, string, string, int> field)
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
        /// Is_T15C_Valid
        /// 
        /// Validate tag 15C
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T15C_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            // 15C is a mandatory field in a mandatory block. It must be present
            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("15C") == true)
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
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T15C_Valid");
                }
            }
            else
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T15D_Valid
        /// 
        /// Validate tag 15D
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T15D_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            // 15D is a mandatory field in a mandatory block. It must be present
            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("15D") == true)
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
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T15D_Valid");
                }
            }
            else
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T15E_Valid
        /// 
        /// Validate tag 15E
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T15E_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            // 15C is a mandatory field in a mandatory block. It must be present
            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("15E") == true)
                {
                    if (field.Present == 0)
                    {
                        if (isAnyTagPresentInSequence(getSequence("E")) == true)
                        {
                            valid = false;
                            Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                        }
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
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T15E_Valid");
                }
            }
            else
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T15F_Valid
        /// 
        /// Validate tag 15F
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T15F_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            // 15C is a mandatory field in a mandatory block. It must be present
            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("15F") == true)
                {
                    if (field.Present == 0)
                    {
                        if (isAnyTagPresentInSequence(getSequence("F")) == true)
                        {
                            valid = false;
                            Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                        }
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
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T15F_Valid");
                }
            }
            else
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T15G_Valid
        /// 
        /// Validate tag 15G
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T15G_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            // 15A is a mandatory field in a mandatory block. It must be present
            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("15G") == true)
                {
                    if (field.Present == 0)
                    {
                        if (isAnyTagPresentInSequence(getSequence("G")) == true)
                        {
                            valid = false;
                            Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                        }
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
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T15G_Valid");
                }
            }
            else
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T15H_Valid
        /// 
        /// Validate tag 15H
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T15H_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            // 15A is a mandatory field in a mandatory block. It must be present
            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("15H") == true)
                {
                    if (field.Present == 0)
                    {
                        if (isAnyTagPresentInSequence(getSequence("H")) == true)
                        {
                            valid = false;
                            Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                        }
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
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T15H_Valid");
                }
            }
            else
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T15I_Valid
        /// 
        /// Validate tag 15I
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T15I_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            // 15A is a mandatory field in an optional block. It must be present
            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("15I") == true)
                {
                    if (field.Present == 0)
                    {
                        if (isAnyTagPresentInSequence(getSequence("I")) == true)
                        {
                            valid = false;
                            Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                        }
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
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T15I_Valid");
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
        /// 
        /// Validate tag 17R
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T17R_Valid(TagData<string, string, string, string, int> field)
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
        /// Is_T18A_Valid
        /// 
        /// Validate tag 18A
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T18A_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            Util util = new Util();
            int num = 0;

            // 18A is a mandatory field in a optional block.
            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("18A") == true)
                {
                    field.Value = field.Value.Trim();
                    if (field.Value.Length > 5)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + " - Incorrect field length : " + field.Value.Length);
                    }
                    if (int.TryParse(field.Value, out num) == false)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + " - is not an integer value");
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T18A_Valid");
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
        /// 
        /// Validate tag 20
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T20_Valid(TagData<string, string, string, string, int> field)
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
                    else
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
        /// 
        /// Validate tag 21
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T21_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("21") == true)
                {
                    if (field.Present == 0)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                    }
                    else
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
        /// Is_T21F_Valid
        /// 
        /// Validate tag 21F
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T21F_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("21F") == true)
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
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T21F_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T21G_Valid
        /// 
        /// Validate tag 21G
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T21G_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 21G is NOT a mandatory field.
                if (field.Tag.Equals("21G") == true)
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
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T21G_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T21N_Valid
        /// 
        /// Validate tag 21N
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T21N_Valid(TagData<string, string, string, string, int> field)
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
        /// Is_T21R_Valid
        /// 
        /// Validate tag 21R
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T21R_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 21G is NOT a mandatory field.
                if (field.Tag.Equals("21R") == true)
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
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T21R_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T22A_Valid
        /// 
        /// Validate tag 22A
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T22A_Valid(TagData<string, string, string, string, int> field)
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
                    else
                    {
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
                        if ((field.Value.Contains("AMND") == false) && (field.Value.Contains("CANC") == false) &&
                             (field.Value.Contains("DUPL") == false) && (field.Value.Contains("NEWT") == false))
                        {
                            valid = false;
                            Anomalies.Add("ERROR T36 - Tag " + field.Tag + "," + field.Name + ", field must contain one of the following: AMND, CANC, DUPL or NEWT");
                        }
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
        /// Is_T22B_Valid
        /// 
        /// Validate tag 22B
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T22B_Valid(TagData<string, string, string, string, int> field)
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
                    else
                    {
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
        /// 
        /// Validate tag 22C
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T22C_Valid(TagData<string, string, string, string, int> field)
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
                    else
                    {
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
        /// Is_T23E_Valid
        /// 
        /// Validate tag 23E
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T23E_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            string code = null;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("23E") == true)
                {
                    field.Value = field.Value.Trim();
                    if (field.Value.Length > 35)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 35 characters.");
                    }

                    code = field.Value.Substring(0, 4);
                    switch(code)
                    {
                        case "CHQB":
                        case "CMSW":
                        case "CMTO":
                        case "CMZB":
                        case "CORT":
                        case "EQUI":
                        case "INTC":
                        case "NETS":
                        case "OTHR":
                        case "PHON":
                        case "REPA":
                        case "RTGS":
                        case "URGP":
                            break;
                        default:
                            Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", invalid code : " + code);
                            break;
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T23E_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T24D_Valid
        /// 
        /// Validate tag 24D
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T24D_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            string code = null;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 24D is NOT a mandatory field.
                if (field.Tag.Equals("24D") == true)
                {
                    if (field.Present == 1)
                    {
                        field.Value = field.Value.Trim();
                        if (field.Value.Length > 40)
                        {
                            valid = false;
                            Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 40 characters.");
                        }

                        code = field.Value.Substring(0, 4);
                        if ((code.Contains("BROK") == false) && (code.Contains("ELEC") == false) && (code.Contains("PHON") == false))
                        {
                            valid = false;
                            Anomalies.Add("ERROR T68 - Tag " + field.Tag + "," + field.Name + ", field must contain one of the following: BROK, ELEC, or PHON");
                        }
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T24D_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T25_Valid
        /// 
        /// Validate tag 25
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T25_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if ((field.Tag.Equals("25") == true && field.Present == 1) || (field.Tag.Equals("25A") == true && field.Present == 1))
                {
                    field.Value = field.Value.Trim();
                    if (field.Value.Length > 35)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 35 characters.");
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T25_Valid");
                }
            }
            else if (field.Mandatory.Equals("M") == true)
            {
                Anomalies.Add("NOTICE: Mandatory Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T26H_Valid
        /// 
        /// Validate tag 26H
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T26H_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 21G is NOT a mandatory field.
                if (field.Tag.Equals("26H") == true)
                {
                    if (field.Present == 1)
                    {
                        field.Value = field.Value.Trim();
                        if (field.Value.Length > 16)
                        {
                            valid = false;
                            Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 16 characters.");
                        }
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T26H_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T27_Valid
        /// 
        /// Validate tag 27
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T27_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            Util util = new Util();
            int val1 = 0;
            int val2 = 0;

            // 30T is a mandatory field in a mandatory block.
            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("27") == true)
                {
                    if (field.Present == 0)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                    }
                    field.Value = field.Value.Trim();
                    if (field.Value.Length == 3)
                    {
                        int.TryParse(field.Value.Substring(0, 1), out val1);
                        int.TryParse(field.Value.Substring(2, 1), out val2);
                        if (val1 < 1 || val1 > 9)
                        {
                            valid = false;
                            Anomalies.Add("ERROR - Tag " + field.Tag + " - Incorrect Message Number - must be between 1 and 9");
                        }
                        if (val2 < 1 || val2 > 9)
                        {
                            valid = false;
                            Anomalies.Add("ERROR - Tag " + field.Tag + " - Incorrect Sequence Number - must be between 1 and 9");
                        }
                    }
                    else
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + " - Incorrect field length : " + field.Value.Length);
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T12_Valid");
                }
            }
            else
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T28D_Valid
        /// 
        /// Validate tag 28D
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T28D_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            int val1 = 0;
            int val2 = 0;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("28D") == true)
                {
                    if (field.Present == 0)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                    }
                    field.Value = field.Value.Trim();
                    char[] separator = new char[] { '/' };
                    string[] nums = field.Value.Split(separator, StringSplitOptions.None);

                    if (nums.Length != 2)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + " - Incorrect number of values - must be 2");
                    }
                    else
                    {
                        int.TryParse(nums[0], out val1);
                        int.TryParse(nums[1], out val2);
                        if (val1 < 1 || val1 > 99999)
                        {
                            valid = false;
                            Anomalies.Add("ERROR - Tag " + field.Tag + " - Incorrect Message Index - must be between 1 and 99999");
                        }
                        if (val2 < 1 || val2 > 99999)
                        {
                            valid = false;
                            Anomalies.Add("ERROR - Tag " + field.Tag + " - Incorrect Message Total - must be between 1 and 99999");
                        }
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T28D_Valid");
                }
            }
            else
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T29A_Valid
        /// 
        /// Validate tag 29A
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T29A_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            string input = field.Value;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 21G is NOT a mandatory field.
                if (field.Tag.Equals("29A") == true)
                {
                    if (field.Present == 1)
                    {
                        if (field.Value.Length > 144)
                        {
                            valid = false;
                            Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 144 characters.");
                        }

                        if (field.Value.Substring(field.Value.Length - 2, 2).Equals("\r\n") == true)
                            input = field.Value.Substring(0, field.Value.Length - 2);
                        string[] stringSeparators = new string[] { "\r\n" };
                        string[] lines = input.Split(stringSeparators, StringSplitOptions.None);
                        if (lines.Length > 4)
                        {
                            valid = false;
                            Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", Maximum number of lines is 4.");
                        }
                        foreach (string ln in lines)
                        {
                            if (ln.Length > 35)
                            {
                                valid = false;
                                Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", Maximum number of characters per line is 35.");
                            }
                        }
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T29A_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T30_Valid
        /// 
        /// Validate tags 30, 30F, 30P, 30T, 30V and 30X
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T30_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            Util util = new Util();

            // 30, 30F, 30P, 30T and 30V are mandatory fields 30X is an optional field
            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if ((field.Tag.Equals("30F") == true) || (field.Tag.Equals("30P") == true) || (field.Tag.Equals("30T") == true) || (field.Tag.Equals("30V") == true))
                {
                    if (field.Present == 0)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                    }
                    else
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
                }
                else if (field.Tag.Equals("30") == true)
                {
                    if (field.Present == 0)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                    }
                    else
                    {
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
                } else if (field.Tag.Equals("30X") == true)
                {
                    if (field.Present == 1)
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
        /// Is_T32A_Valid
        /// 
        /// Validate tag 32A
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T32A_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            Util util = new Util();
            string strValue = null;
            double amt = 0.0;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("32A") == true)
                {
                    if (field.Present == 0)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                    }
                    else
                    {
                        strValue = field.Value;
                        if (field.Value.Length > 24)
                        {
                            valid = false;
                            Anomalies.Add("ERROR - Tag " + field.Tag + " - Incorrect field length : " + strValue.Length);
                        }

                        string date = strValue.Substring(0, 6);
                        if (util.IsDate(date) == false)
                            Anomalies.Add("ERROR - Tag " + field.Tag + " - Incorrect date format ");

                        strValue = strValue.Substring(6, strValue.Length - 6);
                        strValue = strValue.Trim();
                        string ccy = strValue.Substring(0, 3);
                        if (util.IsValidCcy(ccy) == false)
                        {
                            valid = false;
                            Anomalies.Add("ERROR T52 - Tag " + field.Tag + " - Invalid currency : " + ccy);
                        }

                        if(double.TryParse(strValue.Substring(3,strValue.Length-3), out amt) == false)
                        {
                            valid = false;
                            Anomalies.Add("ERROR T52 - Tag " + field.Tag + " - Invalid amount ");
                        }
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
        /// Is_T32B_Valid
        /// 
        /// Validate tag 32B
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T32B_Valid(TagData<string, string, string, string, int> field)
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
        /// Is_T32H_Valid
        /// 
        /// Validate tag 32H
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T32H_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            Util util = new Util();
            string ccy = null;

            // 32H is NOT a mandatory field in a mandatory block.
            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("32H") == true)
                {
                    field.Value = field.Value.Trim();
                    if (field.Value.Length > 19)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + " - Incorrect field length : " + field.Value.Length);
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T32H_Valid");
                }

                if (field.Value.Substring(0, 1).Equals("N") == true)
                    ccy = field.Value.Substring(1, 3);
                else
                    ccy = field.Value.Substring(0, 3);

                if (util.IsValidCcy(ccy) == false)
                {
                    valid = false;
                    Anomalies.Add("ERROR T52 - Tag " + field.Tag + " - Invalid currency : " + ccy);
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T33B_Valid
        /// 
        /// Validate tag 33B
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T33B_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            Util util = new Util();
            double num = 0.0;

            // 33B is a mandatory field in an optional block.
            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("33B") == true)
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
                    if (util.IsValidCcy(ccy) == false)
                    {
                        valid = false;
                        Anomalies.Add("ERROR T52 - Tag " + field.Tag + " - Invalid currency : " + ccy);
                    }

                    string amount = field.Value.Substring(3, field.Value.Length - 3);
                    if (amount.Length > 15 || amount.Length < 2)
                    {
                        valid = false;
                        Anomalies.Add("ERROR T40 - Tag " + field.Tag + "," + field.Name + ", this field must contain at least 1 digit and a decomal point or comma and no more than 12 characters.");
                    }


                    if (amount.Contains(".") == false && amount.Contains(",") == false)
                    {
                        valid = false;
                        Anomalies.Add("ERROR T43 - Tag " + field.Tag + "," + field.Name + ", this field must contain a decomal point or comma.");
                    }

                    if (double.TryParse(amount, out num) == false)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ",this is not a valid numeric value.");
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T33B_Valid");
                }
            }
            else if (field.Mandatory.Equals("M") == true)
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T33E_Valid
        /// 
        /// Validate tag 33E
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T33E_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            Util util = new Util();
            double num = 0.0;

            // 33B is a mandatory field in an optional block.
            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("33E") == true)
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
                    if (util.IsValidCcy(ccy) == false)
                    {
                        valid = false;
                        Anomalies.Add("ERROR T52 - Tag " + field.Tag + " - Invalid currency : " + ccy);
                    }

                    string amount = field.Value.Substring(3, field.Value.Length - 3);
                    if (amount.Length > 15 || amount.Length < 2)
                    {
                        valid = false;
                        Anomalies.Add("ERROR T40 - Tag " + field.Tag + "," + field.Name + ", this field must contain at least 1 digit and a decomal point or comma and no more than 12 characters.");
                    }


                    if (amount.Contains(".") == false && amount.Contains(",") == false)
                    {
                        valid = false;
                        Anomalies.Add("ERROR T43 - Tag " + field.Tag + "," + field.Name + ", this field must contain a decomal point or comma.");
                    }

                    if (double.TryParse(amount, out num) == false)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ",this is not a valid numeric value.");
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T33E_Valid");
                }
            }
            else
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T34C_Valid
        /// 
        /// Validate tag 34C
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T34C_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 24D is NOT a mandatory field.
                if (field.Tag.Equals("34C") == true)
                {
                    if (field.Present == 1)
                    {
                        field.Value = field.Value.Trim();
                        if (field.Value.Length > 23)
                        {
                            valid = false;
                            Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 23 characters.");
                        }
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T34C_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T34E_Valid
        /// 
        /// Validate tag 34E
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T34E_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            Util util = new Util();

            // 32H is NOT a mandatory field in a mandatory block.
            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("34E") == true)
                {
                    field.Value = field.Value.Trim();
                    if (field.Value.Length > 19)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + " - Incorrect field length : " + field.Value.Length);
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T34E_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T36_Valid
        /// 
        /// Validate tag 36
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T36_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            double num = 0.0;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 20 is a mandatory field in a mandatory block. It must be present
                if (field.Tag.Equals("36") == true)
                {
                    if (field.Present == 0)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                    }
                    field.Value = field.Value.Trim();
                    if (field.Value.Length > 12 || field.Value.Length < 2)
                    {
                        valid = false;
                        Anomalies.Add("ERROR T40 - Tag " + field.Tag + "," + field.Name + ", this field must contain at least 1 digit and a decomal point or comma and no more than 12 characters.");
                    }
                    if (field.Value.Contains(".") == false && field.Value.Contains(",") == false)
                    {
                        valid = false;
                        Anomalies.Add("ERROR T43 - Tag " + field.Tag + "," + field.Name + ", this field must contain a decomal point or comma.");
                    }
                    if (double.TryParse(field.Value, out num) == false)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ",this is not a valid numeric value.");
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T36_Valid");
                }
            }
            else if (field.Mandatory.Equals("M") == true)
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T37G_Valid
        /// 
        /// Validate tag 37G
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T37G_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            Util util = new Util();
            double num = 0.0;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("37G") == true)
                {
                    field.Value = field.Value.Trim();
                    if (field.Value.Length > 13)
                    {
                        valid = false;
                        Anomalies.Add("ERROR T40 - Tag " + field.Tag + " - Incorrect field length : " + field.Value.Length);
                    }
                    else if (field.Value.Length == 13)
                    {
                        if (field.Value.Substring(0, 1).Equals("N") == false)
                        {
                            valid = false;
                            Anomalies.Add("ERROR - Tag " + field.Tag + " - Sign must be present for a negative rate. ");
                        }
                        else if (Double.TryParse(field.Value.Substring(1, field.Value.Length), out num) == false)
                        {
                            valid = false;
                            Anomalies.Add("ERROR - Tag " + field.Tag + " - rate is not a double. ");
                        }
                    }
                    else if (Double.TryParse(field.Value.Substring(0, field.Value.Length), out num) == false)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + " - rate is not a double. ");
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T37G_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T37L_Valid
        /// 
        /// Validate tag 37L
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T37L_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            double num = 0.0;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 20 is a mandatory field in a mandatory block. It must be present
                if (field.Tag.Equals("37L") == true)
                {
                    if (field.Present == 0)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - MANDATORY Tag " + field.Tag + "," + field.Name + ", is not present in message");
                    }
                    field.Value = field.Value.Trim();
                    if (field.Value.Length > 12 || field.Value.Length < 2)
                    {
                        valid = false;
                        Anomalies.Add("ERROR T40 - Tag " + field.Tag + "," + field.Name + ", this field must contain at least 1 digit and a decomal point or comma and no more than 12 characters.");
                    }
                    if (field.Value.Contains(".") == false && field.Value.Contains(",") == false)
                    {
                        valid = false;
                        Anomalies.Add("ERROR T43 - Tag " + field.Tag + "," + field.Name + ", this field must contain a decomal point or comma.");
                    }
                    if (double.TryParse(field.Value, out num) == false)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ",this is not a valid numeric value.");
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T37L_Valid");
                }
            }
            else
            {
                Anomalies.Add("NOTICE: Tag " + field.Tag + " was not present in message - not validated.");
            }

            return valid;
        }

        /// <summary>
        /// Is_T38J_Valid
        /// 
        /// Validate tag 38J
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T38J_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            Util util = new Util();
            //double num = 0.0;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("38J") == true)
                {
                    field.Value = field.Value.Trim();
                    if (field.Value.Length != 4)
                    {
                        valid = false;
                        Anomalies.Add("ERROR T40 - Tag " + field.Tag + " - Incorrect field length : " + field.Value.Length);
                    }
                    else if (!field.Value.Substring(0, 1).Equals("D") || !field.Value.Substring(0, 1).Equals("M"))
                    {
                        valid = false;
                        Anomalies.Add("ERROR T61 - Tag " + field.Tag + " - Indicator must be a D or M. ");
                    }
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T39M_Valid
        /// 
        /// Validated tag 39M
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T39M_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            Util util = new Util();

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("39M") == true)
                {
                    field.Value = field.Value.Trim();
                    if (field.Value.Length != 2)
                    {
                        valid = false;
                        Anomalies.Add("ERROR T40 - Tag " + field.Tag + " - Incorrect field length : " + field.Value.Length);
                    }

                    if (util.isValidISOCountryCode(field.Value) == false)
                    {
                        valid = false;
                        Anomalies.Add("ERROR T73 - Tag " + field.Tag + " - " + field.Value + " - Country Code : is not a valid ISO country code. ");
                    }

                }
            }
            return valid;
        }

        /// <summary>
        /// Is_T50_Valid
        /// 
        /// Validate tags 50A, 50C, 50F, 50G, 50H, 50K and 50L
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T50_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                field.Value = field.Value.Trim();
                if ((field.Tag.Equals("50A") == true) && (field.Present == 1))
                {
                    if (field.Value.Length > 48)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 48 characters.");
                    }
                }
                else if ((field.Tag.Equals("50C") == true) && (field.Present == 1))
                {
                    if (field.Value.Length > 11)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 11 characters.");
                    }
                }
                else if ((field.Tag.Equals("50F") == true) && (field.Present == 1))
                {
                    if (field.Value.Length > 179)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 179 characters.");
                    }
                }
                else if ((field.Tag.Equals("50G") == true) && (field.Present == 1))
                {
                    if (field.Value.Length > 46)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 46 characters.");
                    }
                }
                else if ((field.Tag.Equals("50H") == true) && (field.Present == 1) || (field.Tag.Equals("50K") == true) && (field.Present == 1))
                {
                    if (field.Value.Length > 183)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 183 characters.");
                    }
                }
                else if ((field.Tag.Equals("50L") == true) && (field.Present == 1))
                {
                    if (field.Value.Length > 35)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 35 characters.");
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T50_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T51A_Valid
        /// 
        /// Validate tags 51A
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T51A_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                field.Value = field.Value.Trim();
                if ((field.Tag.Equals("51A") == true) && (field.Present == 1))
                {
                    if (field.Value.Length > 50)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 50 characters.");
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T51A_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T52_Valid
        /// 
        /// Validate tags 52A, 52B, 52C and 52D
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T52_Valid(TagData<string, string, string, string, int> field)
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
                else if ((field.Tag.Equals("52C") == true) && (field.Present == 1))
                {
                    if (field.Value.Length > 35)
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
        /// Is_T53_Valid
        /// 
        /// Validate tag 53
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T53_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                field.Value = field.Value.Trim();
                if ((field.Tag.Equals("53A") == true) && (field.Present == 1))
                {
                    if (field.Value.Length > 48)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 48 characters.");
                    }
                }
                else if ((field.Tag.Equals("53B") == true) && (field.Present == 1))
                {
                    if (field.Value.Length > 72)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 72 characters.");
                    }
                }
                else if ((field.Tag.Equals("53D") == true) && (field.Present == 1))
                {
                    if (field.Value.Length > 183)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 183 characters.");
                    }
                }
                else if ((field.Tag.Equals("53J") == true) && (field.Present == 1))
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
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T53_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T54_Valid
        /// 
        /// Validate tags 54A, 54B and 54D
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T54_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 52A / 52B / 52D is an optional field.
                field.Value = field.Value.Trim();
                if ((field.Tag.Equals("54A") == true) && (field.Present == 1))
                {
                    if (field.Value.Length > 48)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 48 characters.");
                    }
                }
                else if ((field.Tag.Equals("54B") == true) && (field.Present == 1))
                {
                    if (field.Value.Length > 72)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 72 characters.");
                    }
                }
                else if ((field.Tag.Equals("54D") == true) && (field.Present == 1))
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
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T54_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T56_Valid
        /// 
        /// Validate tag 56
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T56_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 56A / 56D / 56J is an optional field.
                field.Value = field.Value.Trim();
                if ((field.Tag.Equals("56A") == true) && (field.Present == 1))
                {
                    if (field.Value.Length > 50)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 50 characters.");
                    }
                }
                else if ((field.Tag.Equals("56C") == true) && (field.Present == 1))
                {
                    if (field.Value.Length > 35)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 35 characters.");
                    }
                }
                else if ((field.Tag.Equals("56D") == true) && (field.Present == 1))
                {
                    if (field.Value.Length > 186)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 186 characters.");
                    }
                }
                else if ((field.Tag.Equals("56J") == true) && (field.Present == 1))
                {
                    if (field.Value.Length > 210)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 210 characters.");
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T56_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T57_Valid
        /// 
        /// Validate tag 57
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T57_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 57A / 57D / 57J is an optional field.
                field.Value = field.Value.Trim();
                if ((field.Tag.Equals("57A") == true) && (field.Present == 1))
                {
                    if (field.Value.Length > 50)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 50 characters.");
                    }
                }
                else if ((field.Tag.Equals("57C") == true) && (field.Present == 1))
                {
                    if (field.Value.Length > 35)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 35 characters.");
                    }
                }
                else if ((field.Tag.Equals("57D") == true) && (field.Present == 1))
                {
                    if (field.Value.Length > 186)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 186 characters.");
                    }
                }
                else if ((field.Tag.Equals("57J") == true) && (field.Present == 1))
                {
                    if (field.Value.Length > 210)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 210 characters.");
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T57_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T58_Valid
        /// 
        /// Validate tag 58
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T58_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 58A / 58D / 58J is an optional field.
                if ((field.Tag.Equals("58A") == true) || (field.Tag.Equals("58D") == true) || (field.Tag.Equals("58J") == true))
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
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T58_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T59_Valid
        /// 
        /// Validate tag 59
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T59_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 21 is NOT a mandatory field.
                if ((field.Tag.Equals("59") == true) && (field.Present == 1))
                {
                    field.Value = field.Value.Trim();
                    if (field.Value.Length > 174)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 174 characters.");
                    }
                }
                else if ((field.Tag.Equals("59A") == true) && (field.Present == 1))
                {
                    field.Value = field.Value.Trim();
                    if (field.Value.Length > 48)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 48 characters.");
                    }
                }
                else if ((field.Tag.Equals("59F") == true) && (field.Present == 1))
                {
                    field.Value = field.Value.Trim();
                    if (field.Value.Length > 181)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 181 characters.");
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
        /// Is_T70_Valid
        /// 
        /// Validate tag 70
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T70_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 21 is NOT a mandatory field.
                if ((field.Tag.Equals("70") == true) && (field.Present == 1))
                {
                    field.Value = field.Value.Trim();
                    if (field.Value.Length > 148)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 148 characters.");
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T70_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T71A_Valid
        /// 
        /// Validate tag 71A
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T71A_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if ((field.Tag.Equals("71A") == true && field.Present == 1))
                {
                    field.Value = field.Value.Trim();
                    if (field.Value.Length > 3)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 3 characters.");
                    }

                    switch(field.Value)
                    {
                        case "BEN":
                        case "OUR":
                        case "SHA":
                            break;
                        default:
                            valid = false;
                            Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + " , " + field.Value + ", is not a valid code.");
                            break;
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T71A_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T71F_Valid
        /// 
        /// Validate tag 71F
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T71F_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            string ccy = null;
            Util u = new Util();
            double num = 0.0;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 24D is NOT a mandatory field.
                if (field.Tag.Equals("71F") == true)
                {
                    if (field.Present == 1)
                    {
                        field.Value = field.Value.Trim();
                        if (field.Value.Length > 18)
                        {
                            valid = false;
                            Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 18 characters.");
                        }

                        ccy = field.Value.Substring(0, 3);
                        if ((u.IsValidCcy(ccy) == false))
                        {
                            valid = false;
                            Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", field must contain a valid ISO currency.");
                        }

                        if (Double.TryParse(field.Value.Substring(3, field.Value.Length - 3), out num) == false)
                        {
                            valid = false;
                            Anomalies.Add("ERROR - Tag " + field.Tag + " - amount is not a double. ");
                        }
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T71F_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T72_Valid
        /// 
        /// Validate tag 72
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T72_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;
            string input = field.Value;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 72 is NOT a mandatory field.
                if (field.Tag.Equals("72") == true)
                {
                    if (field.Present == 1)
                    {
                        // validation code
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T72_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T75_Valid
        /// 
        /// Validate tag 75
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T75_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
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

        /// <summary>
        /// Is_T76_Valid
        /// 
        /// Validate tag 76
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T76_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 21 is NOT a mandatory field.
                if (field.Tag.Equals("76") == true)
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
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T76_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T77_Valid
        /// 
        /// Validate tag 77B and 77D
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T77_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                if (field.Tag.Equals("77B") == true && field.Present == 1)
                {
                    field.Value = field.Value.Trim();
                    if (field.Value.Length > 111)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + " value to long");
                    }
                }
                else if (field.Tag.Equals("77D") == true && field.Present == 1)
                {
                    field.Value = field.Value.Trim();
                    if (field.Value.Length > 210)
                    {
                        valid = false;
                        Anomalies.Add("ERROR - Tag " + field.Tag + " value to long");
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T77_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T77F_Valid
        /// 
        /// VAlidate tag 77F
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T77F_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 21 is NOT a mandatory field.
                if (field.Tag.Equals("77F") == true)
                {
                    if (field.Present == 1)
                    {
                        field.Value = field.Value.Trim();
                        if (field.Value.Length > 1800)
                        {
                            valid = false;
                            Anomalies.Add("ERROR - Tag " + field.Tag + "," + field.Name + ", is greater than 1800 characters.");
                        }
                    }
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T77F_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T82_Valid
        /// 
        /// Validate tags 82A, 82D and 82J
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T82_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 82A / 82D / 82J is a mandatory field.
                if ((field.Tag.Equals("82A") == true) || (field.Tag.Equals("82D") == true) || (field.Tag.Equals("82J") == true))
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
        /// Is_T83_Valid
        /// 
        /// Validate tag 83
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T83_Valid(TagData<string, string, string, string, int> field)
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
        /// Is_T84_Valid
        /// 
        /// Validate tag 84
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T84_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 84A / 84B / 84D / 84J is an optional field.
                if ((field.Tag.Equals("84A") == true) || (field.Tag.Equals("84B") == true) || (field.Tag.Equals("84D") == true) || (field.Tag.Equals("84J") == true))
                {
                    // Validation Code
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T84_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T85_Valid
        /// 
        /// VAlidate tag 85
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T85_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 85A / 85B / 85D / 85J is an optional field.
                if ((field.Tag.Equals("85A") == true) || (field.Tag.Equals("85B") == true) || (field.Tag.Equals("85D") == true) || (field.Tag.Equals("85J") == true))
                {
                    // Validation Code
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T85_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T86_Valid
        /// 
        /// Validate tag 86
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T86_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 86A / 86D / 86J is an optional field.
                if ((field.Tag.Equals("86A") == true) || (field.Tag.Equals("86D") == true) || (field.Tag.Equals("86J") == true))
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
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T86_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T87_Valid
        /// 
        /// Validate tags 87A, 87D and 87J
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T87_Valid(TagData<string, string, string, string, int> field)
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
        /// Is_T88_Valid
        /// 
        /// Validate tag 88
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T88_Valid(TagData<string, string, string, string, int> field)
        {
            bool valid = true;

            if (field.Mandatory.Equals("M") || (field.Mandatory.Equals("O") && field.Present == 1) || (AlwaysValidateTag == true))
            {
                // 88A / 88D / 88J is an optional field.
                if ((field.Tag.Equals("88A") == true) || (field.Tag.Equals("88D") == true) || (field.Tag.Equals("88J") == true))
                {
                    // Validation Code
                }
                else
                {
                    valid = false;
                    Anomalies.Add("ERROR - Tag " + field.Tag + " was passed to Is_T88_Valid");
                }
            }

            return valid;
        }

        /// <summary>
        /// Is_T94A_Valid
        /// 
        /// Validate tag 94A
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool Is_T94A_Valid(TagData<string, string, string, string, int> field)
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

        #endregion
    }
}
