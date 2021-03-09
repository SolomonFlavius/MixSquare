using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

[RequireComponent (typeof (Button))]
public class RewardedAdsButton : MonoBehaviour, IUnityAdsListener {

    #if UNITY_IOS
    private string gameId = "3940435";
    #elif UNITY_ANDROID
    private string gameId = "3940434";
    #endif

    Button myButton;
    [SerializeField]
    public GameManager GameManager;
    [SerializeField]
    public Timer timer;
    public string myPlacementId = "rewardedVideo";
    private static bool isInitialized = false;

    void Start () {   
        myButton = GetComponent <Button> ();

        // Initialize the Ads listener and service:
        if (!isInitialized) {
            isInitialized = true;
            Advertisement.AddListener(this);
            Advertisement.Initialize (gameId, false);
        }
        // Set interactivity to be dependent on the Placement’s status:
        myButton.interactable = Advertisement.IsReady (myPlacementId); 
        myButton.onClick.RemoveAllListeners();
        // Map the ShowRewardedVideo function to the button’s click listener:
        if (myButton) myButton.onClick.AddListener (ShowRewardedVideo);
        
        
    }

    // Implement a function for showing a rewarded video ad:
    void ShowRewardedVideo () {
        Advertisement.Show (myPlacementId);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsReady (string placementId) {
        // If the ready Placement is rewarded, activate the button:
        if (placementId == myPlacementId) {     
            myButton.interactable = true;
        }
    }

    public void OnUnityAdsDidFinish (string placementId, ShowResult showResult) {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished) {
        	GameManager.Continue();
        	Debug.Log("reward life");
            // Reward the user for watching the ad to completion.
        } else if (showResult == ShowResult.Skipped) {
            // Do not reward the user for skipping the ad.
        } else if (showResult == ShowResult.Failed) {
            Debug.LogWarning ("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsDidError (string message) {
        // Log the error.
    }

    public void OnUnityAdsDidStart (string placementId) {
    	timer.ok = false;
 		GameManager.InitializeButtons();
        // Optional actions to take when the end-users triggers an ad.
    } 
}