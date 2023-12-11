using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal abstract class _Tween : _Runnable
{
    object target_;
    float duration_;
    Func<object> getCurrentValueFunction_;
    Action<object> updateAction_;
    Func<float, float> easeFunction_;
    
    object initialValue_;
    float position_ = 0f;

    bool force_ = false;

    public _Tween(bool force, object target, float duration, Func<object> getCurrentValue, Action<object> updateAction, Func<float, float> easeFunction)
    {
        force_ = force;
        target_ = target;
        duration_ = duration;
        easeFunction_ = easeFunction;
        getCurrentValueFunction_ = getCurrentValue;
        updateAction_ = updateAction;
    }

    override public IEnumerator Run()
    {
        if (GetState() > State.pending)
            yield break;

        OnRun();

        initialValue_ = getCurrentValueFunction_();

        while (position_ < 1f)
        {
            if (GetState() == State.finished)
                yield break;

            var lastPosition = position_;

            position_ += Time.deltaTime / duration_;

            var tweenValue = easeFunction_(position_);
            var tweenDelta = tweenValue - easeFunction_(lastPosition); //alert efficiency!

            var objectValue = ApplyTweenDelta(tweenDelta);

            updateAction_(objectValue);

            yield return new WaitForEndOfFrame();
        }

        if(force_)
            updateAction_(target_);

        Stop();
    }

    public float GetPosition()
    {
        return position_;
    }
    public object GetTarget()
    {
        return target_;
    }
    public void SetInitialValue(object initialValue)
    {
        initialValue_ = initialValue;
    }
    public object GetInitialValue()
    {
        return initialValue_;
    }

    public Func<object> GetCurrentValueFunction()
    {
        return getCurrentValueFunction_;
    }
    public Action<object> GetUpdateAction()
    {
        return updateAction_;
    }

    protected abstract object ApplyTweenDelta(float tweenDelta); //alert zła nazwa
}
