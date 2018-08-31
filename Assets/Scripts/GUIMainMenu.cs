using UnityEngine;
using System.Collections;

public class GUIMainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void clickEndless()
	{
		Application.LoadLevel("Endless");
	}

	/*public void clickChallenges()
	{
		Application.LoadLevel("ChallengeMenu");
	}*/
}
