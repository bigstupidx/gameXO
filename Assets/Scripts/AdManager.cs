using UnityEngine;
using admob;

public class AdManager : MonoBehaviour
{
    // --- Variables

    /// <summary>
    /// The banner identifier.
    /// </summary>
    private const string BannerId = "ca-app-pub-6762597481863539/6026430848";

    // --- Properties

    /// <summary>
    /// Value of the Instance property.
    /// </summary>
    private static AdManager _instance;
    /// <summary>
    /// Singleton instance.
    /// </summary>
    /// <value>
    /// The instance.
    /// </value>
    public static AdManager Instance
	{
		get { return _instance ?? (_instance = new AdManager()); }
	}

    // --- Methods

    /// <summary>
    /// Tasks ran at the very start of this instance.
    /// </summary>
    private void Awake()
	{
		// Prevent object being destroyed
		DontDestroyOnLoad(gameObject);

		// Instantiate admob
		Admob.Instance().initAdmob(BannerId, "");
		// Set properties
		//Admob.Instance().setTesting(true);
		Admob.Instance().setForChildren(true);
		// Show ad
		Admob.Instance().showBannerRelative(AdSize.Banner, AdPosition.BOTTOM_CENTER, 0);
	}
}