// ILSpyBased#2
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{
    public float scale = 1f;

    public float speed = 1f;

    public Material Fire;

    public Material Frost;

    public Material Acid;

    public Material material;

    public Material FrostMaterial;
    public static Material FrostMateriall;

    private List<ParticleEmitter> flamers;

    public bool On;

    private float turnOffAt;
    public static Texture textureIce;

    public List<ParticleEmitter> Flamers
    {
        get
        {
            if (this.flamers == null)
            {
                this.flamers = new List<ParticleEmitter>();
                ParticleEmitter[] componentsInChildren = base.transform.GetComponentsInChildren<ParticleEmitter>(true);
                foreach (ParticleEmitter particleEmitter in componentsInChildren)
                {
                    if (particleEmitter.transform.parent.gameObject.name.StartsWith("Bip"))
                    {
                        this.flamers.Add(particleEmitter);
                    }
                }
            }
            return this.flamers;
        }
    }

    public Material Material
    {
        get
        {
            return this.material;
        }
        set
        {
            if (this.Flamers != null)
            {
                this.material = value;
                List<ParticleEmitter>.Enumerator enumerator = this.Flamers.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        ParticleEmitter current = enumerator.Current;
                        ((Component)current).GetComponent<Renderer>().material = value;
                    }
                }
                finally
                {
                    ((IDisposable)enumerator).Dispose();
                }
            }
        }
    }

    private void Start()
    {
		FrostMaterial.shader = Shader.Find ("Legacy Shaders/Transparent/Specular");
		Fire.shader = Shader.Find ("Particles/Additive");
		Acid.shader = Shader.Find ("Particles/Additive");
		Frost.shader = Shader.Find ("Particles/Additive");
        if (FrostMaterial.mainTexture != textureIce)
        {
            FrostMaterial.mainTexture = textureIce;
        }
        textureIce = (Texture2D)Resources.Load("Textures/Cloth/icicle_Color");
        this.fire(false);
    }

    public void fire(bool on)
    {
        if (this.Flamers != null)
        {
            if ((UnityEngine.Object)this.material != (UnityEngine.Object)null)
            {
                List<ParticleEmitter>.Enumerator enumerator = this.Flamers.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        ParticleEmitter current = enumerator.Current;
                        current.emit = on;
                    }
                }
                finally
                {
                    ((IDisposable)enumerator).Dispose();
                }
            }
            this.On = on;
            if (on)
            {
                base.StartCoroutine(this.WaitAndFireOff(2f));
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
            PlayerCustomisator customisator = ((Component)base.transform).GetComponent<PlayerCustomisator>();
            if ((UnityEngine.Object)customisator != (UnityEngine.Object)null)
            {
                customisator.RevertMaterials();
            }
        }
    }
}


