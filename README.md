[![Build status](https://ci.appveyor.com/api/projects/status/dubwfc9cr65dy866/branch/master?svg=true)](https://ci.appveyor.com/project/ivanpointer/nulog/branch/master)

# NuLog
_Powerfully Simple Logging. Period._  
_Written in C# for .NET_
***
**Quick Start:** For a quick start, see the [Developer Implementation Guide](https://github.com/ivanpointer/NuLog/wiki#developer-implementation-quick-start-guide).  
**Full Documentation:** Full documentation can be found in the [NuLog Wiki](https://github.com/ivanpointer/NuLog/wiki).  
**NuGet:** NuLog is provided as a NuGet package in the Gallery, just search for "NuLog".  [NuLog in NuGet Gallery](http://www.nuget.org/packages?q=NuLog)  
**Source:** The source and samples are available here on GitHub.  

## 1/21/2017 - Switching to AppVeyor.  Configuration built, switching to IoC.
I'm switching to AppVeyor - a lot of the other open source projects I've seen use it, and Travis CI is a bit slow.  I've got the configuration part built out, reading XML from a custom config section.  Next, I'll be tying the config together with a factory, using Ninject (IoC) as the glue.  I've also started a new Slack group (nulog.slack.com) for the project - initially for AppVeyor build notifications, but if any one else joins the NuLog team, we can communicate this way.

I've got to figure out how to get AppVeyor to automatically run xUnit tests.  I've listed the assembly for the tests, but I'm unsure if it's actually running the tests.  I suppose I can break a test and sync it to GitHub to see if the build fails...

## 1/18/2017 - Slowing down - tying it together
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
