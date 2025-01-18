// ILSpyBased#2
using System;
using System.Collections;
using UnityEngine;

public class AnimationSynchronizer : MonoBehaviour
{
    public enum BaseAnimationState
    {
        Idle,
        RunForward,
        RunBackward
    }

    public enum SecondAnimationState
    {
        None,
        Shot,
        Reload
    }

    public enum AnimationState
    {
        GunStart,
        Gun,
        GunStop
    }

    public Animation anim;

    public Animation anim2;

    public Animation RailGunAnim;

    private BaseAnimationState baseState;

    public void StartReceivingAnimation()
    {
        this.InitAnimations();
        this.UpdateAnimation();
    }

    public void Start()
    {
        if ((bool)this.anim)
        {
            this.anim.Stop();
        }
        if ((bool)this.anim2)
        {
            this.anim.Stop();
        }
    }

    private void InitAnimations()
    {
    }

    private void UpdateAnimation()
    {
    }

    private IEnumerator ResetSecondState(float t)
    {
        yield return (object)new WaitForSeconds(t);
    }

    public void RemoteSecondStateUpdate(string message)
    {
        this.UpdateAnimation();
    }

    public void RemoteStateUpdate(string message)
    {
        BaseAnimationState baseAnimationState = this.baseState = (BaseAnimationState)(int)Enum.Parse(typeof(BaseAnimationState), message);
        this.UpdateAnimation();
    }

    private void SetAnimationState(BaseAnimationState state)
    {
        if (state != this.baseState)
        {
            this.baseState = state;
        }
    }

    public void PlayIdle()
    {
        this.SetAnimationState(BaseAnimationState.Idle);
    }

    public void PlayRunForward()
    {
        this.SetAnimationState(BaseAnimationState.RunForward);
    }

    public void PlayRunBackward()
    {
        this.SetAnimationState(BaseAnimationState.RunBackward);
    }

    public void PlayShotAnimation()
    {
    }

    public void PlayReloadAnimation()
    {
    }

    public void PlayMachineGunAnimation()
    {
        if ((bool)this.anim)
        {
            if ((TrackedReference)this.anim["MachineGunL1"] != (TrackedReference)null)
            {
                this.anim.Play("MachineGunL1");
            }
            else
            {
                this.anim.Play("MachineGun");
            }
        }
        if ((bool)this.anim2)
        {
            this.anim2.Play();
        }
    }

    public void StopMachineGunAnimation()
    {
        if ((bool)this.anim)
        {
            this.anim.Stop();
        }
        if ((bool)this.anim2)
        {
            this.anim2.Stop();
        }
    }

    public void PlayRailGunAnimation()
    {
        this.RailGunAnim.Play("RailGun");
    }
}


