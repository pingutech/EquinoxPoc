using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneLoader : MonoBehaviour
{
    public GameObject objectToHide;

    private void Start()
    {
        UniAndroidPermission.RequestPermission(AndroidPermission.ACCESS_FINE_LOCATION, AccessCamera, AccessCamera);
        Invoke("AccessCamera", 0.5f);
    }

    void AccessCamera()
    {
        UniAndroidPermission.RequestPermission(AndroidPermission.CAMERA);
    }

    public void LoadDemo()
    {
        SceneManager.LoadScene(1);
    }

    public void Hide()
    {
        objectToHide.SetActive(false);
    }
}
