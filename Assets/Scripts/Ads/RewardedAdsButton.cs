using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System;

[RequireComponent(typeof(Button))]
public class RewardedAdsButton : MonoBehaviour, IUnityAdsListener
{
#if UNITY_IOS
    private string gameId = "4203712";
#elif UNITY_ANDROID
    private string gameId = "4203713";
#elif UNITY_EDITOR
    private string gameId = "4203712";
#endif

    Button myButton;
#if UNITY_ANDROID
    private string mySurfacingId = "Rewarded_Android";
#elif UNITY_IOS
    private string mySurfacingId = "Rewarded_iOS";
#endif

    public enum Rewards { gold, cash, stamina }
    public Rewards rewards;
    public Text counterText;
    public string countDownTimer;
    [SerializeField]
    private bool starTimer, timeCompleted;
    float t;
    [Header("Set the timer in seconds here")]
    public float timer;
    private float resetTimer;
    public float goldAmount, cashAmount;

    [SerializeField]
    private bool withTimer;

    public DateTime loginTime;
    public DateTime quitTime;
    public DateTime lastLogin;
    TimeSpan dif;

    void OnValidate()
    {

        /*if(starTimer == true){
            Close();
            starTimer = false;
        }*/

        if (timeCompleted == true)
        {
            Close();
            timeCompleted = false;
        }
    }

    void Start()
    {


        myButton = GetComponent<Button>();

        // Set interactivity to be dependent on the Ad Unit or legacy Placement’s status:
        myButton.interactable = Advertisement.IsReady(mySurfacingId);

        // Map the ShowRewardedVideo function to the button’s click listener:
        if (myButton) myButton.onClick.AddListener(ShowRewardedVideo);

        // Initialize the Ads listener and service:
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, true);

        if (withTimer)
        {

            if (PlayerPrefs.GetInt("freeRewardAds", 0) == 0)
                timeCompleted = false;
            else
                timeCompleted = true;


            //starTimer = false;
            resetTimer = timer;

            loginTime = System.DateTime.UtcNow;
            lastLogin = System.DateTime.Parse(PlayerPrefs.GetString("Recharge_time", "Theres no date registered"));

            if (!timeCompleted)
                CalculateTimeDiferance();
        }
    }

    void FixedUpdate()
    {
        if (starTimer)
        {
            t = timer;
            timer -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(timer / 60F);
            int seconds = Mathf.FloorToInt(timer - minutes * 60);

            countDownTimer = string.Format("{0:00}:{1:00}", minutes, seconds);

            if (timer < 0)
            {
                timer = 0;
                timer = resetTimer;
                starTimer = false;
                timeCompleted = true;

            }

        }

        if (!starTimer)
        {
            myButton.interactable = true;
            counterText.text = "Pronto!";
        }
        else
        {
            myButton.interactable = false;
            counterText.text = "Aguarde: " + countDownTimer;
        }


    }

    // Implement a function for showing a rewarded video ad:
    void ShowRewardedVideo()
    {
        if (!starTimer)
            Advertisement.Show(mySurfacingId);

        if (!withTimer)
            Advertisement.Show(mySurfacingId);

    }

    public void Close()
    {
        PlayerPrefs.SetFloat("recharge", timer);

        if (timeCompleted)
            PlayerPrefs.SetInt("freeRewardAds", 1);
        else
            PlayerPrefs.SetInt("freeRewardAds", 0);

        quitTime = System.DateTime.UtcNow;
        PlayerPrefs.SetString("Recharge_time", quitTime.ToString());

    }

    public void CalculateTimeDiferance()
    {
        dif = loginTime.Subtract(lastLogin);
        Debug.Log("difference: " + dif.ToString());
        if (dif >= System.TimeSpan.FromMinutes(15))
        {
            starTimer = false;
        }
        else
        {
            t = PlayerPrefs.GetFloat("recharge", 0.0f);
            t -= (float)dif.TotalSeconds;
            timer = t;
            starTimer = true;
            timeCompleted = false;
        }
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsReady(string surfacingId)
    {
        // If the ready Ad Unit or legacy Placement is rewarded, activate the button: 
        if (surfacingId == mySurfacingId)
        {
            myButton.interactable = true;
        }
    }

    public void OnUnityAdsDidFinish(string surfacingId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            switch (rewards)
            {
                case Rewards.gold: { /*MoneyManager.AddMoney("gold", goldAmount);*/ break; }
                case Rewards.cash: { /*MoneyManager.AddMoney("cash", cashAmount);*/ break; }
                case Rewards.stamina: { /*EnergyManager.ResetEnergy();*/ break; }
            }
            if (withTimer)
            {
                starTimer = true;
                timeCompleted = false;
            }
            //Debug.LogWarning("Heres your reward! its nothing!!");
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error");
        }
    }

    public void PaidRechard()
    {

        /*if (MoneyManager.GetMoney("cash") < 100)
            return;
        MoneyManager.RemoveMoney("cash", 100);
        EnergyManager.ResetEnergy();*/
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string surfacingId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Close();
        }
    }

    private void OnApplicationQuit()
    {
        Close();
    }
}
