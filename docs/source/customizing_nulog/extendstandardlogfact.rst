.. rst-class:: center

.. image:: sitetitle.png

----

#######################################
  Extending the Standard Logger Factory
#######################################

Introduction
============

The "standard" logger factory is named such, because through the log mnanager, and a custom logger factory, you can change almost any part of NuLog.
I suggest, however, only changing those parts which you need to change, the rest of NuLog is very carefully thought out, implemented and thuroughly tested.

To customize the behavior of NuLog, you would create your own implementation of certain parts of the NuLog system, extend the `StandardLoggerFactory`
to leverage your custom implementation, then set your extension of the logger factory into the `LogManager`, as is discussed in :ref:`logfactman`.

The first thing to consider is that the `StandardLoggerFactory` has a constructor that receives the `Config` of the system.  This means that the `Config`
is available to each of the members.

I suggest using as much of the standard implementation as possible, as performance and stability have been carefully built into them.  While you can
completely customize NuLog, it would be prudent to take advantage of the work we've done here in the standard implementation.

Standard implementation
=======================

MakeDispatcher()
----------------
The `MakeDispatcher` method is responsible for creating the dispatcher for the system.  Overload this to implement your own dispatcher implementation.

MakeTargets()
-------------
The `MakeTargets` method is responsible for creating the various targets based on the config, returning a collection of targets.

MakeTagGroupProcessor()
-----------------------
Tag group processors are responsible for determining aliases between tags, such as `apple` and `tomato` being synonymous for `fruit`.

MakeRuleProcessor()
-------------------
Rule processors are responsible for determining which targets a set of tags correspond to, based on the parsed rules from the config.  The rule
processor is used by the tag router.

MakeTagRouter()
---------------
Responsible for indicating which targets a set of tags go to, unlike the rule processor, the tag router has no knowledge of rules, but instead
will have some caching and performance considerations built in.  In the standard implementation of NuLog, a rule processor is given to the
tag router, and the dispatcher works directly with the tag router, having no knowledge of the rule processor.

MakeTagNormalizer()
-------------------
The tag normalizer is responsible for normalizing the tags, such as making them lower-case and trimming white space, and converting special characters.

MakeLayoutParser()
------------------
Layout parsers are responsible for taking a string representation of a layout, and converting it into a concrete list of layout parameters.

MakePropertyParser()
--------------------
Property parsers are responsible for reflectively traversing a given object, and returning the value of an identified property, based on the list of property names given.

MakeLayout()
------------
Layouts are responsible for converting a log event to a string representation.  The standard layout uses the concept of layout parameters to achieve this.
If you're implementing your own layout, you may need your own layout parser, and my not even need the concept of layout parameters.

MakeFallbackLogger()
--------------------
The fallback logger is used when a failure occurs during the NuLog lifecycle.  It is supposed to fail silently, as it is the fallback for other failures
in the system.  Without a properly behaving fallback logger, NuLog may leak failures into the implementing code.