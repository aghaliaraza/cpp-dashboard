using System;
using System.Linq;
using CppDashboard.DataProvider;
using CppDashboard.Logic.General;
using CppDashboard.Logic.Offline;
using CppDashboard.Logic.Orphans;
using CppDashboard.Logic.Payments;
using CppDashboard.Logic.Refusals;

namespace CppDashboard.Models
{
    public class PageModelBuilder
    {
        private readonly ICancellationsDueToOrphan _cancellationsDueToOrphan;
        private readonly IPspCommunicationFailures _communicationFailures;
        private readonly IPaymentsCalculator _paymentsCalculator;
        private readonly IGatewayRefusals _gatewayRefusals;
        private readonly ILoggingInfo _loggingInfo;
        private readonly SystemOnlineOrOfflineStatus _systemOnlineOrOfflineStatus;

        public PageModelBuilder(ICancellationsDueToOrphan cancellationsDueToOrphan, 
            IPspCommunicationFailures communicationFailures, IPaymentsCalculator paymentsCalculator, 
            IGatewayRefusals gatewayRefusals, ILoggingInfo loggingInfo, SystemOnlineOrOfflineStatus systemOnlineOrOfflineStatus)
        {
            _cancellationsDueToOrphan = cancellationsDueToOrphan;
            _communicationFailures = communicationFailures;
            _paymentsCalculator = paymentsCalculator;
            _gatewayRefusals = gatewayRefusals;
            _loggingInfo = loggingInfo;
            _systemOnlineOrOfflineStatus = systemOnlineOrOfflineStatus;
        }

        public PageModel Build()
        {
            var refusals = _gatewayRefusals.GetTotal();
            var pageModel = new PageModel()
            {
                IsSystemOnline = _systemOnlineOrOfflineStatus.IsSystemOnline(),
                CancellationsDueToGhosts = _cancellationsDueToOrphan.GetTotal(),
                CommsFaliures = _communicationFailures.GetTotal(),
                SuccessPayments = _paymentsCalculator.GetTotalSuccessfulPayments(),
                DeclinedPayments = _paymentsCalculator.GetTotalDeclinedPayments(),
                GatewayMkFaliures = refusals.ServiceLevelRefusals,
                AdyenMkFaliures = refusals.AdyenRefusals,
                Logs = _loggingInfo.Logs.OrderByDescending(d => d.Date).Take(100),
                Current = DateTime.Now,
            };

            return pageModel;
        }
    }
}