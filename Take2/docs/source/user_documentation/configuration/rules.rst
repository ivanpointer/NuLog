.. rst-class:: center

.. image:: \images\sitetitle.png

----

###################
  Rules for Routing
###################

.. highlight:: xml

Introduction to Rules
=====================
Jumping straight in, here's an example rule configuration:

.. literalinclude:: /../../NuLogSnippets/RulesConfig.config
   :tab-width: 4
   :emphasize-lines: 9-14
   :linenos:
  
The :code:`<rules>` section of the NuLog config can contain more than one :code:`<rule>`.  Rules are processed in order, from top to bottom.  Let's walk through each of the attributes of a rule:

  * **include** - A list of tags, comma separated, for include in the rule. One or all of these tags must be present in the log event for the rule to be considered a match.
  * **exclude** - A list of tags, comma separated, for exclude in the rule. If any of these tags are present in the log event, the rule is considered not a match.
  * **targets** - A list of targets, comma separated, that the log event is to be dispatched to if the rule is found to be a match.  Targets are referenced by their configured name, not their type.
  * **strictInclude** - If this flag is set, all of the tags listed in include must be present in the log event for the rule to be considered a match.
  * **final** - A flag, that if set and this rule is considered a match, no further rules will be processed for the log event.

Wildcards
=========
The :code:`*` character can be used as a wildcard for matching tags in the `include` and `exclude` attributes.  Wildcards match zero, or more, of any character.  Wildcards can appear anywhere in the tag, and can be used more than once; for example: :code:`*hello*world*`.  In the `include` attribute, you can use a single wildcard: :code:`*`, to match any (or all) tags.

Automatic Tags
==============
There are some tags added automatically when logging:

  * **Full Class Name** - The full class name, from which the log event was logged, is added as a tag to each call.  This enables us to write rules that target any portion of the namespace, up to, and including the specific class name.  For example, the tag :code:`NuLog.CLI.PerfTune.Program` would be added when logging from a method in the `Program` class in the `NuLog.CLI.PerfTune` namespace.  One limitation here, is that tags are case insensitive, but C# is case sensitive.  This means that if you have two classes in the same namespace that vary only by case, you won't be able to target just a single one, without adding extra tags to key off of.
  * **Exceptions** - When logging an exception, the :code:`exception` tag is automatically added to the log event, allowing us to target rules specifically to ones which contain exceptions.