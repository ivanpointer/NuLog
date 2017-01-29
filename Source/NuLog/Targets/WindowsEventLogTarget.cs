/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 3/17/2016
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using System;
using System.Diagnostics;
using NuLog.Configuration.Targets;
using NuLog.Dispatch;

namespace NuLog.Targets
{
	/// <summary>
	/// A target for writing to the windows event log.
	/// </summary>
	public class WindowsEventLogTarget : LayoutTargetBase
	{
		#region Constants

		public const string MetaEventLogEntryType = "WindowsEventLogEntryType";
		public const string MetaEventID = "WindowsEventID";

		#endregion Constants

		public WindowsEventLogTargetConfig Config { get; set; }

		public override void Log(LogEvent logEvent)
		{
			// Figure out if we've been given other information via meta
			var eventLogEntryType = Config.EventLogEntryType;
			if (logEvent.MetaData != null && logEvent.MetaData.ContainsKey(MetaEventLogEntryType))
				eventLogEntryType = ParseEventLogEntryType(Convert.ToString(logEvent.MetaData[MetaEventLogEntryType]));

			var eventID = Config.EventID;
			if (logEvent.MetaData != null && logEvent.MetaData.ContainsKey(MetaEventID))
				eventID = Convert.ToInt32(logEvent.MetaData[MetaEventLogEntryType]);

			// Write to the windows event log
			EventLog.WriteEntry(Config.Source, Layout.FormatLogEvent(logEvent), eventLogEntryType, eventID);
		}

		/// <summary>
		/// Initializes this target for writing to the windows event log.
		/// </summary>
		/// <param name="targetConfig">The target config to build this target from</param>
		/// <param name="dispatcher">The dispatcher this target is attached to</param>
		/// <param name="synchronous">An override to the synchronous setting in the target config</param>
		public override void Initialize(TargetConfig targetConfig, LogEventDispatcher dispatcher = null, bool? synchronous = null)
		{
			base.Initialize(targetConfig, dispatcher, synchronous);

			// Parse out the target configuration
			Config = targetConfig != null &&
				typeof(WindowsEventLogTargetConfig).IsAssignableFrom(targetConfig.GetType())
					? (WindowsEventLogTargetConfig)targetConfig
					: new WindowsEventLogTargetConfig(targetConfig.Config);

			// Make sure that the windows event log is ready for us
			try
			{
				if (!EventLog.SourceExists(Config.Source))
					EventLog.CreateEventSource(Config.Source, Config.Log);
			}
			catch (Exception cause)
			{
				Debug.WriteLine("Failure ensuring log source exists, try running first as an administrator to create the source.  Error: " + cause.GetType().FullName + ": " + cause.Message);
			}
		}

		/// <summary>
		/// Tries to parse the given string into an EventLogEntryType.  If a failure occurs, it is written to Debug, and the configured EventLogEntryType is return instead.
		/// </summary>
		/// <param name="type">The string to parse for a EventLogEntryType.</param>
		/// <returns>The parsed EventLogEntryType, the configured EventLogEntryType if parsing fails.</returns>
		private EventLogEntryType ParseEventLogEntryType(string type)
		{
			try
			{
				return (EventLogEntryType)Enum.Parse(typeof(EventLogEntryType), type);
			}
			catch (Exception cause)
			{
				Debug.WriteLine("Failure parsing Windows EventLogEntryType from meta data for log event: " + cause.GetType().FullName + ": " + cause.Message);
				return EventLogEntryType.Information;
			}
		}
	}
}