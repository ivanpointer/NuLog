.. rst-class:: center

.. image:: ..\images\sitetitle.png

----

#########
  Targets
#########

.. highlight:: xml

Configuration
=============

NuLog comes with a number of built-in targets.  Targets are configured within the :code:`<targets>` section of the NuLog configuration:

.. literalinclude:: /../../NuLogSnippets/TargetsConfig.config
   :lines: 1-
   :tab-width: 4
   :emphasize-lines: 9
   :linenos:

All targets must have a :code:`name` and a :code:`type`:

  * **name** - The *name* identifies the target to the various rules.
  * **type** - The *type* indicates the concrete type of the target.  When referencing the built-in NuLog targets, you can omit the assembly name from the type string; but if you are referencing a third party, or custom target, you'll need to include the assembly, as is standard when identifying a type: *"Some.Assembly.Namespace.Path.ToATarget, Some.Assembly"*.

----

Trace Target
============
**NuLog.Targets.TraceTarget**

The trace target writes log events to `Trace`:

.. literalinclude:: /../../NuLogSnippets/TraceTargetConfig.config
   :lines: 9-10
   :tab-width: 4
   :dedent: 3
   :linenos:

The trace target has the following properties:

  * **layout** - *Optional* - Defines the layout format for the target.  By default, this is a standard layout, as documented in :ref:`standardlayout`.

----

Console Target
==============
**NuLog.Targets.ConsoleTarget**

The console target writes log events to `Console`:

.. literalinclude:: /../../NuLogSnippets/ConsoleTargetConfig.config
   :lines: 9-12
   :tab-width: 4
   :dedent: 3
   :linenos:

The console target has the following properties:

  * **layout** - *Optional* - Defines the layout format for the target.  By default, this is a standard layout, as documented in :ref:`standardlayout`.
  * **background** - *Optional* - An optional override to the `Background Color <https://msdn.microsoft.com/en-us/library/system.consolecolor(v=vs.110).aspx>`_ of messages written to the console.
  * **foreground** - *Optional* - An optional override to the `Foreground Color <https://msdn.microsoft.com/en-us/library/system.consolecolor(v=vs.110).aspx>`_ of messages written to the console.

----

Text File Target
================
**NuLog.Targets.TextFileTarget**

The text file target writes log events to a text file:

.. literalinclude:: /../../NuLogSnippets/TextFileTargetConfig.config
   :lines: 9-11
   :tab-width: 4
   :dedent: 3
   :linenos:

The text file target has the following properties:

  * **path** - *Required* - The path to the text file to log to.  Can be relative, or absolute.
  * **layout** - *Optional* - Defines the layout format for the target.  By default, this is a standard layout, as documented in :ref:`standardlayout`.

----

Mail Target
===========
**NuLog.Targets.MailTarget**

The mail target sends log events via a SMTP server:

.. literalinclude:: /../../NuLogSnippets/MailTargetConfig.config
   :lines: 9-22
   :tab-width: 4
   :dedent: 3
   :linenos:

The mail target has the following properties:

*TODO*