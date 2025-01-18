// ILSpyBased#2
using UnityEngine;

public class BattleGUICamera : MonoBehaviour
{
    private Transform health;

    private Vector3 healthPos = new Vector3(60f, 53f, 1f);

    private Transform armor;

    private Vector3 armorPos = new Vector3(197f, 53f, 1f);

    private Transform ammo;

    private Vector3 ammoPos = new Vector3(10f, 53f, 1f);

    private Vector2 previosResolution = new Vector2(0f, 0f);

    private Transform impact;

    private Vector3 impactPos = new Vector3(10f, 125f, 1f);

    private Transform overlay;

    public Vector3 overlayPos = new Vector3(1f, 1f, 1f);

    private Transform killstreak;

    public Vector3 killstreakPos = new Vector3(-17f, 558f, 1f);

    private void Start()
    {
        this.health = base.transform.parent.FindChild("Health");
        this.armor = base.transform.parent.FindChild("Armor");
        this.ammo = base.transform.parent.FindChild("Ammo");
        this.impact = base.transform.parent.FindChild("Impact");
        this.overlay = base.transform.parent.FindChild("Overlay");
        this.killstreak = base.transform.parent.FindChild("KillStreak");
    }

    private void Update()
    {
        if (this.previosResolution.x == (float)Screen.width && this.previosResolution.y == (float)Screen.height && 1 == 0)
        {
            return;
        }
        base.GetComponent<Camera>().orthographicSize = (float)base.GetComponent<Camera>().pixelHeight;
        this.ammoPos.x = (float)Screen.width - 77f;
        this.impactPos.x = (float)Screen.width - 40f;
        this.killstreakPos.x = (float)Screen.width / 2f - 40f;
        this.killstreakPos.y = (float)Screen.height - 120f;
        this.health.position = base.GetComponent<Camera>().ScreenToWorldPoint(this.healthPos);
        this.armor.position = base.GetComponent<Camera>().ScreenToWorldPoint(this.armorPos);
        this.ammo.position = base.GetComponent<Camera>().ScreenToWorldPoint(this.ammoPos);
        this.impact.position = base.GetComponent<Camera>().ScreenToWorldPoint(this.impactPos);
        this.killstreak.position = base.GetComponent<Camera>().ScreenToWorldPoint(this.killstreakPos);
        this.overlayPos.x = (float)Screen.width * 0.5f;
        this.overlayPos.y = (float)Screen.height * 0.5f;
        this.overlay.position = base.GetComponent<Camera>().ScreenToWorldPoint(this.overlayPos);
        Transform transform = this.overlay;
        float x = (float)Screen.width;
        Vector3 localScale = this.overlay.localScale;
        transform.localScale = new Vector3(x, localScale.y, (float)Screen.height);
        this.previosResolution.x = (float)Screen.width;
        this.previosResolution.y = (float)Screen.height;
    }
}


