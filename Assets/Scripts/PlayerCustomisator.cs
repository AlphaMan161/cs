// dnSpy decompiler from Assembly-CSharp.dll class: PlayerCustomisator
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Character/Player Customisator")]
public class PlayerCustomisator : MonoBehaviour
{
	private string GetSystemName(string name)
	{
		if (name == string.Empty)
		{
			return string.Empty;
		}
		return name.Split(new char[]
		{
			'_'
		})[1];
	}

	public void SetDiffuseShader(bool quality)
	{
		Shader shader2 = Shader.Find("Legacy Shaders/Diffuse");
		Shader shader = Shader.Find("Legacy Shaders/Diffuse");//Diffuse 2
		Shader shader3 = Shader.Find("Legacy Shaders/Diffuse");
	/*	if (quality)
		{
			shader = Shader.Find("Diffuse"); //Diffuse
			shader2 = Shader.Find("Custom/HalfLambert");
		}  */
		foreach (ClothModel clothModel in this.clothModels)
		{
			if (!(clothModel == null))
			{
				if (clothModel.Material.shader.name == shader.name)
				{
					clothModel.Material.shader = shader3; //2
				}
			}
		}
	}

	private string GetSecondaryName(string name)
	{
		if (name == string.Empty)
		{
			return string.Empty;
		}
		if (name.Split(new char[]
		{
			'_'
		}).Length < 3)
		{
			return string.Empty;
		}
		return name.Split(new char[]
		{
			'_'
		})[2];
	}

	private void Init()
	{
		if (!this.isInit)
		{
			this.isInit = true;
			if (PlayerManager.Instance == null)
			{
				List<MonoBehaviour> list = new List<MonoBehaviour>();
				PlayerRemote component = base.transform.GetComponent<PlayerRemote>();
				component.ResurrectShield.SetActive(false);
				list.Add(component);
				list.Add(base.transform.GetComponent<AnimationSynchronizer>());
				list.Add(base.transform.GetComponent<NetworkTransformInterpolation>());
				list.Add(base.transform.GetComponent<NetworkTransformReceiver>());
				list.Add(base.transform.GetComponent<AudioSourceVC>());
				ActorAnimator component2 = base.transform.GetComponent<ActorAnimator>();
				if (base.transform.GetComponent<WeaponController>() != null)
				{
					list.Add(base.transform.GetComponent<WeaponController>());
				}
				if (base.transform.GetComponentInChildren<EnemyInfo>() != null)
				{
					list.Add(base.transform.GetComponentInChildren<EnemyInfo>());
				}
				foreach (MonoBehaviour monoBehaviour in list)
				{
					if (monoBehaviour != null)
					{
						monoBehaviour.enabled = false;
					}
				}
				EnemyInfo componentInChildren = base.GetComponentInChildren<EnemyInfo>();
				componentInChildren.gameObject.SetActive(false);
				foreach (WeaponLook weaponLook in base.transform.GetComponentsInChildren<WeaponLook>(true))
				{
					weaponLook.gameObject.SetActive(false);
				}
			}
			else if (!this.LocalPlayer)
			{
				foreach (Item item in base.GetComponentsInChildren<Item>(true))
				{
					item.Root = base.transform;
				}
			}
			this.ClothLists.Add(CCWearType.Backpacks, this.Backpacks);
			this.ClothLists.Add(CCWearType.Boots, this.Boots);
			this.ClothLists.Add(CCWearType.Gloves, this.Gloves);
			this.ClothLists.Add(CCWearType.Hats, this.Hats);
			this.ClothLists.Add(CCWearType.Heads, this.Heads);
			this.ClothLists.Add(CCWearType.Masks, this.Masks);
			this.ClothLists.Add(CCWearType.Others, this.Others);
			this.ClothLists.Add(CCWearType.Pants, this.Pants);
			this.ClothLists.Add(CCWearType.Shirts, this.Shirts);
			Dictionary<CCWearType, string> defaultClothes = this.DefaultClothes;
			CCWearType key = CCWearType.Backpacks;
			string value = this.Backpack;
			this.CurrentClothes[CCWearType.Backpacks] = value;
			defaultClothes[key] = value;
			Dictionary<CCWearType, string> defaultClothes2 = this.DefaultClothes;
			CCWearType key2 = CCWearType.Boots;
			value = this.Boot;
			this.CurrentClothes[CCWearType.Boots] = value;
			defaultClothes2[key2] = value;
			Dictionary<CCWearType, string> defaultClothes3 = this.DefaultClothes;
			CCWearType key3 = CCWearType.Gloves;
			value = this.Glove;
			this.CurrentClothes[CCWearType.Gloves] = value;
			defaultClothes3[key3] = value;
			Dictionary<CCWearType, string> defaultClothes4 = this.DefaultClothes;
			CCWearType key4 = CCWearType.Hats;
			value = this.Hat;
			this.CurrentClothes[CCWearType.Hats] = value;
			defaultClothes4[key4] = value;
			Dictionary<CCWearType, string> defaultClothes5 = this.DefaultClothes;
			CCWearType key5 = CCWearType.Heads;
			value = this.Head;
			this.CurrentClothes[CCWearType.Heads] = value;
			defaultClothes5[key5] = value;
			Dictionary<CCWearType, string> defaultClothes6 = this.DefaultClothes;
			CCWearType key6 = CCWearType.Masks;
			value = this.Mask;
			this.CurrentClothes[CCWearType.Masks] = value;
			defaultClothes6[key6] = value;
			Dictionary<CCWearType, string> defaultClothes7 = this.DefaultClothes;
			CCWearType key7 = CCWearType.Others;
			value = this.Other;
			this.CurrentClothes[CCWearType.Others] = value;
			defaultClothes7[key7] = value;
			Dictionary<CCWearType, string> defaultClothes8 = this.DefaultClothes;
			CCWearType key8 = CCWearType.Pants;
			value = this.Pant;
			this.CurrentClothes[CCWearType.Pants] = value;
			defaultClothes8[key8] = value;
			Dictionary<CCWearType, string> defaultClothes9 = this.DefaultClothes;
			CCWearType key9 = CCWearType.Shirts;
			value = this.Shirt;
			this.CurrentClothes[CCWearType.Shirts] = value;
			defaultClothes9[key9] = value;
			this.clothModels = base.transform.GetComponentsInChildren<ClothModel>(true);
			foreach (ClothModel clothModel in this.clothModels)
			{
				if (this.LocalPlayer)
				{
					clothModel.gameObject.SetActive(false);
				}
				if (clothModel.WearType != CCWearType.None)
				{
					this.ClothLists[clothModel.WearType].Add(clothModel.gameObject);
					if (clothModel.SystemName != this.GetSystemName(this.CurrentClothes[clothModel.WearType]))
					{
						clothModel.gameObject.SetActive(false);
					}
				}
			}
			QualityLevel qualityLevel = OptionsManager.QualityLevel;
			this.SetDiffuseShader(qualityLevel != QualityLevel.Fast && qualityLevel != QualityLevel.Fastest);
		}
	}

	private void Start()
	{
		this.Init();
	}

	public void HideCloth()
	{
		foreach (ClothModel clothModel in this.clothModels)
		{
			if (!(clothModel == null))
			{
				clothModel.GetComponent<Renderer>().enabled = false;
				clothModel.gameObject.SetActive(false);
			}
		}
	}

	public void SetDefaultCloth()
	{
		this.Init();
		foreach (ClothModel clothModel in this.clothModels)
		{
			if (!(clothModel == null))
			{
				if (clothModel.WearType != CCWearType.None)
				{
					if (clothModel.SystemName != this.GetSystemName(this.CurrentClothes[clothModel.WearType]))
					{
						clothModel.GetComponent<Renderer>().enabled = false;
						clothModel.gameObject.SetActive(false);
					}
					else
					{
						clothModel.gameObject.SetActive(true);
						clothModel.GetComponent<Renderer>().enabled = true;
					}
				}
			}
		}
	}

	public void SetCloth(CCWearType clothType, string clothName)
	{
		this.Init();
		if (clothName == string.Empty)
		{
			this.CurrentClothes[clothType] = this.DefaultClothes[clothType];
		}
		else
		{
			this.CurrentClothes[clothType] = clothName;
		}
		foreach (ClothModel clothModel in this.clothModels)
		{
			if (!(clothModel == null))
			{
				if (clothModel.WearType != CCWearType.None)
				{
					if (clothModel.SystemName != this.GetSystemName(this.CurrentClothes[clothModel.WearType]))
					{
						clothModel.GetComponent<Renderer>().enabled = false;
						clothModel.gameObject.SetActive(false);
					}
					else
					{
						if (this.LocalPlayer && clothModel.WearType == CCWearType.Gloves)
						{
							this.SetHandTextureID(clothModel);
						}
						clothModel.gameObject.SetActive(true);
						clothModel.GetComponent<Renderer>().enabled = true;
					}
					if (this.CurrentClothes[CCWearType.Hats] != string.Empty && this.GetSecondaryName(clothModel.gameObject.name) == "hair")
					{
						clothModel.GetComponent<Renderer>().enabled = false;
						clothModel.gameObject.SetActive(false);
					}
					if (clothModel.WearType == CCWearType.Heads && this.CurrentClothes[CCWearType.Masks] != string.Empty && this.GetSecondaryName(this.CurrentClothes[CCWearType.Masks]) == "H")
					{
						clothModel.GetComponent<Renderer>().enabled = false;
						clothModel.gameObject.SetActive(false);
					}
				}
			}
		}
	}

	private void SetHandTextureID(ClothModel gloveClothModel)
	{
		foreach (HandTextureLoader handTextureLoader in base.transform.GetComponentsInChildren<HandTextureLoader>(true))
		{
			RuntimeTextureLoader component = gloveClothModel.GetComponent<RuntimeTextureLoader>();
			if (!(component == null))
			{
				string textureID = component.textureID;
				if (!textureID.Contains("CLOTH_Gloves_Thanos") && !textureID.Contains("santa") && !(textureID == "CLOTH_shirt_mummy_Color") && !textureID.Contains("business") && !textureID.Contains("stalker"))
				{
					handTextureLoader.textureID = textureID;
				}
			}
		}
	}

	public void CleanCloth()
	{
		foreach (ClothModel clothModel in this.clothModels)
		{
			if (!(clothModel == null))
			{
				if (clothModel.WearType != CCWearType.None && clothModel.WearType != CCWearType.Others)
				{
					if (clothModel.SystemName != this.GetSystemName(this.CurrentClothes[clothModel.WearType]))
					{
						clothModel.GetComponent<Renderer>().enabled = false;
						clothModel.gameObject.SetActive(false);
						if (NetworkDev.Destroy_Geometry)
						{
							UnityEngine.Object.Destroy(clothModel.gameObject);
						}
					}
					else if (this.CurrentClothes[CCWearType.Hats] != string.Empty && this.GetSecondaryName(clothModel.gameObject.name) == "hair")
					{
						clothModel.GetComponent<Renderer>().enabled = false;
						clothModel.gameObject.SetActive(false);
						if (NetworkDev.Destroy_Geometry)
						{
							UnityEngine.Object.Destroy(clothModel.gameObject);
						}
					}
					else if (clothModel.WearType == CCWearType.Heads && this.CurrentClothes[CCWearType.Masks] != string.Empty && this.GetSecondaryName(this.CurrentClothes[CCWearType.Masks]) == "H")
					{
						clothModel.GetComponent<Renderer>().enabled = false;
						clothModel.gameObject.SetActive(false);
						if (NetworkDev.Destroy_Geometry)
						{
							UnityEngine.Object.Destroy(clothModel.gameObject);
						}
					}
					else
					{
						RuntimeTextureLoader component = clothModel.GetComponent<RuntimeTextureLoader>();
						if (component != null)
						{
							component.ForceLoad();
						}
					}
				}
			}
		}
	}

	public void InitTaunt(bool show, int tauntID)
	{
		foreach (ClothModel clothModel in this.clothModels)
		{
			if (!(clothModel == null))
			{
				if (clothModel.WearType == CCWearType.Others)
				{
					if (clothModel.gameObject.name.StartsWith(string.Format("Taunt{0}", tauntID)))
					{
						clothModel.GetComponent<Renderer>().enabled = show;
						clothModel.gameObject.active = show;
					}
				}
			}
		}
	}

	private void LateUpdate()
	{
	}

	public string CheckCloth()
	{
		foreach (ClothModel clothModel in this.clothModels)
		{
			if (!(clothModel == null))
			{
				if (clothModel.WearType == CCWearType.Shirts)
				{
					if (clothModel.SystemName == this.GetSystemName(this.CurrentClothes[clothModel.WearType]))
					{
						return string.Format("m:{0} t:{1} c:{2} shader:{3}", new object[]
						{
							clothModel.GetComponent<Renderer>().material.name,
							clothModel.GetComponent<Renderer>().material.mainTexture,
							clothModel.GetComponent<Renderer>().material.color,
							clothModel.GetComponent<Renderer>().material.shader
						});
					}
				}
			}
		}
		return string.Empty;
	}

	public void SetMaterial(Material newMaterial)
	{
		foreach (ClothModel clothModel in this.clothModels)
		{
			if (!(clothModel == null))
			{
				clothModel.SetMaterial(newMaterial);
			}
		}
	}

	public void RevertMaterials()
	{
		foreach (ClothModel clothModel in this.clothModels)
		{
			if (!(clothModel == null))
			{
				clothModel.Revert();
			}
		}
	}

	public string Backpack = string.Empty;

	public string Boot = "Boots_foot01";

	public string Glove = "Gloves_hand01";

	public string Hat = string.Empty;

	public string Head = "Heads_head01";

	public string Mask = string.Empty;

	public string Other = string.Empty;

	public string Pant = "Pants_legs01";

	public string Shirt = "Shirts_torso01";

	public Dictionary<CCWearType, string> CurrentClothes = new Dictionary<CCWearType, string>();

	public Dictionary<CCWearType, string> DefaultClothes = new Dictionary<CCWearType, string>();

	private List<GameObject> Backpacks = new List<GameObject>();

	private List<GameObject> Boots = new List<GameObject>();

	private List<GameObject> Gloves = new List<GameObject>();

	private List<GameObject> Hats = new List<GameObject>();

	private List<GameObject> Heads = new List<GameObject>();

	private List<GameObject> Masks = new List<GameObject>();

	private List<GameObject> Others = new List<GameObject>();

	private List<GameObject> Pants = new List<GameObject>();

	private List<GameObject> Shirts = new List<GameObject>();

	public Dictionary<CCWearType, List<GameObject>> ClothLists = new Dictionary<CCWearType, List<GameObject>>();

	private ClothModel[] clothModels;

	public bool LocalPlayer;

	private bool isInit;
}
