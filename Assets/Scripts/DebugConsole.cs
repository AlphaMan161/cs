// ILSpyBased#2
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DebugConsole
{
    private struct ConsoleMessage
    {
        public readonly string message;

        public readonly string stackTrace;

        public readonly LogType type;

        public ConsoleMessage(string message, string stackTrace, LogType type)
        {
            this.message = message;
            this.stackTrace = stackTrace;
            this.type = type;
        }
    }

    private const int margin = 20;

    private static DebugConsole hInstance = null;

    public static readonly Version version = new Version(1, 0);

    public KeyCode toggleKey = KeyCode.BackQuote;

    private List<ConsoleMessage> entries = new List<ConsoleMessage>();

    private int selectMessageIndex = -1;

    private Vector2 scrollPos;

    private bool show;

    private bool collapse;

    private bool auto_scroll = true;

    private bool inText;

    private string currentCommand = string.Empty;

    private StringBuilder inTextStr = new StringBuilder();

    private Rect windowRect = new Rect(20f, 20f, (float)(Screen.width - 40), (float)(Screen.height - 40));

    private List<string> historyCommand = new List<string>();

    public static DebugConsole Instance
    {
        get
        {
            if (DebugConsole.hInstance == null)
            {
                DebugConsole.hInstance = new DebugConsole();
                DebugConsole.hInstance.OnEnable();
            }
            return DebugConsole.hInstance;
        }
    }

    public static bool IsShow
    {
        get
        {
            return DebugConsole.Instance.show;
        }
        set
        {
            DebugConsole.Instance.show = value;
        }
    }

    private void OnEnable()
    {
        Application.RegisterLogCallback(new Application.LogCallback(this.HandleLog));
    }

    private void OnDisable()
    {
        Application.RegisterLogCallback(null);
    }

    public static void ShowDebug(Event curEvent)
    {
        if (curEvent.keyCode == DebugConsole.Instance.toggleKey && curEvent.type == EventType.KeyUp && curEvent.shift)
        {
            DebugConsole.Instance.show = !DebugConsole.Instance.show;
        }
        if (DebugConsole.Instance.show)
        {
            if (curEvent.type == EventType.KeyDown && curEvent.keyCode == KeyCode.UpArrow)
            {
                goto IL_008b;
            }
            if (curEvent.type == EventType.KeyUp && curEvent.keyCode == KeyCode.UpArrow)
            {
                goto IL_008b;
            }
            if (curEvent.type != EventType.KeyDown || curEvent.keyCode != KeyCode.DownArrow)
            {
                ;
            }
            goto IL_012d;
        }
        return;
        IL_0179:
        if (!DebugConsole.Instance.historyCommand.Contains(DebugConsole.Instance.currentCommand))
        {
            DebugConsole.Instance.historyCommand.Add(DebugConsole.Instance.currentCommand.Clone().ToString());
        }
        DebugConsole.Instance.ExecuteCommand(DebugConsole.Instance.currentCommand.Split(' '));
        DebugConsole.Instance.currentCommand = string.Empty;
        goto IL_01ed;
        IL_012d:
        if (curEvent.type == EventType.KeyDown && curEvent.keyCode == KeyCode.Return)
        {
            goto IL_0179;
        }
        if (curEvent.type == EventType.Used && curEvent.keyCode == KeyCode.Return)
        {
            goto IL_0179;
        }
        if (curEvent.type == EventType.KeyDown && curEvent.character == '\n')
        {
            goto IL_0179;
        }
        goto IL_01ed;
        IL_01ed:
        Cursor.visible = true;
        DebugConsole.Instance.windowRect = GUILayout.Window(123456, DebugConsole.Instance.windowRect, new GUI.WindowFunction(DebugConsole.Instance.ConsoleWindow), "Console");
        return;
        IL_008b:
        int num = DebugConsole.Instance.historyCommand.IndexOf(DebugConsole.Instance.currentCommand);
        if (DebugConsole.Instance.historyCommand.Count > 0)
        {
            if (num > 0)
            {
                DebugConsole.Instance.currentCommand = DebugConsole.Instance.historyCommand[num - 1];
            }
            else
            {
                DebugConsole.Instance.currentCommand = DebugConsole.Instance.historyCommand[DebugConsole.Instance.historyCommand.Count - 1];
            }
        }
        goto IL_012d;
    }

    private void ConsoleWindow(int windowID)
    {
        GUI.FocusWindow(windowID);
        this.scrollPos = GUILayout.BeginScrollView(this.scrollPos);
        for (int i = 0; i < this.entries.Count; i++)
        {
            ConsoleMessage consoleMessage = this.entries[i];
            if (this.collapse && i > 0)
            {
                string message = consoleMessage.message;
                ConsoleMessage consoleMessage2 = this.entries[i - 1];
                if (!(message == consoleMessage2.message))
                {
                    goto IL_006f;
                }
                continue;
            }
            goto IL_006f;
            IL_006f:
            switch (consoleMessage.type)
            {
                case LogType.Error:
                case LogType.Exception:
                    GUI.contentColor = Color.red;
                    break;
                case LogType.Warning:
                    GUI.contentColor = Color.yellow;
                    break;
                default:
                    GUI.contentColor = Color.white;
                    break;
            }
            if (this.inText)
            {
                this.inTextStr.AppendLine(consoleMessage.message + "\nStack Trace: " + consoleMessage.stackTrace);
            }
            else
            {
                GUILayout.Label(consoleMessage.message);
                if (i == this.selectMessageIndex)
                {
                    GUILayout.Label(consoleMessage.stackTrace);
                }
                if (Event.current.type == EventType.MouseUp && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                {
                    if (this.selectMessageIndex == i)
                    {
                        this.selectMessageIndex = -1;
                    }
                    else
                    {
                        this.selectMessageIndex = i;
                    }
                }
            }
        }
        if (this.inText)
        {
            GUILayout.TextArea(this.inTextStr.ToString());
            this.inTextStr = new StringBuilder();
        }
        GUI.contentColor = Color.white;
        GUILayout.EndScrollView();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUI.SetNextControlName("consoleInputText");
        this.currentCommand = GUILayout.TextField(this.currentCommand);
        GUI.FocusControl("consoleInputText");
        GUILayout.EndHorizontal();
        GUI.DragWindow(new Rect(0f, 0f, 10000f, 20f));
    }

    public void Clear()
    {
        this.entries.RemoveRange(0, this.entries.Count);
    }

    private void ExecuteCommand(params object[] args)
    {
        if (args.Length < 1)
        {
            UnityEngine.Debug.Log("[DebugConsole] use command `help`");
        }
        else
        {
            string text = args[0].ToString();
            text = "help";
            if (args[0].ToString() == "login" && args.Length == 3)
            {
                Auth.Login(args[1].ToString(), args[2].ToString());
            }
            else if (args[0].ToString() == "help")
            {
                UnityEngine.Debug.Log("[DebugConsole] close clear setlang dhover");
            }
            else if (args[0].ToString() == "cheatmsg")
            {
                GameHUD.Instance.CheatMessage();
            }
            else if (args[0].ToString() == "setlang")
            {
                if (args.Length < 2)
                {
                    UnityEngine.Debug.Log("[DebugConsole] setlang [ru, en]");
                }
                else
                {
                    LanguageManager.ChangeLang(args[1].ToString(), true);
                }
            }
            else if (args[0].ToString() == "debug.fps" && args.Length > 1)
            {
                Configuration.DebugEnableFps = Convert.ToBoolean(args[1]);
            }
            else if (args[0].ToString() == "debug.loadmanager" && args.Length > 1)
            {
                Configuration.DebugLoadManager = Convert.ToBoolean(args[1]);
            }
            else if (args[0].ToString() == "setbadword")
            {
                if (args.Length < 2)
                {
                    UnityEngine.Debug.Log("[DebugConsole] setbadword [1, 0] current: " + OptionsManager.EnableBadWorldFilter);
                }
                else
                {
                    if (args[1].ToString() == "1" || args[1].ToString() == "true")
                    {
                        OptionsManager.EnableBadWorldFilter = true;
                    }
                    else
                    {
                        OptionsManager.EnableBadWorldFilter = false;
                    }
                    UnityEngine.Debug.Log("[DebugConsole] setbadword new value: " + OptionsManager.EnableBadWorldFilter);
                }
            }
            else if (args[0].ToString() == "close" || args[0].ToString() == "exit")
            {
                this.show = false;
            }
            else if (args[0].ToString() == "clear")
            {
                this.Clear();
            }
            else if (args[0].ToString() == "cursor")
            {
                UnityEngine.Debug.Log("UnityEngine.Cursor.visible: + " + Cursor.visible);
                UnityEngine.Debug.Log("Screen.lockCursor: + " + Screen.lockCursor);
            }
            else if (args[0].ToString() == "pl")
            {
                UnityEngine.Debug.Log("Total Player: " + ServersList.TotalPlayers);
            }
            else if (!(args[0].ToString() == "bug"))
            {
                if (args[0].ToString() == "lag")
                {
                    if (args.Length <= 1)
                    {
                        Lagometer.CountLags = !Lagometer.CountLags;
                    }
                }
                else if (args[0].ToString() == "anim")
                {
                    UnityEngine.Debug.Log(string.Format("Animation Keys: {0}", (!NetworkDev.Remote_Animation) ? "Adaptive" : "Remote"));
                }
                else if (args[0].ToString() == "interp")
                {
                    UnityEngine.Debug.Log(string.Format("Interpolation Mode: {0}", NetworkDev.InterpolationMode.ToString()));
                }
                else if (args[0].ToString() == "stype")
                {
                    UnityEngine.Debug.Log(string.Format("STYPE: {0}", Configuration.SType));
                }
                else if (args[0].ToString() == "cspeed")
                {
                    UnityEngine.Debug.Log(string.Format("CSPEED: {0}", PlayerManager.Instance.LocalPlayer.CharacterMotor.movement.maxForwardSpeed));
                }
                else if (args[0].ToString() == "cmspeed")
                {
                    UnityEngine.Debug.Log(string.Format("CMAXSPEEDINDIR: {0}", PlayerManager.Instance.LocalPlayer.CharacterMotor.reportMaxSpeedInDirection));
                }
                else if (args[0].ToString() == "cmsspeed")
                {
                    UnityEngine.Debug.Log(string.Format("CMAXSIDESPEED: {0}", PlayerManager.Instance.LocalPlayer.CharacterMotor.reportMaxSidewaySpeed));
                }
                else if (args[0].ToString() == "cddir")
                {
                    UnityEngine.Debug.Log(string.Format("CDESIREDLOCALDIR: {0}", PlayerManager.Instance.LocalPlayer.CharacterMotor.reportDesiredLocalDirection));
                }
                else if (args[0].ToString() == "cexforce")
                {
                    UnityEngine.Debug.Log(string.Format("CEXPLOSIONFORCE: {0}", PlayerManager.Instance.LocalPlayer.CharacterMotor.reportExplosionForce));
                }
                else if (args[0].ToString() == "cjdir")
                {
                    UnityEngine.Debug.Log(string.Format("CJUMPDIR: {0}", PlayerManager.Instance.LocalPlayer.CharacterMotor.jumping.jumpDir));
                }
                else if (args[0].ToString() == "cjump")
                {
                    UnityEngine.Debug.Log(string.Format("CJUMP: {0} extra:{1}", PlayerManager.Instance.LocalPlayer.CharacterMotor.jumping.baseHeight, PlayerManager.Instance.LocalPlayer.CharacterMotor.jumping.extraHeight));
                }
                else if (args[0].ToString() == "tps")
                {
                    UnityEngine.Debug.Log(string.Format("TPS: {0}", NetworkDev.TPS));
                }
                else if (args[0].ToString() == "server.console")
                {
                    MasterServerNetworkController.SendConsole((string[])args);
                    UnityEngine.Debug.Log(string.Format("TPS: {0}", NetworkDev.TPS));
                }
                else if (args[0].ToString() == "stat")
                {
                    MasterServerMonitor.Instance.ProcessCommand(args);
                }
                else if (args[0].ToString() == "gstat")
                {
                    GameLogicServerMonitor.Instance.ProcessCommand(args);
                }
                else if (args[0].ToString() == "gl")
                {
                    if (args.Length <= 1)
                    {
                        UnityEngine.Debug.Log("GM Commands:");
                        UnityEngine.Debug.Log("  -ls");
                        UnityEngine.Debug.Log("  -c");
                    }
                    if (args[1].ToString() == "-ls")
                    {
                        UnityEngine.Debug.Log(string.Format("List GL [{0}]:", ServerConf.GMList.Count));
                        List<ServerItem>.Enumerator enumerator = ServerConf.GMList.GetEnumerator();
                        try
                        {
                            while (enumerator.MoveNext())
                            {
                                ServerItem current = enumerator.Current;
                                UnityEngine.Debug.Log(string.Format("\t{0}:{1}", current.Host, current.Ports[0]));
                            }
                        }
                        finally
                        {
                            ((IDisposable)enumerator).Dispose();
                        }
                    }
                    if (args[1].ToString() == "-c")
                    {
                        UnityEngine.Debug.Log(string.Format("Current GL: {0}:{1}  state:{2}", GameLogicServerNetworkController.GameLogicServer.Host, GameLogicServerNetworkController.GameLogicServer.Ports[0], GameLogicServerNetworkController.GameLogicServer.IsConnected));
                    }
                    if (args[1].ToString() == "-rc" && args.Length > 2)
                    {
                        GameLogicServerNetworkController.ManualReconnectGameLogicServer(Convert.ToInt32(args[2]));
                        UnityEngine.Debug.Log(string.Format("Current GL: {0}", GameLogicServerNetworkController.GameLogicServer.Host));
                    }
                }
                else if (args[0].ToString() == "spl")
                {
                    UnityEngine.Debug.Log("Total Player On Server: " + GUIRoomList.TotalPlayers);
                }
                else if (args[0].ToString() == "rsp")
                {
                    UnityEngine.Debug.Log("Player RSP: " + PlayerManager.Instance.LocalPlayer.SoldierController.runSpeed);
                }
                else if (args[0].ToString() == "gui_msg")
                {
                    if (args.Length == 0)
                    {
                        UnityEngine.Debug.LogError("need message code");
                    }
                    else
                    {
                        GameHUD.Instance.Message((GameHUDMessageType)(byte)Convert.ToInt32(args[1]));
                    }
                }
                else if (!args[0].ToString().StartsWith("password"))
                {
                    if (args[0].ToString().StartsWith("gfx"))
                    {
                        if (args[0].ToString() == "gfx.pixelLightCount" && args.Length > 1)
                        {
                            QualitySettings.pixelLightCount = Convert.ToInt32(args[1].ToString());
                            UnityEngine.Debug.LogWarning("\tgfx.pixelLightCount = " + QualitySettings.pixelLightCount);
                        }
                        else if (args[0].ToString() == "gfx.shadowProjection" && args.Length > 1)
                        {
                            if (args[1].ToString() == "CloseFit" && args.Length > 1)
                            {
                                QualitySettings.shadowProjection = ShadowProjection.CloseFit;
                            }
                            else if (args[1].ToString() == "StableFit" && args.Length > 1)
                            {
                                QualitySettings.shadowProjection = ShadowProjection.StableFit;
                            }
                            UnityEngine.Debug.LogWarning("\tgfx.shadowProjection = " + QualitySettings.shadowProjection);
                        }
                        else if (args[0].ToString() == "gfx.shadowCascades" && args.Length > 1)
                        {
                            QualitySettings.shadowCascades = Convert.ToInt32(args[1].ToString());
                            UnityEngine.Debug.LogWarning("\tgfx.shadowCascades = " + QualitySettings.shadowCascades);
                        }
                        else if (args[0].ToString() == "gfx.shadowDistance" && args.Length > 1)
                        {
                            QualitySettings.shadowDistance = (float)Convert.ToInt32(args[1].ToString());
                            UnityEngine.Debug.LogWarning("\tgfx.shadowDistance = " + QualitySettings.shadowDistance);
                        }
                        else if (args[0].ToString() == "gfx.masterTextureLimit" && args.Length > 1)
                        {
                            QualitySettings.masterTextureLimit = Convert.ToInt32(args[1].ToString());
                            UnityEngine.Debug.LogWarning("\tgfx.masterTextureLimit = " + QualitySettings.masterTextureLimit);
                        }
                        else if (args[0].ToString() == "gfx.anisotropicFiltering" && args.Length > 1)
                        {
                            if (args[1].ToString() == "Disable" && args.Length > 1)
                            {
                                QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                            }
                            else if (args[1].ToString() == "Enable" && args.Length > 1)
                            {
                                QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
                            }
                            else if (args[1].ToString() == "ForceEnable" && args.Length > 1)
                            {
                                QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                            }
                            UnityEngine.Debug.LogWarning("\tgfx.anisotropicFiltering = " + QualitySettings.anisotropicFiltering);
                        }
                        else if (args[0].ToString() == "gfx.lodBias" && args.Length > 1)
                        {
                            QualitySettings.lodBias = Convert.ToSingle(args[1].ToString());
                            UnityEngine.Debug.LogWarning("\tgfx.maximumLODLevel = " + QualitySettings.lodBias);
                        }
                        else if (args[0].ToString() == "gfx.maximumLODLevel" && args.Length > 1)
                        {
                            QualitySettings.maximumLODLevel = Convert.ToInt32(args[1].ToString());
                            UnityEngine.Debug.LogWarning("\tgfx.maximumLODLevel = " + QualitySettings.maximumLODLevel);
                        }
                        else if (args[0].ToString() == "gfx.particleRaycastBudget" && args.Length > 1)
                        {
                            QualitySettings.particleRaycastBudget = Convert.ToInt32(args[1].ToString());
                            UnityEngine.Debug.LogWarning("\tgfx.particleRaycastBudget = " + QualitySettings.particleRaycastBudget);
                        }
                        else if (args[0].ToString() == "gfx.softVegetation" && args.Length > 1)
                        {
                            QualitySettings.softVegetation = Convert.ToBoolean(args[1].ToString());
                            UnityEngine.Debug.LogWarning("\tgfx.softVegetation = " + QualitySettings.softVegetation);
                        }
                        else if (args[0].ToString() == "gfx.maxQueuedFrames" && args.Length > 1)
                        {
                            QualitySettings.maxQueuedFrames = Convert.ToInt32(args[1].ToString());
                            UnityEngine.Debug.LogWarning("\tgfx.maxQueuedFrames = " + QualitySettings.maxQueuedFrames);
                        }
                        else if (args[0].ToString() == "gfx.vSyncCount" && args.Length > 1)
                        {
                            QualitySettings.vSyncCount = Convert.ToInt32(args[1].ToString());
                            UnityEngine.Debug.LogWarning("\tgfx.vSyncCount = " + QualitySettings.vSyncCount);
                        }
                        else if (args[0].ToString() == "gfx.antiAliasing" && args.Length > 1)
                        {
                            QualitySettings.antiAliasing = Convert.ToInt32(args[1].ToString());
                            UnityEngine.Debug.LogWarning("\tgfx.antiAliasing = " + QualitySettings.antiAliasing);
                        }
                        else if (args[0].ToString() == "gfx.blendWeights" && args.Length > 1)
                        {
                            if (args[1].ToString() == "OneBone" && args.Length > 1)
                            {
                                QualitySettings.blendWeights = BlendWeights.OneBone;
                            }
                            else if (args[1].ToString() == "TwoBones" && args.Length > 1)
                            {
                                QualitySettings.blendWeights = BlendWeights.TwoBones;
                            }
                            else if (args[1].ToString() == "FourBones" && args.Length > 1)
                            {
                                QualitySettings.blendWeights = BlendWeights.FourBones;
                            }
                            UnityEngine.Debug.LogWarning("\tgfx.blendWeights = " + QualitySettings.blendWeights);
                        }
                        else if (args[0].ToString() == "gfx.help")
                        {
                            UnityEngine.Debug.LogWarning("\t\tgfx.help");
                            UnityEngine.Debug.LogWarning("\tgfx.pixelLightCount 1...100");
                            UnityEngine.Debug.LogWarning("\tgfx.shadowProjection  CloseFit / StableFit");
                            UnityEngine.Debug.LogWarning("\tgfx.shadowCascades 1...4");
                            UnityEngine.Debug.LogWarning("\tgfx.shadowDistance 0...1000");
                            UnityEngine.Debug.LogWarning("\tgfx.masterTextureLimit 0...10");
                            UnityEngine.Debug.LogWarning("\tgfx.anisotropicFiltering Disable / Enable / ForceEnable");
                            UnityEngine.Debug.LogWarning("\tgfx.lodBias 0...1");
                            UnityEngine.Debug.LogWarning("\tgfx.maximumLODLevel 0...7");
                            UnityEngine.Debug.LogWarning("\tgfx.particleRaycastBudget 0...1000");
                            UnityEngine.Debug.LogWarning("\tgfx.softVegetation true / false");
                            UnityEngine.Debug.LogWarning("\tgfx.maxQueuedFrames 0...10");
                            UnityEngine.Debug.LogWarning("\tgfx.vSyncCount 0...2");
                            UnityEngine.Debug.LogWarning("\tgfx.antiAliasing 0 / 2 / 4 / 8");
                            UnityEngine.Debug.LogWarning("\tgfx.blendWeights OneBone / TwoBones / FourBones");
                        }
                        else
                        {
                            UnityEngine.Debug.LogWarning("\t\tgfx.info");
                            UnityEngine.Debug.LogWarning("\tgfx.pixelLightCount = " + QualitySettings.pixelLightCount);
                            UnityEngine.Debug.LogWarning("\tgfx.shadowProjection = " + QualitySettings.shadowProjection);
                            UnityEngine.Debug.LogWarning("\tgfx.shadowCascades = " + QualitySettings.shadowCascades);
                            UnityEngine.Debug.LogWarning("\tgfx.shadowDistance = " + QualitySettings.shadowDistance);
                            UnityEngine.Debug.LogWarning("\tgfx.masterTextureLimit = " + QualitySettings.masterTextureLimit);
                            UnityEngine.Debug.LogWarning("\tgfx.anisotropicFiltering = " + QualitySettings.anisotropicFiltering);
                            UnityEngine.Debug.LogWarning("\tgfx.lodBias = " + QualitySettings.lodBias);
                            UnityEngine.Debug.LogWarning("\tgfx.maximumLODLevel = " + QualitySettings.maximumLODLevel);
                            UnityEngine.Debug.LogWarning("\tgfx.particleRaycastBudget = " + QualitySettings.particleRaycastBudget);
                            UnityEngine.Debug.LogWarning("\tgfx.softVegetation = " + QualitySettings.softVegetation);
                            UnityEngine.Debug.LogWarning("\tgfx.maxQueuedFrames = " + QualitySettings.maxQueuedFrames);
                            UnityEngine.Debug.LogWarning("\tgfx.vSyncCount = " + QualitySettings.vSyncCount);
                            UnityEngine.Debug.LogWarning("\tgfx.antiAliasing = " + QualitySettings.antiAliasing);
                            UnityEngine.Debug.LogWarning("\tgfx.desiredColorSpace = " + QualitySettings.desiredColorSpace);
                            UnityEngine.Debug.LogWarning("\tgfx.activeColorSpace = " + QualitySettings.activeColorSpace);
                            UnityEngine.Debug.LogWarning("\tgfx.blendWeights = " + QualitySettings.blendWeights);
                        }
                    }
                    else
                    {
                        UnityEngine.Debug.Log("[DebugConsole] unkown command");
                    }
                }
            }
        }
    }

    public void Log(string message, string stackTrace, LogType type)
    {
        ConsoleMessage item = new ConsoleMessage(DateTime.Now.ToLongTimeString() + ": " + message, stackTrace, type);
        this.entries.Add(item);
        if (this.auto_scroll)
        {
            this.scrollPos.y += 100f;
        }
        if (this.entries.Count > 100)
        {
            this.entries.RemoveRange(0, 10);
        }
    }

    private void HandleLog(string message, string stackTrace, LogType type)
    {
        ConsoleMessage item = new ConsoleMessage(DateTime.Now.ToLongTimeString() + ": " + message, stackTrace, type);
        this.entries.Add(item);
        if (this.auto_scroll)
        {
            this.scrollPos.y += 100f;
        }
        if (this.entries.Count > 100)
        {
            this.entries.RemoveRange(0, 10);
        }
    }
}


