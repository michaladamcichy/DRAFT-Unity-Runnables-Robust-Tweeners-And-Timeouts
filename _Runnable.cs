using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal abstract class _Runnable
{
    public enum State
    {
        pending,
        running,
        finished
    }

    State state_ = State.pending;
    float speed_ = 1f;

    public abstract IEnumerator Run();

    public void Stop()
    {
        state_ = State.finished;
    }

    public State GetState()
    {
        return state_;
    }

    public bool HasStarted()
    {
        return state_ >= State.running;
    }

    public bool IsStopped()
    {
        return state_ == State.finished;
    }

    public bool IsRunnning()
    {
        return state_ == State.running;
    }

    protected void OnRun()
    {
        if (state_ > State.running)
            return;

        state_ = State.running;
    }

    public void SetSpeed(float speed)
    {
        this.speed_ = speed;
    }

    public float GetSpeed()
    {
        return speed_;
    }
}

//Tween
//Timeout
//Interval

