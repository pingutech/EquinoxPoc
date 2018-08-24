using UnityEngine;
using UnityEngine.Experimental.UIElements;
using Image = UnityEngine.UI.Image;

namespace Assets.Scripts
{
	public class Touchable : MonoBehaviour {
		public GameObject toggle;
		public Sprite[] textures;
		private Sprite slika;

		void Start()
		{
			int count = textures.Length;
			int i = Random.Range(0, count);
			slika = textures[i];
			Image[] image = GetComponentsInChildren<Image>();
			//Debug.Log(count + " " + slika + " " + image);
			foreach (Image img in image)
			{
				img.sprite = slika;
			}
			if (image.Length == 1) gameObject.GetComponentInChildren<Canvas>().gameObject.SetActive(false);
		}

		public void Touch () {
			if (toggle != null) toggle.SetActive(!toggle.activeSelf);
		}
	
	}
}
