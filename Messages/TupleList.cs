using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
   public class TupleList<T1, T2, T3> : List<Tuple<T1, T2, T3>>
   {
      public void Add(T1 Tag, T2 Value, T3 Mandatory)
      {
         Add(new Tuple<T1, T2, T3>(Tag, Value, Mandatory));
      }

      public void Delete()
      {

      }
   }
}
