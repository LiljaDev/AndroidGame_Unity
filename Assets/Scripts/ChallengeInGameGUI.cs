using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChallengeInGameGUI : MonoBehaviour {

	[SerializeField] Sprite m_stars1;
	[SerializeField] Sprite m_stars2;
	[SerializeField] Sprite m_stars3;
	[SerializeField] Image m_stars;
	[SerializeField] private Canvas m_victory;
	[SerializeField] private Image m_victoryRating;
	[SerializeField] private Image m_victoryBestRating;
	// Use this for initialization
	void Start ()
	{
		m_victory.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void clickRestart()
	{
		Application.LoadLevel(Application.loadedLevel);
	}

	public void clickBack()
	{
		Application.LoadLevel("ChallengeMenu");
	}

	public void updateStars(int stars)
	{
		switch(stars)
		{
		case 1:
			m_stars.sprite = m_stars1;
			break;
		case 2:
			m_stars.sprite = m_stars2;
			break;
		case 3:
			m_stars.sprite = m_stars3;
			break;
		}
	}

	public void goalReached(int rating, int bestRating)
	{
		m_victory.enabled = true;

		switch(rating)
		{
		case 1:
			m_victoryRating.sprite = m_stars1;
			break;
		case 2:
			m_victoryRating.sprite = m_stars2;
			break;
		case 3:
			m_victoryRating.sprite = m_stars3;
			break;
		}

		switch(bestRating)
		{
		case 1:
			m_victoryBestRating.sprite = m_stars1;
			break;
		case 2:
			m_victoryBestRating.sprite = m_stars2;
			break;
		case 3:
			m_victoryBestRating.sprite = m_stars3;
			break;
		}
	}
}
