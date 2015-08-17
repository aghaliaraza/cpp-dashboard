using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace CppDashboard.DataProvider
{
    public abstract class DataCanRefreshBase<T> : ICanRefresh<T>
    {
        public void Refresh(ref IEnumerable<T> source)
        {
            var result = LoadingFrom();

            lock (((ICollection)source).SyncRoot)
            {
                source.AsList().AddRange(result);
            }

            source = new List<T>(source.Where(AllowedPeriod).ToList());
        }

        protected abstract bool AllowedPeriod(T input);

        protected abstract IEnumerable<T> LoadingFrom();
    }
}