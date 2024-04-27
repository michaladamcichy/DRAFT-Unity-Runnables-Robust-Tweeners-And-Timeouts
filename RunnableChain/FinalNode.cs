using System;

public class FinalNode : RunnableChainNode
{
    public FinalNode(bool force, string parentName, string name, Runnabler runnableMonoBehaviour) : base(force, parentName, name, runnableMonoBehaviour)
    {
    }
}