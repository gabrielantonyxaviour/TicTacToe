using System;
using System.Collections.Generic;
using UnityEngine;



public class ClickTrigger : MonoBehaviour
{


	// Objects
	TicTacToeAI _ai;


	// State variables
	[SerializeField]
	private int _myCoordX = 0;
	[SerializeField]
	private int _myCoordY = 0;
	[SerializeField]
	private bool canClick;
	[SerializeField] 
	


	// Executes on awake
	private void Awake()
	{
		_ai = FindObjectOfType<TicTacToeAI>();
	}


	// Executes on start
	private void Start(){

		_ai.onGameStarted.AddListener(AddReference);
		_ai.onGameStarted.AddListener(() => SetInputEnabled(true));
		_ai.onPlayerWin.AddListener((win) => SetInputEnabled(false));
	}


	/* Summary
	 * SetInputEnabled is a public function that doesn't return anything
	 * Functionality: Updates the ability for player to input data in the box
	 */
	public void SetInputEnabled(bool val){
		canClick = val;
	}


	/* Summary
	 * AddReference is a private function that doesn't return anything
	 * Functionality: Registers this box in the state variable of the TicTacToeAI script
	 */
	private void AddReference()
	{
		_ai.RegisterTransform(_myCoordX, _myCoordY, this);
	}



	/* Summary
	 * OnMouseDown is a UnityEngine event handler callback
	 * Info: This executes when the player presses the left mouse button down
	 * Functionality: It chooses this box if the box is unoccupied and if it's the player's turn to make the move
	 */
	private void OnMouseDown()
	{
		if(isNotOccupied() && _ai.currentTurn==TurnState.player){
			
			_ai.PlayerSelects(_myCoordX, _myCoordY);
			SetInputEnabled(false);
		}
	}



	/* Summary
	 * Info: This implementations prevents the private variables from getting updated
	 * Functionality: Read functions for private variables
	 */
	public bool isNotOccupied()
	{
		return canClick;
	}
	public int getCoordX()
	{
		return _myCoordX;
	}
	public int getCoordY()
	{
		return _myCoordY;
	}
}
