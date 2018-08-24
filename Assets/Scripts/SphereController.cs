using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class SphereController : MonoBehaviour
	{
		[SerializeField] private GameObject sphere;
		[SerializeField] private Toggle mainCam;
		// Use this for initialization
		void Start () {
		
		}
	
		// Update is called once per frame
		void Update () {
		
		}

		public void EnableSphere()
		{
			sphere.SetActive(!mainCam.isOn);
		}
	}
}
