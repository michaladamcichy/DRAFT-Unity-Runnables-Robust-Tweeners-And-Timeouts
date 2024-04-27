using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class _Timeout : _Runnable
{
    float duration_;
    Action callback_;
    public _Timeout(float duration, Action callback)
    {
        duration_ = duration;
        callback_ = callback;
    }
    public override IEnumerator Run()
    {
        if (GetState() > State.pending)
            yield break; //alert czy to działa?

        OnRun();

        float timePassed = 0f;
        while(timePassed < duration_)
        {
            timePassed += GetSpeed() * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        callback_();

        Stop();
    }
}