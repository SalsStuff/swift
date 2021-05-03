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
        #endregion

        protected string whichSequence(List<TagData<string, string, string, string, int>> sequence)
        {
            string seq = null;

            foreach (TagData<string, string, string, string, int> t in sequence)
            {
                if (t.Tag.Contains("15") == true)
                {
                    seq = t.Tag.Substring(2,1);
                    break;
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
                    if (option.Contains("A") == true || option.Contains("B") == true || option.Contains("D"))
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


    }
}
