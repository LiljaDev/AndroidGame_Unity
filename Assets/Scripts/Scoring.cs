using UnityEngine;
using System.Collections;

public class Scoring : MonoBehaviour {

	private int m_currentScore;
	private GUIIngame m_guiIngame;
	private int m_lastId;
	private MeshRenderer m_liveScoreRenderer;
	private TextMesh m_liveScoreMesh;
	private Color m_scoreColor = Color.black;
	// Use this for initialization
	void Start () 
	{
		GameObject textLiveScore = GameObject.Find("TextLiveScore");
		m_liveScoreRenderer = textLiveScore.GetComponent<MeshRenderer>();
		m_liveScoreMesh = textLiveScore.GetComponent<TextMesh>();
		m_currentScore = 0;
		m_lastId = 0;
		m_guiIngame = GameObject.Find("GUI").GetComponent<GUIIngame>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void gameOver()
	{
		const string highscoreKey = "highscore", attemptsKey = "attempts";

		if (PlayerPrefs.HasKey(highscoreKey))
		{
			if(m_currentScore > PlayerPrefs.GetInt(highscoreKey))
				PlayerPrefs.SetInt(highscoreKey, m_currentScore);
		}
		else
			PlayerPrefs.SetInt(highscoreKey, m_currentScore);

		if (PlayerPrefs.HasKey(attemptsKey))
			PlayerPrefs.SetInt(attemptsKey, PlayerPrefs.GetInt(attemptsKey) + 1);
		else
			PlayerPrefs.SetInt(attemptsKey, 1);

		PlayerPrefs.Save();

		m_guiIngame.Highscore = PlayerPrefs.GetInt(highscoreKey);
		m_guiIngame.Attempts = PlayerPrefs.GetInt(attemptsKey);
	}

	public void incrementScoreById(int id, bool perfect, Vector2 pos)
	{
		int pointsScored;
		if(perfect)
			pointsScored = (id - m_lastId) * 2;
		else
			pointsScored = (id - m_lastId);

		m_currentScore += pointsScored;
		m_lastId = id;
		m_guiIngame.Score = m_currentScore;
		showLiveScore(pointsScored, pos);
	}

	private void showLiveScore(int scored, Vector2 pos)
	{
		m_liveScoreMesh.gameObject.transform.position = pos;
		m_liveScoreMesh.text = "+" + scored;
		m_liveScoreRenderer.enabled = true;
		m_liveScoreMesh.color = m_scoreColor;
		StartCoroutine("Fade");
	}

	IEnumerator Fade()
	{
		Color color = m_liveScoreMesh.color;

		while(color.a > 0.0f)
		{
			color.a -= 0.075f;
			m_liveScoreMesh.color = color;
			yield return new WaitForSeconds(0.1f);
		}
	}

	public int Score
	{
		get{return m_currentScore;}
	}
}