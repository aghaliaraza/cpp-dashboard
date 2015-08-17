using System;
using System.Collections.Generic;
using System.Linq;
using CppDashboard.DataProvider.Setup;
using CppDashboard.Models;
using CppDashboard.Extensions;

namespace CppDashboard.DataProvider
{
    public class PaymentEventsDataCanFacade : DataCanRefreshBase<PaymentEvent>, 
        IMonitoringEvents, ICanReload, ILoadVolatileData
    {
        public IEnumerable<PaymentEvent> PaymentEvents
        {
            get
            {
                return _paymentEvents;
            }
        }

        private IEnumerable<PaymentEvent> _paymentEvents;
        private readonly ConnectionCreator _connectionCreator;

        public PaymentEventsDataCanFacade()
        {
            _connectionCreator = new ConnectionCreator(Scope.Monitoring);
        }

        public void Load()
        {
            var sql = string.Format("SELECT  * FROM PaymentEvents WITH (NOLOCK) " +
                      "WHERE EventDateTime BETWEEN '{0}' AND '{1}' " +
                                    "ORDER BY EventDateTime", DateTime.Now.FormattedMins(-Constants.TimeoutDuration), DateTime.Now.Formatted());

            _paymentEvents = _connectionCreator.Exec<PaymentEvent>(sql);
        }

        protected override bool AllowedPeriod(PaymentEvent input)
        {
            return input.EventDateTime >= DateTime.Now.StartMins(Constants.TimeoutDuration) && input.EventDateTime <= DateTime.Now;
        }

        protected override IEnumerable<PaymentEvent> LoadingFrom()
        {
            var maxPrimary = _paymentEvents.Max(m => m.PaymentEventsId);

            var sql = string.Format("SELECT  * FROM PaymentEvents WITH (NOLOCK) " +
                     "WHERE PaymentEventsId >'{0}'" +
                                   "ORDER BY EventDateTime", maxPrimary);

            var data = _connectionCreator.Exec<PaymentEvent>(sql);

            return data;
        }

        public void Reload()
        {
            Refresh(ref _paymentEvents);
        }
    }
}