using System;
using UnityEngine;

public class ActiveNode : RunnableChainNode, IActiveNode
{
    public ActiveNode(bool force, string parentName, string name, Runnabler runnableMonoBehaviour) : base(force, parentName, name, runnableMonoBehaviour)
    {
    }
    public SetupTweenNode Tween(Func<object> currentValue, Action<object> updateValue)
    {
        return new SetupTweenNode(ShouldForce(), GetParentName(), GetName(), GetRunnabler(), currentValue, updateValue);
    }

    public ActiveNode Timeout(float duration, Action callback)
    {
        return GetRunnabler().Queue(ShouldForce(), GetParentName(), GetName(), new _Timeout(duration, callback));
    }

    public ActiveNode Timeout(float duration)
    {
        return Timeout(duration, () => { }); //alert
    }

    public FinalNode Interval(float interval, Action callback)
    {
        var node = GetRunnabler().Queue(ShouldForce(), GetParentName(), GetName(), new _Interval(interval, callback));
        return new FinalNode(node.ShouldForce(), GetParentName(), GetName(), GetRunnabler());
    }
    public FinalNode Interval(Action callback)
    {
        var node = GetRunnabler().Queue(ShouldForce(), GetParentName(), GetName(), new _Interval(null, callback));
        return new FinalNode(node.ShouldForce(), GetParentName(), GetName(), GetRunnabler());
    }

    public ActiveNode Parallel()
    {
        return GetRunnabler().Fork(GetName());
    }

    public ActiveNode Then(Action action)
    {
        return Timeout(0f, action); //alert todo more efficiently
    }

    public SetupTweenNode Position()
    {
        return Tween(() => GetRunnabler().GetParent().transform.position, (object value) => { GetRunnabler().GetParent().transform.position = (Vector3)value; });
    }

    public SetupTweenNode LocalPosition()
    {
        return Tween(() => GetRunnabler().GetParent().transform.localPosition, (object value) => { GetRunnabler().GetParent().transform.localPosition = (Vector3)value; });
    }

    public SetupTweenNode EulerAngles()
    {
        return Tween(() => GetRunnabler().GetParent().transform.eulerAngles, (object value) => { GetRunnabler().GetParent().transform.eulerAngles = (Vector3)value; });
    }

    public SetupTweenNode LocalEulerAngles()
    {
        return Tween(() => GetRunnabler().GetParent().transform.localEulerAngles, (object value) => { GetRunnabler().GetParent().transform.localEulerAngles = (Vector3)value; });
    }
    public SetupTweenNode LocalScale()
    {
        return Tween(() => GetRunnabler().GetParent().transform.localScale, (object value) => { GetRunnabler().GetParent().transform.localScale = (Vector3)value; });
    }

    public SetupTweenNode Transform()
    {
        return Tween(() => {
            var transform = GetRunnabler().GetParent().transform;


            return Runnabler.To(transform);
        }, 
        (object value) => {
            var t = (Tuple<Vector3, Vector3, Vector3>) value;
            Runnabler.Set(GetRunnabler(), t);
        });
    }
}
