using UnityEngine;
using UnityEditor;
using System.IO;

public class ItemSnapshotExporter : MonoBehaviour
{
    public Camera previewCamera;   // deine Preview-Kamera
    public RenderTexture renderTexture; // deine RenderTexture
    public string savePath = "Assets/Sprites/";

    [ContextMenu("Export PNG")]
    public void ExportItemPNG()
    {
        // 2) Kamera rendern
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = renderTexture;
        previewCamera.Render();

        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0,0,renderTexture.width, renderTexture.height),0,0);
        tex.Apply();

        RenderTexture.active = currentRT;

        // 3) PNG speichern
        byte[] bytes = tex.EncodeToPNG();
        if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
        File.WriteAllBytes(savePath + "test.png", bytes);
        Debug.Log("PNG gespeichert: " + savePath + "test.png");

        // 4) Cleanup
    }
}
