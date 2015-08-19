using System;
using System.Collections.Generic;

namespace CppDashboard.Models
{
    public class PageModel
    {
        public bool IsSystemOnline { get; set; }

        public IEnumerable<Log> Logs { get; set; }

        public string Throughput { get; set; }

        public DateTime Current { get; set; }

        public int CyRefunds { get; set; }

        public int AdyRefunds { get; set; }

        public int VerifyCard { get; set; }

        public int SuccessPayments { get; set; }

        public int DeclinedPayments { get; set; }

        public int  CancellationsDueToGhosts { get; set; }

        public int GatewayMkFaliures { get; set; }

        public int AdyenMkFaliures { get; set; }

        public IEnumerable<PaymentEvent> MonitoringEvents { get; set; }
    }
}