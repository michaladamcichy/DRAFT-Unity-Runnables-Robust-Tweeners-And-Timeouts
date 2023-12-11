using System;

public class SetupTweenNode : AnimateTweenNode
{
    public SetupTweenNode(bool force, string parentName, string name, RunnableMonoBehaviour runnableMonoBehaviour, Func<object> currentValue, Action<object> updateValue) 
        : base(force, parentName, name, runnableMonoBehaviour, currentValue, updateValue)
    {
    }

    public ActiveTweenNode Force() //alert skopiowany kod
    {
        GetRunnableMonoBehaviour().Stop(GetParentName());

        return new ActiveTweenNode(true, GetParentName(), GetName(), GetRunnableMonoBehaviour(), GetCurrentValueFunction(), GetUpdateValueAction());
    }

    public ActiveTweenNode Parallel()
    {
        return GetRunnableMonoBehaviour().ForkTween(GetName(), GetCurrentValueFunction(), GetUpdateValueAction());
    }
}
