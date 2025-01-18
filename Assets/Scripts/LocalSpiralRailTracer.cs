// dnSpy decompiler from Assembly-CSharp.dll class: LocalSpiralRailTracer
using System;
using UnityEngine;

public class LocalSpiralRailTracer : MonoBehaviour
{
	private void Start()
	{
	}

	public void Launch(Shot shot, CombatPlayer player, bool control)
	{
		this.shot = shot;
		this.launchTime = shot.TimeStamp;
		base.transform.LookAt(shot.Origin);
		this.player = player;
		this.start = base.transform.position;
		float magnitude = (this.start - shot.Origin).magnitude;
		float num = 3f;
		int num2 = (int)(magnitude * num);
		this.rocket = base.transform.FindChild("Rocket").gameObject;
		LineRenderer component = this.rocket.GetComponent<LineRenderer>();
		component.SetVertexCount(num2);
		for (int i = 0; i < num2; i++)
		{
			float num3 = (float)i / (magnitude * num);
			base.transform.position = this.start * (1f - num3) + shot.Origin * num3;
			float num4 = num3 * magnitude * 0.75f;
			float num5 = 1f;
			Vector3 localPosition = this.rocket.transform.localPosition;
			Vector3 vector = new Vector3(0f, 0f, num4 * 180f / 3.14159274f);
			localPosition.x = Mathf.Cos(num4) * num5;
			localPosition.y = Mathf.Sin(num4) * num5;
			this.rocket.transform.localPosition = localPosition;
			component.SetPosition(i, this.rocket.transform.position);
		}
	}

	private GameObject rocket;

	private Shot shot;

	private Vector3 start;

	private long launchTime;

	public long launchDelay = 200L;

	private CombatPlayer player;
}
