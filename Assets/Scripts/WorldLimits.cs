using UnityEngine;
using System.Collections;

public class WorldLimits : MonoBehaviour 
{
	public float m_worldSpaceWidth;
	public float m_worldSpaceHeight;

	void Awake()
	{
		//Width and height of world that is visible to camera
		m_worldSpaceWidth = (Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, 0f)).x - Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x);
		m_worldSpaceHeight = (Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f, 0f)).y - Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).y);
	}
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public float WorldSpaceWidth
	{
		get{return m_worldSpaceWidth;}
	}

	public float WorldSpaceHeight
	{
		get{return m_worldSpaceHeight;}
	}
}
