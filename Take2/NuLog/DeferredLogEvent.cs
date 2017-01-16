/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;

namespace NuLog
{
    ///// <summary>
    ///// A deferred log event, which delays generation of a <see cref="LogEvent" /> until the first
    ///// time that it is written to a target.
    ///// </summary>
    //public class DeferredLogEvent : ILogEvent
    //{
    //    private readonly Lazy<LogEvent> deferredLogEvent;

    //    public DeferredLogEvent(Func<LogEvent> deferredFunc)
    //    {
    //        this.deferredLogEvent = new Lazy<LogEvent>(() =>
    //        {
    //            return deferredFunc();
    //        });
    //    }

    //    public void WriteTo(ITarget target)
    //    {
    //        this.deferredLogEvent.Value.WriteTo(target);
    //    }

    //    public void Dispose()
    //    {
    //        if (this.deferredLogEvent.IsValueCreated)
    //        {
    //            this.deferredLogEvent.Value.Dispose();
    //        }
    //    }
    //}
}