using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class EndMessage : MonoBehaviour
{

	[SerializeField]
	private TMP_Text _playerMessage = null;



	/* Summary
	 * OnGameEnded is a public function that doesn't return anything
	 * Info: Gets called by the event OnGameEnded Event
	 * Functionality: Updates the Text mesh's text and color
	 */
	public void OnGameEnded(int winner)
	{
		_playerMessage.text = winner == -1 ? "Tie" : winner == 1 ? "AI wins" : "Player wins";
		_playerMessage.color= winner == -1 ? new Color(255, 255, 0, 255) : winner == 1 ? new Color(255, 0, 0, 255) : new Color(0, 255, 0, 255); 
	}
}
