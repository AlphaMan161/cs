// dnSpy decompiler from Assembly-CSharp.dll class: CharacterPusher
using System;
using UnityEngine;

internal class CharacterPusher : MonoBehaviour
{
	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Rigidbody attachedRigidbody = hit.collider.attachedRigidbody;
		if (attachedRigidbody == null || attachedRigidbody.isKinematic)
		{
			return;
		}
		if ((double)hit.moveDirection.y < -0.3)
		{
			return;
		}
		Vector3 a = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z);
		attachedRigidbody.velocity = a * this.pushPower;
	}

	public float pushPower = 2f;
}
