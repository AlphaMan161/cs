// ILSpyBased#2
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationWindow : MonoBehaviour
{
    private static ArrayList notifications = new ArrayList();

    private static NotificationWindow hInstance;

    public GUISkin mainSkin;

    private Notification currentNotification;

    private static object lockCurrentNotification = new object();

    public static bool IsShow
    {
        get
        {
            if ((UnityEngine.Object)NotificationWindow.hInstance == (UnityEngine.Object)null)
            {
                return false;
            }
            return NotificationWindow.hInstance.currentNotification != null;
        }
    }

    public static void Add(Notification notification)
    {
        if (NotificationWindow.notifications == null)
        {
            NotificationWindow.notifications = new ArrayList();
        }
        foreach (Notification notification2 in NotificationWindow.notifications)
        {
            if (notification2.Message == notification.Message && notification2.NType == Notification.Type.CONNECTION_LOST)
            {
                return;
            }
        }
        NotificationWindow.notifications.Add(notification);
    }

    private void Start()
    {
        NotificationWindow.hInstance = this;
    }

    private void OnGUI()
    {
        if (!SetNamePopup.Show && !OptionPopup.Show && !LoadingMapPopup.Show)
        {
            GUISkin skin = GUI.skin;
            GUI.skin = this.mainSkin;
            if ((UnityEngine.Object)NotificationWindow.hInstance != (UnityEngine.Object)null && NotificationWindow.notifications.Count > 0)
            {
                this.currentNotification = (Notification)NotificationWindow.notifications[0];
                GUIHover.Enable = false;
                float num = (float)Screen.width;
                Vector2 windowSize = this.currentNotification.WindowSize;
                float x = (num - windowSize.x) / 2f;
                float num2 = (float)Screen.height;
                Vector2 windowSize2 = this.currentNotification.WindowSize;
                float y = (num2 - windowSize2.y) / 2f;
                Vector2 windowSize3 = this.currentNotification.WindowSize;
                float x2 = windowSize3.x;
                Vector2 windowSize4 = this.currentNotification.WindowSize;
                GUI.Window(2, new Rect(x, y, x2, windowSize4.y), new GUI.WindowFunction(this.draw_popup), GUIContent.none, GUISkinManager.Backgound.window);
                GUI.FocusWindow(2);
                if (this.currentNotification.CanClose)
                {
                    GUI.Button(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), GUIContent.none, GUIStyle.none);
                }
            }
            GUI.skin = skin;
        }
    }

    public static void Complete(Notification completedNotification)
    {
        NotificationWindow.notifications.Remove(completedNotification);
        if (NotificationWindow.hInstance.currentNotification == completedNotification)
        {
            NotificationWindow.hInstance.currentNotification = null;
        }
        GUIHover.Enable = true;
    }

    private int draw_CCItem(CCItem item)
    {
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        if (item.GetType() == typeof(Wear))
        {
            GUILayout.Label(item.Ico, GUISkinManager.Backgound.GetStyle("itemRight"), GUILayout.Width(97f), GUILayout.Height(85f));
        }
        else
        {
            GUILayout.Label(item.Ico, GUISkinManager.Backgound.GetStyle("itemRight02"), GUILayout.Width(97f), GUILayout.Height(85f));
        }
        GUILayout.Space(10f);
        bool flag = (byte)(item.Shop_Cost.isTime1 ? 1 : 0) != 0;
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        if (flag)
        {
            List<string> list = new List<string>();
            List<string> list2 = new List<string>();
            List<DurationType> list3 = new List<DurationType>();
            if (item.Shop_Cost.isTime1)
            {
                list.Add(item.Shop_Cost.Time1VCost.ToString());
                list2.Add(LanguageManager.GetText("Cost 1 day"));
                list3.Add(DurationType.DAY);
            }
            if (item.Shop_Cost.isTime7)
            {
                list.Add(item.Shop_Cost.Time7VCost.ToString());
                list2.Add(LanguageManager.GetText("Cost 7 day"));
                list3.Add(DurationType.WEEK);
            }
            if (item.Shop_Cost.isTime30)
            {
                list.Add(item.Shop_Cost.Time30VCost.ToString());
                list2.Add(LanguageManager.GetText("Cost 30 day"));
                list3.Add(DurationType.MONTH);
            }
            if (item.Shop_Cost.isTimeP)
            {
                list.Add(item.Shop_Cost.TimePVCost.ToString());
                list2.Add(LanguageManager.GetText("Cost permanent"));
                list3.Add(DurationType.PERMANENT);
            }
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            int index = GUILayout.SelectionGrid(list3.IndexOf(item.Shop_Cost.SelectedDuration), list2.ToArray(), 1, GUISkinManager.Button.GetStyle("selectGridRadio"), GUILayout.Width(140f));
            item.Shop_Cost.SelectedDuration = list3[index];
            GUILayout.FlexibleSpace();
            GUILayout.SelectionGrid(0, list.ToArray(), 1, GUISkinManager.Button.GetStyle("selectGridMoney"), GUILayout.Width(80f));
            GUILayout.EndHorizontal();
        }
        else
        {
            GUILayout.Label(item.Desc, GUISkinManager.Label.GetStyle("notificationDesc"));
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        return (int)item.Shop_Cost.SelectedVCost;
    }

    private int draw_Ability(Ability item)
    {
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(item.Ico, GUISkinManager.Backgound.GetStyle("itemRight"), GUILayout.Width(97f), GUILayout.Height(85f));
        GUILayout.Space(10f);
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.Label(item.Description, GUISkinManager.Label.GetStyle("notificationDesc"));
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        return (int)item.Cost.TimePVCost;
    }

    private void draw_DailyBonus()
    {
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("black"), GUILayout.Width(481f));
        int num = Convert.ToInt32((this.currentNotification.Item as GLEvent).eventItems[2]);
        Rect position = new Rect(0f, 0f, 0f, 0f);
        for (int i = 1; i <= 5; i++)
        {
            if (i < num)
            {
                GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("dailyBonusItem"), GUILayout.Width(95f));
                GUILayout.Label(LanguageManager.GetTextFormat("Day {0}", i), GUISkinManager.Text.GetStyle("normal16"), GUILayout.Width(95f), GUILayout.Height(30f));
                GUILayout.Label(GUIContent.none, GUISkinManager.Ico.GetStyle("dailyCoinPast"));
                position = GUILayoutUtility.GetLastRect();
                position.x += position.width - 40f;
                position.y += position.height - 33f;
                position.width = 38f;
                position.height = 31f;
                GUI.Label(position, GUIContent.none, GUISkinManager.Ico.GetStyle(string.Format("reward{0}done", i)));
                GUILayout.Label(GUIContent.none, GUISkinManager.Ico.GetStyle("dailyCheckerDone"));
                GUILayout.EndVertical();
            }
            else if (i == num)
            {
                GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("dailyBonusItemActive"), GUILayout.Width(95f));
                GUILayout.Label(LanguageManager.GetTextFormat("Day {0}", i), GUISkinManager.Text.GetStyle("normal16Active"), GUILayout.Width(95f), GUILayout.Height(30f));
                GUILayout.Label(GUIContent.none, GUISkinManager.Ico.GetStyle("dailyCoinToday"));
                position = GUILayoutUtility.GetLastRect();
                position.x += position.width - 40f;
                position.y += position.height - 33f;
                position.width = 38f;
                position.height = 31f;
                GUI.Label(position, GUIContent.none, GUISkinManager.Ico.GetStyle(string.Format("reward{0}done", i)));
                GUILayout.Label(GUIContent.none, GUISkinManager.Ico.GetStyle("dailyCheckerToday"));
                GUILayout.EndVertical();
            }
            else
            {
                GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("dailyBonusItemDisable"), GUILayout.Width(95f));
                GUILayout.Label(LanguageManager.GetTextFormat("Day {0}", i), GUISkinManager.Text.GetStyle("normal16Disable"), GUILayout.Width(95f), GUILayout.Height(30f));
                GUILayout.Label(GUIContent.none, GUISkinManager.Ico.GetStyle("dailyCoinFuture"));
                position = GUILayoutUtility.GetLastRect();
                position.x += position.width - 40f;
                position.y += position.height - 33f;
                position.width = 38f;
                position.height = 31f;
                GUI.Label(position, GUIContent.none, GUISkinManager.Ico.GetStyle(string.Format("reward{0}future", i)));
                GUILayout.Label(GUIContent.none, GUIStyle.none, GUILayout.Width(95f), GUILayout.Height(31f));
                GUILayout.EndVertical();
            }
            GUILayout.Space(1f);
        }
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(15f);
    }

    private void draw_SetNamePopup()
    {
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.Label(LanguageManager.GetText("You current name:"), GUISkinManager.Text.GetStyle("mapPropertie"), GUILayout.Height(29f));
        GUILayout.Label(LocalUser.Name, GUISkinManager.Text.GetStyle("mapPropertie"), GUILayout.Height(29f));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Label(LanguageManager.GetText("Please, enter the name which will be shown to other players."), GUISkinManager.Label.GetStyle("notificationDesc"));
        GUILayout.Space(15f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUI.SetNextControlName("userNameInput");
        GUILayout.Label(LanguageManager.GetText("Name:"), GUISkinManager.Text.GetStyle("mapPropertie"), GUILayout.Height(29f));
        GUI.SetNextControlName("userNameInput");
        ChangeNameManager.Instance.UserName = GUILayout.TextField(ChangeNameManager.Instance.UserName, GUISkinManager.Main.textField, GUILayout.Height(32f), GUILayout.MinWidth(226f));
        GUI.FocusControl("userNameInput");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        if (ChangeNameManager.Instance.CurrentLastError != 0)
        {
            GUILayout.Space(5f);
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(20f));
            GUILayout.FlexibleSpace();
            GUILayout.Label(LanguageManager.GetTextFormat("'{0}' - " + ChangeNameManager.Instance.CurrentLastError.GetDescription(), ChangeNameManager.Instance.CurrentCheckingName), GUISkinManager.Text.GetStyle("error01"));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            if (ChangeNameManager.Instance.CurrentLastError != ErrorInfo.CODE.USER_NAME_EXISTS && ChangeNameManager.Instance.CurrentLastError != ErrorInfo.CODE.MISSING_MONEY)
            {
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
                GUILayout.Label(LanguageManager.GetText("The name of the character should be created according to the rules:"), GUISkinManager.Text.GetStyle("normal07"));
                GUILayout.Label(LanguageManager.GetText("- character’s name can contain from 3 to 16 characters"), GUISkinManager.Text.GetStyle("normal07"));
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.FlexibleSpace();
    }

    private void draw_EnterPassword()
    {
        GUILayout.Label(LanguageManager.GetText("Type the password to enter the room"), GUISkinManager.Label.GetStyle("notificationDesc"));
        GUILayout.Space(15f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.Label(LanguageManager.GetText("Password:"), GUISkinManager.Text.GetStyle("mapPropertie"), GUILayout.Height(29f));
        GUI.SetNextControlName("passwordInput");
        (this.currentNotification.Item as ResponseRoomList).ConnectingPassword = GUILayout.TextField((this.currentNotification.Item as ResponseRoomList).ConnectingPassword, GUISkinManager.Main.textField, GUILayout.Height(32f), GUILayout.MinWidth(226f));
        GUI.FocusControl("passwordInput");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
    }

    private void draw_popup(int windowId)
    {
        bool flag = false;
        int num = 0;
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(this.currentNotification.Title, GUISkinManager.Text.GetStyle("popupTitle"), GUILayout.Height(34f));
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        if (this.currentNotification.Item != null && this.currentNotification.NType == Notification.Type.BUY_ITEM)
        {
            if (this.currentNotification.Item.GetType() == typeof(Wear) || this.currentNotification.Item.GetType() == typeof(Weapon) || this.currentNotification.Item.GetType() == typeof(Taunt) || this.currentNotification.Item.GetType() == typeof(Enhancer))
            {
                num = this.draw_CCItem(this.currentNotification.Item as CCItem);
            }
            else if (this.currentNotification.Item.GetType() == typeof(Ability))
            {
                num = this.draw_Ability(this.currentNotification.Item as Ability);
            }
        }
        else if (this.currentNotification.NType == Notification.Type.SET_NAME)
        {
            this.draw_SetNamePopup();
        }
        else if (this.currentNotification.NType == Notification.Type.ENTER_PASSWORD)
        {
            this.draw_EnterPassword();
        }
        else
        {
            GUILayout.Label(this.currentNotification.Message, GUISkinManager.Label.GetStyle("notificationDesc"));
            if (this.currentNotification.NType == Notification.Type.DAILY_BONUS)
            {
                this.draw_DailyBonus();
            }
        }
        GUILayout.EndVertical();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        if (this.currentNotification.NType == Notification.Type.DAILY_BONUS && ActionRotater.OfferEnabled)
        {
            if (GUILayout.Button(LanguageManager.GetText("Get more!"), GUISkinManager.Button.GetStyle("cancel"), GUILayout.MinWidth(106f), GUILayout.Height(37f)))
            {
                this.currentNotification.ClickButton();
                flag = true;
                WebCall.OpenOfferWindow(null);
            }
            GUILayout.Space(5f);
        }
        if (this.currentNotification.NType == Notification.Type.BUY_ITEM)
        {
            GUILayout.Label(LanguageManager.GetText("Price:"), GUISkinManager.Text.GetStyle("normal16"), GUILayout.Height(36f));
            GUILayout.Space(3f);
            GUILayout.Label(GUIContent.none, GUISkinManager.Ico.GetStyle("money"), GUILayout.Width(36f), GUILayout.Height(36f));
            GUILayout.Space(5f);
            GUILayout.Label(num.ToString(), GUISkinManager.Label.GetStyle("money"));
            GUILayout.Space(10f);
            if (GUILayout.Button(this.currentNotification.ButtonText, GUISkinManager.Button.GetStyle("green"), GUILayout.MinWidth(106f), GUILayout.Height(37f)))
            {
                this.currentNotification.ClickButton();
                flag = true;
            }
            GUILayout.Space(5f);
        }
        else if (this.currentNotification.ButtonText != null && this.currentNotification.ButtonText != string.Empty)
        {
            if (GUILayout.Button(this.currentNotification.ButtonText, GUISkinManager.Button.GetStyle("green"), GUILayout.MinWidth(106f), GUILayout.Height(37f)))
            {
                this.currentNotification.ClickButton();
                flag = true;
            }
            GUILayout.Space(5f);
        }
        if (this.currentNotification.NType == Notification.Type.SET_NAME)
        {
            GUILayout.Label(LanguageManager.GetText("Price:"), GUISkinManager.Text.GetStyle("normal16"), GUILayout.Height(36f));
            GUILayout.Space(3f);
            GUILayout.Label(GUIContent.none, GUISkinManager.Ico.GetStyle("money"), GUILayout.Width(36f), GUILayout.Height(36f));
            GUILayout.Space(3f);
            GUILayout.Label(ChangeNameManager.CHANGE_NAME_COST.ToString(), GUISkinManager.Label.GetStyle("money"));
            GUILayout.Space(10f);
            if (GUILayout.Button(LanguageManager.GetText("Change"), GUISkinManager.Button.GetStyle("green"), GUILayout.MinWidth(106f), GUILayout.Height(37f)))
            {
                ChangeNameManager.ChangeName(ChangeNameManager.Instance.UserName);
                ChangeNameManager.OnConfirmCompleteCallback = delegate
                {
                    NotificationWindow.Complete(this.currentNotification);
                };
            }
            GUILayout.Space(5f);
        }
        if (this.currentNotification.NType != Notification.Type.BUY_ITEM && this.currentNotification.NType != Notification.Type.CONNECTION_LOST && this.currentNotification.NType != Notification.Type.DAILY_BONUS && !this.currentNotification.CanClose && GUILayout.Button(LanguageManager.GetText("Close"), GUISkinManager.Button.GetStyle("cancel"), GUILayout.Width(106f), GUILayout.Height(37f)))
        {
            flag = true;
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(5f);
        if (!this.currentNotification.CanClose)
        {
            Vector2 windowSize = this.currentNotification.WindowSize;
            if (GUI.Button(new Rect(windowSize.x - 32f, 2f, 30f, 30f), GUIContent.none, GUISkinManager.Button.GetStyle("popupClose")))
            {
                flag = true;
            }
        }
        if (flag)
        {
            NotificationWindow.Complete(this.currentNotification);
        }
    }
}


