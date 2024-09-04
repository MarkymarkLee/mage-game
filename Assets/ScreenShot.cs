using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
    public Camera ScreenShotCamera;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // print("Space key was pressed");
            SaveCameraView(ScreenShotCamera);
        }
    }

    void SaveCameraView(Camera cam)
    {
        RenderTexture screenTexture = new RenderTexture(Screen.width, Screen.height, 16);
        cam.targetTexture = screenTexture;
        RenderTexture.active = screenTexture;
        cam.Render();

        Texture2D renderedTexture = new Texture2D(Screen.width, Screen.height);
        renderedTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        RenderTexture.active = null;

        byte[] byteArray = renderedTexture.EncodeToPNG();
        // print(Application.dataPath + "/ScreenShots/" + System.DateTime.Now.ToString("MM-dd-yy (HH-mm-ss)") + ".png");
        System.IO.File.WriteAllBytes(Application.dataPath + "/ScreenShots/" + System.DateTime.Now.ToString("MM-dd-yy (HH-mm-ss)") + ".png", byteArray);
    }
}
