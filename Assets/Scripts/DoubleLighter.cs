// ILSpyBased#2
using System.Collections;
using UnityEngine;

public class DoubleLighter : MonoBehaviour
{
    public Transform target;

    public float scale = 1f;

    public float speed = 1f;

    private DoubleLightning[] lighters;

    private float turnOffAt;

    private void Start()
    {
        this.lighters = ((Component)base.transform).GetComponentsInChildren<DoubleLightning>();
        this.turn(true);
    }

    public void fire(Transform target)
    {
        if (this.lighters != null)
        {
            DoubleLightning[] array = this.lighters;
            foreach (DoubleLightning doubleLightning in array)
            {
                doubleLightning.fire(target);
            }
            if ((bool)target)
            {
                base.StartCoroutine(this.WaitAndFireOff(0.2f));
            }
        }
    }

    public void turn(bool on)
    {
        DoubleLightning[] array = this.lighters;
        foreach (DoubleLightning doubleLightning in array)
        {
            doubleLightning.turn(on);
            doubleLightning.enabled = on;
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
            this.fire(null);
        }
    }
}


