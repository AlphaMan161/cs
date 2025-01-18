// ILSpyBased#2
using System;
using UnityEngine;

public class RigidBodyForce : MonoBehaviour
{
    public const long SleepDelayPeriod = 20000000L;

    private new Rigidbody rigidbody;

    public Vector3 Vector = new Vector3(0f, -100f, 0f);

    private Vector3 lastPosition;

    public Vector3 ImpulseVector = Vector3.zero;

    public float SleepSpeed = 0.0001f;

    public Rigidbody rootBody;

    public long lastKinematic = DateTime.Now.Ticks;

    private void Start()
    {
        this.rigidbody = base.GetComponent<Rigidbody>();
        CharacterJoint component = base.GetComponent<CharacterJoint>();
        Transform transform = base.transform;
        while (transform.name != "Bip01")
        {
            transform = transform.parent;
        }
        this.rootBody = ((Component)transform).GetComponent<Rigidbody>();
        base.enabled = true;
    }

    private void FixedUpdate()
    {
        if (this.rigidbody.isKinematic)
        {
            this.lastKinematic = DateTime.Now.Ticks;
        }
        else
        {
            if ((UnityEngine.Object)this.rigidbody != (UnityEngine.Object)null && !this.rigidbody.isKinematic)
            {
                this.rigidbody.AddForce(this.Vector + this.ImpulseVector);
                this.ImpulseVector = Vector3.zero;
            }
            if (this.lastKinematic + 20000000 <= DateTime.Now.Ticks)
            {
                float sqrMagnitude = (this.lastPosition - base.transform.position).sqrMagnitude;
                if (sqrMagnitude < this.SleepSpeed && ((UnityEngine.Object)this.rigidbody == (UnityEngine.Object)this.rootBody || this.rootBody.isKinematic))
                {
                    this.rigidbody.isKinematic = true;
                }
                this.lastPosition = base.transform.position;
            }
        }
    }
}


