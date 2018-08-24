using UnityEngine;

namespace Assets.Scripts
{
	public class CameraRotation : MonoBehaviour
	{
		private float _time;
		private float _lastHeading;
		// Use this for initialization
		void Start ()
		{
			_time = Time.time;
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (Mathf.Abs(Input.compass.trueHeading - _lastHeading) <= 5 || Mathf.Abs(Input.compass.trueHeading - _lastHeading) >= 355) return;
			if (_time + 0.4 > Time.time) return;
			_time = Time.time;
			_lastHeading = Input.compass.trueHeading;
			gameObject.transform.rotation = Quaternion.Euler(90, Input.compass.trueHeading, 0);
		}
	}
}
