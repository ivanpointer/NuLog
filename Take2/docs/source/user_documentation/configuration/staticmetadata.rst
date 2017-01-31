.. rst-class:: center

.. image:: \images\sitetitle.png

----

##################
  Static Meta Data
##################

.. highlight:: xml

Introduction to Static Meta Data
================================
*Static Meta Data* is meta data that is added to every log event generated through the system.

.. literalinclude:: /../../NuLogSnippets/StaticMetaDataConfig.config
   :lines: 1-6,12-
   :tab-width: 4
   :emphasize-lines: 9-11
   :linenos:

*Static Meta Data* is useful for including information about the environment the logger is running on, helping to identify problems in your infrastructure.  The benefit of including static meta data this way, is that these values can be transformed during a standard CI/CD process, being automatically updated by your favorite CI/CD tool, such as `Octopus Deploy <https://octopus.com>`_.

Updated Layout to Include Static Meta Data
==========================================
Once you have your static meta data in place, you could include it in a custom layout for your targets:

.. literalinclude:: /../../NuLogSnippets/StaticMetaDataConfig.config
   :lines: 1-12,19-
   :tab-width: 4
   :emphasize-lines: 10
   :linenos:

With the static meta data from the previous section, and this layout, :code:`${Release}-${Server}-${Env}` would be rendered as :code:`1.2.42-Web42B-Prod`.

For more information about layouts, see :ref:`standardlayout` (up next).
