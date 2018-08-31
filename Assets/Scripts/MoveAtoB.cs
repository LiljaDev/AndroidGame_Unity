using UnityEngine;
using System.Collections;

public class MoveAtoB : MonoBehaviour {

    [SerializeField] private int m_movementType;
	[SerializeField] private int m_distanceA;
	[SerializeField] private int m_distanceB;
    [SerializeField] private float m_radius;
    [SerializeField] private float m_movementSpeed;
	[SerializeField] private bool m_horizontal;
    [SerializeField] private Vector2 m_origin;
    private Rigidbody2D m_rigidBody2D;
    private float m_timerMovement;

	// Use this for initialization
	void Start () {
        m_timerMovement = 0.0f;
        m_origin = transform.position;
        m_rigidBody2D = GetComponent<Rigidbody2D>();

        if (m_movementType == 2)
        {
            m_rigidBody2D.MovePosition(new Vector2(m_origin.x - m_radius, m_origin.y));
        }
	}
	
	// Update is called once per frame
	void Update ()
	{
        switch (m_movementType)
        {
            case 1:
                linearMove();
                break;
            case 2:
                circularMove();
                break;
            case 3:     //Half circle?
                break;
        }
	}

    private void linearMove()
    {
        Vector2 pos = transform.position;

        if (m_horizontal)
        {
            pos.x += m_movementSpeed * Time.deltaTime;

            if (m_movementSpeed < 0 && pos.x < m_origin.x - m_distanceA)
            {
                pos.x = m_origin.x - m_distanceA;
                m_movementSpeed *= -1;
            }
            else if (m_movementSpeed > 0 && pos.x > m_origin.x + m_distanceB)
            {
                pos.x = m_origin.x + m_distanceB;
                m_movementSpeed *= -1;
            }
        }
        else
        {
            pos.y += m_movementSpeed * Time.deltaTime;

            if (m_movementSpeed < 0 && pos.y < m_origin.y - m_distanceA)
            {
                pos.y = m_origin.y - m_distanceA;
                m_movementSpeed *= -1;
            }
            else if (m_movementSpeed > 0 && pos.y > m_origin.y + m_distanceB)
            {
                pos.y = m_origin.y + m_distanceB;
                m_movementSpeed *= -1;
            }
        }

        m_rigidBody2D.MovePosition(pos);
    }

    //Move in a circle around the initial position (m_origin) at a distance m_radius
    private void circularMove()
    {
        m_timerMovement += m_movementSpeed * Time.deltaTime;
        Vector2 newPos = new Vector2(m_origin.x + Mathf.Cos(m_timerMovement)*m_radius, m_origin.y + Mathf.Sin(m_timerMovement)*m_radius);
        m_rigidBody2D.MovePosition(newPos);
    }

    //Reset anything necessary when reusing instead of destroying&instantiating
	public void reInitialize()
	{
        m_origin = transform.position;
	}

    public void initialize(int movementType, int distanceA, int distanceB, float movementSpeed)
    {
        m_movementType = movementType;
        m_distanceA = distanceA;
        m_distanceB = distanceB;
        m_movementSpeed = movementSpeed;
    }

    public void initialize(int movementType, float radius, float movementSpeed)
    {
        m_movementType = movementType;
        m_radius = radius;
        m_movementSpeed = movementSpeed;
    }

    public int MovementType
    {
        get { return m_movementType; }
        set { m_movementType = value; }
    }

    public float Radius
    {
        get { return m_radius; }
        set { m_radius = value; }
    }
}