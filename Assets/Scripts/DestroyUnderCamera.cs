using UnityEngine;
using System.Collections;

public class DestroyUnderCamera : MonoBehaviour {

	private float m_worldHeight;
	private Transform m_cameraTransform;
	// Use this for initialization
	void Start () {
		m_cameraTransform = Camera.main.transform;	//Handle for camera transform
		WorldLimits worldLimits = Camera.main.GetComponent<WorldLimits>();
		m_worldHeight = worldLimits.WorldSpaceHeight;	//World space height of whats visible
	}
	
	// Update is called once per frame
	void Update () {
		//Destroy gameobject if it's a bit below what's visible
		if(transform.position.y < m_cameraTransform.position.y - m_worldHeight)
			Destroy(gameObject);
	}
}
