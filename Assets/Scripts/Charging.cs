// ILSpyBased#2
using UnityEngine;

public class Charging : MonoBehaviour
{
    private long tm;

    public Transform target;

    public int zigs = 10;

    private int tzigs = 100;

    public float speed = 1f;

    public float scale = 1f;

    public Light startLight;

    public Light endLight;

    private Perlin noise;

    private float oneOverZigs;

    private Particle[] particles;

    private void Start()
    {
        this.oneOverZigs = 1f / (float)this.zigs;
        base.GetComponent<ParticleEmitter>().emit = false;
        base.GetComponent<ParticleEmitter>().Emit(this.zigs);
        this.particles = base.GetComponent<ParticleEmitter>().particles;
        this.tzigs = this.zigs;
        base.GetComponent<Renderer>().enabled = false;
    }

    public void turn(bool on)
    {
        if ((bool)this.startLight)
        {
            this.startLight.enabled = on;
        }
        if ((bool)this.endLight)
        {
            this.endLight.enabled = on;
        }
        base.GetComponent<Renderer>().enabled = on;
    }

    public void fire(Transform target)
    {
        this.target = target;
        if ((Object)target == (Object)null)
        {
            if ((bool)this.startLight)
            {
                this.startLight.enabled = false;
            }
            if ((bool)this.endLight)
            {
                this.endLight.enabled = false;
            }
            base.GetComponent<ParticleEmitter>().enabled = false;
            base.GetComponent<ParticleEmitter>().ClearParticles();
        }
    }

    private void doCharging()
    {
        float num = Vector3.Distance(base.transform.position, this.target.position);
        this.zigs = (int)(30f * num / 10f);
        if (this.zigs != this.tzigs)
        {
            this.oneOverZigs = 1f / (float)this.zigs;
            base.GetComponent<ParticleEmitter>().emit = false;
            base.GetComponent<ParticleEmitter>().ClearParticles();
            base.GetComponent<ParticleEmitter>().Emit(this.zigs);
            this.particles = base.GetComponent<ParticleEmitter>().particles;
            this.tzigs = this.zigs;
        }
        if (this.noise == null)
        {
            this.noise = new Perlin();
        }
        float num2 = Time.time * this.speed * 0.1365143f;
        float num3 = Time.time * this.speed * 1.21688f;
        float num4 = Time.time * this.speed * 2.5564f;
        int num5 = (int)(Time.time * this.speed * 1.21688f / 3f);
        int num6 = (int)(Time.time * this.speed * 2.5564f / 3f);
        for (int i = 0; i < this.particles.Length; i++)
        {
            Vector3 a = Vector3.Lerp(base.transform.position, this.target.position, this.oneOverZigs * (float)i);
            Vector3 a2 = new Vector3(0f, this.noise.Noise((float)num5 + a.x / 5f, (float)num5, (float)num5), this.noise.Noise((float)num6 + a.x / 5f, (float)num6, (float)num6));
            Vector3 a3 = new Vector3(this.noise.Noise(num2 + a.x / 5f, num2 + a.y, num2 + a.z), this.noise.Noise(num3 + a.x, num3 + a.y, num3 + a.z), this.noise.Noise(num4 + a.x, num4 + a.y, num4 + a.z));
            float num7 = (float)i / (float)this.particles.Length;
            if (num7 > 0.5f)
            {
                num7 = (float)(this.particles.Length - i) / (float)this.particles.Length;
            }
            a += a3 * this.scale * num7 + a2 * num7 * 5f;
            this.particles[i].position = a;
            this.particles[i].color = Color.white;
            this.particles[i].energy = 1f;
        }
        base.GetComponent<ParticleEmitter>().particles = this.particles;
        if (base.GetComponent<ParticleEmitter>().particleCount >= 2)
        {
            if ((bool)this.startLight)
            {
                this.startLight.transform.position = this.particles[0].position;
            }
            if ((bool)this.endLight)
            {
                this.endLight.transform.position = this.particles[this.particles.Length - 1].position;
            }
        }
    }

    private void LateUpdate()
    {
        if ((Object)this.target != (Object)null)
        {
            base.GetComponent<Renderer>().enabled = true;
            this.doCharging();
        }
        else
        {
            base.GetComponent<Renderer>().enabled = false;
        }
    }
}


