.. rst-class:: center

.. image:: sitetitle.png

----

.. _configtemplate:

########################
  Configuration Template
########################

Introduction
============
The standard way to configure NuLog is via the standard custom configuration section in your :code:`web.config` or :code:`app.config` file.  By leveraging this configuration standard, NuLog benefits from the configuration management utilities provided by .Net, including the ability to apply configuration transforms during build/deploy, as is popular with many `CI/CD` solutions.

Copy Pasta
==========
Here's a bare-bones configuration for using NuLog.  This will direct all log events to :code:`Trace`:

.. highlight:: xml

.. literalinclude:: /../../NuLogSnippets/CopyPastaApp.config
   :lines: 1-5,9-25
   :tab-width: 4
   :emphasize-lines: 4,6-21
   :linenos:
