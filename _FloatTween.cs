using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class _FloatTween : _Tween
{
    public _FloatTween(bool force, float target, float duration, Func<object> getCurrentValue, Action<object> updateAction, Func<float, float> easeFunction)
        : base(force, target, duration, getCurrentValue, updateAction, easeFunction)
    {
    }

    protected override object ApplyTweenDelta(float tweenDelta)
    {
        var delta = (float)GetTarget() - (float)GetInitialValue();

        return (float) GetCurrentValueFunction()() + delta * tweenDelta;
    }

    protected override object CorrrectTarget(object initial, object target)
    {
        return target; //alert
    }
}
