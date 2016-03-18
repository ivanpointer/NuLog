/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 3/17/2016
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using System;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using NuLog.Targets;

namespace NuLog.Configuration.Targets
{
	/// <summary>
	/// The configuration for writing to the windows event log
	///
	/// MSDN article for writing to the windows event log:
	/// https://support.microsoft.com/en-us/kb/307024
	/// </summary>
	public class WindowsEventLogTargetConfig : LayoutTargetConfig
	{
		#region Constants

		// Tokens
		// The name of the config token which specifies the source to list in the event logged to the windows event log
		private const string SourceTokenName = "Source";

		// The name of the config token which specifies the log within which to place the source, to which is being logged in the windows event log
		private const string LogTokenName = "Log";

		// The name of the config token which specifies the event log entry type to specify in the event logged to the windows event log
		private const string EventLogEntryTypeTokenName = "EventLogEntryType";

		// The name of the config token which specifies the event ID to specify in the event logged to the windows event log
		private const string EventIDTokenName = "EventID";

		#endregion Constants

		/// <summary>
		/// The source of the log event (the implementing application).
		/// </summary>
		public string Source { get; set; }

		/// <summary>
		/// The log to write to within the Windows Event Log.  This is usually "Application Log".
		/// </summary>
		public string Log { get; set; }

		/// <summary>
		/// The type of the event log entry, such as Information, Error or Warning.
		/// </summary>
		public EventLogEntryType EventLogEntryType { get; set; }

		/// <summary>
		/// The ID of the event.  This is application specific.
		/// </summary>
		public int EventID { get; set; }

		/// <summary>
		/// Builds a default/base windows event log config
		/// </summary>
		public WindowsEventLogTargetConfig()
			: base()
		{
			Type = typeof(WindowsEventLogTarget).FullName;
			SetDefaults();
		}

		/// <summary>
		/// Builds a windows event log target config using the passed JSON token
		/// </summary>
		/// <param name="jToken"></param>
		public WindowsEventLogTargetConfig(JToken jToken)
			: base(jToken)
		{
			// Initialize our type and defaults
			Type = typeof(WindowsEventLogTarget).FullName;
			SetDefaults();

			// If a JSON configuration was provided
			if (jToken != null && jToken.Type == JTokenType.Object)
			{
				// First, the source
				var sourceToken = jToken[SourceTokenName];
				if (sourceToken != null && sourceToken.Type == JTokenType.String)
					Source = sourceToken.Value<string>();

				// And the log
				var logToken = jToken[LogTokenName];
				if (logToken != null && logToken.Type == JTokenType.String)
					Log = logToken.Value<string>();

				// Then the event log entry type
				var eventLogEntryTypeToken = jToken[EventLogEntryTypeTokenName];
				if (eventLogEntryTypeToken != null && eventLogEntryTypeToken.Type == JTokenType.String)
					EventLogEntryType = (EventLogEntryType)Enum.Parse(typeof(EventLogEntryType), eventLogEntryTypeToken.Value<string>());

				// And finally, the event ID
				var eventIDToken = jToken[EventIDTokenName];
				if (eventIDToken != null && eventIDToken.Type == JTokenType.Integer)
					EventID = eventIDToken.Value<int>();
			}
		}

		/// <summary>
		/// Sets the default configuration values.
		/// </summary>
		protected virtual void SetDefaults()
		{
			Source = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
			Log = null;
			EventLogEntryType = EventLogEntryType.Information;
			EventID = 0;
		}
	}
}