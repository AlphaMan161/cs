// dnSpy decompiler from Assembly-CSharp.dll class: ClothModel
using System;
using UnityEngine;

[AddComponentMenu("Character/Cloth Model")]
public class ClothModel : MonoBehaviour
{
	public string SystemName
	{
		get
		{
			if (this.systemName == string.Empty)
			{
				this.systemName = this.GetSystemName(base.gameObject.name);
			}
			return this.systemName;
		}
	}

	public Material Material
	{
		get
		{
			if (this.material == null)
			{
				this.material = base.gameObject.GetComponent<Renderer>().material;
			}
			return this.material;
		}
	}

    private void Start()
    {
        if (gameObject.name == "Backpacks_parr01")
        {
            MeshRenderer components = base.gameObject.GetComponentInChildren<MeshRenderer>();
            components.sharedMaterial.shader = Shader.Find("Legacy Shaders/Diffuse");
        } else if (gameObject.name == "Taunt5_PioneerHorn" || gameObject.name == "Taunt56_PioneerHorn") {
            MeshRenderer components = base.gameObject.GetComponentInChildren<MeshRenderer>();
            components.sharedMaterial.shader = Shader.Find("Legacy Shaders/Diffuse");
        }
		if (this.material == null)
		{
			this.material = base.gameObject.GetComponent<Renderer>().material;
		}
	}

	public void Revert()
	{
		if (this.material == null)
		{
			return;
		}
		base.gameObject.GetComponent<Renderer>().material = this.material;
	}

	public void SetMaterial(Material newMaterial)
	{
		if (this.material == null)
		{
			this.material = base.gameObject.GetComponent<Renderer>().material;
		}
		base.gameObject.GetComponent<Renderer>().material = newMaterial;
	}

	private string GetSystemName(string name)
	{
		if (name == string.Empty)
		{
			return string.Empty;
		}
		if (name.Split(new char[]
		{
			'_'
		}).Length < 2)
		{
			UnityEngine.Debug.LogErrorFormat("Broken Wear: {0}", new object[]
			{
				name
			});
			return string.Empty;
		}
		return name.Split(new char[]
		{
			'_'
		})[1];
	}

	private string systemName = string.Empty;

	public CCWearType WearType;

	public bool Default;

	private Material material;
}
