# DRAFT-Unity-Runnables-Robust-Tweeners-And-Timeouts
![ezgif-3-a43e66f856](https://github.com/michaladamcichy/DRAFT-Unity-Runnables-Robust-Tweeners-And-Timeouts/assets/33597927/8dd841fc-608f-4aa0-9eef-ae353374f3f0)

A draft implementation of tweeners and other coroutine related tools. Existing frameworks didn't meet my needs.

Code needs revision, improvements, optimization. However, I found the concept worth sharing.

**My key goals:**

1. I want to run coroutines changing some variables without necessity of writing a new IEnumerator method each time.
2. I want to create sequences of coroutines and also run coroutines in parallel with an expressive and readable chains of method calls.
3. I want to interpolate variables using easing functions (I use https://easings.net/ as reference).

**Framework overview**

There are:
1. Tweens: change value from A to B in x seconds time OR change value from A to B with a given speed.
2. Timeouts: wait x seconds and then execute the callback method.
3. Intervals: run a callback each x seconds OR run a callback every frame.

You can create sequences of those.

**How to create a sequence:**

First derive your class from RunnableMonoBehaviour instead of MonoBehaviour.
Then override Update method.

```
class MyClass : RunnableMonoBehaviour
{
  protected override void Update()
  {
    base.Update();
  }
}
```
A RunnableMonoBehaviour is a handler of asynchronous tasks, which I call Runnables.
Each runnable will be created for a specific instance of RunnableMonoBehaviour and handled by it.


**How to create a sequence of Runnables using methods of RunnableMonoBehaviour**

Use Name method if you want to access your sequence by name later.

```
Name("sequence name")
  ...
  ...
```
It makes sense to name the sequence after the variable we are manipulating:
```
Name(nameof(variable))
  ...
  ...
```
If we don't use a Name method, the sequence will become anynomous, accessible via the return value of the method chain.

Now you can add Tweens, Timeouts and Intervals to the sequence.

**Setting up a tween** (we need a getter and a setter method, so the tweener can read and modify the variable)
```
Name(nameof(floatVariable))
  .Tween(() => floatVariable, (object value) => { floatVariable = (float) value; })
  ...;

var sequence = Tween(() => floatVariable, (object value) => { floatVariable = (float) value; })
  ...;
```
**Animating the value:**

```  
Name(nameof(floatVariable))
  .Tween(() => floatVariable, (object value) => { floatVariable = (float) value; })
  .Animate(1f, 2f) //interpolate towards value 1f, within time of 2 seconds 
  .Animate(-1f, 2f) // then interpolate towards -1f within next 2 seconds
```

**Adding Timeouts:**

```
Name("say goodbye")
  .Timeout(5f, () => { Debug.Log("Goodbye!"); }); //print after 5 seconds
```
```
Name(nameof(floatVariable))
  .Tween(() => floatVariable, (object value) => { floatVariable = (float) value; })
  .Animate(1f, 2f) 
  .Animate(-1f, 2f) 
  .Timeout(1f, () => /*do something 1 second after previous animations ends*/)
  .Animate(1f, 1f); //then do 1 more animation
```
**Intervals:**

```
Name("do it every 5 seconds)
  .Interval(5f, () => { /**/});
```
```
Name("do it every frame")
  .Interval(() => { /**/});
```

Intervals can be add to chain but since they loop untill they're stoped using Stop() methods, they must be last in the sequence.
Interfaces of chain nodes enforce this, as well as some other rules. Look at the graph below:

**What methods can I call from various chain nodes?**

![image](https://github.com/michaladamcichy/DRAFT-Unity-Runnables-Robust-Tweeners-And-Timeouts/assets/33597927/cb11d45f-887a-47a2-9df4-514194840939)

**Then(Action action)** - you can add a callback somewhere in the chain, so an additional action is executed after some other node in chain.

```
Name(nameof(floatVariable))
  .Tween(() => floatVariable, (object value) => { floatVariable = (float) value; })
  .Animate(1f, 2f) 
  .Animate(-1f, 2f)
  .Then(() => /*do something*/) 
  .Timeout(1f, ())
  .Animate(1f, 1f)
  .Then(() => /*do something*/);
```

**It's only a part of implemented features.
Documentation to be continued.**

