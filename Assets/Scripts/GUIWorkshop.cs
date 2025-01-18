// dnSpy decompiler from Assembly-CSharp.dll class: GUIWorkshop
using System;
using UnityEngine;

public class GUIWorkshop : MonoBehaviour
{
	public static void OnGUI()
	{
		GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("winMain"), new GUILayoutOption[]
		{
			GUILayout.Width(755f),
			GUILayout.Height(454f)
		});
		GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
		if (GUIWorkshop.SELECTED_MENU == 1)
		{
			GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"), new GUILayoutOption[0]);
			GUILayout.Label(LanguageManager.GetText("Equipped weapon"), GUISkinManager.Text.GetStyle("partActive"), new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
		}
		else if (GUILayout.Button(LanguageManager.GetText("Equipped weapon"), GUISkinManager.Text.GetStyle("part"), new GUILayoutOption[0]))
		{
			GUIWorkshop.SELECTED_MENU = 1;
		}
		if (GUIWorkshop.SELECTED_MENU == 2)
		{
			GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"), new GUILayoutOption[0]);
			GUILayout.Label(LanguageManager.GetText("All weapons"), GUISkinManager.Text.GetStyle("partActive"), new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
		}
		else if (GUILayout.Button(LanguageManager.GetText("All weapons"), GUISkinManager.Text.GetStyle("part"), new GUILayoutOption[0]))
		{
			GUIWorkshop.SELECTED_MENU = 2;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), new GUILayoutOption[]
		{
			GUILayout.Height(1f)
		});
		GUILayout.Space(3f);
		GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
		GUILayout.Space(15f);
		bool flag = true;
		GUIWorkshop.weaponScroll = GUILayout.BeginScrollView(GUIWorkshop.weaponScroll, false, true, new GUILayoutOption[]
		{
			GUILayout.MinHeight(378f)
		});
		if (GUIWorkshop.SELECTED_MENU == 1)
		{
			if (LocalUser.WeaponSlot.Weapon1 != null && LocalUser.WeaponSlot.Weapon1.Upgrade != null)
			{
				GUILayout.Space(10f);
				GUIWorkshop.DrawWeapon(LocalUser.WeaponSlot.Weapon1);
				flag = false;
			}
			if (LocalUser.WeaponSlot.Weapon2 != null && LocalUser.WeaponSlot.Weapon2.Upgrade != null)
			{
				GUILayout.Space(10f);
				GUIWorkshop.DrawWeapon(LocalUser.WeaponSlot.Weapon2);
				flag = false;
			}
			if (LocalUser.WeaponSlot.Weapon3 != null && LocalUser.WeaponSlot.Weapon3.Upgrade != null)
			{
				GUILayout.Space(10f);
				GUIWorkshop.DrawWeapon(LocalUser.WeaponSlot.Weapon3);
				flag = false;
			}
			if (LocalUser.WeaponSlot.Weapon4 != null && LocalUser.WeaponSlot.Weapon4.Upgrade != null)
			{
				GUILayout.Space(10f);
				GUIWorkshop.DrawWeapon(LocalUser.WeaponSlot.Weapon4);
				flag = false;
			}
			if (LocalUser.WeaponSlot.Weapon5 != null && LocalUser.WeaponSlot.Weapon5.Upgrade != null)
			{
				GUILayout.Space(10f);
				GUIWorkshop.DrawWeapon(LocalUser.WeaponSlot.Weapon5);
				flag = false;
			}
			if (LocalUser.WeaponSlot.Weapon6 != null && LocalUser.WeaponSlot.Weapon6.Upgrade != null)
			{
				GUILayout.Space(10f);
				GUIWorkshop.DrawWeapon(LocalUser.WeaponSlot.Weapon6);
				flag = false;
			}
			if (LocalUser.WeaponSlot.Weapon7 != null && LocalUser.WeaponSlot.Weapon7.Upgrade != null)
			{
				GUILayout.Space(10f);
				GUIWorkshop.DrawWeapon(LocalUser.WeaponSlot.Weapon7);
				flag = false;
			}
		}
		else
		{
			foreach (Weapon weapon in Inventory.Instance.Weapons)
			{
				if (weapon.Upgrade != null)
				{
					GUILayout.Space(10f);
					GUIWorkshop.DrawWeapon(weapon);
					flag = false;
				}
			}
		}
		if (flag)
		{
			GUILayout.Label(LanguageManager.GetText("Currently you have only basic set of weapons which can't be improved.\\nTo buy new weapon you need to go to the Shop and then press Weapons tab"), GUISkinManager.Text.GetStyle("noneBattle"), new GUILayoutOption[]
			{
				GUILayout.Height(378f)
			});
		}
		GUILayout.EndScrollView();
		GUILayout.Space(5f);
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}

	private static void DrawWeapon(Weapon weapon)
	{
		GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("itemBlock"), new GUILayoutOption[]
		{
			GUILayout.Width(702f),
			GUILayout.Height(106f)
		});
		GUILayout.Label(weapon.Ico, GUISkinManager.Backgound.GetStyle("itemLeft2"), new GUILayoutOption[]
		{
			GUILayout.Width(101f),
			GUILayout.Height(90f)
		});
		GUILayout.Space(6f);
		GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
		GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, new GUILayoutOption[]
		{
			GUILayout.Width(300f)
		});
		GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
		GUILayout.Label(weapon.Name, GUISkinManager.Text.GetStyle("normal01"), new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.Space(3f);
		if (weapon.IsUpgrade)
		{
			GUILayout.Label(LanguageManager.GetText("Upgraded"), GUISkinManager.Text.GetStyle("normal02"), new GUILayoutOption[]
			{
				GUILayout.MinWidth(80f)
			});
		}
		else
		{
			GUILayout.Label(LanguageManager.GetText("Not upgraded"), GUISkinManager.Text.GetStyle("normal05"), new GUILayoutOption[]
			{
				GUILayout.MinWidth(80f)
			});
		}
		GUILayout.Space(3f);
		if (weapon.IsUpgrade)
		{
			GUILayout.Label(weapon.UpgradeTxt, GUISkinManager.Text.GetStyle("normal11Active"), new GUILayoutOption[]
			{
				GUILayout.MinWidth(300f)
			});
		}
		else
		{
			GUILayout.Label(weapon.Upgrade.UpgradeTxt, GUISkinManager.Text.GetStyle("normal11Disable"), new GUILayoutOption[]
			{
				GUILayout.MinWidth(300f)
			});
		}
		GUILayout.EndVertical();
		GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
		GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, new GUILayoutOption[]
		{
			GUILayout.Width(176f)
		});
		GUILayout.Space(22f);
		GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		if (weapon.IsUpgrade)
		{
			GUILayout.Label(LanguageManager.GetTextFormat("remains: {0}", new object[]
			{
				weapon.Duration.ToString()
			}), GUISkinManager.Text.GetStyle("normal05"), new GUILayoutOption[0]);
		}
		else
		{
			GUILayout.Label(GUIContent.none, GUISkinManager.Text.GetStyle("normal05"), new GUILayoutOption[0]);
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.Space(2f);
		if (weapon.IsUpgrade)
		{
			GUIProgressBar.ProgressBar(176f, 864000f, (float)weapon.Duration.TotalSec, "pb4");
			GUIHover.Hover(Event.current, LanguageManager.GetTextFormat("remains: {0}", new object[]
			{
				weapon.Duration.ToString()
			}), GUILayoutUtility.GetLastRect());
		}
		GUILayout.EndVertical();
		GUILayout.Space(5f);
		GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
		GUILayout.Space(28f);
		if (weapon.Upgrade != null)
		{
			if (GUILayout.Button(weapon.Upgrade.Shop_Cost.Time1VCost.ToString(), GUISkinManager.Button.GetStyle("buyAbility"), new GUILayoutOption[]
			{
				GUILayout.Width(82f),
				GUILayout.Height(33f)
			}) && (weapon.Duration == null || weapon.Duration.Day <= 20))
			{
				ShopManager.Instance.BuyWeaponUpgrade(weapon);
			}
		}
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.EndHorizontal();
		GUILayout.EndHorizontal();
	}

	private static Vector2 weaponScroll = Vector2.zero;

	private static int SELECTED_MENU = 1;
}
