// ILSpyBased#2
using UnityEngine;

public class KillStreak : MonoBehaviour
{
    public TextMesh Text;

    public Transform BG;

    public string TextMessage = string.Empty;

    public float BG_Size = 0.5f;

    private Vector3 targetBGSize = new Vector3(200f, 0f, 70f);

    private Vector3 targetTextSize = new Vector3(26f, 26f, 0f);

    private float FadeInSpeed = 0.15f;

    private float FadeOutSpeed = 0.5f;

    private float FadeOutTime = 1f;

    private float TimeShow = 2.5f;

    private float targetTime;

    private float Speed = 1500f;

    private float BGDelay = 0.01f;

    private bool fading;

    public float BG_Shift = 50f;

    public void Show(string text, bool gold)
    {
        this.Text.text = text;
        if (gold)
        {
            ((Component)this.Text).GetComponent<Renderer>().material.color = new Color(1f, 0.8f, 0.5f, 1f);
        }
        else
        {
            ((Component)this.Text).GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 1f);
        }
        Bounds bounds = ((Component)this.Text).GetComponent<Renderer>().bounds;
        Vector3 max = bounds.max;
        float x = max.x;
        Vector3 min = bounds.min;
        float num = x - min.x;
        this.targetBGSize = new Vector3(num * this.BG_Size + this.BG_Shift, 26f, 70f);
        this.BG.localScale = this.targetBGSize * 0.01f;
        this.Text.transform.localScale = this.targetTextSize * 0.01f;
        this.targetTime = Time.time + this.TimeShow;
        iTween.FadeTo(this.Text.gameObject, iTween.Hash("alpha", 1, "time", this.FadeInSpeed));
        iTween.FadeTo(this.BG.gameObject, iTween.Hash("alpha", 1, "delay", this.BGDelay, "time", this.FadeInSpeed));
        iTween.ScaleTo(this.Text.gameObject, iTween.Hash("x", this.targetTextSize.x, "y", this.targetTextSize.y, "z", this.targetTextSize.z, "speed", this.Speed));
        iTween.ScaleTo(this.BG.gameObject, iTween.Hash("x", this.targetBGSize.x, "y", this.targetBGSize.y, "z", this.targetBGSize.z, "delay", this.BGDelay, "speed", this.Speed));
        this.fading = false;
    }

    private void FixedUpdate()
    {
        if (Time.time > this.targetTime && !this.fading)
        {
            this.Fade();
        }
        if (Time.time > this.targetTime + this.FadeOutTime)
        {
            this.Finish();
        }
    }

    private void Fade()
    {
        this.fading = true;
        iTween.FadeTo(this.Text.gameObject, iTween.Hash("alpha", 0, "speed", this.FadeOutSpeed));
        iTween.FadeTo(this.BG.gameObject, iTween.Hash("alpha", 0, "speed", this.FadeOutSpeed));
    }

    private void Finish()
    {
        base.gameObject.SetActive(false);
    }
}


