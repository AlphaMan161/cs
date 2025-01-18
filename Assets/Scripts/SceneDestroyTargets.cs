// ILSpyBased#2
using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneDestroyTargets : Scene
{
    private int prevAmmo;

    private Transform glider;

    private CombatWeapon weapon;

    private List<GameObject> targets;

    public SceneDestroyTargets(SceneCompleteListener sceneCompleteListener, GameObject task, string name)
        : base(sceneCompleteListener, task, name)
    {
    }

    protected override void Init()
    {
        this.targets = (List<GameObject>)base.Objects["targets"];
    }

    protected override bool Trigger()
    {
        bool result = true;
        List<GameObject>.Enumerator enumerator = this.targets.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                GameObject current = enumerator.Current;
                if ((UnityEngine.Object)current != (UnityEngine.Object)null)
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


