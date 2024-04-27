using System;
using UnityEngine;

public class AnimateTweenNode : TweenNode
{
    public AnimateTweenNode(bool force, string parentName, string name, Runnabler runnableMonoBehaviour, Func<object> currentValue, Action<object> updateValue)
        : base(force, parentName, name, runnableMonoBehaviour, currentValue, updateValue)
    {
    }

    public virtual ActiveTweenNode Animate(float target, float duration, Func<float, float> easeFunction = null)
    {
        if (easeFunction == null)
            easeFunction = GetRunnabler().GetDefaultEaseFunction();

        return GetRunnabler().Queue(ShouldForce(), GetParentName(), GetName(), new _FloatTween(ShouldForce(), target, duration, GetCurrentValueFunction(), GetUpdateValueAction(), easeFunction));
    }

    public virtual ActiveTweenNode AnimateRotation(float target, float duration, Func<float, float> easeFunction = null)
    {
        if (easeFunction == null)
            easeFunction = GetRunnabler().GetDefaultEaseFunction();

        return GetRunnabler().Queue(ShouldForce(), GetParentName(), GetName(), new _RotationFloatTween(ShouldForce(), target, duration, GetCurrentValueFunction(), GetUpdateValueAction(), easeFunction));
    }

    public virtual ActiveTweenNode AnimateWithSpeed(float target, float speed, Func<float, float> easeFunction = null)
    {
        var time = Mathf.Abs(target - (float) GetCurrentValueFunction()()) / speed;

        return Animate(target, time, easeFunction);
    }
    public virtual ActiveTweenNode Animate(Vector3 target, float duration, Func<float, float> easeFunction = null)
    {
        if (easeFunction == null)
            easeFunction = GetRunnabler().GetDefaultEaseFunction();

        return GetRunnabler().Queue(ShouldForce(), GetParentName(), GetName(), new _Vector3Tween(ShouldForce(), target, duration, GetCurrentValueFunction(), GetUpdateValueAction(), easeFunction));
    }

    public virtual ActiveTweenNode AnimateRotation(Vector3 target, float duration, Func<float, float> easeFunction = null)
    {
        if (easeFunction == null)
            easeFunction = GetRunnabler().GetDefaultEaseFunction();

        return GetRunnabler().Queue(ShouldForce(), GetParentName(), GetName(), new _RotationTween(ShouldForce(), target, duration, GetCurrentValueFunction(), GetUpdateValueAction(), easeFunction));
    }

    public virtual ActiveTweenNode AnimateWithSpeed(Vector3 target, float speed, Func<float, float> easeFunction = null)
    {
        var time = (target - (Vector3)GetCurrentValueFunction()()).magnitude / speed;

        return Animate(target, time, easeFunction);
    }

    public virtual ActiveTweenNode AnimateRotationWithSpeed(Vector3 target, float speed, Func<float, float> easeFunction = null)
    {
        var time = (target - (Vector3)GetCurrentValueFunction()()).magnitude / speed;

        return AnimateRotation(target, time, easeFunction);
    }

    public virtual ActiveTweenNode Animate(Transform transform, float duration, Func<float, float> easeFunction = null)
    {
        return Animate(Runnabler.To(transform), duration, easeFunction);
    }
    public virtual ActiveTweenNode Animate(Tuple<Vector3, Vector3, Vector3> target, float duration, Func<float, float> easeFunction = null)
    {
        if (easeFunction == null)
            easeFunction = GetRunnabler().GetDefaultEaseFunction();

        return GetRunnabler().Queue(ShouldForce(), GetParentName(), GetName(), new _TransformTween(ShouldForce(), target, duration, GetCurrentValueFunction(), GetUpdateValueAction(), easeFunction));
    }

    public virtual ActiveTweenNode Animate(string keyFrame, float duration, Func<float, float> easeFunction = null)
    {
        return Animate(GetRunnabler().GetKeyFrame(keyFrame), duration, easeFunction);
    }
}
