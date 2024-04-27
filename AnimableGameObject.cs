using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimableGameObject : MonoBehaviour, IRunnablerContainer
{
    Runnabler runnabler_;
    // Start is called before the first frame update
    void Start()
    {
        runnabler_ = new Runnabler(this);
    }

    // Update is called once per frame
    void Update()
    {
        runnabler_.Update();
    }

    public Runnabler GetRunnabler()
    {
        return runnabler_;
    }

    public void OnStop()
    {
    }
}
