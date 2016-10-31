using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class buttonController : MonoBehaviour {

	// Use this for initialization
	public void LoadScene (string sceneName) {
		SceneManager.LoadScene (sceneName);
	}

	public void LoadCurrentScene(){
		Scene current = SceneManager.GetActiveScene ();

		SceneManager.LoadScene (current.name);
	}
}