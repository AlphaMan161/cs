// ILSpyBased#2
using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleGUIImpact : MonoBehaviour
{
    public enum GUIImpact
    {
        Fire = 1,
        Blood,
        Poison,
        Electro,
        Frost,
        Biohazard,
        Stunning
    }

    private Transform state;

    private Dictionary<GUIImpact, Transform> transformList = new Dictionary<GUIImpact, Transform>();

    private Dictionary<GUIImpact, MeshRenderer> renderList = new Dictionary<GUIImpact, MeshRenderer>();

    private Dictionary<GUIImpact, Animation> animationList = new Dictionary<GUIImpact, Animation>();

    private Dictionary<GUIImpact, float> offsetDisable = new Dictionary<GUIImpact, float>();

    private Transform fire;

    private MeshRenderer fireRender;

    private Animation fireAnimator;

    private Transform blood;

    private MeshRenderer bloodRender;

    private Animation bloodAnimator;

    private Transform poison;

    private MeshRenderer poisonRender;

    private Animation poisonAnimator;

    private bool isInit;

    private void Start()
    {
        this.Init();
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        Dictionary<GUIImpact, MeshRenderer>.Enumerator enumerator = this.renderList.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                KeyValuePair<GUIImpact, MeshRenderer> current = enumerator.Current;
                if (current.Value.enabled && this.offsetDisable[current.Key] < Time.time)
                {
                    this.Hide(current.Key);
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }

    private void Init()
    {
        if (!this.isInit)
        {
            foreach (int value in Enum.GetValues(typeof(GUIImpact)))
            {
                this.transformList[(GUIImpact)value] = base.transform.FindChild(((GUIImpact)value).ToString());
                this.renderList[(GUIImpact)value] = ((Component)this.transformList[(GUIImpact)value]).GetComponent<MeshRenderer>();
                this.renderList[(GUIImpact)value].enabled = false;
                this.animationList[(GUIImpact)value] = ((Component)this.transformList[(GUIImpact)value]).GetComponentInChildren<Animation>();
                this.offsetDisable[(GUIImpact)value] = 0f;
            }
        }
    }

    public void Show(GUIImpact currImpact, float offset)
    {
        if (!this.renderList[currImpact].enabled)
        {
            this.renderList[currImpact].enabled = true;
            this.Show(this.transformList[currImpact]);
            this.animationList[currImpact]["Transparency"].speed = 0.3f;
            this.animationList[currImpact]["Transparency"].time = 0f;
            this.animationList[currImpact].Play("Transparency");
        }
        if (offset != 0f)
        {
            this.offsetDisable[currImpact] = Time.time + offset;
        }
        else
        {
            this.offsetDisable[currImpact] = 0f;
        }
    }

    public void Hide(GUIImpact currImpact)
    {
        this.renderList[currImpact].enabled = false;
        this.animationList[currImpact]["Transparency"].speed = 0f;
        this.animationList[currImpact]["Transparency"].time = 0f;
        this.animationList[currImpact].Stop("Transparency");
        this.offsetDisable[currImpact] = 0f;
    }

    private void OnDisable()
    {
    }

    private void OnEnable()
    {
    }

    private void Hide(Transform obj)
    {
    }

    private void Show(Transform obj)
    {
    }
}


