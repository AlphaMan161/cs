// dnSpy decompiler from Assembly-CSharp.dll class: CharacterMotorMovement
using System;
using UnityEngine;

[Serializable]
public class CharacterMotorMovement
{
	public float maxForwardSpeed = 10f;

	public float maxSidewaysSpeed = 10f;

	public float maxBackwardsSpeed = 10f;

	public bool clampSpeedOnSlopes;

	public AnimationCurve maxSpeedOnSlope = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(-90f, 10f),
		new Keyframe(0f, 10f),
		new Keyframe(90f, 0f)
	});

	public float maxGroundAcceleration = 30f;

	public float maxGroundDecceration = 30f;

	public float maxAirAcceleration = 30f;

	public float maxAirDecceration = 30f;

	public float gravity = 30f;

	public float maxFallSpeed = 30f;

	[NonSerialized]
	public CollisionFlags collisionFlags;

	[NonSerialized]
	public Vector3 velocity;

	[NonSerialized]
	public Vector3 frameVelocity = Vector3.zero;

	[NonSerialized]
	public Vector3 hitPoint = Vector3.zero;

	[NonSerialized]
	public Vector3 lastHitPoint = new Vector3(float.PositiveInfinity, 0f, 0f);
}
