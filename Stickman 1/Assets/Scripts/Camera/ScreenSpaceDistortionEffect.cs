using UnityEngine;
using System.Collections;

public class ScreenSpaceDistortionEffect : MonoBehaviour
{
    RenderTexture shieldRT;
    RenderTexture screenRT;
    Camera distortCam;
    Camera mainCam;
    Material effectMaterial;

    void Awake()
    {
        screenRT = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.Default);
        screenRT.wrapMode = TextureWrapMode.Repeat;

        shieldRT = new RenderTexture(Screen.width / 4, Screen.height / 4, 0, RenderTextureFormat.Default);
        shieldRT.wrapMode = TextureWrapMode.Repeat;

        effectMaterial = new Material(Shader.Find("Custom/Composite"));

        mainCam = GetComponent<Camera>();
        mainCam.SetTargetBuffers(screenRT.colorBuffer, screenRT.depthBuffer);

        distortCam = new GameObject("DistortionCam").AddComponent<Camera>();
        distortCam.enabled = false;
    }

    void OnPostRender()
    {
        distortCam.CopyFrom(mainCam);
        distortCam.backgroundColor = Color.grey;
        distortCam.cullingMask = 1 << LayerMask.NameToLayer("Shield");
        distortCam.targetTexture = shieldRT;
        distortCam.Render();

        effectMaterial.SetTexture("_DistortionTex", shieldRT);
        Graphics.Blit(screenRT, null, effectMaterial);
    }

}
