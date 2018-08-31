using UnityEngine;
using System.Collections;

public class ChallengeScoring : MonoBehaviour 
{
	private int m_stars;
	private ChallengeInGameGUI m_gui;

	void Awake()
	{
		m_stars = 0;
	}

	// Use this for initialization
	void Start () 
	{
		m_gui = GameObject.Find("ChallengeInGameGUI").GetComponent<ChallengeInGameGUI>();
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	public int Stars
	{
		get{return m_stars;}
	}

	public void starPickup()
	{
		++m_stars;
		m_gui.updateStars(m_stars);
	}

	public void goalReached()
	{
		string bestRatingKey = Application.loadedLevelName;
		int bestRating = 0;

		if (PlayerPrefs.HasKey(bestRatingKey))
		{
			bestRating = PlayerPrefs.GetInt(bestRatingKey);

			if(m_stars > bestRating)
			{
				PlayerPrefs.SetInt(bestRatingKey, m_stars);
				bestRating = m_stars;
			}
		}
		else
		{
			PlayerPrefs.SetInt(bestRatingKey, m_stars);
			bestRating = m_stars;
		}

		PlayerPrefs.Save();

		m_gui.goalReached(m_stars, bestRating);
	}
}
