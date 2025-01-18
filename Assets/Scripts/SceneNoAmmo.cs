// ILSpyBased#2
using UnityEngine;

public class SceneNoAmmo : Scene
{
    private int prevAmmo;

    private Transform glider;

    private CombatWeapon weapon;

    public SceneNoAmmo(SceneCompleteListener sceneCompleteListener, GameObject task, string name)
        : base(sceneCompleteListener, task, name)
    {
    }

    protected override void Init()
    {
        WeaponType weaponType = (WeaponType)(byte)base.Objects["weaponType"];
        this.glider = LocalPlayerManager.Instance.LocalPlayer.transform;
        this.weapon = LocalShotController.Instance.GetWeaponByType((int)weaponType);
        this.prevAmmo = this.weapon.LoadedAmmo;
    }

    protected override bool Trigger()
    {
        bool result = false;
        if (this.prevAmmo > this.weapon.LoadedAmmo && this.weapon.LoadedAmmo == 0 && this.weapon.AmmoReserve == 0)
        {
            result = true;
        }
        this.prevAmmo = this.weapon.LoadedAmmo;
        return result;
    }
}


