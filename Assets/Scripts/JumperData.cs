using UnityEngine;
using System.Collections;

public class JumperData : MonoBehaviour {

	private bool m_active;
	private bool m_engaged;
	[SerializeField] private int m_id;
	private Vector2 m_jumpVec;
	private Animator m_animator;

	// Use this for initialization
	void Start () {
		m_active = false;
		m_engaged = false;
		m_jumpVec = Vector2.zero;
		m_animator = GetComponent<Animator>();
	}

	public void reInitialize()
	{
		m_active = false;
		m_engaged = false;
		m_jumpVec = Vector2.zero;
	}

	// Update is called once per frame
	void Update () {
	
	}

	public Vector2 JumpVec
	{
		get{return m_jumpVec;}
		set{m_jumpVec = value;}
	}

	public bool Active
	{
		get{return m_active;}
		set{m_active = value;}
	}

	public bool Engaged
	{
		get{return m_engaged;}
		set{m_engaged = value;
			m_animator.SetTrigger("Active");}
	}

	public int ID
	{
		get{return m_id;}
		set{m_id = value;}
	}
}