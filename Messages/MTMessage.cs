using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
    public class MTMessage
    {
        /// <summary>
        /// Get method to return the SWIFT definition of message scope
        /// </summary>
        public string Scope { get; protected set; } = "";


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
        protected void SetTagValue(List<TagData<string, string, string, string, int>> sequence, string tag, string value)
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

    }
}
