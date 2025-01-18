// ILSpyBased#2
using System;
using UnityEngine;

public class GameHUDFPS : MonoBehaviour
{
    public float maxUpdateInterval = 30f;

    private float maxAccum;

    private int maxFrames;

    private float maxTimeleft;

    private string maxFormat;

    public float updateInterval = 5f;

    private float accum;

    private int frames;

    private float timeleft;

    public GUIStyle fpsTextStyle;

    private string format;

    private string[] debugLine = new string[5];

    public static GameHUDFPS instance;

    public static GameHUDFPS Instance
    {
        get
        {
            return GameHUDFPS.instance;
        }
    }

    public int getFrames()
    {
        return this.frames;
    }

    private void Start()
    {
        this.timeleft = this.updateInterval;
        GameHUDFPS.instance = this;
        for (int i = 0; i < this.debugLine.Length; i++)
        {
            this.debugLine[i] = string.Empty;
        }
    }

    private void OnGUI()
    {
        if (Configuration.DebugEnableFps)
        {
            if ((UnityEngine.Object)TimeManager.Instance != (UnityEngine.Object)null)
            {
                GUI.Label(new Rect(5f, 0f, 300f, 20f), string.Format("FPS: {0}", this.format), GUISkinManager.DebugSkin.label);
                for (int i = 0; i < this.debugLine.Length; i++)
                {
                    if (this.debugLine[i] != string.Empty)
                    {
                        GUI.Label(new Rect(5f, (float)(i * 20 + 20), 800f, 20f), string.Format("Debug: {0}", this.debugLine[i]), GUISkinManager.DebugSkin.label);
                    }
                }
            }
            else
            {
                GUI.Label(new Rect(5f, 0f, 300f, 20f), "FPS: " + this.format, GUISkinManager.DebugSkin.label);
            }
        }
    }

    public void SetDebugLine(string debugLine)
    {
        for (int i = 0; i < this.debugLine.Length; i++)
        {
            this.debugLine[i] = string.Empty;
        }
        this.debugLine[0] = debugLine;
    }

    public void SetDebugLine(string debugLine, int index)
    {
        this.debugLine[index] = debugLine;
    }

    private void Update()
    {
        if (Configuration.DebugEnableFps)
        {
            float num = Time.timeScale / Time.deltaTime;
            this.timeleft -= Time.deltaTime;
            this.accum += num;
            this.frames++;
            if ((double)this.timeleft <= 0.0)
            {
                float num2 = this.accum / (float)this.frames;
                this.format = string.Format("{0}", Math.Ceiling((double)num2));
                this.timeleft = this.updateInterval;
                this.accum = 0f;
                this.frames = 0;
            }
            this.maxTimeleft -= Time.deltaTime;
            this.maxAccum += num;
            this.maxFrames++;
            if ((double)this.maxTimeleft <= 0.0)
            {
                float num3 = this.maxAccum / (float)this.maxFrames;
                this.maxFormat = string.Format("{0:F2}", num3);
                this.maxTimeleft = this.maxUpdateInterval;
                this.maxAccum = 0f;
                this.maxFrames = 0;
            }
        }
    }
}


