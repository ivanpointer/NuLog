.. rst-class:: center

.. image:: sitetitle.png

----

.. _logfactman:

######################################
  Logger Factories and the Log Manager
######################################

Introduction
============

The *Log Manager* is the entry point for the NuLog framework - it is where developers and their applications most commonly request loggers.

The *Log Manager* contains a reference to an instance of *ILoggerFactory*, the *StandardLoggerFactory* by default.  The *Log Manager* leverages the
*ILoggerFactory* to build the loggers for the developers and their applications.

Developers don't have to use the *Log Manager* to build their NuLog relics, instead going directly to a *ILoggerFactory*, or even building the
loggers themselves (not suggested).  A better approach, would be to extend the *StandardLoggerFactory* to provide your own implementations of the StandardLoggerFactory
components that make up the typical NuLog architecture.

ILoggerFactory Interface
========================

First, let's take a look at the *ILoggerFactory* interface, which defines the expected behavior of a logger factory.

.. literalinclude:: /../../NuLog/ILoggerFactory.cs
   :lines: 10-20
   :tab-width: 4
   :dedent: 4
   :linenos:

That's it!  The sole job of a logger factory is to construct instances of loggers.  Done.

Duties of the Log Manager
=========================

The log manager has three primary duties:

  * **Retrieval of Loggers** - The `LogManager` is the main point of entry for developers and their applications to retrieve loggers.
  * **Overriding the Default ILoggerFactory** - The `LogManager` provides a way to inject in your own custom implementation of `ILoggerFactory`, to enable you to customize any piece of the framework.
  * **Shutdown NuLog** - The `LogManager` provides a hook for disposing the assigned `ILoggerFactory`, which will shutdown the NuLog system, flushing all queued log events, and not allowing any more to enter the queue.

Overriding the Default Logger Factory
=====================================

The most conventional way to customize NuLog, is to create your own implemenmtations of the various pieces of NuLog, and extend the `StandardLoggerFactory`,
finally assigning it as the factory for the `LogManager`:

.. literalinclude:: /../../NuLogSnippets/Docs/SetLogManagerLoggerFactory.cs
   :start-after: start_snippet
   :end-before: end_snippet
   :tab-width: 4
   :dedent: 4
   :linenos:
