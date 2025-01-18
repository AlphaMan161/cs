// dnSpy decompiler from Assembly-CSharp.dll class: TurretTeslaTracer
using System;
using UnityEngine;

public class TurretTeslaTracer : TurretTracer
{
	private void Start()
	{
		ParticleEmitter[] componentsInChildren = base.transform.GetComponentsInChildren<ParticleEmitter>();
		foreach (ParticleEmitter particleEmitter in componentsInChildren)
		{
			particleEmitter.emit = false;
		}
		ParticleRenderer[] componentsInChildren2 = base.transform.GetComponentsInChildren<ParticleRenderer>();
		foreach (ParticleRenderer particleRenderer in componentsInChildren2)
		{
			particleRenderer.enabled = false;
		}
	}

	public override void setVisible(bool visible)
	{
		MeshRenderer[] componentsInChildren = base.transform.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer meshRenderer in componentsInChildren)
		{
			meshRenderer.enabled = visible;
		}
	}
}
