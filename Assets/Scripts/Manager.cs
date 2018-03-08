using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameMode { SinglePlayer, TwoPlayer }

public class Manager : MonoBehaviour
{
    // --- External variables

    /// <summary>
    /// Instance of Player used to define Player 1.
    /// </summary>
    public Player Player1;

    /// <summary>
    /// Instance of Player used to define Player 2.
    /// </summary>
    public Player Player2;

    /// <summary>
    /// Instance of Button used to control audio.
    /// </summary>
    [SerializeField] private Button _audioButton;

    /// <summary>
    /// Instance of Text used to display the score of X.
    /// </summary>
    [SerializeField] private Text _xScoreText;

    /// <summary>
    /// Instance of Text used to display the score of O.
    /// </summary>
    [SerializeField] private Text _oScoreText;

    /// <summary>
    /// Instance of Text used to display the current player.
    /// </summary>
    [SerializeField] private Text _currentPlayerText;

    /// <summary>
    /// Instance of Text used to display the game's status.
    /// </summary>
    [SerializeField] private Text _gameStatusText;

    // --- Variables

    /// <summary>
    /// Instance of AudioSource.
    /// </summary>
    private AudioSource _audioSource;

    /// <summary>
    /// Instance of Board.
    /// </summary>
    private Board _board;

    /// <summary>
    /// The colour used for the Cross marker.
    /// </summary>
    private readonly Color _crossColour = new Color(0f, 114/255f, 188/255f);

    /// <summary>
    /// The colour used for the nought marker.
    /// </summary>
    private readonly Color _noughtColour = new Color(211/255f, 53/255f, 53/255f);

    /// <summary>
    /// The AI difficulty level.
    /// </summary>
    public static Difficulty AIDifficulty = Difficulty.Easy;

    /// <summary>
    /// Defines the game mode.
    /// </summary>
    public static GameMode GameMode = GameMode.SinglePlayer;

    /// <summary>
    /// Used to determine whether or not the game is over.
    /// </summary>
    public bool GameOver;

    // --- Properties
    /// <summary>
    /// The current player.
    /// </summary>
    public Player CurrentPlayer { get; private set; }

    /// <summary>
    /// Stores the value of the GameStatus property.
    /// </summary>
    private string _gameStatus;
    /// <summary>
    /// The status of the current game.
    /// </summary>
    public string GameStatus
    {
        get { return _gameStatus; }
        set
        {
            // Set variable
            _gameStatus = value;
            // Update text
            _gameStatusText.text = _gameStatus;
            // Enable variable
            _gameStatusText.enabled = true;
            // Display text
            _gameStatusText.gameObject.SetActive(true);

            // Next game
            Invoke("NextGame", 2f);
        }
    }

    /// <summary>
    /// Stores the value of the XScore property.
    /// </summary>
    private int _xScore;
    /// <summary>
    /// The current score of the player using the X marker.
    /// </summary>
    public int XScore
    {
        get { return _xScore; }
        set
        {
            // Update score and text
            _xScore = value;
            _xScoreText.text = _xScore.ToString();
        }
    }

    /// <summary>
    /// Stores the value of the OScore property.
    /// </summary>
    private int _oScore;
    /// <summary>
    /// The current score of the player using the O marker.
    /// </summary>
    public int OScore {
        get { return _oScore; }
        set
        {
            _oScore = value;
            _oScoreText.text = _oScore.ToString();
        }
    }

    // --- Methods

    /// <summary>
    /// Starts the next game.
    /// </summary>
    public void NextGame()
    {
        // Reset game properties
        Wipe();
        // Swap players
        Invoke("SwapPlayer", 1f); // Delay prevents error
    }

    /// <summary>
    /// Starts a new game. (Wipes all previous information)
    /// </summary>
    /// <param name="singlePlayer"></param>
    public void NewGame(bool singlePlayer = true)
    {
        // Reset game manager
        Wipe(true);
    }
  
    /// <summary>
    /// Returns to the menu.
    /// </summary>
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    /// <summary>
    /// Tasks undertaken at the start of this instance.
    /// </summary>
    private void Start()
    {
        // Check game mode
        Player2.Automated = GameMode == GameMode.SinglePlayer;

        // Set variables
        Marker startingMarker = UnityEngine.Random.Range(0f, 1f) < 0.5f ? Marker.Cross : Marker.Nought;
        Player1.Marker = startingMarker;
        Player2.Marker = Player1.Marker == Marker.Cross ? Marker.Nought : Marker.Cross;
        System.Random random = new System.Random();
        CurrentPlayer = random.Next(0, 2) == 0 ? Player1 : Player2;

        // Get components
        _audioSource = GetComponent<AudioSource>();
        _board = GameObject.Find("Board").GetComponent<Board>();

        // Swap player
        Invoke("SwapPlayer", 0.1f); // Delay prevents NullReferenceException
    }

    /// <summary>
    /// Swaps the players.
    /// </summary>
    public void SwapPlayer()
    {
        // Update variable and text
        if (CurrentPlayer.Marker == Marker.Cross)
        {
            _currentPlayerText.color = _noughtColour;
            _currentPlayerText.text = "Noughts";
        }
        else
        {
            _currentPlayerText.color = _crossColour;
            _currentPlayerText.text = "Crosses";
        }

        // Swap players
        CurrentPlayer = CurrentPlayer.gameObject.name == "Player1" ? Player2 : Player1;

        // Check if new player is automated, if so, make a move
        if (CurrentPlayer.Automated)
            CurrentPlayer.AutoMove();
    }

    /// <summary>
    /// Mute/Unmute the audio.
    /// </summary>
    public void ToggleAudio()
    {
        // Mute/unmute audio source
        bool muted = !_audioSource.mute;
        _audioSource.mute = muted;

        // Toggle button colour
        _audioButton.image.color = muted ? Color.grey : Color.white;
    }

    /// <summary>
    /// Resets the board or game.
    /// </summary>
    public void Wipe(bool resetAll = false)
    {
        // Reset variables
        _gameStatusText.enabled = false;

        // Check if everything needs to be reset
        if (resetAll)
        {
            XScore = 0;
            OScore = 0;
        }

        // Reset board
        _board.Clear();
    }
}