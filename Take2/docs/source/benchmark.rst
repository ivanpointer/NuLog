.. rst-class:: center

.. image:: sitetitle.png

----

#############
  Performance
#############

The initial build of NuLog focuses on stability and usability.  Future releases will focus on benchmarking and performance more.

This said, NuLog still compares well to the other leading logging frameworks.

----

Benchmarking
============

Trace Benchmark
---------------

.. image:: benchmark-trace.png

Writing 10,000 log events to trace, average of 10 samples:

* **NuLog** - Writes at a rate of 53,432 messages per second.
* **Log4Net** - Writes at a rate of 7,531 messages per second.
* **NLog** - Writes at a rate of 30,134 messages per second.

Console Benchmark
-----------------

.. image:: benchmark-console.png

Writing 10,000 log events to console, average of 10 samples:

* **NuLog** - Writes at a rate of 5,497 messages per second.
* **Log4Net** - Writes at a rate of 4,327 messages per second.
* **NLog** - Writes at a rate of 9,080 messages per second.

Benchmark Interpretation
------------------------

The following assumptions are made about the trace and console benchmarks, taken together:

* NuLog has a very low overhead in routing log events to targets, when compared to the other logging frameworks.
* NuLog has room to improve in performance tuning for individual targets.
* NuLog falls between Log4Net and NLog in this simplistic performance comparison.
