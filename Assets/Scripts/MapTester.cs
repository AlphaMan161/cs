// ILSpyBased#2
using UnityEngine;

public class MapTester : MonoBehaviour
{
    private BaseEnterTrigger[] triggers;

    private GameObject lastUserGroup;

    public int selectIndex = -1;

    private void Start()
    {
        this.ShowCheatPositions();
    }

    private void FixedUpdate()
    {
    }

    private Color ClassifyCount(int count)
    {
        if (count >= 100)
        {
            return new Color(1f, 0f, 0f, 1f);
        }
        if (count >= 50)
        {
            return new Color(0f, 1f, 0f, 1f);
        }
        return new Color(0f, 0f, 1f, 1f);
    }

    private void RegisterPosition(string name, Vector3 position, int count)
    {
        if ((Object)this.lastUserGroup == (Object)null || this.lastUserGroup.name != name)
        {
            this.lastUserGroup = new GameObject(name);
        }
        GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gameObject.transform.position = position;
        gameObject.GetComponent<Renderer>().material.color = this.ClassifyCount(count);
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.transform.parent = this.lastUserGroup.transform;
    }

    private void ShowCheatPositions()
    {
        this.RegisterPosition("u8618590[6]", new Vector3(67.78063f, 14.938448f, 143.902008f), 2);
        this.RegisterPosition("u8618590[6]", new Vector3(173.481476f, -4.562127f, 188.304367f), 2);
        this.RegisterPosition("u8618590[6]", new Vector3(3.80066681f, 34.0643234f, 182.492767f), 2);
        this.RegisterPosition("u8618590[6]", new Vector3(92.80877f, 13.5497484f, 143.40863f), 2);
        this.RegisterPosition("u8618590[6]", new Vector3(-60.2100945f, -70.76778f, 299.428864f), 2);
        this.RegisterPosition("u8618590[6]", new Vector3(174.103912f, -7.812112f, 189.777222f), 2);
        this.RegisterPosition("u8618590[6]", new Vector3(67.83633f, 11.9017935f, 136.838043f), 2);
    }

    private void InitColliders()
    {
        this.triggers = (UnityEngine.Object.FindSceneObjectsOfType(typeof(BaseEnterTrigger)) as BaseEnterTrigger[]);
        if (this.triggers != null)
        {
            BaseEnterTrigger[] array = this.triggers;
            foreach (BaseEnterTrigger baseEnterTrigger in array)
            {
                this.RegisterPosition("smertipod", ((Component)baseEnterTrigger).GetComponent<Renderer>().bounds.max, 100);
                this.RegisterPosition("smertipod", ((Component)baseEnterTrigger).GetComponent<Renderer>().bounds.min, 10);
            }
        }
    }

    private void CheckColliders()
    {
        if (this.triggers != null)
        {
            BaseEnterTrigger[] array = this.triggers;
            foreach (BaseEnterTrigger baseEnterTrigger in array)
            {
                if ((Object)baseEnterTrigger == (Object)null)
                {
                    UnityEngine.Debug.LogError("Trigger Destroyed");
                }
                if (!baseEnterTrigger.enabled)
                {
                    UnityEngine.Debug.LogError("Trigger Disabled");
                }
                if (!baseEnterTrigger.gameObject.activeSelf)
                {
                    UnityEngine.Debug.LogError("Trigger GameObject Disabled");
                }
                if (!((Component)baseEnterTrigger).GetComponent<Collider>().enabled)
                {
                    UnityEngine.Debug.LogError("Trigger Collider Disabled");
                }
                if (!((Component)baseEnterTrigger).GetComponent<Collider>().enabled)
                {
                    UnityEngine.Debug.LogError("Collider Disabled");
                }
            }
        }
    }

    private void SelectCollider(int index)
    {
        if (this.triggers != null)
        {
            int num = 0;
            BaseEnterTrigger[] array = this.triggers;
            foreach (BaseEnterTrigger baseEnterTrigger in array)
            {
                num++;
                if (num == index)
                {
                    baseEnterTrigger.gameObject.SetActive(true);
                    ((Component)baseEnterTrigger).GetComponent<Renderer>().enabled = true;
                }
                else
                {
                    baseEnterTrigger.gameObject.SetActive(false);
                }
            }
        }
    }
}


