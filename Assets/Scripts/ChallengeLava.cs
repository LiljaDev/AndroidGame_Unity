using UnityEngine;
using System.Collections;

public class ChallengeLava : MonoBehaviour {

	private ChallengePlayerDeath m_playerDeath;
	private float m_killerHeight;
	private Transform m_mainCameraTransform;
	// Use this for initialization
	void Start ()
	{
		m_mainCameraTransform = Camera.main.transform;
		WorldLimits worldLimits = Camera.main.GetComponent<WorldLimits>();
		transform.localScale = new Vector3(worldLimits.m_worldSpaceWidth, worldLimits.m_worldSpaceHeight, 1f);
		m_playerDeath = GameObject.Find("ChallengePlayer").GetComponent<ChallengePlayerDeath>();

	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(m_mainCameraTransform.position.x, m_playerDeath.KillerHeight, 1f);
	}
}
