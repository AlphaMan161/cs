// dnSpy decompiler from Assembly-CSharp.dll class: CharacterCameraManager
using System;
using System.Collections;
using UnityEngine;

public class CharacterCameraManager : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void Init()
	{
		if (!this.isInit)
		{
			CharacterCameraManager.Instance = this;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(CharacterManager.Instance.GetPlayerEnemy());
			this.soldier = gameObject.transform;
			this.soldier.parent = base.transform;
			this.soldier.transform.localPosition = new Vector3(-3.174131f, -3.71f, 23f);
			this.soldier.transform.localEulerAngles = new Vector3(0f, 165f, 0f);
			this.playerCustomisator = (PlayerCustomisator)this.soldier.GetComponent(typeof(PlayerCustomisator));
			this.weaponController = this.soldier.transform.GetComponent<WeaponController>();
			this.actorAnimator = this.soldier.transform.GetComponent<ActorAnimator>();
			base.gameObject.AddComponent<SoundManager>();
			if (this.playerCustomisator != null && CharacterCameraManager.Instance != null)
			{
				this.isInit = true;
				this.SetPlayerViewDefault(LocalUser.View);
				this.SetTauntView(LocalUser.TauntSlot);
				QualityLevel qualityLevel = OptionsManager.QualityLevel;
				this.playerCustomisator.SetDiffuseShader(qualityLevel != QualityLevel.Fast && qualityLevel != QualityLevel.Fastest);
			}
			else
			{
				UnityEngine.Debug.Log("[CharacterCameraManager] Init is not init " + Time.time);
			}
		}
	}

	public void OnDisable()
	{
		if (this.playerViewDefault != null)
		{
			this.playerViewDefault.OnDreesUp -= this.HandleOnDressUp;
			this.playerViewDefault.OnUnDress -= this.HandleOnUnDress;
		}
		if (this.prevTauntSlot != null)
		{
			this.prevTauntSlot.OnPlay -= this.HandleOnPlayTaunt;
		}
	}

	public void SetPlayerViewDefault(PlayerView view)
	{
		if (this.playerViewDefault != null && this.playerViewDefault != view)
		{
			UnityEngine.Debug.Log("[CharacterCameraManager] SetPlayerViewDefault unsubscibe delegate");
			this.playerViewDefault.OnDreesUp -= this.HandleOnDressUp;
			this.playerViewDefault.OnUnDress -= this.HandleOnUnDress;
		}
		if (this.playerViewDefault != view)
		{
			UnityEngine.Debug.Log("[CharacterCameraManager] SetPlayerViewDefault subscribe delegate");
			this.playerViewDefault = view;
			this.playerViewDefault.OnDreesUp += this.HandleOnDressUp;
			this.playerViewDefault.OnUnDress += this.HandleOnUnDress;
		}
		else
		{
			UnityEngine.Debug.Log("[CharacterCameraManager] SetPlayerViewDefault none");
		}
		if (this.playerViewDefault.Hat != null)
		{
			this.playerCustomisator.SetCloth(CCWearType.Hats, this.playerViewDefault.Hat.ModelSystemName);
		}
		else
		{
			this.playerCustomisator.SetCloth(CCWearType.Hats, string.Empty);
		}
		if (this.playerViewDefault.Mask != null)
		{
			this.playerCustomisator.SetCloth(CCWearType.Masks, this.playerViewDefault.Mask.ModelSystemName);
		}
		else
		{
			this.playerCustomisator.SetCloth(CCWearType.Masks, string.Empty);
		}
		if (this.playerViewDefault.Gloves != null)
		{
			this.playerCustomisator.SetCloth(CCWearType.Gloves, this.playerViewDefault.Gloves.ModelSystemName);
		}
		else
		{
			this.playerCustomisator.SetCloth(CCWearType.Gloves, string.Empty);
		}
		if (this.playerViewDefault.Shirt != null)
		{
			this.playerCustomisator.SetCloth(CCWearType.Shirts, this.playerViewDefault.Shirt.ModelSystemName);
		}
		else
		{
			this.playerCustomisator.SetCloth(CCWearType.Shirts, string.Empty);
		}
		if (this.playerViewDefault.Pants != null)
		{
			this.playerCustomisator.SetCloth(CCWearType.Pants, this.playerViewDefault.Pants.ModelSystemName);
		}
		else
		{
			this.playerCustomisator.SetCloth(CCWearType.Pants, string.Empty);
		}
		if (this.playerViewDefault.Boots != null)
		{
			this.playerCustomisator.SetCloth(CCWearType.Boots, this.playerViewDefault.Boots.ModelSystemName);
		}
		else
		{
			this.playerCustomisator.SetCloth(CCWearType.Boots, string.Empty);
		}
		if (this.playerViewDefault.Backpack != null)
		{
			this.playerCustomisator.SetCloth(CCWearType.Backpacks, this.playerViewDefault.Backpack.ModelSystemName);
		}
		else
		{
			this.playerCustomisator.SetCloth(CCWearType.Backpacks, string.Empty);
		}
		if (this.playerViewDefault.Other != null)
		{
			this.playerCustomisator.SetCloth(CCWearType.Others, this.playerViewDefault.Other.ModelSystemName);
		}
		else
		{
			this.playerCustomisator.SetCloth(CCWearType.Others, string.Empty);
		}
		if (this.playerViewDefault.Head != null)
		{
			this.playerCustomisator.SetCloth(CCWearType.Heads, this.playerViewDefault.Head.ModelSystemName);
		}
		else
		{
			this.playerCustomisator.SetCloth(CCWearType.Heads, string.Empty);
		}
	}

	public void SetTauntView(TauntSlot taunts)
	{
		if (this.prevTauntSlot != null)
		{
			this.prevTauntSlot.OnPlay -= this.HandleOnPlayTaunt;
		}
		this.prevTauntSlot = taunts;
		this.prevTauntSlot.OnPlay += this.HandleOnPlayTaunt;
	}

	private void HandleOnPlayTaunt(object sender, int slot)
	{
		Taunt taunt = sender as Taunt;
		this.LaunchTaunt((int)taunt.TauntID);
	}

	public void LaunchTaunt(int tauntID)
	{
		if (this.taunt)
		{
			return;
		}
		SoundManager.Instance.Play(this.soldier.GetComponent<AudioSource>(), string.Format("Taunt{0}", tauntID), AudioPlayMode.Play);
		this.actorAnimator.TauntAnimation(string.Format("Taunt{0}", tauntID));
		this.playerCustomisator.InitTaunt(true, tauntID);
		this.taunt = true;
		base.StartCoroutine(this.FinishTaunt(tauntID));
    }

	private IEnumerator FinishTaunt(int tauntID)
	{
		float seconds = 1.5f;
		if (tauntID == 9)
		{
			seconds = 3.5f;
		}
		else if (tauntID == 8)
		{
			seconds = 8f;
		}
		else if (tauntID == 7)
		{
			seconds = 3.5f;
		}
		else if (tauntID > 5)
		{
			seconds = 5.6f;
		}
		else if (tauntID > 1)
		{
			seconds = 2.8f;
		}
		yield return new WaitForSeconds(seconds);
		this.playerCustomisator.InitTaunt(false, tauntID);
		this.actorAnimator.FinishTauntAnimation(false);
		this.taunt = false;
		yield break;
	}

	public void SetPlayerViewAdditional(PlayerView view)
	{
		if (this.playerViewAdditional != null && this.playerViewAdditional != view)
		{
			this.playerViewAdditional.OnDreesUp -= this.HandleOnDressUp;
			this.playerViewAdditional.OnUnDress -= this.HandleOnUnDressAdditonal;
		}
		if (this.playerViewAdditional != view)
		{
			this.playerViewAdditional = view;
			this.playerViewAdditional.OnDreesUp += this.HandleOnDressUp;
			this.playerViewAdditional.OnUnDress += this.HandleOnUnDressAdditonal;
		}
	}

	public void SetPlayerViewOther(PlayerView view)
	{
		this.playerViewOther = view;
		if (this.playerViewOther.Hat != null)
		{
			this.playerCustomisator.SetCloth(CCWearType.Hats, this.playerViewOther.Hat.ModelSystemName);
		}
		else
		{
			this.playerCustomisator.SetCloth(CCWearType.Hats, string.Empty);
		}
		if (this.playerViewOther.Mask != null)
		{
			this.playerCustomisator.SetCloth(CCWearType.Masks, this.playerViewOther.Mask.ModelSystemName);
		}
		else
		{
			this.playerCustomisator.SetCloth(CCWearType.Masks, string.Empty);
		}
		if (this.playerViewOther.Gloves != null)
		{
			this.playerCustomisator.SetCloth(CCWearType.Gloves, this.playerViewOther.Gloves.ModelSystemName);
		}
		else
		{
			this.playerCustomisator.SetCloth(CCWearType.Gloves, string.Empty);
		}
		if (this.playerViewOther.Shirt != null)
		{
			this.playerCustomisator.SetCloth(CCWearType.Shirts, this.playerViewOther.Shirt.ModelSystemName);
		}
		else
		{
			this.playerCustomisator.SetCloth(CCWearType.Shirts, string.Empty);
		}
		if (this.playerViewOther.Pants != null)
		{
			this.playerCustomisator.SetCloth(CCWearType.Pants, this.playerViewOther.Pants.ModelSystemName);
		}
		else
		{
			this.playerCustomisator.SetCloth(CCWearType.Pants, string.Empty);
		}
		if (this.playerViewOther.Boots != null)
		{
			this.playerCustomisator.SetCloth(CCWearType.Boots, this.playerViewOther.Boots.ModelSystemName);
		}
		else
		{
			this.playerCustomisator.SetCloth(CCWearType.Boots, string.Empty);
		}
		if (this.playerViewOther.Backpack != null)
		{
			this.playerCustomisator.SetCloth(CCWearType.Backpacks, this.playerViewOther.Backpack.ModelSystemName);
		}
		else
		{
			this.playerCustomisator.SetCloth(CCWearType.Backpacks, string.Empty);
		}
		if (this.playerViewOther.Other != null)
		{
			this.playerCustomisator.SetCloth(CCWearType.Others, this.playerViewOther.Other.ModelSystemName);
		}
		else
		{
			this.playerCustomisator.SetCloth(CCWearType.Others, string.Empty);
		}
		if (this.playerViewOther.Head != null)
		{
			this.playerCustomisator.SetCloth(CCWearType.Heads, this.playerViewOther.Head.ModelSystemName);
		}
		else
		{
			this.playerCustomisator.SetCloth(CCWearType.Heads, string.Empty);
		}
	}

	private void HandleOnDressUp(object obj)
	{
		Wear wear = obj as Wear;
		UnityEngine.Debug.Log("SetCloth: " + wear.ModelSystemName);
		this.playerCustomisator.SetCloth(wear.WearType, wear.ModelSystemName);
	}

	private void HandleOnUnDress(object obj)
	{
		UnityEngine.Debug.Log("[CharacterCameraManager] HandleOnUnDress " + obj.ToString());
		Wear wear = obj as Wear;
		UnityEngine.Debug.Log("SetCloth: ");
		this.playerCustomisator.SetCloth(wear.WearType, string.Empty);
	}

	private void HandleOnUnDressAdditonal(object obj)
	{
		UnityEngine.Debug.Log("[CharacterCameraManager] HandleOnUnDress " + obj.ToString());
		Wear wear = obj as Wear;
		UnityEngine.Debug.Log("[CharacterCameraManager] HandleOnUnDress wearType: " + wear.WearType);
		if (wear.WearType == CCWearType.Hats)
		{
			if (this.playerViewDefault.Hat != null)
			{
				this.playerCustomisator.SetCloth(CCWearType.Hats, this.playerViewDefault.Hat.ModelSystemName);
			}
			else
			{
				this.playerCustomisator.SetCloth(CCWearType.Hats, string.Empty);
			}
		}
		if (wear.WearType == CCWearType.Masks)
		{
			if (this.playerViewDefault.Mask != null)
			{
				this.playerCustomisator.SetCloth(CCWearType.Masks, this.playerViewDefault.Mask.ModelSystemName);
			}
			else
			{
				this.playerCustomisator.SetCloth(CCWearType.Masks, string.Empty);
			}
		}
		if (wear.WearType == CCWearType.Gloves)
		{
			if (this.playerViewDefault.Gloves != null)
			{
				this.playerCustomisator.SetCloth(CCWearType.Gloves, this.playerViewDefault.Gloves.ModelSystemName);
			}
			else
			{
				this.playerCustomisator.SetCloth(CCWearType.Gloves, string.Empty);
			}
		}
		if (wear.WearType == CCWearType.Shirts)
		{
			if (this.playerViewDefault.Shirt != null)
			{
				this.playerCustomisator.SetCloth(CCWearType.Shirts, this.playerViewDefault.Shirt.ModelSystemName);
			}
			else
			{
				this.playerCustomisator.SetCloth(CCWearType.Shirts, string.Empty);
			}
		}
		if (wear.WearType == CCWearType.Pants)
		{
			if (this.playerViewDefault.Pants != null)
			{
				this.playerCustomisator.SetCloth(CCWearType.Pants, this.playerViewDefault.Pants.ModelSystemName);
			}
			else
			{
				this.playerCustomisator.SetCloth(CCWearType.Pants, string.Empty);
			}
		}
		if (wear.WearType == CCWearType.Boots)
		{
			if (this.playerViewDefault.Boots != null)
			{
				this.playerCustomisator.SetCloth(CCWearType.Boots, this.playerViewDefault.Boots.ModelSystemName);
			}
			else
			{
				this.playerCustomisator.SetCloth(CCWearType.Boots, string.Empty);
			}
		}
		if (wear.WearType == CCWearType.Backpacks)
		{
			if (this.playerViewDefault.Backpack != null)
			{
				this.playerCustomisator.SetCloth(CCWearType.Backpacks, this.playerViewDefault.Backpack.ModelSystemName);
			}
			else
			{
				this.playerCustomisator.SetCloth(CCWearType.Backpacks, string.Empty);
			}
		}
		if (wear.WearType == CCWearType.Others)
		{
			if (this.playerViewDefault.Other != null)
			{
				this.playerCustomisator.SetCloth(CCWearType.Others, this.playerViewDefault.Other.ModelSystemName);
			}
			else
			{
				this.playerCustomisator.SetCloth(CCWearType.Others, string.Empty);
			}
		}
	}

	private void OnGUI()
	{
		GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[]
		{
			GUILayout.Width((float)Screen.width)
		});
		GUILayout.FlexibleSpace();
		GUILayout.Label(GUIContent.none, GUIStyle.none, new GUILayoutOption[]
		{
			GUILayout.Width(760f),
			GUILayout.Height(581f)
		});
		this.cameraRect = GUILayoutUtility.GetLastRect();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		if (Event.current.type == EventType.MouseDown && this.rotateRect.Contains(Event.current.mousePosition))
		{
			this.isRotate = true;
		}
		else if (Event.current.type == EventType.MouseUp)
		{
			this.isRotate = false;
		}
		if (this.isRotate && Event.current.type == EventType.MouseDrag)
		{
			float y = -Event.current.delta.x / 2f;
			this.soldier.transform.localEulerAngles += new Vector3(0f, y, 0f);
		}
		if (Screen.width > 760)
		{
			GUI.Label(this.stencilLeftRect, GUIContent.none, GUISkinManager.MainMenuCameraFix.GetStyle("left"));
			GUI.Label(this.stencilRightRect, GUIContent.none, GUISkinManager.MainMenuCameraFix.GetStyle("right"));
		}
		if (Screen.height > 694)
		{
			GUI.Label(this.stencilBottomRect, GUIContent.none, GUISkinManager.MainMenuCameraFix.GetStyle("bottom"));
		}
	}

	private void LateUpdate()
	{
		this.Init();
		if (MenuSelecter.MainMenuSelect != MenuSelecter.MainMenuEnum.Home && MenuSelecter.MainMenuSelect != MenuSelecter.MainMenuEnum.Headquarters && MenuSelecter.MainMenuSelect != MenuSelecter.MainMenuEnum.Shop && MenuSelecter.MainMenuSelect != MenuSelecter.MainMenuEnum.Statistic)
		{
			return;
		}
		if (Screen.width > 760)
		{
			this.stencilLeftRect.x = 0f;
			this.stencilLeftRect.y = this.verticalOffset;
			this.stencilLeftRect.width = this.cameraRect.x;
			this.stencilLeftRect.height = this.cameraRect.height;
			this.stencilRightRect.y = this.verticalOffset;
			this.stencilRightRect.x = this.cameraRect.x + this.cameraRect.width;
			this.stencilRightRect.width = (float)Screen.width - this.stencilRightRect.x;
			this.stencilRightRect.height = this.cameraRect.height;
		}
		if (Screen.height > 694)
		{
			this.stencilBottomRect.x = 0f;
			this.stencilBottomRect.y = this.verticalOffset + this.cameraRect.height;
			this.stencilBottomRect.width = (float)Screen.width;
			this.stencilBottomRect.height = (float)Screen.height - this.stencilBottomRect.y;
		}
		this.rotateRect = this.cameraRect;
		this.rotateRect.y = this.verticalOffset + 62f;
		this.rotateRect.width = 187f;
		this.rotateRect.x = this.rotateRect.x + 115f;
		this.rotateRect.height = 420f;
		if (this.CharacterCamera != null)
		{
			this.cameraRect.y = (float)Screen.height - this.cameraRect.height - this.verticalOffset;
			base.GetComponent<Camera>().pixelRect = this.cameraRect;
			if (this.BackgroundCamera != null)
			{
				this.BackgroundCamera.pixelRect = this.cameraRect;
			}
		}
	}

	public void SetCharacterQuality(QualityLevel level)
	{
		this.playerCustomisator.SetDiffuseShader(level != QualityLevel.Fast && level != QualityLevel.Fastest);
	}

	private Rect cameraRect = new Rect(0f, 0f, 0f, 0f);

	private float verticalOffset = 113f;

	public Camera CharacterCamera;

	public Camera BackgroundCamera;

	private Transform soldier;

	public static CharacterCameraManager Instance;

	private bool isInit;

	private PlayerCustomisator playerCustomisator;

	private bool taunt;

	private WeaponController weaponController;

	private ActorAnimator actorAnimator;

	private PlayerView playerViewDefault;

	private TauntSlot prevTauntSlot;

	private PlayerView playerViewAdditional;

	private PlayerView playerViewOther;

	private Rect rotateRect = new Rect(0f, 0f, 0f, 0f);

	private bool isRotate;

	private float startRotationY;

	private Rect stencilLeftRect = new Rect(0f, 0f, 0f, 0f);

	private Rect stencilRightRect = new Rect(0f, 0f, 0f, 0f);

	private Rect stencilBottomRect = new Rect(0f, 0f, 0f, 0f);
}
