using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;



// Enums for easier understanding and more readable code
public enum TicTacToeState{none, cross, circle}
public enum TurnState { player, ai };




// Events
[System.Serializable]
public class WinnerEvent : UnityEvent<int>{}




public class TicTacToeAI : MonoBehaviour
{
	// constants
	private int _aiLevel;



	// State varaibles
	public TurnState currentTurn;
	TicTacToeState[,] boardState;
	[SerializeField]
	private TicTacToeState playerState ;
	[SerializeField]
	private TicTacToeState aiState ;
	private List<int> freeSpaces;
	private ClickTrigger[,] _triggers;



	// Prefab variables
	[SerializeField]
	private GameObject _xPrefab;
	[SerializeField]
	private GameObject _oPrefab;



	// Event variables
	public UnityEvent onGameStarted;
	public WinnerEvent onPlayerWin;



	// Executes on awake
	private void Awake()
	{
		if(onPlayerWin == null){
			onPlayerWin = new WinnerEvent();
		}
	}



	/* Summary
	 * RegisterTransform is a public function that doesn't return anything
	 * Info: This function is called from ClickTrigger.cs only
	 * Functionality: Initializes click triggers for each box when called by the respective boxes
	 */
	public void RegisterTransform(int myCoordX, int myCoordY, ClickTrigger clickTrigger)
	{
		_triggers[myCoordX, myCoordY] = clickTrigger;
	}



	/* Summary
	 * StartGame is a private function that doesn't return anything
	 * Info: This function gets called when the player chooses his difficulty to start the game
	 * Functionality: Sets up the game and randomizes who should start the game
	 */
	public void StartGame(int AILevel)
	{
		BoardSetup();
		_aiLevel = AILevel;
		int start = Mathf.FloorToInt(Random.Range(0, 2));

		if (start == 1)
		{
		
			playerState = TicTacToeState.cross;
			aiState = TicTacToeState.circle;
			currentTurn=TurnState.player;
		}
		else
		{
			playerState = TicTacToeState.circle;
			aiState = TicTacToeState.cross;
			currentTurn = TurnState.ai;
			NoobMode();
		}
	}



	/* Summary
	 * BoardSetup is a private function that doesn't return anything
	 * Functionality: Instantiates click triggers for the boxes and initializes two state variables => freespaces, boardState
	 */
	private void BoardSetup()
	{
		_triggers = new ClickTrigger[3, 3];
		onGameStarted.Invoke();
		freeSpaces=new List<int>();
		for (var i = 0; i < 9; i++)
		{
			
				freeSpaces.Add(i);
			
		}
		boardState=new TicTacToeState[3,3];
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				boardState[i, j] = TicTacToeState.none;
			}

		}
	}



	/* Summary
	 * checkWinner is a private function that returns an integer
	 * Functionality: Checks each row, column and diagonals for the possible chances of winners
	 * Return values:
	 *  -1 => There is no winner yet, Game must continue
	 *   0 => Match has been tied
	 *  10 => AI won the game
	 * -10 => Player won the game
	 */
	private int checkWinner()
	{
		TicTacToeState winner=TicTacToeState.none;

		// Horizontal check
		for (int i = 0; i < 3; i++)
		{
			if(boardState[i,0]==boardState[i,1]&&boardState[i,1]==boardState[i,2])winner=boardState[i,0];
		}

		// Vertical check
		for (int i = 0; i < 3; i++)
		{
			if (boardState[ 0,i] == boardState[ 1,i] && boardState[ 1,i] == boardState[ 2,i])winner = boardState[ 0,i];
		}

		// Diagonal check
		if((boardState[0,0]==boardState[1,1]&& boardState[0, 0] == boardState[2, 2])|| (boardState[0, 2] == boardState[1, 1] && boardState[1, 1] == boardState[2, 0]))winner= boardState[1, 1];
		

		// Open spots check
		int openSpots=0;
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				if ( boardState[i, j] == TicTacToeState.none)
				{
					
					openSpots+=1;
				}
			}

		}
		// Winner check
		if(winner==TicTacToeState.none&&openSpots==0)
		{
			return 0;
		}
		else
		{
			if(winner==aiState)return 10;
			else if(winner==playerState)return -10;
			else return -1;
			
		}
	}



	/* Summary
	 * minimax is a private function that returns an integer
	 * Info: This is the most important function that implements the minimax algorithm to create the UNBEATABLE AI!!!
	 * Functionality: Chooses the path that gives victory (in the worst case pulls off a draw) in the 3pow9 possibilities of moves in tictactoe  
	 * Return values:
	 *  10 => Definite victory
	 * -10 => Definite Loss
	 *   0 => Draw
	 */
	private int minimax(TicTacToeState[,] boardState, int depth, bool isMaximising)
	{
		int result =checkWinner();
		if(result !=-1)return result;
		

		if(isMaximising)
		{
			int bestScore=int.MinValue;
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					if (boardState[i, j] == TicTacToeState.none)
					{
						boardState[i, j] = aiState;
						int score = minimax(boardState, depth+1, false);
						boardState[i, j] = TicTacToeState.none;
						if (score > bestScore)
						{
							bestScore = score;
						}
					}

				}
			}
			return bestScore;

		}
		else
		{
			int bestScore = int.MaxValue;
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					if (boardState[i, j] == TicTacToeState.none)
					{
						boardState[i, j] = playerState;
						int score = minimax(boardState, depth + 1, true);
						boardState[i, j] = TicTacToeState.none;
						if (score < bestScore)
						{
							bestScore = score;
						}
					}

				}
			}
			return bestScore;
		}

	}



	/* Summary
	 * Beastmode is a private function that doesn't return anything
	 * Info: Difficulty: HARD
	 * Functionality: Calls minimax algorithm to make the best move possible
	 */
	private void Beastmode()
	{
		int bestScore = int.MinValue;
		int[] move = new int[2];
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				if (boardState[i, j] == TicTacToeState.none)
				{
					boardState[i, j] = aiState;
					int score = minimax(boardState, 0, false);
					boardState[i, j] = TicTacToeState.none;
					if (score > bestScore)
					{
						bestScore = score;
						move = new int[2] { i, j };
					}
				}
			}

		}

		AiSelects(move[0], move[1]);
		boardState[move[0], move[1]] = aiState;
	}



	/* Summary
	 * NoobMode is a private function that doesn't return anything
	 * Info: Difficulty: EASY
	 * Functionality: Chooses random positions to play the game
	 */
	private void NoobMode()
	{
		int choose = Mathf.FloorToInt(Random.Range(0, freeSpaces.Count));
		AiSelects(freeSpaces[choose] / 3, freeSpaces[choose] % 3);
		freeSpaces.RemoveAt(choose);
	}



	/* Summary
	 * PlayerSelects is a private function that doesn't return anything
	 * Functionality: Displays the AIState and updates state variables and returns if game is over
	 */
	private void AiSelects(int coordX, int coordY){
		SetVisual(coordX, coordY, aiState);
		_triggers[coordX,coordY].SetInputEnabled(false);
		boardState[coordX, coordY] = aiState;
		int result = checkWinner();

		if (result != -1)
		{
			if (result == 10) {

				Debug.Log("Computer Wins"); 
				onPlayerWin.Invoke(1);

			} 
			else onPlayerWin.Invoke(-1);
			return;

		}
		currentTurn = TurnState.player;

	}



	/* Summary
	 * PlayerSelects is a public function that doesn't return anything
	 * Info: This function is called by ClickTrigger.cs to detect the move by the player 
	 * Functionality: Displays the playerState and updates state variables and returns if game is over
	 */
	public void PlayerSelects(int coordX, int coordY)
	{
		SetVisual(coordX, coordY, playerState);
		freeSpaces.Remove(coordX * 3 + coordY);
		boardState[coordX, coordY] = playerState;
		int result = checkWinner();
		if (result != -1)
		{
			if (result == -10) { 
				currentTurn=TurnState.ai;
				onPlayerWin.Invoke(0);
			}
			else onPlayerWin.Invoke(-1);
			return;
		}
		currentTurn = TurnState.ai;
		StartCoroutine(WaitForSomeTime());

	}



	/* Summary
	 * WaitForSomeTime is an IEnumerator that runs asynchronous code
	 * Functionality: Waits 0.5s so that the player's move gets rendered before AI makes it's move
	 */
	IEnumerator WaitForSomeTime()
	{
		yield return new WaitForSeconds(0.5f);
		if (_aiLevel == 0)NoobMode();
		else Beastmode();
	}



	/* Summary
	 * SetVisual is a private function that doesn't return anything
	 * Functionality: Renders the X and O prefab in the game view when player and AI choose their moves
	 */
	private void SetVisual(int coordX, int coordY, TicTacToeState targetState)
	{
		Instantiate(
			targetState == TicTacToeState.circle ? _oPrefab : _xPrefab,
			_triggers[coordX, coordY].transform.position,
			Quaternion.identity
		);
	}
}
