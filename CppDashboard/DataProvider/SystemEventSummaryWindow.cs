using System;
using System.Collections.Generic;
using System.Linq;
using CppDashboard.DataProvider.Setup;
using CppDashboard.Extensions;
using CppDashboard.Models;

namespace CppDashboard.DataProvider
{
    public class SystemEventSummaryWindow : ISystemEventSummaryWindow, ILoadSystemData
    {

        public IEnumerable<SystemEventSummary> EventSummary
        {
            get
            {
                return _summary;
            }
        }

        private readonly ConnectionCreator _connectionCreator;
        private IEnumerable<SystemEventSummary> _summary;

        private const string Sql = "select EventType, Channel, Count(1) Occurrences from PaymentEvents with (nolock) " +
            "where  EventDateTime between '{0}' and '{1}' " + "group by EventType, Channel order by  Occurrences desc, EventType desc, Channel";

        public SystemEventSummaryWindow()
        {
            _connectionCreator = new ConnectionCreator(Scope.Monitoring);
        }

        public void Load()
        {
            var sqlQuery = CreateSqlQuery();

            _summary = _connectionCreator.Exec<SystemEventSummary>(sqlQuery);
        }

      private string CreateSqlQuery()
        {
            var startDate = DateTime.Now.FormattedMins((-Constants.SummaryDurationInHours * 60));
            var endDate = DateTime.Now.Formatted();

            var sqlQuery = string.Format(Sql, startDate, endDate);
            return sqlQuery;
        }
    }
}