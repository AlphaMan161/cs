// dnSpy decompiler from Assembly-CSharp.dll class: TriggerDeleteAfterSeconds
using System;
using UnityEngine;

public class TriggerDeleteAfterSeconds : MonoBehaviour
{
	private void Start()
	{
	}

	public void Trigger()
	{
		UnityEngine.Object.Destroy(base.gameObject, this.seconds);
	}

	public float seconds = 1f;
}
