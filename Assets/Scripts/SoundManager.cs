using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour 
{
	[SerializeField] public AudioClip m_jumperEngage;
	[SerializeField] public AudioClip m_jumperPerfectEngage;
	[SerializeField] public AudioClip m_jumperJump;
	[SerializeField] public AudioClip m_gameOver;

	private static SoundManager m_instance;
	
	public static SoundManager Instance
	{
		get
		{
			if(m_instance == null)
			{
				m_instance = GameObject.FindObjectOfType<SoundManager>();
			}
			
			return m_instance;
		}
	}
	
	void Awake() 
	{
		if(m_instance == null)
		{
			//If I am the first instance, make me the Singleton
			m_instance = this;
		}
		else
		{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if(this != m_instance)
				Destroy(this.gameObject);
		}
	}

	public void playEngage()
	{
		GetComponent<AudioSource>().PlayOneShot(m_jumperEngage);
	}

	public void playPerfectEngage()
	{
		GetComponent<AudioSource>().PlayOneShot(m_jumperPerfectEngage);
	}

	public void playJump()
	{
		GetComponent<AudioSource>().PlayOneShot(m_jumperJump);
	}

	public void playGameOver()
	{
		GetComponent<AudioSource>().PlayOneShot(m_gameOver);
	}
}