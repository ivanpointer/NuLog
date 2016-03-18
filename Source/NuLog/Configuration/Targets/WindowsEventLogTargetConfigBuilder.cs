/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 3/17/2016
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using System.Diagnostics;
using NuLog.Configuration.Layouts;
using NuLog.Targets;

namespace NuLog.Configuration.Targets
{
	/// <summary>
	/// Used to build a windows event log target config at runtime
	/// </summary>
	public class WindowsEventLogTargetConfigBuilder
	{
		/// <summary>
		/// Represents the name of the target
		/// </summary>
		protected string Name { get; set; }

		/// <summary>
		/// Represents the synchronous flag of the target
		/// </summary>
		protected bool? Synchronous { get; set; }

		/// <summary>
		/// Represents the layout configuration of the target
		/// </summary>
		protected LayoutConfig LayoutConfig { get; set; }

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
		/// Builds a default configuration builder
		/// </summary>
		private WindowsEventLogTargetConfigBuilder()
		{
			Name = "windowsEventLog";
			Synchronous = null;
			LayoutConfig = new LayoutConfig();
		}

		/// <summary>
		/// Gets a new instance of this console target config builder
		/// </summary>
		/// <returns>A new instance of this console target config builder</returns>
		public static WindowsEventLogTargetConfigBuilder Create()
		{
			return new WindowsEventLogTargetConfigBuilder();
		}

		/// <summary>
		/// Sets the represented name
		/// </summary>
		/// <param name="name">Represents the name of the target</param>
		/// <returns>This console target config builder instance</returns>
		public WindowsEventLogTargetConfigBuilder SetName(string name)
		{
			Name = name;
			return this;
		}

		/// <summary>
		/// Sets the represented synchronous flag
		/// </summary>
		/// <param name="synchronous">Represents the synchronous flag of the target</param>
		/// <returns>This console target config builder instance</returns>
		public WindowsEventLogTargetConfigBuilder SetSynchronous(bool synchronous)
		{
			Synchronous = synchronous;
			return this;
		}

		/// <summary>
		/// Sets a standard layout with the given layout format
		/// </summary>
		/// <param name="layoutFormat">The layout format to use for the new standard layout config</param>
		/// <returns>This console target config builder instance</returns>
		public WindowsEventLogTargetConfigBuilder SetLayoutConfig(string layoutFormat)
		{
			LayoutConfig = new LayoutConfig(layoutFormat);
			return this;
		}

		/// <summary>
		/// Sets the layout configuration
		/// </summary>
		/// <param name="layoutConfig">The layout configuration to use</param>
		/// <returns>This console target config builder instance</returns>
		public WindowsEventLogTargetConfigBuilder SetLayoutConfig(LayoutConfig layoutConfig)
		{
			LayoutConfig = layoutConfig;
			return this;
		}

		/// <summary>
		/// Sets the source of the WindowsEventLogConfig.
		/// </summary>
		/// <param name="source">The source to set.</param>
		/// <returns>This config builder instance.</returns>
		public WindowsEventLogTargetConfigBuilder SetSource(string source)
		{
			Source = source;
			return this;
		}

		/// <summary>
		/// Sets the log of the WindowsEventLogconfig.
		/// </summary>
		/// <param name="log">The log to set.</param>
		/// <returns>This config builder instance.</returns>
		public WindowsEventLogTargetConfigBuilder SetLog(string log)
		{
			Log = log;
			return this;
		}

		/// <summary>
		/// Sets the EventLogEntryType of the WindowsEventLogconfig.
		/// </summary>
		/// <param name="eventLogEntryType">The EventLogEntryType to set.</param>
		/// <returns>This config builder instance.</returns>
		public WindowsEventLogTargetConfigBuilder SetEventLogEntryType(EventLogEntryType eventLogEntryType)
		{
			EventLogEntryType = eventLogEntryType;
			return this;
		}

		/// <summary>
		/// Sets the event ID of the WindowsEventLogconfig.
		/// </summary>
		/// <param name="eventID">The event ID to set.</param>
		/// <returns>This config builder instance.</returns>
		public WindowsEventLogTargetConfigBuilder SetEventID(int eventID)
		{
			EventID = eventID;
			return this;
		}

		/// <summary>
		/// Builds and returns the windows event log config.
		/// </summary>
		/// <returns>The defined windows event log config.</returns>
		public WindowsEventLogTargetConfig Build()
		{
			return new WindowsEventLogTargetConfig
			{
				Name = this.Name,
				Type = typeof(WindowsEventLogTarget).FullName,
				Synchronous = this.Synchronous,
				LayoutConfig = this.LayoutConfig,
				Source = this.Source,
				Log = this.Log,
				EventLogEntryType = this.EventLogEntryType,
				EventID = this.EventID
			};
		}
	}
}