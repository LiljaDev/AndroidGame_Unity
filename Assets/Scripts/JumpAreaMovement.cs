using UnityEngine;
using System.Collections;

public class JumpAreaMovement : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		Color tempColor = GetComponent<Renderer>().material.color;
		tempColor.a = 0f;
		GetComponent<Renderer>().material.color = tempColor;
	}
	
	// Update is called once per frame
	void Update (){
	}

	public void activate(Vector2 pos)
	{
		StopCoroutine("Fade");

		transform.position = pos;

		Color tempColor = GetComponent<Renderer>().material.color;
		tempColor.a = 0.5f;
		GetComponent<Renderer>().material.color = tempColor;
	}

	public void deactivate()
	{
//		Color tempColor = renderer.material.color;
//		tempColor.a = 0f;
//		renderer.material.color = tempColor;
		StartCoroutine("Fade");
	}

	IEnumerator Fade()
	{
		Color color = GetComponent<Renderer>().material.color;
		while(color.a > 0)
		{
			color.a -= 0.05f;
			GetComponent<Renderer>().material.color = color;
			yield return new WaitForSeconds(0.025f);
		}
	}
}