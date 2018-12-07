using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUtilis.Windows
{
    /// <summary>
    /// 
    /// </summary>
    class TraceEvent
    {
        /// <summary>
        /// Example:Name of Program.
        /// </summary>
        public string EventSource { get; set; }
        /// <summary>
        /// Example: Application,Security
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventSource"></param>
        /// <param name="eventName"></param>
        public TraceEvent(string eventSource = "", string eventName = "")
        {
            EventSource = eventSource;
            EventName = eventName;
        }

        /// <summary>
        /// Write in Event Logs
        /// </summary>
        /// <param name="eventMessage"></param>
        /// <param name="eventType"></param>
        public void Create(string eventMessage, Type eventType)
        {

            EventLogEntryType notificationType = EventLogEntryType.Information;

            switch (eventType)
            {
                case Type.Information:
                    notificationType = EventLogEntryType.Information;
                    break;
                case Type.Warning:
                    notificationType = EventLogEntryType.Warning;
                    break;
                case Type.Error:
                    notificationType = EventLogEntryType.Error;
                    break;
                default:
                    break;
            }

            if (!EventLog.SourceExists(EventSource))
                EventLog.CreateEventSource(EventSource, EventName);

            using (EventLog eventLog = new EventLog(EventName))
            {
                eventLog.Source = EventSource;
                eventLog.WriteEntry(eventMessage, notificationType);
            }
        }

        /// <summary>
        /// Type of events logs
        /// </summary>
        public enum Type
        {
            Information = 0,
            Warning = 1,
            Error = 2
        }
    }
}
