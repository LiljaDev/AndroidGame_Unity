using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIIngame : MonoBehaviour {
	
	private Text m_ingameScore;
	private Text m_gameOverScore;
	private Text m_gameOverHighscore;
	private Text m_gameOverAttempts;
	private Canvas m_canvasGameOver;

	// Use this for initialization
	void Start () 
	{
		m_ingameScore = GameObject.Find("TextInGameScore").GetComponent<Text>();
		m_gameOverScore = GameObject.Find("TextGameOverScore").GetComponent<Text>();
		m_gameOverHighscore = GameObject.Find("TextGameOverHighscore").GetComponent<Text>();
		m_gameOverAttempts = GameObject.Find("TextGameOverAttempts").GetComponent<Text>();

		//Locate the game over canvas..
		Canvas[] canvases = GetComponentsInChildren<Canvas>();
		if(canvases[0].name == "CanvasGameOver")
			m_canvasGameOver = canvases[0];
		else
			m_canvasGameOver = canvases[1];
		
		m_canvasGameOver.enabled = false;
	}
	
	// Update is called once per frame
	void Update (){
	}

	public int Score
	{
		set{m_ingameScore.text = "Score: " + value;}
	}

	public int Highscore
	{
		set{m_gameOverHighscore.text ="Highscore: " + value;}
	}

	public int Attempts
	{
		set{m_gameOverAttempts.text ="Attempts: " + value;}
	}

	public void gameOver()
	{
		m_gameOverScore.text = m_ingameScore.text;
		m_canvasGameOver.enabled = true;
	}

	public void clickRestart()
	{
		Application.LoadLevel(Application.loadedLevel);
	}

	public void clickBack()
	{
		Application.LoadLevel(0);
	}
}