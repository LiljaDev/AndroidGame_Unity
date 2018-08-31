using UnityEngine;
using System.Collections;

public class EndlesslyMoving : MonoBehaviour {

	private float m_moveHeight;
	// Use this for initialization
	void Start () {
		m_moveHeight = 40.96f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnBecameInvisible() {
		Vector3 newPos = gameObject.transform.position;
		newPos.y += m_moveHeight;
		gameObject.transform.position = newPos;
		Debug.Log("I AM INVISIBLE");
	}
}
