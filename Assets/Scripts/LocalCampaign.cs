// ILSpyBased#2
using System;
using System.Collections.Generic;
using UnityEngine;

public class LocalCampaign : MonoBehaviour
{
    private List<Scene> scenes = new List<Scene>();

    private List<Scene> ScenesToDelete = new List<Scene>();

    private List<Scene> ScenesToAdd = new List<Scene>();

    private List<GameObject> objectsToDestroy = new List<GameObject>();

    private Scene currentScene;

    public List<Scene> Scenes
    {
        get
        {
            return this.scenes;
        }
    }

    public List<GameObject> ObjectsToDestroy
    {
        get
        {
            return this.objectsToDestroy;
        }
    }

    public void AddScene(Scene scene)
    {
        this.ScenesToAdd.Add(scene);
    }

    public void RemoveScene(Scene scene)
    {
        this.ScenesToDelete.Add(scene);
    }

    public Scene NextScene()
    {
        if (this.scenes.Count <= 0)
        {
            this.currentScene = null;
            return null;
        }
        return this.scenes[0];
    }

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        List<Scene>.Enumerator enumerator = this.scenes.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Scene current = enumerator.Current;
                current.Update();
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        List<Scene>.Enumerator enumerator2 = this.ScenesToDelete.GetEnumerator();
        try
        {
            while (enumerator2.MoveNext())
            {
                Scene current2 = enumerator2.Current;
                this.scenes.Remove(current2);
            }
        }
        finally
        {
            ((IDisposable)enumerator2).Dispose();
        }
        this.ScenesToDelete.Clear();
        List<Scene>.Enumerator enumerator3 = this.ScenesToAdd.GetEnumerator();
        try
        {
            while (enumerator3.MoveNext())
            {
                Scene current3 = enumerator3.Current;
                this.scenes.Add(current3);
            }
        }
        finally
        {
            ((IDisposable)enumerator3).Dispose();
        }
        this.ScenesToAdd.Clear();
    }
}


