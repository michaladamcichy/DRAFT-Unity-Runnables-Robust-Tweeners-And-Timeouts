using System;

public class TweenNode : RunnableChainNode
{
    Func<object> currentValue_;
    Action<object> updateValue_;

    public TweenNode(bool force, string parentName, string name, RunnableMonoBehaviour runnableMonoBehaviour, Func<object> currentValue, Action<object> updateValue) 
        : base(force, parentName, name, runnableMonoBehaviour)
    {
        currentValue_ = currentValue;
        updateValue_ = updateValue;
    }
    public Func<object> GetCurrentValueFunction()
    {
        return currentValue_;
    }

    public Action<object> GetUpdateValueAction()
    {
        return updateValue_;
    }
}
