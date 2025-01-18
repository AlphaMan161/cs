// ILSpyBased#2
using System;
using UnityEngine;

public class NetworkTransformInterpolation : MonoBehaviour
{
    public InterpolationMode mode = InterpolationMode.BEZIER_LINEAR_IN;

    public bool IgnoreAngles;

    private long interpolationBackTime = 400L;

    private long extrapolationForwardTime = 400L;

    private bool running;

    private NetworkTransform[] bufferedStates = new NetworkTransform[20];

    private NetworkAnimationState[] bufferedAnimationStates = new NetworkAnimationState[20];

    private NetworkAnimationKey[] bufferedAnimationKeys = new NetworkAnimationKey[20];

    private int statesCount;

    private int animationStatesCount;

    private int animationKeysCount;

    private Vector3 interpolatedSpeed = new Vector3(0f, 0f, 0f);

    private float interpolatedRotationSpeed;

    private CombatPlayer player;

    private ActorAnimator animator;

    private float speedRX;

    private float speedRZ;

    private long lastReceiveTransformTime = DateTime.Now.Ticks;

    public Vector3 lastPosition;

    public Vector3 lastRotation;

    public long lastPositionTimeStamp;

    public GameObject RealPositionBox;

    public long InterpolationBackTime
    {
        set
        {
            this.interpolationBackTime = value;
        }
    }

    public Vector3 InterpolatedSpeed
    {
        get
        {
            return this.interpolatedSpeed;
        }
    }

    public float InterpolatedRotationSpeed
    {
        get
        {
            return this.interpolatedRotationSpeed;
        }
    }

    private CombatPlayer Player
    {
        get
        {
            if ((UnityEngine.Object)this.player == (UnityEngine.Object)null)
            {
                this.player = base.GetComponent<CombatPlayer>();
            }
            return this.player;
        }
    }

    private ActorAnimator Animator
    {
        get
        {
            if ((UnityEngine.Object)this.animator == (UnityEngine.Object)null)
            {
                this.animator = base.GetComponent<ActorAnimator>();
            }
            return this.animator;
        }
    }

    public void StartReceiving()
    {
        this.lastPosition = base.transform.position;
        Vector3 localEulerAngles = base.transform.localEulerAngles;
        this.lastRotation = new Vector3(0f, localEulerAngles.y, 0f);
        this.running = true;
    }

    public void StopReceiving()
    {
        this.running = false;
    }

    public long ReceiveTransform(NetworkTransform ntransform)
    {
        if (!this.running)
        {
            return 0L;
        }
        for (int num = this.bufferedStates.Length - 1; num >= 1; num--)
        {
            this.bufferedStates[num] = this.bufferedStates[num - 1];
        }
        this.bufferedStates[0] = ntransform;
        this.statesCount = Mathf.Min(this.statesCount + 1, this.bufferedStates.Length);
        for (int i = 0; i < this.statesCount - 1; i++)
        {
            if (this.bufferedStates[i].TimeStamp >= this.bufferedStates[i + 1].TimeStamp)
            {
                continue;
            }
        }
        long num2 = DateTime.Now.Ticks - this.lastReceiveTransformTime;
        this.lastReceiveTransformTime = DateTime.Now.Ticks;
        return num2 / 10000;
    }

    public void ReceiveAnimationKey(byte key, long timeStamp)
    {
        if (this.running)
        {
            for (int num = this.bufferedAnimationKeys.Length - 1; num >= 1; num--)
            {
                this.bufferedAnimationKeys[num] = this.bufferedAnimationKeys[num - 1];
            }
            this.bufferedAnimationKeys[0] = new NetworkAnimationKey(key, timeStamp);
            this.animationKeysCount = Mathf.Min(this.animationKeysCount + 1, this.bufferedAnimationKeys.Length);
        }
    }

    public void ReceiveAnimationState(byte state, long timeStamp)
    {
        if (this.running)
        {
            for (int num = this.bufferedAnimationStates.Length - 1; num >= 1; num--)
            {
                this.bufferedAnimationStates[num] = this.bufferedAnimationStates[num - 1];
            }
            this.bufferedAnimationStates[0] = new NetworkAnimationState(state, timeStamp);
            this.animationStatesCount = Mathf.Min(this.animationStatesCount + 1, this.bufferedAnimationStates.Length);
        }
    }

    private void NewInterpolation(long interpolationTime)
    {
        if (this.bufferedStates[0].TimeStamp > interpolationTime)
        {
            this.Interpolate(interpolationTime);
        }
        else if (this.mode == InterpolationMode.SMOOTH_LINEAR_IN_EX)
        {
            this.Extrapolate(interpolationTime);
        }
    }

    private void TestBox(Vector3 pos, Color color)
    {
    }

    private void SetRealPlayerPosition(Vector3 pos, Color color)
    {
    }

    private void Interpolate(long interpolationTime)
    {
        int num = 0;
        while (true)
        {
            if (num < this.statesCount)
            {
                if (this.bufferedStates[num].TimeStamp > interpolationTime && num != this.statesCount - 1)
                {
                    num++;
                    continue;
                }
                break;
            }
            return;
        }
        NetworkTransform networkTransform = this.bufferedStates[Mathf.Max(num - 1, 0)];
        NetworkTransform networkTransform2 = this.bufferedStates[num];
        float num2 = (float)(networkTransform.TimeStamp - networkTransform2.TimeStamp);
        float t = 0f;
        if ((double)num2 > 0.0001)
        {
            t = (float)(interpolationTime - networkTransform2.TimeStamp) / num2;
        }
        Vector3 vector = Vector3.Lerp(networkTransform2.Position, networkTransform.Position, t);
        Vector3 vector2 = Vector3.Lerp(this.lastPosition, vector, 0.2f);
        this.interpolatedSpeed = vector2 - base.transform.position;
        if ((int)(Mathf.Abs(this.interpolatedSpeed.x) + Mathf.Abs(this.interpolatedSpeed.y) + Mathf.Abs(this.interpolatedSpeed.z)) > 4)
        {
            this.interpolatedSpeed = Vector3.zero;
        }
        base.transform.position = vector2;
        this.lastPosition = vector2;
        this.lastPositionTimeStamp = interpolationTime;
        if (!this.IgnoreAngles)
        {
            Vector3 finish = this.Clerp(networkTransform2.Speed, networkTransform.Speed, t);
            Vector3 vector3 = this.Clerp(this.lastRotation, finish, 0.2f);
            base.transform.localEulerAngles = new Vector3(0f, vector3.y, 0f);
            this.lastRotation = vector3;
            if ((UnityEngine.Object)this.Animator != (UnityEngine.Object)null)
            {
                this.Animator.BendAnimationAngle = vector3.x;
            }
        }
        this.SetRealPlayerPosition(vector, new Color(0f, 1f, 0f, 1f));
    }

    private void Extrapolate(long interpolationTime)
    {
        float num = Convert.ToSingle(interpolationTime - this.bufferedStates[0].TimeStamp) / 1000f;
        if (num < (float)this.extrapolationForwardTime / 1000f && this.statesCount > 1)
        {
            Vector3 a = this.bufferedStates[0].Position - this.bufferedStates[1].Position;
            float num2 = Vector3.Distance(this.bufferedStates[0].Position, this.bufferedStates[1].Position);
            float num3 = Convert.ToSingle(this.bufferedStates[0].TimeStamp - this.bufferedStates[1].TimeStamp) / 1000f;
            if (Mathf.Approximately(num2, 0f) || Mathf.Approximately(num3, 0f))
            {
                base.transform.position = this.bufferedStates[0].Position;
            }
            else
            {
                float num4 = num2 / num3;
                a = a.normalized;
                Vector3 b = this.bufferedStates[0].Position + a * num * num4;
                Vector3 position = this.bufferedStates[0].Position;
                b.y = Mathf.Lerp(position.y, b.y, 0.5f);
                Vector3 vector = Vector3.Lerp(this.lastPosition, b, Time.deltaTime * num4);
                Vector3 position2 = Vector3.Lerp(this.lastPosition, vector, 0.2f);
                base.transform.position = position2;
                this.lastPosition = position2;
                this.lastPositionTimeStamp = interpolationTime;
                this.SetRealPlayerPosition(vector, new Color(0f, 1f, 0f, 1f));
            }
        }
    }

    private void Update()
    {
        this.mode = NetworkDev.InterpolationMode;
        if (this.running && this.statesCount != 0 && !((UnityEngine.Object)TimeManager.Instance == (UnityEngine.Object)null))
        {
            long networkTime = TimeManager.Instance.NetworkTime;
            long num = networkTime - this.interpolationBackTime - NetworkDev.DelayValue;
            if (NetworkDev.DelayValue > 0 && this.bufferedStates.Length <= 40)
            {
                int num2 = (int)((NetworkDev.DelayValue + this.interpolationBackTime) / 20);
                this.bufferedStates = new NetworkTransform[num2];
                this.bufferedAnimationStates = new NetworkAnimationState[num2];
                this.bufferedAnimationKeys = new NetworkAnimationKey[num2];
                this.animationStatesCount = 0;
                this.animationKeysCount = 0;
                this.statesCount = 0;
            }
            if ((UnityEngine.Object)this.Player != (UnityEngine.Object)null)
            {
                if (this.animationStatesCount > 0)
                {
                    if (this.bufferedAnimationStates[0].TimeStamp > num)
                    {
                        int num3 = 0;
                        while (num3 < this.animationStatesCount)
                        {
                            if (this.bufferedAnimationStates[num3].TimeStamp > num && num3 != this.animationStatesCount - 1)
                            {
                                num3++;
                                continue;
                            }
                            this.player.SetAnimationState(this.bufferedAnimationStates[num3].State);
                            break;
                        }
                    }
                    else
                    {
                        this.player.SetAnimationState(this.bufferedAnimationStates[0].State);
                    }
                }
                if (this.animationKeysCount > 0)
                {
                    if (this.bufferedAnimationKeys[0].TimeStamp > num)
                    {
                        int num4 = 0;
                        while (num4 < this.animationKeysCount)
                        {
                            if (this.bufferedAnimationKeys[num4].TimeStamp > num && num4 != this.animationKeysCount - 1)
                            {
                                num4++;
                                continue;
                            }
                            this.player.SetAnimationKey(this.bufferedAnimationKeys[num4].Key);
                            break;
                        }
                    }
                    else
                    {
                        this.player.SetAnimationKey(this.bufferedAnimationKeys[0].Key);
                    }
                }
                else
                {
                    this.player.SetAnimationKey(0);
                }
            }
            if (this.mode == InterpolationMode.SMOOTH_LINEAR_IN_EX || this.mode == InterpolationMode.SMOOTH_LINEAR_IN)
            {
                this.NewInterpolation(num);
            }
            else if (this.mode == InterpolationMode.BEZIER_CUBIC_IN_EX && this.bufferedStates[0].TimeStamp > num)
            {
                int num5 = 0;
                while (true)
                {
                    if (num5 < this.statesCount)
                    {
                        if (this.bufferedStates[num5].TimeStamp > num && num5 != this.statesCount - 1)
                        {
                            num5++;
                            continue;
                        }
                        break;
                    }
                    return;
                }
                NetworkTransform networkTransform = this.bufferedStates[Mathf.Max(num5 - 1, 0)];
                NetworkTransform networkTransform2 = this.bufferedStates[num5];
                float num6 = (float)(networkTransform.TimeStamp - networkTransform2.TimeStamp);
                float num7 = 0f;
                if ((double)num6 > 0.0001)
                {
                    num7 = (float)(num - networkTransform2.TimeStamp) / num6;
                }
                Vector3 position = networkTransform2.Position;
                Vector3 a = networkTransform2.Position + networkTransform2.Speed * Time.deltaTime / 3f;
                Vector3 a2 = networkTransform.Position - networkTransform.Speed * Time.deltaTime / 3f;
                Vector3 position2 = networkTransform.Position;
                base.transform.position = Mathf.Pow(1f - num7, 3f) * position + 3f * num7 * Mathf.Pow(1f - num7, 2f) * a + 3f * Mathf.Pow(num7, 2f) * (1f - num7) * a2 + Mathf.Pow(num7, 3f) * position2;
                if (!this.IgnoreAngles)
                {
                    base.transform.localEulerAngles = this.Clerp(networkTransform2.Rotation, networkTransform.Rotation, num7);
                }
            }
            else if (this.mode == InterpolationMode.BEZIER_LINEAR_IN && this.bufferedStates[0].TimeStamp > num)
            {
                int num8 = 0;
                while (true)
                {
                    if (num8 < this.statesCount)
                    {
                        if (this.bufferedStates[num8].TimeStamp > num && num8 != this.statesCount - 1)
                        {
                            num8++;
                            continue;
                        }
                        break;
                    }
                    return;
                }
                NetworkTransform networkTransform3 = this.bufferedStates[Mathf.Max(num8 - 1, 0)];
                NetworkTransform networkTransform4 = this.bufferedStates[num8];
                float num9 = (float)(networkTransform3.TimeStamp - networkTransform4.TimeStamp);
                float t = 0f;
                if ((double)num9 > 0.0001)
                {
                    t = (float)(num - networkTransform4.TimeStamp) / num9;
                }
                Vector3 vector = Vector3.Lerp(networkTransform4.Position, networkTransform3.Position, t);
                this.interpolatedSpeed = vector - base.transform.position;
                if ((int)(Mathf.Abs(this.interpolatedSpeed.x) + Mathf.Abs(this.interpolatedSpeed.y) + Mathf.Abs(this.interpolatedSpeed.z)) > 4)
                {
                    this.interpolatedSpeed = Vector3.zero;
                }
                base.transform.position = vector;
                if (!this.IgnoreAngles)
                {
                    Vector3 vector2 = this.Clerp(networkTransform4.Speed, networkTransform3.Speed, t);
                    base.transform.localEulerAngles = new Vector3(0f, vector2.y, 0f);
                    if ((UnityEngine.Object)this.Animator != (UnityEngine.Object)null)
                    {
                        this.Animator.BendAnimationAngle = vector2.x;
                    }
                }
                this.SetRealPlayerPosition(vector, new Color(0f, 1f, 0f, 1f));
            }
            else if (this.mode != InterpolationMode.BEZIER_LINEAR_IN && this.mode != InterpolationMode.BEZIER_CUBIC_IN_EX)
            {
                if (this.mode != InterpolationMode.INTERPOLATION || this.bufferedStates[0].TimeStamp <= num)
                {
                    float num10 = Convert.ToSingle(networkTime - this.bufferedStates[0].TimeStamp) / 1000f;
                    if (this.mode == InterpolationMode.EXTRAPOLATION && num10 < (float)this.extrapolationForwardTime && this.statesCount > 1)
                    {
                        Vector3 a3 = this.bufferedStates[0].Position - this.bufferedStates[1].Position;
                        float num11 = Vector3.Distance(this.bufferedStates[0].Position, this.bufferedStates[1].Position);
                        float num12 = Convert.ToSingle(this.bufferedStates[0].TimeStamp - this.bufferedStates[1].TimeStamp) / 1000f;
                        if (!Mathf.Approximately(num11, 0f) && !Mathf.Approximately(num12, 0f))
                        {
                            float num13 = num11 / num12;
                            a3 = a3.normalized;
                            Vector3 b = this.bufferedStates[0].Position + a3 * num10 * num13;
                            base.transform.position = Vector3.Lerp(base.transform.position, b, Time.deltaTime * num13);
                            goto IL_085f;
                        }
                        base.transform.position = this.bufferedStates[0].Position;
                        if (!this.IgnoreAngles)
                        {
                            base.transform.localEulerAngles = this.bufferedStates[0].Rotation;
                        }
                        return;
                    }
                    base.transform.position = this.bufferedStates[0].Position;
                    goto IL_085f;
                }
                int num14 = 0;
                while (true)
                {
                    if (num14 < this.statesCount)
                    {
                        if (this.bufferedStates[num14].TimeStamp > num && num14 != this.statesCount - 1)
                        {
                            num14++;
                            continue;
                        }
                        break;
                    }
                    return;
                }
                NetworkTransform networkTransform5 = this.bufferedStates[Mathf.Max(num14 - 1, 0)];
                NetworkTransform networkTransform6 = this.bufferedStates[num14];
                float num15 = (float)(networkTransform5.TimeStamp - networkTransform6.TimeStamp);
                float t2 = 0f;
                if ((double)num15 > 0.0001)
                {
                    t2 = (float)(num - networkTransform6.TimeStamp) / num15;
                }
                base.transform.position = Vector3.Lerp(networkTransform6.Position, networkTransform5.Position, t2);
                if (!this.IgnoreAngles)
                {
                    base.transform.localEulerAngles = Vector3.Lerp(networkTransform6.Rotation, networkTransform5.Rotation, t2);
                }
            }
        }
        return;
        IL_085f:
        if (!this.IgnoreAngles)
        {
            base.transform.localEulerAngles = this.bufferedStates[0].Rotation;
        }
    }

    private float AngleDifference(float newAngle, float currentAngle)
    {
        float num = newAngle - currentAngle;
        if (num > 180f)
        {
            num = currentAngle + 360f - newAngle;
        }
        if (num < -180f)
        {
            num = currentAngle - 360f - newAngle;
        }
        return num;
    }

    private Vector3 Clerp(Vector3 start, Vector3 finish, float t)
    {
        Vector3 result = start * (1f - t) + finish * t;
        if (Mathf.Abs(finish.x - start.x) > Math.Abs(360f + Mathf.Min(finish.x, start.x) - Mathf.Max(finish.x, start.x)))
        {
            if (finish.x > start.x)
            {
                result.x = (360f + start.x) * (1f - t) + finish.x * t;
            }
            else
            {
                result.x = start.x * (1f - t) + (360f + finish.x) * t;
            }
            if (result.x > 360f)
            {
                result.x -= 360f;
            }
        }
        if (Mathf.Abs(finish.y - start.y) > Math.Abs(360f + Mathf.Min(finish.y, start.y) - Mathf.Max(finish.y, start.y)))
        {
            if (finish.y > start.y)
            {
                result.y = (360f + start.y) * (1f - t) + finish.y * t;
            }
            else
            {
                result.y = start.y * (1f - t) + (360f + finish.y) * t;
            }
            if (result.y > 360f)
            {
                result.y -= 360f;
            }
        }
        if (Mathf.Abs(finish.z - start.z) > Math.Abs(360f + Mathf.Min(finish.z, start.z) - Mathf.Max(finish.z, start.z)))
        {
            if (finish.z > start.z)
            {
                result.z = (360f + start.z) * (1f - t) + finish.z * t;
            }
            else
            {
                result.z = start.z * (1f - t) + (360f + finish.z) * t;
            }
            if (result.z > 360f)
            {
                result.z -= 360f;
            }
        }
        return result;
    }

    private void UpdateValues()
    {
        double num = (double)TimeManager.Instance.AveragePing;
        if (num < 50.0)
        {
            this.interpolationBackTime = 50L;
        }
        else if (num < 100.0)
        {
            this.interpolationBackTime = 100L;
        }
        else if (num < 200.0)
        {
            this.interpolationBackTime = 200L;
        }
        else if (num < 400.0)
        {
            this.interpolationBackTime = 400L;
        }
        else if (num < 600.0)
        {
            this.interpolationBackTime = 600L;
        }
        else
        {
            this.interpolationBackTime = 1000L;
        }
    }

    public void Reset()
    {
        this.statesCount = 0;
    }
}


