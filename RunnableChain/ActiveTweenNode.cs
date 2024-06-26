﻿using System;

public class ActiveTweenNode : AnimateTweenNode, IActiveNode
{
    ActiveNode activeNode_;
    public ActiveTweenNode(bool force, string parentName, string name, Runnabler runnableMonoBehaviour, Func<object> currentValue, Action<object> updateValue)
        : base(force, parentName, name, runnableMonoBehaviour, currentValue, updateValue)
    {
        activeNode_ = new ActiveNode(force, parentName, name, runnableMonoBehaviour);
    }

    public ActiveNode Timeout(float duration, Action callback)
    {
        return activeNode_.Timeout(duration, callback);
    }

    public FinalNode Interval(float interval, Action callback)
    {
        return activeNode_.Interval(interval, callback);
    }
    public FinalNode Interval(Action callback)
    {
        return activeNode_.Interval(callback);
    }

    public SetupTweenNode Tween(Func<object> currentValue, Action<object> updateValue)
    {
        return activeNode_.Tween(currentValue, updateValue);
    }

    public ActiveTweenNode Parallel()
    {
        return GetRunnabler().ForkTween(GetName(), GetCurrentValueFunction(), GetUpdateValueAction());
    }

    public ActiveNode Then(Action callback)
    {
        return activeNode_.Then(callback);
    }

    public ActiveNode Then()
    {
        return activeNode_.Then(() => { }); //alert 
    }
}
