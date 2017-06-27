**Build Status and Metrics ([AppVeyor](https://ci.appveyor.com/project/ivanpointer/nulog) and [Sonar](https://sonarcloud.io/dashboard?id=NuLog)):**  
[![Build status](https://ci.appveyor.com/api/projects/status/dubwfc9cr65dy866/branch/master?svg=true)](https://ci.appveyor.com/project/ivanpointer/nulog/branch/master)
[![Coverage](https://sonarcloud.io/api/badges/measure?key=NuLog&metric=coverage)](https://sonarcloud.io/dashboard/index/NuLog)

**Release Candidate NuGet Package Feed:** https://ci.appveyor.com/nuget/nulog-yw5ahjoihbcj

**Read the Docs:** http://nulog.readthedocs.io/en/latest/index.html

# NuLog
_Powerfully Simple Logging. Period._  
_Written in C# for .NET_
***

## 6/26/2017 - Back on it - But, Cleanup First
We've finally settled into our new home in bright-and-sunny Texas.  At my day-job, I've been asked to present a case-study of TDD with my experience with the rewrite of NuLog.  This prompted me to integrate into the CI process for NuLog, Sonar.  Sonar found a few bugs and smells.  I'm getting these cleaned up, then I'll finish the documentation, then put out the first release of this new version of NuLog.

## 4/11/2017 - Settling In - Hoping to Start Again Soon
Our move is finished.  We're still settling, unpacking boxes, installing appliances - learning that the garage needs to stay clear for the last-minute alerts of half-dollar sized hail...

I expect we'll be settled soon, at which point, I'll continue polishing, and promoting, the project.

## 2/24/2017 - Still Here
NuLog is still burning in my mind, but the sale of my home and move to someplace sunnier is heavily under way.  My fingers are sore from cleaning old interior caulk, painting, etc.  I'll be back to take NuLog live, hopefully in a couple months!  Keep checking back!

## 2/7/2017 - Short Hiatus
I'm putting my house on the market.  Between replacing lenolium, caulking baseboards and packing, I'm not finding much time for this.  I'll be back soon from a sunnier place!

## 1/30/2017 - Doco Well Under-Way
I'm leveraging "Read the Docs" and Sphynx to build out the documentation.  It is coming along quite well.

## 1/29/2017 - .Net Versions and NuGet Packaging Complete
I've managed to get the solution building for .Net 3.5, 4 and 4.5.2 as a part of the AppVeyor CI builds.  I've also got the AppVeyor build producing a "release candidate" NuGet package, which is being collected as an artifact, and published on the AppVeyor NuGet feed:

https://ci.appveyor.com/nuget/nulog-yw5ahjoihbcj

One step closer to having a viable product here!

## 1/28/2017 - Taking a break from targets - postponing performance/memory tuning - moving to NuGet packaging and .Net versions
I've got the Windows Event Log and SMTP/Mail targets built out, and added some exception handling/fallback logging.  Instead of doing performance and memory tuning, I want to look into NuGet packaging and compiling for different .Net versions.  I'll look at performance/memory tuning later.

I'm approaching an initial alpha release!

## 1/26/2017 - Development continues, if slowly.
Just finished out one of the more complex targets - the mail target.  Before looking into the next round of performance/memeory tuning, then NuGet packaging and compiling for different .Net versions - I want to finish one more target for writing to the Windows Event Log.  I'm considering XML and JSON file writers, but am growing a little weary of writing targets and want a change - so I may do those later.  I might also build a target which performs a HTTP POST to send the log event to a REST service endpoint.  Finally, I want to build extra "packages" for logging to more specific 3rd party destinations, like Slack, SignalR or Elmah.

## 1/23/2017 - Core functionality done.  Performance and memory tuning done.  Targets next.
I've got the core functionality for NuLog in place.  The performance and memory tuning are done for this round of development.  Next, I start fleshing out common targets, like file, SMTP and Windows event logs.

Also, I need some tests around making sure that all log events are dispatched when the system is shutting down.  I failed TDD when I added a finalizer to the logger factory, without writing unit tests to test that the finalizer actually makes sure that the log events are all flushed before disposal.

## 1/22/2017 - Good progress.  Nearing performance and memory tuning.
I've got the tests built out up through the logger factory, and into the log manager.  I've got a little more work to do in the log manager, then I'll be ready to start looking into building out the various targets, and performance and memory tuning.  I'm coming into another work week, so development will slow until next weekend.

## 1/21/2017 - Cancel IoC.
Adding an IoC framework (like Ninject), will add that as a dependency to using NuLog.  I don't want any extra dependencies, so I'll just use a factory pattern to handle DI for NuLog.

## 1/21/2017 - Switching to AppVeyor.  Configuration built, switching to IoC.
I'm switching to AppVeyor - a lot of the other open source projects I've seen use it, and Travis CI is a bit slow.  I've got the configuration part built out, reading XML from a custom config section.  Next, I'll be tying the config together with a factory, using Ninject (IoC) as the glue.  I've also started a new Slack group (nulog.slack.com) for the project - initially for AppVeyor build notifications, but if any one else joins the NuLog team, we can communicate this way.

I've got to figure out how to get AppVeyor to automatically run xUnit tests.  I've listed the assembly for the tests, but I'm unsure if it's actually running the tests.  I suppose I can break a test and sync it to GitHub to see if the build fails...

## 1/18/2017 - Slowing down - Tying it together
Development is slowing a bit; I've got much of the "core" functionality built out.  By using the DIP, and TDD, NuLog has a highly modular design, but, this means building out the functional system takes an unwieldly number of classes.  It's time to shift my development efforts towards the factory for building everything out, and the configuration to drive the factory.  After this, the various targets need to be built out - such as for console, text files, the windows event log, etc.

So now, I dig into figuring out how to approach the factory...

## 1/15/2017 - Rewrite is going well
I was a little skeptical that a rewrite would be worth it, but all doubt has been removed.  Taking the lessons learned from the first write of NuLog, and tampering them with TDD, I'm coming up with a much better design for NuLog.  This thing is going to be even better than before!!

I've decided to use FakeItEasy for mocking in the tests project.  For my reasoning around this, see my new page [Architectural Decisions](https://github.com/ivanpointer/NuLog/wiki/Architectural-Decisions).

## 1/14/2017 - Things are getting hazy
Tests are failing in an unpredictable way, and memory profile sessions indicate that I've got a leak around the configuration part of the library.  Some of the cleanest work I've ever done was through TDD.  I'm starting again - "Take2" is a rebuild of the same NuLog, but this time with TDD guiding the design decisions - hopefully helping me avoid some of these more difficult problems.

If I'm sure of anything, it's that I've got the relationship of the factory, loggers, dispatchers and targets, and the rules, meta data and tags that tie them together, right.  It's the connection points between each of these that leaves one wanting.

## 1/14/2017 UNDER DEVELOPMENT AGAIN
I'm working on NuLog again.  I'm working to flush out some concurrency bugs, and to work to get the framework to be more testable in isolation (true unit tests), and more extensible.

This will be a breaking update, as some of the singleton patterns from before do not work well with unit test isolation, etc.
