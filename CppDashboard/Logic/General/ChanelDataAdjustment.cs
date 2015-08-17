using System.Collections.Generic;
using CppDashboard.Models;

namespace CppDashboard.Logic.General
{
    public class ChanelDataAdjustment
    {
        public void CorrectChannelData(IEnumerable<SystemEventSummary> eventSummary)
        {
            foreach (var systemEventSummary in eventSummary)
            {
                int parsedValue;

                if (int.TryParse(systemEventSummary.Channel, out parsedValue))
                {
                    var channelName = (Channel)parsedValue;

                    systemEventSummary.Channel = channelName.ToString();
                }    
            }
        }
    }
}