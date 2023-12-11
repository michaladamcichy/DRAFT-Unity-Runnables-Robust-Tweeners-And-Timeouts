# DRAFT-Unity-Runnables-Robust-Tweeners-And-Timeouts

A draft implementation of tweeners and other coroutine related tools designed in such a way, that I would like to use such a framework.

My key goals:

1. I want to run coroutines changing some variables without necessity of writing a new IEnumerator method each time.
2. I want to create sequences of coroutines and also run coroutines in parallel with an expressive and readable chains of method calls.
3. I want to interpolate variables using easing functions (I use https://easings.net/ as reference).

