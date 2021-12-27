using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static GameObject levelManager;

    [SerializeField] UnityEngine.UI.Text coinsTxt;

    public int level {get; private set;}
    public int startingTroops {get; private set;}
    public float productionSpeed {get; private set;}
    public int coins {get; private set;}
    public int troopUpgradeLvl {get; private set;}
    public int prodSpeedUpgradeLvl {get; private set;}
    public int offlineEarningsPerSecond {get; private set;}
    public int offlineEarningUpgradeLevel {get; private set;}

    public float offlineTime { get; private set; }

    void Awake()
    {
        if (levelManager == null)
        {
            levelManager = this.gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        offlineTime = ((float)(System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds) - PlayerPrefs.GetFloat(PrefsList.dateTimePref, 0.0f);

        level = PlayerPrefs.GetInt(PrefsList.levelPref, 1);
        startingTroops = PlayerPrefs.GetInt(PrefsList.startingTroopsPref, 10);
        troopUpgradeLvl = PlayerPrefs.GetInt(PrefsList.troopLvlPref, 1);
        productionSpeed = PlayerPrefs.GetFloat(PrefsList.productionSpeedPref, 1);
        prodSpeedUpgradeLvl = PlayerPrefs.GetInt(PrefsList.productionSpeedPref, 1);
        coins = PlayerPrefs.GetInt(PrefsList.coinsPref, 0);
        offlineEarningsPerSecond = PlayerPrefs.GetInt(PrefsList.offlineEarningPerSecPref, 1);
        offlineEarningUpgradeLevel = PlayerPrefs.GetInt(PrefsList.offlineEarningLvlPref, 1);
        coins += offlineEarningsPerSecond * (int)offlineTime;

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0) coinsTxt.text = "Coins: " + coins;

        PlayerPrefs.SetInt(PrefsList.coinsPref, coins);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCoins(int coinsToBeAdded)
    {
        coins += coinsToBeAdded;
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0) coinsTxt.text = "Coins: " + coins;
        PlayerPrefs.SetInt(PrefsList.coinsPref, coins);
    }

    public void IncrementLevel()
    {
        level++;
        PlayerPrefs.SetInt(PrefsList.levelPref, level);
    }

    public void IncrementStartingTroops()
    {
        startingTroops += 2;
        troopUpgradeLvl++;
        PlayerPrefs.SetInt(PrefsList.startingTroopsPref, startingTroops);
        PlayerPrefs.SetInt(PrefsList.troopLvlPref, troopUpgradeLvl);
    }

    public void IncrementProdSpeed()
    {
        productionSpeed += 0.15f;
        prodSpeedUpgradeLvl++;
        PlayerPrefs.SetFloat(PrefsList.productionSpeedPref, productionSpeed);
        PlayerPrefs.SetInt(PrefsList.prodSpeedLvlPref, prodSpeedUpgradeLvl);
    }

    public void IncrementOfflineCoinEarning()
    {
        offlineEarningsPerSecond++;
        offlineEarningUpgradeLevel++;
        PlayerPrefs.SetInt(PrefsList.offlineEarningPerSecPref, offlineEarningsPerSecond);
        PlayerPrefs.SetInt(PrefsList.offlineEarningLvlPref, offlineEarningUpgradeLevel);
    }

    private void OnApplicationQuit()
    {
        float timeToBeSaved = (float)(System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;

        PlayerPrefs.SetFloat(PrefsList.dateTimePref, timeToBeSaved);
        PlayerPrefs.SetInt(PrefsList.coinsPref, coins);
    }
}

public static class PrefsList
{
    public static string levelPref = "Level";
    public static string coinsPref = "Coins";
    public static string startingTroopsPref = "StartingTroops";
    public static string troopLvlPref = "TroopLvl";
    public static string productionSpeedPref = "ProductionSpeed";
    public static string prodSpeedLvlPref = "ProdSpeedLvl";
    public static string offlineEarningPerSecPref = "OfflineEarningPerSec";
    public static string offlineEarningLvlPref = "OfflineEarningLvl";
    public static string dateTimePref = "DateTime";
}
