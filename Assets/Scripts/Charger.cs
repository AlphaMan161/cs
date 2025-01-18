// ILSpyBased#2
using System.Collections;
using UnityEngine;

public class Charger : MonoBehaviour
{
    public Transform target;

    public float scale = 1f;

    public float speed = 1f;

    private bool on = true;

    private Charging[] lighters;

    private float turnOffAt;

    private void Start()
    {
        this.lighters = ((Component)base.transform).GetComponentsInChildren<Charging>();
        this.turn(false);
    }

    public void fire(Transform target)
    {
        if (this.lighters != null)
        {
            if (!this.on)
            {
                this.turn(true);
            }
            Charging[] array = this.lighters;
            foreach (Charging charging in array)
            {
                charging.fire(target);
            }
            if ((bool)target)
            {
                base.StartCoroutine(this.WaitAndFireOff(0.3f));
            }
        }
    }

    public void turn(bool on)
    {
        this.on = on;
        Charging[] array = this.lighters;
        foreach (Charging charging in array)
        {
            charging.turn(on);
            charging.enabled = on;
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
        if ((double)currTime2 + 0.01 >= (double)this.turnOffAt)
        {
            this.fire(null);
        }
    }
}


