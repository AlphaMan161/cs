// ILSpyBased#2
using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneCompleteTasks : Scene
{
    private Dictionary<string, bool> tasks;

    public Dictionary<string, bool> Tasks
    {
        get
        {
            return this.tasks;
        }
    }

    public SceneCompleteTasks(SceneCompleteListener sceneCompleteListener, GameObject task, string name)
        : base(sceneCompleteListener, task, name)
    {
    }

    protected override void Init()
    {
        this.tasks = (Dictionary<string, bool>)base.Objects["tasks"];
    }

    protected override bool Trigger()
    {
        bool result = true;
        Dictionary<string, bool>.ValueCollection.Enumerator enumerator = this.tasks.Values.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                if (!enumerator.Current)
                {
                    result = false;
                }
            }
            return result;
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }
}


