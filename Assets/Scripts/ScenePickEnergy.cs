// ILSpyBased#2
using UnityEngine;

public class ScenePickEnergy : Scene
{
    private CombatPlayer localPlayer;

    private int prevEnergy;

    public ScenePickEnergy(SceneCompleteListener sceneCompleteListener, GameObject task, string name)
        : base(sceneCompleteListener, task, name)
    {
    }

    protected override void Init()
    {
        this.localPlayer = LocalPlayerManager.Instance.LocalPlayer;
        this.prevEnergy = this.localPlayer.Energy;
    }

    protected override bool Trigger()
    {
        bool result = false;
        if (this.prevEnergy < this.localPlayer.Energy)
        {
            result = true;
        }
        this.prevEnergy = this.localPlayer.Energy;
        return result;
    }
}


