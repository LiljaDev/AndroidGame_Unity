using UnityEngine;
using System.Collections;

public class SpawnEnemies : MonoBehaviour {

    [SerializeField] private GameObject m_enemyPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //May spawn an enemy based on spawn rate
    //Enemy is positioned based on given position vector
    public void rollSpawner(Vector2 platformPos)
    {
        float rnd = Random.value;
        if (rnd > 0.65)
        {
            //Instantiate
            GameObject newEnemy = Instantiate(m_enemyPrefab, platformPos, Quaternion.identity) as GameObject;
            MoveAtoB moveAtoB = newEnemy.GetComponent<MoveAtoB>();

            //Randomize and set attributes
            float radius = Random.Range(1.5f, 3.5f);
            float movementSpeed = Random.Range(1f, 2.5f);
            moveAtoB.initialize(2, radius, movementSpeed);
        }
    }
}
