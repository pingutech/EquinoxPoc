using System.Collections.Generic;
using Mapbox.Examples;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
	public class ObjectInitialization : MonoBehaviour
	{
		private AbstractMap _map;
		private LocationService _location;
		private float _latitude;
		private float _longitude;
		private float _x;
		private float _y;
		private Vector2d _latLon;
		private Vector2d[] _latLons;
		[SerializeField] private float _spawnScale = 100f;
		[SerializeField] GameObject _markerPrefab;
		private List<GameObject> _spawnedObjects;
		private bool _initialized = false;

		// Use this for initialization
		void Start ()
		{
			_map = GetComponent<AbstractMap>();
#if UNITY_EDITOR
			_latitude = 45.798776F;
			_longitude = 15.989680F;
#else
			_location = Input.location;
			_latitude = _location.lastData.latitude;
			_longitude = _location.lastData.longitude;
#endif
			_spawnedObjects = new List<GameObject>();
			_latLons = new Vector2d[2];
            Vector2[] offsets = new Vector2[2];
            float[] angles = new float[2];
            offsets[0] = Quaternion.Euler(0, 0, angles[0] = Random.Range(0, 360f)) * new Vector2(0.000635243f, 0f);
            offsets[1] = Quaternion.Euler(0, 0, angles[1] = Random.Range(0, 360f)) * new Vector2(0.000381146f, 0f);

            for (int i = 0; i < 2; i++)
			{
				_latLon.x = _latitude + offsets[i].x;
				_latLon.y = _longitude + offsets[i].y;
				_latLons[i] = _latLon;
				GameObject instance = null;
				instance = Instantiate(_markerPrefab);
				instance.transform.localPosition = _map.GeoToWorldPosition(_latLons[i], true);
				instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                instance.transform.LookAt(Vector3.zero);
                _spawnedObjects.Add(instance);

			}

			StartCoroutine("LookAtZero");
		}

		// Update is called once per frame
		void Update ()
		{
			int count = _spawnedObjects.Count;
			for (int i = 0; i < count; i++)
			{
				var zoom = _map.Zoom;
				var initZoom = _map.InitialZoom == 0 ? 16 : _map.InitialZoom;
				_spawnScale = zoom/initZoom;
				var spawnedObject = _spawnedObjects[i];
				var location = _latLons[i];
				spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
				spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                if (!_initialized) spawnedObject.transform.LookAt(Vector3.zero);
            }
        }

		IEnumerator LookAtZero()
		{
			yield return new WaitForSeconds(1);
			_initialized = true;
		} 
	}
}
