using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawKillerLevel : MonoBehaviour {

	private PlayerDeath m_playerDeath;
	private float m_killerHeight;
	[SerializeField] private Material m_lineMat;

	// Use this for initialization
	void Start ()
	{
		m_playerDeath = GameObject.Find("Player").GetComponent<PlayerDeath>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		m_killerHeight = m_playerDeath.KillerHeight;
	}

	void OnPostRender()
	{
		GL.PushMatrix();
		m_lineMat.SetPass(0);
		GL.LoadOrtho();
		GL.Begin(GL.LINES);
		GL.Color(Color.red);

		float screenY = Camera.main.WorldToScreenPoint(new Vector2(0, m_killerHeight)).y;

		GL.Vertex(new Vector2(0, screenY / Screen.height));
		GL.Vertex(new Vector2(1, screenY / Screen.height));

		GL.End();
		GL.PopMatrix();
	}
}