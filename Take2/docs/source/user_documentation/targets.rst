.. rst-class:: center

.. image:: \images\sitetitle.png

----

#########
  Targets
#########

Configuration
=============

NuLog comes with a number of built-in targets.  Targets are configured within the :code:`<targets>` section of the NuLog configuration:

TODO

All targets must have a :code:`name` and a :code:`type`:

  * **name** - The *name* identifies the target to the various rules.
  * **type** - The *type* indicates the concrete type of the target.  When referencing the built-in NuLog targets, you can omit the assembly name from the type string; but if you are referencing a third party, or custom target, you'll need to include the assembly, as is standard when identifying a type: *"Some.Assembly.Namespace.Path.ToATarget, Some.Assembly"*.

----

Trace Target
============
**NuLog.Targets.TraceTarget**

lorem ipsum.

----

Console Target
==============
**NuLog.Targets.ConsoleTarget**

lorem ipsum.

----

Text File Target
================
**NuLog.Targets.TextFileTarget**

lorem ipsum.

----

Mail Target
===========
**NuLog.Targets.MailTarget**

lorem ipsum.
