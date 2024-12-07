using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RestartGame()
    {
        string currentScene = MainManager.Instance.currentScene;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);
    }
}
