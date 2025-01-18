// ILSpyBased#2
using System;
using System.Collections;
using UnityEngine;

public class PlayerLocal : MonoBehaviour
{
    public PlayerRemote playerRemote;

    private Vector3 Point;

    private float lastKeyPress;

    private Transform SoilderDamage;

    private float nextMoveTime;

    private GUIText nameText;

    private KeyState lastKeyState;

    private KeyState keyState;

    public byte currentWeapon;

    public static bool isSendReliable;

    public bool Walk;

    public bool InAir;

    public bool Crouch;

    public string Name = string.Empty;

    public Transform SoldierTarget;

    private bool oldFire;

    private bool oldReload;

    private FPSCamera _fpsCamera;

    private int num;

    private int numr;

    private FPSCamera fpsCamera
    {
        get
        {
            if ((UnityEngine.Object)this._fpsCamera == (UnityEngine.Object)null)
            {
                this._fpsCamera = base.GetComponentInChildren<FPSCamera>();
            }
            return this._fpsCamera;
        }
    }

    internal Hashtable GetProperties()
    {
        Hashtable hashtable = new Hashtable();
        hashtable.Add("N", this.Name);
        hashtable.Add("W", this.currentWeapon);
        hashtable.Add("C", false);
        hashtable.Add("A", false);
        hashtable.Add("R", false);
        return hashtable;
    }

    public static float[] GetPosition(Vector3 position)
    {
        return new float[3] {
            position.x,
            position.y,
            position.z
        };
    }

    public static float[] GetRotation(Quaternion rotation)
    {
        return new float[4] {
            rotation.x,
            rotation.y,
            rotation.z,
            rotation.w
        };
    }

    public void DownLifeHitSoldier(string Hit)
    {
        this.SoilderDamage.SendMessage("HitSoldier", Hit);
    }

    public void DownLifeHitSoldierGranade(string Hit)
    {
        this.SoilderDamage.SendMessage("HitSoldierGranade", Hit);
    }

    public void Update()
    {
        try
        {
            this.Move();
            this.ReadKeyboardInput();
            this.Anim();
        }
        catch (Exception message)
        {
            UnityEngine.Debug.Log(message);
        }
    }

    public void FixedUpdate()
    {
        CombatPlayer component = base.GetComponent<CombatPlayer>();
        if ((UnityEngine.Object)component != (UnityEngine.Object)null)
        {
            int num = component.Check();
            if (num > 0)
            {
                PlayerManager.Instance.SendEnterBaseRequest(num);
            }
        }
        else
        {
            UnityEngine.Debug.LogError("No Player To Check!");
        }
    }

    private void ReadKeyboardInput()
    {
        if (!((UnityEngine.Object)this.fpsCamera == (UnityEngine.Object)null))
        {
            if (UnityEngine.Input.GetKey(TRInput.RightStrafe))
            {
                if (this.Walk)
                {
                    this.keyState = KeyState.WalkStrafeRight;
                }
                else
                {
                    this.keyState = KeyState.RunStrafeRight;
                }
                this.fpsCamera.Walk = true;
            }
            else if (UnityEngine.Input.GetKey(TRInput.LeftStrafe))
            {
                if (this.Walk)
                {
                    this.keyState = KeyState.WalkStrafeLeft;
                }
                else
                {
                    this.keyState = KeyState.RunStrafeLeft;
                }
                this.fpsCamera.Walk = true;
            }
            else if (UnityEngine.Input.GetKey(TRInput.Forward))
            {
                if (this.Walk)
                {
                    this.keyState = KeyState.Walking;
                }
                else
                {
                    this.keyState = KeyState.Runing;
                }
                this.fpsCamera.Walk = true;
            }
            else if (UnityEngine.Input.GetKey(TRInput.Backward))
            {
                if (this.Walk)
                {
                    this.keyState = KeyState.WalkBack;
                }
                else
                {
                    this.keyState = KeyState.RuningBack;
                }
                this.fpsCamera.Walk = true;
            }
            else
            {
                this.keyState = KeyState.Still;
                this.fpsCamera.Walk = false;
            }
        }
    }

    private void Anim()
    {
        if (this.keyState != this.lastKeyState)
        {
            this.playerRemote.keyState = this.keyState;
            this.lastKeyState = this.keyState;
            if ((UnityEngine.Object)NetworkManager.Instance != (UnityEngine.Object)null && NetworkDev.Remote_Animation_Send)
            {
                NetworkManager.Instance.SendAnimationKey((byte)this.keyState);
            }
        }
    }

    public void Fire(Hashtable _hastable)
    {
        bool flag = false;
        bool flag2 = true;
        Hashtable hashtable = new Hashtable();
        if (_hastable.Contains("reload"))
        {
            flag2 = (bool)_hastable["reload"];
            hashtable.Add("reload", flag2);
        }
        if (_hastable.Contains(2))
        {
            flag = (bool)_hastable[2];
        }
        if (_hastable.Contains(1))
        {
            Vector3 vector = (Vector3)_hastable[1];
        }
        if (_hastable.Contains(3))
        {
            Vector3 vector2 = (Vector3)_hastable[3];
        }
        if (flag2 != this.oldReload && _hastable.Contains("reload"))
        {
            this.num++;
            UnityEngine.Debug.Log("reload" + this.num);
            this.oldReload = flag2;
        }
        if (flag != this.oldFire && _hastable.Contains(2))
        {
            this.numr++;
            UnityEngine.Debug.Log("fire" + this.numr);
            this.oldFire = flag;
        }
    }

    private void SetGunInfoRPC(byte Gun)
    {
        this.currentWeapon = Gun;
        Hashtable hashtable = new Hashtable();
        hashtable.Add("W", Gun);
    }

    private void Move()
    {
        if (!(Time.time > this.nextMoveTime))
        {
            return;
        }
    }

    public void MoveOp(byte operationCode, bool reliable)
    {
        float[] position = PlayerLocal.GetPosition(base.transform.position);
        float[] rotation = PlayerLocal.GetRotation(base.transform.rotation);
        float[] position2 = PlayerLocal.GetPosition(this.SoldierTarget.position);
        this.nextMoveTime = Time.time + 0.1f;
    }

    public void SetAim(bool _Flag)
    {
        this.playerRemote.Aim = _Flag;
    }

    public void SetCrouch(bool _Flag)
    {
        this.Crouch = _Flag;
        this.playerRemote.CrouchStatus = _Flag;
        if ((UnityEngine.Object)this.fpsCamera != (UnityEngine.Object)null)
        {
            this.fpsCamera.Croach = _Flag;
        }
        this.SendAnimationState();
    }

    public void SetInAir(bool _Flag)
    {
        this.InAir = _Flag;
        this.playerRemote.InAir = _Flag;
        this.SendAnimationState();
    }

    public void SetWalk(bool _Flag)
    {
        this.Walk = _Flag;
        this.playerRemote.Walk = _Flag;
        if (NetworkDev.Remote_Animation_Send)
        {
            this.SendAnimationState();
        }
    }

    public void SendAnimationState()
    {
        byte b = 0;
        if (this.Walk)
        {
            b = (byte)(b | 2);
        }
        if (this.InAir)
        {
            b = (byte)(b | 4);
        }
        if (this.Crouch)
        {
            b = (byte)(b | 1);
        }
        if ((UnityEngine.Object)NetworkManager.Instance != (UnityEngine.Object)null)
        {
            NetworkManager.Instance.SendAnimationState(b);
        }
    }
}


