using System.Collections.Generic;

namespace CppDashboard.DataProvider
{
    public interface ICanRefresh<T>
    {
        void Refresh(ref IEnumerable<T> source);
    }

    public interface ICanReload
    {
        void Reload();
    }

    public interface ILoadSystemData
    {
        void Load();
    }

    public interface ILoadVolatileData
    {
        void Load();
    }
}