using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class WindController : MonoBehaviour {

    private int m_maxPower;
    private Vector2 m_currentWindForce;
    private Scoring m_scoring;
    private ConstantForce2D m_forceComponent;
    private Text m_windForceText;
    private Image m_windForceImage;
    [SerializeField] private Sprite[] m_windForceSprites;
    private LinkedList<GameObject> m_clouds;

    void Awake()
    {
        m_clouds = new LinkedList<GameObject>();
    }

    // Use this for initialization
    void Start () {
        m_scoring = GameObject.Find("InGameData").GetComponent<Scoring>();
        m_forceComponent = GetComponent<ConstantForce2D>();
        m_windForceText = GameObject.Find("TextWindForce").GetComponent<Text>();
        m_windForceImage = GameObject.Find("ImageWindForce").GetComponent<Image>();

        m_currentWindForce = new Vector2(0,0);
        m_maxPower = 0;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    //Called when player successfully land on a new plaform
    //Randomizes a new wind power/direction
    public void updateWind()
    {
        float lastDir = Mathf.Sign(m_currentWindForce.x);
        m_maxPower = Mathf.Min(m_scoring.Score/2, 8);
        m_currentWindForce.x = Random.Range(-m_maxPower, m_maxPower);

        //Flip arrow if direction changes
        if (lastDir != Mathf.Sign(m_currentWindForce.x))
        {
            m_windForceImage.transform.RotateAround(m_windForceText.transform.position, Vector3.forward, 180);
        }

        //Change wind arrow according to wind power
        //Render with 0 alpha if there's no wind
        if (m_currentWindForce.x != 0f)
        {
            m_windForceImage.canvasRenderer.SetAlpha(1f);
            m_windForceImage.sprite = m_windForceSprites[(int)Mathf.Abs(m_currentWindForce.x) - 1];
        }
        else
            m_windForceImage.canvasRenderer.SetAlpha(0f);

        m_forceComponent.force = m_currentWindForce;
        m_windForceText.text = Mathf.Abs(m_currentWindForce.x).ToString();

        //Notify clouds of change
        if(m_clouds.Count > 0)
        {
            LinkedListNode<GameObject> it;
            for (it = m_clouds.First; it != m_clouds.Last.Next; it = it.Next)
            {
                it.Value.GetComponent<CloudMovement>().updateMovement(m_currentWindForce);
            }
        }
    }

    public void registerCloud(GameObject cloud)
    {
        m_clouds.AddLast(cloud);
    }

    public Vector2 WindForce
    {
        get { return m_currentWindForce; }
    }
}