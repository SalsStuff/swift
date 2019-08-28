using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
   public sealed class TagData<TFirst, TSecond, TThird, TFourth> : IEquatable<TagData<TFirst, TSecond, TThird, TFourth>>
   {
      private readonly TFirst TagNumber;
      private readonly TSecond TagName;
      private TThird TagValue;
      private readonly TFourth TagMandatory;

      public TagData(TFirst tag, TSecond name, TThird value, TFourth mandatory)
      {
         this.TagNumber = tag;
         this.TagName = name;
         this.TagValue = value;
         this.TagMandatory = mandatory;
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

      public bool Equals(TagData<TFirst, TSecond, TThird, TFourth> other)
      {
         if (other == null)
         {
            return false;
         }
         return EqualityComparer<TFirst>.Default.Equals(this.TagNumber, other.TagNumber) &&
                EqualityComparer<TSecond>.Default.Equals(this.TagName, other.TagName) &&
                EqualityComparer<TThird>.Default.Equals(this.TagValue, other.TagValue) &&
                EqualityComparer<TFourth>.Default.Equals(this.TagMandatory, other.TagMandatory);
      }

      public override bool Equals(object o)
      {
         return Equals(o as TagData<TFirst, TSecond, TThird, TFourth>);
      }

      public override int GetHashCode()
      {
         return EqualityComparer<TFirst>.Default.GetHashCode(TagNumber) * 37 +
                EqualityComparer<TSecond>.Default.GetHashCode(TagName) * 25 +
                EqualityComparer<TThird>.Default.GetHashCode(TagValue) * 17 +
                EqualityComparer<TFourth>.Default.GetHashCode(TagMandatory) * 5;
      }
   }
}
