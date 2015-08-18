using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CppDashboard.DataProvider;
using CppDashboard.Initialisers;
using CppDashboard.Logic.General;
using CppDashboard.Models;

namespace CppDashboard.Controllers
{
    public class SystemDataController : ApiController
    {
        private readonly IInitialiser _initialiser;
        private readonly IErrorSummaryWindow _errorSummary;
        private readonly ISystemEventSummaryWindow _eventSummaryWindow;
        private readonly ChanelDataAdjustment _chanelDataAdjustment;

        public SystemDataController(IInitialiser systemInitialiser, 
            IErrorSummaryWindow errorSummary, 
            ISystemEventSummaryWindow eventSummaryWindow, 
            ChanelDataAdjustment chanelDataAdjustment)
        {
            _initialiser = systemInitialiser;
            _initialiser.Load();
            _errorSummary = errorSummary;
            _eventSummaryWindow = eventSummaryWindow;
            _chanelDataAdjustment = chanelDataAdjustment;
        }

        [HttpGet]
        public IEnumerable<ErrorSummary> GetSystemErrors()
        {
            return _errorSummary.ErrorSummaries;
        }

        [HttpGet]
        public IEnumerable<EventGroup> GetSystemEventsEx()
        {
            _chanelDataAdjustment.CorrectChannelData(_eventSummaryWindow.EventSummary);

            var result = _eventSummaryWindow.EventSummary
                .GroupBy(evt => evt.EventType)
                .Select(e => new EventGroup { Key = e.Key, Values = e }).ToList();

            return result;
        }
    }
}
