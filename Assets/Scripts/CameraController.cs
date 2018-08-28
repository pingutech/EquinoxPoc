using System;
using System.Collections;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField]
		[Range(1, 20)]
		public float _panSpeed = 1.0f;

		[SerializeField]
		float _zoomSpeed = 0.25f;

		[SerializeField]
		public Camera _referenceCamera;

		[SerializeField]
		AbstractMap _mapManager;

		[SerializeField]
		bool _useDegreeMethod;

		private Vector3 _origin;
		private Vector3 _mousePosition;
		private Vector3 _mousePositionPrevious;
		private bool _shouldDrag;
		private bool _isInitialized = false;
		private Plane _groundPlane = new Plane(Vector3.up, 0);

		[SerializeField]
		private Toggle _mainCameraToggle;
		private float _time = 0;
		private bool _changed = false;
		private float _previousZoom = 16;

		void Awake()
		{
			if (null == _referenceCamera)
			{
				_referenceCamera = GetComponent<Camera>();
				if (null == _referenceCamera) { Debug.LogErrorFormat("{0}: reference camera not set", this.GetType().Name); }
			}
			_mapManager.OnInitialized += () =>
			{
				_isInitialized = true;
			};
			enabled = false;
		}


		private void LateUpdate()
		{
			if (!_isInitialized) { return; }

			if (Input.touchSupported && Input.touchCount > 0)
			{
				HandleTouch();
			}
			else
			{
				HandleMouseAndKeyBoard();
			}

			if (_changed && _time + 5 < Time.time)
			{
				ResetMap();
			}
		}

		public void ResetMap()
		{
            return;

			//Debug.Log("reseting map");
			_changed = false;
            //reseting zoom
            //_previousZoom = _mapManager.InitialZoom;

            StartCoroutine(ZoomReset());
		}

        IEnumerator ZoomReset()
        {
            _mapManager.gameObject.transform.localScale = new Vector3(1, 1, 1);
            yield return new WaitForSeconds(1f);

            _mapManager.UpdateMap(_mapManager.CenterLatitudeLongitude, _previousZoom);

            Debug.Log("resetirao sam");
        }

		public void Enable()
		{
			if (_mainCameraToggle.isOn) enabled = false;
			if (!_mainCameraToggle.isOn) enabled = true;
		}

		void HandleMouseAndKeyBoard()
		{
			// zoom
			float scrollDelta = 0.0f;
			scrollDelta = Input.GetAxis("Mouse ScrollWheel");
			ZoomMapUsingTouchOrMouse(scrollDelta);
		}

		void HandleTouch()
		{
			float zoomFactor = 0.0f;
			//pinch to zoom. 
			switch (Input.touchCount)
			{
				case 2:
					{
						// Store both touches.
						Touch touchZero = Input.GetTouch(0);
						Touch touchOne = Input.GetTouch(1);

						// Find the position in the previous frame of each touch.
						Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
						Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

						// Find the magnitude of the vector (the distance) between the touches in each frame.
						float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
						float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

						// Find the difference in the distances between each frame.
						zoomFactor = 0.01f * (touchDeltaMag - prevTouchDeltaMag);
					}
					ZoomMapUsingTouchOrMouse(zoomFactor);
					break;
				default:
					break;
			}
		}

		void ZoomMapUsingTouchOrMouse(float zoomFactor)
		{
			var zoom = Mathf.Max(0.0f, Mathf.Min(_mapManager.Zoom + zoomFactor * _zoomSpeed, 21.0f));

			if (zoom != _previousZoom)
			{
				_previousZoom = zoom;
				_changed = true;
				_time = Time.time;
			}

			_mapManager.UpdateMap(_mapManager.CenterLatitudeLongitude, zoom);
		}	

		private Vector3 getGroundPlaneHitPoint(Ray ray)
		{
			float distance;
			if (!_groundPlane.Raycast(ray, out distance)) { return Vector3.zero; }
			return ray.GetPoint(distance);
		}
	}
}