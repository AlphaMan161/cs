// dnSpy decompiler from Assembly-CSharp.dll class: DeleteAfterSeconds
using System;
using UnityEngine;

public class DeleteAfterSeconds : MonoBehaviour
{
	private void Start()
	{
        if (gameObject.name.StartsWith("Explo("))
        {
            Projector[] componentsInChildren = base.GetComponentsInChildren<Projector>();
            componentsInChildren[0].material.shader = Shader.Find("Projector/Multiply");
        }
        else if (gameObject.name.StartsWith("HealthHit"))
        {
            ParticleRenderer[] componentsInChildren2 = base.GetComponentsInChildren<ParticleRenderer>();
            componentsInChildren2[0].material.shader = Shader.Find("Particles/Multiply");
        }
        else if (gameObject.name.StartsWith("Blood"))
        {
            ParticleRenderer[] componentsInChildren3 = base.GetComponentsInChildren<ParticleRenderer>();
            componentsInChildren3[0].material.shader = Shader.Find("Particles/Multiply");
            componentsInChildren3[1].material.shader = Shader.Find("Particles/Multiply");
        }
        else if (gameObject.name.StartsWith("ExploSnowPrefab"))
        {
            ParticleRenderer[] componentsInChildren4 = base.GetComponentsInChildren<ParticleRenderer>();
            Projector[] componentsInChildren5 = base.GetComponentsInChildren<Projector>();
            componentsInChildren5[0].material.shader = Shader.Find("Projector/Multiply");
            componentsInChildren4[1].material.shader = Shader.Find("Particles/Additive");
            //    componentsInChildren4[3].material.shader = Shader.Find("Particles/Additive");
        }
        else if (gameObject.name.StartsWith("CrossbowBoltPrefab"))
        {
            TrailRenderer componentInChildren5 = base.GetComponentInChildren<TrailRenderer>();
            componentInChildren5.material.shader = Shader.Find("Particles/Additive");
        }
            UnityEngine.Object.Destroy(base.gameObject, this.seconds);
	}

	public float seconds = 2f;
}
