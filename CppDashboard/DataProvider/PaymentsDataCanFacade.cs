using System;
using System.Collections.Generic;
using System.Linq;
using CppDashboard.DataProvider.Setup;
using CppDashboard.Models;
using CppDashboard.Extensions;

namespace CppDashboard.DataProvider
{
    public class PaymentsDataCanFacade : DataCanRefreshBase<Payment>, IPaymentInfo, ICanReload, ILoadVolatileData
    {
        public IEnumerable<Payment> Payments
        {
            get
            {
                return _payments;
            }
        }

        private IEnumerable<Payment> _payments; 
        private readonly ConnectionCreator _connectionCreator;

        public PaymentsDataCanFacade()
        {
            _connectionCreator = new ConnectionCreator(Scope.CustomerPayment);
        }

        public void Load()
        {
            var sql = string.Format("SELECT  * FROM Payment WITH (NOLOCK) " +
                      "WHERE CreationDateTime BETWEEN '{0}' AND '{1}' " +
                                    "ORDER BY CreationDateTime", DateTime.Now.FormattedMins(-Constants.TimeoutDuration), DateTime.Now.Formatted());

            _payments = _connectionCreator.Exec<Payment>(sql);
        }

        protected override bool AllowedPeriod(Payment input)
        {
            return input.CreationDateTime >= DateTime.Now.StartMins(Constants.TimeoutDuration) && input.CreationDateTime <= DateTime.Now;
        }

        protected override IEnumerable<Payment> LoadingFrom()
        {
            var maxPrimary = _payments.Max(m => m.PaymentId);

            var sql = string.Format("SELECT  * FROM Payment WITH (NOLOCK) " +
                    "WHERE PaymentId > '{0}' ", maxPrimary);

            var data = _connectionCreator.Exec<Payment>(sql);

            return data;
        }

        public void Reload()
        {
            Refresh(ref _payments);
        }
    }
}