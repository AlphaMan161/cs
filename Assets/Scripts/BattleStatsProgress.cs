// ILSpyBased#2
using UnityEngine;

public class BattleStatsProgress : MonoBehaviour
{
    private Material material;

    private void Start()
    {
        this.material = base.GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
    }

    public void SetValue(float val)
    {
        if ((Object)this.material != (Object)null)
        {
            this.material.mainTextureOffset = Vector2.Lerp(new Vector2(0f, 0f), new Vector2(0f, -1f), val);
            base.transform.localPosition = Vector3.Lerp(new Vector3(0f, 0.1f, 0f), new Vector3(0f, 0.1f, 760f), val);
        }
        else
        {
            UnityEngine.Debug.LogError("[BattleStatsProgress] material is null");
        }
    }
}


