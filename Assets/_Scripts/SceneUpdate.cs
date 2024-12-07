using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MainManager.Instance.currentScene = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
