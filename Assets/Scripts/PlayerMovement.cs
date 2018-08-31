using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	private int m_acc;
	private float m_jumpPower;
	private int m_maxSpeed;
	private bool m_frozen;
	//private float m_jumpDelay;
	private Vector2 m_accVec;
	private bool m_facingRight;
	[SerializeField] private bool m_grounded;
	private Vector2 m_groundCheckPos0;
	private Vector2 m_groundCheckPos1;
	private Scoring m_scoring;
	private Animator m_animator;
	private GameObject m_currentModuleEngaged;
    private InputTouch m_inputTouch;
    private WindController m_windController;

	// Use this for initialization
	void Start () 
	{
		m_acc = 22;
		m_jumpPower = 247;
		m_maxSpeed = 10;
		//m_jumpDelay = 2.2f;
		m_frozen = false;
		m_facingRight = true;
		m_grounded = false;
		m_accVec = new Vector2(m_acc, 0);
		//m_groundCheckPos0 = new Vector2(transform.position.x - 0.4f, transform.position.y -0.4f);
		//m_groundCheckPos1 = new Vector2(transform.position.x + 0.4f, transform.position.y -0.4f);
		m_scoring = GameObject.Find("InGameData").GetComponent<Scoring>();
		m_animator = GetComponent<Animator>();
        m_inputTouch = GameObject.Find("TouchHandler").GetComponent<InputTouch>();
        m_windController = GetComponent<WindController>();
    }
	
	// Update is called once per frame
	void Update () 
	{
		if(!m_frozen)
		{
			m_groundCheckPos0 = new Vector2(transform.position.x - 0.545f, transform.position.y -0.49f);
			m_groundCheckPos1 = new Vector2(transform.position.x + 0.545f, transform.position.y -0.49f);

			if(Physics2D.Linecast(transform.position, m_groundCheckPos0, 1 << LayerMask.NameToLayer("Ground")))
				m_grounded = true;
			else if(Physics2D.Linecast(transform.position, m_groundCheckPos1, 1 << LayerMask.NameToLayer("Ground")))
				m_grounded = true;
			else
				m_grounded = false;

			if(GetComponent<Rigidbody2D>().velocity.x > 0.35f)
			{
				if(!m_facingRight)
					flip();

				m_facingRight = true;
			}
			else if(GetComponent<Rigidbody2D>().velocity.x < -0.35f)
			{
				if(m_facingRight)
					flip();

				m_facingRight = false;
			}

			if(m_grounded)
			{
				//Accelerate
				if(m_facingRight)
					GetComponent<Rigidbody2D>().velocity += m_accVec * Time.deltaTime;
				else
					GetComponent<Rigidbody2D>().velocity += -1 * m_accVec * Time.deltaTime;

				//Limit max speed
				if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > m_maxSpeed)
				{
					GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * m_maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
				}
			}
		}
	}

	private void flip()
	{
		Vector2 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

	void FixedUpdate()
	{
		if(m_frozen)
			GetComponent<Rigidbody2D>().MovePosition(transform.position + (m_currentModuleEngaged.transform.position - transform.position) * 0.1f);
	}

	void OnTriggerStay2D(Collider2D col)
	{
		//If contact with jumper module
		if(col.CompareTag("JumperModule"))
		{
			JumperData jumperData = col.GetComponent<JumperData>();
            MoveAtoB moveComp = col.GetComponentInParent<MoveAtoB>();
            if (moveComp)
                moveComp.enabled = false;

			//If this is first contact with jumper
			if(!jumperData.Engaged)
			{
				jumperData.Engaged = true;
                m_inputTouch.PlayerCurrentCol = col;
                m_inputTouch.PlayerLocked = true;

                m_windController.updateWind();
				m_scoring.incrementScoreById(jumperData.ID, !m_grounded, jumperData.gameObject.transform.position);		//If landing is perfect is the opposite of m_grounded when first touching the jumper

				if(m_grounded)
					SoundManager.Instance.playEngage();
				else
					SoundManager.Instance.playPerfectEngage();

				StartCoroutine(Freeze (col.gameObject));
				m_currentModuleEngaged = col.gameObject;
				GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				GetComponent<PlayerDeath>().setKillerHeight(jumperData.ID - 1);
			}
			else if(!m_frozen && jumperData.Active)		//If we are no longer frozen then jump according to jumper module
			{
				col.GetComponent<Collider2D>().enabled = false;
				m_grounded = false;				//Might not be needed
				GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				GetComponent<Rigidbody2D>().AddForce(jumperData.JumpVec * m_jumpPower);
				m_animator.SetTrigger("jump");
				SoundManager.Instance.playJump();
			}

			//FIXME Faulty, good enough for now, make it linear or something..
			if(m_frozen)
				transform.position = Vector2.Lerp(transform.position, col.transform.position, Time.deltaTime);	//"Pull" player toward middle of jumper
		}
	}

	//Freezes and unfreezes player for some time
	//Manipulates the look of engaged jumper module
	IEnumerator Freeze(GameObject jumperModule) 
	{
		GetComponent<Rigidbody2D>().gravityScale = 0.0f;
		m_frozen = true;
        //float timer = m_jumpDelay;
        //float waitTime = m_jumpDelay / 15f;
        bool playerLocked = m_inputTouch.PlayerLocked;
        //FIXME: Need to set up a wait lock until player releases a jump command
        while (playerLocked)
        {
            playerLocked = m_inputTouch.PlayerLocked;
            //TODO: TWEAK WAIT TIME 
            yield return new WaitForSeconds(0.05f);
        }
		//{
//			Color color = jumperModule.renderer.material.color;
//			color.a -= waitTime / m_jumpDelay;
//			jumperModule.renderer.material.color = color;

		
			
		//}

		GetComponent<Rigidbody2D>().gravityScale = 1.0f;
		m_frozen = false;
		JumperActivate jumperActivate = jumperModule.GetComponent<JumperActivate>();
		jumperActivate.deActivate();
	}

	/*IEnumerator Fade(GameObject jumperModule)
	{
		Color color = jumperModule.GetComponent<Renderer>().material.color;
		while(color.a > 0)
		{
			color.a -= 0.1f;
			jumperModule.GetComponent<Renderer>().material.color = color;
			yield return new WaitForSeconds(0.1f);
		}
	}*/
}