using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class _TransformTween : _Tween
{
    public _TransformTween(bool force, Tuple<Vector3, Vector3, Vector3> target, float duration, Func<object> getCurrentValue, Action<object> updateAction, Func<float, float> easeFunction)
        : base(force, target, duration, getCurrentValue, updateAction, easeFunction)
    {
    }

    protected override object ApplyTweenDelta(float tweenDelta)
    {
        var initial = (Tuple<Vector3, Vector3, Vector3>) GetInitialValue();
        var target = (Tuple<Vector3, Vector3, Vector3>) GetTarget();
        var current = (Tuple<Vector3, Vector3, Vector3>) GetCurrentValueFunction()();

        var deltaPosition = target.Item1 - initial.Item1;
        var deltaRotation = target.Item2 - initial.Item2;
        var deltaScale = target.Item3 - initial.Item3;

        var position = current.Item1 + deltaPosition * tweenDelta;
        var rotation = current.Item2 + deltaRotation * tweenDelta;
        var scale = current.Item3 + deltaScale * tweenDelta;

        return new Tuple<Vector3, Vector3, Vector3>(position, rotation, scale);
    }

    protected override object CorrrectTarget(object initial, object target)
    {
        var targetValue = ((Tuple<Vector3, Vector3, Vector3>) target).Item2;
        var initialValue = ((Tuple<Vector3, Vector3, Vector3>) initial).Item2;
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

        var output = ((Tuple<Vector3, Vector3, Vector3>)target);

        return new Tuple<Vector3, Vector3, Vector3>(output.Item1, targetValue, output.Item3);
    }

    //protected override object NonAdditive(float tweenValue) //alert
    //{
    //    var delta = (Vector3)GetTarget() - (Vector3)GetInitialValue();

    //    return (Vector3)GetInitialValue() + delta * tweenValue;
    //}
}
