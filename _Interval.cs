using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class _Interval : _Runnable
{
    float? interval_;
    Action callback_;
    public _Interval(float? interval, Action callback)
    {
        interval_ = interval;
        callback_ = callback;
    }

    public override IEnumerator Run()
    {
        if (GetState() > State.pending)
            yield break; //alert czy to działa?

        OnRun();

        while(GetState() == State.running)
        {
            callback_();


            if(!interval_.HasValue)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }

            float timePassed = 0f;
            while (timePassed < interval_.Value)
            {
                timePassed += GetSpeed() * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        Stop();
    }
}