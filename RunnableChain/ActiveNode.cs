using System;
using UnityEngine;

public class ActiveNode : RunnableChainNode, IActiveNode
{
    public ActiveNode(bool force, string parentName, string name, RunnableMonoBehaviour runnableMonoBehaviour) : base(force, parentName, name, runnableMonoBehaviour)
    {
    }
    public SetupTweenNode Tween(Func<object> currentValue, Action<object> updateValue)
    {
        return new SetupTweenNode(ShouldForce(), GetParentName(), GetName(), GetRunnableMonoBehaviour(), currentValue, updateValue);
    }

    public ActiveNode Timeout(float duration, Action callback)
    {
        return GetRunnableMonoBehaviour().Queue(ShouldForce(), GetParentName(), GetName(), new _Timeout(duration, callback));
    }

    public FinalNode Interval(float interval, Action callback)
    {
        var node = GetRunnableMonoBehaviour().Queue(ShouldForce(), GetParentName(), GetName(), new _Interval(interval, callback));
        return new FinalNode(node.ShouldForce(), GetParentName(), GetName(), GetRunnableMonoBehaviour());
    }
    public FinalNode Interval(Action callback)
    {
        var node = GetRunnableMonoBehaviour().Queue(ShouldForce(), GetParentName(), GetName(), new _Interval(null, callback));
        return new FinalNode(node.ShouldForce(), GetParentName(), GetName(), GetRunnableMonoBehaviour());
    }

    public ActiveNode Parallel()
    {
        return GetRunnableMonoBehaviour().Fork(GetName());
    }

    public ActiveNode Then(Action action)
    {
        return Timeout(0f, action); //alert todo more efficiently
    }

    public SetupTweenNode Position()
    {
        return Tween(() => GetRunnableMonoBehaviour().transform.position, (object value) => { GetRunnableMonoBehaviour().transform.position = (Vector3)value; });
    }

    public SetupTweenNode LocalPosition()
    {
        return Tween(() => GetRunnableMonoBehaviour().transform.localPosition, (object value) => { GetRunnableMonoBehaviour().transform.localPosition = (Vector3)value; });
    }

    public SetupTweenNode EulerAngles()
    {
        return Tween(() => GetRunnableMonoBehaviour().transform.eulerAngles, (object value) => { GetRunnableMonoBehaviour().transform.eulerAngles = (Vector3)value; });
    }

    public SetupTweenNode LocalEulerAngles()
    {
        return Tween(() => GetRunnableMonoBehaviour().transform.localEulerAngles, (object value) => { GetRunnableMonoBehaviour().transform.localEulerAngles = (Vector3)value; });
    }
    public SetupTweenNode LocalScale()
    {
        return Tween(() => GetRunnableMonoBehaviour().transform.localScale, (object value) => { GetRunnableMonoBehaviour().transform.localScale = (Vector3)value; });
    }
}
