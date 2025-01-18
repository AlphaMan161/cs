// ILSpyBased#2
using System;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public ProgressBar lifeBar;

    public TextMesh userName;

    public MeshRenderer dominationIcon;

    private int team;

    private bool show;

    private long lastTargetTime = DateTime.Now.Ticks;

    private static readonly long TargetSetPeriod = 20000000L;

    private bool isTarget;

    private Renderer[] renderers;

    public bool Visible
    {
        get
        {
            return this.show;
        }
        set
        {
            this.show = value;
        }
    }

    public bool IsTarget
    {
        get
        {
            if (!this.isTarget)
            {
                return false;
            }
            if (this.lastTargetTime + EnemyInfo.TargetSetPeriod < DateTime.Now.Ticks)
            {
                this.isTarget = false;
            }
            return this.isTarget;
        }
        set
        {
            this.lastTargetTime = DateTime.Now.Ticks;
            this.isTarget = value;
        }
    }

    private void Awake()
    {
        this.renderers = base.GetComponentsInChildren<Renderer>();
        MeshRenderer[] componentsInChildren = base.GetComponentsInChildren<MeshRenderer>();
        componentsInChildren[0].sharedMaterial.shader = Shader.Find("GUI/3D Text Shader - Cull Back");
        MeshRenderer[] componentsInChildrenT = transform.parent.GetChild(6).GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer meshRenderer in componentsInChildrenT)
        {
            if (meshRenderer.gameObject.name == "nemesis")
            {
                meshRenderer.sharedMaterial.shader = Shader.Find("Particles/Alpha Blended");
            }
        }
        foreach (MeshRenderer meshRenderer in componentsInChildren)
        {
            if (meshRenderer.gameObject.name == "nemesis")
            {
                meshRenderer.sharedMaterial.shader = Shader.Find("Particles/Alpha Blended");
                this.dominationIcon = meshRenderer;
            }
        }
        if ((UnityEngine.Object)this.dominationIcon != (UnityEngine.Object)null)
        {
            this.dominationIcon.enabled = false;
            this.dominationIcon.gameObject.SetActive(false);
        }
    }

    public void SetName(string name)
    {
        this.userName.text = BadWorldFilter.CheckLite(name);
        this.userName.characterSize = 0.2f;
    }

    public void SetTeam(int team)
    {
        this.team = team;
        switch (team)
        {
            case 0:
                ((Component)this.userName).GetComponent<Renderer>().material.color = new Color(0.5803922f, 0.745098054f, 0.0431372561f, 1f);
                break;
            case 1:
                ((Component)this.userName).GetComponent<Renderer>().material.color = new Color(0.905882359f, 0.41568628f, 0.3647059f, 1f);
                break;
            case 2:
                ((Component)this.userName).GetComponent<Renderer>().material.color = new Color(0.3647059f, 0.6627451f, 0.905882359f, 1f);
                break;
        }
    }

    public void SetDomination(bool isDominator)
    {
        this.dominationIcon.gameObject.SetActive(true);
        this.dominationIcon.enabled = isDominator;
    }

    public void SetLife(float val)
    {
        this.lifeBar.SetValue(val);
    }

    public void Hide()
    {
        Renderer[] array = this.renderers;
        foreach (Renderer renderer in array)
        {
            renderer.enabled = false;
        }
        this.show = false;
    }

    public void Show()
    {
        Renderer[] array = this.renderers;
        foreach (Renderer renderer in array)
        {
            renderer.enabled = true;
        }
        this.show = true;
    }

    private void LateUpdate()
    {
        if ((PlayerManager.Instance.LocalPlayer.IsDead || (PlayerManager.Instance.LocalPlayer.Team == this.team && this.team != 0)) && this.show)
        {
            ((Component)this.userName).GetComponent<Renderer>().enabled = true;
            goto IL_006b;
        }
        ((Component)this.userName).GetComponent<Renderer>().enabled = false;
        goto IL_006b;
        IL_006b:
        if ((UnityEngine.Object)PlayerManager.Instance != (UnityEngine.Object)null)
        {
            if ((UnityEngine.Object)PlayerManager.Instance.ActiveCamera != (UnityEngine.Object)null)
            {
                base.transform.LookAt(PlayerManager.Instance.ActiveCamera.transform);
            }
        }
        else
        {
            base.transform.LookAt(Camera.main.transform);
        }
    }
}


