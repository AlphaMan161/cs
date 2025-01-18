// ILSpyBased#2
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GUIFriends : MonoBehaviour
{
    private static Vector2 friendListScroll = Vector2.zero;

    private static Vector2 requestListScroll = Vector2.zero;

    private static string userMessage = string.Empty;

    private static int SELECTED_MENU = 1;

    private static Friend SELECTED_FRIEND = null;

    private static bool enableTimer = false;

    private static int timerCount = 20;

    private static float timer_nextActionTime = 0f;

    public static float timer_period = 1f;

    public static void UpdateTimer()
    {
        if (GUIFriends.enableTimer && Time.time > GUIFriends.timer_nextActionTime)
        {
            GUIFriends.timer_nextActionTime += GUIFriends.timer_period;
            GUIFriends.timerCount -= (int)GUIFriends.timer_period;
            if (GUIFriends.timerCount < 0)
            {
                GUIFriends.timerCount = 10;
            }
        }
    }

    public static void OnGUI()
    {
        if (!MasterServerNetworkController.Connected)
        {
            GUIFriends.enableTimer = true;
            GUIFriends.UpdateTimer();
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("winMain"), GUILayout.Width(755f), GUILayout.Height(454f));
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.FlexibleSpace();
            GUILayout.Label(LanguageManager.GetTextFormat("Please wait. We're accessing the battle field! {0} sec", GUIFriends.timerCount), GUISkinManager.Text.GetStyle("friendConnecting"));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        else
        {
            GUIFriends.enableTimer = false;
            GUIFriends.timerCount = 20;
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("winMain"), GUILayout.Width(755f), GUILayout.Height(454f));
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            if (GUIFriends.SELECTED_MENU == 1)
            {
                GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
                GUILayout.Label(LanguageManager.GetText("Comrades"), GUISkinManager.Text.GetStyle("partActive"));
                GUILayout.EndHorizontal();
            }
            else
            {
                GUIContent gUIContent = new GUIContent(LanguageManager.GetText("Comrades"));
                if (MasterServerNetworkController.Instance.FriendList != null && MasterServerNetworkController.NewPrivateMessages && GUIFriends.SELECTED_MENU != 1)
                {
                    gUIContent.image = (Texture)Resources.Load("GUI/Icons/Alert/message_alert01");
                }
                if (GUILayout.Button(gUIContent, GUISkinManager.Text.GetStyle("part")))
                {
                    GUIFriends.SELECTED_MENU = 1;
                }
            }
            if (GUIFriends.SELECTED_MENU == 2)
            {
                GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
                GUILayout.Label(LanguageManager.GetText("Requests"), GUISkinManager.Text.GetStyle("partActive"));
                GUILayout.EndHorizontal();
            }
            else
            {
                GUIContent gUIContent2 = new GUIContent(LanguageManager.GetText("Requests"));
                if (MasterServerNetworkController.Instance.FriendList != null && MasterServerNetworkController.Instance.FriendList.Request.Count > 0 && GUIFriends.SELECTED_MENU != 2)
                {
                    gUIContent2.image = (Texture)Resources.Load("GUI/Icons/Alert/message_alert01");
                }
                if (GUILayout.Button(gUIContent2, GUISkinManager.Text.GetStyle("part")))
                {
                    GUIFriends.SELECTED_MENU = 2;
                }
            }
            if (GUIFriends.SELECTED_MENU == 3)
            {
                GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
                GUILayout.Label(LanguageManager.GetText("Add comrade"), GUISkinManager.Text.GetStyle("partActive"));
                GUILayout.EndHorizontal();
            }
            else if (GUILayout.Button(LanguageManager.GetText("Add comrade"), GUISkinManager.Text.GetStyle("part")))
            {
                GUIFriends.SELECTED_MENU = 3;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            if (GUIFriends.SELECTED_MENU == 1)
            {
                GUIFriends.OnGUIFriends();
            }
            else if (GUIFriends.SELECTED_MENU == 2)
            {
                GUIFriends.OnGUIRequests();
            }
            else if (GUIFriends.SELECTED_MENU == 3)
            {
                GUIAddFriend.OnGUI();
            }
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            if (Event.current.type == EventType.Repaint && MasterServerNetworkController.Instance.ChatList != null && GUIFriends.SELECTED_FRIEND != null && MasterServerNetworkController.Instance.ChatList.PrivateConversations != null && MasterServerNetworkController.Instance.ChatList.PrivateConversations.ContainsKey(GUIFriends.SELECTED_FRIEND.UserID))
            {
                object lockDisplayMessages = MasterServerNetworkController.Instance.ChatList.PrivateConversations[GUIFriends.SELECTED_FRIEND.UserID].LockDisplayMessages;
                Monitor.Enter(lockDisplayMessages);
                try
                {
                    MasterServerNetworkController.Instance.ChatList.PrivateConversations[GUIFriends.SELECTED_FRIEND.UserID].TryRefreshDisplayMessages();
                }
                finally
                {
                    Monitor.Exit(lockDisplayMessages);
                }
            }
        }
    }

    private static void OnGUIRequests()
    {
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuTitle"), GUILayout.Height(36f));
        GUILayout.Label(LanguageManager.GetTextFormat("Comrade requests: ({0})", (MasterServerNetworkController.Instance.FriendList != null) ? MasterServerNetworkController.Instance.FriendList.Request.Count : 0), GUISkinManager.Text.GetStyle("menuTitle"));
        GUILayout.EndHorizontal();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.Space(2f);
        GUIFriends.requestListScroll = GUILayout.BeginScrollView(GUIFriends.requestListScroll, false, true, GUILayout.Height(344f));
        if (MasterServerNetworkController.Instance.FriendList != null)
        {
            Dictionary<int, Friend>.ValueCollection.Enumerator enumerator = MasterServerNetworkController.Instance.FriendList.Request.Values.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    SocialPlayer current = enumerator.Current;
                    if (current != null)
                    {
                        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
                        GUILayout.Space(4f);
                        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(36f));
                        GUILayout.Space(11f);
                        GUILayout.Label(LanguageManager.GetTextFormat("{0} (level {1})", current.Name, current.Level), GUISkinManager.Text.GetStyle("room"));
                        GUILayout.Space(5f);
                        if (GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle("btnInfo"), GUILayout.Width(20f), GUILayout.Height(20f)))
                        {
                            StatisticManager.View(current.UserID);
                        }
                        GUILayout.FlexibleSpace();
                        if (current.Status == UserStatus.Offline)
                        {
                            GUILayout.Label(GUIContent.none, GUISkinManager.Label.GetStyle("userOffline"), GUILayout.Width(15f), GUILayout.Height(15f));
                        }
                        else
                        {
                            GUILayout.Label(GUIContent.none, GUISkinManager.Label.GetStyle("userOnline"), GUILayout.Width(15f), GUILayout.Height(15f));
                        }
                        GUILayout.Space(18f);
                        if (GUILayout.Button(LanguageManager.GetText("Decline"), GUISkinManager.Button.GetStyle("btn01"), GUILayout.Width(111f), GUILayout.Height(32f)))
                        {
                            MasterServerNetworkController.FriendDecline(current.UserID);
                        }
                        GUILayout.Space(4f);
                        if (GUILayout.Button(LanguageManager.GetText("Accept"), GUISkinManager.Button.GetStyle("btn01"), GUILayout.Width(111f), GUILayout.Height(32f)))
                        {
                            MasterServerNetworkController.FriendConfirm(current.UserID);
                        }
                        GUILayout.Space(4f);
                        GUILayout.EndHorizontal();
                        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
                        GUILayout.EndVertical();
                    }
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
        }
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUILayout.Space(4f);
        GUILayout.EndHorizontal();
    }

    private static void OnGUIFriends()
    {
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuTitle"), GUILayout.Height(36f));
        if (GUIFriends.SELECTED_FRIEND != null)
        {
            if (GUIFriends.SELECTED_FRIEND.Status == UserStatus.Online)
            {
                GUILayout.Label(LanguageManager.GetTextFormat("Chat with: ({0})", GUIFriends.SELECTED_FRIEND.Name), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(323f));
            }
            else if (GUIFriends.SELECTED_FRIEND.Status == UserStatus.Offline)
            {
                GUILayout.Label(LanguageManager.GetTextFormat("Impossible to chat with {0}, comrade is offline", GUIFriends.SELECTED_FRIEND.Name), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(323f), GUILayout.MaxWidth(323f));
            }
            else
            {
                GUILayout.Label(LanguageManager.GetTextFormat("Impossible to chat with {0}, comrade is in the battle", GUIFriends.SELECTED_FRIEND.Name), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(323f));
            }
        }
        else
        {
            GUILayout.Label(GUIContent.none, GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(323f));
        }
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Hor"), GUILayout.Width(1f));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetTextFormat("Comrades ({0})", (MasterServerNetworkController.Instance.FriendList != null) ? MasterServerNetworkController.Instance.FriendList.List.Count : 0), GUISkinManager.Text.GetStyle("menuTitle"));
        GUILayout.Label(LanguageManager.GetText("Search the list for comrade"), GUISkinManager.Text.GetStyle("menuTitle02"));
        GUILayout.Space(2f);
        GUILayout.TextField(string.Empty, GUISkinManager.Main.GetStyle("textfield02"), GUILayout.Width(125f), GUILayout.Height(29f));
        GUILayout.Space(2f);
        GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle("btnSearch"), GUILayout.Width(32f), GUILayout.Height(32f));
        GUILayout.Space(2f);
        GUILayout.EndHorizontal();
        GUILayout.EndHorizontal();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("chat"), GUILayout.Width(324f));
        GUILayout.Space(2f);
        if (MasterServerNetworkController.Instance.ChatList != null && GUIFriends.SELECTED_FRIEND != null && MasterServerNetworkController.Instance.ChatList.PrivateConversations.ContainsKey(GUIFriends.SELECTED_FRIEND.UserID))
        {
            MasterServerNetworkController.Instance.ChatList.PrivateConversations[GUIFriends.SELECTED_FRIEND.UserID].Scroll = GUILayout.BeginScrollView(MasterServerNetworkController.Instance.ChatList.PrivateConversations[GUIFriends.SELECTED_FRIEND.UserID].Scroll, false, true, GUILayout.MinHeight(308f));
            float num = 0f;
            List<ChatMessage>.Enumerator enumerator = MasterServerNetworkController.Instance.ChatList.PrivateConversations[GUIFriends.SELECTED_FRIEND.UserID].DisplayMessages.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    ChatMessage current = enumerator.Current;
                    GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
                    GUILayout.Label(LanguageManager.GetTextFormat("{0} [{1}]", current.UserName, string.Format("{0:00}:{1:00}", current.Time.Hours, current.Time.Minutes)), GUISkinManager.Text.GetStyle((!current.UserName.Equals(LocalUser.Name)) ? "chatAuthor" : "chatAuthorLocal"));
                    GUILayout.Label(current.Message, GUISkinManager.Text.GetStyle("chatMsg"));
                    GUILayout.EndVertical();
                    num += GUILayoutUtility.GetLastRect().height + 8f;
                    GUILayout.Space(8f);
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
            GUILayout.EndScrollView();
            if (Event.current.type == EventType.Repaint)
            {
                float num2 = num - GUILayoutUtility.GetLastRect().height;
                Vector2 scroll = MasterServerNetworkController.Instance.ChatList.PrivateConversations[GUIFriends.SELECTED_FRIEND.UserID].Scroll;
                if (num2 == scroll.y)
                {
                    MasterServerNetworkController.Instance.ChatList.PrivateConversations[GUIFriends.SELECTED_FRIEND.UserID].Tail = true;
                }
            }
        }
        else
        {
            GUILayout.BeginScrollView(Vector2.zero, false, true, GUILayout.MinHeight(308f));
            GUILayout.EndScrollView();
        }
        GUILayout.EndVertical();
        GUILayout.Space(2f);
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Hor"), GUILayout.Width(1f));
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("chat02"));
        GUILayout.Space(2f);
        GUIFriends.friendListScroll = GUILayout.BeginScrollView(GUIFriends.friendListScroll, false, true, GUILayout.MinHeight(308f));
        if (MasterServerNetworkController.Instance.FriendList != null)
        {
            Dictionary<int, Friend>.ValueCollection.Enumerator enumerator2 = MasterServerNetworkController.Instance.FriendList.List.Values.GetEnumerator();
            try
            {
                while (enumerator2.MoveNext())
                {
                    Friend current2 = enumerator2.Current;
                    if (current2 != null)
                    {
                        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
                        GUILayout.Space(4f);
                        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(36f));
                        GUIContent gUIContent = new GUIContent(current2.Name);
                        if (MasterServerNetworkController.Instance.FriendList != null && GUIFriends.SELECTED_FRIEND != current2 && MasterServerNetworkController.Instance.ChatList.PrivateConversations.ContainsKey(current2.UserID) && MasterServerNetworkController.Instance.ChatList.PrivateConversations[current2.UserID].NewMessages)
                        {
                            gUIContent.image = (Texture)Resources.Load("GUI/Icons/Alert/message_alert01");
                        }
                        if (current2.Status == UserStatus.Offline)
                        {
                            GUILayout.Space(2f);
                            if (GUILayout.Button(gUIContent, GUISkinManager.Text.GetStyle((GUIFriends.SELECTED_FRIEND != current2) ? "friendNameDisable" : "friendNameActive"), GUILayout.Height(32f)))
                            {
                                GUIFriends.SELECTED_FRIEND = current2;
                            }
                            if (GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle("btnInfo"), GUILayout.Width(20f), GUILayout.Height(20f)) && current2.UserID != 0)
                            {
                                StatisticManager.View(current2.UserID);
                            }
                            GUILayout.FlexibleSpace();
                            GUILayout.Label(GUIContent.none, GUISkinManager.Label.GetStyle("userOffline"), GUILayout.Width(15f), GUILayout.Height(15f));
                            GUILayout.Space(6f);
                            GUILayout.Label(LanguageManager.GetText("JOIN"), GUISkinManager.Button.GetStyle("btn01Disable"), GUILayout.Width(85f), GUILayout.Height(32f));
                        }
                        else
                        {
                            GUILayout.Space(2f);
                            if (GUILayout.Button(gUIContent, GUISkinManager.Text.GetStyle((GUIFriends.SELECTED_FRIEND != current2) ? "friendName" : "friendNameActive"), GUILayout.Height(32f)))
                            {
                                GUIFriends.SELECTED_FRIEND = current2;
                            }
                            if (GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle("btnInfo"), GUILayout.Width(20f), GUILayout.Height(20f)) && current2.UserID != 0)
                            {
                                StatisticManager.View(current2.UserID);
                            }
                            GUILayout.FlexibleSpace();
                            if (current2.Status == UserStatus.Fighting)
                            {
                                GUILayout.Label(current2.RoomInfo.FilteredName, GUISkinManager.Text.GetStyle("friendRoom"), GUILayout.Height(32f));
                            }
                            GUILayout.Label(GUIContent.none, GUISkinManager.Label.GetStyle("userOnline"), GUILayout.Width(15f), GUILayout.Height(15f));
                            GUILayout.Space(6f);
                            if (current2.Status == UserStatus.Fighting)
                            {
                                if (GUILayout.Button(LanguageManager.GetText("JOIN"), GUISkinManager.Button.GetStyle("btn01"), GUILayout.Width(85f), GUILayout.Height(32f)))
                                {
                                    ServersList.ConnectFriend(current2.RoomInfo.ConnectionString, current2.RoomInfo.Name);
                                }
                            }
                            else
                            {
                                GUILayout.Label(LanguageManager.GetText("JOIN"), GUISkinManager.Button.GetStyle("btn01Disable"), GUILayout.Width(85f), GUILayout.Height(32f));
                            }
                        }
                        GUILayout.Space(4f);
                        GUILayout.EndHorizontal();
                        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
                        GUILayout.EndVertical();
                    }
                }
            }
            finally
            {
                ((IDisposable)enumerator2).Dispose();
            }
        }
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUILayout.Space(4f);
        GUILayout.EndHorizontal();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuBottom"), GUILayout.Height(36f));
        if (GUIFriends.SELECTED_FRIEND != null && GUIFriends.SELECTED_FRIEND.Status == UserStatus.Online)
        {
            GUI.SetNextControlName("chatInputText");
            GUIFriends.userMessage = GUILayout.TextField(GUIFriends.userMessage, 512, GUISkinManager.Main.GetStyle("textfield02"), GUILayout.Width(285f), GUILayout.Height(29f)).Replace('\n', '\0');
            GUILayout.Space(2f);
            if (GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle("btnEnterSmall"), GUILayout.Width(33f), GUILayout.Height(32f)))
            {
                GUIFriends.OnSendMessage(GUIFriends.userMessage, BattleChat.MessageType.PRIVATE, GUIFriends.SELECTED_FRIEND);
                GUIFriends.userMessage = string.Empty;
            }
            else
            {
                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
                {
                    goto IL_0c94;
                }
                if (Event.current.type == EventType.Used && Event.current.keyCode == KeyCode.Return)
                {
                    goto IL_0c94;
                }
                if (Event.current.type == EventType.KeyDown && Event.current.character == '\n')
                {
                    goto IL_0c94;
                }
            }
        }
        else
        {
            GUILayout.Label(GUIContent.none, GUISkinManager.Main.GetStyle("textfield02"), GUILayout.Width(285f), GUILayout.Height(29f));
            GUILayout.Space(2f);
            GUILayout.Label(GUIContent.none, GUISkinManager.Button.GetStyle("btnEnterSmall"), GUILayout.Width(33f), GUILayout.Height(32f));
        }
        goto IL_0d33;
        IL_0c94:
        GUIFriends.OnSendMessage(GUIFriends.userMessage, BattleChat.MessageType.PRIVATE, GUIFriends.SELECTED_FRIEND);
        GUIFriends.userMessage = string.Empty;
        goto IL_0d33;
        IL_0d33:
        GUILayout.Space(2f);
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Hor"), GUILayout.Width(1f));
        if (GUIFriends.SELECTED_FRIEND != null)
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle("btnDelete"), GUILayout.Width(32f), GUILayout.Height(32f)))
            {
                NotificationWindow.Add(new Notification(Notification.Type.NOTIFICATION, LanguageManager.GetText("Remove comrade"), LanguageManager.GetTextFormat("Do you really want to remove comrade {0} from the comrades list?", GUIFriends.SELECTED_FRIEND.Name), LanguageManager.GetText("Remove"), delegate
                {
                    MasterServerNetworkController.FriendRemove(GUIFriends.SELECTED_FRIEND.UserID);
                    GUIFriends.SELECTED_FRIEND = null;
                }, null));
            }
            GUILayout.Space(10f);
            GUILayout.EndHorizontal();
        }
        else
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(16f);
        if (!NotificationWindow.IsShow && !OptionPopup.Show)
        {
            GUI.FocusControl("chatInputText");
        }
    }

    public static void OnSendMessage(string msg, BattleChat.MessageType messageType, Friend targetUser)
    {
        if (targetUser != null)
        {
            msg = msg.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine).Trim();
            msg = msg.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
            MasterServerNetworkController.SendChatMessage(msg, messageType, targetUser);
        }
    }
}


