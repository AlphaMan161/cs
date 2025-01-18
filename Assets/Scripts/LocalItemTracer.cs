// dnSpy decompiler from Assembly-CSharp.dll class: LocalItemTracer
using System;
using UnityEngine;

public class LocalItemTracer : ItemTracer
{
	public ItemType ItemType
	{
		get
		{
			return this.itemType;
		}
	}

	public short ItemSubType
	{
		get
		{
			return this.itemSubType;
		}
	}

	public int Amount
	{
		get
		{
			return this.amount;
		}
	}

	private void Start()
	{
	}

	public void Launch(Vector3 position, ItemType itemType, short itemSubType, int amount)
	{
		this.itemType = itemType;
		this.itemSubType = itemSubType;
		this.amount = amount;
		this.itemType = itemType;
		this.launchTime = TimeManager.Instance.NetworkTime + (long)this.launchDelay;
		base.transform.position = position;
		this.lastScanTime = TimeManager.Instance.NetworkTime;
	}

	public void setVisible(bool visible)
	{
		ParticleEmitter[] componentsInChildren = base.transform.GetComponentsInChildren<ParticleEmitter>();
		foreach (ParticleEmitter particleEmitter in componentsInChildren)
		{
			particleEmitter.emit = visible;
		}
		Renderer[] componentsInChildren2 = base.transform.GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in componentsInChildren2)
		{
			renderer.enabled = visible;
		}
	}

	private void LateUpdate()
	{
		if (TimeManager.Instance.NetworkTime >= this.launchTime)
		{
			this.active = true;
		}
		if (this.active && this.lastScanTime < TimeManager.Instance.NetworkTime + 100L)
		{
			this.lastScanTime = TimeManager.Instance.NetworkTime;
			if (LocalPlayerManager.Instance.LocalScan(base.transform.position, this.touchDistance))
			{
				this.Trigger();
				return;
			}
		}
	}

	protected void Trigger()
	{
		LocalPlayerManager.Instance.PickItem(this);
	}

	public override void Destroy()
	{
		this.active = false;
		base.enabled = false;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public int launchDelay = 1500;

	protected long lastScanTime;

	protected bool active;

	protected float touchDistance = 8f;

	protected CombatPlayer player;

	private ItemType itemType;

	private short itemSubType;

	private int amount;
}
