using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpEp_UVScroller : MonoBehaviour {
	
	
	public float speedY=0.5f;
	public float speedX=0.0f;
	public float timeWentX=0f;
	public float timeWentY=0f;
	public Material thisMat;

	void Start () {

		

	}

	void Update () {
		timeWentY += Time.deltaTime*speedY;
		timeWentX += Time.deltaTime*speedX;


		thisMat.SetTextureOffset ("_MainTex", new Vector2(timeWentX, timeWentY));
		//thisMat.SetTextureOffset ("_BumpMap", new Vector2(timeWentX, timeWentY));



	}
}
