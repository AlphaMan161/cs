// dnSpy decompiler from Assembly-CSharp.dll class: LocalArrowTracer
using System;
using UnityEngine;

public class LocalArrowTracer : MonoBehaviour
{
	private void Start()
	{
		this.launchDelay = 200L;
	}

	public void Launch(Vector3 position, WeaponType weaponType, int life)
	{
		this.launchTime = TimeManager.Instance.NetworkTime + this.launchDelay;
		this.lastScanTime = TimeManager.Instance.NetworkTime;
	}

	private void FixedUpdate()
	{
		if (TimeManager.Instance.NetworkTime >= this.launchTime || this.launchTime == 0L)
		{
			this.active = true;
		}
		if (this.active && (this.lastScanTime < TimeManager.Instance.NetworkTime + 100L || this.lastScanTime == 0L))
		{
			this.lastScanTime = TimeManager.Instance.NetworkTime;
			if (LocalPlayerManager.Instance.LocalScan(base.transform.position, (float)this.touchDistance))
			{
				LocalPlayerManager.Instance.hideArrow(base.transform.GetComponent<AlphaTween>());
				this.active = false;
				UnityEngine.Object.Destroy(this);
				return;
			}
		}
	}

	private long launchDelay = 200L;

	private long launchTime;

	private long lastScanTime;

	private long touchDistance = 20L;

	private bool active;
}
