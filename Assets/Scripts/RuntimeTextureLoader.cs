// ILSpyBased#2
using System.Collections;
using UnityEngine;

public class RuntimeTextureLoader : MonoBehaviour
{
    private Material material;

    public string textureID = string.Empty;

    public string Folder = "Cloth/";

    public int materialID;

    protected void LoadTexture(string textureID)
    {
        if ((Object)this.material != (Object)null && textureID != string.Empty)
        {
            if (textureID == "steel_7" || textureID == "wood_06a")
            {
                UnityEngine.Debug.LogError(string.Format("{0} on {1}", textureID, base.name));
            }
            Hashtable hashtable = new Hashtable();
            hashtable[(byte)151] = this.Folder + textureID;
            this.LoadTextureFromResources(textureID, "_MainTex");
        }
    }

    protected void ApplyTexture(bool success, AssetBundle assetBundle, Hashtable parameters)
    {
        if (success)
        {
            Texture2D texture = assetBundle.LoadAllAssets()[0] as Texture2D;
            this.material.SetTexture("_MainTex", texture);
        }
    }

    protected IEnumerator ApplyTextureAsync(bool success, AssetBundle assetBundle, Hashtable parameters)
    {
        AssetBundleRequest assetLoadRequest = assetBundle.LoadAllAssetsAsync();
        yield return (object)assetLoadRequest;
        Texture2D texture = assetLoadRequest.allAssets[0] as Texture2D;
        this.material.SetTexture("_MainTex", texture);
        UnityEngine.Object.DestroyImmediate(this);
    }

    private void Start()
    {
        this.ForceLoad();
    }

    public void ForceLoad()
    {
        Renderer component = base.gameObject.GetComponent<Renderer>();
        if (!((Object)component == (Object)null))
        {
            if (this.materialID == 0)
            {
                this.material = component.sharedMaterial;
				if (component.sharedMaterial.shader.name == material.shader.name)
				{
					if (Folder == "Cloth/") {
						if (textureID.StartsWith ("CLOTH") || textureID.StartsWith ("guybrush")) {
                            switch (textureID)
                            {
                                case "CLOTH_mask_01_color_02":
                                    component.sharedMaterial.shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
                                    break;
                                case "CLOTH_mask_01_color_03":
                                    component.sharedMaterial.shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
                                    break;
                                case "CLOTH_Masks_Goggles_Business01_Color":
                                    component.sharedMaterial.shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
                                    break;
                            }
                            if (component.sharedMaterial.name != "icicle_Color_")
                            {
                                component.sharedMaterial.shader = Shader.Find("Legacy Shaders/Diffuse");
                            }
                            else {
                                component.sharedMaterial.mainTexture = (Texture2D)Resources.Load("Textures/Cloth/icicle_Color");
                            }
                            } else {
                            switch (textureID) {
                                case "milkor_grenade_Color":
                                    component.sharedMaterial.shader = Shader.Find("Legacy Shaders/Diffuse");
                                    break;
                                case "christmas_candy":
                                    component.sharedMaterial.shader = Shader.Find("Legacy Shaders/Diffuse");
                                    break;
                                case "christmas_candy2":
                                    component.sharedMaterial.shader = Shader.Find("Legacy Shaders/Diffuse");
                                    break;
                                case "bat_wooden_color":
                                    component.sharedMaterial.shader = Shader.Find("Legacy Shaders/Diffuse");
                                    break;
                                case "icicle_Color":
                                    component.sharedMaterial.mainTexture = (Texture2D)Resources.Load("Textures/Cloth/icicle_Color");
                                    component.sharedMaterial.shader = Shader.Find("Legacy Shaders/Transparent/Specular");
                                    break;
                                case "katana_Color":
                                    component.sharedMaterial.shader = Shader.Find("Legacy Shaders/Diffuse");
                                    break;
                                case "katana_Blood_Color":
                                    component.sharedMaterial.shader = Shader.Find("Legacy Shaders/Diffuse");
                                    break;
                                case "torch_color":
                                    component.sharedMaterial.shader = Shader.Find("Legacy Shaders/Diffuse");
                                    ParticleSystemRenderer components = base.gameObject.GetComponentInChildren<ParticleSystemRenderer>();
                                    components.sharedMaterial.shader = Shader.Find("Particles/Additive");
                                    break;
                                case "scythe_Color":
                                    component.sharedMaterial.shader = Shader.Find("Legacy Shaders/Diffuse");
                                    break;
                                case "Crowbar_color":
                                    component.sharedMaterial.shader = Shader.Find("Legacy Shaders/Diffuse");
                                    break;
                                case "n1_Color":
                                    component.sharedMaterial.shader = Shader.Find("Reflective/Diffuse");
                                    break;
                                case "Taurus_Color_02":
                                    component.sharedMaterial.shader = Shader.Find("Reflective/Diffuse");
                                    if (gameObject.name == "Taurus_LOD0")
                                    {
                                        transform.parent.parent.parent.parent.parent.parent.parent.GetChild(1).GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader = Shader.Find("Legacy Shaders/Diffuse");
                                        transform.parent.parent.parent.parent.parent.parent.parent.GetChild(2).GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader = Shader.Find("Legacy Shaders/Diffuse");
                                    }
                                    break;
                                case "Stickybomb_Diesad":
                                    component.sharedMaterial.shader = Shader.Find("Legacy Shaders/Diffuse");
                                    break;
                                default:
                                    if (gameObject.name.StartsWith("Gloves")) {
                                        component.sharedMaterial.shader = Shader.Find("Legacy Shaders/Diffuse");
                                    }
                                    else if(textureID == "crossbow_01_d_AIO")
                                    {
                                        component.sharedMaterial.shader = Shader.Find("Reflective/Diffuse");
                                     //   gameObject.AddComponent<BoxCollider>();
                                    }
                                    else{
                                        component.sharedMaterial.shader = Shader.Find("Reflective/Diffuse");
                                    }
                                    break;
                            }
                        }
					} else {
                        RenderSettings.skybox.shader = Shader.Find ("Skybox/Cubemap");
                        if (textureID == "wire_var2_color") {
                            component.sharedMaterial.shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
                        }
                        else if (textureID == "glass_02") {
                            component.sharedMaterial.shader = Shader.Find("Transparent/Diffuse");
                        } else {
                            component.sharedMaterial.shader = Shader.Find("Legacy Shaders/Diffuse");
                        }
					}
				}
            }
            else
            {
                this.material = component.sharedMaterials[this.materialID - 1];
                if (textureID == "CLOTH_Mask_Gasmask02_Color" && this.materialID == 2)
                {
                    component.sharedMaterial.shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
                    Renderer[] components = GetComponents<Renderer>();
                    foreach (Renderer renderer in components)
                    {
                        renderer.materials[1].shader = Shader.Find("Legacy Shaders/Diffuse");
                    }
                }
                if (textureID == "CLOTH_Mask_Gasmask01_Color" && this.materialID == 2)
                {
                    component.sharedMaterial.shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
                    Renderer[] components = GetComponents<Renderer>();
                    foreach (Renderer renderer in components)
                    {
                        renderer.materials[1].shader = Shader.Find("Legacy Shaders/Diffuse");
                    }
                }
            }
            if (component.sharedMaterial.name == "icicle_Color_")
            {
                component.sharedMaterial.mainTexture = (Texture2D)Resources.Load("Textures/Cloth/icicle_Color");
            }
            else
            {
                this.LoadTexture(this.textureID);
            }
        }
    }
    public virtual void OnTextureLoaded(bool success, AssetBundle assetBundle, Hashtable parameters)
    {
        if (success)
        {
            base.StartCoroutine(this.ApplyTextureAsync(success, assetBundle, parameters));
        }
    }

    public virtual void OnTextureLoadedAndReady(bool success, Texture2D texture, Hashtable parameters)
    {
        if (success)
        {
            this.material.SetTexture("_MainTex", texture);
        }
        UnityEngine.Object.DestroyImmediate(this);
    }

    protected void LoadTextureFromResources(string textureID, string textureName)
    {
        if ((Object)this.material != (Object)null && textureID != string.Empty)
        {
            base.StartCoroutine(this.LoadAsyncFromResources("Textures/" + this.Folder + textureID, textureName));
        }
    }

    private IEnumerator LoadAsyncFromResources(string path, string name)
    {
        ResourceRequest request = Resources.LoadAsync(path);
        yield return (object)request;
        Texture texture = (Texture)request.asset;
        this.material.SetTexture(name, texture);
    }
}


