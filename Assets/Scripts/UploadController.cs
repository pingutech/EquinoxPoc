using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class UploadController : MonoBehaviour {
	public string uploadUrl = "http://divit.hr/gps/upload.php";
	public int jpegQuality = 75;
    public string uploadKey = "cAoE2X5mPUCFC4V0547SKU!bRf*&D74r";
    public string uploadHeader = "file_contents";
	
	public void Upload(Texture2D image, float lat, float lon)
	{

        Debug.Log("Upload2");
        string deviceid = PlayerPrefs.GetString("deviceid");
		string id = System.DateTime.Now.Ticks.ToString();
		id = id.Substring(id.Length - 15, 11);
		id = Random.Range(10000, 99999).ToString() + id;
		StartCoroutine(UploadPhoto(image, lat, lon, deviceid + id));
    }

    public IEnumerator UploadPhoto(Texture2D image, float lat, float lon, string id)
    {
        Debug.Log("Upload coroutine");
        yield return 0;

		byte[] bytes = image.EncodeToJPG(jpegQuality);

        WWWForm form = new WWWForm();
        form.AddBinaryData(uploadHeader, bytes, id+".jpg", "image/jpeg");
        if (uploadKey.Length > 1)
			form.AddField("hash", uploadKey);
		form.AddField("id", id);
		form.AddField("lat", lat.ToString());
		form.AddField("lon", lon.ToString());

		UnityWebRequest www = UnityWebRequest.Post(uploadUrl, form);
		yield return www.SendWebRequest();
    }
}
