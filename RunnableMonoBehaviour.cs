using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

public class RunnableMonoBehaviour : MonoBehaviour
{
    Dictionary<string, Queue<_Runnable>> nameToQueue_ = new Dictionary<string, Queue<_Runnable>>();
    Dictionary<string, string> nameToParent_ = new Dictionary<string, string>();
    
    Func<float, float> defaultEaseFunction_ = EaseFunction.EaseInOutSine;

    long id = 0;
    string GenerateName()
    {
        var name = (++id).ToString();

        while (nameToQueue_.ContainsKey(name))
        {
            name = (++id).ToString();
        }

        return name;
    }

    internal ActiveNode Queue(bool force, string parentName, string name, _Runnable runnable)
    {
        Debug.Log("spawn " + name);

        if (!nameToQueue_.ContainsKey(name))
            nameToQueue_[name] = new Queue<_Runnable>();

        var queue = nameToQueue_[name];
        queue.Enqueue(runnable);

        return new ActiveNode(force, parentName, name, this);
    }

    internal ActiveTweenNode Queue(bool force, string parentName, string name, _Tween tween)
    {
        Queue(force, parentName, name, (_Runnable) tween);

        return new ActiveTweenNode(force, parentName, name, this, tween.GetCurrentValueFunction(), tween.GetUpdateAction());
    }

    internal ActiveTweenNode Queue(bool force, _Tween tween)
    {
        var name = GenerateName();
        return Queue(force, name, name, tween);
    }

    internal ActiveNode Queue(bool force, _Runnable runnable)
    {
        var name = GenerateName();
        return Queue(force, name, name, runnable);
    }

    internal ActiveTweenNode ForkTween(string parentName, Func<object> currentValue, Action<object> updateValue)
    {
        var name = GenerateName();

       
        nameToParent_[name] = parentName; //alert nie czyszczę nigdy tych struktur

        return new ActiveTweenNode(false, parentName, name, this, currentValue, updateValue);
    }
    internal ActiveNode Fork(string parentName)
    {
        var name = GenerateName();
        nameToParent_[name] = parentName;

        return new ActiveNode(false, parentName, name, this);
    }

    public SetupNode Name(string name)
    {
        return new SetupNode(false, name, name, this);
    }

    public SetupTweenNode Tween(Func<object> currentValue, Action<object> updateValue)
    {
        var name = GenerateName();
        return new SetupTweenNode(false, name, name, this, currentValue, updateValue);
    }

    public ActiveNode Timeout(float duration, Action callback)
    {
        var name = GenerateName();
        return Queue(false, name, name, new _Timeout(duration, callback));
    }

    public FinalNode Interval(float interval, Action callback)
    {
        var node = Queue(false, new _Interval(interval, callback));
        return new FinalNode(false, node.GetName(), node.GetName(), this);
    }

    public FinalNode Interval(Action callback)
    {
        var node = Queue(false, new _Interval(null, callback));
        return new FinalNode(false, node.GetName(), node.GetName(), this);
    }

    public SetupTweenNode Position()
    {
        var name = GenerateName();
        return new ActiveNode(false, name, name, this).Position();
    }

    public SetupTweenNode LocalPosition()
    {
        var name = GenerateName();
        return new ActiveNode(false, name, name, this).LocalPosition();
    }
    public SetupTweenNode EulerAngles()
    {
        var name = GenerateName();
        return new ActiveNode(false, name, name, this).EulerAngles();
    }

    public SetupTweenNode LocalEulerAngles()
    {
        var name = GenerateName();
        return new ActiveNode(false, name, name, this).LocalEulerAngles();
    }
    public SetupTweenNode LocalScale()
    {
        var name = GenerateName();
        return new ActiveNode(false, name, name, this).LocalScale();
    }

    public void Stop(string name)
    {
        Debug.Log("stop " + name);

        if (!nameToQueue_.ContainsKey(name))
            return;

        var queue = nameToQueue_[name];

        if (queue.Count == 0)
            return;

        var first = queue.Dequeue();
        first.Stop();

        nameToQueue_.Remove(name);
        nameToParent_.Remove(name);

        foreach(var item in nameToParent_.Where(item => item.Value == name).ToList()) //alert efficiency
        {
            Stop(item.Key);
        }
    }

    public void Stop()
    {
        foreach(var name in nameToQueue_.Keys)
        {
            Stop(name);
        }
    }

    virtual protected void Update()
    {
        var queues = nameToQueue_.Values.ToList();

        foreach (var queue in queues)
        {
            if (queue.Count == 0) continue;

            var runnable = queue.Peek();

            if (!runnable.HasStarted())
            {
                StartCoroutine(runnable.Run());
                continue;
            }

            if (!runnable.IsStopped())
                continue;

            queue.Dequeue();

            if (queue.Count == 0)
                continue;

            var next = queue.Peek();

            StartCoroutine(next.Run());
        }
    }

    public Func<float, float> GetDefaultEaseFunction()
    {
        return defaultEaseFunction_;
    }
    protected void SetDefaultEaseFunction(Func<float, float> easeFunction)
    {
        defaultEaseFunction_ = easeFunction;
    }
}
