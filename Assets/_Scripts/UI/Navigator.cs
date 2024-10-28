using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Navigator : MonoBehaviour
{
    public string sceneToLoad;

    public void Navigate()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneToLoad);
    }
}