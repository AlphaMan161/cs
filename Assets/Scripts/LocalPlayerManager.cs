// dnSpy decompiler from Assembly-CSharp.dll class: LocalPlayerManager
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerManager : MonoBehaviour
{
    static Dictionary<string, int> _003C_003Ef__switch_0024map4;
	public static LocalPlayerManager Instance
	{
		get
		{
			return LocalPlayerManager.instance;
		}
	}

	private EffectManager EffectManager
	{
		get
		{
			if (this.effectManager == null)
			{
				this.effectManager = base.transform.GetComponent<EffectManager>();
			}
			return this.effectManager;
		}
	}

	public static GameScore GameScore
	{
		get
		{
			return LocalPlayerManager.Instance.gameScore;
		}
	}

	public bool IsInit
	{
		get
		{
			return this.isInit;
		}
	}

	public MyRoomSetting RoomSettings
	{
		get
		{
			return this.roomSettings;
		}
	}

	public LocalCampaign Campaign
	{
		get
		{
			return this.campaign;
		}
	}

	private void Start()
	{
		LocalPlayerManager.instance = this;
		this.Init();
	}

	public void Init()
	{
		string name = OptionsManager.ConnectingMap.Name;
		this.roomSettings = new MyRoomSetting(name, 1, 0);
		this.roomSettings.StartTime = DateTime.Now.ToBinary() / 10000L;
		LocalGameHUD.Instance.Play();
		LocalGameHUD.Instance.LockAndHideCursor();
		this.LocalPlayer = base.transform.Find("PlayerStarship").GetComponent<CombatPlayer>();
		this.LocalPlayer.Init();
		this.LocalPlayer.AuthID = 111;
		this.LocalPlayer.Name = "Player";
		this.localCamera = this.LocalPlayer.Camera;
		if (this.LocalPlayer.RailCamera != null)
		{
			this.LocalPlayer.RailCamera.gameObject.SetActive(false);
		}
		this.freeCamera = base.transform.Find("finish4").GetComponentInChildren<Camera>();
		this.freeCamera.gameObject.SetActive(false);
		UnityEngine.Debug.Log("Player AUTH ID: " + this.LocalPlayer.AuthID);
		this.LocalPlayer.playerID = 1;
		this.LocalPlayer.Audio = this.LocalPlayer.Camera.gameObject.GetComponent<AudioSource>();
		this.LocalPlayer.Health = 100;
		this.LocalPlayer.Energy = 100;
		this.LocalPlayer.Audio = this.LocalPlayer.Camera.gameObject.GetComponent<AudioSource>();
		this.Players = new Dictionary<int, CombatPlayer>();
		this.Players[this.LocalPlayer.playerID] = this.LocalPlayer;
		this.LocalPlayer.transform.localEulerAngles = new Vector3(0f, -180f, 0f);
		this.LocalPlayer.WalkController.RotationY = 0f;
		string text = name;
		if (text != null)
		{
			if (LocalPlayerManager._003C_003Ef__switch_0024map4 == null)
			{
				LocalPlayerManager._003C_003Ef__switch_0024map4 = new Dictionary<string, int>(2)
				{
					{
						"TutorialMap",
						0
					},
					{
						"BattleTutorialMap",
						1
					}
				};
			}
			int num;
			if (LocalPlayerManager._003C_003Ef__switch_0024map4.TryGetValue(text, out num))
			{
				if (num != 0)
				{
					if (num == 1)
					{
						this.LocalPlayer.LocalShotController.InitWeapons(true);
						base.transform.Find("finish4").Find("Wall").gameObject.SetActive(false);
						this.InitPosition = this.LocalPlayer.transform.position;
						this.InitRotation = this.LocalPlayer.transform.localEulerAngles;
						string[] array = new string[]
						{
							"Archon",
							"Nephilim",
							"Alseid",
							"Tyrant",
							"Boggart",
							"Markar",
							"Crinar",
							"Grumgar",
							"Sattek",
							"Anakt"
						};
						int num2 = 1;
						Transform transform = base.transform.Find("BotStarship" + num2);
						while (transform != null)
						{
							this.Players[num2 + 1] = transform.GetComponent<CombatPlayer>();
							this.Players[num2 + 1].Audio = this.Players[num2 + 1].gameObject.GetComponent<AudioSource>();
							this.Players[num2 + 1].AuthID = 110 + num2 + 1;
							if (array.Length > num2)
							{
								this.Players[num2 + 1].Name = array[num2 - 1];
							}
							else
							{
								this.Players[num2 + 1].Name = "Enemy Bot";
							}
							this.Players[num2 + 1].Health = 50;
							this.Players[num2 + 1].Energy = 50;
							this.Players[num2 + 1].gameObject.SetActive(false);
							Transform transform2 = base.transform.Find("BotTrajectory" + num2);
							int num3 = 1;
							Transform transform3 = transform2.Find("Point" + num3);
							while (transform3 != null)
							{
								this.Players[num2 + 1].transform.GetComponent<BotNavigationController>().Trajectory.AddNode(transform3.position);
								num3++;
								transform3 = transform2.Find("Point" + num3);
							}
							this.Players[num2 + 1].transform.GetComponent<BotNavigationController>().Trajectory.Finalize();
							num2++;
							transform = base.transform.Find("BotStarship" + num2);
						}
						this.Players[2].gameObject.SetActive(true);
						this.Players[3].gameObject.SetActive(true);
						this.campaign = base.gameObject.AddComponent<LocalCampaign>();
						GameObject task = this.showTask(LanguageManager.GetText("tutorialQuest02_01"), "image1", this.task4Prefab);
						SceneKillTargets sceneKillTargets = new SceneKillTargets(new Scene.SceneCompleteListener(this.BattleSceneCompleteListener), task, "Scene1KillTargets");
						sceneKillTargets.Objects["targets"] = new List<GameObject>();
						List<GameObject> list = (List<GameObject>)sceneKillTargets.Objects["targets"];
						list.Add(this.Players[2].gameObject);
						list.Add(this.Players[3].gameObject);
						GameObject gameObject = this.showArrow(new Vector3(23f, -5f, 20f));
						gameObject.AddComponent<LocalArrowTracer>();
						this.campaign.AddScene(sceneKillTargets);
					}
				}
				else
				{
					this.LocalPlayer.LocalShotController.InitWeapons(false);
					base.transform.Find("finish4").Find("BattleWall1").gameObject.SetActive(false);
					base.transform.Find("finish4").Find("BattleWall2").gameObject.SetActive(false);
					int num2 = 1;
					Transform transform = base.transform.Find("BotStarship" + num2);
					while (transform != null)
					{
						UnityEngine.Object.Destroy(transform.gameObject);
						num2++;
						transform = base.transform.Find("BotStarship" + num2);
					}
					this.campaign = base.gameObject.AddComponent<LocalCampaign>();
					GameObject task2 = this.showTask(LanguageManager.GetText("tutorialQuest01_01"), "image1", this.task1Prefab);
					this.campaign.AddScene(new SceneTurn(new Scene.SceneCompleteListener(this.SceneCompleteListener), task2, "Scene1Turn"));
				}
			}
		}
		this.gameScore = new GameScore(this.roomSettings.GameMode);
		this.isInit = true;
		foreach (CombatPlayer combatPlayer in this.Players.Values)
		{
			this.gameScore.AddUser(combatPlayer.AuthID, combatPlayer.Name, combatPlayer.Level, false, 0, null, combatPlayer.ClanArmId, combatPlayer.ClanTag);
			this.gameScore[combatPlayer.AuthID].IsDead = false;
		}
	}

	public void SpawnMe()
	{
		short team = 0;
		int health = 100;
		int energy = 100;
		this.LocalPlayer.Spawn(base.transform, this.InitPosition, this.InitRotation, team, health, energy, false, ZombieType.Boss);
		if (this.LocalPlayer.Team >= 0)
		{
			this.gameScore.AddUser(this.LocalPlayer.AuthID, this.LocalPlayer.Name, this.LocalPlayer.Level, false, (int)this.LocalPlayer.Team, null, this.LocalPlayer.ClanArmId, this.LocalPlayer.ClanTag);
			this.SetupGates((int)team);
		}
		else
		{
			UnityEngine.Debug.LogError("Spawning Player With Team -1 !!!");
		}
		LocalGameHUD.Instance.Play();
		LocalGameHUD.Instance.LockAndHideCursor();
		this.LocalPlayer.LocalShotController.InitWeapons(true);
		this.LocalPlayer.LocalShotController.enabled = true;
		this.LocalPlayer.WalkController.enabled = true;
		this.gameScore[this.LocalPlayer.AuthID].IsDead = false;
		this.freeCamera.gameObject.SetActive(false);
		this.localCamera.gameObject.SetActive(true);
		this.isInit = true;
		this.gameScore[this.Players[2].AuthID].IsDead = false;
		this.Players[2].Spawn(base.transform, this.Players[2].transform.position, this.Players[2].transform.localEulerAngles, 0, 50, 50, false, ZombieType.Boss);
		this.Players[2].gameObject.SetActive(true);
		this.gameScore[this.Players[3].AuthID].IsDead = false;
		this.Players[3].Spawn(base.transform, this.Players[3].transform.position, this.Players[3].transform.localEulerAngles, 0, 50, 50, false, ZombieType.Boss);
		this.Players[3].gameObject.SetActive(true);
		this.gameScore[this.Players[4].AuthID].IsDead = false;
		this.Players[4].Spawn(base.transform, this.Players[4].transform.position, this.Players[4].transform.localEulerAngles, 0, 50, 50, false, ZombieType.Boss);
		this.Players[4].gameObject.SetActive(false);
		this.gameScore[this.Players[5].AuthID].IsDead = false;
		this.Players[5].Spawn(base.transform, this.Players[5].transform.position, this.Players[5].transform.localEulerAngles, 0, 50, 50, false, ZombieType.Boss);
		this.Players[5].gameObject.SetActive(false);
		this.gameScore[this.Players[6].AuthID].IsDead = false;
		this.Players[6].Spawn(base.transform, this.Players[6].transform.position, this.Players[6].transform.localEulerAngles, 0, 50, 50, false, ZombieType.Boss);
		this.Players[6].gameObject.SetActive(false);
		this.gameScore[this.Players[7].AuthID].IsDead = false;
		this.Players[7].Spawn(base.transform, this.Players[7].transform.position, this.Players[7].transform.localEulerAngles, 0, 50, 50, false, ZombieType.Boss);
		this.Players[7].gameObject.SetActive(false);
		this.gameScore[this.Players[8].AuthID].IsDead = false;
		this.Players[8].Spawn(base.transform, this.Players[8].transform.position, this.Players[8].transform.localEulerAngles, 0, 50, 50, false, ZombieType.Boss);
		this.Players[8].gameObject.SetActive(false);
		this.gameScore[this.Players[9].AuthID].IsDead = false;
		this.Players[9].Spawn(base.transform, this.Players[9].transform.position, this.Players[9].transform.localEulerAngles, 0, 50, 50, false, ZombieType.Boss);
		this.Players[9].gameObject.SetActive(false);
		this.gameScore[this.Players[10].AuthID].IsDead = false;
		this.Players[10].Spawn(base.transform, this.Players[10].transform.position, this.Players[10].transform.localEulerAngles, 0, 50, 50, false, ZombieType.Boss);
		this.Players[10].gameObject.SetActive(false);
		base.transform.Find("finish4").Find("BattleWall1").gameObject.SetActive(true);
		base.transform.Find("finish4").Find("BattleWall2").gameObject.SetActive(true);
		this.campaign = base.gameObject.AddComponent<LocalCampaign>();
		GameObject task = this.showTask(LanguageManager.GetText("tutorialQuest02_01"), "image1", this.task4Prefab);
		SceneKillTargets sceneKillTargets = new SceneKillTargets(new Scene.SceneCompleteListener(this.BattleSceneCompleteListener), task, "Scene1KillTargets");
		sceneKillTargets.Objects["targets"] = new List<GameObject>();
		List<GameObject> list = (List<GameObject>)sceneKillTargets.Objects["targets"];
		list.Add(this.Players[2].gameObject);
		list.Add(this.Players[3].gameObject);
		GameObject gameObject = this.showArrow(new Vector3(23f, -5f, 9f));
		gameObject.AddComponent<LocalArrowTracer>();
		this.campaign.AddScene(sceneKillTargets);
	}

	private void SceneCompleteListener(Scene scene)
	{
		UnityEngine.Debug.Log("Scene Completed: " + scene.Name);
		string name = scene.Name;
		switch (name)
		{
		case "Scene1Turn":
		{
			this.hideTask(scene.Task.GetComponent<TransformTween>());
			this.campaign.RemoveScene(scene);
			GameObject task = this.showTask(LanguageManager.GetText("tutorialQuest01_02"), "image1", this.task2Prefab);
			this.campaign.AddScene(new SceneCameraScroll(new Scene.SceneCompleteListener(this.SceneCompleteListener), task, "Scene2CameraScroll"));
			break;
		}
		case "Scene2CameraScroll":
		{
			this.hideTask(scene.Task.GetComponent<TransformTween>());
			this.campaign.RemoveScene(scene);
			GameObject task2 = this.showTask(LanguageManager.GetText("tutorialQuest01_03"), "image1", this.task3Prefab);
			this.campaign.AddScene(new SceneMove(new Scene.SceneCompleteListener(this.SceneCompleteListener), task2, "Scene3Move"));
			break;
		}
		case "Scene3Move":
		{
			this.hideTask(scene.Task.GetComponent<TransformTween>());
			this.campaign.RemoveScene(scene);
			GameObject task3 = this.showTask(LanguageManager.GetText("tutorialQuest01_04"), "image1", this.task4Prefab);
			GameObject value = this.showArrow(new Vector3(-75f, -5f, 60f));
			GameObject gameObject = this.showArrow(new Vector3(23f, -5f, 9f));
			gameObject.AddComponent<LocalArrowTracer>();
			gameObject = this.showArrow(new Vector3(10f, -5f, -32f));
			gameObject.AddComponent<LocalArrowTracer>();
			gameObject = this.showArrow(new Vector3(-23f, -5f, 45f));
			gameObject.AddComponent<LocalArrowTracer>();
			gameObject = this.showArrow(new Vector3(-54f, -5f, -33f));
			gameObject.AddComponent<LocalArrowTracer>();
			gameObject = this.showArrow(new Vector3(-76f, -5f, 8f));
			gameObject.AddComponent<LocalArrowTracer>();
			SceneMoveTo sceneMoveTo = new SceneMoveTo(new Scene.SceneCompleteListener(this.SceneCompleteListener), task3, "Scene4MoveTo");
			sceneMoveTo.Objects["min"] = new Vector3(-91f, -8f, 46f);
			sceneMoveTo.Objects["max"] = new Vector3(-57f, 2f, 76f);
			sceneMoveTo.Objects["arrow"] = value;
			this.campaign.AddScene(sceneMoveTo);
			break;
		}
		case "Scene4MoveTo":
		{
			this.hideTask(scene.Task.GetComponent<TransformTween>());
			GameObject gameObject2 = (GameObject)scene.Objects["arrow"];
			this.hideArrow(gameObject2.GetComponent<AlphaTween>());
			this.campaign.RemoveScene(scene);
			GameObject task4 = this.showTask(LanguageManager.GetText("tutorialQuest01_05"), "image1", this.task5Prefab);
			this.campaign.AddScene(new SceneJump(new Scene.SceneCompleteListener(this.SceneCompleteListener), task4, "Scene5Jump"));
			break;
		}
		case "Scene5Jump":
		{
			this.hideTask(scene.Task.GetComponent<TransformTween>());
			this.campaign.RemoveScene(scene);
			GameObject task5 = this.showTask(LanguageManager.GetText("tutorialQuest01_06"), "image1", this.task6Prefab);
			GameObject value2 = this.showArrow(new Vector3(-75f, -6f, 108f));
			SceneMoveTo sceneMoveTo2 = new SceneMoveTo(new Scene.SceneCompleteListener(this.SceneCompleteListener), task5, "Scene6MoveTo");
			sceneMoveTo2.Objects["min"] = new Vector3(-91f, -8f, 91f);
			sceneMoveTo2.Objects["max"] = new Vector3(-57f, 2f, 121f);
			sceneMoveTo2.Objects["arrow"] = value2;
			this.campaign.AddScene(sceneMoveTo2);
			break;
		}
		case "Scene6MoveTo":
		{
			this.hideTask(scene.Task.GetComponent<TransformTween>());
			GameObject gameObject3 = (GameObject)scene.Objects["arrow"];
			this.hideArrow(gameObject3.GetComponent<AlphaTween>());
			this.campaign.RemoveScene(scene);
			GameObject task6 = this.showTask(LanguageManager.GetText("tutorialQuest01_07"), "image1", this.task7Prefab);
			ScenePickAmmo scenePickAmmo = new ScenePickAmmo(new Scene.SceneCompleteListener(this.SceneCompleteListener), task6, "Scene7PickAmmo");
			scenePickAmmo.Objects["weaponType"] = WeaponType.MACHINE_GUN;
			this.SpawnItem(0, ItemType.Ammo, new Vector3(-80f, -7f, 141f), 100, 4);
			GameObject gameObject4 = this.showArrow(new Vector3(-80f, -2f, 141f));
			gameObject4.AddComponent<LocalArrowTracer>();
			this.campaign.AddScene(scenePickAmmo);
			break;
		}
		case "Scene7PickAmmo":
		{
			this.hideTask(scene.Task.GetComponent<TransformTween>());
			this.campaign.RemoveScene(scene);
			GameObject task7 = this.showTask(LanguageManager.GetText("tutorialQuest01_08"), "image1", this.task8Prefab);
			SceneDestroyTargets sceneDestroyTargets = new SceneDestroyTargets(new Scene.SceneCompleteListener(this.SceneCompleteListener), task7, "Scene8DestroyTargets");
			sceneDestroyTargets.Objects["targets"] = new List<GameObject>();
			List<GameObject> list = (List<GameObject>)sceneDestroyTargets.Objects["targets"];
			list.Add(this.SpawnTurret(WeaponType.TURRET_TESLA, new Vector3(-90f, -9f, 195f), this.LocalPlayer, true, true, 1).gameObject);
			list.Add(this.SpawnTurret(WeaponType.TURRET_TESLA, new Vector3(-75f, -9f, 195f), this.LocalPlayer, true, true, 2).gameObject);
			list.Add(this.SpawnTurret(WeaponType.TURRET_TESLA, new Vector3(-60f, -9f, 195f), this.LocalPlayer, true, true, 3).gameObject);
			this.campaign.AddScene(sceneDestroyTargets);
			SceneNoAmmo sceneNoAmmo = new SceneNoAmmo(new Scene.SceneCompleteListener(this.SceneCompleteListener), task7, "Scene8aNoAmmo");
			sceneNoAmmo.Objects["weaponType"] = WeaponType.MACHINE_GUN;
			this.campaign.AddScene(sceneNoAmmo);
			sceneDestroyTargets.Objects["sceneA"] = sceneNoAmmo;
			break;
		}
		case "Scene8aNoAmmo":
		{
			TextMesh componentInChildren = scene.Task.transform.GetComponentInChildren<TextMesh>();
			componentInChildren.text = LanguageManager.GetText("tutorialQuest01_08a");
			this.SpawnItem(0, ItemType.Ammo, this.LocalPlayer.transform.position, 100, 4);
			GameObject gameObject5 = this.showArrow(this.LocalPlayer.transform.position + new Vector3(0f, 5f, 0f));
			gameObject5.AddComponent<LocalArrowTracer>();
			break;
		}
		case "Scene8DestroyTargets":
		{
			this.hideTask(scene.Task.GetComponent<TransformTween>());
			this.campaign.RemoveScene((Scene)scene.Objects["sceneA"]);
			this.campaign.RemoveScene(scene);
			LocalShotController.Instance.UpdateAmmoCount(0, 0, 0);
			GameObject task8 = this.showTask(LanguageManager.GetText("tutorialQuest01_09"), "image1", this.task9Prefab);
			ScenePickAmmo scenePickAmmo2 = new ScenePickAmmo(new Scene.SceneCompleteListener(this.SceneCompleteListener), task8, "Scene9PickAmmo");
			scenePickAmmo2.Objects["weaponType"] = WeaponType.ROCKET_LAUNCHER;
			this.SpawnItem(0, ItemType.Ammo, this.LocalPlayer.transform.position, 10, 8);
			GameObject gameObject6 = this.showArrow(this.LocalPlayer.transform.position + new Vector3(0f, 5f, 0f));
			gameObject6.AddComponent<LocalArrowTracer>();
			this.campaign.AddScene(scenePickAmmo2);
			break;
		}
		case "Scene9PickAmmo":
		{
			this.hideTask(scene.Task.GetComponent<TransformTween>());
			this.campaign.RemoveScene(scene);
			GameObject task9 = this.showTask(LanguageManager.GetText("tutorialQuest01_10"), "image1", this.task10Prefab);
			SceneDestroyTargets sceneDestroyTargets2 = new SceneDestroyTargets(new Scene.SceneCompleteListener(this.SceneCompleteListener), task9, "Scene10DestroyTargets");
			sceneDestroyTargets2.Objects["targets"] = new List<GameObject>();
			List<GameObject> list2 = (List<GameObject>)sceneDestroyTargets2.Objects["targets"];
			list2.Add(this.SpawnTurret(WeaponType.TURRET_TESLA, new Vector3(-90f, -9f, 195f), this.LocalPlayer, true, true, 1).gameObject);
			list2.Add(this.SpawnTurret(WeaponType.TURRET_TESLA, new Vector3(-82.5f, -9f, 195f), this.LocalPlayer, true, true, 2).gameObject);
			list2.Add(this.SpawnTurret(WeaponType.TURRET_TESLA, new Vector3(-75f, -9f, 195f), this.LocalPlayer, true, true, 3).gameObject);
			list2.Add(this.SpawnTurret(WeaponType.TURRET_TESLA, new Vector3(-67.5f, -9f, 195f), this.LocalPlayer, true, true, 4).gameObject);
			list2.Add(this.SpawnTurret(WeaponType.TURRET_TESLA, new Vector3(-60f, -9f, 195f), this.LocalPlayer, true, true, 5).gameObject);
			this.campaign.AddScene(sceneDestroyTargets2);
			SceneNoAmmo sceneNoAmmo2 = new SceneNoAmmo(new Scene.SceneCompleteListener(this.SceneCompleteListener), task9, "Scene10aNoAmmo");
			sceneNoAmmo2.Objects["weaponType"] = WeaponType.ROCKET_LAUNCHER;
			this.campaign.AddScene(sceneNoAmmo2);
			sceneDestroyTargets2.Objects["sceneA"] = sceneNoAmmo2;
			break;
		}
		case "Scene10aNoAmmo":
		{
			TextMesh componentInChildren2 = scene.Task.transform.GetComponentInChildren<TextMesh>();
			componentInChildren2.text = LanguageManager.GetText("tutorialQuest01_10a");
			this.SpawnItem(0, ItemType.Ammo, this.LocalPlayer.transform.position, 10, 8);
			GameObject gameObject7 = this.showArrow(this.LocalPlayer.transform.position + new Vector3(0f, 5f, 0f));
			gameObject7.AddComponent<LocalArrowTracer>();
			break;
		}
		case "Scene10DestroyTargets":
		{
			this.hideTask(scene.Task.GetComponent<TransformTween>());
			this.campaign.RemoveScene((Scene)scene.Objects["sceneA"]);
			this.campaign.RemoveScene(scene);
			LocalShotController.Instance.UpdateAmmoCount(1, 0, 0);
			GameObject task10 = this.showTask(LanguageManager.GetText("tutorialQuest01_11"), "image1", this.task11Prefab);
			ScenePickAmmo scenePickAmmo3 = new ScenePickAmmo(new Scene.SceneCompleteListener(this.SceneCompleteListener), task10, "Scene11PickAmmo");
			scenePickAmmo3.Objects["weaponType"] = WeaponType.SNIPER_RIFLE;
			this.SpawnItem(0, ItemType.Ammo, this.LocalPlayer.transform.position, 10, 10);
			GameObject gameObject8 = this.showArrow(this.LocalPlayer.transform.position + new Vector3(0f, 5f, 0f));
			gameObject8.AddComponent<LocalArrowTracer>();
			this.campaign.AddScene(scenePickAmmo3);
			break;
		}
		case "Scene11PickAmmo":
		{
			this.hideTask(scene.Task.GetComponent<TransformTween>());
			this.campaign.RemoveScene(scene);
			GameObject task11 = this.showTask(LanguageManager.GetText("tutorialQuest01_12"), "image1", this.task12Prefab);
			SceneDestroyTargets sceneDestroyTargets3 = new SceneDestroyTargets(new Scene.SceneCompleteListener(this.SceneCompleteListener), task11, "Scene12DestroyTargets");
			sceneDestroyTargets3.Objects["targets"] = new List<GameObject>();
			List<GameObject> list3 = (List<GameObject>)sceneDestroyTargets3.Objects["targets"];
			list3.Add(this.SpawnTurret(WeaponType.TURRET_TESLA, new Vector3(-82.5f, -9f, 195f), this.LocalPlayer, true, true, 1).gameObject);
			list3.Add(this.SpawnTurret(WeaponType.TURRET_TESLA, new Vector3(-75f, -9f, 195f), this.LocalPlayer, true, true, 2).gameObject);
			list3.Add(this.SpawnTurret(WeaponType.TURRET_TESLA, new Vector3(-67.5f, -9f, 195f), this.LocalPlayer, true, true, 3).gameObject);
			this.campaign.AddScene(sceneDestroyTargets3);
			SceneNoAmmo sceneNoAmmo3 = new SceneNoAmmo(new Scene.SceneCompleteListener(this.SceneCompleteListener), task11, "Scene12aNoAmmo");
			sceneNoAmmo3.Objects["weaponType"] = WeaponType.SNIPER_RIFLE;
			this.campaign.AddScene(sceneNoAmmo3);
			sceneDestroyTargets3.Objects["sceneA"] = sceneNoAmmo3;
			break;
		}
		case "Scene12aNoAmmo":
		{
			TextMesh componentInChildren3 = scene.Task.transform.GetComponentInChildren<TextMesh>();
			componentInChildren3.text = LanguageManager.GetText("tutorialQuest01_12a");
			this.SpawnItem(0, ItemType.Ammo, this.LocalPlayer.transform.position, 10, 10);
			GameObject gameObject9 = this.showArrow(this.LocalPlayer.transform.position + new Vector3(0f, 5f, 0f));
			gameObject9.AddComponent<LocalArrowTracer>();
			break;
		}
		case "Scene12DestroyTargets":
		{
			this.hideTask(scene.Task.GetComponent<TransformTween>());
			this.campaign.RemoveScene((Scene)scene.Objects["sceneA"]);
			this.campaign.RemoveScene(scene);
			GameObject task12 = this.showTask(LanguageManager.GetText("tutorialQuest01_13"), "image1", this.task13Prefab);
			SceneZoom scene2 = new SceneZoom(new Scene.SceneCompleteListener(this.SceneCompleteListener), task12, "Scene13Zoom");
			this.campaign.AddScene(scene2);
			break;
		}
		case "Scene13Zoom":
		{
			this.hideTask(scene.Task.GetComponent<TransformTween>());
			this.campaign.RemoveScene(scene);
			GameObject task13 = this.showTask(LanguageManager.GetText("tutorialQuest01_14"), "image1", this.task14Prefab);
			SceneDestroyTargets sceneDestroyTargets4 = new SceneDestroyTargets(new Scene.SceneCompleteListener(this.SceneCompleteListener), task13, "Scene14DestroyTargets");
			sceneDestroyTargets4.Objects["targets"] = new List<GameObject>();
			List<GameObject> list4 = (List<GameObject>)sceneDestroyTargets4.Objects["targets"];
			list4.Add(this.SpawnTurret(WeaponType.TURRET_TESLA, new Vector3(-75f, -9f, 220f), this.LocalPlayer, true, true, 1).gameObject);
			this.campaign.AddScene(sceneDestroyTargets4);
			SceneNoAmmo sceneNoAmmo4 = new SceneNoAmmo(new Scene.SceneCompleteListener(this.SceneCompleteListener), task13, "Scene14aNoAmmo");
			sceneNoAmmo4.Objects["weaponType"] = WeaponType.SNIPER_RIFLE;
			this.campaign.AddScene(sceneNoAmmo4);
			sceneDestroyTargets4.Objects["sceneA"] = sceneNoAmmo4;
			CombatWeapon weaponByType = LocalShotController.Instance.GetWeaponByType(10);
			if (weaponByType.LoadedAmmo + weaponByType.AmmoReserve <= 0)
			{
				this.SpawnItem(0, ItemType.Ammo, this.LocalPlayer.transform.position, 10, 10);
				GameObject gameObject10 = this.showArrow(this.LocalPlayer.transform.position + new Vector3(0f, 5f, 0f));
				gameObject10.AddComponent<LocalArrowTracer>();
			}
			break;
		}
		case "Scene14aNoAmmo":
		{
			TextMesh componentInChildren4 = scene.Task.transform.GetComponentInChildren<TextMesh>();
			componentInChildren4.text = LanguageManager.GetText("tutorialQuest01_14a");
			this.SpawnItem(0, ItemType.Ammo, this.LocalPlayer.transform.position, 10, 10);
			GameObject gameObject11 = this.showArrow(this.LocalPlayer.transform.position + new Vector3(0f, 5f, 0f));
			gameObject11.AddComponent<LocalArrowTracer>();
			break;
		}
		case "Scene14DestroyTargets":
		{
			this.hideTask(scene.Task.GetComponent<TransformTween>());
			this.campaign.RemoveScene((Scene)scene.Objects["sceneA"]);
			this.campaign.RemoveScene(scene);
			LocalShotController.Instance.UpdateAmmoCount(3, 0, 0);
			GameObject task14 = this.showTask(LanguageManager.GetText("tutorialQuest01_15"), "image1", this.task4Prefab);
			SceneCompleteTasks sceneCompleteTasks = new SceneCompleteTasks(new Scene.SceneCompleteListener(this.SceneCompleteListener), task14, "Scene15CompleteTasks");
			sceneCompleteTasks.Objects["tasks"] = new Dictionary<string, bool>();
			Dictionary<string, bool> dictionary = (Dictionary<string, bool>)sceneCompleteTasks.Objects["tasks"];
			dictionary.Add("WaitForSeconds", false);
			dictionary.Add("WaitForRequest", false);
			this.campaign.AddScene(sceneCompleteTasks);
			base.StartCoroutine(this.WaitForSecondsIdle());
			break;
		}
		case "Scene15CompleteTasks":
			this.hideTask(scene.Task.GetComponent<TransformTween>());
			this.campaign.RemoveScene((Scene)scene.Objects["sceneA"]);
			this.campaign.RemoveScene(scene);
			this.DestroyCampaign();
			LocalGameHUD.Instance.ExitToMenu();
			break;
		}
	}

	private void OnQuestRequestComplete()
	{
		base.StartCoroutine(this.WaitForRequestIdle());
	}

	private IEnumerator WaitForSecondsIdle()
	{
		yield return new WaitForSeconds(4f);
		Scene sceneCompleteTasks = null;
		foreach (Scene scene in this.campaign.Scenes)
		{
			if (scene.GetType().ToString() == "SceneCompleteTasks")
			{
				sceneCompleteTasks = scene;
				break;
			}
		}
		if (sceneCompleteTasks == null)
		{
			base.StartCoroutine(this.WaitForSecondsIdle());
		}
		else
		{
			Dictionary<string, bool> sceneTasks = (Dictionary<string, bool>)sceneCompleteTasks.Objects["tasks"];
			sceneTasks["WaitForSeconds"] = true;
		}
		yield break;
	}

	private IEnumerator WaitForRequestIdle()
	{
		yield return new WaitForSeconds(0.1f);
		Scene sceneCompleteTasks = null;
		foreach (Scene scene in this.campaign.Scenes)
		{
			if (scene.GetType().ToString() == "SceneCompleteTasks")
			{
				sceneCompleteTasks = scene;
				break;
			}
		}
		if (sceneCompleteTasks == null)
		{
			base.StartCoroutine(this.WaitForRequestIdle());
		}
		else
		{
			Dictionary<string, bool> sceneTasks = (Dictionary<string, bool>)sceneCompleteTasks.Objects["tasks"];
			sceneTasks["WaitForRequest"] = true;
		}
		yield break;
	}

	private void BattleSceneCompleteListener(Scene scene)
	{
		UnityEngine.Debug.Log("Scene Completed: " + scene.Name);
		string name = scene.Name;
		switch (name)
		{
		case "Scene1KillTargets":
		{
			this.hideTask(scene.Task.GetComponent<TransformTween>());
			this.campaign.RemoveScene(scene);
			GameObject task = this.showTask(LanguageManager.GetText("tutorialQuest02_02"), "image1", this.task4Prefab);
			ScenePickEnergy scene2 = new ScenePickEnergy(new Scene.SceneCompleteListener(this.BattleSceneCompleteListener), task, "Scene2PickEnergy");
			this.Players[4].gameObject.SetActive(true);
			this.Players[5].gameObject.SetActive(true);
			this.Players[6].gameObject.SetActive(true);
			this.Players[7].gameObject.SetActive(true);
			this.Campaign.ObjectsToDestroy.Add(this.SpawnItem(0, ItemType.Health, new Vector3(-75f, -7f, 8f), 100, 0));
			GameObject gameObject = this.showArrow(new Vector3(-75f, -2f, 8f));
			gameObject.AddComponent<LocalArrowTracer>();
			if (this.LocalPlayer.Energy == 100)
			{
				this.LocalPlayer.Energy = 95;
			}
			this.campaign.AddScene(scene2);
			base.transform.Find("finish4").Find("BattleWall1").gameObject.SetActive(false);
			break;
		}
		case "Scene2PickEnergy":
		{
			this.hideTask(scene.Task.GetComponent<TransformTween>());
			this.campaign.RemoveScene(scene);
			GameObject task2 = this.showTask(LanguageManager.GetText("tutorialQuest02_03"), "image1", this.task4Prefab);
			SceneKillTargets sceneKillTargets = new SceneKillTargets(new Scene.SceneCompleteListener(this.BattleSceneCompleteListener), task2, "Scene3KillTargets");
			sceneKillTargets.Objects["targets"] = new List<GameObject>();
			List<GameObject> list = (List<GameObject>)sceneKillTargets.Objects["targets"];
			list.Add(this.Players[4].gameObject);
			list.Add(this.Players[5].gameObject);
			list.Add(this.Players[6].gameObject);
			list.Add(this.Players[7].gameObject);
			this.campaign.AddScene(sceneKillTargets);
			break;
		}
		case "Scene3KillTargets":
		{
			this.hideTask(scene.Task.GetComponent<TransformTween>());
			this.campaign.RemoveScene(scene);
			this.Campaign.ObjectsToDestroy.Add(this.SpawnItem(1, ItemType.Ammo, new Vector3(-81f, -7f, 179f), 200, 4));
			this.Campaign.ObjectsToDestroy.Add(this.SpawnItem(2, ItemType.Ammo, new Vector3(-62f, -7f, 186f), 10, 8));
			this.Campaign.ObjectsToDestroy.Add(this.SpawnItem(3, ItemType.Ammo, new Vector3(-62f, -7f, 206f), 3, 10));
			this.Campaign.ObjectsToDestroy.Add(this.SpawnItem(4, ItemType.Ammo, new Vector3(-81f, -7f, 212f), 100, 3));
			this.Campaign.ObjectsToDestroy.Add(this.SpawnItem(5, ItemType.Ammo, new Vector3(-95f, -7f, 195f), 200, 5));
			this.Campaign.ObjectsToDestroy.Add(this.SpawnItem(0, ItemType.Health, new Vector3(-78f, -7f, 196f), 100, 0));
			GameObject task3 = this.showTask(LanguageManager.GetText("tutorialQuest02_04"), "image1", this.task4Prefab);
			SceneKillTargets sceneKillTargets2 = new SceneKillTargets(new Scene.SceneCompleteListener(this.BattleSceneCompleteListener), task3, "Scene4KillTargets");
			sceneKillTargets2.Objects["targets"] = new List<GameObject>();
			List<GameObject> list2 = (List<GameObject>)sceneKillTargets2.Objects["targets"];
			list2.Add(this.Players[8].gameObject);
			list2.Add(this.Players[9].gameObject);
			list2.Add(this.Players[10].gameObject);
			GameObject gameObject2 = this.showArrow(new Vector3(-106f, -5f, 196f));
			gameObject2.transform.localEulerAngles = new Vector3(0f, 0f, 270f);
			SceneMoveTo sceneMoveTo = new SceneMoveTo(new Scene.SceneCompleteListener(this.BattleSceneCompleteListener), task3, "Scene4MoveTo");
			sceneMoveTo.Objects["min"] = new Vector3(-141f, -8f, 178f);
			sceneMoveTo.Objects["max"] = new Vector3(-106f, 2f, 208f);
			sceneMoveTo.Objects["arrow"] = gameObject2;
			this.Players[8].gameObject.SetActive(true);
			this.Players[9].gameObject.SetActive(true);
			this.Players[10].gameObject.SetActive(true);
			this.campaign.AddScene(sceneKillTargets2);
			this.campaign.AddScene(sceneMoveTo);
			sceneKillTargets2.Objects["sceneA"] = sceneMoveTo;
			base.transform.Find("finish4").Find("BattleWall2").gameObject.SetActive(false);
			break;
		}
		case "Scene4MoveTo":
		{
			GameObject gameObject3 = (GameObject)scene.Objects["arrow"];
			this.hideArrow(gameObject3.GetComponent<AlphaTween>());
			TextMesh componentInChildren = scene.Task.transform.GetComponentInChildren<TextMesh>();
			this.Campaign.ObjectsToDestroy.Add(this.SpawnItem(0, ItemType.Health, new Vector3(-274f, -7f, 223f), 100, 0));
			componentInChildren.text = LanguageManager.GetText("tutorialQuest02_04a");
			this.campaign.RemoveScene(scene);
			break;
		}
		case "Scene4KillTargets":
		{
			this.hideTask(scene.Task.GetComponent<TransformTween>());
			this.campaign.RemoveScene(scene);
			GameObject task4 = this.showTask(LanguageManager.GetText("tutorialQuest02_05"), "image1", this.task4Prefab);
			SceneCompleteTasks sceneCompleteTasks = new SceneCompleteTasks(new Scene.SceneCompleteListener(this.BattleSceneCompleteListener), task4, "Scene5CompleteTasks");
			sceneCompleteTasks.Objects["tasks"] = new Dictionary<string, bool>();
			Dictionary<string, bool> dictionary = (Dictionary<string, bool>)sceneCompleteTasks.Objects["tasks"];
			dictionary.Add("WaitForSeconds", false);
			dictionary.Add("WaitForRequest", false);
			this.campaign.AddScene(sceneCompleteTasks);
			base.StartCoroutine(this.WaitForSecondsIdle());
			break;
		}
		case "Scene5CompleteTasks":
			this.hideTask(scene.Task.GetComponent<TransformTween>());
			this.campaign.RemoveScene(scene);
			this.DestroyCampaign();
			OptionsManager.SoundVolumeMusic = 0.5f;
			OptionsManager.Instance.Save();
			LocalGameHUD.Instance.ExitToMenu();
			break;
		}
	}

	private void DestroyCampaign()
	{
		foreach (GameObject gameObject in this.campaign.ObjectsToDestroy)
		{
			if (!(gameObject == null))
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}
		UnityEngine.Object.Destroy(this.campaign);
	}

	public GameObject showArrow(Vector3 position)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.arrowPrefab);
		gameObject.transform.position = position;
		AlphaTween component = gameObject.GetComponent<AlphaTween>();
		component.Launch(0f, 1f, 1f);
		return gameObject;
	}

	public void hideArrow(AlphaTween arrowTween)
	{
		arrowTween.Launch(1f, 0f, 0.5f);
		DeleteAfterSeconds deleteAfterSeconds = arrowTween.gameObject.AddComponent<DeleteAfterSeconds>();
		deleteAfterSeconds.seconds = 1f;
	}

	public void hideTask(TransformTween taskTween)
	{
		taskTween.Launch(new Vector3(9f, 4.5f, 0f), new Vector3(9f, -14f, 0f), 0.7f, true);
		DeleteAfterSeconds deleteAfterSeconds = taskTween.gameObject.AddComponent<DeleteAfterSeconds>();
		deleteAfterSeconds.seconds = 1f;
	}

	public GameObject showTask(string label, string imageResource, GameObject taskPrefab)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(taskPrefab);
		gameObject.transform.parent = LocalGameHUD.Instance.TaskBox;
		gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		TextMesh componentInChildren = gameObject.transform.GetComponentInChildren<TextMesh>();
		componentInChildren.text = label;
		Transform transform = gameObject.transform.FindChild("picture");
		transform.position = new Vector3(transform.position.x, componentInChildren.transform.GetComponent<MeshRenderer>().bounds.min.y - 0.5f, transform.position.z);
		TransformTween component = gameObject.GetComponent<TransformTween>();
		component.Launch(new Vector3(9f, 9f, 0f), new Vector3(9f, 4.5f, 0f), 0.5f);
		return gameObject;
	}

	public void Init(Hashtable joinData)
	{
		this.freeCamera = base.transform.GetComponentInChildren<Camera>();
		Hashtable data = (Hashtable)joinData[100];
		this.roomSettings = new MyRoomSetting(data);
		this.gameScore = new GameScore(this.roomSettings.GameMode);
		Hashtable hashtable = (Hashtable)joinData[99];
		Hashtable actorData = (Hashtable)joinData[98];
		int actorID = (int)joinData[97];
		this.Players = new Dictionary<int, CombatPlayer>();
		this.InitMe(actorID, actorData);
		if (hashtable != null)
		{
			foreach (object obj in hashtable.Keys)
			{
				this.InitEnemy((int)obj, (Hashtable)hashtable[obj]);
			}
		}
		this.InitControlPoints(this.roomSettings.GameMode == MapMode.MODE.CONTROL_POINTS);
		this.InitFlagPoints(this.roomSettings.GameMode == MapMode.MODE.CAPTURE_THE_FLAG);
		this.InitGates(this.roomSettings.GameMode != MapMode.MODE.DEATHMATCH);
		this.InitWater();
		this.InitEnvironment();
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Init Player Manager: GameMode = ",
			this.roomSettings.GameMode,
			" startTime=",
			this.RoomSettings.StartTime
		}));
	}

	public void InitControlPoints(bool on)
	{
		int num = 1;
		this.cpoints = new Transform[5];
		while (base.transform.Find("finish4").Find("ControlPoint" + num))
		{
			this.cpoints[num - 1] = base.transform.Find("finish4").Find("ControlPoint" + num).Find("ControlPoint");
			if (!on)
			{
				this.cpoints[num - 1].GetComponent<Renderer>().enabled = false;
				this.cpoints[num - 1].gameObject.SetActive(false);
			}
			num++;
		}
	}

	public void InitFlagPoints(bool on)
	{
		int num = 1;
		this.flags = new FlagPoint[2];
		while (base.transform.Find("finish4").Find("FlagPoint" + num))
		{
			Transform transform = base.transform.Find("finish4").Find("FlagPoint" + num);
			if (!on)
			{
				transform.GetComponent<Renderer>().enabled = false;
				transform.gameObject.SetActive(false);
			}
			num++;
		}
	}

	public void InitWater()
	{
		Transform transform = base.transform.FindChild("finish4").FindChild("Water");
		if (transform != null)
		{
			transform.gameObject.SetActive(true);
			for (int i = 0; i < transform.GetChildCount(); i++)
			{
				transform.GetChild(i).gameObject.SetActive(true);
			}
			this.water = true;
		}
		else
		{
			this.water = false;
		}
	}

	public void InitGates(bool on)
	{
		int num = 1;
		while (base.transform.Find("finish4").Find("GatesRed" + num))
		{
			Transform transform = base.transform.Find("finish4").Find("GatesRed" + num);
			if (!on)
			{
				transform.GetComponent<Renderer>().enabled = false;
				transform.gameObject.SetActive(false);
			}
			num++;
		}
		num = 1;
		while (base.transform.Find("finish4").Find("GatesBlue" + num))
		{
			Transform transform = base.transform.Find("finish4").Find("GatesBlue" + num);
			if (!on)
			{
				transform.GetComponent<Renderer>().enabled = false;
				transform.gameObject.SetActive(false);
			}
			num++;
		}
	}

	public void InitEnvironment()
	{
		RenderSettings.fog = true;
		RenderSettings.fogColor = new Color(0.5137255f, 0.6313726f, 0.7490196f);
		RenderSettings.fogDensity = 0.001f;
	}

	private void SetupPlayerItems(Hashtable playerItemData)
	{
		foreach (object obj in playerItemData.Keys)
		{
			Dictionary<long, Hashtable> dictionary = (Dictionary<long, Hashtable>)playerItemData[obj];
			int actorID = (int)obj;
			foreach (Hashtable shotData in dictionary.Values)
			{
				this.SpawnPlayerItem(actorID, shotData);
			}
		}
	}

	public void InitMe(int actorID, Hashtable actorData)
	{
		if (this.LocalPlayer != null)
		{
			UnityEngine.Debug.LogError("Init Me Second Time!!!");
		}
		Hashtable hashtable = (Hashtable)actorData[96];
		int num = (int)hashtable[93];
		this.LocalPlayer.Init(actorID, actorData, this.LocalPlayer.Camera.gameObject.GetComponent<AudioSource>());
		this.LocalPlayer.WalkController.setSpeed(this.LocalPlayer.Speed, this.LocalPlayer.Jump);
		this.Players.Add(actorID, this.LocalPlayer);
		this.localCamera = this.LocalPlayer.Camera;
		this.localCamera.gameObject.SetActive(false);
		this.LocalPlayer.WalkController.enabled = false;
		this.LocalPlayer.ShotController.enabled = false;
	}

	public void InitEnemy(int actorID, Hashtable actorData)
	{
		if (actorID == this.LocalPlayer.playerID)
		{
			UnityEngine.Debug.LogError("Init Me As Enemy Leak!!!");
			return;
		}
		Hashtable hashtable = (Hashtable)actorData[96];
		int num = (int)hashtable[93];
	}

	public void UpdateScore(Hashtable scoreData, bool init)
	{
		int reward = (int)scoreData[89];
		if (scoreData.ContainsKey(88))
		{
			Hashtable hashtable = (Hashtable)scoreData[88];
			foreach (object obj in hashtable.Keys)
			{
				int num = (int)obj;
				Hashtable hashtable2 = (Hashtable)hashtable[num];
				CombatPlayer combatPlayer = this.Players[num];
				if (init)
				{
					short num2 = (short)hashtable2[239];
					if (num2 >= 0)
					{
						this.gameScore.AddUser(combatPlayer.AuthID, combatPlayer.Name, combatPlayer.Level, false, (int)num2, null, combatPlayer.ClanArmId, combatPlayer.ClanTag);
					}
					if (hashtable2.ContainsKey(101) && GameHUD.Instance.PlayerState != GameHUD.PlayerStates.TimeOver)
					{
						UnityEngine.Debug.Log("SetGameState: SPAWN");
						NetworkTransform ntransform = NetworkTransform.FromHashtable((Hashtable)hashtable2[237]);
						this.SpawnEnemy(num, ntransform, num2);
					}
				}
				this.gameScore[combatPlayer.AuthID].Kill = (int)hashtable2[69];
				this.gameScore[combatPlayer.AuthID].Death = (int)hashtable2[68];
				this.gameScore[combatPlayer.AuthID].Point = (int)hashtable2[67];
				this.gameScore[combatPlayer.AuthID].Domination = (int)hashtable2[32];
			}
		}
		if (scoreData.ContainsKey(87))
		{
			Hashtable hashtable3 = (Hashtable)scoreData[87];
			foreach (object obj2 in hashtable3.Keys)
			{
				byte b = (byte)obj2;
				Hashtable hashtable4 = (Hashtable)hashtable3[b];
				this.gameScore.SetTeamPoints((short)b, (int)hashtable4[67]);
			}
		}
		GameHUD.Instance.setReward(reward);
	}

	public void SetupGates(int team)
	{
		if (this.roomSettings.GameMode == MapMode.MODE.DEATHMATCH)
		{
			return;
		}
		int num = 1;
		while (base.transform.Find("finish4").Find("GatesRed" + num))
		{
			Transform transform = base.transform.Find("finish4").Find("GatesRed" + num);
			BoxCollider component = transform.GetComponent<BoxCollider>();
			if (team == 2)
			{
				component.enabled = true;
			}
			else
			{
				component.enabled = false;
			}
			num++;
		}
		num = 1;
		while (base.transform.Find("finish4").Find("GatesBlue" + num))
		{
			Transform transform = base.transform.Find("finish4").Find("GatesBlue" + num);
			BoxCollider component = transform.GetComponent<BoxCollider>();
			if (team == 1)
			{
				component.enabled = true;
			}
			else
			{
				component.enabled = false;
			}
			num++;
		}
	}

	public void MovePlayer(int actorID, Hashtable data)
	{
		NetworkTransform ntransform = NetworkTransform.FromHashtable(data);
		if (!this.Players.ContainsKey(actorID))
		{
			UnityEngine.Debug.Log("No actor to move!!!");
			return;
		}
		CombatPlayer combatPlayer = this.Players[actorID];
		if (actorID == this.LocalPlayer.playerID)
		{
			UnityEngine.Debug.LogError("No actor to move!!! LOCAL");
			return;
		}
		combatPlayer.NetworkTransformReceiver.ReceiveTransform(ntransform);
		if (this.gameScore.ContainsUser(combatPlayer.AuthID) && data.ContainsKey(81))
		{
			this.gameScore[combatPlayer.AuthID].Ping = (int)((short)data[81]);
		}
	}

	public GameObject SpawnItem(Hashtable itemData)
	{
		int key = (int)itemData[75];
		ItemType itemType = (ItemType)((byte)itemData[73]);
		NetworkTransform networkTransform = NetworkTransform.FromHashtable((Hashtable)itemData[71]);
		int type = 0;
		if (itemData.ContainsKey(72))
		{
			type = (int)((short)itemData[72]);
		}
		GameObject original = null;
		switch (itemType)
		{
		case ItemType.Ammo:
			this.items[key] = this.SpawnAmmo(networkTransform, type);
			return this.items[key];
		case ItemType.Health:
			original = this.healthPrefab;
			break;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
		gameObject.transform.position = networkTransform.Position + new Vector3(0f, 0f, 0f);
		gameObject.transform.localEulerAngles = networkTransform.Rotation;
		gameObject.transform.parent = base.transform;
		if (itemType == ItemType.Health)
		{
			gameObject.AddComponent<AudioSource>();
			SoundManager.Instance.Play(gameObject.GetComponent<AudioSource>(), "health container birth", AudioPlayMode.Play);
		}
		this.items[key] = gameObject;
		return gameObject;
	}

	public GameObject SpawnItem(int id, ItemType itemType, Vector3 position, int amount)
	{
		return this.SpawnItem(id, itemType, position, amount, 0);
	}

	public GameObject SpawnItem(int id, ItemType itemType, Vector3 position, int amount, short itemSubType)
	{
		GameObject original = null;
		switch (itemType)
		{
		case ItemType.Ammo:
			return this.SpawnAmmo(position, itemSubType, amount);
		case ItemType.Health:
			original = this.healthPrefab;
			break;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
		gameObject.transform.position = position + new Vector3(0f, 0f, 0f);
		gameObject.transform.parent = base.transform;
		if (itemType == ItemType.Health)
		{
			gameObject.AddComponent<AudioSource>();
			SoundManager.Instance.Play(gameObject.GetComponent<AudioSource>(), "health container birth", AudioPlayMode.Play);
		}
		LocalItemTracer localItemTracer = gameObject.AddComponent<LocalItemTracer>();
		localItemTracer.Launch(position, itemType, itemSubType, amount);
		return gameObject;
	}

	public GameObject SpawnAmmo(NetworkTransform ntransform, int type)
	{
		GameObject original = (GameObject)Resources.Load("AmmoCrates/AmmoCrate" + type);
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
		gameObject.transform.position = ntransform.Position + new Vector3(0f, 0f, 0f);
		gameObject.transform.localEulerAngles = ntransform.Rotation;
		gameObject.transform.parent = base.transform;
		gameObject.AddComponent<AudioSource>();
		SoundManager.Instance.Play(gameObject.GetComponent<AudioSource>(), "ammo container birth", AudioPlayMode.Play);
		return gameObject;
	}

	public GameObject SpawnAmmo(Vector3 position, short type, int amount)
	{
		GameObject original = (GameObject)Resources.Load("AmmoCrates/AmmoCrate" + type);
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
		gameObject.transform.parent = base.transform;
		gameObject.AddComponent<AudioSource>();
		SoundManager.Instance.Play(gameObject.GetComponent<AudioSource>(), "ammo container birth", AudioPlayMode.Play);
		LocalItemTracer localItemTracer = gameObject.AddComponent<LocalItemTracer>();
		localItemTracer.Launch(position, ItemType.Ammo, type, amount);
		return gameObject;
	}

	public LocalTurretTracer SpawnTurret(WeaponType weaponType, Vector3 position, CombatPlayer player, bool isMe, bool success, int launchAdd)
	{
		GameObject original = null;
		if (weaponType == WeaponType.TURRET_TESLA)
		{
			original = this.turretPrefab;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
		LocalTurretTracer component = gameObject.GetComponent<LocalTurretTracer>();
		component.Launch(position, weaponType, 100, launchAdd);
		player.RegisterItem(component);
		return component;
	}

	public void MoveItem(int id, NetworkTransform ntransform)
	{
		GameObject gameObject = this.items[id];
		gameObject.SetActive(true);
		NetworkTransformReceiver networkTransformReceiver = (NetworkTransformReceiver)gameObject.GetComponent("NetworkTransformReceiver");
		networkTransformReceiver.ReceiveTransform(ntransform);
	}

	public void PickItem(LocalItemTracer itemTracer)
	{
		ItemType itemType = itemTracer.ItemType;
		short itemSubType = itemTracer.ItemSubType;
		int amount = itemTracer.Amount;
		if (itemType == ItemType.Health)
		{
			SoundManager.Instance.Play(this.LocalPlayer.Camera.transform.GetComponent<AudioSource>(), "pick up energy container", AudioPlayMode.Play);
			this.LocalPlayer.Energy += amount;
			if (this.LocalPlayer.Energy > 100)
			{
				this.LocalPlayer.Energy = 100;
			}
			LocalGameHUD.Instance.UpdateHealth();
		}
		else if (itemType == ItemType.Ammo)
		{
			SoundManager.Instance.Play(this.LocalPlayer.Camera.transform.GetComponent<AudioSource>(), "pick up ammo container", AudioPlayMode.Play);
			LocalShotController.Instance.AddAmmoToReserve((int)itemSubType, amount);
		}
		UnityEngine.Object.Destroy(itemTracer.gameObject);
	}

	public void RemoveItem(int id)
	{
		if (this.items.ContainsKey(id))
		{
			UnityEngine.Object.Destroy(this.items[id]);
			this.items.Remove(id);
		}
	}

	public void RemoveWhizbang(int id)
	{
		if (this.items.ContainsKey(id))
		{
			TriggerDeleteAfterSeconds component = this.items[id].transform.GetComponent<TriggerDeleteAfterSeconds>();
			this.items[id].transform.FindChild("Rocket").gameObject.GetComponent<Renderer>().enabled = false;
			ParticleEmitter[] componentsInChildren = this.items[id].transform.GetComponentsInChildren<ParticleEmitter>();
			foreach (ParticleEmitter particleEmitter in componentsInChildren)
			{
				particleEmitter.emit = false;
			}
			component.Trigger();
		}
	}

	public void ReloadWeapon(int actorID, Hashtable ammoData)
	{
		if (actorID != this.LocalPlayer.playerID)
		{
			UnityEngine.Debug.LogError("Reloading Message Leak!!!");
			return;
		}
		int loadedAmmo = (int)ammoData[81];
		int ammo = (int)ammoData[80];
		int index = (int)ammoData[78];
		this.LocalPlayer.ShotController.OnReload(index, loadedAmmo, ammo);
	}

	public void UpdateEnhancer(int actorID, Hashtable enhancerData)
	{
		UnityEngine.Debug.Log("UpdateEnhacer:");
		if (actorID != this.LocalPlayer.playerID)
		{
			UnityEngine.Debug.LogError("Update Enhancer Leak!!!");
			return;
		}
		this.LocalPlayer.UpdateEnhancer(enhancerData);
	}

	public void SpawnPlayer(int actorID, Hashtable spawnData)
	{
		Hashtable data = (Hashtable)spawnData[237];
		short team = (short)spawnData[239];
		int health = (int)spawnData[100];
		int energy = (int)spawnData[99];
		NetworkTransform ntransform = NetworkTransform.FromHashtable(data);
		if (actorID != this.LocalPlayer.playerID)
		{
			this.SpawnEnemy(actorID, ntransform, team);
		}
		else
		{
			this.SpawnMe(actorID, ntransform, team, health, energy);
		}
	}

	public void SpawnMe(int actorID, NetworkTransform ntransform, short team, int health, int energy)
	{
		this.LocalPlayer.Spawn(base.transform, ntransform.Position, ntransform.Rotation, team, health, energy, false, ZombieType.Human);
		this.LocalPlayer.NetworkTransformSender.enabled = true;
		this.LocalPlayer.NetworkTransformSender.StartSendTransform();
		if (this.LocalPlayer.Team >= 0)
		{
			this.gameScore.AddUser(this.LocalPlayer.AuthID, this.LocalPlayer.Name, this.LocalPlayer.Level, false, (int)this.LocalPlayer.Team, null, this.LocalPlayer.ClanArmId, this.LocalPlayer.ClanTag);
			this.SetupGates((int)team);
		}
		else
		{
			UnityEngine.Debug.LogError("Spawning Player With Team -1 !!!");
		}
		this.LocalPlayer.InitWeapon();
		GameHUD.Instance.Play();
		this.LocalPlayer.ShotController.enabled = true;
		this.LocalPlayer.WalkController.enabled = true;
		this.gameScore[this.Players[actorID].AuthID].IsDead = false;
		this.freeCamera.gameObject.SetActive(false);
		this.localCamera.gameObject.SetActive(true);
		this.isInit = true;
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"SpawnMe ",
			actorID,
			" team:",
			team
		}));
	}

	public void SpawnEnemy(int actorID, NetworkTransform ntransform, short team)
	{
		CombatPlayer combatPlayer = this.Players[actorID];
		combatPlayer.Spawn(base.transform, ntransform.Position, ntransform.Rotation, team, 100, 100, false, ZombieType.Human);
		if (combatPlayer.Team >= 0)
		{
			this.gameScore.AddUser(combatPlayer.AuthID, combatPlayer.Name, combatPlayer.Level, false, (int)combatPlayer.Team, null, combatPlayer.ClanArmId, combatPlayer.ClanTag);
		}
		else
		{
			UnityEngine.Debug.LogError("Spawning Player With Team -1 !!!");
		}
		if (this.roomSettings.GameMode == MapMode.MODE.CAPTURE_THE_FLAG)
		{
			foreach (FlagPoint flagPoint in this.flags)
			{
				if (flagPoint != null && flagPoint.bearerID == combatPlayer.playerID)
				{
					flagPoint.flagObject.transform.parent = combatPlayer.transform;
					flagPoint.flagObject.transform.localPosition = new Vector3(0f, 1.8f, 0f);
				}
			}
		}
		combatPlayer.NetworkTransformReceiver.Reset();
		this.gameScore[this.Players[actorID].AuthID].IsDead = false;
		UnityEngine.Debug.Log("SpawnEnemy " + actorID);
	}

	public void UpdatePlayerEnergy(int actorID, Hashtable energyData)
	{
		if (actorID != this.LocalPlayer.playerID)
		{
			UnityEngine.Debug.LogError("Player Energy Message Leak!!!");
			return;
		}
		this.LocalPlayer.Energy = (int)energyData[99];
	}

	public void DestroyEnemy(int id)
	{
		CombatPlayer combatPlayer = this.Players[id];
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Actor ",
			combatPlayer.Name,
			" No.",
			id,
			" leaves"
		}));
		BattleChat.UserLeave(this.gameScore[combatPlayer.AuthID]);
		this.gameScore.RemoveUser(combatPlayer.AuthID);
		this.DetachFlag(combatPlayer.gameObject);
		combatPlayer.DestroyItems();
		this.Players.Remove(id);
		UnityEngine.Object.Destroy(combatPlayer.gameObject);
	}

	private void DetachFlag(GameObject owner)
	{
		if (this.flags == null)
		{
			return;
		}
		if (this.roomSettings.GameMode != MapMode.MODE.CAPTURE_THE_FLAG)
		{
			return;
		}
		for (int i = 0; i < this.flags.Length; i++)
		{
			if (!(this.flags[i] == null))
			{
				if (this.flags[i].flagObject.transform.parent == owner.transform)
				{
					this.flags[i].flagObject.transform.parent = null;
				}
			}
		}
	}

	public void SyncAnimation(int id, string msg, int layer)
	{
		CombatPlayer combatPlayer = this.Players[id];
		if (combatPlayer == null)
		{
			return;
		}
		if (layer == 0)
		{
			combatPlayer.AnimationSynchronizer.RemoteStateUpdate(msg);
		}
		else if (layer == 1)
		{
			combatPlayer.AnimationSynchronizer.RemoteSecondStateUpdate(msg);
		}
	}

	public void KillPlayer(int actorID, Hashtable killData)
	{
		int num = -1;
		if (killData.ContainsKey(94))
		{
			num = (int)killData[94];
		}
		WeaponType weaponType = WeaponType.NONE;
		if (killData.ContainsKey(91))
		{
			weaponType = (WeaponType)((byte)killData[91]);
		}
		if (num != this.LocalPlayer.playerID)
		{
			this.KillEnemy(num, false);
		}
		else
		{
			this.KillMe(false);
		}
		string name = this.Players[num].Name;
		string name2 = this.Players[actorID].Name;
		FragKill.FragType fragType = FragKill.FragType.None;
		if (killData.ContainsKey(33))
		{
			if ((byte)killData[33] == 35)
			{
				fragType = FragKill.FragType.Domination;
			}
			else
			{
				fragType = FragKill.FragType.Revenge;
			}
		}
		if (num == this.LocalPlayer.playerID)
		{
			if (actorID != this.LocalPlayer.playerID)
			{
				if (fragType == FragKill.FragType.Domination)
				{
					CombatPlayer combatPlayer = this.Players[actorID];
					if (combatPlayer == null)
					{
						return;
					}
					combatPlayer.IsDominator = true;
					LocalPlayerManager.GameScore[this.Players[actorID].AuthID].Nemesis = true;
				}
				LocalPlayerManager.GameScore[this.Players[actorID].AuthID].Victim = false;
			}
		}
		else if (actorID == this.LocalPlayer.playerID)
		{
			if (fragType == FragKill.FragType.Revenge)
			{
				CombatPlayer combatPlayer2 = this.Players[num];
				if (combatPlayer2 == null)
				{
					return;
				}
				combatPlayer2.IsDominator = false;
				LocalPlayerManager.GameScore[this.Players[num].AuthID].Nemesis = false;
			}
			else if (fragType == FragKill.FragType.Domination)
			{
				LocalPlayerManager.GameScore[this.Players[num].AuthID].Victim = true;
			}
		}
		LocalPlayerManager.GameScore.AddFrag(new FragKill(this.Players[actorID].AuthID, name2, this.Players[actorID].ClanTag, PlayerManager.GameScore.GetUserTeam(this.Players[actorID].AuthID), this.Players[num].AuthID, name, this.Players[num].ClanTag, PlayerManager.GameScore.GetUserTeam(this.Players[num].AuthID), weaponType, fragType));
	}

	public void KillEnemy(int id, bool endGame)
	{
		CombatPlayer combatPlayer = this.Players[id];
		this.gameScore[combatPlayer.AuthID].IsDead = true;
		GameObject gameObject = combatPlayer.gameObject;
		gameObject.SetActive(false);
		this.DetachFlag(gameObject);
		combatPlayer.Kill();
		if (endGame)
		{
			combatPlayer.IsDominator = false;
			combatPlayer.DestroyItems();
			return;
		}
		this.BloodEffect(gameObject.transform);
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.starshipExplodePrefab);
		gameObject2.transform.position = gameObject.transform.position;
		gameObject2.transform.rotation = gameObject.transform.rotation;
		gameObject2.transform.Rotate(Vector3.right * 90f);
		GameObject gameObject3 = gameObject2.transform.FindChild("Bomb").gameObject;
		gameObject3.GetComponent<Renderer>().enabled = false;
		GameObject gameObject4 = gameObject2.transform.FindChild("Parts").gameObject;
		SoundManager.Instance.Play(gameObject.GetComponent<AudioSource>(), "glider explosion", AudioPlayMode.Play);
		foreach (object obj in gameObject4.transform)
		{
			Transform transform = (Transform)obj;
			ParticleEmitter particleEmitter = transform.gameObject.AddComponent<MeshParticleEmitter>();
			ParticleRenderer particleRenderer = transform.gameObject.AddComponent<ParticleRenderer>();
			particleEmitter.minSize = 10f;
			particleEmitter.maxSize = 12f;
			particleEmitter.minEmission = 10f;
			particleEmitter.maxEmission = 12f;
			particleEmitter.minEnergy = 0.1f;
			particleEmitter.maxEnergy = 0.2f;
			particleEmitter.worldVelocity = new Vector3(0f, 15f, 0f);
			particleRenderer.material = this.SmokeMaterial;
		}
	}

	public void KillMe(bool endGame)
	{
		if (this.LocalPlayer == null)
		{
			return;
		}
		this.localCamera.gameObject.SetActive(false);
		this.freeCamera.gameObject.SetActive(true);
		this.freeCamera.transform.position = this.LocalPlayer.transform.position + new Vector3(0f, 0f, 0f);
		this.freeCamera.transform.rotation = this.LocalPlayer.transform.rotation;
		GameObject gameObject = this.LocalPlayer.gameObject;
		this.DetachFlag(gameObject);
		this.gameScore[this.LocalPlayer.AuthID].IsDead = true;
		this.LocalPlayer.LocalShotController.ResetWeapon();
		this.LocalPlayer.LocalShotController.enabled = false;
		this.LocalPlayer.WalkController.enabled = false;
		this.LocalPlayer.Kill();
		LocalGameHUD.Instance.UpdateHealth();
		if (endGame)
		{
			this.LocalPlayer.DestroyItems();
			return;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.starshipExplodePrefab);
		gameObject2.transform.position = gameObject.transform.position;
		gameObject2.transform.rotation = gameObject.transform.rotation;
		gameObject2.transform.Rotate(Vector3.right * 90f);
		GameObject gameObject3 = gameObject2.transform.FindChild("Bomb").gameObject;
		gameObject3.GetComponent<Renderer>().enabled = false;
		GameObject gameObject4 = gameObject2.transform.FindChild("Parts").gameObject;
		SoundManager.Instance.Play(gameObject2.GetComponent<AudioSource>(), "glider explosion", AudioPlayMode.Play);
		foreach (object obj in gameObject4.transform)
		{
			Transform transform = (Transform)obj;
			ParticleEmitter particleEmitter = transform.gameObject.AddComponent<MeshParticleEmitter>();
			ParticleRenderer particleRenderer = transform.gameObject.AddComponent<ParticleRenderer>();
			particleEmitter.minSize = 10f;
			particleEmitter.maxSize = 12f;
			particleEmitter.minEmission = 10f;
			particleEmitter.maxEmission = 12f;
			particleEmitter.minEnergy = 0.1f;
			particleEmitter.maxEnergy = 0.2f;
			particleEmitter.worldVelocity = new Vector3(0f, 15f, 0f);
			particleRenderer.material = this.SmokeMaterial;
		}
		if (this.campaign.Scenes.Count == 0)
		{
			UnityEngine.Object.Destroy(this.campaign);
			return;
		}
		Scene scene = this.campaign.Scenes[0];
		this.hideTask(scene.Task.GetComponent<TransformTween>());
		this.campaign.RemoveScene(scene);
		this.DestroyCampaign();
	}

	public void TimeOver(Hashtable rewardData)
	{
		GameHUD.Instance.setReward(0);
		MonoBehaviour.print("Time Over! Kill Players!");
		GameHUD.Instance.TimeOver();
		foreach (CombatPlayer combatPlayer in this.Players.Values)
		{
			if (!(combatPlayer == this.LocalPlayer))
			{
				this.KillEnemy(combatPlayer.playerID, true);
			}
		}
		this.KillMe(true);
	}

	public void NewGame(Hashtable newGameData)
	{
		long startTime = (long)newGameData[95];
		this.RoomSettings.StartTime = startTime;
		GameHUD.Instance.SetTimeOverReadyState();
		this.gameScore.Reset();
	}

	public void BloodEffect(Transform t)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.bloodPrefab);
		gameObject.transform.position = t.position;
	}

	public void SpawnPlayerItem(int actorID, Hashtable shotData)
	{
		Shot shot = Shot.FromHashtable(shotData);
		CombatPlayer player = this.Players[actorID];
		if (actorID != this.LocalPlayer.playerID)
		{
			this.ShotEffect(shot, player, ShotEffectType.ENEMY_SET_GAME_STATE);
		}
	}

	public void ShotPlayer(int actorID, Hashtable shotData)
	{
		Shot shot = Shot.FromHashtable(shotData);
		CombatPlayer combatPlayer = this.Players[actorID];
		if (shot.Targets != null)
		{
			foreach (ShotTarget shotTarget in shot.Targets)
			{
				CombatPlayer combatPlayer2;
				if (shotTarget.TargetID != this.LocalPlayer.playerID)
				{
					combatPlayer2 = this.Players[shotTarget.TargetID];
				}
				else
				{
					combatPlayer2 = this.LocalPlayer;
					this.LocalPlayer.Health -= shotTarget.HealthDamage;
					this.LocalPlayer.Energy -= shotTarget.EnergyDamage;
					GameHUD.Instance.UpdateHealth();
					if (shot.WeaponType != WeaponType.CHARGER && shot.LaunchMode == LaunchModes.SHOT)
					{
						GameHUD.Instance.ShotMe();
					}
					if (actorID != this.LocalPlayer.playerID && combatPlayer != null)
					{
						GameHUD.Instance.ShotMe(this.LocalPlayer.transform, combatPlayer.transform, combatPlayer.playerID);
					}
				}
				if (actorID == this.LocalPlayer.playerID && shotTarget.HealthDamage + shotTarget.EnergyDamage != 0)
				{
					this.ShotDamage(shotTarget.HealthDamage + shotTarget.EnergyDamage, combatPlayer2);
				}
				if (shotTarget.HealthDamage + shotTarget.EnergyDamage > 0)
				{
					this.EffectManager.HitEffect(shot, combatPlayer, combatPlayer2, shotTarget.EnergyDamage, shotTarget.HealthDamage);
				}
				shotTarget.TargetTransform = combatPlayer2.transform;
			}
		}
		if (actorID != this.LocalPlayer.playerID)
		{
			this.ShotEffect(shot, combatPlayer, ShotEffectType.ENEMY_AFTER_SERVER);
		}
		else if (this.ShotEffect(shot, combatPlayer, ShotEffectType.ME_AFTER_SERVER))
		{
			ShotController.Instance.OnShot(shot.WeaponType);
		}
	}

	public void ShotPlayer(int actorID, Shot shot)
	{
		CombatPlayer combatPlayer = this.Players[actorID];
		if (shot.Targets != null)
		{
			foreach (ShotTarget shotTarget in shot.Targets)
			{
				CombatPlayer combatPlayer2 = null;
				if (shotTarget.TargetID != this.LocalPlayer.playerID)
				{
					combatPlayer2 = this.Players[shotTarget.TargetID];
					combatPlayer2.Energy -= shotTarget.EnergyDamage;
					if (combatPlayer2.Energy < 0)
					{
						int num = -combatPlayer2.Energy;
						combatPlayer2.Energy = 0;
						combatPlayer2.Health -= num;
						if (combatPlayer2.Health <= 0)
						{
							this.KillEnemy(combatPlayer2.playerID, false);
						}
					}
				}
				else if (actorID != this.LocalPlayer.playerID)
				{
					combatPlayer2 = this.LocalPlayer;
					combatPlayer2.Energy -= shotTarget.EnergyDamage;
					if (combatPlayer2.Energy < 0)
					{
						int num2 = -combatPlayer2.Energy;
						combatPlayer2.Energy = 0;
						combatPlayer2.Health -= num2;
						if (combatPlayer2.Health <= 0)
						{
							this.KillMe(false);
						}
					}
					LocalGameHUD.Instance.UpdateHealth();
					if (shot.WeaponType != WeaponType.CHARGER && shot.LaunchMode == LaunchModes.SHOT)
					{
						LocalGameHUD.Instance.ShotMe();
					}
					if (actorID != this.LocalPlayer.playerID && combatPlayer != null)
					{
						LocalGameHUD.Instance.ShotMe(this.LocalPlayer.transform, combatPlayer.transform, combatPlayer.playerID);
					}
				}
				if (!(combatPlayer2 == null))
				{
					if (actorID == this.LocalPlayer.playerID && shotTarget.HealthDamage + shotTarget.EnergyDamage != 0)
					{
						this.ShotDamage(shotTarget.HealthDamage + shotTarget.EnergyDamage, combatPlayer2);
					}
					if (shotTarget.HealthDamage + shotTarget.EnergyDamage > 0)
					{
						this.EffectManager.HitEffect(shot, combatPlayer, combatPlayer2, shotTarget.EnergyDamage, shotTarget.HealthDamage);
					}
					shotTarget.TargetTransform = combatPlayer2.transform;
				}
			}
		}
		if (actorID != this.LocalPlayer.playerID)
		{
			this.ShotEffect(shot, combatPlayer, ShotEffectType.ENEMY_AFTER_SERVER);
		}
		else if (this.ShotEffect(shot, combatPlayer, ShotEffectType.ME_AFTER_SERVER))
		{
			LocalShotController.Instance.ShootAmmo((int)shot.WeaponType);
		}
	}

	private bool ShotEffect(Shot shot, CombatPlayer player, ShotEffectType shotEffectType)
	{
		bool isMe = shotEffectType == ShotEffectType.ME_BEFORE_SERVER || shotEffectType == ShotEffectType.ME_AFTER_SERVER;
		switch (shot.WeaponType)
		{
		case WeaponType.HAND_GUN:
		case WeaponType.MACHINE_GUN:
		case WeaponType.GATLING_GUN:
		case WeaponType.SHOT_GUN:
			if (shotEffectType == ShotEffectType.ME_AFTER_SERVER)
			{
				return true;
			}
			this.EffectManager.machineGunEffect(shot, player, isMe);
			break;
		case WeaponType.FLAMER:
			if (shotEffectType == ShotEffectType.ME_AFTER_SERVER)
			{
				return true;
			}
			this.EffectManager.flamerEffect(shot, player, isMe);
			break;
		case WeaponType.ROCKET_LAUNCHER:
			if (shotEffectType == ShotEffectType.ME_AFTER_SERVER && shot.LaunchMode == LaunchModes.LAUNCH)
			{
				return true;
			}
			if (shot.LaunchMode != LaunchModes.LAUNCH)
			{
				this.EffectManager.launcherEffect(shot, player, this.LocalPlayer, isMe);
				return false;
			}
			this.EffectManager.rocketLauncherEffect(shot, player, isMe);
			break;
		case WeaponType.GRENADE_LAUNCHER:
			if (shotEffectType == ShotEffectType.ME_AFTER_SERVER && shot.LaunchMode == LaunchModes.LAUNCH)
			{
				return true;
			}
			if (shot.LaunchMode != LaunchModes.LAUNCH)
			{
				this.EffectManager.launcherEffect(shot, player, this.LocalPlayer, isMe);
				return false;
			}
			this.EffectManager.grenadeLauncherEffect(shot, player, isMe);
			break;
		case WeaponType.SNIPER_RIFLE:
			if (shotEffectType == ShotEffectType.ME_AFTER_SERVER)
			{
				return true;
			}
			this.EffectManager.rgunEffect(shot, player, isMe);
			break;
		}
		return true;
	}

	public void turretTeslaEffect(Shot shot, CombatPlayer player, LocalTurretTracer turretTracer)
	{
		if (player == null)
		{
			return;
		}
		GameObject gameObject = turretTracer.gameObject;
		Charger componentInChildren = gameObject.GetComponentInChildren<Charger>();
		Transform targetTransform = shot.Targets[0].TargetTransform;
		if (targetTransform == null)
		{
			return;
		}
		if (componentInChildren)
		{
			componentInChildren.fire(targetTransform);
		}
	}

	public void hitTurret(Shot shot, CombatPlayer player, bool isMe, int damage)
	{
		if (!shot.HasTargets || shot.Targets.Count == 0)
		{
			return;
		}
		ShotTarget shotTarget = shot.Targets[0];
		if (shotTarget.ItemTimeStamp != -1L && player.RegisteredItems.ContainsKey(shotTarget.ItemTimeStamp))
		{
			LocalTurretTracer localTurretTracer = (LocalTurretTracer)player.RegisteredItems[shotTarget.ItemTimeStamp];
			if (localTurretTracer.RemoveHealth(damage))
			{
				Shot shot2 = new Shot(localTurretTracer.transform.position, new Vector3(0f, 1f, 0f), 108);
				this.EffectManager.launcherEffect(shot2, player, player, isMe);
				UnityEngine.Object.Destroy(localTurretTracer.gameObject);
				player.UnregisterItem(localTurretTracer.TimeStamp);
			}
		}
	}

	private float DistanceCoeffictient(float dist)
	{
		float num = 6.5f;
		float num2 = 20f;
		float result = 1f;
		if (dist > num2)
		{
			return 0f;
		}
		if (dist > num)
		{
			result = (num2 - dist) / (num2 - num);
		}
		return result;
	}

	private float AngleCoefficient(float angle)
	{
		float num = 5f;
		float num2 = 15f;
		float result = 1f;
		if (angle > num2)
		{
			return 0f;
		}
		if (angle > num)
		{
			result = (num2 - angle) / (num2 - num);
		}
		return result;
	}

	public void launcherEffect(Shot shot, CombatPlayer player, CombatPlayer localPlayer, bool isMe)
	{
		List<LocalTurretTracer> list = new List<LocalTurretTracer>();
		this.EffectManager.launcherEffect(shot, player, player, isMe);
		foreach (ItemTracer itemTracer in player.RegisteredItems.Values)
		{
			if (itemTracer.GetType() == typeof(LocalTurretTracer))
			{
				LocalTurretTracer localTurretTracer = (LocalTurretTracer)itemTracer;
				float magnitude = (shot.Origin - localTurretTracer.transform.position).magnitude;
				if (localTurretTracer.RemoveHealth((int)(100f * this.DistanceCoeffictient(magnitude))))
				{
					list.Add(localTurretTracer);
				}
			}
		}
		foreach (LocalTurretTracer localTurretTracer2 in list)
		{
			Shot shot2 = new Shot(localTurretTracer2.transform.position, new Vector3(0f, 1f, 0f), 108);
			this.EffectManager.launcherEffect(shot2, player, player, isMe);
			player.UnregisterItem(localTurretTracer2.TimeStamp);
			UnityEngine.Object.Destroy(localTurretTracer2.gameObject);
		}
		foreach (CombatPlayer combatPlayer in this.Players.Values)
		{
			if (!combatPlayer.IsDead)
			{
				if (!(combatPlayer == this.LocalPlayer))
				{
					float magnitude = (shot.Origin - combatPlayer.transform.position).magnitude;
					int num = (int)(200f * this.DistanceCoeffictient(magnitude));
					combatPlayer.Energy -= num;
					if (combatPlayer.Energy < 0)
					{
						combatPlayer.Health += combatPlayer.Energy;
						combatPlayer.Energy = 0;
						if (combatPlayer.Health < 0)
						{
							combatPlayer.Health = 0;
							this.KillEnemy(combatPlayer.playerID, false);
						}
					}
				}
			}
		}
	}

	public void flamerEffect(Shot shot, CombatPlayer player, bool isMe)
	{
		Vector3 position = new Vector3(0f, 0f, 0f);
		Vector3 position2 = player.transform.position;
		Vector3 origin = shot.Origin;
		Vector3 to = origin - position2;
		foreach (CombatPlayer combatPlayer in this.Players.Values)
		{
			if (!combatPlayer.IsDead)
			{
				if (!(combatPlayer == this.LocalPlayer))
				{
					position = combatPlayer.transform.position;
					Vector3 from = position - position2;
					float dist = from.magnitude;
					float angle = Vector3.Angle(from, to);
					int num = (int)(60f * this.DistanceCoeffictient(dist) * this.AngleCoefficient(angle));
					if (num > 0)
					{
						ShotTarget shotTarget = new ShotTarget();
						shotTarget.TargetID = combatPlayer.playerID;
						combatPlayer.Energy -= num;
						if (combatPlayer.Energy < 0)
						{
							combatPlayer.Health += combatPlayer.Energy;
							combatPlayer.Energy = 0;
							shotTarget.EnergyDamage = num + combatPlayer.Energy;
							shotTarget.HealthDamage = -combatPlayer.Energy;
							if (combatPlayer.Health < 0)
							{
								combatPlayer.Health = 0;
								this.KillEnemy(combatPlayer.playerID, false);
							}
						}
						else
						{
							shotTarget.EnergyDamage = num;
						}
						shot.Targets.Add(shotTarget);
					}
				}
			}
		}
	}

	public void ShotDamage(int damage, CombatPlayer player)
	{
		if (player == null)
		{
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.damageInfoPrefab);
		TextMesh componentInChildren = gameObject.GetComponentInChildren<TextMesh>();
		componentInChildren.text = damage.ToString();
		gameObject.transform.position = player.transform.position + new Vector3(0f, 3f, 0f);
	}

	public int Scan(Vector3 pos, float dist)
	{
		return this.Scan(pos, dist, false);
	}

	public int Scan(Vector3 pos, float dist, bool includingMe)
	{
		foreach (CombatPlayer combatPlayer in this.Players.Values)
		{
			if (!(combatPlayer.transform == null))
			{
				if (!combatPlayer.IsDead)
				{
					if (!(combatPlayer == this.LocalPlayer) || includingMe)
					{
						if (Vector3.Distance(combatPlayer.transform.position, pos) < dist)
						{
							return combatPlayer.playerID;
						}
					}
				}
			}
		}
		return -1;
	}

	public bool LocalScan(Vector3 pos, float dist)
	{
		return !(this.LocalPlayer.transform == null) && !this.LocalPlayer.IsDead && Vector3.Distance(this.LocalPlayer.transform.position, pos) <= dist;
	}

	public Transform ClosestTransformScan(Vector3 pos, float dist)
	{
		return null;
	}

	public int getTeamCount(short team)
	{
		int num = 0;
		foreach (CombatPlayer combatPlayer in this.Players.Values)
		{
			if (combatPlayer.Team == team)
			{
				num++;
			}
		}
		return num;
	}

	public GameObject enemyPrefab;

	public GameObject playerPrefab;

	public GameObject starshipExplodePrefab;

	public GameObject ammoPrefab;

	public GameObject healthPrefab;

	public GameObject bloodPrefab;

	public GameObject flagPrefab;

	public Material flagRed;

	public Material flagBlue;

	public Material flagCaptureRed;

	public Material flagCaptureBlue;

	public Material controlPointNeutral;

	public Material controlPointRed;

	public Material controlPointBlue;

	public GameObject damageInfoPrefab;

	public GameObject task1Prefab;

	public GameObject task2Prefab;

	public GameObject task3Prefab;

	public GameObject task4Prefab;

	public GameObject task5Prefab;

	public GameObject task6Prefab;

	public GameObject task7Prefab;

	public GameObject task8Prefab;

	public GameObject task9Prefab;

	public GameObject task10Prefab;

	public GameObject task11Prefab;

	public GameObject task12Prefab;

	public GameObject task13Prefab;

	public GameObject task14Prefab;

	public GameObject arrowPrefab;

	public GameObject turretPrefab;

	public Material SmokeMaterial;

	public bool water;

	private Camera freeCamera;

	private Camera localCamera;

	private static LocalPlayerManager instance;

	private EffectManager effectManager;

	private Dictionary<int, GameObject> items = new Dictionary<int, GameObject>();

	public Dictionary<int, CombatPlayer> Players;

	public CombatPlayer LocalPlayer;

	private FlagPoint[] flags;

	private Transform[] cpoints;

	private GameObject[] gates;

	private int cpointc;

	private MyRoomSetting roomSettings;

	private GameScore gameScore;

	private bool isInit;

	private LocalCampaign campaign;

	private Vector3 InitPosition;

	private Vector3 InitRotation;
}
