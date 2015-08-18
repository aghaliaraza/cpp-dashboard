using System.Collections.Generic;

namespace CppDashboard.Models
{
    public class EventGroup
    {
        public string Key { get; set; }

        public IEnumerable<SystemEventSummary> Values { get; set; }
    }
}