using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnCosmetics : MonoBehaviour
{
	[SerializeField] private LinkedList<GameObject> m_clouds;
	//[SerializeField] private GameObject[] m_cloudPrefabs;
	[SerializeField] private GameObject m_simpleSpritePrefab;
	[SerializeField] private Sprite[] m_cloudSprites;
	private float m_worldSpaceWidth;
	private float m_worldSpaceHeight;
    private Transform m_player;
    private WindController m_windController;

	//private LinkedList<Transform> m_clouds;
	//private Transform m_mainCameraTransform;
	//private SpawnJumperPlatforms m_platformSpawner;

    void Awake()
    {
        m_clouds = new LinkedList<GameObject>();

        instantiateClouds();
    }

	// Use this for initialization
	void Start () 
	{
		//m_clouds = new Queue<GameObject>();
		
		WorldLimits worldLimits = Camera.main.GetComponent<WorldLimits>();
		m_worldSpaceWidth = worldLimits.WorldSpaceWidth;
		m_worldSpaceHeight = worldLimits.WorldSpaceHeight;

        m_player = GameObject.FindWithTag("Player").transform;
        Debug.Log("Player found at pos: " + m_player.position);

        m_windController = m_player.gameObject.GetComponent<WindController>();
        StartCoroutine("MoveCloudsHorizontal");

        LinkedListNode<GameObject> it;
        for (it = m_clouds.First; it != m_clouds.Last.Next; it = it.Next)
        {
            m_windController.registerCloud(it.Value);
        }
            
        //m_mainCameraTransform = Camera.main.transform;

        //m_platformSpawner = GameObject.Find("Spawner").GetComponent<SpawnJumperPlatforms>();

        /*for(ushort i = 0; i < 10; ++i)
		{
			Vector2 newCloudPos = m_platformSpawner.NewestPlatPos;
			newCloudPos.x += (Random.value - 0.5f) * 2f * m_worldSpaceWidth;
			newCloudPos.y += (Random.value - 0.5f) * 2f * m_worldSpaceHeight;
			GameObject newCloud = Instantiate(m_cloudPrefabs[Random.Range(0, 2)], newCloudPos, Quaternion.identity) as GameObject;
			m_clouds.AddLast(newCloud.transform);
		}

		StartCoroutine("SpawnClouds");*/
    }
	
    private void instantiateClouds()
    {
        for(int i = 0; i < 30; ++i)
        {
            Vector2 newCloudPos = new Vector2(0f, -100f);
            GameObject newCloud = Instantiate(m_simpleSpritePrefab, newCloudPos, Quaternion.identity) as GameObject;

            m_clouds.AddLast(newCloud);
            int spriteNumber = Random.Range(0, 4);
            newCloud.GetComponent<SpriteRenderer>().sprite = m_cloudSprites[spriteNumber];
            float scale = Random.Range(0.6f, 1.3f);
            newCloud.transform.localScale = new Vector2(scale, scale);
           // CloudMovement simpleMovement = newCloud.GetComponent<CloudMovement>();
           // simpleMovement.Velocity = new Vector2(scale * -1 / 1.65f, 0.0f);

            newCloud.SetActive(false);
        }
    }

	// Update is called once per frame
	void Update () 
	{
       // Debug.Log(m_clouds.Count);
	}

	public void SpawnClouds(Vector2 pos)
	{
		if(m_worldSpaceWidth == 0 || m_worldSpaceHeight == 0)
		{
			WorldLimits worldLimits = Camera.main.GetComponent<WorldLimits>();
			m_worldSpaceWidth = worldLimits.WorldSpaceWidth;
			m_worldSpaceHeight = worldLimits.WorldSpaceHeight;
		}

		for(int i = 0; i < 3; ++i)
		{
            if (m_clouds.Count > 0 && m_clouds.First.Value.transform.position.y < m_player.position.y - m_worldSpaceHeight)
            {
                //Get the cloud to be moved
                GameObject cloud = m_clouds.First.Value;
                m_clouds.RemoveFirst();

                //Randomize and set new position
                Vector2 newCloudPos = pos;
                newCloudPos.x += (Random.value - 0.5f) * 2f * m_worldSpaceWidth;
                newCloudPos.y += (Random.value - 0.5f) * m_worldSpaceHeight;
                cloud.transform.position = newCloudPos;

                //Re-queue and activate the cloud
                m_clouds.AddLast(cloud);
                cloud.SetActive(true);
            }



			//GameObject newCloud = Instantiate(m_simpleSpritePrefab, newCloudPos, Quaternion.identity) as GameObject;
            
          //  m_clouds.Enqueue(newCloud);
          //  int spriteNumber = Random.Range(0,4);
			//newCloud.GetComponent<SpriteRenderer>().sprite = m_cloudSprites[spriteNumber];
			//float scale = Random.Range(0.6f, 1.3f);
			//newCloud.transform.localScale = new Vector2(scale, scale);
			//SimpleMovement simpleMovement = newCloud.GetComponent<SimpleMovement>();
			//simpleMovement.Velocity = new Vector2(scale*-1/1.65f, 0.0f);
			
			//Debug.Log(m_clouds.Count);
		}
	}


	private IEnumerator MoveCloudsHorizontal()
	{
        //List iterator
        LinkedListNode<GameObject> it;

        //Run until explicitly stopped
        while (true)
        {
            //Iterate list
            for (it = m_clouds.First; it != m_clouds.Last.Next; it = it.Next)
            {
                //Get transform of the object
                Transform toTranslate = it.Value.transform;
                Vector2 cloudPosition = toTranslate.position;

                //If object is far enough outside the view to be moved
                float windForceX = m_windController.WindForce.x;

                if (windForceX < 0f && cloudPosition.x < m_player.position.x - m_worldSpaceWidth / 1.4f)
                {
                    cloudPosition.x = m_player.position.x + (Random.value + 1f) * m_worldSpaceWidth;
                    toTranslate.position = cloudPosition;
                }
                else if (windForceX > 0f && cloudPosition.x > m_player.position.x + m_worldSpaceWidth / 1.4f)
                {
                    cloudPosition.x = m_player.position.x - (Random.value + 1f) * m_worldSpaceWidth;
                    toTranslate.position = cloudPosition;
                }
            }

            //float waitTime = Random.Range(0,2) * (Random.value + 1f);
            yield return new WaitForSeconds(2f);
        }
	}
}