// ILSpyBased#2
using UnityEngine;

public class FlagPoint : MonoBehaviour
{
    public GameObject flagObject;

    public int bearerID = -1;

    public int team;

    private long lastScanTime;

    private float touchDistance = 8f;

    public void Start()
    {
        this.touchDistance = 8f;
    }

    public void LateUpdate()
    {
        CombatPlayer localPlayer = PlayerManager.Instance.LocalPlayer;
        if (localPlayer.Team == this.team && this.lastScanTime < TimeManager.Instance.NetworkTime + 100)
        {
            this.lastScanTime = TimeManager.Instance.NetworkTime;
            if (Vector3.Distance(localPlayer.transform.position, base.transform.position + new Vector3(0f, 3f, 0f)) < this.touchDistance && this.bearerID != -1)
            {
                GameHUD.Instance.Message(GameHUDMessageType.EMPTY_FLAG_POINT);
            }
        }
    }
}


