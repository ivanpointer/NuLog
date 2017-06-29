.. rst-class:: center

.. image:: sitetitle.png

----

###############################
  Custom Targets - Introduction
###############################

We believe that the most common customization of NuLog that will be sought after, will be custom targets.  Here's what it takes.

Where to Start?
===============

Aiming to be as flexible as possible, there are many different layers of abstraction to our targets.  This allows you to create targets with just the amount of functionality you desire.

As an example, let's take a look at the inheritance tree of our `ConsoleTarget`:

`ConsoleTarget` > `LayoutTargetBase` > `TargetBase`, `ILayoutTarget` > `ITarget`

  * **ConsoleTarget** - The console target implementation.
  * **LayoutTargetBase** - Provides standard layout parsing and instantiation functionality, for leverage by implementing targets in their `Write` methods.
  * **ILayoutTarget** - Defines the base expected behavior of a target with a layout.
  * **TargetBase** - Provides an empty implementation of the `IDisposable` pattern, and a couple helper methods for configuration.
  * **ITarget** - Defines the expected behavior of a target.

Bare Minimum: Implement the ITarget Interface
=============================================

The ITarget Interface
---------------------

A target must implement the `ITarget` interface.  This states that a target must:

  * Be disposable (`IDisposable` interface)
  * Have a Name
  * Must be able to recieve configuration.
  * Must be able to recieve a log event for writing.

.. literalinclude:: /../../NuLog/ITarget.cs
   :lines: 14-30
   :tab-width: 4
   :dedent: 1
   :linenos:

The lowest level implementation of a target may look something like this:

.. literalinclude:: /../../NuLogSnippets/Docs/CustomTargets/HelloWorldTarget.cs
   :lines: 12-30
   :tab-width: 4
   :dedent: 1
   :linenos:

Using Your Custom Target
------------------------

Once you've created your custom target, you need to reference it in your config:

.. literalinclude:: /../../NuLogSnippets/HelloWorldTargetConfig.config
   :language: xml
   :lines: 6-13
   :tab-width: 2
   :dedent: 1
   :linenos:


Less is More: Extend the TargetBase
-----------------------------------

You can actually implement your custom target in fewer lines of code by leverage the given `TargetBase` abstract class:

.. literalinclude:: /../../NuLogSnippets/Docs/CustomTargets/HelloWorldShortTarget.cs
   :lines: 11-17
   :tab-width: 4
   :dedent: 1
   :linenos:

Most Useful: Extend the LayoutTargetBase
========================================

Since it is text data that we're generally logging, the abstract class that provides the best starting point for most
custom targets, will be the `LayoutTargetBase`.  With the `LayoutTargetBase`, you get:

  * *Everything you get with the `TargetBase`, plus:*
  * A `protected` `ILayout` instance that is automatically configured using the `layout` attribute from the config.

The `StandardFactory` specifically recognizes the `ILayoutTarget` interface, which has a special `Configure` method which recieves an instance of `ILayoutFactory`.
This inverts the dependency on `ILayoutFactory`, allowing it to be injected into the target from above, in compliance of the *Dependency Inversion Principle*.

When extending the `LayoutTargetBase`, you get access to the automatically configured `layout` member:

.. literalinclude:: /../../NuLogSnippets/Docs/CustomTargets/HelloLayoutTarget.cs
   :lines: 11-18
   :tab-width: 4
   :dedent: 1
   :emphasize-lines: 5
   :linenos:

Now, when referencing your custom target in your config, you can set the layout:

.. literalinclude:: /../../NuLogSnippets/HelloLayoutTargetConfig.config
   :language: xml
   :lines: 6-14
   :tab-width: 4
   :dedent: 1
   :emphasize-lines: 4-6
   :linenos: