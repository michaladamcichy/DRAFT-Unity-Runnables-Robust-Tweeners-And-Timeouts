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
    RunnableMonoBehaviour runnableMonoBehaviour_;
    bool force_ = false;
    public RunnableChainNode(bool force, string parentName, string name, RunnableMonoBehaviour runnableMonoBehaviour)
    {
        parentName_ = parentName;
        name_ = name;
        runnableMonoBehaviour_ = runnableMonoBehaviour;
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

    public RunnableMonoBehaviour GetRunnableMonoBehaviour()
    {
        return runnableMonoBehaviour_;
    }

    public void Stop()
    {
        runnableMonoBehaviour_.Stop(parentName_);
    }

    protected bool ShouldForce()
    {
        return force_;
    }
}
