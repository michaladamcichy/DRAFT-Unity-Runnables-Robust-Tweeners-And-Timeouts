using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class _Vector3Tween : _Tween
{
    public _Vector3Tween(bool force, Vector3 target, float duration, Func<object> getCurrentValue, Action<object> updateAction, Func<float, float> easeFunction)
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
        return target; //alert
    }

    //protected override object NonAdditive(float tweenValue) //alert
    //{
    //    var delta = (Vector3)GetTarget() - (Vector3)GetInitialValue();

    //    return (Vector3)GetInitialValue() + delta * tweenValue;
    //}
}
