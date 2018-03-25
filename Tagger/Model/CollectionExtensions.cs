using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tagger.Model
{
    public static class CollectionExtensions
    {
        public static T[] SliceToEnd<T>(this T[] sourceArray, int startIndex)
        {
            int len = sourceArray.Length - startIndex;
            T[] resultArray = new T[len];
            Array.Copy(sourceArray, startIndex, resultArray, 0, len);
            return resultArray;
        }
    }
}
