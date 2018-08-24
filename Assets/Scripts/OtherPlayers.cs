using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class OtherPlayers : MonoBehaviour {
		private AbstractMap map;
		public Transform[] players;
		public string url = "http://divit.hr/gps/";
		public string key = "cAoE2X5mPUCFC4V0547SKU!bRf*&D74r";
		string deviceid;
		public Vector2d[] latLon;
		private List<string> markerIds = new List<string>();
		List<Vector2d> markerList = new List<Vector2d>();
		private List<GameObject> spawnedMarkers = new List<GameObject>();
		[SerializeField] private GameObject[] _markerPrefabs;

		private void Start()
		{
			if (PlayerPrefs.HasKey("deviceid")) deviceid = PlayerPrefs.GetString("deviceid");
			else
			{
				deviceid = System.DateTime.Now.Ticks.ToString();
				deviceid = deviceid.Substring(deviceid.Length - 15, 11);
				deviceid = Random.Range(10000000, 99999999).ToString() + deviceid;
				PlayerPrefs.SetString("deviceid", deviceid);
				PlayerPrefs.Save();
			}
		}
		private void OnEnable()
		{
			map = GetComponentInChildren<AbstractMap>();
			StartCoroutine(Loop());
		}

		IEnumerator Loop()
		{
			while (true)
			{
				yield return new WaitForSeconds(1f);

				WWWForm form = new WWWForm();
				form.AddField("hash", key);
				form.AddField("id", deviceid);
				form.AddField("lat", Input.location.lastData.latitude.ToString());
				form.AddField("lon", Input.location.lastData.longitude.ToString());

				UnityWebRequest www = UnityWebRequest.Post(url, form);
				yield return www.SendWebRequest();
				if (string.IsNullOrEmpty(www.error))
				{
					string[] coords = www.downloadHandler.text.Split('\n');
					var list = new List<Vector2d>();
					bool poi = false;
					foreach(string coord in coords)
						Debug.Log(coord);
					foreach (var c in coords)
					{
						if (poi)
						{
							if (c == null || c.Length < 4) continue;
							string[] data = c.Split(',');
							// data[0] = id
							// data [1] = url
							// data[2,3] = lat, lon
							string id = data[0];
							string imgUrl = data[1];
							if (markerIds.Contains(id)) continue;
							//Debug.Log(data[2] + " " + data[3] + " " + markerList[index]);
							StartCoroutine(SpawnMarker(imgUrl, id, new Vector2d(double.Parse(data[2]), double.Parse(data[3]))));
						}
						else
						{
							if (c != null && c.StartsWith("poi"))
							{
								poi = true;
								continue;
							} 
							if (c == null || c.Length < 4) continue;
							string[] data = c.Split(',');
							list.Add(new global::Mapbox.Utils.Vector2d(double.Parse(data[0]), double.Parse(data[1])));
						}
					}
					latLon = list.ToArray();
				}

				int i = 0;
				for (; i < latLon.Length && i < players.Length; ++i)
				{
					players[i].gameObject.SetActive(true);
					players[i].localPosition = map.GeoToWorldPosition(latLon[i], true);
				}
				for (; i < players.Length; ++i)
				{
					players[i].gameObject.SetActive(false);
				}
				
			}
		}

		IEnumerator SpawnMarker(string imgUrl, string id, Vector2d latLon)
		{
			GameObject instance = null;
			WWW www = new WWW(imgUrl);
			//Debug.Log(imgUrl);
			yield return www;
            //int i = Random.Range(0, _markerPrefabs.Length);
            //instance = Instantiate(_markerPrefabs[i]);
            instance = Instantiate(_markerPrefabs[0]);
            instance.transform.localPosition = map.GeoToWorldPosition(latLon, true);
			instance.transform.localScale = new Vector3(1,1,1);
			instance.transform.LookAt(Vector3.zero);
			//Debug.Log(www.texture.width + " " + www.texture.height);
			Canvas canvas = instance.GetComponentInChildren<Canvas>();
			canvas.gameObject.SetActive(true);
			
			RawImage[] images = instance.GetComponentsInChildren<RawImage>();
			foreach (RawImage image in images)
			{
				image.texture = www.texture;
			}
			AspectRatioFitter[] aspects = instance.GetComponentsInChildren<AspectRatioFitter>();
			foreach (AspectRatioFitter aspect in aspects)
			{
				aspect.aspectRatio = (float)www.texture.width / www.texture.height;
			}
			//var scale = (float) www.texture.width/www.texture.height;
			//Debug.Log(scale);
			Canvas[] canvases = instance.GetComponentsInChildren<Canvas>();
			if (canvases.Length == 1) canvas.gameObject.SetActive(false);
			markerIds.Add(id);
			markerList.Add(latLon);
			spawnedMarkers.Add(instance);
		}

		void Update()
		{
			int count = spawnedMarkers.Count;
			for (int j = 0; j < count; j++)
			{
				var zoom = map.Zoom;
				var initZoom = map.InitialZoom == 0 ? 16 : map.InitialZoom;
				var spawnScale = zoom / initZoom;
				var spawnedObject = spawnedMarkers[j];
				var location = markerList[j];
				spawnedObject.transform.localPosition = map.GeoToWorldPosition(location, true);
				spawnedObject.transform.localScale = new Vector3(spawnScale, spawnScale, spawnScale);
			}
		}

	}
}
