using UnityEngine;
using System.Collections;

public class JumperActivate : MonoBehaviour {
	
	private Color m_green = Color.green;
	private LineRenderer m_lineRenderer;
	private JumperData m_jumperData;
	[SerializeField] private Material m_lineMat;
	// Use this for initialization
	void Start () {
		m_lineRenderer = gameObject.AddComponent<LineRenderer>();
		m_lineRenderer.material = m_lineMat;
		m_lineRenderer.material.color = m_green;
		m_lineRenderer.SetWidth(0.2F, 0.1F);
		m_lineRenderer.SetVertexCount(2);
		m_lineRenderer.SetPosition(0, new Vector3(0.0f, 0.0f, -2.0f));
		m_lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		m_lineRenderer.receiveShadows = false;
		m_lineRenderer.enabled = false;
		m_lineRenderer.useWorldSpace = false;
		m_lineRenderer.sortingLayerName = "Foreground";
		m_jumperData = GetComponent<JumperData>();
	}

	public void reInitialize()
	{
		StopCoroutine("Fade");

		Color color = GetComponent<Renderer>().material.color;
		color.a = 1;
		GetComponent<Renderer>().material.color = color;
		GetComponent<Collider2D>().enabled = true;
		m_lineRenderer.enabled = false;
		m_lineRenderer.SetPosition(1, Vector2.zero);

		MoveAtoB moveAtoB = GetComponentInParent<MoveAtoB>();
		if(moveAtoB != null)
			moveAtoB.reInitialize();
	}

	// Update is called once per frame
	void Update () 
	{

	}

	public void deActivate()
	{
		StartCoroutine("Fade", gameObject);
	}

	IEnumerator Fade(GameObject jumperModule)
	{
		Color color = jumperModule.GetComponent<Renderer>().material.color;
		while(color.a > 0)
		{
			color.a -= 0.1f;
			jumperModule.GetComponent<Renderer>().material.color = color;
			yield return new WaitForSeconds(0.1f);
		}
	}

	public void activateJumper(Vector2 endPos)
	{
		Vector2 clamped = endPos - (Vector2)gameObject.transform.position;
		clamped = Vector2.ClampMagnitude(clamped, 8.0f);
		m_jumperData.JumpVec = clamped;
//		m_jumperData.JumpVec = (endPos - (Vector2)gameObject.transform.position);
		m_jumperData.Active = true;
		m_lineRenderer.SetPosition(1, new Vector3(clamped.x, clamped.y, -2.0f));
		m_lineRenderer.enabled = true;
	}
}