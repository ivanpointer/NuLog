[![Build Status](https://travis-ci.org/ivanpointer/NuLog.svg?branch=master)](https://travis-ci.org/ivanpointer/NuLog)

# UNDER DEVELOPMENT AGAIN
I'm working on NuLog again.  I'm working to flush out some concurrency bugs, and to work to get the framework to be more testable in isolation (true unit tests), and more extensible.

This will be a breaking update, as some of the singleton patterns from before do not work well with unit test isolation, etc.

## 1/14/2017 - Things are getting hazy
Tests are failing in an unpredictable way, and memory profile sessions indicate that I've got a leak around the configuration part of the library.  Some of the cleanest work I've ever done was through TDD.  I'm starting again - "Take2" is a rebuild of the same NuLog, but this time with TDD guiding the design decisions - hopefully helping me avoid some of these more difficult problems.

If I'm sure of anything, it's that I've got the relationship of the factory, loggers, dispatchers and targets, and the rules, meta data and tags that tie them together, right.  It's the connection points between each of these that leaves one wanting.

# NuLog
_Powerfully Simple Logging. Period._  
_Written in C# for .NET_
***
**Quick Start:** For a quick start, see the [Developer Implementation Guide](https://github.com/ivanpointer/NuLog/wiki#developer-implementation-quick-start-guide).  
**Full Documentation:** Full documentation can be found in the [NuLog Wiki](https://github.com/ivanpointer/NuLog/wiki).  
**NuGet:** NuLog is provided as a NuGet package in the Gallery, just search for "NuLog".  [NuLog in NuGet Gallery](http://www.nuget.org/packages?q=NuLog)  
**Source:** The source and samples are available here on GitHub.  
