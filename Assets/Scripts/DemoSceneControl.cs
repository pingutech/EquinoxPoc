using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoSceneControl : MonoBehaviour {

    int sceneIdx;

    // Use this for initialization
    void Start()
    {
        sceneIdx = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape) && sceneIdx > 0)
        //    SceneManager.LoadScene(sceneIdx - 1);
        //else Application.Quit();
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

    }
}
