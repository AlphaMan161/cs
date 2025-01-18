// ILSpyBased#2
using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public Material frozenMaterial;

    public GameObject laserGunPrefab;

    public GameObject laserSparkPrefab;

    public GameObject laserShaftSparkPrefab;

    public GameObject grenadeLauncherShaftSparkPrefab;

    public GameObject plasmaSparkPrefab;

    public GameObject plasmaShaftSparkPrefab;

    public GameObject rocketLauncherShaftSparkPrefab;

    public GameObject machineGunShaftSparkPrefab;

    public GameObject machineGunSparkPrefab;

    public GameObject machineGunPrefab;

    public GameObject railPrefab;

    public GameObject blizzRailPrefab;

    public GameObject blizzRailArcPrefab;

    public GameObject blizzRailSparkPrefab;

    public GameObject plasmaPrefab;

    public GameObject exploPrefab;

    public GameObject exploDeactivationPrefab;

    public GameObject minePrefab;

    public GameObject mineTimePrefab;

    public GameObject whizbangPrefab;

    public GameObject mortarWhizbangPrefab;

    public GameObject mineRemotePrefab;

    public GameObject turretTeslaMontagePrefab;

    public GameObject turretTeslaPrefab;

    public GameObject turretTeslaRedPrefab;

    public GameObject turretTeslaBluePrefab;

    public GameObject energyShieldPrefab;

    public GameObject healthHitPrefab;

    public GameObject ultraPrefab;

    public GameObject ultraSparkPrefab;

    public GameObject ultraShaftSparkPrefab;

    public GameObject tripleRocketPrefab;

    public GameObject spiralRailPrefab;

    public GameObject powerLaserPrefab;

    public GameObject powerLaserSparkPrefab;

    public GameObject powerLaserShaftSparkPrefab;

    public GameObject blizzChainArcPrefab;

    public GameObject blizzChainSparkPrefab;

    public GameObject ultraSonicPrefab;

    public GameObject mineElectricPrefab;

    public GameObject electricExploPrefab;

    public GameObject energyBallPrefab;

    public GameObject energyBallExploPrefab;

    public GameObject turretMachineGunMontagePrefab;

    public GameObject turretMachineGunPrefab;

    public GameObject turretMachineGunRedPrefab;

    public GameObject turretMachineGunBluePrefab;

    private static EffectManager instance;

    private System.Random rnd = new System.Random();

    public GameObject LaserGunPrefab
    {
        get
        {
            return this.laserGunPrefab;
        }
        set
        {
            this.laserGunPrefab = value;
        }
    }

    public GameObject LaserSparkPrefab
    {
        get
        {
            return this.laserSparkPrefab;
        }
        set
        {
            this.laserSparkPrefab = value;
        }
    }

    public GameObject LaserShaftSparkPrefab
    {
        get
        {
            return this.laserShaftSparkPrefab;
        }
        set
        {
            this.laserShaftSparkPrefab = value;
        }
    }

    public GameObject GrenadeLauncherShaftSparkPrefab
    {
        get
        {
            return this.grenadeLauncherShaftSparkPrefab;
        }
        set
        {
            this.grenadeLauncherShaftSparkPrefab = value;
        }
    }

    public GameObject PlasmaSparkPrefab
    {
        get
        {
            return this.plasmaSparkPrefab;
        }
        set
        {
            this.plasmaSparkPrefab = value;
        }
    }

    public GameObject PlasmaShaftSparkPrefab
    {
        get
        {
            return this.plasmaShaftSparkPrefab;
        }
        set
        {
            this.plasmaShaftSparkPrefab = value;
        }
    }

    public GameObject RocketLauncherShaftSparkPrefab
    {
        get
        {
            return this.rocketLauncherShaftSparkPrefab;
        }
        set
        {
            this.rocketLauncherShaftSparkPrefab = value;
        }
    }

    public GameObject MachineGunShaftSparkPrefab
    {
        get
        {
            return this.machineGunShaftSparkPrefab;
        }
        set
        {
            this.machineGunShaftSparkPrefab = value;
        }
    }

    public GameObject MachineGunSparkPrefab
    {
        get
        {
            return this.machineGunSparkPrefab;
        }
        set
        {
            this.machineGunSparkPrefab = value;
        }
    }

    public GameObject MachineGunPrefab
    {
        get
        {
            return this.machineGunPrefab;
        }
        set
        {
            this.machineGunPrefab = value;
        }
    }

    public GameObject RailPrefab
    {
        get
        {
            return this.railPrefab;
        }
        set
        {
            this.railPrefab = value;
        }
    }

    public GameObject BlizzRailPrefab
    {
        get
        {
            return this.blizzRailPrefab;
        }
        set
        {
            this.blizzRailPrefab = value;
        }
    }

    public GameObject BlizzRailArcPrefab
    {
        get
        {
            return this.blizzRailArcPrefab;
        }
        set
        {
            this.blizzRailArcPrefab = value;
        }
    }

    public GameObject BlizzRailSparkPrefab
    {
        get
        {
            return this.blizzRailSparkPrefab;
        }
        set
        {
            this.blizzRailSparkPrefab = value;
        }
    }

    public GameObject PlasmaPrefab
    {
        get
        {
            return this.plasmaPrefab;
        }
        set
        {
            this.plasmaPrefab = value;
        }
    }

    public GameObject ExploPrefab
    {
        get
        {
            return this.exploPrefab;
        }
        set
        {
            this.exploPrefab = value;
        }
    }

    public GameObject ExploDeactivationPrefab
    {
        get
        {
            return this.exploDeactivationPrefab;
        }
        set
        {
            this.exploDeactivationPrefab = value;
        }
    }

    public GameObject MinePrefab
    {
        get
        {
            return this.minePrefab;
        }
        set
        {
            this.minePrefab = value;
        }
    }

    public GameObject MineTimePrefab
    {
        get
        {
            return this.mineTimePrefab;
        }
        set
        {
            this.mineTimePrefab = value;
        }
    }

    public GameObject WhizbangPrefab
    {
        get
        {
            return this.whizbangPrefab;
        }
        set
        {
            this.whizbangPrefab = value;
        }
    }

    public GameObject MortarWhizbangPrefab
    {
        get
        {
            return this.mortarWhizbangPrefab;
        }
        set
        {
            this.mortarWhizbangPrefab = value;
        }
    }

    public GameObject MineRemotePrefab
    {
        get
        {
            return this.mineRemotePrefab;
        }
        set
        {
            this.mineRemotePrefab = value;
        }
    }

    public GameObject TurretTeslaMontagePrefab
    {
        get
        {
            return this.turretTeslaMontagePrefab;
        }
        set
        {
            this.turretTeslaMontagePrefab = value;
        }
    }

    public GameObject TurretTeslaPrefab
    {
        get
        {
            return this.turretTeslaPrefab;
        }
        set
        {
            this.turretTeslaPrefab = value;
        }
    }

    public GameObject TurretTeslaRedPrefab
    {
        get
        {
            return this.turretTeslaRedPrefab;
        }
        set
        {
            this.turretTeslaRedPrefab = value;
        }
    }

    public GameObject TurretTeslaBluePrefab
    {
        get
        {
            return this.turretTeslaBluePrefab;
        }
        set
        {
            this.turretTeslaBluePrefab = value;
        }
    }

    public GameObject EnergyShieldPrefab
    {
        get
        {
            return this.energyShieldPrefab;
        }
        set
        {
            this.energyShieldPrefab = value;
        }
    }

    public GameObject HealthHitPrefab
    {
        get
        {
            return this.healthHitPrefab;
        }
        set
        {
            this.healthHitPrefab = value;
        }
    }

    public GameObject UltraPrefab
    {
        get
        {
            return this.ultraPrefab;
        }
        set
        {
            this.ultraPrefab = value;
        }
    }

    public GameObject UltraSparkPrefab
    {
        get
        {
            return this.ultraSparkPrefab;
        }
        set
        {
            this.ultraSparkPrefab = value;
        }
    }

    public GameObject UltraShaftSparkPrefab
    {
        get
        {
            return this.ultraShaftSparkPrefab;
        }
        set
        {
            this.ultraShaftSparkPrefab = value;
        }
    }

    public GameObject TripleRocketPrefab
    {
        get
        {
            return this.tripleRocketPrefab;
        }
        set
        {
            this.tripleRocketPrefab = value;
        }
    }

    public GameObject SpiralRailPrefab
    {
        get
        {
            return this.spiralRailPrefab;
        }
        set
        {
            this.spiralRailPrefab = value;
        }
    }

    public GameObject PowerLaserPrefab
    {
        get
        {
            return this.powerLaserPrefab;
        }
        set
        {
            this.powerLaserPrefab = value;
        }
    }

    public GameObject PowerLaserSparkPrefab
    {
        get
        {
            return this.powerLaserSparkPrefab;
        }
        set
        {
            this.powerLaserSparkPrefab = value;
        }
    }

    public GameObject PowerLaserShaftSparkPrefab
    {
        get
        {
            return this.powerLaserShaftSparkPrefab;
        }
        set
        {
            this.powerLaserShaftSparkPrefab = value;
        }
    }

    public GameObject BlizzChainArcPrefab
    {
        get
        {
            return this.blizzChainArcPrefab;
        }
        set
        {
            this.blizzChainArcPrefab = value;
        }
    }

    public GameObject BlizzChainSparkPrefab
    {
        get
        {
            return this.blizzChainSparkPrefab;
        }
        set
        {
            this.blizzChainSparkPrefab = value;
        }
    }

    public GameObject UltraSonicPrefab
    {
        get
        {
            return this.ultraSonicPrefab;
        }
        set
        {
            this.ultraSonicPrefab = value;
        }
    }

    public GameObject MineElectricPrefab
    {
        get
        {
            return this.mineElectricPrefab;
        }
        set
        {
            this.mineElectricPrefab = value;
        }
    }

    public GameObject ElectricExploPrefab
    {
        get
        {
            return this.electricExploPrefab;
        }
        set
        {
            this.electricExploPrefab = value;
        }
    }

    public GameObject EnergyBallPrefab
    {
        get
        {
            return this.energyBallPrefab;
        }
        set
        {
            this.energyBallPrefab = value;
        }
    }

    public GameObject EnergyBallExploPrefab
    {
        get
        {
            return this.energyBallExploPrefab;
        }
        set
        {
            this.energyBallExploPrefab = value;
        }
    }

    public GameObject TurretMachineGunMontagePrefab
    {
        get
        {
            return this.turretMachineGunMontagePrefab;
        }
        set
        {
            this.turretMachineGunMontagePrefab = value;
        }
    }

    public GameObject TurretMachineGunPrefab
    {
        get
        {
            return this.turretMachineGunPrefab;
        }
        set
        {
            this.turretMachineGunPrefab = value;
        }
    }

    public GameObject TurretMachineGunRedPrefab
    {
        get
        {
            return this.turretMachineGunRedPrefab;
        }
        set
        {
            this.turretMachineGunRedPrefab = value;
        }
    }

    public GameObject TurretMachineGunBluePrefab
    {
        get
        {
            return this.turretMachineGunBluePrefab;
        }
        set
        {
            this.turretMachineGunBluePrefab = value;
        }
    }

    public static EffectManager Instance
    {
        get
        {
            return EffectManager.instance;
        }
    }

    public System.Random RandomGenerator
    {
        get
        {
            return this.rnd;
        }
    }

    private void Awake()
    {
        EffectManager.instance = this;
    }

    public void weaponBlockEffect(CombatPlayer player, WeaponType weaponType)
    {
    }

    public void mineRemoteEffect(Shot shot, CombatPlayer player, bool isMe, bool success)
    {
        List<MineRemoteTracer> list = new List<MineRemoteTracer>();
        Dictionary<long, ItemTracer>.ValueCollection.Enumerator enumerator = player.RegisteredItems.Values.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                ItemTracer current = enumerator.Current;
                if (current.GetType() == typeof(MineRemoteTracer))
                {
                    list.Add((MineRemoteTracer)current);
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        List<MineRemoteTracer>.Enumerator enumerator2 = list.GetEnumerator();
        try
        {
            while (enumerator2.MoveNext())
            {
                MineRemoteTracer current2 = enumerator2.Current;
                current2.BlowRemoteMine();
            }
        }
        finally
        {
            ((IDisposable)enumerator2).Dispose();
        }
    }

    public void mineEffect(Shot shot, CombatPlayer player, bool isMe, bool success)
    {
        if (!success)
        {
            SoundManager.Instance.Play(player.Audio, "energy container birth", AudioPlayMode.Play);
        }
        else
        {
            GameObject original = null;
            switch (shot.WeaponType)
            {
                case WeaponType.MINE_ELECTRIC:
                    SoundManager.Instance.Play(player.Audio, "set mine-bomb", AudioPlayMode.Play);
                    original = this.mineElectricPrefab;
                    break;
                case WeaponType.MINE_TOUCH:
                    SoundManager.Instance.Play(player.Audio, "set mine-bomb", AudioPlayMode.Play);
                    original = this.minePrefab;
                    break;
                case WeaponType.MINE_TIME:
                    SoundManager.Instance.Play(player.Audio, "set mine-bomb", AudioPlayMode.Play);
                    original = this.mineTimePrefab;
                    break;
                case WeaponType.MINE_ELECTRIC_REMOTE:
                case WeaponType.MINE_REMOTE:
                    SoundManager.Instance.Play(player.Audio, "mechnic_mine_set_2", AudioPlayMode.Play);
                    original = this.mineRemotePrefab;
                    break;
            }
            GameObject gameObject = UnityEngine.Object.Instantiate(original);
            MineTracer mineTracer = (MineTracer)gameObject.GetComponent("MineTracer");
            mineTracer.WeaponType = shot.WeaponType;
            mineTracer.Launch(shot, player, isMe);
            player.RegisterItem(mineTracer);
        }
    }

    public void turretEffect(Shot shot, CombatPlayer player, bool isMe, bool success)
    {
        if (!success)
        {
            SoundManager.Instance.Play(player.Audio, "energy container birth", AudioPlayMode.Play);
        }
        else
        {
            SoundManager.Instance.Play(player.Audio, "mechnic_turret_set2", AudioPlayMode.Play);
            GameObject original = null;
            switch (shot.WeaponType)
            {
                case WeaponType.TURRET_TESLA:
                    original = this.turretTeslaPrefab;
                    if (player.Team == 1)
                    {
                        original = this.turretTeslaRedPrefab;
                    }
                    else if (player.Team == 2)
                    {
                        original = this.turretTeslaBluePrefab;
                    }
                    break;
                case WeaponType.TURRET_MACHINE_GUN:
                    original = this.turretMachineGunPrefab;
                    if (player.Team == 1)
                    {
                        original = this.turretMachineGunRedPrefab;
                    }
                    else if (player.Team == 2)
                    {
                        original = this.turretMachineGunBluePrefab;
                    }
                    break;
            }
            GameObject gameObject = UnityEngine.Object.Instantiate(original);
            TurretTracer turretTracer = (TurretTracer)gameObject.GetComponent("TurretTracer");
            turretTracer.WeaponType = shot.WeaponType;
            turretTracer.Launch(shot, player, isMe);
            if (shot.WeaponType == WeaponType.TURRET_MACHINE_GUN)
            {
                TurretMachineGunTracer turretMachineGunTracer = (TurretMachineGunTracer)turretTracer;
            }
            player.RegisterItem(turretTracer);
        }
    }

    public void turretTeslaShotEffect(Shot shot, CombatPlayer player, ShotEffectType shotEffectType)
    {
        if (!((UnityEngine.Object)player == (UnityEngine.Object)null))
        {
            if (shotEffectType == ShotEffectType.ME_BEFORE_SERVER || shotEffectType == ShotEffectType.ENEMY_AFTER_SERVER)
            {
                SoundManager.Instance.Play(player.Audio, "mechnic gun heal", AudioPlayMode.Play);
            }
            if (shotEffectType != ShotEffectType.ME_BEFORE_SERVER)
            {
                TurretTeslaTracer turretTeslaTracer = null;
                Dictionary<long, ItemTracer>.ValueCollection.Enumerator enumerator = player.RegisteredItems.Values.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        ItemTracer current = enumerator.Current;
                        if (current.GetType() == typeof(TurretTeslaTracer))
                        {
                            turretTeslaTracer = (TurretTeslaTracer)current;
                        }
                    }
                }
                finally
                {
                    ((IDisposable)enumerator).Dispose();
                }
                if (!((UnityEngine.Object)turretTeslaTracer == (UnityEngine.Object)null))
                {
                    GameObject gameObject = turretTeslaTracer.gameObject;
                    Charger componentInChildren = gameObject.GetComponentInChildren<Charger>();
                    if (shot.HasTargets)
                    {
                        Transform targetTransform = shot.Targets[0].TargetTransform;
                        if (!((UnityEngine.Object)targetTransform == (UnityEngine.Object)null) && (bool)componentInChildren)
                        {
                            componentInChildren.fire(targetTransform);
                        }
                    }
                }
            }
        }
    }

    public void turretMachineGunShotEffect(Shot shot, CombatPlayer player, ShotEffectType shotEffectType)
    {
        if (!((UnityEngine.Object)player == (UnityEngine.Object)null))
        {
            SoundManager.Instance.Play(player.Audio, "machinegun shot", AudioPlayMode.Play);
            GameObject gameObject = null;
            Transform transform = null;
            Transform transform2 = null;
            TurretMachineGunTracer turretMachineGunTracer = null;
            Dictionary<long, ItemTracer>.ValueCollection.Enumerator enumerator = player.RegisteredItems.Values.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    ItemTracer current = enumerator.Current;
                    if (current.GetType() == typeof(TurretMachineGunTracer))
                    {
                        turretMachineGunTracer = (TurretMachineGunTracer)current;
                    }
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
            if (!((UnityEngine.Object)turretMachineGunTracer == (UnityEngine.Object)null))
            {
                turretMachineGunTracer.SetLookTarget(shot.Origin, true);
                if (shot.LaunchMode != LaunchModes.TURRET_CONTROL)
                {
                    gameObject = turretMachineGunTracer.gameObject;
                    Transform transform3 = gameObject.transform.Find("Head");
                    Transform transform4 = transform3.transform.Find("HeadModel");
                    if ((UnityEngine.Object)transform4 != (UnityEngine.Object)null)
                    {
                        Transform transform5 = transform4.Find("BarrellLeft");
                        if ((UnityEngine.Object)transform5 != (UnityEngine.Object)null)
                        {
                            transform = transform5.Find("Target");
                        }
                        transform5 = transform4.Find("BarrellRight");
                        if ((UnityEngine.Object)transform5 != (UnityEngine.Object)null)
                        {
                            transform2 = transform5.Find("Target");
                        }
                    }
                    if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
                    {
                        GameObject gameObject2 = UnityEngine.Object.Instantiate((UnityEngine.Object)this.machineGunShaftSparkPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
                        gameObject2.transform.parent = transform;
                        gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
                    }
                    if ((UnityEngine.Object)transform2 != (UnityEngine.Object)null)
                    {
                        GameObject gameObject2 = UnityEngine.Object.Instantiate((UnityEngine.Object)this.machineGunShaftSparkPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
                        gameObject2.transform.parent = transform2;
                        gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
                    }
                    MachineGun[] componentsInChildren = gameObject.GetComponentsInChildren<MachineGun>();
                    MachineGun[] array = componentsInChildren;
                    foreach (MachineGun machineGun in array)
                    {
                        machineGun.fire();
                    }
                    GameObject gameObject3 = UnityEngine.Object.Instantiate(this.machineGunSparkPrefab);
					gameObject3.GetComponentInChildren<Renderer> ().material.shader = Shader.Find ("Particles/Additive (Soft)");
                    gameObject3.transform.position = shot.Origin;
                    if (shot.Direction != Vector3.zero)
                    {
                        gameObject3.transform.rotation = Quaternion.LookRotation(shot.Direction);
                    }
                    GameObject gameObject4 = UnityEngine.Object.Instantiate(this.machineGunPrefab);
                    Tracer tracer = (Tracer)gameObject4.GetComponent("Tracer");
                    tracer.Target = shot.Origin;
                    float num = 0f;
                    if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
                    {
                        float magnitude = (shot.Origin - transform.position).magnitude;
                        num = 6f / magnitude;
                        gameObject4.transform.position = shot.Origin * num + transform.position * (1f - num);
                        gameObject4.transform.LookAt(shot.Origin);
                        if ((UnityEngine.Object)transform2 != (UnityEngine.Object)null)
                        {
                            gameObject4 = UnityEngine.Object.Instantiate(this.machineGunPrefab);
                            tracer = (Tracer)gameObject4.GetComponent("Tracer");
                            tracer.Target = shot.Origin;
                            float magnitude2 = (shot.Origin - transform2.position).magnitude;
                            num = 6f / magnitude2;
                            gameObject4.transform.position = shot.Origin * num + transform2.position * (1f - num);
                            gameObject4.transform.LookAt(shot.Origin);
                        }
                    }
                    else
                    {
                        gameObject4.transform.position = shot.Origin * num + gameObject.transform.position * (1f - num);
                        gameObject4.transform.LookAt(shot.Origin);
                    }
                }
            }
        }
    }

    public void TauntEffect(AudioSource audio, string tauntID)
    {
        SoundManager.Instance.Play(audio, tauntID, AudioPlayMode.Play);
    }

    public void PlayerSoundEffect(AudioSource audio, string effectName)
    {
        SoundManager.Instance.Play(audio, effectName, AudioPlayMode.Play);
    }

    public void FallDownEffect(CombatPlayer player)
    {
        SoundManager.Instance.Play(player.Audio, "FallDown", AudioPlayMode.Play);
    }

    public void HeadShotEffect(CombatPlayer player)
    {
        SoundManager.Instance.Play(player.Audio, "HeadShot", AudioPlayMode.Play);
    }

    public void NutShotEffect(CombatPlayer player)
    {
        SoundManager.Instance.Play(player.Audio, "NutShot", AudioPlayMode.Play);
    }

    public void FirstBloodEffect(CombatPlayer player)
    {
        SoundManager.Instance.Play(player.Audio, "FirstBlood", AudioPlayMode.Play);
    }

    public void DoubleKillEffect(CombatPlayer player)
    {
        SoundManager.Instance.Play(player.Audio, "DoubleKill", AudioPlayMode.Play);
    }

    public void StartGameEffect(CombatPlayer player)
    {
        SoundManager.Instance.Play(player.Audio, "Trevoga", AudioPlayMode.Play);
    }

    public void EndGameEffect(CombatPlayer player)
    {
        SoundManager.Instance.Play(player.Audio, "RadioMoscow", AudioPlayMode.Play);
    }

    public Transform getWeaponTransformByName(Transform playerTransform, string weaponName)
    {
        Transform result = null;
        WeaponLook[] componentsInChildren = playerTransform.GetComponentsInChildren<WeaponLook>(true);
        WeaponLook[] array = componentsInChildren;
        foreach (WeaponLook weaponLook in array)
        {
            if (weaponLook.gameObject.name == weaponName)
            {
                result = weaponLook.transform;
                break;
            }
        }
        return result;
    }

    public void shotGunEffect(Shot shot, CombatPlayer player, bool isMe)
    {
        if (!((UnityEngine.Object)player == (UnityEngine.Object)null))
        {
            GameObject gameObject = null;
            Transform transform = null;
            Transform transform2 = null;
            gameObject = player.gameObject;
            System.Random random = new System.Random();
            string name = CombatWeapon.getName(shot.WeaponType);
            Transform transform3 = this.getWeaponTransformByName(player.transform, name);
            if ((UnityEngine.Object)transform3 != (UnityEngine.Object)null)
            {
                transform3 = transform3.GetChild(0);
                if ((UnityEngine.Object)transform3 != (UnityEngine.Object)null && transform3.GetChildCount() > 0)
                {
                    transform = transform3.Find("Target");
                    this.muzzleSparkEffect(transform, this.MachineGunShaftSparkPrefab, isMe);
                }
                else
                {
                    transform = gameObject.transform;
                }
            }
            if (transform3.name == "SG_DB")
            {
                SoundManager.Instance.Play(player.Audio, "DB_Shot", AudioPlayMode.Play);
            }
            else
            {
                SoundManager.Instance.Play(player.Audio, "Winchester1887_Shot", AudioPlayMode.Play);
            }
            int num = 6;
            float num2 = (shot.Origin - transform.position).magnitude * 0.07f;
            float num3 = num2 / 2f;
            for (int i = 0; i < num; i++)
            {
                Vector3 b = new Vector3((float)random.NextDouble() * num2 - num3, (float)random.NextDouble() * num2 - num3, (float)random.NextDouble() * num2 - num3);
                if (shot.Targets.Count > 0)
                {
                    this.sparkEffect(shot.Origin + b, shot.Direction, this.laserSparkPrefab);
                }
                else
                {
                    this.sparkEffect(shot.Origin + b, shot.Direction, this.machineGunSparkPrefab);
                }
                this.tracerEffect(transform.position, shot.Origin + b, this.machineGunPrefab, true, 0.15f, 0.03f);
            }
        }
    }

    public void reloadEffect(WeaponType weaponType, CombatPlayer player, string weaponSystemName, int ammoToLoad)
    {
        switch (weaponType)
        {
            case WeaponType.FLAMER:
            case WeaponType.SNOW_GUN:
            case WeaponType.ACID_THROWER:
            case WeaponType.ELECTRO_SHOCKER:
            case WeaponType.BIO_SHOCKER:
                break;
            case WeaponType.GATLING_GUN:
                break;
            case WeaponType.HAND_GUN:
                if (weaponSystemName == "HG_Glock_S")
                {
                    SoundManager.Instance.Play(player.Audio, "Glock_Reload", AudioPlayMode.Play);
                }
                else
                {
                    SoundManager.Instance.Play(player.Audio, "Makarov_Reload", AudioPlayMode.Play);
                }
                break;
            case WeaponType.MACHINE_GUN:
                if (weaponSystemName == "MG_UMP45" || weaponSystemName.StartsWith("MG_AUG"))
                {
                    SoundManager.Instance.Play(player.Audio, "TMP_Reload", AudioPlayMode.Play);
                }
                else
                {
                    SoundManager.Instance.Play(player.Audio, "AK47_Reload", AudioPlayMode.Play);
                }
                break;
            case WeaponType.SHOT_GUN:
                if (weaponSystemName == "SG_DB")
                {
                    SoundManager.Instance.Play(player.Audio, "DB_Reload", AudioPlayMode.Play);
                }
                else
                {
                    if (ammoToLoad == 0)
                    {
                        ammoToLoad = 2;
                    }
                    SoundManager.Instance.Play(player.Audio, "Winchester1887_ReloadStart", AudioPlayMode.Play);
                    float num3 = 0.3f;
                    for (int k = 0; k < ammoToLoad; k++)
                    {
                        SoundManager.Instance.PlayAfterSeconds(player.Audio, "Winchester1887_ReloadAmmo", AudioPlayMode.Play, num3);
                        num3 += 0.5f;
                    }
                    SoundManager.Instance.PlayAfterSeconds(player.Audio, "Winchester1887_ReloadEnd", AudioPlayMode.Play, num3);
                }
                break;
            case WeaponType.ROCKET_LAUNCHER:
                SoundManager.Instance.Play(player.Audio, "RPG26_Reload", AudioPlayMode.Play);
                break;
            case WeaponType.SNIPER_RIFLE:
                if (!weaponSystemName.StartsWith("SR_Wildcat"))
                {
                    if (weaponSystemName == "SR_Steyr" || weaponSystemName == "SR_Arctic")
                    {
                        SoundManager.Instance.Play(player.Audio, "Steyr_Reload", AudioPlayMode.Play);
                    }
                    else
                    {
                        SoundManager.Instance.Play(player.Audio, "SVD_Reload", AudioPlayMode.Play);
                    }
                }
                break;
            case WeaponType.GRENADE_LAUNCHER:
            case WeaponType.BOMB_LAUNCHER:
                if (weaponSystemName.Contains("SnowLauncher"))
                {
                    if (ammoToLoad == 0)
                    {
                        ammoToLoad = 2;
                    }
                    SoundManager.Instance.Play(player.Audio, "SnowLauncher_ReloadStart", AudioPlayMode.Play);
                    float num = 0.7f;
                    for (int i = 0; i < ammoToLoad; i++)
                    {
                        SoundManager.Instance.PlayAfterSeconds(player.Audio, "SnowLauncher_ReloadAmmo", AudioPlayMode.Play, num);
                        num += 1f;
                    }
                    SoundManager.Instance.PlayAfterSeconds(player.Audio, "SnowLauncher_ReloadEnd", AudioPlayMode.Play, num);
                }
                else if (weaponSystemName.Contains("Sticky"))
                {
                    if (ammoToLoad == 0)
                    {
                        ammoToLoad = 2;
                    }
                    float num2 = 0.7f;
                    for (int j = 0; j < ammoToLoad; j++)
                    {
                        SoundManager.Instance.PlayAfterSeconds(player.Audio, "Steyr_Reload", AudioPlayMode.Play, num2);
                        num2 += 1f;
                    }
                }
                break;
        }
    }

    public void batEffect(Shot shot, CombatPlayer player, bool isMe)
    {
        if (!((UnityEngine.Object)player == (UnityEngine.Object)null))
        {
            switch (shot.LaunchMode)
            {
                case LaunchModes.LAUNCH:
                    SoundManager.Instance.Play(player.Audio, "Bat_Launch", AudioPlayMode.Play);
                    break;
                case LaunchModes.SHOT:
                    if (shot.HasTargets)
                    {
                        SoundManager.Instance.Play(player.Audio, "Bat_Shot_Enemy", AudioPlayMode.PlayStop);
                    }
                    else if (shot.HasOrigin && shot.HasDirection)
                    {
                        this.sparkEffect(shot.Origin, shot.Direction, this.laserSparkPrefab);
                        SoundManager.Instance.Play(player.Audio, "Bat_Shot_Stone", AudioPlayMode.PlayStop);
                    }
                    break;
            }
        }
    }

    public void gatlingGunEffect(LaunchModes launchMode, CombatPlayer player, bool isMe)
    {
        if (!((UnityEngine.Object)player == (UnityEngine.Object)null))
        {
            switch (launchMode)
            {
                case LaunchModes.TURRET_SHOT:
                case LaunchModes.TURRET_CONTROL:
                    break;
                case LaunchModes.SPIN:
                    SoundManager.Instance.Play(player.Audio, "M134_Spin", AudioPlayMode.PlayLoop);
                    break;
                case LaunchModes.LAUNCH:
                    SoundManager.Instance.Play(player.Audio, "M134_Launch", AudioPlayMode.Play);
                    break;
                case LaunchModes.BLOW:
                    SoundManager.Instance.Play(player.Audio, "M134_Stop", AudioPlayMode.PlayStop);
                    break;
            }
        }
    }

    public void machineGunEffect(Shot shot, CombatPlayer player, bool isMe)
    {
        if (!((UnityEngine.Object)player == (UnityEngine.Object)null))
        {
            GameObject gameObject = null;
            Transform transform = null;
            Transform transform2 = null;
            gameObject = player.gameObject;
            System.Random random = new System.Random();
            string name = CombatWeapon.getName(shot.WeaponType);
            Transform transform3 = this.getWeaponTransformByName(player.transform, name);
            if ((UnityEngine.Object)transform3 != (UnityEngine.Object)null && transform3.GetChildCount() > 0)
            {
                transform3 = transform3.GetChild(0);
                if ((UnityEngine.Object)transform3 != (UnityEngine.Object)null)
                {
                    transform = transform3.Find("Target");
                }
                else
                {
                    transform3 = this.getWeaponTransformByName(player.transform, name + "L");
                    if ((UnityEngine.Object)transform3 != (UnityEngine.Object)null)
                    {
                        transform = transform3.Find("Target");
                    }
                    transform3 = this.getWeaponTransformByName(player.transform, name + "R");
                    if ((UnityEngine.Object)transform3 != (UnityEngine.Object)null)
                    {
                        transform2 = transform3.Find("Target");
                    }
                }
            }
            float tracerTime = 0.03f;
            float tracerWidth = 0.05f;
            switch (shot.WeaponType)
            {
                case WeaponType.HAND_GUN:
                    if (transform3.name == "HG_Glock_S")
                    {
                        SoundManager.Instance.Play(player.Audio, "Glock_Shot", AudioPlayMode.Play);
                    }
                    else
                    {
                        SoundManager.Instance.Play(player.Audio, "Makarov_Shot", AudioPlayMode.Play);
                    }
                    break;
                case WeaponType.MACHINE_GUN:
                {
                    tracerWidth = 0.1f;
                    CombatWeapon weaponByType2 = player.GetWeaponByType(shot.WeaponType);
                    string text = "AK47_Shot";
                    if (weaponByType2.SystemName.StartsWith("MG_UMP45"))
                    {
                        text = ((!weaponByType2.SystemName.StartsWith("MG_UMP45D")) ? "UMP45D_Shot" : "UMP45_Shot");
                    }
                    else if (weaponByType2.SystemName.StartsWith("MG_AUG") || weaponByType2.SystemName.StartsWith("MG_AssaultRifle03"))
                    {
                        text = "UMP45_Shot";
                    }
                    SoundManager.Instance.Play(player.Audio, text, AudioPlayMode.Play);
                    if (weaponByType2 != null)
                    {
                        SoundManager.Instance.PlayAfterSeconds(player.Audio, string.Format("{0}{1}", text, 1), AudioPlayMode.Play, (float)weaponByType2.ShotTime / 2E+07f);
                    }
                    break;
                }
                case WeaponType.SHOT_GUN:
                    tracerWidth = 0.07f;
                    if (transform3.name == "SG_DB")
                    {
                        SoundManager.Instance.Play(player.Audio, "DB_Shot", AudioPlayMode.Play);
                    }
                    else
                    {
                        SoundManager.Instance.Play(player.Audio, "Winchester1887_Shot", AudioPlayMode.Play);
                    }
                    break;
                case WeaponType.SNIPER_RIFLE:
                {
                    tracerWidth = 0.1f;
                    tracerTime = 0.1f;
                    CombatWeapon weaponByType = player.GetWeaponByType(shot.WeaponType);
                    if (weaponByType.SystemName.StartsWith("SR_Wildcat"))
                    {
                        SoundManager.Instance.Play(player.Audio, "Crossbow_Shot", AudioPlayMode.Play);
                    }
                    else
                    {
                        SoundManager.Instance.Play(player.Audio, "SVD_Shot", AudioPlayMode.Play);
                    }
                    break;
                }
                case WeaponType.GATLING_GUN:
                    if (transform3.name == "GG_M249")
                    {
                        tracerWidth = 0.2f;
                        SoundManager.Instance.Play(player.Audio, "M249_Shot", AudioPlayMode.PlayLoop);
                    }
                    else
                    {
                        tracerWidth = 0.1f;
                        SoundManager.Instance.Play(player.Audio, "M134_Shot", AudioPlayMode.PlayLoop);
                    }
                    break;
            }
            if (transform3.name.StartsWith("SR_Wildcat"))
            {
                if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
                {
                    GameObject prefab = CharacterManager.Instance.GetPrefab("CrossbowBoltPrefab");
                    GameObject gameObject2 = UnityEngine.Object.Instantiate(prefab);
                    this.sparkEffect(shot.Origin, shot.Direction, this.laserSparkPrefab);
                    this.arrowTracerEffect(transform.position, shot.Origin, prefab, false, tracerWidth, 0.015f, shot.Targets != null && shot.Targets.Count > 0);
                }
            }
            else
            {
                GameObject gameObject3 = null;
                if (shot.Targets.Count == 0)
                {
					gameObject3 = this.sparkEffect(shot.Origin, shot.Direction, this.machineGunSparkPrefab);
                    SoundManager.Instance.Play(gameObject3.GetComponent<AudioSource>(), "BulletImpact", AudioPlayMode.Play);
                }
                if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
                {
                    this.muzzleSparkEffect(transform, this.MachineGunShaftSparkPrefab, isMe);
                    this.tracerEffect(transform.position, shot.Origin, this.machineGunPrefab, false, tracerWidth, tracerTime);
                    if ((UnityEngine.Object)transform2 != (UnityEngine.Object)null)
                    {
                        this.muzzleSparkEffect(transform2, this.MachineGunShaftSparkPrefab, isMe);
                        this.tracerEffect(transform2.position, shot.Origin, this.machineGunPrefab, false, tracerWidth, tracerTime);
                    }
                }
                else
                {
                    this.tracerEffect(gameObject.transform.position, shot.Origin, this.machineGunPrefab, false, tracerWidth, tracerTime);
                }
            }
        }
    }

    public void muzzleSparkEffect(Transform muzzle, GameObject sparkPrefab, bool isMe)
    {
        if (!((UnityEngine.Object)muzzle == (UnityEngine.Object)null))
        {
            GameObject gameObject = UnityEngine.Object.Instantiate((UnityEngine.Object)sparkPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            gameObject.transform.parent = muzzle;
            gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
            if (isMe)
            {
                gameObject.layer = muzzle.gameObject.layer;
            }
        }
    }

    public GameObject sparkEffect(Vector3 origin, Vector3 direction, GameObject sparkPrefab)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate(sparkPrefab);
        gameObject.transform.position = origin;
        if (direction != Vector3.zero)
        {
            gameObject.transform.rotation = Quaternion.LookRotation(direction);
        }
        return gameObject;
    }

    public void arrowTracerEffect(Vector3 origin, Vector3 target, GameObject tracerPrefab, bool randomShift, float tracerWidth, float tracerTime, bool destroy)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate(tracerPrefab);
        ArrowTracer component = gameObject.GetComponent<ArrowTracer>();
        component.destroy = destroy;
        float num = 0f;
        component.FlatSpeed = 9f;
        if (randomShift)
        {
            component.FlatSpeed = 6f;
            num = 0f - Convert.ToSingle(this.rnd.NextDouble() * 0.004999999888241291);
        }
        gameObject.transform.position = target * num + origin * (1f - num);
        gameObject.transform.LookAt(target);
        component.Target = target;
        if ((UnityEngine.Object)component.Trail != (UnityEngine.Object)null)
        {
            component.Trail.time = tracerTime;
            component.Trail.startWidth = tracerWidth;
            component.Trail.endWidth = tracerWidth * 2f;
        }
    }

    public void tracerEffect(Vector3 origin, Vector3 target, GameObject tracerPrefab, bool randomShift, float tracerWidth, float tracerTime)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate(tracerPrefab);
        Tracer tracer = (Tracer)gameObject.GetComponent("Tracer");
        float num = 0f;
        tracer.FlatSpeed = 11f;
        if (randomShift)
        {
            tracer.FlatSpeed = 6f;
            num = 0f - Convert.ToSingle(this.rnd.NextDouble() * 0.004999999888241291);
        }
        gameObject.transform.position = target * num + origin * (1f - num);
        gameObject.transform.LookAt(target);
        tracer.Target = target;
        if ((UnityEngine.Object)tracer.Trail != (UnityEngine.Object)null)
        {
            tracer.Trail.time = tracerTime;
            tracer.Trail.startWidth = tracerWidth;
            tracer.Trail.endWidth = tracerWidth * 2f;
        }
    }

    public void energyBallLauncherEffect(Shot shot, CombatPlayer player, bool isMe)
    {
        if (!((UnityEngine.Object)player == (UnityEngine.Object)null))
        {
            SoundManager.Instance.Play(player.Audio, "Mini Rocket Flight", AudioPlayMode.Play);
            Transform transform = null;
            GameObject gameObject = player.gameObject;
            Transform transform2 = gameObject.transform.Find("SShip");
            Transform transform3 = transform2.Find("GrenadeLauncherWeapon");
            if ((UnityEngine.Object)transform3 != (UnityEngine.Object)null)
            {
                transform = transform3.Find("Target");
            }
            if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
            {
                GameObject gameObject2 = UnityEngine.Object.Instantiate((UnityEngine.Object)this.rocketLauncherShaftSparkPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
                gameObject2.transform.parent = transform;
                gameObject2.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
            }
            GameObject gameObject3 = UnityEngine.Object.Instantiate(this.energyBallPrefab);
            RocketTracer rocketTracer = (RocketTracer)gameObject3.GetComponent("RocketTracer");
            if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
            {
                gameObject3.transform.position = transform.position;
            }
            else
            {
                gameObject3.transform.position = gameObject.transform.position;
            }
            rocketTracer.Launch(shot, player, isMe);
            player.RegisterItem(rocketTracer);
        }
    }

    public void rocketLauncherEffect(Shot shot, CombatPlayer player, bool isMe)
    {
        if (!((UnityEngine.Object)player == (UnityEngine.Object)null))
        {
            SoundManager.Instance.Play(player.Audio, "RPG26_Shot", AudioPlayMode.Play);
            Transform transform = null;
            GameObject gameObject = player.gameObject;
            Transform weaponTransformByName = this.getWeaponTransformByName(player.transform, "RocketLauncherWeapon");
            if ((UnityEngine.Object)weaponTransformByName != (UnityEngine.Object)null)
            {
                weaponTransformByName = weaponTransformByName.GetChild(0);
                if ((UnityEngine.Object)weaponTransformByName != (UnityEngine.Object)null && weaponTransformByName.GetChildCount() > 0)
                {
                    transform = weaponTransformByName.Find("Target");
                }
                if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
                {
                    GameObject gameObject2 = UnityEngine.Object.Instantiate((UnityEngine.Object)this.rocketLauncherShaftSparkPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
                    gameObject2.transform.parent = transform;
                    gameObject2.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                    gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
                }
            }
            GameObject gameObject3 = UnityEngine.Object.Instantiate(this.whizbangPrefab);
            RocketTracer rocketTracer = (RocketTracer)gameObject3.GetComponent("RocketTracer");
            if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
            {
                gameObject3.transform.position = transform.position;
            }
            else
            {
                gameObject3.transform.position = gameObject.transform.position;
            }
            rocketTracer.Launch(shot, player, isMe);
            player.RegisterItem(rocketTracer);
        }
    }

    public void tripleRocketLauncherEffect(Shot shot, CombatPlayer player, bool isMe, float rocketIndex)
    {
        if (!((UnityEngine.Object)player == (UnityEngine.Object)null))
        {
            SoundManager.Instance.Play(player.Audio, "Mini Rocket Flight", AudioPlayMode.Play);
            Transform transform = null;
            GameObject gameObject = player.gameObject;
            Transform transform2 = gameObject.transform.Find("SShip");
            Transform transform3 = transform2.Find("TripleRocketLauncherWeapon");
            if ((UnityEngine.Object)transform3 != (UnityEngine.Object)null)
            {
                transform = transform3.Find("Target");
            }
            if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
            {
                GameObject gameObject2 = UnityEngine.Object.Instantiate((UnityEngine.Object)this.rocketLauncherShaftSparkPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
                gameObject2.transform.parent = transform;
                gameObject2.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
            }
            GameObject gameObject3 = UnityEngine.Object.Instantiate(this.tripleRocketPrefab);
            TripleRocketTracer tripleRocketTracer = (TripleRocketTracer)gameObject3.GetComponent("RocketTracer");
            if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
            {
                gameObject3.transform.position = transform.position;
            }
            else
            {
                gameObject3.transform.position = gameObject.transform.position;
            }
            tripleRocketTracer.Launch(shot, player, isMe, rocketIndex);
            player.RegisterItem(tripleRocketTracer);
        }
    }

    public void bombLauncherEffect(Shot shot, CombatWeapon weapon, CombatPlayer player, bool isMe, bool gameState)
    {
        if (!((UnityEngine.Object)player == (UnityEngine.Object)null))
        {
            SoundManager.Instance.Play(player.Audio, "SnowLauncher_Shot", AudioPlayMode.Play);
            Transform transform = null;
            GameObject gameObject = player.gameObject;
            Transform transform2 = this.getWeaponTransformByName(player.transform, "BombLauncherWeapon");
            if ((UnityEngine.Object)transform2 != (UnityEngine.Object)null)
            {
                transform2 = transform2.FindChild(player.GetWeaponByType(WeaponType.BOMB_LAUNCHER).SystemName);
                if ((UnityEngine.Object)transform2 != (UnityEngine.Object)null && transform2.GetChildCount() > 0)
                {
                    transform = transform2.Find("Target");
                }
                if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
                {
                    GameObject gameObject2 = UnityEngine.Object.Instantiate((UnityEngine.Object)this.grenadeLauncherShaftSparkPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
                    gameObject2.transform.parent = transform;
                    gameObject2.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                    gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
                }
            }
            GameObject prefab = this.mortarWhizbangPrefab;
            if ((UnityEngine.Object)transform2 != (UnityEngine.Object)null && transform2.name.Contains("Sticky"))
            {
                prefab = CharacterManager.Instance.GetPrefab("BombPrefab");
            }
            GameObject gameObject3 = UnityEngine.Object.Instantiate(prefab);
            MeshRenderer componentInChildren = gameObject3.GetComponentInChildren<MeshRenderer>();
            switch (player.Team)
            {
                case 1:
                    componentInChildren.material.color = new Color(1f, 0.321568638f, 0.321568638f);
                    break;
                case 2:
                    componentInChildren.material.color = new Color(0.274509817f, 0.5176471f, 1f);
                    break;
            }
            BombTracer bombTracer = (BombTracer)gameObject3.GetComponent("BombTracer");
            if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
            {
                gameObject3.transform.position = transform.position;
            }
            else
            {
                gameObject3.transform.position = gameObject.transform.position;
            }
            bombTracer.Launch(shot, weapon, player, isMe, gameState);
            player.RegisterItem(bombTracer);
        }
    }

    public void grenadeLauncherEffect(Shot shot, CombatPlayer player, bool isMe)
    {
        if (!((UnityEngine.Object)player == (UnityEngine.Object)null))
        {
            SoundManager.Instance.Play(player.Audio, "SnowLauncher_Shot", AudioPlayMode.Play);
            Transform transform = null;
            GameObject gameObject = player.gameObject;
            Transform transform2 = this.getWeaponTransformByName(player.transform, "GrenadeLauncherWeapon");
            if ((UnityEngine.Object)transform2 != (UnityEngine.Object)null)
            {
                transform2 = transform2.FindChild(player.GetWeaponByType(WeaponType.GRENADE_LAUNCHER).SystemName);
                if ((UnityEngine.Object)transform2 != (UnityEngine.Object)null && transform2.GetChildCount() > 0)
                {
                    transform = transform2.Find("Target");
                }
                if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
                {
                    GameObject gameObject2 = UnityEngine.Object.Instantiate((UnityEngine.Object)this.grenadeLauncherShaftSparkPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
                    gameObject2.transform.parent = transform;
                    gameObject2.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                    gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
                }
            }
            GameObject gameObject3 = this.mortarWhizbangPrefab;
            gameObject3 = ((!((UnityEngine.Object)transform2 != (UnityEngine.Object)null) || !transform2.name.Contains("Snow")) ? CharacterManager.Instance.GetPrefab("GrenadePrefab") : CharacterManager.Instance.GetPrefab("SnowballPrefab"));
            GameObject gameObject4 = UnityEngine.Object.Instantiate(gameObject3);
			GrenadeTracer grenadeTracer = (GrenadeTracer)gameObject4.GetComponent("GrenadeTracer");
            if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
            {
                gameObject4.transform.position = transform.position;
            }
            else
            {
                gameObject4.transform.position = gameObject.transform.position;
            }
            grenadeTracer.Launch(shot, player, isMe);
            player.RegisterItem(grenadeTracer);
        }
    }

    public void powerLaserEffect(Shot shot, CombatPlayer player, bool isMe)
    {
        if (!((UnityEngine.Object)player == (UnityEngine.Object)null))
        {
            SoundManager.Instance.Play(player.Audio, "Mini Laser", AudioPlayMode.Play);
            Transform transform = null;
            Transform transform2 = null;
            GameObject gameObject = player.gameObject;
            Transform transform3 = gameObject.transform.Find("SShip");
            Transform transform4 = transform3.Find("LaserGunWeaponL");
            if ((UnityEngine.Object)transform4 != (UnityEngine.Object)null)
            {
                transform = transform4.Find("Target");
            }
            if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
            {
                GameObject gameObject2 = UnityEngine.Object.Instantiate((UnityEngine.Object)this.powerLaserShaftSparkPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
                gameObject2.transform.parent = transform;
                gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
            }
            transform4 = transform3.Find("LaserGunWeaponR");
            if ((UnityEngine.Object)transform4 != (UnityEngine.Object)null)
            {
                transform2 = transform4.Find("Target");
            }
            GameObject gameObject3 = UnityEngine.Object.Instantiate(this.powerLaserSparkPrefab);
            gameObject3.transform.position = shot.Origin;
            if (shot.Direction != Vector3.zero)
            {
                gameObject3.transform.rotation = Quaternion.LookRotation(shot.Direction);
            }
            GameObject gameObject4 = UnityEngine.Object.Instantiate(this.powerLaserPrefab);
            Tracer tracer = (Tracer)gameObject4.GetComponent("Tracer");
            tracer.Target = shot.Origin;
            float num = 0f;
            if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
            {
                float magnitude = (shot.Origin - transform.position).magnitude;
                num = 6f / magnitude;
                gameObject4.transform.position = shot.Origin * num + transform.position * (1f - num);
            }
            else
            {
                num = 0.1f;
                gameObject4.transform.position = shot.Origin * num + gameObject.transform.position * (1f - num);
            }
            gameObject4.transform.LookAt(shot.Origin);
            if ((UnityEngine.Object)transform2 != (UnityEngine.Object)null)
            {
                gameObject4 = UnityEngine.Object.Instantiate(this.powerLaserPrefab);
                tracer = (Tracer)gameObject4.GetComponent("Tracer");
                tracer.Target = shot.Origin;
                float magnitude2 = (shot.Origin - transform2.position).magnitude;
                num = 6f / magnitude2;
                gameObject4.transform.position = shot.Origin * num + transform2.position * (1f - num);
            }
            gameObject4.transform.LookAt(shot.Origin);
        }
    }

    public void laserEffect(Shot shot, CombatPlayer player, bool isMe)
    {
        if (!((UnityEngine.Object)player == (UnityEngine.Object)null))
        {
            SoundManager.Instance.Play(player.Audio, "Mini Laser", AudioPlayMode.Play);
            Transform transform = null;
            Transform transform2 = null;
            GameObject gameObject = player.gameObject;
            Transform weaponTransformByName = this.getWeaponTransformByName(player.transform, "LaserGunWeapon");
            if ((UnityEngine.Object)weaponTransformByName != (UnityEngine.Object)null)
            {
                transform = weaponTransformByName.Find("Target");
            }
            else
            {
                weaponTransformByName = this.getWeaponTransformByName(player.transform, "LaserGunWeaponL");
                if ((UnityEngine.Object)weaponTransformByName != (UnityEngine.Object)null)
                {
                    transform = weaponTransformByName.Find("Target");
                }
                weaponTransformByName = this.getWeaponTransformByName(player.transform, "LaserGunWeaponR");
                if ((UnityEngine.Object)weaponTransformByName != (UnityEngine.Object)null)
                {
                    transform2 = weaponTransformByName.Find("Target");
                }
            }
            if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
            {
                GameObject gameObject2 = UnityEngine.Object.Instantiate((UnityEngine.Object)this.laserShaftSparkPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
                gameObject2.transform.parent = transform;
                gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
            }
            if ((UnityEngine.Object)transform2 != (UnityEngine.Object)null)
            {
                GameObject gameObject2 = UnityEngine.Object.Instantiate((UnityEngine.Object)this.laserShaftSparkPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
                gameObject2.transform.parent = transform2;
                gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
            }
            GameObject gameObject3 = UnityEngine.Object.Instantiate(this.laserSparkPrefab);
            gameObject3.transform.position = shot.Origin;
            if (shot.Direction != Vector3.zero)
            {
                gameObject3.transform.rotation = Quaternion.LookRotation(shot.Direction);
            }
            GameObject gameObject4 = UnityEngine.Object.Instantiate(this.laserGunPrefab);
            Tracer tracer = (Tracer)gameObject4.GetComponent("Tracer");
            tracer.Target = shot.Origin;
            float num = 0f;
            if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
            {
                float magnitude = (shot.Origin - transform.position).magnitude;
                num = 6f / magnitude;
                gameObject4.transform.position = shot.Origin * num + transform.position * (1f - num);
            }
            else
            {
                num = 0.1f;
                gameObject4.transform.position = shot.Origin * num + gameObject.transform.position * (1f - num);
            }
            gameObject4.transform.LookAt(shot.Origin);
            if ((UnityEngine.Object)transform2 != (UnityEngine.Object)null)
            {
                gameObject4 = UnityEngine.Object.Instantiate(this.laserGunPrefab);
                tracer = (Tracer)gameObject4.GetComponent("Tracer");
                tracer.Target = shot.Origin;
                float magnitude2 = (shot.Origin - transform2.position).magnitude;
                num = 6f / magnitude2;
                gameObject4.transform.position = shot.Origin * num + transform2.position * (1f - num);
                gameObject4.transform.LookAt(shot.Origin);
            }
        }
    }

    public void plasmaEffect(Shot shot, CombatPlayer player, bool isMe)
    {
        if (!((UnityEngine.Object)player == (UnityEngine.Object)null))
        {
            SoundManager.Instance.Play(player.Audio, "plasma", AudioPlayMode.Play);
            Transform transform = null;
            GameObject gameObject = player.gameObject;
            Transform transform2 = gameObject.transform.Find("SShip");
            Transform transform3 = transform2.Find("PlasmaWeapon");
            if ((UnityEngine.Object)transform3 != (UnityEngine.Object)null)
            {
                transform = transform3.Find("Target");
            }
            if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
            {
                GameObject gameObject2 = UnityEngine.Object.Instantiate((UnityEngine.Object)this.plasmaShaftSparkPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
                gameObject2.transform.parent = transform;
                gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
            }
            GameObject gameObject3 = UnityEngine.Object.Instantiate(this.plasmaSparkPrefab);
            gameObject3.transform.position = shot.Origin;
            if (shot.Direction != Vector3.zero)
            {
                gameObject3.transform.rotation = Quaternion.LookRotation(shot.Direction);
            }
            GameObject gameObject4 = UnityEngine.Object.Instantiate(this.plasmaPrefab);
            Tracer tracer = (Tracer)gameObject4.GetComponent("Tracer");
            tracer.Target = shot.Origin;
            float num = 0f;
            if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
            {
                float magnitude = (shot.Origin - transform.position).magnitude;
                num = 5f / magnitude;
                gameObject4.transform.position = shot.Origin * num + transform.position * (1f - num);
            }
            else
            {
                gameObject4.transform.position = shot.Origin * num + gameObject.transform.position * (1f - num);
            }
            gameObject4.transform.LookAt(shot.Origin);
        }
    }

    public void ultraEffect(Shot shot, CombatPlayer player, bool isMe)
    {
        if (!((UnityEngine.Object)player == (UnityEngine.Object)null))
        {
            SoundManager.Instance.Play(player.Audio, "plasma", AudioPlayMode.Play);
            Transform transform = null;
            GameObject gameObject = player.gameObject;
            Transform transform2 = gameObject.transform.Find("SShip");
            Transform transform3 = transform2.Find("PowerPlasmaWeapon");
            if ((UnityEngine.Object)transform3 != (UnityEngine.Object)null)
            {
                transform = transform3.Find("Target");
            }
            if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
            {
                GameObject gameObject2 = UnityEngine.Object.Instantiate((UnityEngine.Object)this.ultraShaftSparkPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
                gameObject2.transform.parent = transform;
                gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
            }
            GameObject gameObject3 = UnityEngine.Object.Instantiate(this.ultraSparkPrefab);
            gameObject3.transform.position = shot.Origin;
            if (shot.Direction != Vector3.zero)
            {
                gameObject3.transform.rotation = Quaternion.LookRotation(shot.Direction);
            }
            GameObject gameObject4 = UnityEngine.Object.Instantiate(this.ultraPrefab);
            Tracer tracer = (Tracer)gameObject4.GetComponent("Tracer");
            tracer.Target = shot.Origin;
            float num = 0f;
            if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
            {
                float magnitude = (shot.Origin - transform.position).magnitude;
                num = 5f / magnitude;
                gameObject4.transform.position = shot.Origin * num + transform.position * (1f - num);
            }
            else
            {
                gameObject4.transform.position = shot.Origin * num + gameObject.transform.position * (1f - num);
            }
            gameObject4.transform.LookAt(shot.Origin);
        }
    }

    public void rgunEffect(Shot shot, CombatPlayer player, bool isMe)
    {
        if (!((UnityEngine.Object)player == (UnityEngine.Object)null))
        {
            SoundManager.Instance.Play(player.Audio, "rail shot", AudioPlayMode.Play);
            Transform transform = null;
            GameObject gameObject = player.gameObject;
            Transform transform2 = gameObject.transform.Find("SShip");
            Transform transform3 = transform2.Find("RailGunWeapon");
            if ((UnityEngine.Object)transform3 != (UnityEngine.Object)null)
            {
                transform = transform3.Find("Target");
            }
            AnimationSynchronizer component = gameObject.GetComponent<AnimationSynchronizer>();
            if ((UnityEngine.Object)component != (UnityEngine.Object)null)
            {
                component.PlayRailGunAnimation();
            }
            GameObject gameObject2 = UnityEngine.Object.Instantiate(this.railPrefab);
            float num = Vector3.Distance(shot.Origin, gameObject.transform.position);
            float num2 = 0f;
            if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
            {
                gameObject2.transform.position = shot.Origin * num2 + transform.position * (1f - num2);
            }
            else
            {
                gameObject2.transform.position = shot.Origin * num2 + gameObject.transform.position * (1f - num2);
            }
            gameObject2.transform.LookAt(shot.Origin);
            gameObject2.transform.localScale = new Vector3(3f, 3f, num / 11f);
        }
    }

    public void SpiralRailGunEffect(Shot shot, CombatPlayer player, bool isMe)
    {
        if (!((UnityEngine.Object)player == (UnityEngine.Object)null))
        {
            SoundManager.Instance.Play(player.Audio, "rail shot", AudioPlayMode.Play);
            Transform transform = null;
            GameObject gameObject = player.gameObject;
            Transform transform2 = gameObject.transform.Find("SShip");
            Transform transform3 = transform2.Find("RailGunWeapon");
            if ((UnityEngine.Object)transform3 != (UnityEngine.Object)null)
            {
                transform = transform3.Find("Target");
            }
            GameObject gameObject2 = UnityEngine.Object.Instantiate(this.spiralRailPrefab);
            float num = Vector3.Distance(shot.Origin, gameObject.transform.position);
            float num2 = 0f;
            if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
            {
                gameObject2.transform.position = shot.Origin * num2 + transform.position * (1f - num2);
            }
            else
            {
                gameObject2.transform.position = shot.Origin * num2 + gameObject.transform.position * (1f - num2);
            }
            gameObject2.transform.LookAt(shot.Origin);
            LocalSpiralRailTracer component = gameObject2.GetComponent<LocalSpiralRailTracer>();
            component.Launch(shot, player, isMe);
        }
    }

    public void electricLauncherEffect(Shot shot, CombatPlayer player, CombatPlayer localPlayer, bool isMe)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate(this.electricExploPrefab);
        gameObject.transform.position = shot.Origin;
        SoundManager.Instance.Play(gameObject.GetComponent<AudioSource>(), "rocket explosion 2", AudioPlayMode.Play);
        if (shot.Direction != Vector3.zero)
        {
            gameObject.transform.rotation = Quaternion.LookRotation(shot.Direction);
        }
        if (!((UnityEngine.Object)localPlayer == (UnityEngine.Object)null))
        {
            float num = Vector3.Distance(localPlayer.transform.position, gameObject.transform.position);
            if (num < 30f)
            {
                localPlayer.WalkController.setExplosionForce(gameObject.transform.position, (30f - num) * 1.5f);
            }
            player.UnregisterItem(shot.TimeStamp);
            if (shot.HasTargets)
            {
                List<ShotTarget>.Enumerator enumerator = shot.Targets.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        ShotTarget current = enumerator.Current;
                        Transform targetTransform = current.TargetTransform;
                        if (!((UnityEngine.Object)targetTransform == (UnityEngine.Object)null))
                        {
                            GameObject gameObject2 = UnityEngine.Object.Instantiate(this.blizzRailArcPrefab);
                            gameObject2.transform.position = shot.Origin;
                            MomentLightning componentInChildren = gameObject2.GetComponentInChildren<MomentLightning>();
                            componentInChildren.target = targetTransform;
                        }
                    }
                }
                finally
                {
                    ((IDisposable)enumerator).Dispose();
                }
            }
        }
    }

    public void energyBallLauncherExplosionEffect(Shot shot, CombatPlayer player, CombatPlayer localPlayer, bool isMe)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate(this.energyBallExploPrefab);
        gameObject.transform.position = shot.Origin;
        SoundManager.Instance.Play(gameObject.GetComponent<AudioSource>(), "rocket explosion 2", AudioPlayMode.Play);
        if (shot.Direction != Vector3.zero)
        {
            gameObject.transform.rotation = Quaternion.LookRotation(shot.Direction);
        }
        if (!((UnityEngine.Object)localPlayer == (UnityEngine.Object)null))
        {
            float num = Vector3.Distance(localPlayer.transform.position, gameObject.transform.position);
            if (num < 30f)
            {
                localPlayer.WalkController.setExplosionForce(gameObject.transform.position, (30f - num) * 1.5f);
            }
            player.UnregisterItem(shot.TimeStamp);
        }
    }

    public void mineDeactivateEffect(Shot shot, CombatPlayer player, CombatPlayer localPlayer, bool isMe)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate(this.exploDeactivationPrefab);
        gameObject.transform.position = shot.Origin;
        SoundManager.Instance.Play(gameObject.GetComponent<AudioSource>(), "rocket explosion 2", AudioPlayMode.Play);
        if (shot.Direction != Vector3.zero)
        {
            gameObject.transform.rotation = Quaternion.LookRotation(shot.Direction);
        }
        if (!((UnityEngine.Object)localPlayer == (UnityEngine.Object)null))
        {
            player.UnregisterItem(shot.TimeStamp);
        }
    }

    public void launcherEffect(Shot shot, CombatPlayer player, CombatPlayer localPlayer, bool isMe)
    {
        CombatWeapon weaponByType = player.GetWeaponByType(shot.WeaponType);
        GameObject gameObject;
        if (weaponByType.SystemName.Contains("SnowLauncher"))
        {
            gameObject = UnityEngine.Object.Instantiate(CharacterManager.Instance.GetPrefab("ExploSnowPrefab"));
        }
        else
        {
            gameObject = UnityEngine.Object.Instantiate(this.exploPrefab);
            if (shot.WeaponType == WeaponType.BOMB_LAUNCHER)
            {
                Projector componentInChildren = gameObject.GetComponentInChildren<Projector>();
                componentInChildren.enabled = false;
            }
            float sqrMagnitude = (PlayerManager.Instance.LocalPlayer.transform.position - shot.Origin).sqrMagnitude;
            float num = 0f;
            switch (OptionsManager.QualityLevel)
            {
                case QualityLevel.Fastest:
                case QualityLevel.Fast:
                    num = 0f;
                    break;
                case QualityLevel.Good:
                    num = 24000f;
                    break;
                case QualityLevel.Simple:
                    num = 48000f;
                    break;
                case QualityLevel.Beautiful:
                case QualityLevel.Fantastic:
                    num = 90000f;
                    break;
            }
            if (sqrMagnitude >= num)
            {
                Projector componentInChildren2 = gameObject.GetComponentInChildren<Projector>();
                componentInChildren2.enabled = false;
            }
        }
        gameObject.transform.position = shot.Origin;
        SoundManager.Instance.Play(gameObject.GetComponent<AudioSource>(), "RPG26_Explosion", AudioPlayMode.Play);
        if (shot.Direction != Vector3.zero)
        {
            gameObject.transform.rotation = Quaternion.LookRotation(shot.Direction);
        }
        if (!((UnityEngine.Object)localPlayer == (UnityEngine.Object)null))
        {
            float num2 = Vector3.Distance(localPlayer.transform.position, gameObject.transform.position);
            float num3 = weaponByType.Distance * 1.5f;
            if (shot.WeaponType == WeaponType.ROCKET_LAUNCHER)
            {
                num3 = 30f;
            }
            if (shot.WeaponType == WeaponType.BOMB_LAUNCHER && shot.Targets.Count == 0)
            {
                player.UnregisterItem(shot.TimeStamp);
            }
            else
            {
                if (num2 < num3)
                {
                    CharacterMotor componentInChildren3 = ((Component)localPlayer).GetComponentInChildren<CharacterMotor>();
                    float num4 = (num3 - num2) * 1.5f;
                    switch (localPlayer.ZombieType)
                    {
                        case ZombieType.Boss:
                            num4 /= 3f;
                            break;
                        case ZombieType.Regular:
                            num4 /= 2f;
                            break;
                    }
                    if (shot.WeaponType == WeaponType.BOMB_LAUNCHER)
                    {
                        num4 *= 0.33f;
                    }
                    componentInChildren3.SetExplosionForce(gameObject.transform.position - new Vector3(0f, 5f, 0f), num4);
                }
                player.UnregisterItem(shot.TimeStamp);
            }
        }
    }

    public void acidThrowerEffect(CombatPlayer player, bool isMe)
    {
        if (!((UnityEngine.Object)player == (UnityEngine.Object)null))
        {
            GameObject gameObject = player.gameObject;
            if (UnityEngine.Random.value >= 0.5f)
            {
                SoundManager.Instance.Play(player.Audio, "FT1", AudioPlayMode.Play);
            }
            else
            {
                SoundManager.Instance.Play(player.Audio, "FT2", AudioPlayMode.Play);
            }
            Flamer componentInChildren = gameObject.GetComponentInChildren<Flamer>();
            if ((bool)componentInChildren)
            {
                componentInChildren.fire(true);
            }
        }
    }

    public void flamerEffect(LaunchModes launchMode, CombatPlayer player, bool isMe)
    {
        if (launchMode == LaunchModes.BLOW)
        {
            GameObject gameObject = player.gameObject;
            Flamer componentInChildren = gameObject.GetComponentInChildren<Flamer>();
            if ((bool)componentInChildren)
            {
                componentInChildren.fire(false);
            }
        }
    }

    public void flamerEffect(Shot shot, CombatPlayer player, bool isMe)
    {
        if (!((UnityEngine.Object)player == (UnityEngine.Object)null))
        {
            GameObject gameObject = player.gameObject;
            if (player.GetWeaponByType(shot.WeaponType).SystemName.Contains("Snowgun"))
            {
                SoundManager.Instance.Play(player.Audio, "Snowgun_Shot", AudioPlayMode.Play);
            }
            else
            {
                SoundManager.Instance.Play(player.Audio, "Flamer_Shot", AudioPlayMode.Play);
            }
            Flamer componentInChildren = gameObject.GetComponentInChildren<Flamer>();
            if ((bool)componentInChildren)
            {
                componentInChildren.fire(true);
            }
        }
    }

    public void ultraSonicEffect(Shot shot, CombatPlayer player, bool isMe)
    {
        if (!((UnityEngine.Object)player == (UnityEngine.Object)null))
        {
            GameObject gameObject = player.gameObject;
            if (UnityEngine.Random.value >= 0.5f)
            {
                SoundManager.Instance.Play(player.Audio, "FT1", AudioPlayMode.Play);
            }
            else
            {
                SoundManager.Instance.Play(player.Audio, "FT2", AudioPlayMode.Play);
            }
            GameObject gameObject2 = UnityEngine.Object.Instantiate(this.ultraSonicPrefab);
            gameObject2.transform.parent = player.transform;
            gameObject2.transform.localPosition = new Vector3(0f, 0f, 5f);
            if (shot.Direction != Vector3.zero)
            {
                gameObject2.transform.rotation = Quaternion.LookRotation(shot.Direction);
            }
        }
    }

    public void lightningEffect(Shot shot, CombatPlayer player, ShotEffectType shotEffectType)
    {
        if (shotEffectType == ShotEffectType.ME_BEFORE_SERVER || shotEffectType == ShotEffectType.ENEMY_AFTER_SERVER)
        {
            if (UnityEngine.Random.value >= 0.5f)
            {
                SoundManager.Instance.Play(player.Audio, "MiniShock 1", AudioPlayMode.Play);
            }
            else
            {
                SoundManager.Instance.Play(player.Audio, "MiniShock 2", AudioPlayMode.Play);
            }
        }
        if (shotEffectType != ShotEffectType.ME_BEFORE_SERVER && !((UnityEngine.Object)player == (UnityEngine.Object)null))
        {
            GameObject gameObject = player.gameObject;
            DoubleLighter componentInChildren = gameObject.GetComponentInChildren<DoubleLighter>();
            if (shot.HasTargets)
            {
                Transform targetTransform = shot.Targets[0].TargetTransform;
                if (!((UnityEngine.Object)targetTransform == (UnityEngine.Object)null) && (bool)componentInChildren)
                {
                    componentInChildren.fire(targetTransform);
                }
            }
        }
    }

    public void megaChargerEffect(Shot shot, CombatPlayer player, ShotEffectType shotEffectType)
    {
        if (!((UnityEngine.Object)player == (UnityEngine.Object)null))
        {
            if (shotEffectType == ShotEffectType.ME_BEFORE_SERVER || shotEffectType == ShotEffectType.ENEMY_AFTER_SERVER)
            {
                SoundManager.Instance.Play(player.Audio, "mechnic gun heal", AudioPlayMode.Play);
            }
            if (shotEffectType != ShotEffectType.ME_BEFORE_SERVER)
            {
                GameObject gameObject = player.gameObject;
                MegaCharger componentInChildren = gameObject.GetComponentInChildren<MegaCharger>();
                if (shot.HasTargets)
                {
                    Transform targetTransform = shot.Targets[0].TargetTransform;
                    if (!((UnityEngine.Object)targetTransform == (UnityEngine.Object)null) && (bool)componentInChildren)
                    {
                        componentInChildren.fire(targetTransform);
                    }
                }
            }
        }
    }

    public void chargerEffect(Shot shot, CombatPlayer player, ShotEffectType shotEffectType)
    {
        if (!((UnityEngine.Object)player == (UnityEngine.Object)null))
        {
            if (shotEffectType == ShotEffectType.ME_BEFORE_SERVER || shotEffectType == ShotEffectType.ENEMY_AFTER_SERVER)
            {
                SoundManager.Instance.Play(player.Audio, "mechnic gun heal", AudioPlayMode.Play);
            }
            if (shotEffectType != ShotEffectType.ME_BEFORE_SERVER)
            {
                GameObject gameObject = player.gameObject;
                Charger componentInChildren = gameObject.GetComponentInChildren<Charger>();
                if (shot.HasTargets)
                {
                    Transform targetTransform = shot.Targets[0].TargetTransform;
                    if (!((UnityEngine.Object)targetTransform == (UnityEngine.Object)null) && (bool)componentInChildren)
                    {
                        componentInChildren.fire(targetTransform);
                    }
                }
            }
        }
    }

    public void blizzRailEffect(Shot shot, CombatPlayer player, ShotEffectType shotEffectType)
    {
        GameObject gameObject = null;
        if (!((UnityEngine.Object)player == (UnityEngine.Object)null))
        {
            gameObject = player.gameObject;
            SoundManager.Instance.Play(player.Audio, "rail+lightning", AudioPlayMode.Play);
            if (shotEffectType == ShotEffectType.ME_AFTER_SERVER && shot.HasTargets)
            {
                List<ShotTarget>.Enumerator enumerator = shot.Targets.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        ShotTarget current = enumerator.Current;
                        Transform targetTransform = current.TargetTransform;
                        if (!((UnityEngine.Object)targetTransform == (UnityEngine.Object)null))
                        {
                            GameObject gameObject2 = UnityEngine.Object.Instantiate(this.blizzRailArcPrefab);
                            gameObject2.transform.position = shot.Origin;
                            MomentLightning componentInChildren = gameObject2.GetComponentInChildren<MomentLightning>();
                            componentInChildren.target = targetTransform;
                        }
                    }
                }
                finally
                {
                    ((IDisposable)enumerator).Dispose();
                }
            }
            if (shotEffectType != ShotEffectType.ME_AFTER_SERVER)
            {
                GameObject gameObject3 = UnityEngine.Object.Instantiate(this.blizzRailSparkPrefab);
                gameObject3.transform.position = shot.Origin;
                if (shot.Direction != Vector3.zero)
                {
                    gameObject3.transform.rotation = Quaternion.LookRotation(shot.Direction);
                }
                GameObject gameObject2 = UnityEngine.Object.Instantiate(this.blizzRailPrefab);
                float num = Vector3.Distance(shot.Origin, gameObject.transform.position);
                gameObject2.transform.position = gameObject.transform.position;
                gameObject2.transform.LookAt(shot.Origin);
                gameObject2.transform.localScale = new Vector3(3f, 3f, num / 11f);
                gameObject2 = UnityEngine.Object.Instantiate(this.blizzRailArcPrefab);
                gameObject2.transform.position = gameObject.transform.position;
                MomentLightning componentInChildren = gameObject2.GetComponentInChildren<MomentLightning>();
                componentInChildren.target = gameObject3.transform;
            }
        }
    }

    public void blizzChainEffect(Shot shot, CombatPlayer player, ShotEffectType shotEffectType)
    {
        GameObject gameObject = null;
        Vector3 position = shot.Origin;
        bool flag = false;
        Transform transform = null;
        if (!((UnityEngine.Object)player == (UnityEngine.Object)null))
        {
            gameObject = player.gameObject;
            SoundManager.Instance.Play(player.Audio, "rail+lightning", AudioPlayMode.Play);
            if (shotEffectType == ShotEffectType.ME_AFTER_SERVER && shot.HasTargets)
            {
                if (!shot.Targets[0].Direct)
                {
                    flag = true;
                }
                int num = 150;
                List<ShotTarget>.Enumerator enumerator = shot.Targets.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        ShotTarget current = enumerator.Current;
                        Transform targetTransform = current.TargetTransform;
                        if (!((UnityEngine.Object)targetTransform == (UnityEngine.Object)null))
                        {
                            if (flag)
                            {
                                GameObject gameObject2 = UnityEngine.Object.Instantiate(this.blizzChainArcPrefab);
                                gameObject2.transform.position = position;
                                MomentChainLightning componentInChildren = gameObject2.GetComponentInChildren<MomentChainLightning>();
                                componentInChildren.target = targetTransform;
                                if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
                                {
                                    componentInChildren.origin = transform;
                                }
                                componentInChildren.Launch(num, this.blizzChainSparkPrefab);
                            }
                            position = targetTransform.position;
                            flag = true;
                            transform = targetTransform;
                            num += 150;
                        }
                    }
                }
                finally
                {
                    ((IDisposable)enumerator).Dispose();
                }
            }
            if (shotEffectType != ShotEffectType.ME_AFTER_SERVER)
            {
                GameObject gameObject3 = UnityEngine.Object.Instantiate(this.blizzChainSparkPrefab);
                gameObject3.transform.position = shot.Origin;
                if (shot.Direction != Vector3.zero)
                {
                    gameObject3.transform.rotation = Quaternion.LookRotation(shot.Direction);
                }
                GameObject gameObject2 = UnityEngine.Object.Instantiate(this.blizzRailPrefab);
                float num2 = Vector3.Distance(shot.Origin, gameObject.transform.position);
                gameObject2.transform.position = gameObject.transform.position;
                gameObject2.transform.LookAt(shot.Origin);
                gameObject2.transform.localScale = new Vector3(3f, 3f, num2 / 11f);
                gameObject2 = UnityEngine.Object.Instantiate(this.blizzChainArcPrefab);
                gameObject2.transform.position = gameObject.transform.position;
                MomentChainLightning componentInChildren = gameObject2.GetComponentInChildren<MomentChainLightning>();
                componentInChildren.target = gameObject3.transform;
                componentInChildren.Launch(0L, null);
            }
        }
    }

    public void mineChainEffect(Shot shot, CombatPlayer player, CombatPlayer localPlayer, bool isMe)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate(this.electricExploPrefab);
        gameObject.transform.position = shot.Origin;
        SoundManager.Instance.Play(gameObject.GetComponent<AudioSource>(), "rocket explosion 2", AudioPlayMode.Play);
        if (shot.Direction != Vector3.zero)
        {
            gameObject.transform.rotation = Quaternion.LookRotation(shot.Direction);
        }
        if (!((UnityEngine.Object)localPlayer == (UnityEngine.Object)null))
        {
            float num = Vector3.Distance(localPlayer.transform.position, gameObject.transform.position);
            if (num < 30f)
            {
                localPlayer.WalkController.setExplosionForce(gameObject.transform.position, (30f - num) * 1.5f);
            }
            player.UnregisterItem(shot.TimeStamp);
            GameObject gameObject2 = null;
            Vector3 position = shot.Origin;
            bool flag = false;
            Transform transform = null;
            if (!((UnityEngine.Object)player == (UnityEngine.Object)null))
            {
                gameObject2 = player.gameObject;
                SoundManager.Instance.Play(player.Audio, "rail+lightning", AudioPlayMode.Play);
                if (shot.HasTargets)
                {
                    if (!shot.Targets[0].Direct)
                    {
                        flag = true;
                    }
                    int num2 = 150;
                    List<ShotTarget>.Enumerator enumerator = shot.Targets.GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            ShotTarget current = enumerator.Current;
                            Transform targetTransform = current.TargetTransform;
                            if (!((UnityEngine.Object)targetTransform == (UnityEngine.Object)null))
                            {
                                if (flag)
                                {
                                    GameObject gameObject3 = UnityEngine.Object.Instantiate(this.blizzChainArcPrefab);
                                    gameObject3.transform.position = position;
                                    MomentChainLightning componentInChildren = gameObject3.GetComponentInChildren<MomentChainLightning>();
                                    componentInChildren.target = targetTransform;
                                    if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
                                    {
                                        componentInChildren.origin = transform;
                                    }
                                    componentInChildren.Launch(num2, this.blizzChainSparkPrefab);
                                }
                                position = targetTransform.position;
                                flag = true;
                                transform = targetTransform;
                                num2 += 150;
                            }
                        }
                    }
                    finally
                    {
                        ((IDisposable)enumerator).Dispose();
                    }
                }
            }
        }
    }

    public bool ImpactEffect(ImpactType impact, CombatPlayer player, CombatPlayer targetPlayer)
    {
        switch (impact)
        {
            case ImpactType.Blood:
            {
                GameObject gameObject = UnityEngine.Object.Instantiate(this.healthHitPrefab);
                gameObject.transform.position = targetPlayer.transform.position;
                gameObject.transform.parent = targetPlayer.transform;
                Transform transform = gameObject.transform;
                transform.localPosition += new Vector3(0f, 3f, 0f);
                gameObject = UnityEngine.Object.Instantiate(this.healthHitPrefab);
                gameObject.transform.position = targetPlayer.transform.position;
                gameObject.transform.parent = targetPlayer.transform;
                Transform transform2 = gameObject.transform;
                transform2.localPosition += new Vector3(0f, 5f, 0f);
                gameObject = UnityEngine.Object.Instantiate(this.healthHitPrefab);
                gameObject.transform.position = targetPlayer.transform.position;
                gameObject.transform.parent = targetPlayer.transform;
                Transform transform3 = gameObject.transform;
                transform3.localPosition += new Vector3(0f, 7f, 0f);
                if ((UnityEngine.Object)targetPlayer == (UnityEngine.Object)PlayerManager.Instance.LocalPlayer)
                {
                    GameHUD.Instance.ShowImpact(BattleGUIImpact.GUIImpact.Blood, 2f);
                }
                break;
            }
            case ImpactType.Fire:
                if ((UnityEngine.Object)targetPlayer == (UnityEngine.Object)PlayerManager.Instance.LocalPlayer)
                {
                    GameHUD.Instance.ShowImpact(BattleGUIImpact.GUIImpact.Fire, 2f);
                }
                targetPlayer.Flame.Material = targetPlayer.Flame.Fire;
                targetPlayer.Flame.fire(true);
                break;
            case ImpactType.Poison:
                if ((UnityEngine.Object)targetPlayer == (UnityEngine.Object)PlayerManager.Instance.LocalPlayer)
                {
                    GameHUD.Instance.ShowImpact(BattleGUIImpact.GUIImpact.Poison, 2f);
                }
                targetPlayer.Flame.Material = targetPlayer.Flame.Acid;
                targetPlayer.Flame.fire(true);
                break;
            case ImpactType.Frost:
                targetPlayer.PlayerCustomisator.SetMaterial(targetPlayer.Flame.FrostMaterial);
                if ((UnityEngine.Object)targetPlayer == (UnityEngine.Object)PlayerManager.Instance.LocalPlayer)
                {
                    GameHUD.Instance.ShowImpact(BattleGUIImpact.GUIImpact.Frost, 2f);
                }
                targetPlayer.Flame.Material = targetPlayer.Flame.Frost;
                targetPlayer.Flame.fire(true);
                break;
            case ImpactType.Stunning:
                if ((UnityEngine.Object)targetPlayer == (UnityEngine.Object)PlayerManager.Instance.LocalPlayer)
                {
                    GameHUD.Instance.ShowImpact(BattleGUIImpact.GUIImpact.Stunning, 2f);
                }
                targetPlayer.Flame.Material = null;
                targetPlayer.Flame.fire(true);
                break;
        }
        return true;
    }

    public bool HitEffect(Shot shot, CombatPlayer player, CombatPlayer targetPlayer, int energy, int health)
    {
        Vector3 vector = targetPlayer.transform.position - shot.Origin;
        GameObject gameObject = UnityEngine.Object.Instantiate(this.healthHitPrefab);
        ParticleRenderer[] componentsInChildren2 = gameObject.GetComponentsInChildren<ParticleRenderer>();
        componentsInChildren2[0].material.shader = Shader.Find("Particles/Multiply");
        gameObject.transform.position = shot.Origin;
        gameObject.transform.parent = targetPlayer.transform;
        SoundManager.Instance.Play(targetPlayer.Audio, "Bat_Shot_Enemy", AudioPlayMode.Play);
        CombatWeapon combatWeapon = null;
        switch (shot.WeaponType)
        {
            case WeaponType.ONE_HANDED_COLD_ARMS:
            case WeaponType.TWO_HANDED_COLD_ARMS:
            case WeaponType.HAND_GUN:
            case WeaponType.MACHINE_GUN:
            case WeaponType.FLAMER:
            case WeaponType.ROCKET_LAUNCHER:
            case WeaponType.GRENADE_LAUNCHER:
            case WeaponType.SNIPER_RIFLE:
            case WeaponType.SNOW_GUN:
            case WeaponType.ACID_THROWER:
                combatWeapon = null;
                if ((UnityEngine.Object)player.WeaponController != (UnityEngine.Object)null)
                {
                    combatWeapon = player.WeaponController.GetWeaponByType((int)shot.WeaponType);
                }
                else if ((UnityEngine.Object)player.ShotController != (UnityEngine.Object)null)
                {
                    combatWeapon = player.ShotController.GetWeaponByType((int)shot.WeaponType);
                }
                if (combatWeapon != null)
                {
                    break;
                }
                return true;
        }
        return true;
    }
}


