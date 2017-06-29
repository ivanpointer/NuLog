.. rst-class:: center

.. image:: sitetitle.png

----

########################
  Architectural Overview
########################

Logger, Dispatcher and Target
=============================
lorem ipsum.

----

.. _arch_princ:

Architectural Principles
========================

Here are our guiding principles, for the NuLog project.  These principles come together to form the "philosophy" of the project:

  #. **Honest Mistakes, and Honesty in General** - We tolerate people's honest mistakes.  In fact, we encourage them.  This means malicious finger-pointing is not tolerated, and may get you banned from the project.  It is up to me (Ivan Pointer), to decide if someone is being malicious, or not.  I want people to feel the freedom needed to take risks, honestly.  I expect high integrity of all our official members.
  
  #. **Simple** - Period.  Avoid introducing complexity until it is *necessary*, and no sooner.  It is the simplicity of the framework that gives it power.
  
  #. **NuLog is a Logging Framework** -  Not a message bus or queue.  NuLog shouldn't be handling notifications out to your end users, or acting as a bus to deliver messages between your application's tiers.
  
  #. **Unassuming** - NuLog needs to stay out of the way.  If NuLog breaks, it needs to break "silently" without letting exceptions bubble up.  Leveraging NuLog cannot get in the way of the developer, but must instead, support the developer.  NuLog is a useful tool.  NuLog must not be a helpful tool.  Useful tools require the developer to take action, whereas helpful tools take action on behalf of the developer.
  
  #. **Tag-Based** - Offers more simplicity, and more flexibility, than the traditional "level" based logging.
  
  #. **Extensible** - Build using the *'Dependency Inversion Principle <https://en.wikipedia.org/wiki/Dependency_inversion_principle>'_*.  Developers who leverage the framework must be able to replace any portion of the framework with their own pieces, given that they conform to the *'Liskov Substitution Principle <https://en.wikipedia.org/wiki/Liskov_substitution_principle>'_*.
  
  #. **SOLID Principles** - We adhere to the *'SOLID Principles <https://en.wikipedia.org/wiki/SOLID_(object-oriented_design)>'_*, as best we reasonably can.

  #. **Business-Driven-Development** - We believe that the best documentation of a system are well written unit and integration tests.  We demand that there be *near* complete code coverage, and the checks must be high quality.  All code that can be tested, must be done so before said code is written.  Tests are first to document the expected behavior of the code, and second, have an ansilary benifit of verifying that the implementation actually does what is expected.  * **Any code (pull requests, etc.) that is not properly covered will be rejected.** *
  
  #. **Continuous-Integration** - Continuous integration is a **must**.  We recognize and accept that CI is a discipline, and not a set of tools.  We embrace completely the discipline of CI. **This means that if you break it, you fix it:** If you break the build, you are expected to fix the build.  We don't tolerate a broken build being left to rot: if the build breaks, all other changes/work stops until the build is fixed.
  
  #. **Perfromant** - Performance is a high priority of the NuLog project.  This said - code first for readability and maintainability.  After this, use performance analysis tools to identify hot-spots in the code, and very clearly document any "smells" that are *necessary* for performance.  If it isn't *necessary*, don't do it.

  #. **No External Dependencies** - No third-party libraries should be required to use the core of NuLog.  A logging framework shouldn't pull in any other dependencies, and should be unassuming.  This means no *IoC* containers, or even a JSON parsing library.  The core project needs to only reference the standard .Net assemblies.

  #. **External Dependencies in Extension Libraries** - Extenral dependencies need to be brought in through extension libraries.  An example of this would be a separate project for a target which posts log events to a `Slack <https://slack.com/>`_ channel.

  #. **NO TOLERANCE FOR GPL. PERIOD.** - GPL is not to come within 1,000 miles of NuLog.  GPL is a cancer the likes this world hasn't seen before, and as *mamma says, GPL is the devil*.  But-**NO.  PERIOD.**  We prefer MIT and Apache 2.0 around here.  This same policy applies for any *copyleft* style licenses, which are designed to restrict your freedom and rights, as opposed to protecting them.

----

Architectural Policies
======================

These policies are more fluid than the principles, and are therefore, more flexible, and change more frequently.  They are also more detailed in nature.  These help to refine the vision set forth by the principles:

  #. **Fallback Loggers Should be Independent** - The fallback loggers should not leverage the targets, or other parts of the NuLog system, to perform their duties.  The functionality of the fallback loggers need to be completely contained within the fallback loggers.  The reason for this is simple: we need the fallback loggers to work, even when targets and other things are breaking.  If the fallback logger depends on a target's implementation, when that target fails, so will the fallback logger, and consequently, the developer will not be informed of the failure.

  #. **Using FakeItEasy for a mocking framework** - I was tempted to not even use a mocking framework, especially once I saw that Moq had a BSD license. After a little searching, I found FakeItEasy, which is under the MIT license, and has had a fairly active community. Adding a mocking framework won't add any dependencies to NuLog itself, as the tests aren't distributed with the library. FakeItEasy will definitely decrease complexity. Between the loose coupling, the reduction in complexity, and the friendly license, I've decided to leverage FakeItEasy for some of the more complex tests.
  
  #. **Text File Target: No rotation/archiving** - There are a lot of different log aggregation frameworks that handle this, and, it's pretty easy to set this up with PowerShell, Batch, Shell, etc. This is generally something that system administrators prefer to manage externally. Because of this, the text file target's focus will be to be very "hands off" of the text file it produces - avoiding keeping file handles open, etc. - to play nicer with a separate process performing log management.

  #. **Simple Email Target: This is for logging, not message queuing** - Not only does it significantly increase the complexity of the target, but for a purpose I believe to be out of scope of the core purpose of NuLog. Adding "advanced" features would encourage the abuse of the logging system, as use as a notification engine - which NuLog is not. This doesn't prevent a later "extended" email target (as an add-on package, or 3rd party contribution, perhaps).
