// ILSpyBased#2
using UnityEngine;

public class Notification
{
    public enum Type
    {
        ERROR = 1,
        WARNING,
        NOTIFICATION,
        CONGRATULATION,
        CONNECTION_LOST,
        SET_NAME,
        BUY_ITEM,
        DAILY_BONUS,
        ENTER_PASSWORD,
        GAMELOGIC_ERROR
    }

    public delegate void ButtonClick(object param);

    private Vector2 windowSize = new Vector2(360f, 230f);

    public bool CanClose;

    private Texture2D ico;

    private Type nType;

    private string title;

    private string message;

    private string buttonText;

    private object item;

    private ButtonClick callbackClick;

    private object clickParam;

    public Vector2 WindowSize
    {
        get
        {
            return this.windowSize;
        }
        set
        {
            this.windowSize = value;
        }
    }

    public Texture2D Ico
    {
        get
        {
            return this.ico;
        }
    }

    public Type NType
    {
        get
        {
            return this.nType;
        }
    }

    public string Title
    {
        get
        {
            return this.title;
        }
    }

    public string Message
    {
        get
        {
            return this.message;
        }
    }

    public string ButtonText
    {
        get
        {
            return this.buttonText;
        }
    }

    public object Item
    {
        get
        {
            return this.item;
        }
        set
        {
            this.item = value;
        }
    }

    public ButtonClick CallbackClick
    {
        get
        {
            return this.callbackClick;
        }
        set
        {
            this.callbackClick = value;
        }
    }

    public Notification(Type type, string title, string message)
    {
        this.nType = type;
        this.title = title;
        this.message = message;
        switch (type)
        {
            case Type.CONGRATULATION:
                this.ico = (Texture2D)Resources.Load("Notification/icon_popupnotification_congrat");
                break;
            case Type.ERROR:
                this.ico = (Texture2D)Resources.Load("Notification/icon_popupnotification_error");
                break;
            case Type.NOTIFICATION:
                this.ico = (Texture2D)Resources.Load("Notification/icon_popupnotification_notification");
                break;
            case Type.WARNING:
                this.ico = (Texture2D)Resources.Load("Notification/icon_popupnotification_warning");
                break;
            case Type.CONNECTION_LOST:
                this.ico = (Texture2D)Resources.Load("Notification/icon_popupnotification_error");
                break;
        }
    }

    public Notification(Type type, string title, string message, string buttonText, ButtonClick clickCallback, object clickParam)
    {
        this.nType = type;
        this.title = title;
        this.message = message;
        this.buttonText = buttonText;
        this.callbackClick = clickCallback;
        this.clickParam = clickParam;
        switch (type)
        {
            case Type.CONGRATULATION:
                this.ico = (Texture2D)Resources.Load("Notification/icon_popupnotification_congrat");
                break;
            case Type.ERROR:
                this.ico = (Texture2D)Resources.Load("Notification/icon_popupnotification_error");
                break;
            case Type.NOTIFICATION:
                this.ico = (Texture2D)Resources.Load("Notification/icon_popupnotification_notification");
                break;
            case Type.WARNING:
                this.ico = (Texture2D)Resources.Load("Notification/icon_popupnotification_warning");
                break;
            case Type.CONNECTION_LOST:
                this.ico = (Texture2D)Resources.Load("Notification/icon_popupnotification_error");
                break;
        }
    }

    public Notification(Type type, object item, ButtonClick clickCallback, object clickParam)
    {
        this.nType = type;
        this.callbackClick = clickCallback;
        this.clickParam = clickParam;
        switch (type)
        {
            case Type.CONGRATULATION:
                this.ico = (Texture2D)Resources.Load("Notification/icon_popupnotification_congrat");
                break;
            case Type.ERROR:
                this.ico = (Texture2D)Resources.Load("Notification/icon_popupnotification_error");
                break;
            case Type.NOTIFICATION:
                this.ico = (Texture2D)Resources.Load("Notification/icon_popupnotification_notification");
                break;
            case Type.WARNING:
                this.ico = (Texture2D)Resources.Load("Notification/icon_popupnotification_warning");
                break;
            case Type.CONNECTION_LOST:
                this.ico = (Texture2D)Resources.Load("Notification/icon_popupnotification_error");
                break;
            case Type.BUY_ITEM:
                this.ico = (Texture2D)Resources.Load("Notification/icon_popupnotification_notification");
                this.title = (item as CCItem).Name;
                this.message = "Message";
                this.item = item;
                this.buttonText = LanguageManager.GetText("Buy");
                break;
        }
    }

    public void ClickButton()
    {
        UnityEngine.Debug.Log("[Notification] ClickButton");
        if (this.callbackClick != null)
        {
            this.callbackClick(this.clickParam);
        }
    }
}


