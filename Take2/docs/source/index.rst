Welcome to NuLog's documentation!
=================================

A blurb about NuLog here.

Martin's First Law of Documentation
===================================
*Produce no document unless its need is immediate and significant.*

We feel that the best form of documentation for a software project, are well written business driven tests (:ref:`arch_princ`).

`Head over to GitHub <https://github.com/ivanpointer/NuLog/tree/master/Take2/NuLog.Tests>`_ to look through the unit and integration tests for the project.

What's Changed From Version 1
=============================
Much of the functionality of version 1 has been rebuilt into version 2.  However, the functionality has been simplified, and some features were deprecated, and not brought over.  Here's a quick overview of what's changed:

  * **Runtime Config Helpers** - The runtime config helpers...
  * **MEF Deprecated**
  * **Static Meta Data Providers Deprecated**
  * **Configuration Changed from JSON to use standard XML-based config**
  * **Legacy logging extension deprecated**
  * **Configuration Extenders Deprecated**
  * TODO - CONTINUE HERE

.. toctree::
   :maxdepth: 2
   :caption: User Documentation
   
   user_documentation/gettingstarted
   user_documentation/configuration/index
   user_documentation/advancedusage

.. toctree::
   :maxdepth: 2
   :caption: Customizing NuLog

   customizing_nulog/architecture
   customizing_nulog/loggerfactlogmanager
   
.. toctree::
   :maxdepth: 2
   :caption: Additional Information

   about
   performance
   license
