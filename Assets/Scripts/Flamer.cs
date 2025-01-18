// ILSpyBased#2
using System.Collections;
using UnityEngine;

public class Flamer : MonoBehaviour
{
    public float scale = 1f;

    public float speed = 1f;

    private ParticleEmitter[] flamers;

    private float turnOffAt;

    public ParticleEmitter[] Flamers
    {
        get
        {
            if (this.flamers == null)
            {
                this.flamers = base.transform.GetComponentsInChildren<ParticleEmitter>(true);
            }
            return this.flamers;
        }
    }

    private void Start()
    {
        ParticleRenderer[] components = base.gameObject.GetComponentsInChildren<ParticleRenderer>();
        components[0].sharedMaterial.shader = Shader.Find("Particles/Additive");
        components[1].sharedMaterial.shader = Shader.Find("Particles/Additive");
        if (gameObject.name == "flamethrower_n1")
        {
            if (transform.parent.GetChild(1).name == "Flame")
            {
                transform.parent.GetChild(1).GetComponent<ParticleRenderer>().sharedMaterial.shader = Shader.Find("Particles/Additive");
            }
        }
        this.fire(false);
    }

    public void fire(bool on)
    {
        if (this.Flamers != null)
        {
            ParticleEmitter[] array = this.Flamers;
            foreach (ParticleEmitter particleEmitter in array)
            {
                particleEmitter.emit = on;
            }
            if (on)
            {
                base.StartCoroutine(this.WaitAndFireOff(0.3f));
            }
        }
    }

    private IEnumerator WaitAndFireOff(float waitTime)
    {
        float currTime2 = Time.time;
        if (currTime2 + waitTime > this.turnOffAt)
        {
            this.turnOffAt = currTime2 + waitTime;
        }
        yield return (object)new WaitForSeconds(waitTime);
        currTime2 = Time.time;
        if (currTime2 + 0.01f >= this.turnOffAt)
        {
            this.fire(false);
        }
    }
}


