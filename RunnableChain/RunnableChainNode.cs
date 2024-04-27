using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class RunnableChainNode
{
    string parentName_;
    string name_;
    Runnabler runnabler_;
    bool force_ = false;
    public RunnableChainNode(bool force, string parentName, string name, Runnabler runnableMonoBehaviour)
    {
        parentName_ = parentName;
        name_ = name;
        runnabler_ = runnableMonoBehaviour;
        force_ = force;
    }

    public string GetParentName()
    {
        return parentName_;
    }
    public string GetName()
    {
        return name_;
    }

    public Runnabler GetRunnabler()
    {
        return runnabler_;
    }

    public void Stop()
    {
        runnabler_.Stop(parentName_);
    }

    public void StopIfExists()
    {
        runnabler_.StopIfExists(parentName_);
    }

    protected bool ShouldForce()
    {
        return force_;
    }

    public bool Exists()
    {
        return runnabler_.RunnableExists(parentName_);
    }
}
