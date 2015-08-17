﻿using System;
using System.Collections.Generic;
using System.Linq;
using CppDashboard.DataProvider.Setup;
using CppDashboard.Models;
using CppDashboard.Extensions;
using Ninject.Infrastructure.Language;

namespace CppDashboard.DataProvider
{
    public class LoggingDataCanFacade : DataCanRefreshBase<Log>, ILoggingInfo, ICanReload, ILoadVolatileData
    {
        public IEnumerable<Log> Logs
        {
            get
            {
                return _logs;
            }
        }

        private IEnumerable<Log> _logs;
 
        private readonly ConnectionCreator _connectionCreator;

        public LoggingDataCanFacade()
        {
            _connectionCreator = new ConnectionCreator(Scope.Logging);
        }

        public override void Load()
        {
            var sql = string.Format("SELECT  * FROM logging.CustomerPayments_log WITH (NOLOCK) " +
                   "WHERE Date BETWEEN '{0}' AND '{1}' " +
                                 "ORDER BY Date DESC", DateTime.Now.FormattedMins(-Constants.TimeoutDuration), DateTime.Now.Formatted());

            _logs = _connectionCreator.Exec<Log>(sql);
        }

        protected override bool AllowedPeriod(Log input)
        {
            return input.Date >= DateTime.Now.StartMins(Constants.TimeoutDuration) && input.Date <= DateTime.Now;
        }

        protected override IEnumerable<Log> LoadingFrom()
        {
            var maxPrimary = _logs.Max(m => m.Id);

            var sql = string.Format("SELECT  * FROM logging.CustomerPayments_log WITH (NOLOCK) " +
                 "WHERE Id > '{0}'", maxPrimary);

            var data = _connectionCreator.Exec<Log>(sql);

            return data;
        }

        public void Reload()
        {
            IList<Log> l = _logs.ToList();
            Refresh(ref l);
        }
    }
}