.. rst-class:: hideheader

###################################
  Welcome to NuLog's Documentation!
###################################

.. rst-class:: center

.. image:: sitetitle.png

----

Welcome to the NuLog documentation!  These docs are under heavy development, and will be finished soon.  Check back soon!

Martin's First Law of Documentation
===================================
*Produce no document unless its need is immediate and significant.*

We feel that the best form of documentation for a software project, are well written business driven tests (:ref:`arch_princ`).

`Head over to GitHub <https://github.com/ivanpointer/NuLog/tree/master/Take2/NuLog.Tests>`_ to look through the unit and integration tests for the project.

What's Changed From Version 1
=============================
**Please be aware: this is a ground-up rewrite of NuLog, so only the high-level concepts and architectural pattern have been kept.** *This is a full-on breaking change, which is why the version number has been advanced to 2.X.*

Much of the functionality of version 1 has been rebuilt into version 2.  However, the functionality has been simplified, and some features were deprecated, and not brought over.  Here's a quick overview of what's changed:

  * **Runtime Config Helpers** - The runtime config helpers haven't been built into version 2.  This doesn't mean they won't be added in the future, but that they just haven't been needed yet.
  * **MEF Deprecated** - MEF, while quite powerful, also leads to obfuscating functionality from the developer, which could lead to unexpected behavior (from the developer's perspective).  The rewrite of the framework allows for easier extension, using the *Dependency Inversion Principle*, and MEF is no longer needed.
  * **Static Meta Data Providers Deprecated** - The static meta data providers haven't been shown to be needed yet.  They don't conflict with the new philosophy of NuLog, but are an example of complexity that hasn't been shown to be necessary yet, so they haven't been ported over - at least yet.
  * **Configuration Changed from JSON to use standard XML-based config** - To better support standard CI processes, and to simplify the management of the configuration of the framework, the configuration has been moved away from a custom JSON based mechanism, to leverage the built-in configuration management.  The configuration is now stored as a custom configuration section, right in the app/web config files.
  * **Legacy Logging Extension Deprecated** - If neccessary, we can build this out as a separate package, but NuLog is a new approach to logging, and we encourage organizations to make the transition towards tag-based logging, as opposed to just emulating level-based logging, using a tag based framework.
  * **Configuration Extenders Deprecated** - Another example of complexity that isn't yet necessary - the configuration extenders haven't been ported over.  Again, if the need presents, we can build this out.
  * **Console Targets Simplified** - The two console targets have been replaced with a single console target, which is somewhere between the two of the original, in terms of functionality.
  * **Target Configuration Simplified** - Target configuration has been simplified to a name, type and then a series of key/value pairs for configuration.  The complexity of the previous targets was unneccessary.  If needed, we can extend the target configuration to support storing the original XML configuration as a part of the TargetConfig class, but we'll refrain from adding this, until it is needed.
  * **Target Lifecycle Simplified** - Targets no longer are concerned with managing their own message queue.  The previous version of NuLog maintained a queue both in the dispatcher, and in each target.  Having queues in both places adde complexity, but didn't add any convenience or performance.  The new version only has a queue in the dispatcher, targets are now "single threaded" in terms of lifecycle.
  * **Observer Pattern for Configuration Removed** - The observer pattern for the previous system has been removed.  Changes to the web configuration are handled in IIS by performing a blue/green recycle of the application, removing the need to "watch" the configuration for changes.

----

.. toctree::
   :maxdepth: 2
   :caption: User Documentation
   
   user_documentation/gettingstarted
   user_documentation/configuration/configtemplate
   user_documentation/configuration/generalsettings
   user_documentation/configuration/rules
   user_documentation/configuration/taggroups
   user_documentation/configuration/staticmetadata
   user_documentation/configuration/standardlayout
   user_documentation/targets
   user_documentation/advancedusage

.. toctree::
   :maxdepth: 2
   :caption: Customizing NuLog

   customizing_nulog/architecture
   customizing_nulog/customtargets
   customizing_nulog/loggerfactlogmanager
   
.. toctree::
   :maxdepth: 2
   :caption: Additional Information

   introtologging
   intrototaglogging
   about
   performance
   license
