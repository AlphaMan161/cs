// ILSpyBased#2
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iTween : MonoBehaviour
{
    static Dictionary<string, int> _003C_003Ef__switch_0024mapC;
    static Dictionary<string, int> _003C_003Ef__switch_0024mapB;
    static Dictionary<string, int> _003C_003Ef__switch_0024map9;
    static Dictionary<string, int> _003C_003Ef__switch_0024map8;
    static Dictionary<string, int> _003C_003Ef__switch_0024map7;
    static Dictionary<string, int> _003C_003Ef__switch_0024map6;
    static Dictionary<string, int> _003C_003Ef__switch_0024map5;
    static Dictionary<string, int> _003C_003Ef__switch_0024map4;
    static Dictionary<string, int> _003C_003Ef__switch_0024map3;
    static Dictionary<string, int> _003C_003Ef__switch_0024map2;
    static Dictionary<string, int> _003C_003Ef__switch_0024map1;
    static Dictionary<string, int> _003C_003Ef__switch_0024mapA;
    static Dictionary<string, int> _003C_003Ef__switch_0024map0;
    public enum EaseType
    {
        easeInQuad,
        easeOutQuad,
        easeInOutQuad,
        easeInCubic,
        easeOutCubic,
        easeInOutCubic,
        easeInQuart,
        easeOutQuart,
        easeInOutQuart,
        easeInQuint,
        easeOutQuint,
        easeInOutQuint,
        easeInSine,
        easeOutSine,
        easeInOutSine,
        easeInExpo,
        easeOutExpo,
        easeInOutExpo,
        easeInCirc,
        easeOutCirc,
        easeInOutCirc,
        linear,
        spring,
        easeInBounce,
        easeOutBounce,
        easeInOutBounce,
        easeInBack,
        easeOutBack,
        easeInOutBack,
        easeInElastic,
        easeOutElastic,
        easeInOutElastic,
        punch
    }

    public enum LoopType
    {
        none,
        loop,
        pingPong
    }

    public enum NamedValueColor
    {
        _Color,
        _SpecColor,
        _Emission,
        _ReflectColor
    }

    public static class Defaults
    {
        public static float time = 1f;

        public static float delay = 0f;

        public static NamedValueColor namedColorValue = NamedValueColor._Color;

        public static LoopType loopType = LoopType.none;

        public static EaseType easeType = EaseType.easeOutExpo;

        public static float lookSpeed = 3f;

        public static bool isLocal = false;

        public static Space space = Space.Self;

        public static bool orientToPath = false;

        public static Color color = Color.white;

        public static float updateTimePercentage = 0.05f;

        public static float updateTime = 1f * Defaults.updateTimePercentage;

        public static int cameraFadeDepth = 999999;

        public static float lookAhead = 0.05f;

        public static bool useRealTime = false;

        public static Vector3 up = Vector3.up;
    }

    private class CRSpline
    {
        public Vector3[] pts;

        public CRSpline(params Vector3[] pts)
        {
            this.pts = new Vector3[pts.Length];
            Array.Copy(pts, this.pts, pts.Length);
        }

        public Vector3 Interp(float t)
        {
            int num = this.pts.Length - 3;
            int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
            float num3 = t * (float)num - (float)num2;
            Vector3 a = this.pts[num2];
            Vector3 a2 = this.pts[num2 + 1];
            Vector3 vector = this.pts[num2 + 2];
            Vector3 b = this.pts[num2 + 3];
            return 0.5f * ((-a + 3f * a2 - 3f * vector + b) * (num3 * num3 * num3) + (2f * a - 5f * a2 + 4f * vector - b) * (num3 * num3) + (-a + vector) * num3 + 2f * a2);
        }
    }

    private delegate float EasingFunction(float start, float end, float Value);

    private delegate void ApplyTween();

    public static List<Hashtable> tweens = new List<Hashtable>();

    private static GameObject cameraFade;

    public string id;

    public string type;

    public string method;

    public EaseType easeType;

    public float time;

    public float delay;

    public LoopType loopType;

    public bool isRunning;

    public bool isPaused;

    public string _name;

    private float runningTime;

    private float percentage;

    private float delayStarted;

    private bool kinematic;

    private bool isLocal;

    private bool loop;

    private bool reverse;

    private bool wasPaused;

    private bool physics;

    private Hashtable tweenArguments;

    private Space space;

    private EasingFunction ease;

    private ApplyTween apply;

    private AudioSource audioSource;

    private Vector3[] vector3s;

    private Vector2[] vector2s;

    private Color[,] colors;

    private float[] floats;

    private Rect[] rects;

    private CRSpline path;

    private Vector3 preUpdate;

    private Vector3 postUpdate;

    private NamedValueColor namedcolorvalue;

    private float lastRealTime;

    private bool useRealTime;

    private Transform thisTransform;

    private iTween(Hashtable h)
    {
        this.tweenArguments = h;
    }

    public static void Init(GameObject target)
    {
        iTween.MoveBy(target, Vector3.zero, 0f);
    }

    public static void CameraFadeFrom(float amount, float time)
    {
        if ((bool)iTween.cameraFade)
        {
            iTween.CameraFadeFrom(iTween.Hash("amount", amount, "time", time));
        }
        else
        {
            UnityEngine.Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
        }
    }

    public static void CameraFadeFrom(Hashtable args)
    {
        if ((bool)iTween.cameraFade)
        {
            iTween.ColorFrom(iTween.cameraFade, args);
        }
        else
        {
            UnityEngine.Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
        }
    }

    public static void CameraFadeTo(float amount, float time)
    {
        if ((bool)iTween.cameraFade)
        {
            iTween.CameraFadeTo(iTween.Hash("amount", amount, "time", time));
        }
        else
        {
            UnityEngine.Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
        }
    }

    public static void CameraFadeTo(Hashtable args)
    {
        if ((bool)iTween.cameraFade)
        {
            iTween.ColorTo(iTween.cameraFade, args);
        }
        else
        {
            UnityEngine.Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
        }
    }

    public static void ValueTo(GameObject target, Hashtable args)
    {
        args = iTween.CleanArgs(args);
        if (!args.Contains("onupdate") || !args.Contains("from") || !args.Contains("to"))
        {
            UnityEngine.Debug.LogError("iTween Error: ValueTo() requires an 'onupdate' callback function and a 'from' and 'to' property.  The supplied 'onupdate' callback must accept a single argument that is the same type as the supplied 'from' and 'to' properties!");
        }
        else
        {
            args["type"] = "value";
            if (args["from"].GetType() == typeof(Vector2))
            {
                args["method"] = "vector2";
                goto IL_0162;
            }
            if (args["from"].GetType() == typeof(Vector3))
            {
                args["method"] = "vector3";
                goto IL_0162;
            }
            if (args["from"].GetType() == typeof(Rect))
            {
                args["method"] = "rect";
                goto IL_0162;
            }
            if (args["from"].GetType() == typeof(float))
            {
                args["method"] = "float";
                goto IL_0162;
            }
            if (args["from"].GetType() == typeof(Color))
            {
                args["method"] = "color";
                goto IL_0162;
            }
            UnityEngine.Debug.LogError("iTween Error: ValueTo() only works with interpolating Vector3s, Vector2s, floats, ints, Rects and Colors!");
        }
        return;
        IL_0162:
        if (!args.Contains("easetype"))
        {
            args.Add("easetype", EaseType.linear);
        }
        iTween.Launch(target, args);
    }

    public static void FadeFrom(GameObject target, float alpha, float time)
    {
        iTween.FadeFrom(target, iTween.Hash("alpha", alpha, "time", time));
    }

    public static void FadeFrom(GameObject target, Hashtable args)
    {
        iTween.ColorFrom(target, args);
    }

    public static void FadeTo(GameObject target, float alpha, float time)
    {
        iTween.FadeTo(target, iTween.Hash("alpha", alpha, "time", time));
    }

    public static void FadeTo(GameObject target, Hashtable args)
    {
        iTween.ColorTo(target, args);
    }

    public static void ColorFrom(GameObject target, Color color, float time)
    {
        iTween.ColorFrom(target, iTween.Hash("color", color, "time", time));
    }

    public static void ColorFrom(GameObject target, Hashtable args)
    {
        Color color = default(Color);
        Color color2 = default(Color);
        args = iTween.CleanArgs(args);
        if (!args.Contains("includechildren") || (bool)args["includechildren"])
        {
            foreach (Transform item in target.transform)
            {
                Hashtable hashtable = (Hashtable)args.Clone();
                hashtable["ischild"] = true;
                iTween.ColorFrom(item.gameObject, hashtable);
            }
        }
        if (!args.Contains("easetype"))
        {
            args.Add("easetype", EaseType.linear);
        }
        if ((bool)target.GetComponent<GUITexture>())
        {
            color2 = (color = target.GetComponent<GUITexture>().color);
        }
        else if ((bool)target.GetComponent<GUIText>())
        {
            color2 = (color = target.GetComponent<GUIText>().material.color);
        }
        else if ((bool)target.GetComponent<Renderer>())
        {
            color2 = (color = target.GetComponent<Renderer>().material.color);
        }
        else if ((bool)target.GetComponent<Light>())
        {
            color2 = (color = target.GetComponent<Light>().color);
        }
        if (args.Contains("color"))
        {
            color = (Color)args["color"];
        }
        else
        {
            if (args.Contains("r"))
            {
                color.r = (float)args["r"];
            }
            if (args.Contains("g"))
            {
                color.g = (float)args["g"];
            }
            if (args.Contains("b"))
            {
                color.b = (float)args["b"];
            }
            if (args.Contains("a"))
            {
                color.a = (float)args["a"];
            }
        }
        if (args.Contains("amount"))
        {
            color.a = (float)args["amount"];
            args.Remove("amount");
        }
        else if (args.Contains("alpha"))
        {
            color.a = (float)args["alpha"];
            args.Remove("alpha");
        }
        if ((bool)target.GetComponent<GUITexture>())
        {
            target.GetComponent<GUITexture>().color = color;
        }
        else if ((bool)target.GetComponent<GUIText>())
        {
            target.GetComponent<GUIText>().material.color = color;
        }
        else if ((bool)target.GetComponent<Renderer>())
        {
            target.GetComponent<Renderer>().material.color = color;
        }
        else if ((bool)target.GetComponent<Light>())
        {
            target.GetComponent<Light>().color = color;
        }
        args["color"] = color2;
        args["type"] = "color";
        args["method"] = "to";
        iTween.Launch(target, args);
    }

    public static void ColorTo(GameObject target, Color color, float time)
    {
        iTween.ColorTo(target, iTween.Hash("color", color, "time", time));
    }

    public static void ColorTo(GameObject target, Hashtable args)
    {
        args = iTween.CleanArgs(args);
        if (!args.Contains("includechildren") || (bool)args["includechildren"])
        {
            foreach (Transform item in target.transform)
            {
                Hashtable hashtable = (Hashtable)args.Clone();
                hashtable["ischild"] = true;
                iTween.ColorTo(item.gameObject, hashtable);
            }
        }
        if (!args.Contains("easetype"))
        {
            args.Add("easetype", EaseType.linear);
        }
        args["type"] = "color";
        args["method"] = "to";
        iTween.Launch(target, args);
    }

    public static void AudioFrom(GameObject target, float volume, float pitch, float time)
    {
        iTween.AudioFrom(target, iTween.Hash("volume", volume, "pitch", pitch, "time", time));
    }

    public static void AudioFrom(GameObject target, Hashtable args)
    {
        args = iTween.CleanArgs(args);
        AudioSource audioSource;
        if (args.Contains("audiosource"))
        {
            audioSource = (AudioSource)args["audiosource"];
            goto IL_0055;
        }
        if ((bool)target.GetComponent<AudioSource>())
        {
            audioSource = target.GetComponent<AudioSource>();
            goto IL_0055;
        }
        UnityEngine.Debug.LogError("iTween Error: AudioFrom requires an AudioSource.");
        return;
        IL_0055:
        Vector2 vector = default(Vector2);
        Vector2 vector2 = default(Vector2);
        vector2.x = audioSource.volume; vector.x = (vector2.x );
        vector2.y = audioSource.pitch; vector.y = (vector2.y );
        if (args.Contains("volume"))
        {
            vector2.x = (float)args["volume"];
        }
        if (args.Contains("pitch"))
        {
            vector2.y = (float)args["pitch"];
        }
        audioSource.volume = vector2.x;
        audioSource.pitch = vector2.y;
        args["volume"] = vector.x;
        args["pitch"] = vector.y;
        if (!args.Contains("easetype"))
        {
            args.Add("easetype", EaseType.linear);
        }
        args["type"] = "audio";
        args["method"] = "to";
        iTween.Launch(target, args);
    }

    public static void AudioTo(GameObject target, float volume, float pitch, float time)
    {
        iTween.AudioTo(target, iTween.Hash("volume", volume, "pitch", pitch, "time", time));
    }

    public static void AudioTo(GameObject target, Hashtable args)
    {
        args = iTween.CleanArgs(args);
        if (!args.Contains("easetype"))
        {
            args.Add("easetype", EaseType.linear);
        }
        args["type"] = "audio";
        args["method"] = "to";
        iTween.Launch(target, args);
    }

    public static void Stab(GameObject target, AudioClip audioclip, float delay)
    {
        iTween.Stab(target, iTween.Hash("audioclip", audioclip, "delay", delay));
    }

    public static void Stab(GameObject target, Hashtable args)
    {
        args = iTween.CleanArgs(args);
        args["type"] = "stab";
        iTween.Launch(target, args);
    }

    public static void LookFrom(GameObject target, Vector3 looktarget, float time)
    {
        iTween.LookFrom(target, iTween.Hash("looktarget", looktarget, "time", time));
    }

    public static void LookFrom(GameObject target, Hashtable args)
    {
        args = iTween.CleanArgs(args);
        Vector3 eulerAngles = target.transform.eulerAngles;
        if (args["looktarget"].GetType() == typeof(Transform))
        {
            Transform transform = target.transform;
            Transform target2 = (Transform)args["looktarget"];
            Vector3? nullable = (Vector3?)args["up"];
            transform.LookAt(target2, (!nullable.HasValue) ? Defaults.up : nullable.Value);
        }
        else if (args["looktarget"].GetType() == typeof(Vector3))
        {
            Transform transform2 = target.transform;
            Vector3 worldPosition = (Vector3)args["looktarget"];
            Vector3? nullable2 = (Vector3?)args["up"];
            transform2.LookAt(worldPosition, (!nullable2.HasValue) ? Defaults.up : nullable2.Value);
        }
        if (args.Contains("axis"))
        {
            Vector3 eulerAngles2 = target.transform.eulerAngles;
            string text = (string)args["axis"];
            if (text != null)
            {
                if (iTween._003C_003Ef__switch_0024map0 == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
                    dictionary.Add("x", 0);
                    dictionary.Add("y", 1);
                    dictionary.Add("z", 2);
                    iTween._003C_003Ef__switch_0024map0 = dictionary;
                }
                int num = default(int);
                if (iTween._003C_003Ef__switch_0024map0.TryGetValue(text, out num))
                {
                    switch (num)
                    {
                        case 0:
                            eulerAngles2.y = eulerAngles.y;
                            eulerAngles2.z = eulerAngles.z;
                            break;
                        case 1:
                            eulerAngles2.x = eulerAngles.x;
                            eulerAngles2.z = eulerAngles.z;
                            break;
                        case 2:
                            eulerAngles2.x = eulerAngles.x;
                            eulerAngles2.y = eulerAngles.y;
                            break;
                    }
                }
            }
            target.transform.eulerAngles = eulerAngles2;
        }
        args["rotation"] = eulerAngles;
        args["type"] = "rotate";
        args["method"] = "to";
        iTween.Launch(target, args);
    }

    public static void LookTo(GameObject target, Vector3 looktarget, float time)
    {
        iTween.LookTo(target, iTween.Hash("looktarget", looktarget, "time", time));
    }

    public static void LookTo(GameObject target, Hashtable args)
    {
        args = iTween.CleanArgs(args);
        if (args.Contains("looktarget") && args["looktarget"].GetType() == typeof(Transform))
        {
            Transform transform = (Transform)args["looktarget"];
            Hashtable hashtable = args;
            Vector3 position = transform.position;
            float x = position.x;
            Vector3 position2 = transform.position;
            float y = position2.y;
            Vector3 position3 = transform.position;
            hashtable["position"] = new Vector3(x, y, position3.z);
            Hashtable hashtable2 = args;
            Vector3 eulerAngles = transform.eulerAngles;
            float x2 = eulerAngles.x;
            Vector3 eulerAngles2 = transform.eulerAngles;
            float y2 = eulerAngles2.y;
            Vector3 eulerAngles3 = transform.eulerAngles;
            hashtable2["rotation"] = new Vector3(x2, y2, eulerAngles3.z);
        }
        args["type"] = "look";
        args["method"] = "to";
        iTween.Launch(target, args);
    }

    public static void MoveTo(GameObject target, Vector3 position, float time)
    {
        iTween.MoveTo(target, iTween.Hash("position", position, "time", time));
    }

    public static void MoveTo(GameObject target, Hashtable args)
    {
        args = iTween.CleanArgs(args);
        if (args.Contains("position") && args["position"].GetType() == typeof(Transform))
        {
            Transform transform = (Transform)args["position"];
            Hashtable hashtable = args;
            Vector3 position = transform.position;
            float x = position.x;
            Vector3 position2 = transform.position;
            float y = position2.y;
            Vector3 position3 = transform.position;
            hashtable["position"] = new Vector3(x, y, position3.z);
            Hashtable hashtable2 = args;
            Vector3 eulerAngles = transform.eulerAngles;
            float x2 = eulerAngles.x;
            Vector3 eulerAngles2 = transform.eulerAngles;
            float y2 = eulerAngles2.y;
            Vector3 eulerAngles3 = transform.eulerAngles;
            hashtable2["rotation"] = new Vector3(x2, y2, eulerAngles3.z);
            Hashtable hashtable3 = args;
            Vector3 localScale = transform.localScale;
            float x3 = localScale.x;
            Vector3 localScale2 = transform.localScale;
            float y3 = localScale2.y;
            Vector3 localScale3 = transform.localScale;
            hashtable3["scale"] = new Vector3(x3, y3, localScale3.z);
        }
        args["type"] = "move";
        args["method"] = "to";
        iTween.Launch(target, args);
    }

    public static void MoveFrom(GameObject target, Vector3 position, float time)
    {
        iTween.MoveFrom(target, iTween.Hash("position", position, "time", time));
    }

    public static void MoveFrom(GameObject target, Hashtable args)
    {
        args = iTween.CleanArgs(args);
        bool flag = (!args.Contains("islocal")) ? Defaults.isLocal : ((bool)args["islocal"]);
        if (args.Contains("path"))
        {
            Vector3[] array2;
            if (args["path"].GetType() == typeof(Vector3[]))
            {
                Vector3[] array = (Vector3[])args["path"];
                array2 = new Vector3[array.Length];
                Array.Copy(array, array2, array.Length);
            }
            else
            {
                Transform[] array3 = (Transform[])args["path"];
                array2 = new Vector3[array3.Length];
                for (int i = 0; i < array3.Length; i++)
                {
                    array2[i] = array3[i].position;
                }
            }
            if (array2[array2.Length - 1] != target.transform.position)
            {
                Vector3[] array4 = new Vector3[array2.Length + 1];
                Array.Copy(array2, array4, array2.Length);
                if (flag)
                {
                    array4[array4.Length - 1] = target.transform.localPosition;
                    target.transform.localPosition = array4[0];
                }
                else
                {
                    array4[array4.Length - 1] = target.transform.position;
                    target.transform.position = array4[0];
                }
                args["path"] = array4;
            }
            else
            {
                if (flag)
                {
                    target.transform.localPosition = array2[0];
                }
                else
                {
                    target.transform.position = array2[0];
                }
                args["path"] = array2;
            }
        }
        else
        {
            Vector3 vector;
            vector = target.transform.position; vector = target.transform.localPosition; Vector3 vector2 = (!flag) ? (vector ) : (vector );
            if (args.Contains("position"))
            {
                if (args["position"].GetType() == typeof(Transform))
                {
                    Transform transform = (Transform)args["position"];
                    vector = transform.position;
                }
                else if (args["position"].GetType() == typeof(Vector3))
                {
                    vector = (Vector3)args["position"];
                }
            }
            else
            {
                if (args.Contains("x"))
                {
                    vector.x = (float)args["x"];
                }
                if (args.Contains("y"))
                {
                    vector.y = (float)args["y"];
                }
                if (args.Contains("z"))
                {
                    vector.z = (float)args["z"];
                }
            }
            if (flag)
            {
                target.transform.localPosition = vector;
            }
            else
            {
                target.transform.position = vector;
            }
            args["position"] = vector2;
        }
        args["type"] = "move";
        args["method"] = "to";
        iTween.Launch(target, args);
    }

    public static void MoveAdd(GameObject target, Vector3 amount, float time)
    {
        iTween.MoveAdd(target, iTween.Hash("amount", amount, "time", time));
    }

    public static void MoveAdd(GameObject target, Hashtable args)
    {
        args = iTween.CleanArgs(args);
        args["type"] = "move";
        args["method"] = "add";
        iTween.Launch(target, args);
    }

    public static void MoveBy(GameObject target, Vector3 amount, float time)
    {
        iTween.MoveBy(target, iTween.Hash("amount", amount, "time", time));
    }

    public static void MoveBy(GameObject target, Hashtable args)
    {
        args = iTween.CleanArgs(args);
        args["type"] = "move";
        args["method"] = "by";
        iTween.Launch(target, args);
    }

    public static void ScaleTo(GameObject target, Vector3 scale, float time)
    {
        iTween.ScaleTo(target, iTween.Hash("scale", scale, "time", time));
    }

    public static void ScaleTo(GameObject target, Hashtable args)
    {
        args = iTween.CleanArgs(args);
        if (args.Contains("scale") && args["scale"].GetType() == typeof(Transform))
        {
            Transform transform = (Transform)args["scale"];
            Hashtable hashtable = args;
            Vector3 position = transform.position;
            float x = position.x;
            Vector3 position2 = transform.position;
            float y = position2.y;
            Vector3 position3 = transform.position;
            hashtable["position"] = new Vector3(x, y, position3.z);
            Hashtable hashtable2 = args;
            Vector3 eulerAngles = transform.eulerAngles;
            float x2 = eulerAngles.x;
            Vector3 eulerAngles2 = transform.eulerAngles;
            float y2 = eulerAngles2.y;
            Vector3 eulerAngles3 = transform.eulerAngles;
            hashtable2["rotation"] = new Vector3(x2, y2, eulerAngles3.z);
            Hashtable hashtable3 = args;
            Vector3 localScale = transform.localScale;
            float x3 = localScale.x;
            Vector3 localScale2 = transform.localScale;
            float y3 = localScale2.y;
            Vector3 localScale3 = transform.localScale;
            hashtable3["scale"] = new Vector3(x3, y3, localScale3.z);
        }
        args["type"] = "scale";
        args["method"] = "to";
        iTween.Launch(target, args);
    }

    public static void ScaleFrom(GameObject target, Vector3 scale, float time)
    {
        iTween.ScaleFrom(target, iTween.Hash("scale", scale, "time", time));
    }

    public static void ScaleFrom(GameObject target, Hashtable args)
    {
        args = iTween.CleanArgs(args);
        Vector3 localScale;
        Vector3 vector = localScale = target.transform.localScale;
        if (args.Contains("scale"))
        {
            if (args["scale"].GetType() == typeof(Transform))
            {
                Transform transform = (Transform)args["scale"];
                localScale = transform.localScale;
            }
            else if (args["scale"].GetType() == typeof(Vector3))
            {
                localScale = (Vector3)args["scale"];
            }
        }
        else
        {
            if (args.Contains("x"))
            {
                localScale.x = (float)args["x"];
            }
            if (args.Contains("y"))
            {
                localScale.y = (float)args["y"];
            }
            if (args.Contains("z"))
            {
                localScale.z = (float)args["z"];
            }
        }
        target.transform.localScale = localScale;
        args["scale"] = vector;
        args["type"] = "scale";
        args["method"] = "to";
        iTween.Launch(target, args);
    }

    public static void ScaleAdd(GameObject target, Vector3 amount, float time)
    {
        iTween.ScaleAdd(target, iTween.Hash("amount", amount, "time", time));
    }

    public static void ScaleAdd(GameObject target, Hashtable args)
    {
        args = iTween.CleanArgs(args);
        args["type"] = "scale";
        args["method"] = "add";
        iTween.Launch(target, args);
    }

    public static void ScaleBy(GameObject target, Vector3 amount, float time)
    {
        iTween.ScaleBy(target, iTween.Hash("amount", amount, "time", time));
    }

    public static void ScaleBy(GameObject target, Hashtable args)
    {
        args = iTween.CleanArgs(args);
        args["type"] = "scale";
        args["method"] = "by";
        iTween.Launch(target, args);
    }

    public static void RotateTo(GameObject target, Vector3 rotation, float time)
    {
        iTween.RotateTo(target, iTween.Hash("rotation", rotation, "time", time));
    }

    public static void RotateTo(GameObject target, Hashtable args)
    {
        args = iTween.CleanArgs(args);
        if (args.Contains("rotation") && args["rotation"].GetType() == typeof(Transform))
        {
            Transform transform = (Transform)args["rotation"];
            Hashtable hashtable = args;
            Vector3 position = transform.position;
            float x = position.x;
            Vector3 position2 = transform.position;
            float y = position2.y;
            Vector3 position3 = transform.position;
            hashtable["position"] = new Vector3(x, y, position3.z);
            Hashtable hashtable2 = args;
            Vector3 eulerAngles = transform.eulerAngles;
            float x2 = eulerAngles.x;
            Vector3 eulerAngles2 = transform.eulerAngles;
            float y2 = eulerAngles2.y;
            Vector3 eulerAngles3 = transform.eulerAngles;
            hashtable2["rotation"] = new Vector3(x2, y2, eulerAngles3.z);
            Hashtable hashtable3 = args;
            Vector3 localScale = transform.localScale;
            float x3 = localScale.x;
            Vector3 localScale2 = transform.localScale;
            float y3 = localScale2.y;
            Vector3 localScale3 = transform.localScale;
            hashtable3["scale"] = new Vector3(x3, y3, localScale3.z);
        }
        args["type"] = "rotate";
        args["method"] = "to";
        iTween.Launch(target, args);
    }

    public static void RotateFrom(GameObject target, Vector3 rotation, float time)
    {
        iTween.RotateFrom(target, iTween.Hash("rotation", rotation, "time", time));
    }

    public static void RotateFrom(GameObject target, Hashtable args)
    {
        args = iTween.CleanArgs(args);
        bool flag = (!args.Contains("islocal")) ? Defaults.isLocal : ((bool)args["islocal"]);
        Vector3 vector;
        vector = target.transform.eulerAngles; vector = target.transform.localEulerAngles; Vector3 vector2 = (!flag) ? (vector ) : (vector );
        if (args.Contains("rotation"))
        {
            if (args["rotation"].GetType() == typeof(Transform))
            {
                Transform transform = (Transform)args["rotation"];
                vector = transform.eulerAngles;
            }
            else if (args["rotation"].GetType() == typeof(Vector3))
            {
                vector = (Vector3)args["rotation"];
            }
        }
        else
        {
            if (args.Contains("x"))
            {
                vector.x = (float)args["x"];
            }
            if (args.Contains("y"))
            {
                vector.y = (float)args["y"];
            }
            if (args.Contains("z"))
            {
                vector.z = (float)args["z"];
            }
        }
        if (flag)
        {
            target.transform.localEulerAngles = vector;
        }
        else
        {
            target.transform.eulerAngles = vector;
        }
        args["rotation"] = vector2;
        args["type"] = "rotate";
        args["method"] = "to";
        iTween.Launch(target, args);
    }

    public static void RotateAdd(GameObject target, Vector3 amount, float time)
    {
        iTween.RotateAdd(target, iTween.Hash("amount", amount, "time", time));
    }

    public static void RotateAdd(GameObject target, Hashtable args)
    {
        args = iTween.CleanArgs(args);
        args["type"] = "rotate";
        args["method"] = "add";
        iTween.Launch(target, args);
    }

    public static void RotateBy(GameObject target, Vector3 amount, float time)
    {
        iTween.RotateBy(target, iTween.Hash("amount", amount, "time", time));
    }

    public static void RotateBy(GameObject target, Hashtable args)
    {
        args = iTween.CleanArgs(args);
        args["type"] = "rotate";
        args["method"] = "by";
        iTween.Launch(target, args);
    }

    public static void ShakePosition(GameObject target, Vector3 amount, float time)
    {
        iTween.ShakePosition(target, iTween.Hash("amount", amount, "time", time));
    }

    public static void ShakePosition(GameObject target, Hashtable args)
    {
        args = iTween.CleanArgs(args);
        args["type"] = "shake";
        args["method"] = "position";
        iTween.Launch(target, args);
    }

    public static void ShakeScale(GameObject target, Vector3 amount, float time)
    {
        iTween.ShakeScale(target, iTween.Hash("amount", amount, "time", time));
    }

    public static void ShakeScale(GameObject target, Hashtable args)
    {
        args = iTween.CleanArgs(args);
        args["type"] = "shake";
        args["method"] = "scale";
        iTween.Launch(target, args);
    }

    public static void ShakeRotation(GameObject target, Vector3 amount, float time)
    {
        iTween.ShakeRotation(target, iTween.Hash("amount", amount, "time", time));
    }

    public static void ShakeRotation(GameObject target, Hashtable args)
    {
        args = iTween.CleanArgs(args);
        args["type"] = "shake";
        args["method"] = "rotation";
        iTween.Launch(target, args);
    }

    public static void PunchPosition(GameObject target, Vector3 amount, float time)
    {
        iTween.PunchPosition(target, iTween.Hash("amount", amount, "time", time));
    }

    public static void PunchPosition(GameObject target, Hashtable args)
    {
        args = iTween.CleanArgs(args);
        args["type"] = "punch";
        args["method"] = "position";
        args["easetype"] = EaseType.punch;
        iTween.Launch(target, args);
    }

    public static void PunchRotation(GameObject target, Vector3 amount, float time)
    {
        iTween.PunchRotation(target, iTween.Hash("amount", amount, "time", time));
    }

    public static void PunchRotation(GameObject target, Hashtable args)
    {
        args = iTween.CleanArgs(args);
        args["type"] = "punch";
        args["method"] = "rotation";
        args["easetype"] = EaseType.punch;
        iTween.Launch(target, args);
    }

    public static void PunchScale(GameObject target, Vector3 amount, float time)
    {
        iTween.PunchScale(target, iTween.Hash("amount", amount, "time", time));
    }

    public static void PunchScale(GameObject target, Hashtable args)
    {
        args = iTween.CleanArgs(args);
        args["type"] = "punch";
        args["method"] = "scale";
        args["easetype"] = EaseType.punch;
        iTween.Launch(target, args);
    }

    private void GenerateTargets()
    {
        string text = this.type;
        if (text != null)
        {
            if (iTween._003C_003Ef__switch_0024mapA == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(10);
                dictionary.Add("value", 0);
                dictionary.Add("color", 1);
                dictionary.Add("audio", 2);
                dictionary.Add("move", 3);
                dictionary.Add("scale", 4);
                dictionary.Add("rotate", 5);
                dictionary.Add("shake", 6);
                dictionary.Add("punch", 7);
                dictionary.Add("look", 8);
                dictionary.Add("stab", 9);
                iTween._003C_003Ef__switch_0024mapA = dictionary;
            }
            int num = default(int);
            if (iTween._003C_003Ef__switch_0024mapA.TryGetValue(text, out num))
            {
                int num2 = default(int);
                switch (num)
                {
                    case 0:
                    {
                        string text2 = this.method;
                        if (text2 != null)
                        {
                            if (iTween._003C_003Ef__switch_0024map1 == null)
                            {
                                Dictionary<string, int> dictionary = new Dictionary<string, int>(5);
                                dictionary.Add("float", 0);
                                dictionary.Add("vector2", 1);
                                dictionary.Add("vector3", 2);
                                dictionary.Add("color", 3);
                                dictionary.Add("rect", 4);
                                iTween._003C_003Ef__switch_0024map1 = dictionary;
                            }
                            if (iTween._003C_003Ef__switch_0024map1.TryGetValue(text2, out num2))
                            {
                                switch (num2)
                                {
                                    case 0:
                                        this.GenerateFloatTargets();
                                        this.apply = new ApplyTween(this.ApplyFloatTargets);
                                        break;
                                    case 1:
                                        this.GenerateVector2Targets();
                                        this.apply = new ApplyTween(this.ApplyVector2Targets);
                                        break;
                                    case 2:
                                        this.GenerateVector3Targets();
                                        this.apply = new ApplyTween(this.ApplyVector3Targets);
                                        break;
                                    case 3:
                                        this.GenerateColorTargets();
                                        this.apply = new ApplyTween(this.ApplyColorTargets);
                                        break;
                                    case 4:
                                        this.GenerateRectTargets();
                                        this.apply = new ApplyTween(this.ApplyRectTargets);
                                        break;
                                }
                            }
                        }
                        break;
                    }
                    case 1:
                    {
                        string text2 = this.method;
                        if (text2 != null)
                        {
                            if (iTween._003C_003Ef__switch_0024map2 == null)
                            {
                                Dictionary<string, int> dictionary = new Dictionary<string, int>(1);
                                dictionary.Add("to", 0);
                                iTween._003C_003Ef__switch_0024map2 = dictionary;
                            }
                            if (iTween._003C_003Ef__switch_0024map2.TryGetValue(text2, out num2) && num2 == 0)
                            {
                                this.GenerateColorToTargets();
                                this.apply = new ApplyTween(this.ApplyColorToTargets);
                            }
                        }
                        break;
                    }
                    case 2:
                    {
                        string text2 = this.method;
                        if (text2 != null)
                        {
                            if (iTween._003C_003Ef__switch_0024map3 == null)
                            {
                                Dictionary<string, int> dictionary = new Dictionary<string, int>(1);
                                dictionary.Add("to", 0);
                                iTween._003C_003Ef__switch_0024map3 = dictionary;
                            }
                            if (iTween._003C_003Ef__switch_0024map3.TryGetValue(text2, out num2) && num2 == 0)
                            {
                                this.GenerateAudioToTargets();
                                this.apply = new ApplyTween(this.ApplyAudioToTargets);
                            }
                        }
                        break;
                    }
                    case 3:
                    {
                        string text2 = this.method;
                        if (text2 != null)
                        {
                            if (iTween._003C_003Ef__switch_0024map4 == null)
                            {
                                Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
                                dictionary.Add("to", 0);
                                dictionary.Add("by", 1);
                                dictionary.Add("add", 1);
                                iTween._003C_003Ef__switch_0024map4 = dictionary;
                            }
                            if (iTween._003C_003Ef__switch_0024map4.TryGetValue(text2, out num2))
                            {
                                switch (num2)
                                {
                                    case 0:
                                        if (this.tweenArguments.Contains("path"))
                                        {
                                            this.GenerateMoveToPathTargets();
                                            this.apply = new ApplyTween(this.ApplyMoveToPathTargets);
                                        }
                                        else
                                        {
                                            this.GenerateMoveToTargets();
                                            this.apply = new ApplyTween(this.ApplyMoveToTargets);
                                        }
                                        break;
                                    case 1:
                                        this.GenerateMoveByTargets();
                                        this.apply = new ApplyTween(this.ApplyMoveByTargets);
                                        break;
                                }
                            }
                        }
                        break;
                    }
                    case 4:
                    {
                        string text2 = this.method;
                        if (text2 != null)
                        {
                            if (iTween._003C_003Ef__switch_0024map5 == null)
                            {
                                Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
                                dictionary.Add("to", 0);
                                dictionary.Add("by", 1);
                                dictionary.Add("add", 2);
                                iTween._003C_003Ef__switch_0024map5 = dictionary;
                            }
                            if (iTween._003C_003Ef__switch_0024map5.TryGetValue(text2, out num2))
                            {
                                switch (num2)
                                {
                                    case 0:
                                        this.GenerateScaleToTargets();
                                        this.apply = new ApplyTween(this.ApplyScaleToTargets);
                                        break;
                                    case 1:
                                        this.GenerateScaleByTargets();
                                        this.apply = new ApplyTween(this.ApplyScaleToTargets);
                                        break;
                                    case 2:
                                        this.GenerateScaleAddTargets();
                                        this.apply = new ApplyTween(this.ApplyScaleToTargets);
                                        break;
                                }
                            }
                        }
                        break;
                    }
                    case 5:
                    {
                        string text2 = this.method;
                        if (text2 != null)
                        {
                            if (iTween._003C_003Ef__switch_0024map6 == null)
                            {
                                Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
                                dictionary.Add("to", 0);
                                dictionary.Add("add", 1);
                                dictionary.Add("by", 2);
                                iTween._003C_003Ef__switch_0024map6 = dictionary;
                            }
                            if (iTween._003C_003Ef__switch_0024map6.TryGetValue(text2, out num2))
                            {
                                switch (num2)
                                {
                                    case 0:
                                        this.GenerateRotateToTargets();
                                        this.apply = new ApplyTween(this.ApplyRotateToTargets);
                                        break;
                                    case 1:
                                        this.GenerateRotateAddTargets();
                                        this.apply = new ApplyTween(this.ApplyRotateAddTargets);
                                        break;
                                    case 2:
                                        this.GenerateRotateByTargets();
                                        this.apply = new ApplyTween(this.ApplyRotateAddTargets);
                                        break;
                                }
                            }
                        }
                        break;
                    }
                    case 6:
                    {
                        string text2 = this.method;
                        if (text2 != null)
                        {
                            if (iTween._003C_003Ef__switch_0024map7 == null)
                            {
                                Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
                                dictionary.Add("position", 0);
                                dictionary.Add("scale", 1);
                                dictionary.Add("rotation", 2);
                                iTween._003C_003Ef__switch_0024map7 = dictionary;
                            }
                            if (iTween._003C_003Ef__switch_0024map7.TryGetValue(text2, out num2))
                            {
                                switch (num2)
                                {
                                    case 0:
                                        this.GenerateShakePositionTargets();
                                        this.apply = new ApplyTween(this.ApplyShakePositionTargets);
                                        break;
                                    case 1:
                                        this.GenerateShakeScaleTargets();
                                        this.apply = new ApplyTween(this.ApplyShakeScaleTargets);
                                        break;
                                    case 2:
                                        this.GenerateShakeRotationTargets();
                                        this.apply = new ApplyTween(this.ApplyShakeRotationTargets);
                                        break;
                                }
                            }
                        }
                        break;
                    }
                    case 7:
                    {
                        string text2 = this.method;
                        if (text2 != null)
                        {
                            if (iTween._003C_003Ef__switch_0024map8 == null)
                            {
                                Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
                                dictionary.Add("position", 0);
                                dictionary.Add("rotation", 1);
                                dictionary.Add("scale", 2);
                                iTween._003C_003Ef__switch_0024map8 = dictionary;
                            }
                            if (iTween._003C_003Ef__switch_0024map8.TryGetValue(text2, out num2))
                            {
                                switch (num2)
                                {
                                    case 0:
                                        this.GeneratePunchPositionTargets();
                                        this.apply = new ApplyTween(this.ApplyPunchPositionTargets);
                                        break;
                                    case 1:
                                        this.GeneratePunchRotationTargets();
                                        this.apply = new ApplyTween(this.ApplyPunchRotationTargets);
                                        break;
                                    case 2:
                                        this.GeneratePunchScaleTargets();
                                        this.apply = new ApplyTween(this.ApplyPunchScaleTargets);
                                        break;
                                }
                            }
                        }
                        break;
                    }
                    case 8:
                    {
                        string text2 = this.method;
                        if (text2 != null)
                        {
                            if (iTween._003C_003Ef__switch_0024map9 == null)
                            {
                                Dictionary<string, int> dictionary = new Dictionary<string, int>(1);
                                dictionary.Add("to", 0);
                                iTween._003C_003Ef__switch_0024map9 = dictionary;
                            }
                            if (iTween._003C_003Ef__switch_0024map9.TryGetValue(text2, out num2) && num2 == 0)
                            {
                                this.GenerateLookToTargets();
                                this.apply = new ApplyTween(this.ApplyLookToTargets);
                            }
                        }
                        break;
                    }
                    case 9:
                        this.GenerateStabTargets();
                        this.apply = new ApplyTween(this.ApplyStabTargets);
                        break;
                }
            }
        }
    }

    private void GenerateRectTargets()
    {
        this.rects = new Rect[3];
        this.rects[0] = (Rect)this.tweenArguments["from"];
        this.rects[1] = (Rect)this.tweenArguments["to"];
    }

    private void GenerateColorTargets()
    {
        this.colors = new Color[1, 3];
        Color[,] array = this.colors;
        Color obj = (Color)this.tweenArguments["from"];
        array[0, 0] = obj;
        Color[,] array2 = this.colors;
        Color obj2 = (Color)this.tweenArguments["to"];
        array2[0, 1] = obj2;
    }

    private void GenerateVector3Targets()
    {
        this.vector3s = new Vector3[3];
        this.vector3s[0] = (Vector3)this.tweenArguments["from"];
        this.vector3s[1] = (Vector3)this.tweenArguments["to"];
        if (this.tweenArguments.Contains("speed"))
        {
            float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
            this.time = num / (float)this.tweenArguments["speed"];
        }
    }

    private void GenerateVector2Targets()
    {
        this.vector2s = new Vector2[3];
        this.vector2s[0] = (Vector2)this.tweenArguments["from"];
        this.vector2s[1] = (Vector2)this.tweenArguments["to"];
        if (this.tweenArguments.Contains("speed"))
        {
            Vector3 a = new Vector3(this.vector2s[0].x, this.vector2s[0].y, 0f);
            Vector3 b = new Vector3(this.vector2s[1].x, this.vector2s[1].y, 0f);
            float num = Math.Abs(Vector3.Distance(a, b));
            this.time = num / (float)this.tweenArguments["speed"];
        }
    }

    private void GenerateFloatTargets()
    {
        this.floats = new float[3];
        this.floats[0] = (float)this.tweenArguments["from"];
        this.floats[1] = (float)this.tweenArguments["to"];
        if (this.tweenArguments.Contains("speed"))
        {
            float num = Math.Abs(this.floats[0] - this.floats[1]);
            this.time = num / (float)this.tweenArguments["speed"];
        }
    }

    private void GenerateColorToTargets()
    {
        if ((bool)base.GetComponent<GUITexture>())
        {
            this.colors = new Color[1, 3];
            Color[,] array = this.colors;
            Color[,] array2 = this.colors;
            Color color;
            Color color2 = color = base.GetComponent<GUITexture>().color;
            array2[0, 1] = color2;
            array[0, 0] = color;
        }
        else if ((bool)base.GetComponent<GUIText>())
        {
            this.colors = new Color[1, 3];
            Color[,] array3 = this.colors;
            Color[,] array4 = this.colors;
            Color color;
            Color color3 = color = base.GetComponent<GUIText>().material.color;
            array4[0, 1] = color3;
            array3[0, 0] = color;
        }
        else if ((bool)base.GetComponent<Renderer>())
        {
         /*   this.colors = new Color[base.GetComponent<Renderer>().materials.Length, 3];
            for (int i = 0; i < base.GetComponent<Renderer>().materials.Length; i++)
            {
                Color[,] array5 = this.colors;
                int num = i;
                Color color4 = base.GetComponent<Renderer>().materials[i].GetColor(this.namedcolorvalue.ToString());
                array5[num, 0] = color4;
                Color[,] array6 = this.colors;
                int num2 = i;
                Color color5 = base.GetComponent<Renderer>().materials[i].GetColor(this.namedcolorvalue.ToString());
                array6[num2, 1] = color5;
            } */
        }
        else if ((bool)base.GetComponent<Light>())
        {
            this.colors = new Color[1, 3];
            Color[,] array7 = this.colors;
            Color[,] array8 = this.colors;
            Color color;
            Color color6 = color = base.GetComponent<Light>().color;
            array8[0, 1] = color6;
            array7[0, 0] = color;
        }
        else
        {
            this.colors = new Color[1, 3];
        }
        if (this.tweenArguments.Contains("color"))
        {
            for (int j = 0; j < this.colors.GetLength(0); j++)
            {
                Color[,] array9 = this.colors;
                int num3 = j;
                Color obj = (Color)this.tweenArguments["color"];
                array9[num3, 1] = obj;
            }
        }
        else
        {
            if (this.tweenArguments.Contains("r"))
            {
                for (int k = 0; k < this.colors.GetLength(0); k++)
                {
                    this.colors[k, 1].r = (float)this.tweenArguments["r"];
                }
            }
            if (this.tweenArguments.Contains("g"))
            {
                for (int l = 0; l < this.colors.GetLength(0); l++)
                {
                    this.colors[l, 1].g = (float)this.tweenArguments["g"];
                }
            }
            if (this.tweenArguments.Contains("b"))
            {
                for (int m = 0; m < this.colors.GetLength(0); m++)
                {
                    this.colors[m, 1].b = (float)this.tweenArguments["b"];
                }
            }
            if (this.tweenArguments.Contains("a"))
            {
                for (int n = 0; n < this.colors.GetLength(0); n++)
                {
                    this.colors[n, 1].a = (float)this.tweenArguments["a"];
                }
            }
        }
        if (this.tweenArguments.Contains("amount"))
        {
            for (int num4 = 0; num4 < this.colors.GetLength(0); num4++)
            {
                this.colors[num4, 1].a = (float)this.tweenArguments["amount"];
            }
        }
        else if (this.tweenArguments.Contains("alpha"))
        {
            for (int num5 = 0; num5 < this.colors.GetLength(0); num5++)
            {
                this.colors[num5, 1].a = (float)this.tweenArguments["alpha"];
            }
        }
    }

    private void GenerateAudioToTargets()
    {
        this.vector2s = new Vector2[3];
        if (this.tweenArguments.Contains("audiosource"))
        {
            this.audioSource = (AudioSource)this.tweenArguments["audiosource"];
        }
        else if ((bool)base.GetComponent<AudioSource>())
        {
            this.audioSource = base.GetComponent<AudioSource>();
        }
        else
        {
            UnityEngine.Debug.LogError("iTween Error: AudioTo requires an AudioSource.");
            this.Dispose();
        }
        this.vector2s[0] = (this.vector2s[1] = new Vector2(this.audioSource.volume, this.audioSource.pitch));
        if (this.tweenArguments.Contains("volume"))
        {
            this.vector2s[1].x = (float)this.tweenArguments["volume"];
        }
        if (this.tweenArguments.Contains("pitch"))
        {
            this.vector2s[1].y = (float)this.tweenArguments["pitch"];
        }
    }

    private void GenerateStabTargets()
    {
        if (this.tweenArguments.Contains("audiosource"))
        {
            this.audioSource = (AudioSource)this.tweenArguments["audiosource"];
        }
        else if ((bool)base.GetComponent<AudioSource>())
        {
            this.audioSource = base.GetComponent<AudioSource>();
        }
        else
        {
            base.gameObject.AddComponent<AudioSource>();
            this.audioSource = base.GetComponent<AudioSource>();
            this.audioSource.playOnAwake = false;
        }
        this.audioSource.clip = (AudioClip)this.tweenArguments["audioclip"];
        if (this.tweenArguments.Contains("pitch"))
        {
            this.audioSource.pitch = (float)this.tweenArguments["pitch"];
        }
        if (this.tweenArguments.Contains("volume"))
        {
            this.audioSource.volume = (float)this.tweenArguments["volume"];
        }
        this.time = this.audioSource.clip.length / this.audioSource.pitch;
    }

    private void GenerateLookToTargets()
    {
        this.vector3s = new Vector3[3];
        this.vector3s[0] = this.thisTransform.eulerAngles;
        if (this.tweenArguments.Contains("looktarget"))
        {
            if (this.tweenArguments["looktarget"].GetType() == typeof(Transform))
            {
                Transform transform = this.thisTransform;
                Transform target = (Transform)this.tweenArguments["looktarget"];
                Vector3? nullable = (Vector3?)this.tweenArguments["up"];
                transform.LookAt(target, (!nullable.HasValue) ? Defaults.up : nullable.Value);
            }
            else if (this.tweenArguments["looktarget"].GetType() == typeof(Vector3))
            {
                Transform transform2 = this.thisTransform;
                Vector3 worldPosition = (Vector3)this.tweenArguments["looktarget"];
                Vector3? nullable2 = (Vector3?)this.tweenArguments["up"];
                transform2.LookAt(worldPosition, (!nullable2.HasValue) ? Defaults.up : nullable2.Value);
            }
        }
        else
        {
            UnityEngine.Debug.LogError("iTween Error: LookTo needs a 'looktarget' property!");
            this.Dispose();
        }
        this.vector3s[1] = this.thisTransform.eulerAngles;
        this.thisTransform.eulerAngles = this.vector3s[0];
        if (this.tweenArguments.Contains("axis"))
        {
            string text = (string)this.tweenArguments["axis"];
            if (text != null)
            {
                if (iTween._003C_003Ef__switch_0024mapB == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
                    dictionary.Add("x", 0);
                    dictionary.Add("y", 1);
                    dictionary.Add("z", 2);
                    iTween._003C_003Ef__switch_0024mapB = dictionary;
                }
                int num = default(int);
                if (iTween._003C_003Ef__switch_0024mapB.TryGetValue(text, out num))
                {
                    switch (num)
                    {
                        case 0:
                            this.vector3s[1].y = this.vector3s[0].y;
                            this.vector3s[1].z = this.vector3s[0].z;
                            break;
                        case 1:
                            this.vector3s[1].x = this.vector3s[0].x;
                            this.vector3s[1].z = this.vector3s[0].z;
                            break;
                        case 2:
                            this.vector3s[1].x = this.vector3s[0].x;
                            this.vector3s[1].y = this.vector3s[0].y;
                            break;
                    }
                }
            }
        }
        this.vector3s[1] = new Vector3(this.clerp(this.vector3s[0].x, this.vector3s[1].x, 1f), this.clerp(this.vector3s[0].y, this.vector3s[1].y, 1f), this.clerp(this.vector3s[0].z, this.vector3s[1].z, 1f));
        if (this.tweenArguments.Contains("speed"))
        {
            float num2 = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
            this.time = num2 / (float)this.tweenArguments["speed"];
        }
    }

    private void GenerateMoveToPathTargets()
    {
        Vector3[] array2;
        if (this.tweenArguments["path"].GetType() == typeof(Vector3[]))
        {
            Vector3[] array = (Vector3[])this.tweenArguments["path"];
            if (array.Length == 1)
            {
                UnityEngine.Debug.LogError("iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!");
                this.Dispose();
            }
            array2 = new Vector3[array.Length];
            Array.Copy(array, array2, array.Length);
        }
        else
        {
            Transform[] array3 = (Transform[])this.tweenArguments["path"];
            if (array3.Length == 1)
            {
                UnityEngine.Debug.LogError("iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!");
                this.Dispose();
            }
            array2 = new Vector3[array3.Length];
            for (int i = 0; i < array3.Length; i++)
            {
                array2[i] = array3[i].position;
            }
        }
        bool flag;
        int num;
        if (this.thisTransform.position != array2[0])
        {
            if (!this.tweenArguments.Contains("movetopath") || (bool)this.tweenArguments["movetopath"])
            {
                flag = true;
                num = 3;
            }
            else
            {
                flag = false;
                num = 2;
            }
        }
        else
        {
            flag = false;
            num = 2;
        }
        this.vector3s = new Vector3[array2.Length + num];
        if (flag)
        {
            this.vector3s[1] = this.thisTransform.position;
            num = 2;
        }
        else
        {
            num = 1;
        }
        Array.Copy(array2, 0, this.vector3s, num, array2.Length);
        this.vector3s[0] = this.vector3s[1] + (this.vector3s[1] - this.vector3s[2]);
        this.vector3s[this.vector3s.Length - 1] = this.vector3s[this.vector3s.Length - 2] + (this.vector3s[this.vector3s.Length - 2] - this.vector3s[this.vector3s.Length - 3]);
        if (this.vector3s[1] == this.vector3s[this.vector3s.Length - 2])
        {
            Vector3[] array4 = new Vector3[this.vector3s.Length];
            Array.Copy(this.vector3s, array4, this.vector3s.Length);
            array4[0] = array4[array4.Length - 3];
            array4[array4.Length - 1] = array4[2];
            this.vector3s = new Vector3[array4.Length];
            Array.Copy(array4, this.vector3s, array4.Length);
        }
        this.path = new CRSpline(this.vector3s);
        if (this.tweenArguments.Contains("speed"))
        {
            float num2 = iTween.PathLength(this.vector3s);
            this.time = num2 / (float)this.tweenArguments["speed"];
        }
    }

    private void GenerateMoveToTargets()
    {
        this.vector3s = new Vector3[3];
        if (this.isLocal)
        {
            this.vector3s[0] = (this.vector3s[1] = this.thisTransform.localPosition);
        }
        else
        {
            this.vector3s[0] = (this.vector3s[1] = this.thisTransform.position);
        }
        if (this.tweenArguments.Contains("position"))
        {
            if (this.tweenArguments["position"].GetType() == typeof(Transform))
            {
                Transform transform = (Transform)this.tweenArguments["position"];
                this.vector3s[1] = transform.position;
            }
            else if (this.tweenArguments["position"].GetType() == typeof(Vector3))
            {
                this.vector3s[1] = (Vector3)this.tweenArguments["position"];
            }
        }
        else
        {
            if (this.tweenArguments.Contains("x"))
            {
                this.vector3s[1].x = (float)this.tweenArguments["x"];
            }
            if (this.tweenArguments.Contains("y"))
            {
                this.vector3s[1].y = (float)this.tweenArguments["y"];
            }
            if (this.tweenArguments.Contains("z"))
            {
                this.vector3s[1].z = (float)this.tweenArguments["z"];
            }
        }
        if (this.tweenArguments.Contains("orienttopath") && (bool)this.tweenArguments["orienttopath"])
        {
            this.tweenArguments["looktarget"] = this.vector3s[1];
        }
        if (this.tweenArguments.Contains("speed"))
        {
            float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
            this.time = num / (float)this.tweenArguments["speed"];
        }
    }

    private void GenerateMoveByTargets()
    {
        this.vector3s = new Vector3[6];
        this.vector3s[4] = this.thisTransform.eulerAngles;
        this.vector3s[0] = (this.vector3s[1] = (this.vector3s[3] = this.thisTransform.position));
        if (this.tweenArguments.Contains("amount"))
        {
            this.vector3s[1] = this.vector3s[0] + (Vector3)this.tweenArguments["amount"];
        }
        else
        {
            if (this.tweenArguments.Contains("x"))
            {
                this.vector3s[1].x = this.vector3s[0].x + (float)this.tweenArguments["x"];
            }
            if (this.tweenArguments.Contains("y"))
            {
                this.vector3s[1].y = this.vector3s[0].y + (float)this.tweenArguments["y"];
            }
            if (this.tweenArguments.Contains("z"))
            {
                this.vector3s[1].z = this.vector3s[0].z + (float)this.tweenArguments["z"];
            }
        }
        this.thisTransform.Translate(this.vector3s[1], this.space);
        this.vector3s[5] = this.thisTransform.position;
        this.thisTransform.position = this.vector3s[0];
        if (this.tweenArguments.Contains("orienttopath") && (bool)this.tweenArguments["orienttopath"])
        {
            this.tweenArguments["looktarget"] = this.vector3s[1];
        }
        if (this.tweenArguments.Contains("speed"))
        {
            float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
            this.time = num / (float)this.tweenArguments["speed"];
        }
    }

    private void GenerateScaleToTargets()
    {
        this.vector3s = new Vector3[3];
        this.vector3s[0] = (this.vector3s[1] = this.thisTransform.localScale);
        if (this.tweenArguments.Contains("scale"))
        {
            if (this.tweenArguments["scale"].GetType() == typeof(Transform))
            {
                Transform transform = (Transform)this.tweenArguments["scale"];
                this.vector3s[1] = transform.localScale;
            }
            else if (this.tweenArguments["scale"].GetType() == typeof(Vector3))
            {
                this.vector3s[1] = (Vector3)this.tweenArguments["scale"];
            }
        }
        else
        {
            if (this.tweenArguments.Contains("x"))
            {
                this.vector3s[1].x = (float)this.tweenArguments["x"];
            }
            if (this.tweenArguments.Contains("y"))
            {
                this.vector3s[1].y = (float)this.tweenArguments["y"];
            }
            if (this.tweenArguments.Contains("z"))
            {
                this.vector3s[1].z = (float)this.tweenArguments["z"];
            }
        }
        if (this.tweenArguments.Contains("speed"))
        {
            float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
            this.time = num / (float)this.tweenArguments["speed"];
        }
    }

    private void GenerateScaleByTargets()
    {
        this.vector3s = new Vector3[3];
        this.vector3s[0] = (this.vector3s[1] = this.thisTransform.localScale);
        if (this.tweenArguments.Contains("amount"))
        {
            this.vector3s[1] = Vector3.Scale(this.vector3s[1], (Vector3)this.tweenArguments["amount"]);
        }
        else
        {
            if (this.tweenArguments.Contains("x"))
            {
                this.vector3s[1].x *= (float)this.tweenArguments["x"];
            }
            if (this.tweenArguments.Contains("y"))
            {
                this.vector3s[1].y *= (float)this.tweenArguments["y"];
            }
            if (this.tweenArguments.Contains("z"))
            {
                this.vector3s[1].z *= (float)this.tweenArguments["z"];
            }
        }
        if (this.tweenArguments.Contains("speed"))
        {
            float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
            this.time = num / (float)this.tweenArguments["speed"];
        }
    }

    private void GenerateScaleAddTargets()
    {
        this.vector3s = new Vector3[3];
        this.vector3s[0] = (this.vector3s[1] = this.thisTransform.localScale);
        if (this.tweenArguments.Contains("amount"))
        {
            Vector3 val = this.vector3s[1];
            val += (Vector3)this.tweenArguments["amount"];
        }
        else
        {
            if (this.tweenArguments.Contains("x"))
            {
                this.vector3s[1].x += (float)this.tweenArguments["x"];
            }
            if (this.tweenArguments.Contains("y"))
            {
                this.vector3s[1].y += (float)this.tweenArguments["y"];
            }
            if (this.tweenArguments.Contains("z"))
            {
                this.vector3s[1].z += (float)this.tweenArguments["z"];
            }
        }
        if (this.tweenArguments.Contains("speed"))
        {
            float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
            this.time = num / (float)this.tweenArguments["speed"];
        }
    }

    private void GenerateRotateToTargets()
    {
        this.vector3s = new Vector3[3];
        if (this.isLocal)
        {
            this.vector3s[0] = (this.vector3s[1] = this.thisTransform.localEulerAngles);
        }
        else
        {
            this.vector3s[0] = (this.vector3s[1] = this.thisTransform.eulerAngles);
        }
        if (this.tweenArguments.Contains("rotation"))
        {
            if (this.tweenArguments["rotation"].GetType() == typeof(Transform))
            {
                Transform transform = (Transform)this.tweenArguments["rotation"];
                this.vector3s[1] = transform.eulerAngles;
            }
            else if (this.tweenArguments["rotation"].GetType() == typeof(Vector3))
            {
                this.vector3s[1] = (Vector3)this.tweenArguments["rotation"];
            }
        }
        else
        {
            if (this.tweenArguments.Contains("x"))
            {
                this.vector3s[1].x = (float)this.tweenArguments["x"];
            }
            if (this.tweenArguments.Contains("y"))
            {
                this.vector3s[1].y = (float)this.tweenArguments["y"];
            }
            if (this.tweenArguments.Contains("z"))
            {
                this.vector3s[1].z = (float)this.tweenArguments["z"];
            }
        }
        this.vector3s[1] = new Vector3(this.clerp(this.vector3s[0].x, this.vector3s[1].x, 1f), this.clerp(this.vector3s[0].y, this.vector3s[1].y, 1f), this.clerp(this.vector3s[0].z, this.vector3s[1].z, 1f));
        if (this.tweenArguments.Contains("speed"))
        {
            float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
            this.time = num / (float)this.tweenArguments["speed"];
        }
    }

    private void GenerateRotateAddTargets()
    {
        this.vector3s = new Vector3[5];
        this.vector3s[0] = (this.vector3s[1] = (this.vector3s[3] = this.thisTransform.eulerAngles));
        if (this.tweenArguments.Contains("amount"))
        {
            Vector3 val = this.vector3s[1];
            val += (Vector3)this.tweenArguments["amount"];
        }
        else
        {
            if (this.tweenArguments.Contains("x"))
            {
                this.vector3s[1].x += (float)this.tweenArguments["x"];
            }
            if (this.tweenArguments.Contains("y"))
            {
                this.vector3s[1].y += (float)this.tweenArguments["y"];
            }
            if (this.tweenArguments.Contains("z"))
            {
                this.vector3s[1].z += (float)this.tweenArguments["z"];
            }
        }
        if (this.tweenArguments.Contains("speed"))
        {
            float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
            this.time = num / (float)this.tweenArguments["speed"];
        }
    }

    private void GenerateRotateByTargets()
    {
        this.vector3s = new Vector3[4];
        this.vector3s[0] = (this.vector3s[1] = (this.vector3s[3] = this.thisTransform.eulerAngles));
        if (this.tweenArguments.Contains("amount"))
        {
            Vector3 val = this.vector3s[1];
            val += Vector3.Scale((Vector3)this.tweenArguments["amount"], new Vector3(360f, 360f, 360f));
        }
        else
        {
            if (this.tweenArguments.Contains("x"))
            {
                this.vector3s[1].x += 360f * (float)this.tweenArguments["x"];
            }
            if (this.tweenArguments.Contains("y"))
            {
                this.vector3s[1].y += 360f * (float)this.tweenArguments["y"];
            }
            if (this.tweenArguments.Contains("z"))
            {
                this.vector3s[1].z += 360f * (float)this.tweenArguments["z"];
            }
        }
        if (this.tweenArguments.Contains("speed"))
        {
            float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
            this.time = num / (float)this.tweenArguments["speed"];
        }
    }

    private void GenerateShakePositionTargets()
    {
        this.vector3s = new Vector3[4];
        this.vector3s[3] = this.thisTransform.eulerAngles;
        this.vector3s[0] = this.thisTransform.position;
        if (this.tweenArguments.Contains("amount"))
        {
            this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
        }
        else
        {
            if (this.tweenArguments.Contains("x"))
            {
                this.vector3s[1].x = (float)this.tweenArguments["x"];
            }
            if (this.tweenArguments.Contains("y"))
            {
                this.vector3s[1].y = (float)this.tweenArguments["y"];
            }
            if (this.tweenArguments.Contains("z"))
            {
                this.vector3s[1].z = (float)this.tweenArguments["z"];
            }
        }
    }

    private void GenerateShakeScaleTargets()
    {
        this.vector3s = new Vector3[3];
        this.vector3s[0] = this.thisTransform.localScale;
        if (this.tweenArguments.Contains("amount"))
        {
            this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
        }
        else
        {
            if (this.tweenArguments.Contains("x"))
            {
                this.vector3s[1].x = (float)this.tweenArguments["x"];
            }
            if (this.tweenArguments.Contains("y"))
            {
                this.vector3s[1].y = (float)this.tweenArguments["y"];
            }
            if (this.tweenArguments.Contains("z"))
            {
                this.vector3s[1].z = (float)this.tweenArguments["z"];
            }
        }
    }

    private void GenerateShakeRotationTargets()
    {
        this.vector3s = new Vector3[3];
        this.vector3s[0] = this.thisTransform.eulerAngles;
        if (this.tweenArguments.Contains("amount"))
        {
            this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
        }
        else
        {
            if (this.tweenArguments.Contains("x"))
            {
                this.vector3s[1].x = (float)this.tweenArguments["x"];
            }
            if (this.tweenArguments.Contains("y"))
            {
                this.vector3s[1].y = (float)this.tweenArguments["y"];
            }
            if (this.tweenArguments.Contains("z"))
            {
                this.vector3s[1].z = (float)this.tweenArguments["z"];
            }
        }
    }

    private void GeneratePunchPositionTargets()
    {
        this.vector3s = new Vector3[5];
        this.vector3s[4] = this.thisTransform.eulerAngles;
        this.vector3s[0] = this.thisTransform.position;
        this.vector3s[1] = (this.vector3s[3] = Vector3.zero);
        if (this.tweenArguments.Contains("amount"))
        {
            this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
        }
        else
        {
            if (this.tweenArguments.Contains("x"))
            {
                this.vector3s[1].x = (float)this.tweenArguments["x"];
            }
            if (this.tweenArguments.Contains("y"))
            {
                this.vector3s[1].y = (float)this.tweenArguments["y"];
            }
            if (this.tweenArguments.Contains("z"))
            {
                this.vector3s[1].z = (float)this.tweenArguments["z"];
            }
        }
    }

    private void GeneratePunchRotationTargets()
    {
        this.vector3s = new Vector3[4];
        this.vector3s[0] = this.thisTransform.eulerAngles;
        this.vector3s[1] = (this.vector3s[3] = Vector3.zero);
        if (this.tweenArguments.Contains("amount"))
        {
            this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
        }
        else
        {
            if (this.tweenArguments.Contains("x"))
            {
                this.vector3s[1].x = (float)this.tweenArguments["x"];
            }
            if (this.tweenArguments.Contains("y"))
            {
                this.vector3s[1].y = (float)this.tweenArguments["y"];
            }
            if (this.tweenArguments.Contains("z"))
            {
                this.vector3s[1].z = (float)this.tweenArguments["z"];
            }
        }
    }

    private void GeneratePunchScaleTargets()
    {
        this.vector3s = new Vector3[3];
        this.vector3s[0] = this.thisTransform.localScale;
        this.vector3s[1] = Vector3.zero;
        if (this.tweenArguments.Contains("amount"))
        {
            this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
        }
        else
        {
            if (this.tweenArguments.Contains("x"))
            {
                this.vector3s[1].x = (float)this.tweenArguments["x"];
            }
            if (this.tweenArguments.Contains("y"))
            {
                this.vector3s[1].y = (float)this.tweenArguments["y"];
            }
            if (this.tweenArguments.Contains("z"))
            {
                this.vector3s[1].z = (float)this.tweenArguments["z"];
            }
        }
    }

    private void ApplyRectTargets()
    {
        this.rects[2].x = this.ease(this.rects[0].x, this.rects[1].x, this.percentage);
        this.rects[2].y = this.ease(this.rects[0].y, this.rects[1].y, this.percentage);
        this.rects[2].width = this.ease(this.rects[0].width, this.rects[1].width, this.percentage);
        this.rects[2].height = this.ease(this.rects[0].height, this.rects[1].height, this.percentage);
        this.tweenArguments["onupdateparams"] = this.rects[2];
        if (this.percentage == 1f)
        {
            this.tweenArguments["onupdateparams"] = this.rects[1];
        }
    }

    private void ApplyColorTargets()
    {
        this.colors[0, 2].r = this.ease(this.colors[0, 0].r, this.colors[0, 1].r, this.percentage);
        this.colors[0, 2].g = this.ease(this.colors[0, 0].g, this.colors[0, 1].g, this.percentage);
        this.colors[0, 2].b = this.ease(this.colors[0, 0].b, this.colors[0, 1].b, this.percentage);
        this.colors[0, 2].a = this.ease(this.colors[0, 0].a, this.colors[0, 1].a, this.percentage);
        this.tweenArguments["onupdateparams"] = this.colors[0, 2];
        if (this.percentage == 1f)
        {
            this.tweenArguments["onupdateparams"] = this.colors[0, 1];
        }
    }

    private void ApplyVector3Targets()
    {
        this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
        this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
        this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
        this.tweenArguments["onupdateparams"] = this.vector3s[2];
        if (this.percentage == 1f)
        {
            this.tweenArguments["onupdateparams"] = this.vector3s[1];
        }
    }

    private void ApplyVector2Targets()
    {
        this.vector2s[2].x = this.ease(this.vector2s[0].x, this.vector2s[1].x, this.percentage);
        this.vector2s[2].y = this.ease(this.vector2s[0].y, this.vector2s[1].y, this.percentage);
        this.tweenArguments["onupdateparams"] = this.vector2s[2];
        if (this.percentage == 1f)
        {
            this.tweenArguments["onupdateparams"] = this.vector2s[1];
        }
    }

    private void ApplyFloatTargets()
    {
        this.floats[2] = this.ease(this.floats[0], this.floats[1], this.percentage);
        this.tweenArguments["onupdateparams"] = this.floats[2];
        if (this.percentage == 1f)
        {
            this.tweenArguments["onupdateparams"] = this.floats[1];
        }
    }

    private void ApplyColorToTargets()
    {
        for (int i = 0; i < this.colors.GetLength(0); i++)
        {
            this.colors[i, 2].r = this.ease(this.colors[i, 0].r, this.colors[i, 1].r, this.percentage);
            this.colors[i, 2].g = this.ease(this.colors[i, 0].g, this.colors[i, 1].g, this.percentage);
            this.colors[i, 2].b = this.ease(this.colors[i, 0].b, this.colors[i, 1].b, this.percentage);
            this.colors[i, 2].a = this.ease(this.colors[i, 0].a, this.colors[i, 1].a, this.percentage);
        }
        if ((bool)base.GetComponent<GUITexture>())
        {
            base.GetComponent<GUITexture>().color = this.colors[0, 2];
        }
        else if ((bool)base.GetComponent<GUIText>())
        {
            base.GetComponent<GUIText>().material.color = this.colors[0, 2];
        }
        else if ((bool)base.GetComponent<Renderer>())
        {
            for (int j = 0; j < this.colors.GetLength(0); j++)
            {
                base.GetComponent<Renderer>().materials[j].SetColor(this.namedcolorvalue.ToString(), this.colors[j, 2]);
            }
        }
        else if ((bool)base.GetComponent<Light>())
        {
            base.GetComponent<Light>().color = this.colors[0, 2];
        }
        if (this.percentage == 1f)
        {
            if ((bool)base.GetComponent<GUITexture>())
            {
                base.GetComponent<GUITexture>().color = this.colors[0, 1];
            }
            else if ((bool)base.GetComponent<GUIText>())
            {
                base.GetComponent<GUIText>().material.color = this.colors[0, 1];
            }
            else if ((bool)base.GetComponent<Renderer>())
            {
                for (int k = 0; k < this.colors.GetLength(0); k++)
                {
                    base.GetComponent<Renderer>().materials[k].SetColor(this.namedcolorvalue.ToString(), this.colors[k, 1]);
                }
            }
            else if ((bool)base.GetComponent<Light>())
            {
                base.GetComponent<Light>().color = this.colors[0, 1];
            }
        }
    }

    private void ApplyAudioToTargets()
    {
        this.vector2s[2].x = this.ease(this.vector2s[0].x, this.vector2s[1].x, this.percentage);
        this.vector2s[2].y = this.ease(this.vector2s[0].y, this.vector2s[1].y, this.percentage);
        this.audioSource.volume = this.vector2s[2].x;
        this.audioSource.pitch = this.vector2s[2].y;
        if (this.percentage == 1f)
        {
            this.audioSource.volume = this.vector2s[1].x;
            this.audioSource.pitch = this.vector2s[1].y;
        }
    }

    private void ApplyStabTargets()
    {
    }

    private void ApplyMoveToPathTargets()
    {
        this.preUpdate = this.thisTransform.position;
        float value = this.ease(0f, 1f, this.percentage);
        if (this.isLocal)
        {
            this.thisTransform.localPosition = this.path.Interp(Mathf.Clamp(value, 0f, 1f));
        }
        else
        {
            this.thisTransform.position = this.path.Interp(Mathf.Clamp(value, 0f, 1f));
        }
        if (this.tweenArguments.Contains("orienttopath") && (bool)this.tweenArguments["orienttopath"])
        {
            float num = (!this.tweenArguments.Contains("lookahead")) ? Defaults.lookAhead : ((float)this.tweenArguments["lookahead"]);
            float value2 = this.ease(0f, 1f, Mathf.Min(1f, this.percentage + num));
            this.tweenArguments["looktarget"] = this.path.Interp(Mathf.Clamp(value2, 0f, 1f));
        }
        this.postUpdate = this.thisTransform.position;
        if (this.physics)
        {
            this.thisTransform.position = this.preUpdate;
            base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
        }
    }

    private void ApplyMoveToTargets()
    {
        this.preUpdate = this.thisTransform.position;
        this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
        this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
        this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
        if (this.isLocal)
        {
            this.thisTransform.localPosition = this.vector3s[2];
        }
        else
        {
            this.thisTransform.position = this.vector3s[2];
        }
        if (this.percentage == 1f)
        {
            if (this.isLocal)
            {
                this.thisTransform.localPosition = this.vector3s[1];
            }
            else
            {
                this.thisTransform.position = this.vector3s[1];
            }
        }
        this.postUpdate = this.thisTransform.position;
        if (this.physics)
        {
            this.thisTransform.position = this.preUpdate;
            base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
        }
    }

    private void ApplyMoveByTargets()
    {
        this.preUpdate = this.thisTransform.position;
        Vector3 eulerAngles = default(Vector3);
        if (this.tweenArguments.Contains("looktarget"))
        {
            eulerAngles = this.thisTransform.eulerAngles;
            this.thisTransform.eulerAngles = this.vector3s[4];
        }
        this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
        this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
        this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
        this.thisTransform.Translate(this.vector3s[2] - this.vector3s[3], this.space);
        this.vector3s[3] = this.vector3s[2];
        if (this.tweenArguments.Contains("looktarget"))
        {
            this.thisTransform.eulerAngles = eulerAngles;
        }
        this.postUpdate = this.thisTransform.position;
        if (this.physics)
        {
            this.thisTransform.position = this.preUpdate;
            base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
        }
    }

    private void ApplyScaleToTargets()
    {
        this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
        this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
        this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
        this.thisTransform.localScale = this.vector3s[2];
        if (this.percentage == 1f)
        {
            this.thisTransform.localScale = this.vector3s[1];
        }
    }

    private void ApplyLookToTargets()
    {
        this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
        this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
        this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
        if (this.isLocal)
        {
            this.thisTransform.localRotation = Quaternion.Euler(this.vector3s[2]);
        }
        else
        {
            this.thisTransform.rotation = Quaternion.Euler(this.vector3s[2]);
        }
    }

    private void ApplyRotateToTargets()
    {
        this.preUpdate = this.thisTransform.eulerAngles;
        this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
        this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
        this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
        if (this.isLocal)
        {
            this.thisTransform.localRotation = Quaternion.Euler(this.vector3s[2]);
        }
        else
        {
            this.thisTransform.rotation = Quaternion.Euler(this.vector3s[2]);
        }
        if (this.percentage == 1f)
        {
            if (this.isLocal)
            {
                this.thisTransform.localRotation = Quaternion.Euler(this.vector3s[1]);
            }
            else
            {
                this.thisTransform.rotation = Quaternion.Euler(this.vector3s[1]);
            }
        }
        this.postUpdate = this.thisTransform.eulerAngles;
        if (this.physics)
        {
            this.thisTransform.eulerAngles = this.preUpdate;
            base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(this.postUpdate));
        }
    }

    private void ApplyRotateAddTargets()
    {
        this.preUpdate = this.thisTransform.eulerAngles;
        this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
        this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
        this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
        this.thisTransform.Rotate(this.vector3s[2] - this.vector3s[3], this.space);
        this.vector3s[3] = this.vector3s[2];
        this.postUpdate = this.thisTransform.eulerAngles;
        if (this.physics)
        {
            this.thisTransform.eulerAngles = this.preUpdate;
            base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(this.postUpdate));
        }
    }

    private void ApplyShakePositionTargets()
    {
        if (this.isLocal)
        {
            this.preUpdate = this.thisTransform.localPosition;
        }
        else
        {
            this.preUpdate = this.thisTransform.position;
        }
        Vector3 eulerAngles = default(Vector3);
        if (this.tweenArguments.Contains("looktarget"))
        {
            eulerAngles = this.thisTransform.eulerAngles;
            this.thisTransform.eulerAngles = this.vector3s[3];
        }
        if (this.percentage == 0f)
        {
            this.thisTransform.Translate(this.vector3s[1], this.space);
        }
        if (this.isLocal)
        {
            this.thisTransform.localPosition = this.vector3s[0];
        }
        else
        {
            this.thisTransform.position = this.vector3s[0];
        }
        float num = 1f - this.percentage;
        this.vector3s[2].x = UnityEngine.Random.Range((0f - this.vector3s[1].x) * num, this.vector3s[1].x * num);
        this.vector3s[2].y = UnityEngine.Random.Range((0f - this.vector3s[1].y) * num, this.vector3s[1].y * num);
        this.vector3s[2].z = UnityEngine.Random.Range((0f - this.vector3s[1].z) * num, this.vector3s[1].z * num);
        if (this.isLocal)
        {
            Transform transform = this.thisTransform;
            transform.localPosition += this.vector3s[2];
        }
        else
        {
            Transform transform2 = this.thisTransform;
            transform2.position += this.vector3s[2];
        }
        if (this.tweenArguments.Contains("looktarget"))
        {
            this.thisTransform.eulerAngles = eulerAngles;
        }
        this.postUpdate = this.thisTransform.position;
        if (this.physics)
        {
            this.thisTransform.position = this.preUpdate;
            base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
        }
    }

    private void ApplyShakeScaleTargets()
    {
        if (this.percentage == 0f)
        {
            this.thisTransform.localScale = this.vector3s[1];
        }
        this.thisTransform.localScale = this.vector3s[0];
        float num = 1f - this.percentage;
        this.vector3s[2].x = UnityEngine.Random.Range((0f - this.vector3s[1].x) * num, this.vector3s[1].x * num);
        this.vector3s[2].y = UnityEngine.Random.Range((0f - this.vector3s[1].y) * num, this.vector3s[1].y * num);
        this.vector3s[2].z = UnityEngine.Random.Range((0f - this.vector3s[1].z) * num, this.vector3s[1].z * num);
        Transform transform = this.thisTransform;
        transform.localScale += this.vector3s[2];
    }

    private void ApplyShakeRotationTargets()
    {
        this.preUpdate = this.thisTransform.eulerAngles;
        if (this.percentage == 0f)
        {
            this.thisTransform.Rotate(this.vector3s[1], this.space);
        }
        this.thisTransform.eulerAngles = this.vector3s[0];
        float num = 1f - this.percentage;
        this.vector3s[2].x = UnityEngine.Random.Range((0f - this.vector3s[1].x) * num, this.vector3s[1].x * num);
        this.vector3s[2].y = UnityEngine.Random.Range((0f - this.vector3s[1].y) * num, this.vector3s[1].y * num);
        this.vector3s[2].z = UnityEngine.Random.Range((0f - this.vector3s[1].z) * num, this.vector3s[1].z * num);
        this.thisTransform.Rotate(this.vector3s[2], this.space);
        this.postUpdate = this.thisTransform.eulerAngles;
        if (this.physics)
        {
            this.thisTransform.eulerAngles = this.preUpdate;
            base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(this.postUpdate));
        }
    }

    private void ApplyPunchPositionTargets()
    {
        this.preUpdate = this.thisTransform.position;
        Vector3 eulerAngles = default(Vector3);
        if (this.tweenArguments.Contains("looktarget"))
        {
            eulerAngles = this.thisTransform.eulerAngles;
            this.thisTransform.eulerAngles = this.vector3s[4];
        }
        if (this.vector3s[1].x > 0f)
        {
            this.vector3s[2].x = this.punch(this.vector3s[1].x, this.percentage);
        }
        else if (this.vector3s[1].x < 0f)
        {
            this.vector3s[2].x = 0f - this.punch(Mathf.Abs(this.vector3s[1].x), this.percentage);
        }
        if (this.vector3s[1].y > 0f)
        {
            this.vector3s[2].y = this.punch(this.vector3s[1].y, this.percentage);
        }
        else if (this.vector3s[1].y < 0f)
        {
            this.vector3s[2].y = 0f - this.punch(Mathf.Abs(this.vector3s[1].y), this.percentage);
        }
        if (this.vector3s[1].z > 0f)
        {
            this.vector3s[2].z = this.punch(this.vector3s[1].z, this.percentage);
        }
        else if (this.vector3s[1].z < 0f)
        {
            this.vector3s[2].z = 0f - this.punch(Mathf.Abs(this.vector3s[1].z), this.percentage);
        }
        this.thisTransform.Translate(this.vector3s[2] - this.vector3s[3], this.space);
        this.vector3s[3] = this.vector3s[2];
        if (this.tweenArguments.Contains("looktarget"))
        {
            this.thisTransform.eulerAngles = eulerAngles;
        }
        this.postUpdate = this.thisTransform.position;
        if (this.physics)
        {
            this.thisTransform.position = this.preUpdate;
            base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
        }
    }

    private void ApplyPunchRotationTargets()
    {
        this.preUpdate = this.thisTransform.eulerAngles;
        if (this.vector3s[1].x > 0f)
        {
            this.vector3s[2].x = this.punch(this.vector3s[1].x, this.percentage);
        }
        else if (this.vector3s[1].x < 0f)
        {
            this.vector3s[2].x = 0f - this.punch(Mathf.Abs(this.vector3s[1].x), this.percentage);
        }
        if (this.vector3s[1].y > 0f)
        {
            this.vector3s[2].y = this.punch(this.vector3s[1].y, this.percentage);
        }
        else if (this.vector3s[1].y < 0f)
        {
            this.vector3s[2].y = 0f - this.punch(Mathf.Abs(this.vector3s[1].y), this.percentage);
        }
        if (this.vector3s[1].z > 0f)
        {
            this.vector3s[2].z = this.punch(this.vector3s[1].z, this.percentage);
        }
        else if (this.vector3s[1].z < 0f)
        {
            this.vector3s[2].z = 0f - this.punch(Mathf.Abs(this.vector3s[1].z), this.percentage);
        }
        this.thisTransform.Rotate(this.vector3s[2] - this.vector3s[3], this.space);
        this.vector3s[3] = this.vector3s[2];
        this.postUpdate = this.thisTransform.eulerAngles;
        if (this.physics)
        {
            this.thisTransform.eulerAngles = this.preUpdate;
            base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(this.postUpdate));
        }
    }

    private void ApplyPunchScaleTargets()
    {
        if (this.vector3s[1].x > 0f)
        {
            this.vector3s[2].x = this.punch(this.vector3s[1].x, this.percentage);
        }
        else if (this.vector3s[1].x < 0f)
        {
            this.vector3s[2].x = 0f - this.punch(Mathf.Abs(this.vector3s[1].x), this.percentage);
        }
        if (this.vector3s[1].y > 0f)
        {
            this.vector3s[2].y = this.punch(this.vector3s[1].y, this.percentage);
        }
        else if (this.vector3s[1].y < 0f)
        {
            this.vector3s[2].y = 0f - this.punch(Mathf.Abs(this.vector3s[1].y), this.percentage);
        }
        if (this.vector3s[1].z > 0f)
        {
            this.vector3s[2].z = this.punch(this.vector3s[1].z, this.percentage);
        }
        else if (this.vector3s[1].z < 0f)
        {
            this.vector3s[2].z = 0f - this.punch(Mathf.Abs(this.vector3s[1].z), this.percentage);
        }
        this.thisTransform.localScale = this.vector3s[0] + this.vector3s[2];
    }

    private IEnumerator TweenDelay()
    {
        this.delayStarted = Time.time;
        yield return (object)new WaitForSeconds(this.delay);
        if (this.wasPaused)
        {
            this.wasPaused = false;
            this.TweenStart();
        }
    }

    private void TweenStart()
    {
        this.CallBack("onstart");
        if (!this.loop)
        {
            this.ConflictCheck();
            this.GenerateTargets();
        }
        if (this.type == "stab")
        {
            this.audioSource.PlayOneShot(this.audioSource.clip);
        }
        if (this.type == "move" || this.type == "scale" || this.type == "rotate" || this.type == "punch" || this.type == "shake" || this.type == "curve" || this.type == "look")
        {
            this.EnableKinematic();
        }
        this.isRunning = true;
    }

    private IEnumerator TweenRestart()
    {
        if (this.delay > 0f)
        {
            this.delayStarted = Time.time;
            yield return (object)new WaitForSeconds(this.delay);
        }
        this.loop = true;
        this.TweenStart();
    }

    private void TweenUpdate()
    {
        this.apply();
        this.CallBack("onupdate");
        this.UpdatePercentage();
    }

    private void TweenComplete()
    {
        this.isRunning = false;
        if (this.percentage > 0.5f)
        {
            this.percentage = 1f;
        }
        else
        {
            this.percentage = 0f;
        }
        this.apply();
        if (this.type == "value")
        {
            this.CallBack("onupdate");
        }
        if (this.loopType == LoopType.none)
        {
            this.Dispose();
        }
        else
        {
            this.TweenLoop();
        }
        this.CallBack("oncomplete");
    }

    private void TweenLoop()
    {
        this.DisableKinematic();
        switch (this.loopType)
        {
            case LoopType.loop:
                this.percentage = 0f;
                this.runningTime = 0f;
                this.apply();
                base.StartCoroutine("TweenRestart");
                break;
            case LoopType.pingPong:
                this.reverse = !this.reverse;
                this.runningTime = 0f;
                base.StartCoroutine("TweenRestart");
                break;
        }
    }

    public static Rect RectUpdate(Rect currentValue, Rect targetValue, float speed)
    {
        return new Rect(iTween.FloatUpdate(currentValue.x, targetValue.x, speed), iTween.FloatUpdate(currentValue.y, targetValue.y, speed), iTween.FloatUpdate(currentValue.width, targetValue.width, speed), iTween.FloatUpdate(currentValue.height, targetValue.height, speed));
    }

    public static Vector3 Vector3Update(Vector3 currentValue, Vector3 targetValue, float speed)
    {
        Vector3 a = targetValue - currentValue;
        currentValue += a * speed * Time.deltaTime;
        return currentValue;
    }

    public static Vector2 Vector2Update(Vector2 currentValue, Vector2 targetValue, float speed)
    {
        Vector2 a = targetValue - currentValue;
        currentValue += a * speed * Time.deltaTime;
        return currentValue;
    }

    public static float FloatUpdate(float currentValue, float targetValue, float speed)
    {
        float num = targetValue - currentValue;
        currentValue += num * speed * Time.deltaTime;
        return currentValue;
    }

    public static void FadeUpdate(GameObject target, Hashtable args)
    {
        args["a"] = args["alpha"];
        iTween.ColorUpdate(target, args);
    }

    public static void FadeUpdate(GameObject target, float alpha, float time)
    {
        iTween.FadeUpdate(target, iTween.Hash("alpha", alpha, "time", time));
    }

    public static void ColorUpdate(GameObject target, Hashtable args)
    {
        iTween.CleanArgs(args);
        Color[] array = new Color[4];
        if (!args.Contains("includechildren") || (bool)args["includechildren"])
        {
            foreach (Transform item in target.transform)
            {
                iTween.ColorUpdate(item.gameObject, args);
            }
        }
        float num;
        if (args.Contains("time"))
        {
            num = (float)args["time"];
            num *= Defaults.updateTimePercentage;
        }
        else
        {
            num = Defaults.updateTime;
        }
        if ((bool)target.GetComponent<GUITexture>())
        {
            array[0] = (array[1] = target.GetComponent<GUITexture>().color);
        }
        else if ((bool)target.GetComponent<GUIText>())
        {
            array[0] = (array[1] = target.GetComponent<GUIText>().material.color);
        }
        else if ((bool)target.GetComponent<Renderer>())
        {
            array[0] = (array[1] = target.GetComponent<Renderer>().material.color);
        }
        else if ((bool)target.GetComponent<Light>())
        {
            array[0] = (array[1] = target.GetComponent<Light>().color);
        }
        if (args.Contains("color"))
        {
            array[1] = (Color)args["color"];
        }
        else
        {
            if (args.Contains("r"))
            {
                array[1].r = (float)args["r"];
            }
            if (args.Contains("g"))
            {
                array[1].g = (float)args["g"];
            }
            if (args.Contains("b"))
            {
                array[1].b = (float)args["b"];
            }
            if (args.Contains("a"))
            {
                array[1].a = (float)args["a"];
            }
        }
        array[3].r = Mathf.SmoothDamp(array[0].r, array[1].r, ref array[2].r, num);
        array[3].g = Mathf.SmoothDamp(array[0].g, array[1].g, ref array[2].g, num);
        array[3].b = Mathf.SmoothDamp(array[0].b, array[1].b, ref array[2].b, num);
        array[3].a = Mathf.SmoothDamp(array[0].a, array[1].a, ref array[2].a, num);
        if ((bool)target.GetComponent<GUITexture>())
        {
            target.GetComponent<GUITexture>().color = array[3];
        }
        else if ((bool)target.GetComponent<GUIText>())
        {
            target.GetComponent<GUIText>().material.color = array[3];
        }
        else if ((bool)target.GetComponent<Renderer>())
        {
            target.GetComponent<Renderer>().material.color = array[3];
        }
        else if ((bool)target.GetComponent<Light>())
        {
            target.GetComponent<Light>().color = array[3];
        }
    }

    public static void ColorUpdate(GameObject target, Color color, float time)
    {
        iTween.ColorUpdate(target, iTween.Hash("color", color, "time", time));
    }

    public static void AudioUpdate(GameObject target, Hashtable args)
    {
        iTween.CleanArgs(args);
        Vector2[] array = new Vector2[4];
        float num;
        if (args.Contains("time"))
        {
            num = (float)args["time"];
            num *= Defaults.updateTimePercentage;
        }
        else
        {
            num = Defaults.updateTime;
        }
        AudioSource audioSource;
        if (args.Contains("audiosource"))
        {
            audioSource = (AudioSource)args["audiosource"];
            goto IL_008f;
        }
        if ((bool)target.GetComponent<AudioSource>())
        {
            audioSource = target.GetComponent<AudioSource>();
            goto IL_008f;
        }
        UnityEngine.Debug.LogError("iTween Error: AudioUpdate requires an AudioSource.");
        return;
        IL_008f:
        array[0] = (array[1] = new Vector2(audioSource.volume, audioSource.pitch));
        if (args.Contains("volume"))
        {
            array[1].x = (float)args["volume"];
        }
        if (args.Contains("pitch"))
        {
            array[1].y = (float)args["pitch"];
        }
        array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
        array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
        audioSource.volume = array[3].x;
        audioSource.pitch = array[3].y;
    }

    public static void AudioUpdate(GameObject target, float volume, float pitch, float time)
    {
        iTween.AudioUpdate(target, iTween.Hash("volume", volume, "pitch", pitch, "time", time));
    }

    public static void RotateUpdate(GameObject target, Hashtable args)
    {
        iTween.CleanArgs(args);
        Vector3[] array = new Vector3[4];
        Vector3 eulerAngles = target.transform.eulerAngles;
        float num;
        if (args.Contains("time"))
        {
            num = (float)args["time"];
            num *= Defaults.updateTimePercentage;
        }
        else
        {
            num = Defaults.updateTime;
        }
        bool flag = (!args.Contains("islocal")) ? Defaults.isLocal : ((bool)args["islocal"]);
        if (flag)
        {
            array[0] = target.transform.localEulerAngles;
        }
        else
        {
            array[0] = target.transform.eulerAngles;
        }
        if (args.Contains("rotation"))
        {
            if (args["rotation"].GetType() == typeof(Transform))
            {
                Transform transform = (Transform)args["rotation"];
                array[1] = transform.eulerAngles;
            }
            else if (args["rotation"].GetType() == typeof(Vector3))
            {
                array[1] = (Vector3)args["rotation"];
            }
        }
        array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
        array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
        array[3].z = Mathf.SmoothDampAngle(array[0].z, array[1].z, ref array[2].z, num);
        if (flag)
        {
            target.transform.localEulerAngles = array[3];
        }
        else
        {
            target.transform.eulerAngles = array[3];
        }
        if ((UnityEngine.Object)target.GetComponent<Rigidbody>() != (UnityEngine.Object)null)
        {
            Vector3 eulerAngles2 = target.transform.eulerAngles;
            target.transform.eulerAngles = eulerAngles;
            target.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(eulerAngles2));
        }
    }

    public static void RotateUpdate(GameObject target, Vector3 rotation, float time)
    {
        iTween.RotateUpdate(target, iTween.Hash("rotation", rotation, "time", time));
    }

    public static void ScaleUpdate(GameObject target, Hashtable args)
    {
        iTween.CleanArgs(args);
        Vector3[] array = new Vector3[4];
        float num;
        if (args.Contains("time"))
        {
            num = (float)args["time"];
            num *= Defaults.updateTimePercentage;
        }
        else
        {
            num = Defaults.updateTime;
        }
        array[0] = (array[1] = target.transform.localScale);
        if (args.Contains("scale"))
        {
            if (args["scale"].GetType() == typeof(Transform))
            {
                Transform transform = (Transform)args["scale"];
                array[1] = transform.localScale;
            }
            else if (args["scale"].GetType() == typeof(Vector3))
            {
                array[1] = (Vector3)args["scale"];
            }
        }
        else
        {
            if (args.Contains("x"))
            {
                array[1].x = (float)args["x"];
            }
            if (args.Contains("y"))
            {
                array[1].y = (float)args["y"];
            }
            if (args.Contains("z"))
            {
                array[1].z = (float)args["z"];
            }
        }
        array[3].x = Mathf.SmoothDamp(array[0].x, array[1].x, ref array[2].x, num);
        array[3].y = Mathf.SmoothDamp(array[0].y, array[1].y, ref array[2].y, num);
        array[3].z = Mathf.SmoothDamp(array[0].z, array[1].z, ref array[2].z, num);
        target.transform.localScale = array[3];
    }

    public static void ScaleUpdate(GameObject target, Vector3 scale, float time)
    {
        iTween.ScaleUpdate(target, iTween.Hash("scale", scale, "time", time));
    }

    public static void MoveUpdate(GameObject target, Hashtable args)
    {
        iTween.CleanArgs(args);
        Vector3[] array = new Vector3[4];
        Vector3 position = target.transform.position;
        float num;
        if (args.Contains("time"))
        {
            num = (float)args["time"];
            num *= Defaults.updateTimePercentage;
        }
        else
        {
            num = Defaults.updateTime;
        }
        bool flag = (!args.Contains("islocal")) ? Defaults.isLocal : ((bool)args["islocal"]);
        if (flag)
        {
            array[0] = (array[1] = target.transform.localPosition);
        }
        else
        {
            array[0] = (array[1] = target.transform.position);
        }
        if (args.Contains("position"))
        {
            if (args["position"].GetType() == typeof(Transform))
            {
                Transform transform = (Transform)args["position"];
                array[1] = transform.position;
            }
            else if (args["position"].GetType() == typeof(Vector3))
            {
                array[1] = (Vector3)args["position"];
            }
        }
        else
        {
            if (args.Contains("x"))
            {
                array[1].x = (float)args["x"];
            }
            if (args.Contains("y"))
            {
                array[1].y = (float)args["y"];
            }
            if (args.Contains("z"))
            {
                array[1].z = (float)args["z"];
            }
        }
        array[3].x = Mathf.SmoothDamp(array[0].x, array[1].x, ref array[2].x, num);
        array[3].y = Mathf.SmoothDamp(array[0].y, array[1].y, ref array[2].y, num);
        array[3].z = Mathf.SmoothDamp(array[0].z, array[1].z, ref array[2].z, num);
        if (args.Contains("orienttopath") && (bool)args["orienttopath"])
        {
            args["looktarget"] = array[3];
        }
        if (args.Contains("looktarget"))
        {
            iTween.LookUpdate(target, args);
        }
        if (flag)
        {
            target.transform.localPosition = array[3];
        }
        else
        {
            target.transform.position = array[3];
        }
        if ((UnityEngine.Object)target.GetComponent<Rigidbody>() != (UnityEngine.Object)null)
        {
            Vector3 position2 = target.transform.position;
            target.transform.position = position;
            target.GetComponent<Rigidbody>().MovePosition(position2);
        }
    }

    public static void MoveUpdate(GameObject target, Vector3 position, float time)
    {
        iTween.MoveUpdate(target, iTween.Hash("position", position, "time", time));
    }

    public static void LookUpdate(GameObject target, Hashtable args)
    {
        iTween.CleanArgs(args);
        Vector3[] array = new Vector3[5];
        float num;
        if (args.Contains("looktime"))
        {
            num = (float)args["looktime"];
            num *= Defaults.updateTimePercentage;
        }
        else if (args.Contains("time"))
        {
            num = (float)args["time"] * 0.15f;
            num *= Defaults.updateTimePercentage;
        }
        else
        {
            num = Defaults.updateTime;
        }
        array[0] = target.transform.eulerAngles;
        if (args.Contains("looktarget"))
        {
            if (args["looktarget"].GetType() == typeof(Transform))
            {
                Transform transform = target.transform;
                Transform target2 = (Transform)args["looktarget"];
                Vector3? nullable = (Vector3?)args["up"];
                transform.LookAt(target2, (!nullable.HasValue) ? Defaults.up : nullable.Value);
            }
            else if (args["looktarget"].GetType() == typeof(Vector3))
            {
                Transform transform2 = target.transform;
                Vector3 worldPosition = (Vector3)args["looktarget"];
                Vector3? nullable2 = (Vector3?)args["up"];
                transform2.LookAt(worldPosition, (!nullable2.HasValue) ? Defaults.up : nullable2.Value);
            }
            array[1] = target.transform.eulerAngles;
            target.transform.eulerAngles = array[0];
            array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
            array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
            array[3].z = Mathf.SmoothDampAngle(array[0].z, array[1].z, ref array[2].z, num);
            target.transform.eulerAngles = array[3];
            if (args.Contains("axis"))
            {
                array[4] = target.transform.eulerAngles;
                string text = (string)args["axis"];
                if (text != null)
                {
                    if (iTween._003C_003Ef__switch_0024mapC == null)
                    {
                        Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
                        dictionary.Add("x", 0);
                        dictionary.Add("y", 1);
                        dictionary.Add("z", 2);
                        iTween._003C_003Ef__switch_0024mapC = dictionary;
                    }
                    int num2 = default(int);
                    if (iTween._003C_003Ef__switch_0024mapC.TryGetValue(text, out num2))
                    {
                        switch (num2)
                        {
                            case 0:
                                array[4].y = array[0].y;
                                array[4].z = array[0].z;
                                break;
                            case 1:
                                array[4].x = array[0].x;
                                array[4].z = array[0].z;
                                break;
                            case 2:
                                array[4].x = array[0].x;
                                array[4].y = array[0].y;
                                break;
                        }
                    }
                }
                target.transform.eulerAngles = array[4];
            }
        }
        else
        {
            UnityEngine.Debug.LogError("iTween Error: LookUpdate needs a 'looktarget' property!");
        }
    }

    public static void LookUpdate(GameObject target, Vector3 looktarget, float time)
    {
        iTween.LookUpdate(target, iTween.Hash("looktarget", looktarget, "time", time));
    }

    public static float PathLength(Transform[] path)
    {
        Vector3[] array = new Vector3[path.Length];
        float num = 0f;
        for (int i = 0; i < path.Length; i++)
        {
            array[i] = path[i].position;
        }
        Vector3[] pts = iTween.PathControlPointGenerator(array);
        Vector3 a = iTween.Interp(pts, 0f);
        int num2 = path.Length * 20;
        for (int j = 1; j <= num2; j++)
        {
            float t = (float)j / (float)num2;
            Vector3 vector = iTween.Interp(pts, t);
            num += Vector3.Distance(a, vector);
            a = vector;
        }
        return num;
    }

    public static float PathLength(Vector3[] path)
    {
        float num = 0f;
        Vector3[] pts = iTween.PathControlPointGenerator(path);
        Vector3 a = iTween.Interp(pts, 0f);
        int num2 = path.Length * 20;
        for (int i = 1; i <= num2; i++)
        {
            float t = (float)i / (float)num2;
            Vector3 vector = iTween.Interp(pts, t);
            num += Vector3.Distance(a, vector);
            a = vector;
        }
        return num;
    }

    public static Texture2D CameraTexture(Color color)
    {
        Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
        Color[] array = new Color[Screen.width * Screen.height];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = color;
        }
        texture2D.SetPixels(array);
        texture2D.Apply();
        return texture2D;
    }

    public static void PutOnPath(GameObject target, Vector3[] path, float percent)
    {
        target.transform.position = iTween.Interp(iTween.PathControlPointGenerator(path), percent);
    }

    public static void PutOnPath(Transform target, Vector3[] path, float percent)
    {
        target.position = iTween.Interp(iTween.PathControlPointGenerator(path), percent);
    }

    public static void PutOnPath(GameObject target, Transform[] path, float percent)
    {
        Vector3[] array = new Vector3[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            array[i] = path[i].position;
        }
        target.transform.position = iTween.Interp(iTween.PathControlPointGenerator(array), percent);
    }

    public static void PutOnPath(Transform target, Transform[] path, float percent)
    {
        Vector3[] array = new Vector3[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            array[i] = path[i].position;
        }
        target.position = iTween.Interp(iTween.PathControlPointGenerator(array), percent);
    }

    public static Vector3 PointOnPath(Transform[] path, float percent)
    {
        Vector3[] array = new Vector3[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            array[i] = path[i].position;
        }
        return iTween.Interp(iTween.PathControlPointGenerator(array), percent);
    }

    public static void DrawLine(Vector3[] line)
    {
        if (line.Length > 0)
        {
            iTween.DrawLineHelper(line, Defaults.color, "gizmos");
        }
    }

    public static void DrawLine(Vector3[] line, Color color)
    {
        if (line.Length > 0)
        {
            iTween.DrawLineHelper(line, color, "gizmos");
        }
    }

    public static void DrawLine(Transform[] line)
    {
        if (line.Length > 0)
        {
            Vector3[] array = new Vector3[line.Length];
            for (int i = 0; i < line.Length; i++)
            {
                array[i] = line[i].position;
            }
            iTween.DrawLineHelper(array, Defaults.color, "gizmos");
        }
    }

    public static void DrawLine(Transform[] line, Color color)
    {
        if (line.Length > 0)
        {
            Vector3[] array = new Vector3[line.Length];
            for (int i = 0; i < line.Length; i++)
            {
                array[i] = line[i].position;
            }
            iTween.DrawLineHelper(array, color, "gizmos");
        }
    }

    public static void DrawLineGizmos(Vector3[] line)
    {
        if (line.Length > 0)
        {
            iTween.DrawLineHelper(line, Defaults.color, "gizmos");
        }
    }

    public static void DrawLineGizmos(Vector3[] line, Color color)
    {
        if (line.Length > 0)
        {
            iTween.DrawLineHelper(line, color, "gizmos");
        }
    }

    public static void DrawLineGizmos(Transform[] line)
    {
        if (line.Length > 0)
        {
            Vector3[] array = new Vector3[line.Length];
            for (int i = 0; i < line.Length; i++)
            {
                array[i] = line[i].position;
            }
            iTween.DrawLineHelper(array, Defaults.color, "gizmos");
        }
    }

    public static void DrawLineGizmos(Transform[] line, Color color)
    {
        if (line.Length > 0)
        {
            Vector3[] array = new Vector3[line.Length];
            for (int i = 0; i < line.Length; i++)
            {
                array[i] = line[i].position;
            }
            iTween.DrawLineHelper(array, color, "gizmos");
        }
    }

    public static void DrawLineHandles(Vector3[] line)
    {
        if (line.Length > 0)
        {
            iTween.DrawLineHelper(line, Defaults.color, "handles");
        }
    }

    public static void DrawLineHandles(Vector3[] line, Color color)
    {
        if (line.Length > 0)
        {
            iTween.DrawLineHelper(line, color, "handles");
        }
    }

    public static void DrawLineHandles(Transform[] line)
    {
        if (line.Length > 0)
        {
            Vector3[] array = new Vector3[line.Length];
            for (int i = 0; i < line.Length; i++)
            {
                array[i] = line[i].position;
            }
            iTween.DrawLineHelper(array, Defaults.color, "handles");
        }
    }

    public static void DrawLineHandles(Transform[] line, Color color)
    {
        if (line.Length > 0)
        {
            Vector3[] array = new Vector3[line.Length];
            for (int i = 0; i < line.Length; i++)
            {
                array[i] = line[i].position;
            }
            iTween.DrawLineHelper(array, color, "handles");
        }
    }

    public static Vector3 PointOnPath(Vector3[] path, float percent)
    {
        return iTween.Interp(iTween.PathControlPointGenerator(path), percent);
    }

    public static void DrawPath(Vector3[] path)
    {
        if (path.Length > 0)
        {
            iTween.DrawPathHelper(path, Defaults.color, "gizmos");
        }
    }

    public static void DrawPath(Vector3[] path, Color color)
    {
        if (path.Length > 0)
        {
            iTween.DrawPathHelper(path, color, "gizmos");
        }
    }

    public static void DrawPath(Transform[] path)
    {
        if (path.Length > 0)
        {
            Vector3[] array = new Vector3[path.Length];
            for (int i = 0; i < path.Length; i++)
            {
                array[i] = path[i].position;
            }
            iTween.DrawPathHelper(array, Defaults.color, "gizmos");
        }
    }

    public static void DrawPath(Transform[] path, Color color)
    {
        if (path.Length > 0)
        {
            Vector3[] array = new Vector3[path.Length];
            for (int i = 0; i < path.Length; i++)
            {
                array[i] = path[i].position;
            }
            iTween.DrawPathHelper(array, color, "gizmos");
        }
    }

    public static void DrawPathGizmos(Vector3[] path)
    {
        if (path.Length > 0)
        {
            iTween.DrawPathHelper(path, Defaults.color, "gizmos");
        }
    }

    public static void DrawPathGizmos(Vector3[] path, Color color)
    {
        if (path.Length > 0)
        {
            iTween.DrawPathHelper(path, color, "gizmos");
        }
    }

    public static void DrawPathGizmos(Transform[] path)
    {
        if (path.Length > 0)
        {
            Vector3[] array = new Vector3[path.Length];
            for (int i = 0; i < path.Length; i++)
            {
                array[i] = path[i].position;
            }
            iTween.DrawPathHelper(array, Defaults.color, "gizmos");
        }
    }

    public static void DrawPathGizmos(Transform[] path, Color color)
    {
        if (path.Length > 0)
        {
            Vector3[] array = new Vector3[path.Length];
            for (int i = 0; i < path.Length; i++)
            {
                array[i] = path[i].position;
            }
            iTween.DrawPathHelper(array, color, "gizmos");
        }
    }

    public static void DrawPathHandles(Vector3[] path)
    {
        if (path.Length > 0)
        {
            iTween.DrawPathHelper(path, Defaults.color, "handles");
        }
    }

    public static void DrawPathHandles(Vector3[] path, Color color)
    {
        if (path.Length > 0)
        {
            iTween.DrawPathHelper(path, color, "handles");
        }
    }

    public static void DrawPathHandles(Transform[] path)
    {
        if (path.Length > 0)
        {
            Vector3[] array = new Vector3[path.Length];
            for (int i = 0; i < path.Length; i++)
            {
                array[i] = path[i].position;
            }
            iTween.DrawPathHelper(array, Defaults.color, "handles");
        }
    }

    public static void DrawPathHandles(Transform[] path, Color color)
    {
        if (path.Length > 0)
        {
            Vector3[] array = new Vector3[path.Length];
            for (int i = 0; i < path.Length; i++)
            {
                array[i] = path[i].position;
            }
            iTween.DrawPathHelper(array, color, "handles");
        }
    }

    public static void CameraFadeDepth(int depth)
    {
        if ((bool)iTween.cameraFade)
        {
            Transform transform = iTween.cameraFade.transform;
            Vector3 position = iTween.cameraFade.transform.position;
            float x = position.x;
            Vector3 position2 = iTween.cameraFade.transform.position;
            transform.position = new Vector3(x, position2.y, (float)depth);
        }
    }

    public static void CameraFadeDestroy()
    {
        if ((bool)iTween.cameraFade)
        {
            UnityEngine.Object.Destroy(iTween.cameraFade);
        }
    }

    public static void CameraFadeSwap(Texture2D texture)
    {
        if ((bool)iTween.cameraFade)
        {
            iTween.cameraFade.GetComponent<GUITexture>().texture = texture;
        }
    }

    public static GameObject CameraFadeAdd(Texture2D texture, int depth)
    {
        if ((bool)iTween.cameraFade)
        {
            return null;
        }
        iTween.cameraFade = new GameObject("iTween Camera Fade");
        iTween.cameraFade.transform.position = new Vector3(0.5f, 0.5f, (float)depth);
        iTween.cameraFade.AddComponent<GUITexture>();
        iTween.cameraFade.GetComponent<GUITexture>().texture = texture;
        iTween.cameraFade.GetComponent<GUITexture>().color = new Color(0.5f, 0.5f, 0.5f, 0f);
        return iTween.cameraFade;
    }

    public static GameObject CameraFadeAdd(Texture2D texture)
    {
        if ((bool)iTween.cameraFade)
        {
            return null;
        }
        iTween.cameraFade = new GameObject("iTween Camera Fade");
        iTween.cameraFade.transform.position = new Vector3(0.5f, 0.5f, (float)Defaults.cameraFadeDepth);
        iTween.cameraFade.AddComponent<GUITexture>();
        iTween.cameraFade.GetComponent<GUITexture>().texture = texture;
        iTween.cameraFade.GetComponent<GUITexture>().color = new Color(0.5f, 0.5f, 0.5f, 0f);
        return iTween.cameraFade;
    }

    public static GameObject CameraFadeAdd()
    {
        if ((bool)iTween.cameraFade)
        {
            return null;
        }
        iTween.cameraFade = new GameObject("iTween Camera Fade");
        iTween.cameraFade.transform.position = new Vector3(0.5f, 0.5f, (float)Defaults.cameraFadeDepth);
        iTween.cameraFade.AddComponent<GUITexture>();
        iTween.cameraFade.GetComponent<GUITexture>().texture = iTween.CameraTexture(Color.black);
        iTween.cameraFade.GetComponent<GUITexture>().color = new Color(0.5f, 0.5f, 0.5f, 0f);
        return iTween.cameraFade;
    }

    public static void Resume(GameObject target)
    {
        Component[] components = target.GetComponents<iTween>();
        Component[] array = components;
        for (int i = 0; i < array.Length; i++)
        {
            iTween iTween = (iTween)array[i];
            iTween.enabled = true;
        }
    }

    public static void Resume(GameObject target, bool includechildren)
    {
        iTween.Resume(target);
        if (includechildren)
        {
            foreach (Transform item in target.transform)
            {
                iTween.Resume(item.gameObject, true);
            }
        }
    }

    public static void Resume(GameObject target, string type)
    {
        Component[] components = target.GetComponents<iTween>();
        Component[] array = components;
        for (int i = 0; i < array.Length; i++)
        {
            iTween iTween = (iTween)array[i];
            string text = iTween.type + iTween.method;
            text = text.Substring(0, type.Length);
            if (text.ToLower() == type.ToLower())
            {
                iTween.enabled = true;
            }
        }
    }

    public static void Resume(GameObject target, string type, bool includechildren)
    {
        Component[] components = target.GetComponents<iTween>();
        Component[] array = components;
        for (int i = 0; i < array.Length; i++)
        {
            iTween iTween = (iTween)array[i];
            string text = iTween.type + iTween.method;
            text = text.Substring(0, type.Length);
            if (text.ToLower() == type.ToLower())
            {
                iTween.enabled = true;
            }
        }
        if (includechildren)
        {
            foreach (Transform item in target.transform)
            {
                iTween.Resume(item.gameObject, type, true);
            }
        }
    }

    public static void Resume()
    {
        for (int i = 0; i < iTween.tweens.Count; i++)
        {
            Hashtable hashtable = iTween.tweens[i];
            GameObject target = (GameObject)hashtable["target"];
            iTween.Resume(target);
        }
    }

    public static void Resume(string type)
    {
        ArrayList arrayList = new ArrayList();
        for (int i = 0; i < iTween.tweens.Count; i++)
        {
            Hashtable hashtable = iTween.tweens[i];
            GameObject value = (GameObject)hashtable["target"];
            arrayList.Insert(arrayList.Count, value);
        }
        for (int j = 0; j < arrayList.Count; j++)
        {
            iTween.Resume((GameObject)arrayList[j], type);
        }
    }

    public static void Pause(GameObject target)
    {
        Component[] components = target.GetComponents<iTween>();
        Component[] array = components;
        for (int i = 0; i < array.Length; i++)
        {
            iTween iTween = (iTween)array[i];
            if (iTween.delay > 0f)
            {
                iTween.delay -= Time.time - iTween.delayStarted;
                iTween.StopCoroutine("TweenDelay");
            }
            iTween.isPaused = true;
            iTween.enabled = false;
        }
    }

    public static void Pause(GameObject target, bool includechildren)
    {
        iTween.Pause(target);
        if (includechildren)
        {
            foreach (Transform item in target.transform)
            {
                iTween.Pause(item.gameObject, true);
            }
        }
    }

    public static void Pause(GameObject target, string type)
    {
        Component[] components = target.GetComponents<iTween>();
        Component[] array = components;
        for (int i = 0; i < array.Length; i++)
        {
            iTween iTween = (iTween)array[i];
            string text = iTween.type + iTween.method;
            text = text.Substring(0, type.Length);
            if (text.ToLower() == type.ToLower())
            {
                if (iTween.delay > 0f)
                {
                    iTween.delay -= Time.time - iTween.delayStarted;
                    iTween.StopCoroutine("TweenDelay");
                }
                iTween.isPaused = true;
                iTween.enabled = false;
            }
        }
    }

    public static void Pause(GameObject target, string type, bool includechildren)
    {
        Component[] components = target.GetComponents<iTween>();
        Component[] array = components;
        for (int i = 0; i < array.Length; i++)
        {
            iTween iTween = (iTween)array[i];
            string text = iTween.type + iTween.method;
            text = text.Substring(0, type.Length);
            if (text.ToLower() == type.ToLower())
            {
                if (iTween.delay > 0f)
                {
                    iTween.delay -= Time.time - iTween.delayStarted;
                    iTween.StopCoroutine("TweenDelay");
                }
                iTween.isPaused = true;
                iTween.enabled = false;
            }
        }
        if (includechildren)
        {
            foreach (Transform item in target.transform)
            {
                iTween.Pause(item.gameObject, type, true);
            }
        }
    }

    public static void Pause()
    {
        for (int i = 0; i < iTween.tweens.Count; i++)
        {
            Hashtable hashtable = iTween.tweens[i];
            GameObject target = (GameObject)hashtable["target"];
            iTween.Pause(target);
        }
    }

    public static void Pause(string type)
    {
        ArrayList arrayList = new ArrayList();
        for (int i = 0; i < iTween.tweens.Count; i++)
        {
            Hashtable hashtable = iTween.tweens[i];
            GameObject value = (GameObject)hashtable["target"];
            arrayList.Insert(arrayList.Count, value);
        }
        for (int j = 0; j < arrayList.Count; j++)
        {
            iTween.Pause((GameObject)arrayList[j], type);
        }
    }

    public static int Count()
    {
        return iTween.tweens.Count;
    }

    public static int Count(string type)
    {
        int num = 0;
        for (int i = 0; i < iTween.tweens.Count; i++)
        {
            Hashtable hashtable = iTween.tweens[i];
            string text = (string)hashtable["type"] + (string)hashtable["method"];
            text = text.Substring(0, type.Length);
            if (text.ToLower() == type.ToLower())
            {
                num++;
            }
        }
        return num;
    }

    public static int Count(GameObject target)
    {
        Component[] components = target.GetComponents<iTween>();
        return components.Length;
    }

    public static int Count(GameObject target, string type)
    {
        int num = 0;
        Component[] components = target.GetComponents<iTween>();
        Component[] array = components;
        for (int i = 0; i < array.Length; i++)
        {
            iTween iTween = (iTween)array[i];
            string text = iTween.type + iTween.method;
            text = text.Substring(0, type.Length);
            if (text.ToLower() == type.ToLower())
            {
                num++;
            }
        }
        return num;
    }

    public static void Stop()
    {
        for (int i = 0; i < iTween.tweens.Count; i++)
        {
            Hashtable hashtable = iTween.tweens[i];
            GameObject target = (GameObject)hashtable["target"];
            iTween.Stop(target);
        }
        iTween.tweens.Clear();
    }

    public static void Stop(string type)
    {
        ArrayList arrayList = new ArrayList();
        for (int i = 0; i < iTween.tweens.Count; i++)
        {
            Hashtable hashtable = iTween.tweens[i];
            GameObject value = (GameObject)hashtable["target"];
            arrayList.Insert(arrayList.Count, value);
        }
        for (int j = 0; j < arrayList.Count; j++)
        {
            iTween.Stop((GameObject)arrayList[j], type);
        }
    }

    public static void StopByName(string name)
    {
        ArrayList arrayList = new ArrayList();
        for (int i = 0; i < iTween.tweens.Count; i++)
        {
            Hashtable hashtable = iTween.tweens[i];
            GameObject value = (GameObject)hashtable["target"];
            arrayList.Insert(arrayList.Count, value);
        }
        for (int j = 0; j < arrayList.Count; j++)
        {
            iTween.StopByName((GameObject)arrayList[j], name);
        }
    }

    public static void Stop(GameObject target)
    {
        Component[] components = target.GetComponents<iTween>();
        Component[] array = components;
        for (int i = 0; i < array.Length; i++)
        {
            iTween iTween = (iTween)array[i];
            iTween.Dispose();
        }
    }

    public static void Stop(GameObject target, bool includechildren)
    {
        iTween.Stop(target);
        if (includechildren)
        {
            foreach (Transform item in target.transform)
            {
                iTween.Stop(item.gameObject, true);
            }
        }
    }

    public static void Stop(GameObject target, string type)
    {
        Component[] components = target.GetComponents<iTween>();
        Component[] array = components;
        for (int i = 0; i < array.Length; i++)
        {
            iTween iTween = (iTween)array[i];
            string text = iTween.type + iTween.method;
            text = text.Substring(0, type.Length);
            if (text.ToLower() == type.ToLower())
            {
                iTween.Dispose();
            }
        }
    }

    public static void StopByName(GameObject target, string name)
    {
        Component[] components = target.GetComponents<iTween>();
        Component[] array = components;
        for (int i = 0; i < array.Length; i++)
        {
            iTween iTween = (iTween)array[i];
            if (iTween._name == name)
            {
                iTween.Dispose();
            }
        }
    }

    public static void Stop(GameObject target, string type, bool includechildren)
    {
        Component[] components = target.GetComponents<iTween>();
        Component[] array = components;
        for (int i = 0; i < array.Length; i++)
        {
            iTween iTween = (iTween)array[i];
            string text = iTween.type + iTween.method;
            text = text.Substring(0, type.Length);
            if (text.ToLower() == type.ToLower())
            {
                iTween.Dispose();
            }
        }
        if (includechildren)
        {
            foreach (Transform item in target.transform)
            {
                iTween.Stop(item.gameObject, type, true);
            }
        }
    }

    public static void StopByName(GameObject target, string name, bool includechildren)
    {
        Component[] components = target.GetComponents<iTween>();
        Component[] array = components;
        for (int i = 0; i < array.Length; i++)
        {
            iTween iTween = (iTween)array[i];
            if (iTween._name == name)
            {
                iTween.Dispose();
            }
        }
        if (includechildren)
        {
            foreach (Transform item in target.transform)
            {
                iTween.StopByName(item.gameObject, name, true);
            }
        }
    }

    public static Hashtable Hash(params object[] args)
    {
        Hashtable hashtable = new Hashtable(args.Length / 2);
        if (args.Length % 2 != 0)
        {
            UnityEngine.Debug.LogError("Tween Error: Hash requires an even number of arguments!");
            return null;
        }
        for (int i = 0; i < args.Length - 1; i += 2)
        {
            hashtable.Add(args[i], args[i + 1]);
        }
        return hashtable;
    }

    private void Awake()
    {
        this.thisTransform = base.transform;
        this.RetrieveArgs();
        this.lastRealTime = Time.realtimeSinceStartup;
    }

    private IEnumerator Start()
    {
        if (this.delay > 0f)
        {
            yield return (object)base.StartCoroutine("TweenDelay");
        }
        this.TweenStart();
    }

    private void Update()
    {
        if (this.isRunning && !this.physics)
        {
            if (!this.reverse)
            {
                if (this.percentage < 1f)
                {
                    this.TweenUpdate();
                }
                else
                {
                    this.TweenComplete();
                }
            }
            else if (this.percentage > 0f)
            {
                this.TweenUpdate();
            }
            else
            {
                this.TweenComplete();
            }
        }
    }

    private void FixedUpdate()
    {
        if (this.isRunning && this.physics)
        {
            if (!this.reverse)
            {
                if (this.percentage < 1f)
                {
                    this.TweenUpdate();
                }
                else
                {
                    this.TweenComplete();
                }
            }
            else if (this.percentage > 0f)
            {
                this.TweenUpdate();
            }
            else
            {
                this.TweenComplete();
            }
        }
    }

    private void LateUpdate()
    {
        if (this.tweenArguments.Contains("looktarget") && this.isRunning)
        {
            if (!(this.type == "move") && !(this.type == "shake") && !(this.type == "punch"))
            {
                return;
            }
            iTween.LookUpdate(base.gameObject, this.tweenArguments);
        }
    }

    private void OnEnable()
    {
        if (this.isRunning)
        {
            this.EnableKinematic();
        }
        if (this.isPaused)
        {
            this.isPaused = false;
            if (this.delay > 0f)
            {
                this.wasPaused = true;
                this.ResumeDelay();
            }
        }
    }

    private void OnDisable()
    {
        this.DisableKinematic();
    }

    private static void DrawLineHelper(Vector3[] line, Color color, string method)
    {
        Gizmos.color = color;
        for (int i = 0; i < line.Length - 1; i++)
        {
            if (method == "gizmos")
            {
                Gizmos.DrawLine(line[i], line[i + 1]);
            }
            else if (method == "handles")
            {
                UnityEngine.Debug.LogError("iTween Error: Drawing a line with Handles is temporarily disabled because of compatability issues with Unity 2.6!");
            }
        }
    }

    private static void DrawPathHelper(Vector3[] path, Color color, string method)
    {
        Vector3[] pts = iTween.PathControlPointGenerator(path);
        Vector3 to = iTween.Interp(pts, 0f);
        Gizmos.color = color;
        int num = path.Length * 20;
        for (int i = 1; i <= num; i++)
        {
            float t = (float)i / (float)num;
            Vector3 vector = iTween.Interp(pts, t);
            if (method == "gizmos")
            {
                Gizmos.DrawLine(vector, to);
            }
            else if (method == "handles")
            {
                UnityEngine.Debug.LogError("iTween Error: Drawing a path with Handles is temporarily disabled because of compatability issues with Unity 2.6!");
            }
            to = vector;
        }
    }

    private static Vector3[] PathControlPointGenerator(Vector3[] path)
    {
        int num = 2;
        Vector3[] array = new Vector3[path.Length + num];
        Array.Copy(path, 0, array, 1, path.Length);
        array[0] = array[1] + (array[1] - array[2]);
        array[array.Length - 1] = array[array.Length - 2] + (array[array.Length - 2] - array[array.Length - 3]);
        if (array[1] == array[array.Length - 2])
        {
            Vector3[] array2 = new Vector3[array.Length];
            Array.Copy(array, array2, array.Length);
            array2[0] = array2[array2.Length - 3];
            array2[array2.Length - 1] = array2[2];
            array = new Vector3[array2.Length];
            Array.Copy(array2, array, array2.Length);
        }
        return array;
    }

    private static Vector3 Interp(Vector3[] pts, float t)
    {
        int num = pts.Length - 3;
        int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
        float num3 = t * (float)num - (float)num2;
        Vector3 a = pts[num2];
        Vector3 a2 = pts[num2 + 1];
        Vector3 vector = pts[num2 + 2];
        Vector3 b = pts[num2 + 3];
        return 0.5f * ((-a + 3f * a2 - 3f * vector + b) * (num3 * num3 * num3) + (2f * a - 5f * a2 + 4f * vector - b) * (num3 * num3) + (-a + vector) * num3 + 2f * a2);
    }

    private static void Launch(GameObject target, Hashtable args)
    {
        if (!args.Contains("id"))
        {
            args["id"] = iTween.GenerateID();
        }
        if (!args.Contains("target"))
        {
            args["target"] = target;
        }
        iTween.tweens.Insert(0, args);
        target.AddComponent<iTween>();
    }

    private static Hashtable CleanArgs(Hashtable args)
    {
        Hashtable hashtable = new Hashtable(args.Count);
        Hashtable hashtable2 = new Hashtable(args.Count);
        IDictionaryEnumerator enumerator = args.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                DictionaryEntry dictionaryEntry = (DictionaryEntry)enumerator.Current;
                hashtable.Add(dictionaryEntry.Key, dictionaryEntry.Value);
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
        IDictionaryEnumerator enumerator2 = hashtable.GetEnumerator();
        try
        {
            while (enumerator2.MoveNext())
            {
                DictionaryEntry dictionaryEntry2 = (DictionaryEntry)enumerator2.Current;
                if (dictionaryEntry2.Value.GetType() == typeof(int))
                {
                    int num = (int)dictionaryEntry2.Value;
                    float num2 = (float)num;
                    args[dictionaryEntry2.Key] = num2;
                }
                if (dictionaryEntry2.Value.GetType() == typeof(double))
                {
                    double num3 = (double)dictionaryEntry2.Value;
                    float num4 = (float)num3;
                    args[dictionaryEntry2.Key] = num4;
                }
            }
        }
        finally
        {
            IDisposable disposable2 = enumerator2 as IDisposable;
            if (disposable2 != null)
            {
                disposable2.Dispose();
            }
        }
        IDictionaryEnumerator enumerator3 = args.GetEnumerator();
        try
        {
            while (enumerator3.MoveNext())
            {
                DictionaryEntry dictionaryEntry3 = (DictionaryEntry)enumerator3.Current;
                hashtable2.Add(dictionaryEntry3.Key.ToString().ToLower(), dictionaryEntry3.Value);
            }
        }
        finally
        {
            IDisposable disposable3 = enumerator3 as IDisposable;
            if (disposable3 != null)
            {
                disposable3.Dispose();
            }
        }
        args = hashtable2;
        return args;
    }

    private static string GenerateID()
    {
        return Guid.NewGuid().ToString();
    }

    private void RetrieveArgs()
    {
        List<Hashtable>.Enumerator enumerator = iTween.tweens.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Hashtable current = enumerator.Current;
                if ((UnityEngine.Object)(GameObject)current["target"] == (UnityEngine.Object)base.gameObject)
                {
                    this.tweenArguments = current;
                    break;
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        this.id = (string)this.tweenArguments["id"];
        this.type = (string)this.tweenArguments["type"];
        this._name = (string)this.tweenArguments["name"];
        this.method = (string)this.tweenArguments["method"];
        if (this.tweenArguments.Contains("time"))
        {
            this.time = (float)this.tweenArguments["time"];
        }
        else
        {
            this.time = Defaults.time;
        }
        if ((UnityEngine.Object)base.GetComponent<Rigidbody>() != (UnityEngine.Object)null)
        {
            this.physics = true;
        }
        if (this.tweenArguments.Contains("delay"))
        {
            this.delay = (float)this.tweenArguments["delay"];
        }
        else
        {
            this.delay = Defaults.delay;
        }
        if (this.tweenArguments.Contains("namedcolorvalue"))
        {
            if (this.tweenArguments["namedcolorvalue"].GetType() == typeof(NamedValueColor))
            {
                this.namedcolorvalue = (NamedValueColor)(int)this.tweenArguments["namedcolorvalue"];
            }
            else
            {
                try
                {
                    this.namedcolorvalue = (NamedValueColor)(int)Enum.Parse(typeof(NamedValueColor), (string)this.tweenArguments["namedcolorvalue"], true);
                }
                catch
                {
                    UnityEngine.Debug.LogWarning("iTween: Unsupported namedcolorvalue supplied! Default will be used.");
                    this.namedcolorvalue = NamedValueColor._Color;
                }
            }
        }
        else
        {
            this.namedcolorvalue = Defaults.namedColorValue;
        }
        if (this.tweenArguments.Contains("looptype"))
        {
            if (this.tweenArguments["looptype"].GetType() == typeof(LoopType))
            {
                this.loopType = (LoopType)(int)this.tweenArguments["looptype"];
            }
            else
            {
                try
                {
                    this.loopType = (LoopType)(int)Enum.Parse(typeof(LoopType), (string)this.tweenArguments["looptype"], true);
                }
                catch
                {
                    UnityEngine.Debug.LogWarning("iTween: Unsupported loopType supplied! Default will be used.");
                    this.loopType = LoopType.none;
                }
            }
        }
        else
        {
            this.loopType = LoopType.none;
        }
        if (this.tweenArguments.Contains("easetype"))
        {
            if (this.tweenArguments["easetype"].GetType() == typeof(EaseType))
            {
                this.easeType = (EaseType)(int)this.tweenArguments["easetype"];
            }
            else
            {
                try
                {
                    this.easeType = (EaseType)(int)Enum.Parse(typeof(EaseType), (string)this.tweenArguments["easetype"], true);
                }
                catch
                {
                    UnityEngine.Debug.LogWarning("iTween: Unsupported easeType supplied! Default will be used.");
                    this.easeType = Defaults.easeType;
                }
            }
        }
        else
        {
            this.easeType = Defaults.easeType;
        }
        if (this.tweenArguments.Contains("space"))
        {
            if (this.tweenArguments["space"].GetType() == typeof(Space))
            {
                this.space = (Space)(int)this.tweenArguments["space"];
            }
            else
            {
                try
                {
                    this.space = (Space)(int)Enum.Parse(typeof(Space), (string)this.tweenArguments["space"], true);
                }
                catch
                {
                    UnityEngine.Debug.LogWarning("iTween: Unsupported space supplied! Default will be used.");
                    this.space = Defaults.space;
                }
            }
        }
        else
        {
            this.space = Defaults.space;
        }
        if (this.tweenArguments.Contains("islocal"))
        {
            this.isLocal = (bool)this.tweenArguments["islocal"];
        }
        else
        {
            this.isLocal = Defaults.isLocal;
        }
        if (this.tweenArguments.Contains("ignoretimescale"))
        {
            this.useRealTime = (bool)this.tweenArguments["ignoretimescale"];
        }
        else
        {
            this.useRealTime = Defaults.useRealTime;
        }
        this.GetEasingFunction();
    }

    private void GetEasingFunction()
    {
        switch (this.easeType)
        {
            case EaseType.easeInQuad:
                this.ease = new EasingFunction(this.easeInQuad);
                break;
            case EaseType.easeOutQuad:
                this.ease = new EasingFunction(this.easeOutQuad);
                break;
            case EaseType.easeInOutQuad:
                this.ease = new EasingFunction(this.easeInOutQuad);
                break;
            case EaseType.easeInCubic:
                this.ease = new EasingFunction(this.easeInCubic);
                break;
            case EaseType.easeOutCubic:
                this.ease = new EasingFunction(this.easeOutCubic);
                break;
            case EaseType.easeInOutCubic:
                this.ease = new EasingFunction(this.easeInOutCubic);
                break;
            case EaseType.easeInQuart:
                this.ease = new EasingFunction(this.easeInQuart);
                break;
            case EaseType.easeOutQuart:
                this.ease = new EasingFunction(this.easeOutQuart);
                break;
            case EaseType.easeInOutQuart:
                this.ease = new EasingFunction(this.easeInOutQuart);
                break;
            case EaseType.easeInQuint:
                this.ease = new EasingFunction(this.easeInQuint);
                break;
            case EaseType.easeOutQuint:
                this.ease = new EasingFunction(this.easeOutQuint);
                break;
            case EaseType.easeInOutQuint:
                this.ease = new EasingFunction(this.easeInOutQuint);
                break;
            case EaseType.easeInSine:
                this.ease = new EasingFunction(this.easeInSine);
                break;
            case EaseType.easeOutSine:
                this.ease = new EasingFunction(this.easeOutSine);
                break;
            case EaseType.easeInOutSine:
                this.ease = new EasingFunction(this.easeInOutSine);
                break;
            case EaseType.easeInExpo:
                this.ease = new EasingFunction(this.easeInExpo);
                break;
            case EaseType.easeOutExpo:
                this.ease = new EasingFunction(this.easeOutExpo);
                break;
            case EaseType.easeInOutExpo:
                this.ease = new EasingFunction(this.easeInOutExpo);
                break;
            case EaseType.easeInCirc:
                this.ease = new EasingFunction(this.easeInCirc);
                break;
            case EaseType.easeOutCirc:
                this.ease = new EasingFunction(this.easeOutCirc);
                break;
            case EaseType.easeInOutCirc:
                this.ease = new EasingFunction(this.easeInOutCirc);
                break;
            case EaseType.linear:
                this.ease = new EasingFunction(this.linear);
                break;
            case EaseType.spring:
                this.ease = new EasingFunction(this.spring);
                break;
            case EaseType.easeInBounce:
                this.ease = new EasingFunction(this.easeInBounce);
                break;
            case EaseType.easeOutBounce:
                this.ease = new EasingFunction(this.easeOutBounce);
                break;
            case EaseType.easeInOutBounce:
                this.ease = new EasingFunction(this.easeInOutBounce);
                break;
            case EaseType.easeInBack:
                this.ease = new EasingFunction(this.easeInBack);
                break;
            case EaseType.easeOutBack:
                this.ease = new EasingFunction(this.easeOutBack);
                break;
            case EaseType.easeInOutBack:
                this.ease = new EasingFunction(this.easeInOutBack);
                break;
            case EaseType.easeInElastic:
                this.ease = new EasingFunction(this.easeInElastic);
                break;
            case EaseType.easeOutElastic:
                this.ease = new EasingFunction(this.easeOutElastic);
                break;
            case EaseType.easeInOutElastic:
                this.ease = new EasingFunction(this.easeInOutElastic);
                break;
        }
    }

    private void UpdatePercentage()
    {
        if (this.useRealTime)
        {
            this.runningTime += Time.realtimeSinceStartup - this.lastRealTime;
        }
        else
        {
            this.runningTime += Time.deltaTime;
        }
        if (this.reverse)
        {
            this.percentage = 1f - this.runningTime / this.time;
        }
        else
        {
            this.percentage = this.runningTime / this.time;
        }
        this.lastRealTime = Time.realtimeSinceStartup;
    }

    private void CallBack(string callbackType)
    {
        if (this.tweenArguments.Contains(callbackType) && !this.tweenArguments.Contains("ischild"))
        {
            GameObject gameObject = (!this.tweenArguments.Contains(callbackType + "target")) ? base.gameObject : ((GameObject)this.tweenArguments[callbackType + "target"]);
            if (this.tweenArguments[callbackType].GetType() == typeof(string))
            {
                gameObject.SendMessage((string)this.tweenArguments[callbackType], this.tweenArguments[callbackType + "params"], SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                UnityEngine.Debug.LogError("iTween Error: Callback method references must be passed as a String!");
                UnityEngine.Object.Destroy(this);
            }
        }
    }

    private void Dispose()
    {
        for (int i = 0; i < iTween.tweens.Count; i++)
        {
            Hashtable hashtable = iTween.tweens[i];
            if ((string)hashtable["id"] == this.id)
            {
                iTween.tweens.RemoveAt(i);
                break;
            }
        }
        UnityEngine.Object.Destroy(this);
    }

    private void ConflictCheck()
    {
        Component[] components = base.GetComponents<iTween>();
        Component[] array = components;
        int num = 0;
        iTween iTween;
        while (true)
        {
            if (num < array.Length)
            {
                iTween = (iTween)array[num];
                if (!(iTween.type == "value"))
                {
                    if (!iTween.isRunning || !(iTween.type == this.type))
                    {
                        goto IL_014c;
                    }
                    if (!(iTween.method != this.method))
                    {
                        if (iTween.tweenArguments.Count == this.tweenArguments.Count)
                        {
                            IDictionaryEnumerator enumerator = this.tweenArguments.GetEnumerator();
                            try
                            {
                                while (enumerator.MoveNext())
                                {
                                    DictionaryEntry dictionaryEntry = (DictionaryEntry)enumerator.Current;
                                    if (!iTween.tweenArguments.Contains(dictionaryEntry.Key))
                                    {
                                        iTween.Dispose();
                                        return;
                                    }
                                    if (!iTween.tweenArguments[dictionaryEntry.Key].Equals(this.tweenArguments[dictionaryEntry.Key]) && (string)dictionaryEntry.Key != "id")
                                    {
                                        iTween.Dispose();
                                        return;
                                    }
                                }
                            }
                            finally
                            {
                                IDisposable disposable = enumerator as IDisposable;
                                if (disposable != null)
                                {
                                    disposable.Dispose();
                                }
                            }
                            this.Dispose();
                            goto IL_014c;
                        }
                        break;
                    }
                }
            }
            return;
            IL_014c:
            num++;
        }
        iTween.Dispose();
    }

    private void EnableKinematic()
    {
    }

    private void DisableKinematic()
    {
    }

    private void ResumeDelay()
    {
        base.StartCoroutine("TweenDelay");
    }

    private float linear(float start, float end, float value)
    {
        return Mathf.Lerp(start, end, value);
    }

    private float clerp(float start, float end, float value)
    {
        float num = 0f;
        float num2 = 360f;
        float num3 = Mathf.Abs((num2 - num) * 0.5f);
        float num4 = 0f;
        float num5 = 0f;
        if (end - start < 0f - num3)
        {
            num5 = (num2 - start + end) * value;
            return start + num5;
        }
        if (end - start > num3)
        {
            num5 = (0f - (num2 - end + start)) * value;
            return start + num5;
        }
        return start + (end - start) * value;
    }

    private float spring(float start, float end, float value)
    {
        value = Mathf.Clamp01(value);
        value = (Mathf.Sin(value * 3.14159274f * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + 1.2f * (1f - value));
        return start + (end - start) * value;
    }

    private float easeInQuad(float start, float end, float value)
    {
        end -= start;
        return end * value * value + start;
    }

    private float easeOutQuad(float start, float end, float value)
    {
        end -= start;
        return (0f - end) * value * (value - 2f) + start;
    }

    private float easeInOutQuad(float start, float end, float value)
    {
        value /= 0.5f;
        end -= start;
        if (value < 1f)
        {
            return end * 0.5f * value * value + start;
        }
        value -= 1f;
        return (0f - end) * 0.5f * (value * (value - 2f) - 1f) + start;
    }

    private float easeInCubic(float start, float end, float value)
    {
        end -= start;
        return end * value * value * value + start;
    }

    private float easeOutCubic(float start, float end, float value)
    {
        value -= 1f;
        end -= start;
        return end * (value * value * value + 1f) + start;
    }

    private float easeInOutCubic(float start, float end, float value)
    {
        value /= 0.5f;
        end -= start;
        if (value < 1f)
        {
            return end * 0.5f * value * value * value + start;
        }
        value -= 2f;
        return end * 0.5f * (value * value * value + 2f) + start;
    }

    private float easeInQuart(float start, float end, float value)
    {
        end -= start;
        return end * value * value * value * value + start;
    }

    private float easeOutQuart(float start, float end, float value)
    {
        value -= 1f;
        end -= start;
        return (0f - end) * (value * value * value * value - 1f) + start;
    }

    private float easeInOutQuart(float start, float end, float value)
    {
        value /= 0.5f;
        end -= start;
        if (value < 1f)
        {
            return end * 0.5f * value * value * value * value + start;
        }
        value -= 2f;
        return (0f - end) * 0.5f * (value * value * value * value - 2f) + start;
    }

    private float easeInQuint(float start, float end, float value)
    {
        end -= start;
        return end * value * value * value * value * value + start;
    }

    private float easeOutQuint(float start, float end, float value)
    {
        value -= 1f;
        end -= start;
        return end * (value * value * value * value * value + 1f) + start;
    }

    private float easeInOutQuint(float start, float end, float value)
    {
        value /= 0.5f;
        end -= start;
        if (value < 1f)
        {
            return end * 0.5f * value * value * value * value * value + start;
        }
        value -= 2f;
        return end * 0.5f * (value * value * value * value * value + 2f) + start;
    }

    private float easeInSine(float start, float end, float value)
    {
        end -= start;
        return (0f - end) * Mathf.Cos(value * 1.57079637f) + end + start;
    }

    private float easeOutSine(float start, float end, float value)
    {
        end -= start;
        return end * Mathf.Sin(value * 1.57079637f) + start;
    }

    private float easeInOutSine(float start, float end, float value)
    {
        end -= start;
        return (0f - end) * 0.5f * (Mathf.Cos(3.14159274f * value) - 1f) + start;
    }

    private float easeInExpo(float start, float end, float value)
    {
        end -= start;
        return end * Mathf.Pow(2f, 10f * (value - 1f)) + start;
    }

    private float easeOutExpo(float start, float end, float value)
    {
        end -= start;
        return end * (0f - Mathf.Pow(2f, -10f * value) + 1f) + start;
    }

    private float easeInOutExpo(float start, float end, float value)
    {
        value /= 0.5f;
        end -= start;
        if (value < 1f)
        {
            return end * 0.5f * Mathf.Pow(2f, 10f * (value - 1f)) + start;
        }
        value -= 1f;
        return end * 0.5f * (0f - Mathf.Pow(2f, -10f * value) + 2f) + start;
    }

    private float easeInCirc(float start, float end, float value)
    {
        end -= start;
        return (0f - end) * (Mathf.Sqrt(1f - value * value) - 1f) + start;
    }

    private float easeOutCirc(float start, float end, float value)
    {
        value -= 1f;
        end -= start;
        return end * Mathf.Sqrt(1f - value * value) + start;
    }

    private float easeInOutCirc(float start, float end, float value)
    {
        value /= 0.5f;
        end -= start;
        if (value < 1f)
        {
            return (0f - end) * 0.5f * (Mathf.Sqrt(1f - value * value) - 1f) + start;
        }
        value -= 2f;
        return end * 0.5f * (Mathf.Sqrt(1f - value * value) + 1f) + start;
    }

    private float easeInBounce(float start, float end, float value)
    {
        end -= start;
        float num = 1f;
        return end - this.easeOutBounce(0f, end, num - value) + start;
    }

    private float easeOutBounce(float start, float end, float value)
    {
        value /= 1f;
        end -= start;
        if (value < 0.363636374f)
        {
            return end * (7.5625f * value * value) + start;
        }
        if (value < 0.727272749f)
        {
            value -= 0.545454562f;
            return end * (7.5625f * value * value + 0.75f) + start;
        }
        if ((double)value < 0.90909090909090906)
        {
            value -= 0.8181818f;
            return end * (7.5625f * value * value + 0.9375f) + start;
        }
        value -= 0.954545438f;
        return end * (7.5625f * value * value + 0.984375f) + start;
    }

    private float easeInOutBounce(float start, float end, float value)
    {
        end -= start;
        float num = 1f;
        if (value < num * 0.5f)
        {
            return this.easeInBounce(0f, end, value * 2f) * 0.5f + start;
        }
        return this.easeOutBounce(0f, end, value * 2f - num) * 0.5f + end * 0.5f + start;
    }

    private float easeInBack(float start, float end, float value)
    {
        end -= start;
        value /= 1f;
        float num = 1.70158f;
        return end * value * value * ((num + 1f) * value - num) + start;
    }

    private float easeOutBack(float start, float end, float value)
    {
        float num = 1.70158f;
        end -= start;
        value -= 1f;
        return end * (value * value * ((num + 1f) * value + num) + 1f) + start;
    }

    private float easeInOutBack(float start, float end, float value)
    {
        float num = 1.70158f;
        end -= start;
        value /= 0.5f;
        if (value < 1f)
        {
            num *= 1.525f;
            return end * 0.5f * (value * value * ((num + 1f) * value - num)) + start;
        }
        value -= 2f;
        num *= 1.525f;
        return end * 0.5f * (value * value * ((num + 1f) * value + num) + 2f) + start;
    }

    private float punch(float amplitude, float value)
    {
        float num = 9f;
        if (value == 0f)
        {
            return 0f;
        }
        if (value == 1f)
        {
            return 0f;
        }
        float num2 = 0.3f;
        num = num2 / 6.28318548f * Mathf.Asin(0f);
        return amplitude * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * 1f - num) * 6.28318548f / num2);
    }

    private float easeInElastic(float start, float end, float value)
    {
        end -= start;
        float num = 1f;
        float num2 = num * 0.3f;
        float num3 = 0f;
        float num4 = 0f;
        if (value == 0f)
        {
            return start;
        }
        if ((value /= num) == 1f)
        {
            return start + end;
        }
        if (num4 == 0f || num4 < Mathf.Abs(end))
        {
            num4 = end;
            num3 = num2 / 4f;
        }
        else
        {
            num3 = num2 / 6.28318548f * Mathf.Asin(end / num4);
        }
        return 0f - num4 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num3) * 6.28318548f / num2) + start;
    }

    private float easeOutElastic(float start, float end, float value)
    {
        end -= start;
        float num = 1f;
        float num2 = num * 0.3f;
        float num3 = 0f;
        float num4 = 0f;
        if (value == 0f)
        {
            return start;
        }
        if ((value /= num) == 1f)
        {
            return start + end;
        }
        if (num4 == 0f || num4 < Mathf.Abs(end))
        {
            num4 = end;
            num3 = num2 * 0.25f;
        }
        else
        {
            num3 = num2 / 6.28318548f * Mathf.Asin(end / num4);
        }
        return num4 * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * num - num3) * 6.28318548f / num2) + end + start;
    }

    private float easeInOutElastic(float start, float end, float value)
    {
        end -= start;
        float num = 1f;
        float num2 = num * 0.3f;
        float num3 = 0f;
        float num4 = 0f;
        if (value == 0f)
        {
            return start;
        }
        if ((value /= num * 0.5f) == 2f)
        {
            return start + end;
        }
        if (num4 == 0f || num4 < Mathf.Abs(end))
        {
            num4 = end;
            num3 = num2 / 4f;
        }
        else
        {
            num3 = num2 / 6.28318548f * Mathf.Asin(end / num4);
        }
        if (value < 1f)
        {
            return -0.5f * (num4 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num3) * 6.28318548f / num2)) + start;
        }
        return num4 * Mathf.Pow(2f, -10f * (value -= 1f)) * Mathf.Sin((value * num - num3) * 6.28318548f / num2) * 0.5f + end + start;
    }
}


