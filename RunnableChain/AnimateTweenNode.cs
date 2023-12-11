using System;
using UnityEngine;

public class AnimateTweenNode : TweenNode
{
    public AnimateTweenNode(bool force, string parentName, string name, RunnableMonoBehaviour runnableMonoBehaviour, Func<object> currentValue, Action<object> updateValue)
        : base(force, parentName, name, runnableMonoBehaviour, currentValue, updateValue)
    {
    }

    public virtual ActiveTweenNode Animate(float target, float duration, Func<float, float> easeFunction = null)
    {
        if (easeFunction == null)
            easeFunction = GetRunnableMonoBehaviour().GetDefaultEaseFunction();

        return GetRunnableMonoBehaviour().Queue(ShouldForce(), GetParentName(), GetName(), new _FloatTween(ShouldForce(), target, duration, GetCurrentValueFunction(), GetUpdateValueAction(), easeFunction));
    }

    public virtual ActiveTweenNode AnimateWithSpeed(float target, float speed, Func<float, float> easeFunction = null)
    {
        var time = Mathf.Abs(target - (float) GetCurrentValueFunction()()) / speed;

        return Animate(target, time, easeFunction);
    }
    public virtual ActiveTweenNode Animate(Vector3 target, float duration, Func<float, float> easeFunction = null)
    {
        if (easeFunction == null)
            easeFunction = GetRunnableMonoBehaviour().GetDefaultEaseFunction();

        return GetRunnableMonoBehaviour().Queue(ShouldForce(), GetParentName(), GetName(), new _Vector3Tween(ShouldForce(), target, duration, GetCurrentValueFunction(), GetUpdateValueAction(), easeFunction));
    }

    public virtual ActiveTweenNode AnimateWithSpeed(Vector3 target, float speed, Func<float, float> easeFunction = null)
    {
        var time = (target - (Vector3) GetCurrentValueFunction()()).magnitude / speed;

        return Animate(target, time, easeFunction);
    }
}
