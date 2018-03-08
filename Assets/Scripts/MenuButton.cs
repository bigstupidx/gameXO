using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
	// --- External Variables

	/// <summary>
	/// Instance of Text.
	/// </summary>
	private Text _difficultyButtonText;

	// --- Methods

	/// <summary>
	/// Run during the very start of this instance.
	/// </summary>
	private void Awake()
	{
		_difficultyButtonText = GameObject.Find("DifficultyButton").GetComponent<Text>();

		if (!PlayerPrefs.HasKey("Difficulty")) {
			PlayerPrefs.SetString("Difficulty", "Easy");
		}
		// Set saved difficulty
		SetDifficulty((Difficulty)Enum.Parse(typeof(Difficulty), PlayerPrefs.GetString("Difficulty")));
	}

	/// <summary>
	/// Changes the AI's difficulty to the next level up.
	/// </summary>
	public void ChangeDifficulty()
	{
		// Get button text
		string buttonText = _difficultyButtonText.text.Normalize();
		buttonText = buttonText.Replace("\n", "");
        
		// Change button text
		if (buttonText == Difficulty.Easy.ToString())
			SetDifficulty(Difficulty.Average);
		else if (buttonText == Difficulty.Average.ToString())
			SetDifficulty(Difficulty.Hard);
		else if (buttonText == Difficulty.Hard.ToString())
			SetDifficulty(Difficulty.Impossible);
		else if (buttonText == Difficulty.Impossible.ToString())
			SetDifficulty(Difficulty.Easy);
	}

	/// <summary>
	/// Sets the AI's difficulty level.
	/// </summary>
	public void SetDifficulty(Difficulty difficulty)
	{
		switch (difficulty) {
			case Difficulty.Easy:
				_difficultyButtonText.text = "\nEasy";
				Manager.AIDifficulty = Difficulty.Easy;
				_difficultyButtonText.color = Color.green;
				break;
			case Difficulty.Average:
				_difficultyButtonText.text = "\nAverage";
				Manager.AIDifficulty = Difficulty.Average;
				_difficultyButtonText.color = Color.yellow;
				break;
			case Difficulty.Hard:
				_difficultyButtonText.text = "\nHard";
				Manager.AIDifficulty = Difficulty.Hard;
				_difficultyButtonText.color = Color.red;
				break;
			case Difficulty.Impossible:
				_difficultyButtonText.text = "\nImpossible";
				Manager.AIDifficulty = Difficulty.Impossible;
				_difficultyButtonText.color = Color.magenta;
				break;
		}

		// Save to PlayerPrefs
		PlayerPrefs.SetString("Difficulty", difficulty.ToString());
		PlayerPrefs.Save();
	}

	/// <summary>
	/// Starts the game.
	/// </summary>
	/// <param name="singlePlayer">if set to <c>true</c> [single player].</param>
	public void StartGame(bool singlePlayer)
	{
		// Set game type and difficulty
		if (singlePlayer) {
			Manager.GameMode = GameMode.SinglePlayer;
		} else {
			Manager.GameMode = GameMode.TwoPlayer;
		}
		// Load scene
		SceneManager.LoadScene("Game");
	}
}