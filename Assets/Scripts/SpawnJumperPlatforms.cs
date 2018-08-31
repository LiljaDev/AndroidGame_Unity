using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnJumperPlatforms : MonoBehaviour {
	[SerializeField] private GameObject m_largePlat;
	[SerializeField] private GameObject m_smallPlat;
	[SerializeField] private GameObject m_movingPlat;
	private SpawnCosmetics m_cosmeticsSpawner;
	private LinkedList<GameObject> m_jumperPlatforms;
	private Queue<GameObject> m_movingJumperPlatforms;
	private Queue<GameObject> m_platformsInUse;
	private Transform m_player;
	private float m_xMax;
	private int m_xMin;
	private int m_yMin;
	private float m_yMax;
	private int m_nextId;
	private Vector2 m_worldSpaceVisibleSize;
	private Vector2 m_newestPlatPos;
	private const ushort m_numberOfPlatforms = 15;
    private SpawnEnemies m_enemySpawner;

	void Awake()
	{
		m_newestPlatPos = Vector2.zero;
	}

	// Use this for initialization
	void Start () 
	{
		m_cosmeticsSpawner = GameObject.Find("BackgroundGFX").GetComponent<SpawnCosmetics>();
        m_enemySpawner = GetComponent<SpawnEnemies>();

		//m_worldVisible = (Vector2)(Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)) - Camera.main.transform.position);
		WorldLimits worldLimits = Camera.main.GetComponent<WorldLimits>();
		m_worldSpaceVisibleSize.x = worldLimits.WorldSpaceWidth;
		m_worldSpaceVisibleSize.y = worldLimits.WorldSpaceHeight;
		m_nextId = 1;
		m_xMin = 5;
		m_xMax = worldLimits.WorldSpaceWidth;

//		Debug.Log("MIN ALLOWED X = " + m_xMin);
//		Debug.Log("MAX ALLOWED X = " + m_xMax);
		//m_xMax = 10;
		m_yMin = 2;
//		m_yMax = (int)(m_worldVisible.y * 1.5f);
		m_yMax = 8;

		//List of spawned platforms with jumpers
		m_jumperPlatforms = new LinkedList<GameObject>();
		m_movingJumperPlatforms = new Queue<GameObject>();
		m_platformsInUse = new Queue<GameObject>();

		//Instantiate a first jumper platform in origo
		GameObject temp = Instantiate(m_largePlat, Vector2.zero, Quaternion.identity) as GameObject;
		temp.GetComponentInChildren<JumperData>().ID = m_nextId;
		m_jumperPlatforms.AddLast(temp);
		++m_nextId;
		m_platformsInUse.Enqueue(temp);
		//Instantiate the rest of the platforms
		for(short i = 0; i < m_numberOfPlatforms - 1; ++i)
		{
			float newPosX, newPosY;
			do
			{
				newPosX = m_jumperPlatforms.Last.Value.transform.position.x + (Random.value - 0.5f) * m_xMax;
			}while(Mathf.Abs(newPosX - m_jumperPlatforms.Last.Value.transform.position.x) < m_xMin);	//Maybe revert this to last platform width instead of static 4.0f
			//			Debug.Log("NEW PLATFORM X_DIF = " + Mathf.Abs(newPosX - lastPlatPos.x));
			newPosY = m_jumperPlatforms.Last.Value.transform.position.y + Random.Range(m_yMin, m_yMax);
			
			Vector2 newPlatPos = new Vector2(newPosX, newPosY);
			m_newestPlatPos = newPlatPos;
			//GameObject newPlat;
			if(m_nextId < 5)
			{
				temp = Instantiate(m_largePlat, newPlatPos, Quaternion.identity) as GameObject;
				m_cosmeticsSpawner.SpawnClouds(newPlatPos);
			}
			else
			{
				float spawnValue = Random.value;
				if(spawnValue > 0.8f)
					temp = Instantiate(m_smallPlat, newPlatPos, Quaternion.identity) as GameObject;
				else
					temp = Instantiate(m_largePlat, newPlatPos, Quaternion.identity) as GameObject;

				m_cosmeticsSpawner.SpawnClouds(newPlatPos);
			}
			temp.GetComponentInChildren<JumperData>().ID = m_nextId;
			m_jumperPlatforms.AddLast(temp);
			m_platformsInUse.Enqueue(temp);
			++m_nextId;
		}

		Vector2 invisiblePos = new Vector2(0f, -20f);
		//Instantiate a few moving platforms as well
		for(int i = 0; i < m_numberOfPlatforms; ++i)
		{
			temp = Instantiate(m_movingPlat, invisiblePos*(i+1), Quaternion.identity) as GameObject;
            temp.GetComponent<MoveAtoB>().initialize(1, 2, 2, 1.5f);
			m_movingJumperPlatforms.Enqueue(temp);
		}

		//Get player transform
		m_player = GameObject.FindWithTag("Player").transform;	//Position used to determine when to move platforms
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector2 lastPlatPos = m_newestPlatPos;		//Newest platform
		//Vector2 firstPlatPos = m_jumperPlatforms.First.Value.transform.position;	//Oldest platform
		Vector2 firstPlatPos = m_platformsInUse.Peek().transform.position;	//Oldest platform
		//If newest platform is less than x units above player, spawn new platform
		if(m_player.transform.position.y - firstPlatPos.y > 15)
		{
			//TODO: Maybe bring this back instead of a static 4.0f
//			foreach(Transform child in m_jumperPlatforms.Last.Value.transform)
//			{
//				if(child.CompareTag("Platform"))
//				   lastPlatWidth = child.collider2D.bounds.size.x;
//			}


			float newPosX, newPosY;
			do
			{
				newPosX = lastPlatPos.x + (Random.value - 0.5f) * m_xMax;
			}while(Mathf.Abs(newPosX - lastPlatPos.x) < m_xMin);	//Maybe revert this to last platform width instead of static 4.0f
			newPosY = lastPlatPos.y + Random.Range(m_yMin, m_yMax);
			m_newestPlatPos = new Vector2(newPosX, newPosY);

			m_platformsInUse.Dequeue();		//This platform is no longer "in use"
			//Platform that is to be moved
			GameObject objToMove;

			float spawnValue = Random.value;
			if(spawnValue > 0.85f && m_player.transform.position.y - m_movingJumperPlatforms.Peek().transform.position.y > 15)
			{
				objToMove = m_movingJumperPlatforms.Dequeue();
				m_movingJumperPlatforms.Enqueue(objToMove);
			}
			else
			{
				objToMove = m_jumperPlatforms.First.Value;
				//FIXME: This does not check if the platform is actually at a safe distance
				//Please review this list usage once again, doesn't feel too effective at the moment!!!

				//Remove it from start of list and add it to the end
				m_jumperPlatforms.RemoveFirst();
				m_jumperPlatforms.AddLast(objToMove);
			}

			m_platformsInUse.Enqueue(objToMove);

			objToMove.transform.position = m_newestPlatPos;	//Move the platform
			m_cosmeticsSpawner.SpawnClouds(m_newestPlatPos);//Spawn some clouds
			JumperActivate jumperActivate = objToMove.GetComponentInChildren<JumperActivate>();
			jumperActivate.reInitialize();

			JumperData jumperData = objToMove.GetComponentInChildren<JumperData>();
			jumperData.reInitialize();
			jumperData.ID = m_nextId;

			++m_nextId;

            m_enemySpawner.rollSpawner(m_newestPlatPos);

            //			if(m_nextId < 2)
            //				temp = Instantiate(m_largePlat, newPlatPos, Quaternion.identity) as GameObject;
            //			else
            //			{
            //				if(Random.Range(1, 10) > 8)
            //					temp = Instantiate(m_smallPlat, newPlatPos, Quaternion.identity) as GameObject;
            //				else
            //					temp = Instantiate(m_largePlat, newPlatPos, Quaternion.identity) as GameObject;
            //			}
            //			temp.GetComponentInChildren<JumperData>().ID = m_nextId;
            //			m_jumperPlatforms.AddLast(temp);

            //			++m_nextId;
        }

//		if(m_player.transform.position.y - firstPlatPos.y > 15)
//		{
//			Destroy(m_jumperPlatforms.First.Value.gameObject);
//			m_jumperPlatforms.RemoveFirst();
//		}

//		LinkedListNode<GameObject> currentPlat;
//		currentPlat = m_jumperPlatforms.First;	//Start at oldest platform node

		//Check if player passed a new height
		//GOTTA MAKE THIS HAPPEN WHEN WE ATTACH INSTEAD OF JUMP ABOVE
//		while(currentPlat != null)
//		{
//			if(currentPlat.Value.transform.position.y > m_killerHeight && m_player.position.y > currentPlat.Value.transform.position.y)
//				m_killerHeight = currentPlat.Previous.Value.transform.position.y;
//
//			currentPlat = currentPlat.Next;
//		}
	}

	public LinkedList<GameObject> Platforms
	{
		get{return m_jumperPlatforms;}
	}

	public Vector2 NewestPlatPos
	{
		get{return m_newestPlatPos;}
	}

//	public float KillerHeight
//	{
//		get{return m_killerHeight;}
//	}
}