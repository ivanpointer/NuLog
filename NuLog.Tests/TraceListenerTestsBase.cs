/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;
using System.Diagnostics;
using Xunit.Abstractions;

namespace NuLog.Tests {

    /// <summary>
    /// A base class for the test facilities that redirects trace output to the test output.
    /// </summary>
    public class TraceListenerTestsBase : IDisposable {
        private XUnitOutputTraceListener traceListener;

        public TraceListenerTestsBase(ITestOutputHelper output) {
            this.traceListener = new XUnitOutputTraceListener(output);
            Trace.Listeners.Add(this.traceListener);
        }

        public virtual void Dispose() {
            Trace.Listeners.Remove(this.traceListener);
            this.traceListener = null;
        }
    }

    internal class XUnitOutputTraceListener : TraceListener {
        private readonly ITestOutputHelper testOutputHelper;

        public XUnitOutputTraceListener(ITestOutputHelper testOutputHelper) {
            this.testOutputHelper = testOutputHelper;
        }

        public override void Write(string message) {
            this.testOutputHelper.WriteLine(message);
        }

        public override void WriteLine(string message) {
            this.testOutputHelper.WriteLine(message);
        }
    }
}