using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

public class Runnabler
{
    Dictionary<string, Queue<_Runnable>> nameToQueue_ = new Dictionary<string, Queue<_Runnable>>();
    Dictionary<string, string> nameToParent_ = new Dictionary<string, string>();
    
    Func<float, float> defaultEaseFunction_ = EaseFunction.EaseInOutSine;

    long id = 0;

    MonoBehaviour parent_;
    Tuple<Vector3, Vector3, Vector3> initialTransform_;
    
    public Runnabler(MonoBehaviour parent)
    {
        parent_ = parent;
        initialTransform_ = To(parent_.transform);

        var keyFramesObject = parent_.transform.parent?.Find(parent_.transform.name + "__KeyFrames");
        keyFramesObject?.gameObject.SetActive(false);
    }

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

    public SetupTweenNode Transform()
    {
        var name = GenerateName();
        return new ActiveNode(false, name, name, this).Transform();
    }

    public void Stop(string name)
    {
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

    public void StopIfExists(string name)
    {
        if (!nameToQueue_.ContainsKey(name))
            return;

        Stop(name);
    }

    public void Stop()
    {
        var names = nameToQueue_.Keys.ToList(); //alert
        foreach (var name in names)
        {
            StopIfExists(name);
        }

        var container = (IRunnablerContainer)this.GetParent();
        container.OnStop();
    }
    public void Update()
    {
        var queues = nameToQueue_.ToList();

        foreach (var (name, queue) in queues)
        {
            if (queue.Count == 0) //alert not tested ale chyba działa
            {
                nameToQueue_.Remove(name);
                nameToParent_.Remove(name);
                continue;
            }

            var runnable = queue.Peek();

            if (!runnable.HasStarted())
            {
                GetParent().StartCoroutine(runnable.Run());
                continue;
            }

            if (!runnable.IsStopped())
                continue;

            queue.Dequeue();

            if (queue.Count == 0)
                continue;

            var next = queue.Peek();

            GetParent().StartCoroutine(next.Run());
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

    public Tuple<Vector3, Vector3, Vector3> GetKeyFrame(string name) //alert efficiency
    {
        if (name == "Base")
            return BaseKeyFrame();

        var parent = GetParent().transform.parent;

        var keyFrame = parent.Find(GetParent().transform.name + "__KeyFrames/" + name);
        return To(keyFrame);
    }

    public Tuple<Vector3, Vector3, Vector3> GetWorldKeyFrame(string name) //alert efficiency
    {
        if (name == "Base")
            return BaseKeyFrame();

        var parent = GetParent().transform.parent;

        var keyFrame = parent.Find(GetParent().transform.name + "__KeyFrames/" + name);
        return ToGlobal(keyFrame);
    }


    //public Transform GetKeyFrame(Transform transform, string name) //alert efficiency
    //{
    //    var parent = transform.parent;

    //    return parent.Find("KeyFrames/" + name);
    //}

    public void SetKeyFrame(Transform transform)
    {
        //this.transform.localPosition = transform.localPosition;
        //this.transform.localEulerAngles = transform.localEulerAngles;
        //this.transform.localScale = transform.localScale;
        Set(this, To(transform));

    }
    public void SetKeyFrame(string keyFrame)
    {
        //this.transform.localPosition = transform.localPosition;
        //this.transform.localEulerAngles = transform.localEulerAngles;
        //this.transform.localScale = transform.localScale;
        Set(this, GetKeyFrame(keyFrame));
    }

    public Tuple<Vector3, Vector3, Vector3> BaseKeyFrame()
    {
        return initialTransform_;
    }

    public static Tuple<Vector3, Vector3, Vector3> ToLocal(Transform transform)
    {
        return new Tuple<Vector3, Vector3, Vector3>(transform.localPosition, transform.localEulerAngles, transform.localScale);
    }

    public static Tuple<Vector3, Vector3, Vector3> ToGlobal(Transform transform)
    {
        return new Tuple<Vector3, Vector3, Vector3>(transform.position, transform.eulerAngles, transform.localScale);
    }

    static bool isLocal = true;

    public static void SetLocal(Runnabler r, Tuple<Vector3, Vector3, Vector3> t)
    {
        r.GetParent().transform.localPosition = t.Item1;
        r.GetParent().transform.localEulerAngles = t.Item2;
        r.GetParent().transform.localScale = t.Item3;
    }

    public static void SetGlobal(Runnabler r, Tuple<Vector3, Vector3, Vector3> t)
    {
        r.GetParent().transform.position = t.Item1;
        r.GetParent().transform.eulerAngles = t.Item2;
        r.GetParent().transform.localScale = t.Item3; 
    }

    public static Tuple<Vector3, Vector3, Vector3> To(Transform transform)
    {
        if (isLocal)
            return ToLocal(transform);
        else
            return ToGlobal(transform);
    }

    public static void Set(Runnabler r, Tuple<Vector3, Vector3, Vector3> t)
    {
        if (isLocal)
            SetLocal(r, t);
        else
            SetGlobal(r, t);
    }

    public void SetSpeed(string name, float speed)
    {
        foreach(var runnable in nameToQueue_[name])
        {
            runnable.SetSpeed(speed);
        }

        foreach (var item in nameToParent_.Where(item => item.Value == name).ToList()) //alert efficiency
        {
            SetSpeed(item.Key, speed);
        }
    }

    public bool RunnableExists(string name)
    {
        return nameToQueue_.ContainsKey(name);
    }

    public MonoBehaviour GetParent()
    {
        return parent_;
    }

    public int GetRunnablesCount()
    {
        return nameToQueue_.Count;
    }
}
