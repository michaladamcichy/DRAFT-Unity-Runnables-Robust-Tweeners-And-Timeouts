using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class _RotationTween : _Tween
{
    public _RotationTween(bool force, Vector3 target, float duration, Func<object> getCurrentValue, Action<object> updateAction, Func<float, float> easeFunction)
        : base(force, target, duration, getCurrentValue, updateAction, easeFunction)
    {
        
    }

    protected override object ApplyTweenDelta(float tweenDelta)
    {
        var delta = (Vector3) GetTarget() - (Vector3) GetInitialValue();

        return (Vector3) GetCurrentValueFunction()() + delta * tweenDelta;
    }

    protected override object CorrrectTarget(object initial, object target)
    {
        var targetValue = (Vector3) target;
        var initialValue = (Vector3) initial;

        while (Mathf.Abs(targetValue.x - initialValue.x) > 180.0f) //alert dummy :)
        {
            targetValue.x -= 360f * (Mathf.Sign(targetValue.x - initialValue.x));
        }

        while (Mathf.Abs(targetValue.y - initialValue.y) > 180.0f)
        {
            targetValue.y -= 360f * (Mathf.Sign(targetValue.y - initialValue.y));
        }

        while (Mathf.Abs(targetValue.z - initialValue.z) > 180.0f)
        {
            targetValue.z -= 360f * (Mathf.Sign(targetValue.z - initialValue.z));
        }

        return targetValue;
    }

    //protected override object NonAdditive(float tweenValue) //alert
    //{
    //    var delta = (Vector3)GetTarget() - (Vector3)GetInitialValue();

    //    return (Vector3)GetInitialValue() + delta * tweenValue;
    //}
}
