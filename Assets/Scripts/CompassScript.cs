using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class CompassScript : MonoBehaviour {

		private double _timestampt;

		// Use this for initialization
		void Start ()
		{
			_timestampt = Input.compass.timestamp;
			GetComponent<Text>().text = "\ntimestamp: " + Input.compass.timestamp
			                            + "\nenabled: " + Input.compass.enabled;
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (Mathf.Approximately((float) _timestampt, (float) Input.compass.timestamp)) return;
			GetComponent<Text>().text = "\ntimestamp: " + Input.compass.timestamp 
			                            + "\nenabled: " + Input.compass.enabled 
			                            + "\ntrue heading: " + Input.compass.trueHeading 
			                            + "\nmagnetic heading: " + Input.compass.magneticHeading 
			                            + "\nheading accuracy: " + Input.compass.headingAccuracy;
			_timestampt = Input.compass.timestamp;
		}
	}
}
