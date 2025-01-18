// ILSpyBased#2
using System;
using System.Collections;
using UnityEngine;

public class Scene : IDisposable
{
    public delegate void SceneCompleteListener(Scene scene);

    protected SceneCompleteListener sceneCompleteListener;

    protected GameObject task;

    protected bool inited;

    private string name;

    private Hashtable objects;

    public string Name
    {
        get
        {
            return this.name;
        }
    }

    public GameObject Task
    {
        get
        {
            return this.task;
        }
    }

    public Hashtable Objects
    {
        get
        {
            return this.objects;
        }
    }

    public Scene(SceneCompleteListener sceneCompleteListener, GameObject task, string name)
    {
        this.objects = new Hashtable();
        this.name = name;
        this.inited = false;
        this.task = task;
        this.sceneCompleteListener = sceneCompleteListener;
    }

    protected virtual void Init()
    {
    }

    protected virtual bool Trigger()
    {
        return false;
    }

    public bool Update()
    {
        TransformTween component = this.task.GetComponent<TransformTween>();
        if (!component.Finished)
        {
            return false;
        }
        if (!this.inited)
        {
            this.Init();
            this.inited = true;
        }
        if (this.sceneCompleteListener == null)
        {
            return false;
        }
        if (this.Trigger())
        {
            this.sceneCompleteListener(this);
            return true;
        }
        return false;
    }

    public void Dispose()
    {
        this.Objects.Clear();
    }
}


