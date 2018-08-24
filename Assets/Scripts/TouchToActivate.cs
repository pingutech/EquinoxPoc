using UnityEngine;

namespace Assets.Scripts
{
	public class TouchToActivate : MonoBehaviour {
		Camera cam;
		// Use this for initialization
		void Start () {
			cam = GetComponent<Camera>();
		}
	
		// Update is called once per frame
		void Update () {
			for(var i = 0; i < Input.touchCount; ++i) {
				if (Input.GetTouch(i).phase == TouchPhase.Began)
				{

					// Construct a ray from the current touch coordinates
					Ray ray = cam.ScreenPointToRay(Input.GetTouch(i).position);
					// Create a particle if hit
					RaycastHit hit;
					if (Physics.Raycast(ray, out hit)) if (hit.transform != null)
					{
						Touchable t = hit.transform.GetComponent<Touchable>();
						if (t != null) t.Touch();
					}
					//Instantiate(particle, transform.position, transform.rotation);
				}
			}
		}
	}
}
