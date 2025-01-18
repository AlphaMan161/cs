// dnSpy decompiler from Assembly-CSharp.dll class: BaseEnterTrigger
using System;
using UnityEngine;

public class BaseEnterTrigger : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		CombatPlayer component = other.transform.GetComponent<CombatPlayer>();
		if (component == PlayerManager.Instance.LocalPlayer)
		{
			PlayerManager.Instance.SendEnterBaseRequest(this.Team);
			return;
		}
	}

	public int Team = 3;
}
