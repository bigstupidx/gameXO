using UnityEngine;
using UnityEngine.UI;

public class BoardSpace : MonoBehaviour
{
    // --- External variables

    /// <summary>
    /// Instance of AudioSource.
    /// </summary>
    [SerializeField] private AudioSource _audioSource;

    /// <summary>
    /// Instance of AudioClip.
    /// </summary>
    [SerializeField] private AudioClip _placementSound;

    // --- Variables

    /// <summary>
    /// Gets a value indicating whether this <see cref="BoardSpace"/> is occupied.
    /// </summary>
    /// <value>
    ///   <c>true</c> if occupied; otherwise, <c>false</c>.
    /// </value>
    public bool Occupied { get; private set; }

    /// <summary>
    /// Stores the value of the Marker property.
    /// </summary>
    private Marker _marker;
    /// <summary>
    /// Gets or sets the marker occupying the space.
    /// </summary>
    /// <value>
    /// The marker.
    /// </value>
    public Marker Marker
    {
        get { return _marker; }
        set
        {
            if (value != Marker.None)
                Occupied = true;

            _marker = value;
        }
    }

    /// <summary>
    /// Animator component.
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// Image component.
    /// </summary>
    private Image _image;

    // --- Functions

    /// <summary>
    /// Places a marker.
    /// </summary>
    /// <param name="marker">The marker.</param>
    /// <returns></returns>
    public bool PlaceMarker(Marker marker)
    {
        // Check if any animations are playing or marker type is none
        if (marker == Marker.None)
            return false;

        // Set marker type
        Marker = marker;
        // Set sprite
        _image.sprite = Marker == Marker.Cross ? Board.CrossSprite : Board.NoughtSprite;
        // Set colour
        _image.color = Color.white;
        // Set space as occupied
        Occupied = true;
        // Set animation
        _animator.SetBool("markerPlace", true);
        // Play sound effect
        _audioSource.clip = _placementSound;
        _audioSource.Play();

        return true;
    }

    // --- Methods

    /// <summary>
    /// Clears the board space.
    /// </summary>
    public void Clear()
    {
        // Clear board space
        Occupied = false;
        Marker = Marker.None;

        // Set animation
        _animator.SetBool("markerClear", true);
    }

    /// <summary>
    /// Resets the animation variables.
    /// </summary>
    public void ResetAnimationVariables()
    {
        _animator.SetBool("markerPlace", false);
        _animator.SetBool("markerClear", false);
    }

    /// <summary>
    /// Tasks undertaken at the start of this instance.
    /// </summary>
    private void Start()
    {
        // Get components
        _animator = GetComponent<Animator>();
        _image = GetComponent<Image>();
    }
}