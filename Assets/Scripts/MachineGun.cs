// dnSpy decompiler from Assembly-CSharp.dll class: MachineGun
using System;
using UnityEngine;

public class MachineGun : MonoBehaviour
{
	private void Start()
	{
		this.barrels = base.transform.GetComponentsInChildren<Animation>();
		this.turn(false);
		foreach (Animation animation in this.barrels)
		{
			animation.Stop();
		}
	}

	public void fire()
	{
		if (this.barrels == null)
		{
			return;
		}
		this.endTime = TimeManager.Instance.NetworkTime + (long)this.periodTime;
		if (!this.on)
		{
			this.turn(true);
		}
	}

	public void turn(bool on)
	{
		if (on)
		{
			foreach (Animation animation in this.barrels)
			{
				animation.Stop();
				animation.Play(this.AnimationName);
			}
		}
		else
		{
			foreach (Animation animation2 in this.barrels)
			{
				animation2.Stop();
			}
		}
		this.on = on;
	}

	private void LateUpdate()
	{
		if (!this.on)
		{
			return;
		}
		if (this.endTime < TimeManager.Instance.NetworkTime)
		{
			this.turn(false);
		}
	}

	public int periodTime = 150;

	private long endTime;

	private Animation[] barrels;

	public string AnimationName;

	private bool on;
}
