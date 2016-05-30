using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
	
	public void LoadLevel(string name){
		//Debug.Log ("New Level load: " + name);
		SceneManager.LoadScene(name);
	}

	public void LoadLevelWithIndex (int a){
		Debug.Log ("New Level load: " + a);
		SceneManager.LoadScene(a);
	}

	public void LoadNextLevel(){
		int a = SceneManager.GetActiveScene().buildIndex + 1;
		SceneManager.LoadScene(a);
	}
	
	public void QuitRequest(){
		//Debug.Log ("Quit requested");
		Application.Quit ();
	}
	
}
