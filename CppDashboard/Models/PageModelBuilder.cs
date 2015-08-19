using System;
using System.Linq;
using CppDashboard.DataProvider;
using CppDashboard.Logic.Offline;
using CppDashboard.Logic.Orphans;
using CppDashboard.Logic.Payments;
using CppDashboard.Logic.Refusals;

namespace CppDashboard.Models
{
    public class PageModelBuilder
    {
        private readonly ICancellationsDueToOrphan _cancellationsDueToOrphan;
        private readonly IPaymentsCalculator _paymentsCalculator;
        private readonly IGatewayRefusals _gatewayRefusals;
        private readonly ILoggingInfo _loggingInfo;
        private readonly SystemOnlineOrOfflineStatus _systemOnlineOrOfflineStatus;
        private readonly IMonitoringEvents _monitoringEvents;

        public PageModelBuilder(ICancellationsDueToOrphan cancellationsDueToOrphan, IPaymentsCalculator paymentsCalculator, 
            IGatewayRefusals gatewayRefusals, ILoggingInfo loggingInfo, 
            SystemOnlineOrOfflineStatus systemOnlineOrOfflineStatus, IMonitoringEvents monitoringEvents)
        {
            _cancellationsDueToOrphan = cancellationsDueToOrphan;
            _paymentsCalculator = paymentsCalculator;
            _gatewayRefusals = gatewayRefusals;
            _loggingInfo = loggingInfo;
            _systemOnlineOrOfflineStatus = systemOnlineOrOfflineStatus;
            _monitoringEvents = monitoringEvents;
        }

        public PageModel Build()
        {
            var refusals = _gatewayRefusals.GetTotal();
            var pageModel = new PageModel()
            {
                IsSystemOnline = _systemOnlineOrOfflineStatus.IsSystemOnline(),
                CancellationsDueToGhosts = _cancellationsDueToOrphan.GetTotal(),
                SuccessPayments = _paymentsCalculator.GetTotalSuccessfulPayments(),
                DeclinedPayments = _paymentsCalculator.GetTotalDeclinedPayments(),
                GatewayMkFaliures = refusals.ServiceLevelRefusals,
                AdyenMkFaliures = refusals.AdyenRefusals,
                Logs = _loggingInfo.Logs.OrderByDescending(d => d.Date).Take(100),
                Current = DateTime.Now,
                MonitoringEvents = _monitoringEvents.PaymentEvents
            };

            return pageModel;
        }
    }
}