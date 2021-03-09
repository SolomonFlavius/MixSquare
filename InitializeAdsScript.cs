using UnityEngine;
using UnityEngine.Advertisements;

public class InitializeAdsScript : MonoBehaviour { 

    string gameId = "3940435";
    bool testMode = false;

    void Start () {
        Advertisement.Initialize (gameId, testMode);
    }
}