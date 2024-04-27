using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class _RotationFloatTween : _Tween
{
    public _RotationFloatTween(bool force, float target, float duration, Func<object> getCurrentValue, Action<object> updateAction, Func<float, float> easeFunction)
        : base(force, target, duration, getCurrentValue, updateAction, easeFunction)
    {
        
    }

    protected override object ApplyTweenDelta(float tweenDelta)
    {
        var delta = (float) GetTarget() - (float) GetInitialValue();

        return (float) GetCurrentValueFunction()() + delta * tweenDelta;
    }

    protected override object CorrrectTarget(object initial, object target)
    {
        var targetValue = (float) target;
        var initialValue = (float) initial;

        while (Mathf.Abs(targetValue - initialValue) > 180.0f) //alert dummy :)
        {
            targetValue -= 360f * (Mathf.Sign(targetValue - initialValue));
        }

        return targetValue;
    }

    //protected override object NonAdditive(float tweenValue) //alert
    //{
    //    var delta = (Vector3)GetTarget() - (Vector3)GetInitialValue();

    //    return (Vector3)GetInitialValue() + delta * tweenValue;
    //}
}
