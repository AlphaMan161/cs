// dnSpy decompiler from Assembly-CSharp.dll class: MineRemoteTracer
using System;

public class MineRemoteTracer : MineTracer
{
	public void BlowRemoteMine()
	{
		base.BlowMine(this.shot.Origin);
	}

	private void FixedUpdate()
	{
		if (TimeManager.Instance.NetworkTime >= this.launchTime && TimeManager.Instance.NetworkTime < this.landingTime)
		{
			this.active = true;
			base.setMineVisible(true);
		}
		else if (TimeManager.Instance.NetworkTime > this.landingTime)
		{
			if (this.shot != null)
			{
				base.BlowMine(this.shot.Origin);
			}
			else
			{
				this.Destroy();
			}
		}
		if (this.active)
		{
			long num = this.landingTime - TimeManager.Instance.NetworkTime;
			if (num <= 0L)
			{
				base.BlowMine(this.shot.Origin);
				return;
			}
		}
	}
}
