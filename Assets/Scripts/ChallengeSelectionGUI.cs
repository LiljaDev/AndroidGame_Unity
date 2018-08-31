using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChallengeSelectionGUI : MonoBehaviour {

	[SerializeField] Sprite m_stars1;
	[SerializeField] Sprite m_stars2;
	[SerializeField] Sprite m_stars3;
	[SerializeField] Image[] m_challengeRatings;
	// Use this for initialization
	void Start () 
	{
		for(int i = 0; i < m_challengeRatings.Length; ++i)
		{
			string currentKey = "C" + (i + 1);

			if (PlayerPrefs.HasKey(currentKey))
			{
				int rating = PlayerPrefs.GetInt(currentKey);

				switch(rating)
				{
				case 1:
					m_challengeRatings[i].sprite = m_stars1;
					break;
				case 2:
					m_challengeRatings[i].sprite = m_stars2;
					break;
				case 3:
					m_challengeRatings[i].sprite = m_stars3;
					break;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	public void clickChallenge(int level)
	{
		Application.LoadLevel("C" + level);
	}

	public void clickBack()
	{
		Application.LoadLevel("MainMenu");
	}
}
