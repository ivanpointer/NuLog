.. rst-class:: center

.. image:: sitetitle.png

----

################
  Advanced Usage
################

Meta Data Providers
===================
Much like static meta data, dynamic meta data can be included in log events to be delivered to the targets.  To do this, a `IMetaDataProvider`
instance must be passed to the log manager when requesting a logger.

Custom Meta Data Provider
-------------------------

The `IMetaDataProvider` interface has a single method for providing meta data:

.. literalinclude:: /../../NuLog/Loggers/IMetaDataProvider.cs
   :lines: 9-18
   :emphasize-lines: 9
   :linenos:

There's not any more to it than that.  As an illustration, however, let's take a look at an implementation that includes some request data for a MVC controller:

.. literalinclude:: /../../NuLogSnippets/Docs/MyControllerMetaDataProvider.cs
   :lines: 11-29
   :emphasize-lines: 10-18
   :linenos:

This meta data provider will include information specific to the request, for the given controller.  Next, let's take a look at how to use our new meta data provider.

Using Meta Data Providers
-------------------------


To use our custom meta data provider, we pass an instance of it to our log manager when requesting a logger:

.. literalinclude:: /../../NuLogSnippets/Docs/MyMetaDataProviderController.cs
   :lines: 10-25
   :emphasize-lines: 7-8,13
   :linenos:


Advanced Rule Routing
=====================

The 'final' and 'strictInclude' Flags
-------------------------------------

Complex routing can be achieved using the `final` and `strictInclude` flags:

* **final** - When set to `true`, and if a log message matches the rule, no further rules will be processed.  Rules are processed in consecutive order, as listed in the rule list.
* **strictInclude** - By default, if any tag is matched in the `include` list, the rule is matched.  By seting the `StrictInclude` flag, all tags (or tag patterns) listed in `include` must match for the rule to be considered a match.

Examples
--------

**final**

Let's say that you want to send messages with the "consoleonly" tag, only to console, but have a general, catch-all rule in place.  In the following example, events with the `consoleonly` tag will be routed only to the console target, all others will only go to the file target:

.. highlight:: xml

.. literalinclude:: /../../NuLogSnippets/ConsoleOnlyRule.config
   :lines: 8-16
   :tab-width: 4
   :dedent: 2
   :linenos:

**strictInclude**

lorem ipsum.