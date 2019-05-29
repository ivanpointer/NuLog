.. rst-class:: center

.. image:: sitetitle.png

----

############
  Tag Groups
############

.. highlight:: xml

Introduction to Tag Groups
==========================

Tag groups were created to simplify managing tags and to allow for hierarchical tags. A list of tags are grouped under a single tag. Take a look at the configuration below as an example:

.. literalinclude:: /../../NuLogSnippets/TagGroupsConfig.config
   :lines: 1-6,13-17,23-
   :tab-width: 4
   :emphasize-lines: 9-11
   :linenos:

What this means:

  * **baseTag** - The base tag that the alias tags equate to.  If this tag is listed in a rule's :code:`include` or :code:`exclude` attributes, all of the tags listed as aliases would then qualify/satisfy the `include` or `exclude` requirement.
  * **aliases** - The list of tags, comma delimited, which are considered the same as the named base tag.

An Impractical Example: Fruit
=============================

Perhaps you wanted to simply be able to say :code:`fruit` in a rule to say that you wanted to send all log events containing `fruit` to be dispatched to a list of targets.  You could create a tag group for the base tag :code:`fruit`, and list under it :code:`apple`, :code:`pear`, :code:`tomato`, and on.

Now, I know you aren't likely to be using fruit as tags, but this could be used to group all user actions together, for example: :code:`useractions` could have the aliases :code:`login`, :code:`signout` and :code:`resetpassword`.  This becomes useful once you have more advanced tag routing, where you have multiple rules that would leverage the :code:`useractions` base tag.

A Practical Example: Traditional Log Levels
===========================================

Tag groups can be used to emulate the traditional behavior of log levels.  This is done by equating all of the lower levels, to a given level:

.. literalinclude:: /../../NuLogSnippets/TagGroupsConfig.config
   :lines: 1-6,13-14,18-
   :tab-width: 4
   :emphasize-lines: 9-13
   :linenos:

With these tag groups in place, we can then set a "level" to our rules:

.. literalinclude:: /../../NuLogSnippets/TagGroupsConfig.config
   :lines: 9-11
   :tab-width: 4
   :dedent: 3
   :emphasize-lines: 2
   :linenos:

If we look at our :code:`warn` tag group, it tells us that any log events with any of :code:`warn`, :code:`error` or :code:`fatal`, will cause the rule to match, and be sent to the target named `email`.
