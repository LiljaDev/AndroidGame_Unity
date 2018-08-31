using UnityEngine;
using System.Collections;

public class CloudMovement : MonoBehaviour {
	[SerializeField] private Vector3 m_velocity;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += m_velocity * Time.deltaTime;
	}

	public Vector3 Velocity
	{
		set{m_velocity = value;}
	}

    public void updateMovement(Vector3 movementVec)
    {
        m_velocity = movementVec * transform.localScale.x / 3f;
    }
}
