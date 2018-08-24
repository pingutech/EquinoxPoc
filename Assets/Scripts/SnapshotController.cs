using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;
using GoogleARCoreInternal;

public class SnapshotController : MonoBehaviour
{
    public UploadController UploadController;

    public Camera arCamera;

    public GameObject EnableSnapshotCam;
    public GameObject TakeSnapshot;
    public GameObject CancelCam;
    public GameObject Upload;

    public RawImage SnapshotImage;
    public AspectRatioFitter fit;

    private WebCamTexture _webCamTexture;
    private Texture2D _photo;

    private bool _camAvailable;
    private bool _uploaded;

    public Text arStatus;

    private void Start()
    {
        string selectedDeviceName = "";
        WebCamDevice[] allDevices = WebCamTexture.devices;
        for (int i = 0; i < allDevices.Length; i++)
        {
            if (!allDevices[i].isFrontFacing)
            {
                selectedDeviceName = allDevices[i].name;
                _camAvailable = true;
                break;
            }
        }

        _webCamTexture = new WebCamTexture(selectedDeviceName);
    }

    private void Update()
    {
        //arStatus.text = Session.Status.ToString();
        /*if(!_uploaded)
        {
            var latitude = Input.location.lastData.latitude;
            var longitude = Input.location.lastData.longitude;
            arStatus.text = latitude.ToString() + " " + longitude;
        }*/

        if (_camAvailable)
            return;

        float ratio = (float)_webCamTexture.height / (float)_webCamTexture.width;
        fit.aspectRatio = ratio;

        float scaleY = _webCamTexture.videoVerticallyMirrored ? -1f : 1f;
        SnapshotImage.rectTransform.localScale = new Vector3(1, scaleY, -1f);

        int orient = -_webCamTexture.videoRotationAngle;
        SnapshotImage.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }

    public void StartCamera()
    {
        LifecycleManager.Instance.DisableSession();

        _webCamTexture.Play();
        SnapshotImage.texture = _webCamTexture;

        SnapshotImage.gameObject.SetActive(true);
        EnableSnapshotCam.SetActive(false);
        TakeSnapshot.SetActive(true);
        CancelCam.SetActive(true);
    }

    public void TakeSnapshotClick()
    {
        StartCoroutine(TakeScreenshot());
    }

    public void StopCamera()
    {
        _webCamTexture.Stop();

        SnapshotImage.gameObject.SetActive(false);
        EnableSnapshotCam.SetActive(true);
        TakeSnapshot.SetActive(false);
        CancelCam.SetActive(false);
        Upload.SetActive(false);

        ResetCamera();
    }

    IEnumerator TakeScreenshot()
    {
        yield return new WaitForEndOfFrame();
        _photo = new Texture2D(_webCamTexture.width, _webCamTexture.height);
        _photo.SetPixels(_webCamTexture.GetPixels());
        _photo.Apply();

        _webCamTexture.Stop();

        yield return new WaitForEndOfFrame();

        SnapshotImage.texture = _photo;

        Upload.SetActive(true);
    }

    public void UploadSnapshot()
    {
        var latitude = GetLatitude();
        var longitude = GetLongitude();
       UploadController.Upload(_photo, latitude, longitude);

        SnapshotImage.gameObject.SetActive(false);
        EnableSnapshotCam.SetActive(true);
        TakeSnapshot.SetActive(false);
        CancelCam.SetActive(false);
        Upload.SetActive(false);

        ResetCamera();
    }

    float GetLatitude()
    {
        var latitude = Input.location.lastData.latitude;
        //latitude = latitude + Random.Range(0, 3.7f);

        return latitude;
    }

    float GetLongitude()
    {
        var longitude = Input.location.lastData.longitude;
        //longitude = longitude + Random.Range(0, 3.7f);

        return longitude;
    }

    private void ResetCamera()
    {
        LifecycleManager.Instance.EnableSession();
    }
}
