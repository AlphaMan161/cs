// ILSpyBased#2
using UnityEngine;

public class BattleGUI : MonoBehaviour
{
    private KillStreak killStreak;

    private Transform health;

    private TextMesh healthText;

    private Transform armor;

    private TextMesh armorText;

    private Transform ammo;

    private TextMesh ammoText;

    private TextMesh ammo2Text;

    private BattleStatsProgress healthStats;

    private BattleStatsProgress armorStats;

    private BattleStatsProgress ammoStats;

    private Renderer zombieOverlayRender;

    private Renderer zoomOverlayRender;

    private Renderer bloodOverlayRender;

    private BattleGUIImpact impact;

    private Camera BattleGUICamera;

    private bool isInit;

    private short team_num;

    private int prev_health = -1;

    private int prev_maxHealth = -1;

    private int prev_armor = -1;

    private int prev_maxArmor = -1;

    private int prev_ammo = -1;

    private int prev_maxAmmo = -1;

    private int prev_ammoReserve = -1;

    private bool showAmmo = true;

    private void Start()
    {
        this.Init();
    }

    private void Update()
    {
    }

    private void Init()
    {
        if (!this.isInit)
        {
            this.health = base.transform.FindChild("Health");
            this.armor = base.transform.FindChild("Armor");
            this.ammo = base.transform.FindChild("Ammo");
            this.impact = ((Component)base.transform.FindChild("Impact")).GetComponent<BattleGUIImpact>();
            this.killStreak = ((Component)base.transform.FindChild("KillStreak")).GetComponentInChildren<KillStreak>();
            this.zombieOverlayRender = ((Component)base.transform.FindChild("Overlay").FindChild("Zombie")).GetComponent<MeshRenderer>();
            this.bloodOverlayRender = ((Component)base.transform.FindChild("Overlay").FindChild("Blood")).GetComponent<MeshRenderer>();
            this.zoomOverlayRender = ((Component)base.transform.FindChild("Zoom").FindChild("ZoomOverlay")).GetComponent<MeshRenderer>();
            this.zoomOverlayRender.enabled = false;
            this.healthText = ((Component)this.health.transform.FindChild("Text")).GetComponent<TextMesh>();
            this.armorText = ((Component)this.armor.transform.FindChild("Text")).GetComponent<TextMesh>();
            this.ammoText = ((Component)this.ammo.transform.FindChild("Text")).GetComponent<TextMesh>();
            this.ammo2Text = ((Component)this.ammo.transform.FindChild("Text2")).GetComponent<TextMesh>();
            this.isInit = true;
            this.BattleGUICamera = ((Component)base.GetComponentInChildren<BattleGUICamera>()).GetComponent<Camera>();
			this.bloodOverlayRender.material.shader = Shader.Find ("Particles/Additive");
        }
    }

    private void OnDisable()
    {
        if ((Object)this.BattleGUICamera != (Object)null)
        {
            this.BattleGUICamera.enabled = false;
        }
    }

    private void OnEnable()
    {
        if ((Object)this.BattleGUICamera != (Object)null)
        {
            this.BattleGUICamera.enabled = true;
        }
    }

    public void SetZoom(bool on)
    {
        if (this.zoomOverlayRender.enabled != on)
        {
            this.zoomOverlayRender.transform.localScale = new Vector3(0.483516484f * this.BattleGUICamera.orthographicSize, 0.461538464f * this.BattleGUICamera.orthographicSize, 0f);
            this.zoomOverlayRender.enabled = on;
        }
    }

    public void SetBloodOverlay(float amount)
    {
        if (amount > 0f)
        {
            if (!this.bloodOverlayRender.enabled)
            {
                this.bloodOverlayRender.enabled = true;
            }
            this.bloodOverlayRender.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, amount));
        }
        else if (this.bloodOverlayRender.enabled)
        {
            this.bloodOverlayRender.enabled = false;
        }
    }

    public void SetTeam(short teamNum, MapMode.MODE gameMode)
    {
        this.Init();
        this.team_num = teamNum;
        ((Component)this.health.transform.FindChild("Blue")).GetComponent<MeshRenderer>().enabled = false;
        ((Component)this.health.transform.FindChild("Green")).GetComponent<MeshRenderer>().enabled = false;
        ((Component)this.health.transform.FindChild("Red")).GetComponent<MeshRenderer>().enabled = false;
        ((Component)this.health.transform.FindChild("Zombie")).GetComponent<MeshRenderer>().enabled = false;
        ((Component)this.health.transform.FindChild("Background")).GetComponent<MeshRenderer>().enabled = false;
        ((Component)this.armor.transform.FindChild("Blue")).GetComponent<MeshRenderer>().enabled = false;
        ((Component)this.armor.transform.FindChild("Green")).GetComponent<MeshRenderer>().enabled = false;
        ((Component)this.armor.transform.FindChild("Red")).GetComponent<MeshRenderer>().enabled = false;
        ((Component)this.armor.transform.FindChild("Background")).GetComponent<MeshRenderer>().enabled = false;
        ((Component)this.armor.transform.FindChild("Text")).GetComponent<MeshRenderer>().enabled = false;
        ((Component)this.ammo.transform.FindChild("Blue")).GetComponent<MeshRenderer>().enabled = false;
        ((Component)this.ammo.transform.FindChild("Green")).GetComponent<MeshRenderer>().enabled = false;
        ((Component)this.ammo.transform.FindChild("Red")).GetComponent<MeshRenderer>().enabled = false;
        this.killStreak.gameObject.SetActive(false);
        this.zombieOverlayRender.enabled = false;
        if (gameMode == MapMode.MODE.ZOMBIE && teamNum == 1)
        {
            ((Component)this.health.transform.FindChild("Zombie")).GetComponent<MeshRenderer>().enabled = true;
            this.zombieOverlayRender.enabled = true;
        }
        else
        {
            ((Component)this.health.transform.FindChild("Background")).GetComponent<MeshRenderer>().enabled = true;
            ((Component)this.armor.transform.FindChild("Background")).GetComponent<MeshRenderer>().enabled = true;
            ((Component)this.armor.transform.FindChild("Text")).GetComponent<MeshRenderer>().enabled = true;
            switch (teamNum)
            {
                case 1:
                    ((Component)this.health.transform.FindChild("Red")).GetComponent<MeshRenderer>().enabled = true;
                    ((Component)this.armor.transform.FindChild("Red")).GetComponent<MeshRenderer>().enabled = true;
                    ((Component)this.ammo.transform.FindChild("Red")).GetComponent<MeshRenderer>().enabled = true;
                    this.healthStats = ((Component)this.health.transform.FindChild("Red")).GetComponent<BattleStatsProgress>();
                    this.armorStats = ((Component)this.armor.transform.FindChild("Red")).GetComponent<BattleStatsProgress>();
                    this.ammoStats = ((Component)this.ammo.transform.FindChild("Red")).GetComponent<BattleStatsProgress>();
                    break;
                case 2:
                    ((Component)this.health.transform.FindChild("Blue")).GetComponent<MeshRenderer>().enabled = true;
                    ((Component)this.armor.transform.FindChild("Blue")).GetComponent<MeshRenderer>().enabled = true;
                    ((Component)this.ammo.transform.FindChild("Blue")).GetComponent<MeshRenderer>().enabled = true;
                    this.healthStats = ((Component)this.health.transform.FindChild("Blue")).GetComponent<BattleStatsProgress>();
                    this.armorStats = ((Component)this.armor.transform.FindChild("Blue")).GetComponent<BattleStatsProgress>();
                    this.ammoStats = ((Component)this.ammo.transform.FindChild("Blue")).GetComponent<BattleStatsProgress>();
                    break;
                default:
                    ((Component)this.health.transform.FindChild("Green")).GetComponent<MeshRenderer>().enabled = true;
                    ((Component)this.armor.transform.FindChild("Green")).GetComponent<MeshRenderer>().enabled = true;
                    ((Component)this.ammo.transform.FindChild("Green")).GetComponent<MeshRenderer>().enabled = true;
                    this.healthStats = ((Component)this.health.transform.FindChild("Green")).GetComponent<BattleStatsProgress>();
                    this.armorStats = ((Component)this.armor.transform.FindChild("Green")).GetComponent<BattleStatsProgress>();
                    this.ammoStats = ((Component)this.ammo.transform.FindChild("Green")).GetComponent<BattleStatsProgress>();
                    break;
            }
        }
    }

    public void SetHealth(int health, int maxHealth)
    {
        this.Init();
        if (this.prev_health == health && this.prev_maxHealth == maxHealth)
        {
            return;
        }
        this.prev_health = health;
        this.prev_maxHealth = maxHealth;
        this.healthText.text = health.ToString();
        if (maxHealth < health)
        {
            maxHealth = health;
        }
        if ((Object)this.healthStats != (Object)null)
        {
            this.healthStats.SetValue(1f - 100f / (float)maxHealth * (float)health * 0.01f);
        }
    }

    public void SetArmor(int armor, int maxArmor)
    {
        this.Init();
        if (this.prev_armor == armor && this.prev_maxArmor == maxArmor)
        {
            return;
        }
        this.prev_armor = armor;
        this.prev_maxArmor = maxArmor;
        if (maxArmor < armor)
        {
            maxArmor = armor;
        }
        this.armorText.text = armor.ToString();
        if ((Object)this.armorStats != (Object)null)
        {
            this.armorStats.SetValue(1f - 100f / (float)maxArmor * (float)armor * 0.01f);
        }
    }

    public void SetAmmo(int ammo_val, int maxAmmo, int ammoReserve)
    {
        this.Init();
        if (this.prev_ammo == ammo_val && this.prev_maxAmmo == maxAmmo && this.prev_ammoReserve == ammoReserve)
        {
            return;
        }
        this.prev_ammo = ammo_val;
        this.prev_maxAmmo = maxAmmo;
        this.prev_ammoReserve = ammoReserve;
        if (ammo_val == maxAmmo && maxAmmo == ammoReserve && ammoReserve == 0 && this.showAmmo)
        {
            this.showAmmo = false;
            this.Hide(this.ammo);
        }
        else
        {
            if (!this.showAmmo)
            {
                this.showAmmo = true;
                this.Show(this.ammo);
            }
            this.ammoText.text = ammo_val.ToString();
            this.ammo2Text.text = ammoReserve.ToString();
            if (maxAmmo == 0 && ammo_val == 0)
            {
                ammo_val = 1; maxAmmo = (ammo_val );
            }
            if ((Object)this.ammoStats != (Object)null)
            {
                this.ammoStats.SetValue(1f - 100f / (float)maxAmmo * (float)ammo_val * 0.01f);
            }
        }
    }

    private void Hide(Transform obj)
    {
        obj.gameObject.SetActive(false);
    }

    public void Show(Transform obj)
    {
        obj.gameObject.SetActive(true);
    }

    public void ShowImpact(BattleGUIImpact.GUIImpact currImpact)
    {
        this.impact.Show(currImpact, -0f);
    }

    public void ShowImpact(BattleGUIImpact.GUIImpact currImpact, float offset)
    {
        this.impact.Show(currImpact, offset);
    }

    public void HideState(BattleGUIImpact.GUIImpact currImpact)
    {
        this.impact.Hide(currImpact);
    }

    public void SetKillStreak(string text, bool gold)
    {
        this.killStreak.gameObject.SetActive(true);
        this.killStreak.Show(text, gold);
    }
}


