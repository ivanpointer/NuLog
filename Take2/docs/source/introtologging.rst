.. rst-class:: center

.. image:: \images\sitetitle.png

----

.. _introtologging:

#########################
  Introduction to Logging
#########################

The Basics
==========

  * Logging consists of two simple elements: log events and log targets. For example, a family's signature on a guest role at a wedding, or a security guard's notes in his notebook.
  * In software, logging is primarily used for troubleshooting/debugging and auditing purposes.
  * A logging framework is responsible for providing a structured way to get log events into log targets.

Log Events
==========

  * Log events are messages or objects to be delivered to a log target (the family's signature or the security guard's note)
  * Log events contain information about an event that happened
  * A log event can be a simple message like "Hello, World!", or a complex multilevel object that represents a web request

Log Targets
===========

  * Log targets receive or store log events
  * A log target can display a log event immediately, such as in a terminal window, or store the log event to persistent storage, such as in a file or database
  * Log targets can also interface web services or email clients, delivering the log event to an external destination
  * A single log target is generally responsible for a single destination. For example, a single "Text File Target" would write to a single file
  * A single application will likely have multiple targets