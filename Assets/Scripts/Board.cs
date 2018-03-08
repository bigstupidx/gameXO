using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to define the state of a game.
/// </summary>
public enum GameState { None, Tie, Lose, Win }

/// <summary>
/// Used to define the type of a playing piece.
/// </summary>
public enum Marker { None, Cross, Nought }

public class Board : MonoBehaviour
{
    // --- External Variables    

    /// <summary>
    /// Instance of AudioSource.
    /// </summary>
    [SerializeField]
    private AudioSource _audioSource;

    /// <summary>
    /// Instance of AudioClip.
    /// </summary>
    [SerializeField]
    private AudioClip _clearBoardSound;

    /// <summary>
    /// Instance of Manager.
    /// </summary>
    [SerializeField]
    private Manager _manager;

    // --- Variables

    /// <summary>
    /// The board spaces.
    /// </summary>
    private BoardSpace[] _boardSpaces = new BoardSpace[9];

    /// <summary>
    /// The sprites of playing pieces.
    /// </summary>
    public static Sprite CrossSprite, NoughtSprite;

    /// <summary>
    /// The board spaces as Markers, used as an efficent way of calculating the current state of the game.
    /// </summary>
    public Marker[] Spaces = new Marker[9];

    // --- Functions

    /// <summary>
    /// Processes the board.
    /// </summary>
    /// <param name="Spaces">The spaces being processed.</param>
    /// <param name="perspective">The perspective to view the game from.</param>
    /// <returns>The game state.</returns>
    public static GameState Process(Marker[] Spaces, Marker perspective)
    {
        // Return game state

        // Horizontal
        for (int i = 0; i < 7; i += 3)
            if ((Spaces[i] == Marker.Cross || Spaces[i] == Marker.Nought) &&
                Spaces[i] == Spaces[i + 1] && Spaces[i + 1] == Spaces[i + 2])
            {
                return Spaces[i] == perspective ? GameState.Win : GameState.Lose;
            }

        // Vertical
        for (int i = 0; i < 3; i++)
            if ((Spaces[i] == Marker.Cross || Spaces[i] == Marker.Nought) &&
                Spaces[i] == Spaces[i + 3] && Spaces[i + 3] == Spaces[i + 6])
            {
                return Spaces[i] == perspective ? GameState.Win : GameState.Lose;
            }

        // Diagonal
        if ((Spaces[4] == Marker.Cross || Spaces[4] == Marker.Nought) &&
            (Spaces[0] == Spaces[4] && Spaces[4] == Spaces[8] ||
             Spaces[2] == Spaces[4] && Spaces[4] == Spaces[6]))
        {
            return Spaces[4] == perspective ? GameState.Win : GameState.Lose;
        }

        // Check for tie
        int occupiedCounter = Spaces.Count(marker => marker == Marker.Cross || marker == Marker.Nought);

        return occupiedCounter == 9 ? GameState.Tie : GameState.None;
    }

    // --- Methods

    /// <summary>
    /// Tasks undertaken at the start of this instance.
    /// </summary>
    private void Start()
    {
        // Get assets
        CrossSprite = Resources.Load<Sprite>("Images/cross");
        NoughtSprite = Resources.Load<Sprite>("Images/nought");

        // Get board spaces
        int childCounter = 0;
        foreach (Transform child in transform)
        {
            _boardSpaces[childCounter] = child.GetComponent<BoardSpace>();
            childCounter++;
        }
    }

    /// <summary>
    /// Places a marker at the specified position.
    /// </summary>
    /// <param name="position">The position.</param>
    public void PlaceMarker(int position)
    {
        BoardSpace currentBoardSpace = _boardSpaces[position];

        /* Check if the game is already over
           Check if space is occupied
           Check if the clear animation has completed */
        if (_manager.GameOver || currentBoardSpace.Occupied)
            return;

        // Place marker
        Marker currentPlayerMarker = _manager.CurrentPlayer.Marker;

        // Set space
        if (currentBoardSpace.PlaceMarker(currentPlayerMarker))
            Spaces[position] = currentPlayerMarker;
        else
            return;

        // Check for winner
        Evaluate();

        // Swap player
        if (!_manager.GameOver)
            _manager.SwapPlayer();
    }

    /// <summary>
    /// Clears the board.
    /// </summary>
    public void Clear()
    {
        // Clear board spaces
        foreach (BoardSpace space in _boardSpaces)
            space.Clear();

        // Reset spaces
        Array.Clear(Spaces, 0, 9);

        // Play sound effect
        _audioSource.clip = _clearBoardSound;
        _audioSource.Play();

        // Reset GameOver (Use same delay as animation)
        Invoke("ResetGameOver", 2.0f);
    }

    /// <summary>
    /// Resets the GameOver variable. (This implementation allows for delayed animations at the end of the game.)
    /// </summary>
    private void ResetGameOver()
    {
        _manager.GameOver = false;
    }

    /// <summary>
    /// Evaluates the state of the board.
    /// </summary>
    public void Evaluate()
    {
        // Check game state
        GameMode gameMode = Manager.GameMode;
        GameState gameState = Process(Spaces, _manager.Player1.Marker);
        Player currentPlayer = _manager.CurrentPlayer;

        if (gameState == GameState.Tie)
        {
            GameOver("Tie");
        }
        else if (gameState != GameState.None)
        {
            // Check who won/lost
            if (gameMode == GameMode.SinglePlayer)
                GameOver(gameState == GameState.Win ? "You Win!" : "You Lose!");
            else
                GameOver(currentPlayer.Marker + " Wins!");

            // Update score
            currentPlayer.Score++;
        }
    }

    /// <summary>
    /// End the current game.
    /// </summary>
    /// <param name="msg">The message to be displayed.</param>
    private void GameOver(string msg)
    {
        _manager.GameStatus = msg;
        _manager.GameOver = true;
    }
}