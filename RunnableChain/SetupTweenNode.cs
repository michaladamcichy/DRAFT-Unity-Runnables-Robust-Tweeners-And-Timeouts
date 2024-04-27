using System;

public class SetupTweenNode : AnimateTweenNode
{
    public SetupTweenNode(bool force, string parentName, string name, Runnabler runnableMonoBehaviour, Func<object> currentValue, Action<object> updateValue) 
        : base(force, parentName, name, runnableMonoBehaviour, currentValue, updateValue)
    {
    }

    public ActiveTweenNode Force() //alert skopiowany kod
    {
        GetRunnabler().StopIfExists(GetParentName());

        return new ActiveTweenNode(true, GetParentName(), GetName(), GetRunnabler(), GetCurrentValueFunction(), GetUpdateValueAction());
    }

    public ActiveTweenNode Parallel()
    {
        return GetRunnabler().ForkTween(GetName(), GetCurrentValueFunction(), GetUpdateValueAction());
    }
    public void Speed(float speed) //alert duplication
    {
        GetRunnabler().SetSpeed(GetParentName(), speed);
    }
}
