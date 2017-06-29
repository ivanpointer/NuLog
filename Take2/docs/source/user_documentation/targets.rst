.. rst-class:: center

.. image:: sitetitle.png

----

#########
  Targets
#########

Configuration
=============

NuLog comes with a number of built-in targets.  Targets are configured within the :code:`<targets>` section of the NuLog configuration:

.. literalinclude:: /../../NuLogSnippets/TargetsConfig.config
   :language: xml
   :lines: 1-
   :tab-width: 4
   :emphasize-lines: 9
   :linenos:

All targets must have a :code:`name` and a :code:`type`:

  * **name** - The *name* identifies the target to the various rules.
  * **type** - The *type* indicates the concrete type of the target.  When referencing the built-in NuLog targets, you can omit the assembly name from the type string; but if you are referencing a third party, or custom target, you'll need to include the assembly, as is standard when identifying a type: *"Some.Assembly.Namespace.Path.ToATarget, Some.Assembly"*.

----

Trace Target
============
**NuLog.Targets.TraceTarget**

The trace target writes log events to `Trace`:

.. literalinclude:: /../../NuLogSnippets/TraceTargetConfig.config
   :language: xml
   :lines: 9-10
   :tab-width: 4
   :dedent: 3
   :linenos:

The trace target has the following properties:

  * **layout** - *Optional* - Defines the layout format for the target.  By default, this is a standard layout, as documented in :ref:`standardlayout`.

----

Console Target
==============
**NuLog.Targets.ConsoleTarget**

The console target writes log events to `Console`:

.. literalinclude:: /../../NuLogSnippets/ConsoleTargetConfig.config
   :language: xml
   :lines: 9-12
   :tab-width: 4
   :dedent: 3
   :linenos:

The console target has the following properties:

  * **layout** - *Optional* - Defines the layout format for the target.  By default, this is a standard layout, as documented in :ref:`standardlayout`.
  * **background** - *Optional* - An optional override to the `Background Color <https://msdn.microsoft.com/en-us/library/system.consolecolor(v=vs.110).aspx>`_ of messages written to the console.
  * **foreground** - *Optional* - An optional override to the `Foreground Color <https://msdn.microsoft.com/en-us/library/system.consolecolor(v=vs.110).aspx>`_ of messages written to the console.

----

Text File Target
================
**NuLog.Targets.TextFileTarget**

The text file target writes log events to a text file:

.. literalinclude:: /../../NuLogSnippets/TextFileTargetConfig.config
   :language: xml
   :lines: 9-11
   :tab-width: 4
   :dedent: 3
   :linenos:

The text file target has the following properties:

  * **path** - *Required* - The path to the text file to log to.  Can be relative, or absolute.
  * **layout** - *Optional* - Defines the layout format for the target.  By default, this is a standard layout, as documented in :ref:`standardlayout`.

----

Mail Target
===========
**NuLog.Targets.MailTarget**

The mail target sends log events via a SMTP server:

.. literalinclude:: /../../NuLogSnippets/MailTargetConfig.config
   :language: xml
   :lines: 9-23
   :tab-width: 4
   :dedent: 3
   :linenos:

The mail target has the following properties:

  * **subject** - *Required* - The subject line of the email.  This is a layout format, as documented in :ref:`standardlayout`.
  * **to** - *Required* - A semi-colon delimited list of email addresses to send the email to.
  * **from** - *Required-ish* - Required if not set in the application/web config, in the :code:`<system.net>` section.  The email address to send the message *"from"*.
  * **smtpServer** - *Required-ish* - Required if not set in the application/web config, in the :code:`<system.net>` section.  The network address of the SMTP server to send email messages through.
  * **body** - *Optional* - An optional override for the layout format for the body of the email.  By default, this is a standard layout, as documented in :ref:`standardlayout`.
  * **html** - *Optional* - If set to `true`, signals that the body of the email is HTML.  Defaults to `false`.
  * **convertNewlineInHtml** - *Optional* - If set to `true`, replaces newline characters in the body of the email with :code:`<br />` tags.
  * **smtpUserName** - *Optional* - The username for authenticating to the SMTP server.  If given, the password is required:
  * **smtpPassword** - *Optional-ish* - The password for authenticating to the SMTP server.  Required if the user name is given.
  * **enableSsl** - *Optional* - If set to `true`, will use SSL when connecting to the SMTP server.
  * **smtpPort** - *Optional* - The port to connect to the SMTP server on.
  * **smtpDeliveryMethod** - *Optional* - The `SMTP Delivery Method <https://msdn.microsoft.com/en-us/library/system.net.mail.smtpdeliverymethod(v=vs.110).aspx>`_ for sending the email through.  Defaults to :code:`Network`.
  * **pickupDirectory** - *Optional-ish* - Required if `SpecifiedPickupDirectory` is set for the `smtpDeliveryMethod`.  The path of the directory to which to write mail messages.
  * **timeout** - *Optional* - The timeout value, in milliseconds, for when connecting to the SMTP server to send a mail message.
