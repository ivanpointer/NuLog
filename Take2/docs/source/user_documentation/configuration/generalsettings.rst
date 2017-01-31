.. rst-class:: center

.. image:: \images\sitetitle.png

----

##################
  General Settings
##################

.. highlight:: xml

Stack Frame
===========
By default, the stack frame of the logging method is not included in log events.  This stack frame is needed if you want to log the name of the method which made the log call (see :ref:`standardlayout` for more information).

To include the calling stack frame in the log events, turn it on on the main :code:`<nulog>` element in the config:

.. literalinclude:: /../../NuLogSnippets/GeneralSettingsApp.config
   :lines: 1-7,9-11
   :tab-width: 4
   :emphasize-lines: 7
   :linenos:

Fallback Logging
================
By default, failures within NuLog are logged to `Trace`.  To log to a text file, instead, set the `fallbackLog` to the file to log to.  The path can be relative, or absolute:

.. literalinclude:: /../../NuLogSnippets/GeneralSettingsApp.config
   :lines: 1-6,8-11
   :tab-width: 4
   :emphasize-lines: 7
   :linenos:
