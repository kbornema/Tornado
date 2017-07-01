using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{

    public Renderer TextureRenderer;
    public MeshFilter MeshFilter;
    public MeshRenderer MeshRenderer;

    public void DrawTextureMap(Texture2D texture2D)
    {

        TextureRenderer.sharedMaterial.mainTexture = texture2D;
        TextureRenderer.transform.localScale = new Vector3(texture2D.width,1,texture2D.height);
    }

    public void DrawMesh(MeshData meshData, Texture2D texture2D)
    {
        MeshFilter.sharedMesh = meshData.CreateMesh();
        Constants.Instance.UpdateMesh(MeshFilter.gameObject);
        MeshRenderer.sharedMaterial.mainTexture = texture2D;
    }
}
