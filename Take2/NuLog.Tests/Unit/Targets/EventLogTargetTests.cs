using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NuLog.Tests.Unit.Targets
{
    /// <summary>
    /// Documents (and verifies) the expected behavior of the event log target.
    /// </summary>
    [Trait("Category", "Unit")]
    public class EventLogTargetTests
    {

        /// <summary>
        /// The event log target should write an event.
        /// </summary>
        [Fact(DisplayName = "Should_WriteEvent")]
        public void Should_WriteEvent()
        {

        }


    }
}
