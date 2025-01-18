// ILSpyBased#2
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public GameObject lifeMeterFill;

    public Transform leftTransform;

    public Transform rightTransform;

    private int axis;

    private float leftPos;

    private float rightPos;

    private Transform thisTransform;

    private Transform fillTransform;

    private bool isEnabled = true;

    private void Awake()
    {
        this.fillTransform = this.lifeMeterFill.transform;
        this.leftPos = this.leftTransform.localPosition[this.axis];
        this.rightPos = this.rightTransform.localPosition[this.axis];
    }

    public void SetValue(float f)
    {
        if (this.isEnabled)
        {
            f = Mathf.Clamp(f, 0f, 1f);
            Vector3 localPosition = this.fillTransform.localPosition;
            localPosition[this.axis] = Mathf.Lerp(this.leftPos, this.rightPos, f) + (1f - f) / 2f;
            this.fillTransform.localPosition = localPosition;
            Vector3 localScale = this.fillTransform.localScale;
            localScale[this.axis] = f;
            this.fillTransform.localScale = localScale;
        }
    }

    public void Enable()
    {
        this.isEnabled = true;
        base.gameObject.SetActiveRecursively(true);
    }

    public void Disable()
    {
        this.isEnabled = false;
        base.gameObject.SetActiveRecursively(false);
    }
}


