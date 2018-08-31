using UnityEngine;
using System.Collections;

public class ChallengePlayerDeath : MonoBehaviour {

	private float m_killerHeight;
//	private GUIIngame m_gui;
//	private Scoring m_scoring;
//	private SpawnJumperPlatforms m_spawnJumperPlatforms;
//	private LinkedList<GameObject> m_platforms;
	
	// Use this for initialization
	void Start () 
	{
//		m_gui = GameObject.Find("GUI").GetComponent<GUIIngame>();
//		m_scoring = GameObject.Find("InGameData").GetComponent<Scoring>();
		m_killerHeight = -2;
		//m_spawnJumperPlatforms = GameObject.Find("Spawner").GetComponent<SpawnJumperPlatforms>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(transform.position.y <= m_killerHeight)
			onDeath();
	}
	
//	public void setKillerHeight(int jumperId)
//	{
//		m_platforms = m_spawnJumperPlatforms.Platforms;
//		LinkedListNode<GameObject> currentPlat = m_platforms.First;
//		
//		while(currentPlat != null)
//		{
//			if(currentPlat.Value.GetComponentInChildren<JumperData>().ID == jumperId)
//			{
//				m_killerHeight = currentPlat.Value.transform.position.y;
//				break;
//			}
//			
//			currentPlat = currentPlat.Next;
//		}
//	}
	
	public float KillerHeight
	{
		get{return m_killerHeight;}
//		set{m_killerHeight = value;}
	}
	
	private void onDeath()
	{
		SoundManager.Instance.playGameOver();
//		GameObject.Find("Spawner").SetActive(false);
		//m_scoring.gameOver();
//		m_gui.gameOver();
		Destroy(gameObject);
	}
}
