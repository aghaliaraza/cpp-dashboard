using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CppDashboard.DataProvider.Setup;
using CppDashboard.Models;

namespace CppDashboard.DataProvider
{
    public class OfflineDataFacade : ICanRefresh<OfflineConfig>, IOfflineConfigs, ICanReload, ILoadVolatileData
    {
        public IEnumerable<OfflineConfig> Configs
        {
            get
            {
                return _configs;
            }
        }

        private IEnumerable<OfflineConfig> _configs; 
        private readonly ConnectionCreator _customerPaymentsConnection;

        public OfflineDataFacade()
        {
            _customerPaymentsConnection = new ConnectionCreator(Scope.CustomerPayment);
        }

        public void Load()
        {
            var sql = string.Format("SELECT  * FROM Configuration WITH (NOLOCK) " +
                    "WHERE [Key] = '{0}' OR [Key]= '{1}'", "Offline:Status", "Offline:ManualOverrideEnabled");

            _configs = _customerPaymentsConnection.Exec<OfflineConfig>(sql);
        }

        public void Refresh(ref IEnumerable<OfflineConfig> source)
        {
            var sql = string.Format("SELECT  * FROM Configuration WITH (NOLOCK) " +
                  "WHERE [Key] = '{0}' OR [Key]= '{1}'", "Offline:Status", "Offline:ManualOverrideEnabled");

            var data = _customerPaymentsConnection.Exec<OfflineConfig>(sql);

            lock (((ICollection)_configs).SyncRoot)
            {
                _configs.ToList().Clear();
                _configs.ToList().AddRange(data);
            }
        }

        public void Reload()
        {
            Refresh(ref _configs);
        }
    }
}