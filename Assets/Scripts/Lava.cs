using UnityEngine;
using System.Collections;

public class Lava : MonoBehaviour {

	private PlayerDeath m_playerDeath;
	private float m_killerHeight;
	private Transform m_mainCameraTransform;
	// Use this for initialization
	void Start ()
	{
		m_mainCameraTransform = Camera.main.transform;
		WorldLimits worldLimits = Camera.main.GetComponent<WorldLimits>();
		transform.localScale = new Vector3(worldLimits.m_worldSpaceWidth, worldLimits.m_worldSpaceHeight, 1f);
		m_playerDeath = GameObject.Find("Player").GetComponent<PlayerDeath>();
	}
	
	// Update is called once per frame
	void Update () {
		m_killerHeight = m_playerDeath.KillerHeight;
		transform.position = new Vector3(m_mainCameraTransform.position.x, m_killerHeight, 1f);
	}
}