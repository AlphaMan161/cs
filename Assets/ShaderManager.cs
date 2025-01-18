using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderManager : MonoBehaviour {

    void Awake() {
        List<Material> objectsInScene = new List<Material>();
        foreach (Material mat in Resources.FindObjectsOfTypeAll(typeof(Material)) as Material[]) {
            switch (mat.name)
            {
                case "Standard_2":
                    mat.shader = Shader.Find("Legacy Shaders/Diffuse");
                    break;
                case "whitesmoke":
                    mat.shader = Shader.Find("Particles/Additive");
                    break;
                case "Default-Particle":
                    mat.shader = Shader.Find("Particles/Additive");
                    break;
                case "MachineGunParticle":
                    mat.shader = Shader.Find("Particles/Alpha Blended");
                    break;
                case "GrenadeTraill":
                    mat.shader = Shader.Find("Particles/~Additive-Multiply");
                    break;
                case "hitSmokeParticles":
                    mat.shader = Shader.Find("Particles/Additive (Soft)");
                   break;
                case "hitFireHotParticles":
                    mat.shader = Shader.Find("Particles/Additive");
                    break;
                case "HotParticles":
                    mat.shader = Shader.Find("Particles/Additive (Soft)");
                    break;
                case "hitFireParticles":
                    mat.shader = Shader.Find("Particles/Multiply");
                    break;
                case "MachineGunShaftSpark":
                    mat.shader = Shader.Find("Particles/Additive");
                    break;
                case "TripleRocketTrail":
                    mat.shader = Shader.Find("Particles/Additive");
                    break;
                case "No Name":
                    mat.shader = Shader.Find("Legacy Shaders/Diffuse");
                    break;
           //     case "RessurectShild":
             //       mat.shader = Shader.Find("Particles/Alpha Blended");
             //       break;
                case "ControlPointNeutral":
                    mat.shader = Shader.Find("Legacy Shaders/Diffuse");
                    break;
                case "ControlPointBlue":
                    mat.shader = Shader.Find("Legacy Shaders/Diffuse");
                    break;
                case "ControlPointRed":
                    mat.shader = Shader.Find("Legacy Shaders/Diffuse");
                    break;
                case "cp_tablet_A_Color":
                    mat.shader = Shader.Find("Legacy Shaders/Diffuse");
                    break;
                case "cp_aura_mini_Neutral_Color":
                    mat.shader = Shader.Find("Particles/Additive (Soft)");
                    break;
                case "FlagPlatform_Blue":
                    mat.shader = Shader.Find("Legacy Shaders/Diffuse");
                    break;
                case "FlagPlatform_Red":
                    mat.shader = Shader.Find("Legacy Shaders/Diffuse");
                    break;
                case "enemyHitIndicator":
                    mat.shader = Shader.Find("Particles/Alpha Blended");
                    break;
                case "cp_tablet_B":
                    mat.shader = Shader.Find("Legacy Shaders/Diffuse");
                    break;
                case "guybrush_torso01_Color_0":
                    mat.shader = Shader.Find("Legacy Shaders/Diffuse");
                    break;
                case "guybrush_hands01_Color_0":
                    mat.shader = Shader.Find("Legacy Shaders/Diffuse");
                    break;
                case "Taurus_bullets_Color":
                    mat.shader = Shader.Find("Legacy Shaders/Diffuse");
                    break;
                case "FlamerJetBlue":
                    mat.shader = Shader.Find("Particles/Additive");
                    break;
                case "cp_tablet_C":
                    mat.shader = Shader.Find("Legacy Shaders/Diffuse");//FlamerJetBlue
                    break;
                case "icicle_Color_":
                    mat.shader = Shader.Find("Legacy Shaders/Transparent/Specular");
                    break;
            }
        }
    }
}
