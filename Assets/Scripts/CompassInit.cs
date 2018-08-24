using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class CompassInit : MonoBehaviour {

		// Use this for initialization
		void Start ()
		{
#if UNITY_EDITOR
		return ;
#endif 
			while (!Input.compass.enabled);
			StartCoroutine("AllignCompass");
		}

		IEnumerator AllignCompass()
		{
			yield return new WaitForSeconds(0);
			int current = 0;
			float average = 0;
			while (current < 10)
			{
				average += Input.compass.trueHeading; ;
				current++;
			}
			average /= 10;
			gameObject.transform.rotation = Quaternion.Euler(0, average, 0);
		}
	}
}
