using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsUpgrade : MonoBehaviour
{
    int troopPrice, prodSpeedPrice, offlineEarningPrice;
    [SerializeField] LevelManager levMan;

    void Start()
    {
        levMan = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        troopPrice = 1000 + (levMan.troopUpgradeLvl * 5);
        prodSpeedPrice = 1000 + (levMan.prodSpeedUpgradeLvl * 5);
        offlineEarningPrice = 1000 + (levMan.offlineEarningUpgradeLevel * 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyTroopUpgrade()
    {
        if (levMan.coins >= troopPrice)
        {
            levMan.AddCoins(-troopPrice);
            levMan.IncrementStartingTroops();
            troopPrice = 1000 + (levMan.troopUpgradeLvl * 5);
        }
    }

    public void BuyProdSpeedUpgrade()
    {
        if (levMan.coins >= prodSpeedPrice)
        {
            levMan.AddCoins(-prodSpeedPrice);
            levMan.IncrementProdSpeed();
            prodSpeedPrice = 1000 + (levMan.prodSpeedUpgradeLvl * 5);
        }
    }

    public void BuyOfflineEarningUpgrade()
    {
        if (levMan.coins >= offlineEarningPrice)
        {
            levMan.AddCoins(-offlineEarningPrice);
            levMan.IncrementOfflineCoinEarning();
            offlineEarningPrice = 1000 + (levMan.offlineEarningUpgradeLevel * 5);
        }
    }
}
