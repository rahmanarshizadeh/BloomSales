using System.Collections.Generic;

namespace BloomSales.TestHelpers
{
    public static class Equality
    {
        public static bool AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            if (expected == null && actual != null ||
                expected != null && actual == null)
                return false;

            IEnumerator<T> it1 = expected.GetEnumerator();
            IEnumerator<T> it2 = actual.GetEnumerator();

            bool expectedMoved;
            bool actualMoved;

            while (true)
            {
                expectedMoved = it1.MoveNext();
                actualMoved = it2.MoveNext();

                if ((!expectedMoved && actualMoved) ||
                     (expectedMoved && !actualMoved))
                    return false;

                if (!expectedMoved && !actualMoved)
                    return true;

                if (!it1.Current.Equals(it2.Current))
                    return false;
            }
        }
    }
}