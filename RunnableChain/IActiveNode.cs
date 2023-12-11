using System;

public interface IActiveNode
{
    public SetupTweenNode Tween(Func<object> currentValue, Action<object> updateValue);

    public ActiveNode Timeout(float duration, Action callback);

    public ActiveNode Then(Action callback);

    public FinalNode Interval(float duration, Action callback);
    public FinalNode Interval(Action callback);
}

