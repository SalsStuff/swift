using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
    public sealed class TagData<TFirst, TSecond, TThird, TFourth, TFifth> : IEquatable<TagData<TFirst, TSecond, TThird, TFourth, TFifth>>
    {
        private readonly TFirst TagNumber;
        private readonly TSecond TagName;
        private TThird TagValue;
        private readonly TFourth TagMandatory;
        private TFifth TagPresent;
        
        public TagData(TFirst tag, TSecond name, TThird value, TFourth mandatory, TFifth present)
        {
            this.TagNumber = tag;
            this.TagName = name;
            this.TagValue = value;
            this.TagMandatory = mandatory;
            this.TagPresent = present;
        }
        
        public TFirst Tag
        {
            get { return TagNumber; }
        }
        
        public TSecond Name
        {
            get { return TagName; }
        }
    
        public TThird Value
        {
            get { return TagValue; }
            set { this.TagValue = value; }
        }
        
        public TFourth Mandatory
        {
            get { return TagMandatory; }
        }
        
        public TFifth Present
        {
            get { return TagPresent; }
            set { this.TagPresent = value; }
        }
    
        public bool Equals(TagData<TFirst, TSecond, TThird, TFourth, TFifth> other)
        {
            if (other == null)
            {
                return false;
            }
            return EqualityComparer<TFirst>.Default.Equals(this.TagNumber, other.TagNumber) &&
                   EqualityComparer<TSecond>.Default.Equals(this.TagName, other.TagName) &&
                   EqualityComparer<TThird>.Default.Equals(this.TagValue, other.TagValue) &&
                   EqualityComparer<TFourth>.Default.Equals(this.TagMandatory, other.TagMandatory) &&
                   EqualityComparer<TFifth>.Default.Equals(this.TagPresent, other.TagPresent);
        }
    
        public override bool Equals(object o)
        {
            return Equals(o as TagData<TFirst, TSecond, TThird, TFourth, TFifth>);
        }
    
        public override int GetHashCode()
        {
            return EqualityComparer<TFirst>.Default.GetHashCode(TagNumber) * 37 +
                   EqualityComparer<TSecond>.Default.GetHashCode(TagName) * 25 +
                   EqualityComparer<TThird>.Default.GetHashCode(TagValue) * 17 +
                   EqualityComparer<TFourth>.Default.GetHashCode(TagMandatory) * 5 +
                   EqualityComparer<TFifth>.Default.GetHashCode(TagPresent);
        }
    }
}
