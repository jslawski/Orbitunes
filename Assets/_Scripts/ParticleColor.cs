using UnityEngine;
using System.Collections;

public class ParticleColor : MonoBehaviour {
	float rate = 15f;
	float timer;
	Color rand_col;

	[SerializeField]
	private ParticleSystem particles;

	// Use this for initialization
	void Start () {
		timer = 0;
		rand_col = new Color(0,0,0,1);
		rand_col.a = 1;
		rand_col.r = Random.Range(0,255)/255f;
		rand_col.g = Random.Range(0,255)/255f;
		rand_col.b = Random.Range(0,255)/255f;
	}
	
	// Update is called once per frame
	void Update () {
		if (timer < 1){
			timer += Time.deltaTime * rate;
			float i = timer*timer;
			ParticleSystem.MainModule settings = particles.main;
			settings.startColor = Color.Lerp(settings.startColor.color, rand_col, i);
		}
		//get a new random color to lerp to
		else {
			timer = 0;
			rand_col.r = Random.Range(0,255)/255f;
			rand_col.g = Random.Range(0,255)/255f;
			rand_col.b = Random.Range(0,255)/255f;
			
			//if the color is too dark, brighten it to a random color
			if (rand_col.r < 100f/255f && rand_col.g < 100f/255f && rand_col.b < 100f/255f){
				int i = Random.Range(0,3);
				if (i == 0)
					rand_col.r += 100/255f;
				else if (i == 1)
					rand_col.g += 100/255f;
				else
					rand_col.b += 100/255f;
			}
		}
		
	}
	
}
