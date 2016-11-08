using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScreenTransition : MonoBehaviour {

    private const int LAST_LEVEL = 17;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space") || 
            (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {   
            Scene currScene = SceneManager.GetActiveScene();
                if (currScene.buildIndex < LAST_LEVEL) // not last scene yet
                    SceneManager.LoadScene(currScene.buildIndex + 1, LoadSceneMode.Single);
        }
	}
}
