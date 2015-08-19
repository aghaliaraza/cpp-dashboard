using System.Collections.Generic;
using System.Linq;
using CppDashboard.DataProvider;
using CppDashboard.Models;

namespace CppDashboard.Logic.Cancellations
{
    public class GetCancelledPayment
    {
        private readonly IMonitoringEvents _monitoringEvents;

        public GetCancelledPayment(IMonitoringEvents monitoringEvents)
        {
            _monitoringEvents = monitoringEvents;
        }

        public IEnumerable<PaymentEvent> GetCancellations()
        {
            return _monitoringEvents
                .PaymentEvents
                .Where(pe => pe.EventType.Equals("CancelPaymentSubmitted"));
        }
    }
}