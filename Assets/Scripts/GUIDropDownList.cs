// ILSpyBased#2
using UnityEngine;

public class GUIDropDownList
{
    public class GUIDropDownEntry
    {
        private string text = string.Empty;

        private object tag;

        private bool enable = true;

        private Rect position = new Rect(0f, 0f, 1f, 1f);

        public string Text
        {
            get
            {
                return this.text;
            }
        }

        public object Tag
        {
            get
            {
                return this.tag;
            }
        }

        public bool Enable
        {
            get
            {
                return this.enable;
            }
        }

        public Rect Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;
            }
        }

        public GUIDropDownEntry(string text)
        {
            this.text = text;
        }

        public GUIDropDownEntry(string text, bool enable)
        {
            this.text = text;
            this.enable = enable;
        }

        public GUIDropDownEntry(string text, object tag)
        {
            this.text = text;
            this.tag = tag;
        }

        public GUIDropDownEntry(string text, object tag, bool enable)
        {
            this.text = text;
            this.tag = tag;
            this.enable = enable;
        }

        public GUIDropDownEntry(object entry)
        {
            this.text = entry.ToString();
            this.tag = entry;
        }

        public GUIDropDownEntry(object entry, bool enable)
        {
            this.text = entry.ToString();
            this.tag = entry;
            this.enable = enable;
        }

        public override string ToString()
        {
            return this.text;
        }
    }

    public class GUIDropDownSetting
    {
        public Rect Position = new Rect(0f, 0f, 0f, 0f);

        private bool showList;

        public float HidedTime;

        public float ShowedTime;

        private int listEntry = -1;

        public GUIDropDownEntry[] List;

        public ListCallBack Callback;

        private object owner;

        private GUISkin skin = GUISkinManager.DropDownList;

        private bool isValidRect;

        private bool isInitEntryPositions;

        public bool ShowList
        {
            get
            {
                return this.showList;
            }
            set
            {
                this.showList = value;
                if (value)
                {
                    this.ShowedTime = Time.time;
                }
                else
                {
                    this.HidedTime = Time.time;
                }
            }
        }

        public int ListEntry
        {
            get
            {
                return this.listEntry;
            }
            set
            {
                if (value != this.listEntry)
                {
                    this.listEntry = value;
                    if (this.Callback != null)
                    {
                        this.Callback(this.Owner, this.List[this.listEntry]);
                    }
                }
            }
        }

        public object Owner
        {
            get
            {
                return this.owner;
            }
        }

        public GUISkin Skin
        {
            get
            {
                return this.skin;
            }
        }

        public bool ValidRect
        {
            get
            {
                return this.isValidRect;
            }
        }

        public bool IsInitEntryPositions
        {
            get
            {
                return this.isInitEntryPositions;
            }
            set
            {
                this.isInitEntryPositions = value;
            }
        }

        public GUIDropDownSetting(object owner, bool showList, GUIDropDownEntry[] list, ListCallBack callBack)
        {
            this.owner = owner;
            this.ShowList = showList;
            this.ListEntry = -1;
            this.List = list;
            this.Callback = callBack;
        }

        public void SetValidRect(Rect validRect)
        {
            this.Position = validRect;
            this.isValidRect = true;
        }
    }

    public delegate void ListCallBack(object sender, object entry);

    private static int popupListHash = "GUIDropDownList".GetHashCode();

    private static GUIDropDownSetting currentSetting = null;

    private static float lastClickedTime = 0f;

    private static Vector2 scrollView = new Vector2(0f, 0f);

    private static bool enableScrollView = false;

    private static void OnClick(int index)
    {
        if (GUIDropDownList.currentSetting.List[index].Enable)
        {
            GUIDropDownList.currentSetting.ListEntry = index;
            if (GUIDropDownList.currentSetting.ShowList)
            {
                GUIDropDownList.currentSetting.ShowList = false;
            }
        }
    }

    public static bool List(GUIDropDownSetting setting)
    {
        GUIDropDownList.currentSetting = setting;
        GUI.depth = 0;
        int controlID = GUIUtility.GetControlID(GUIDropDownList.popupListHash, FocusType.Passive);
        bool flag = false;
        switch (Event.current.GetTypeForControl(controlID))
        {
            case EventType.Used:
                if (GUIDropDownList.lastClickedTime + 0.2f < Time.time)
                {
                    GUIDropDownList.lastClickedTime = Time.time;
                    if (GUIDropDownList.currentSetting.Position.Contains(Event.current.mousePosition))
                    {
                        Vector2 mousePosition = Event.current.mousePosition;
                        float num = mousePosition.y - GUIDropDownList.currentSetting.Position.y;
                        for (int i = 0; i < GUIDropDownList.currentSetting.List.Length; i++)
                        {
                            if (GUIDropDownList.currentSetting.List[i].Position.y <= num && GUIDropDownList.currentSetting.List[i].Position.y + GUIDropDownList.currentSetting.List[i].Position.height >= num)
                            {
                                GUIDropDownList.OnClick(i);
                            }
                        }
                    }
                    else if (GUIDropDownList.currentSetting.ShowList && GUIDropDownList.currentSetting.ShowedTime + 2f < Time.time)
                    {
                        UnityEngine.Debug.LogError("HIDE dropDown" + string.Format(" showedTime:{0}, time:{1}", GUIDropDownList.currentSetting.ShowedTime, Time.time));
                        flag = true;
                    }
                }
                break;
            case EventType.MouseDown:
                if (GUIDropDownList.currentSetting.Position.Contains(Event.current.mousePosition))
                {
                    GUIUtility.hotControl = controlID;
                    GUIDropDownList.currentSetting.ShowList = true;
                }
                break;
            case EventType.MouseUp:
                if (GUIDropDownList.currentSetting.ShowList)
                {
                    flag = true;
                }
                break;
        }
        float num2 = 0f;
        GUILayout.BeginArea(GUIDropDownList.currentSetting.Position, GUIContent.none, GUIStyle.none);
        if (GUIDropDownList.enableScrollView)
        {
            GUIDropDownList.scrollView = GUILayout.BeginScrollView(GUIDropDownList.scrollView, false, true, GUILayout.Height(GUIDropDownList.currentSetting.Position.height));
        }
        GUILayout.BeginVertical(GUIContent.none, GUIDropDownList.currentSetting.Skin.GetStyle("box"));
        Rect lastRect;
        for (int j = 0; j < GUIDropDownList.currentSetting.List.Length; j++)
        {
            if (GUIDropDownList.currentSetting.List[j].Enable)
            {
                if (GUILayout.Button(GUIDropDownList.currentSetting.List[j].Text, GUIDropDownList.currentSetting.Skin.GetStyle("button")))
                {
                    GUIDropDownList.OnClick(j);
                }
            }
            else
            {
                GUILayout.Button(GUIDropDownList.currentSetting.List[j].Text, GUIDropDownList.currentSetting.Skin.GetStyle("buttonDisable"));
            }
            lastRect = GUILayoutUtility.GetLastRect();
            num2 += lastRect.height;
            if (Event.current.type == EventType.Repaint && !GUIDropDownList.currentSetting.IsInitEntryPositions)
            {
                GUIDropDownList.currentSetting.List[j].Position = lastRect;
            }
            if (GUIDropDownList.currentSetting.Position.y + lastRect.height > (float)Screen.height)
            {
                GUIDropDownList.currentSetting.Position.y -= GUIDropDownList.currentSetting.Position.y + lastRect.height - (float)Screen.height;
            }
        }
        GUILayout.EndVertical();
        lastRect = GUILayoutUtility.GetLastRect();
        if (GUIDropDownList.enableScrollView)
        {
            GUILayout.EndScrollView();
        }
        if (Event.current.type == EventType.Repaint)
        {
            if (num2 > GUIDropDownList.currentSetting.Position.height)
            {
                GUIDropDownList.enableScrollView = true;
            }
            else
            {
                GUIDropDownList.enableScrollView = false;
            }
        }
        lastRect = GUILayoutUtility.GetLastRect();
        if (GUIDropDownList.currentSetting.Position.y + lastRect.height > (float)Screen.height)
        {
            GUIDropDownList.currentSetting.Position.y -= GUIDropDownList.currentSetting.Position.y + lastRect.height - (float)Screen.height;
        }
        GUILayout.EndArea();
        if (Event.current.type == EventType.Repaint && !GUIDropDownList.currentSetting.IsInitEntryPositions)
        {
            GUIDropDownList.currentSetting.IsInitEntryPositions = true;
        }
        if (flag)
        {
            GUIDropDownList.currentSetting.ShowList = false;
        }
        return flag;
    }
}


