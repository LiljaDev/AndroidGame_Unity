using UnityEngine;
using System.Collections;

public class InputTouch : MonoBehaviour {

	private float m_radius;
	private Vector2 m_touchBegin, m_touchEnd;
	private bool m_touchingModule;
	private float m_maxLength;
	private LineRenderer m_lineRenderer;
	[SerializeField] private Material m_lineMat;
	[SerializeField] private GameObject m_jumpAreaPrefab;
	private JumpAreaMovement m_jumpAreaMovement;
	private Collider2D m_currentCol;
    private Collider2D m_playerCurrentCol;
    private bool m_playerLocked;

	void Awake()
	{
		GameObject jumpArea = Instantiate(m_jumpAreaPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		m_jumpAreaMovement = jumpArea.GetComponent<JumpAreaMovement>();
	}

	// Use this for initialization
	void Start () 
	{
		m_radius = 1.0f;
		m_touchingModule = false;
		m_lineRenderer = GetComponent<LineRenderer>();
		m_lineRenderer.sortingLayerName = "Foreground";
        m_playerLocked = false;
        //		m_lineRenderer = gameObject.AddComponent<LineRenderer>();
        //		m_lineRenderer.material = m_lineMat;
        //		m_lineRenderer.material.color = Color.red;
        //		m_lineRenderer.SetWidth(0.2F, 0.1F);
        //		m_lineRenderer.SetVertexCount(2);
        //		m_lineRenderer.castShadows = false;
        //		m_lineRenderer.receiveShadows = false;
        //		m_lineRenderer.enabled = false;
    }
	
	// Update is called once per frame
	void Update () 
	{
//		if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
//		{
//			m_touchBegin = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
//			m_touching = true;
//		}
//		
//		if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
//			m_touchEnd = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

		foreach(Touch touch in Input.touches)
		{
			if(touch.phase == TouchPhase.Began)
			{
				m_touchBegin = Camera.main.ScreenToWorldPoint(touch.position);

				Collider2D[] cols = Physics2D.OverlapCircleAll(m_touchBegin, m_radius);
				foreach(Collider2D col in cols)
				{
					if(col.CompareTag("JumperModule"))
					{
						m_touchingModule = true;
						m_currentCol = col;

						m_lineRenderer.SetPosition(0, new Vector3(col.transform.position.x, col.transform.position.y, -2.0f));
						m_lineRenderer.SetPosition(1, new Vector3(col.transform.position.x, col.transform.position.y, -2.0f));
						m_lineRenderer.enabled = true;

						m_jumpAreaMovement.activate(m_currentCol.transform.position);
						break;
					}
				}
			}

			if(m_touchingModule)
			{
				//if(Input.GetTouch(0).phase == TouchPhase.Moved)
				{
//					m_lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position));
					Vector3 clamped = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position) - m_currentCol.transform.position;
					clamped = Vector2.ClampMagnitude(clamped, 8.0f);
					m_lineRenderer.SetPosition(0, new Vector3(m_currentCol.transform.position.x, m_currentCol.transform.position.y, -2.0f));
					m_lineRenderer.SetPosition(1, clamped + new Vector3(m_currentCol.transform.position.x, m_currentCol.transform.position.y, -2.0f));
				}
			}

			if(touch.phase == TouchPhase.Ended)
			{
				m_touchEnd = Camera.main.ScreenToWorldPoint(touch.position);

				//if(m_currentCol.CompareTag("JumperModule"))
				if(m_touchingModule)
				{
				   	JumperActivate jumperActivate = m_currentCol.GetComponent<JumperActivate>();
					jumperActivate.activateJumper(m_touchEnd);
				}

				m_touchingModule = false;

                if (m_currentCol == m_playerCurrentCol)
                    m_playerLocked = false;

				m_lineRenderer.enabled = false;

				m_jumpAreaMovement.deactivate();
//				Collider2D[] cols = Physics2D.OverlapCircleAll(m_touchBegin, m_radius);
//
//				foreach(Collider2D col in cols)
//				{
//					if(col.CompareTag("JumperModule"))
//					{
//					   	JumperActivate jumperActivate = col.GetComponent<JumperActivate>();
//						jumperActivate.activateJumper(m_touchEnd);
//						break;
//					}
//				}
			}
		}
	}

    public bool IsTouchingModule
    {
        get { return m_touchingModule; }
    }

    public Collider2D PlayerCurrentCol
    {
        set { m_playerCurrentCol = value; }
    }

    public bool PlayerLocked
    {
        set { m_playerLocked = value; }
        get { return m_playerLocked; }
    }
}
