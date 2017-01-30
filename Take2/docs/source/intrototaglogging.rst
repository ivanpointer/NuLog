.. rst-class:: center

.. image:: \images\sitetitle.png

----

.. _intrototaglogging:

###################################
  Introduction to Tag-Based Logging
###################################

Traditional logging frameworks log based on a "level" (in terms of loudness, or volume), such as `trace`, `debug`, `info` and up to `error` and `fatal`.  A sensitivity is configured, and anything equal to, or louder than the sensitivity, would be logged.  This style of logging allows for "filtering" the log messages based on the amount of noise desired.  This kind of filtering doesn't naturally support being more selective in which messages are filtered.

Tag-based logging facilitates a far-higher level of control in filtering log messages.  The various "levels" can be represented by tags, but also, so much more.  Instead of being limited to "loudness", log events can be tagged with any trait needed, such as the type of request being processed, or even the roles of the user associated with the event.  The possibilities are much more free than with traditional logging frameworks.

**NuLog follows a basic process for handling log events:**

  * **Tags** are assigned to log events
  * **Rules** determine which log targets specific log events are dispatched to using the assigned tags. Not all log events are dispatched to all log targets
  * **Targets** are responsible for handling the log events dispatched to them

----

Tags
====

Tags can represent practically anything pertaining to a log event:

  * A particular target, such as "database" or "file"
  * A particular status or event, such as "exception" or "authenticated"
  * A particular source of log events, such as "SomeMVCApp.SomeController"
  * NuLog automatically includes the full class name of the logging class as a tag on the log event
  * Any other helpful grouping!

Targets
=======

A single target represents a single destination for log events. A target can represent:

  * A text file
  * A database table
  * A web service for logging
  * A console window
  * A trace log
  * Any other custom log event destination

Rules
=====

Rules define which targets a log event is dispatched to using the tags associated to said log event. Rules can be defined with:

  * Which tags are included in the rule (include)
  * Which tags are excluded from the rule, (specifically from those selected by "include") (exclude)
  * Whether or not all tags defined in "include" must be present in the log event for the log event to match the rule (strictInclude)
  * Which targets matching log events are to be dispatched to (targets)
  * Whether or not to process any further rules for the log event if the current rule matches (final)
