using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



[RequireComponent(typeof(Button))]
public class RetryButton : MonoBehaviour
{


	// Gets executed on awake
	private void Awake()
	{
		GetComponent<Button>().onClick.AddListener(Retry);
	}



	// Re-Loads the scene for new game
	public void Retry(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
