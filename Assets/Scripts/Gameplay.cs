using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Gameplay : MonoBehaviour
{
    [SerializeField] Color playerColor;
    [SerializeField] bool _canSelectRegions, _isGameOver;
    [SerializeField] List<GameObject> selectedRegionsList;
    public List<Color> enemyColors;
    [SerializeField] int troopsToBeDeployed, levelScore;
    public List<GameObject> playerRegions, unruledRegions;
    [SerializeField] GameObject troopRingPrefab, resultUI, statesHolder, pauseBtn, lvlTxt;
    [SerializeField] LevelManager levMan;

    void Start()
    {
        levMan = GameObject.Find("LevelManager")?.GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckRegion();
        }

        if (Input.GetMouseButtonUp(0))
        {
            _canSelectRegions = false;
            AttemptRaid();
        }

        if (Input.GetMouseButton(0) && _canSelectRegions)
        {
            SelectRegions();
        }

        if (!_isGameOver)CheckGameOver();
    }

    void CheckRegion()
    {
        Vector3 raycastPos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -20);

        RaycastHit2D hit = Physics2D.Raycast(raycastPos, Vector3.forward, 30);

        if (hit && hit.collider.gameObject.GetComponent<StateDetails>().currentRuler == Ruler.Player)
        {
            _canSelectRegions = true;
        }
    }

    void SelectRegions()
    {
        Vector3 raycastPos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -20);

        RaycastHit2D hit = Physics2D.Raycast(raycastPos, Vector3.forward, 30);

        if (hit && hit.collider.gameObject.GetComponent<StateDetails>().currentRuler == Ruler.Player)
        {
            if (!selectedRegionsList.Contains(hit.collider.gameObject))
            {
                selectedRegionsList.Add(hit.collider.gameObject);
                troopsToBeDeployed += hit.collider.gameObject.GetComponent<StateDetails>().troopsStationed;
            }
        }
    }

    void AttemptRaid()
    {
        Vector3 raycastPos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -20);

        RaycastHit2D hit = Physics2D.Raycast(raycastPos, Vector3.forward, 30);

        if (hit && hit.collider.gameObject.GetComponent<StateDetails>().currentRuler == Ruler.Player)
        {
            if (!selectedRegionsList.Contains(hit.collider.gameObject))
            {
                hit.collider.gameObject.GetComponent<StateDetails>().troopsStationed += troopsToBeDeployed;

                for (int i = 0; i<selectedRegionsList.Count; i++)
                {
                    selectedRegionsList[i].GetComponent<StateDetails>().troopsStationed = 0;
                }
            }

            else if (selectedRegionsList.Contains(hit.collider.gameObject))
            {
                hit.collider.gameObject.GetComponent<StateDetails>().troopsStationed = troopsToBeDeployed;

                selectedRegionsList.Remove(hit.collider.gameObject);

                for (int i = 0; i < selectedRegionsList.Count; i++)
                {
                    selectedRegionsList[i].GetComponent<StateDetails>().troopsStationed = 0;
                }
            }

            foreach (GameObject region in selectedRegionsList)
            {
                GameObject troopRing = Instantiate(troopRingPrefab, region.transform.position, Quaternion.identity);

                troopRing.transform.DOMove(hit.collider.transform.position, .5f).OnComplete(delegate
                {
                    Destroy(troopRing);
                });
            }

            selectedRegionsList.Clear();
            troopsToBeDeployed = 0;
        }

        else if (hit && hit.collider.gameObject.GetComponent<StateDetails>().currentRuler != Ruler.Player)
        {
            if (hit.collider.gameObject.GetComponent<StateDetails>().troopsStationed >= troopsToBeDeployed)
            {
                hit.collider.gameObject.GetComponent<StateDetails>().troopsStationed -= troopsToBeDeployed;

                levelScore += (troopsToBeDeployed*50);

                foreach (GameObject region in selectedRegionsList)
                {
                    GameObject troopRing = Instantiate(troopRingPrefab, region.transform.position, Quaternion.identity);

                    troopRing.transform.DOMove(hit.collider.transform.position, .5f).OnComplete(delegate
                    {
                        Destroy(troopRing);
                    });
                }

                for (int i = 0; i < selectedRegionsList.Count; i++)
                {
                    selectedRegionsList[i].GetComponent<StateDetails>().troopsStationed = 0;
                }

                selectedRegionsList.Clear();
                troopsToBeDeployed = 0;
            }

            else if (hit.collider.gameObject.GetComponent<StateDetails>().troopsStationed < troopsToBeDeployed)
            {
                levelScore += ((hit.collider.gameObject.GetComponent<StateDetails>().troopsStationed) * 50);

                hit.collider.gameObject.GetComponent<StateDetails>().troopsStationed = troopsToBeDeployed - hit.collider.gameObject.GetComponent<StateDetails>().troopsStationed;
                hit.collider.gameObject.GetComponent<StateDetails>().currentRuler = Ruler.Player;
                playerRegions.Add(hit.collider.gameObject);

                float colorTransitionTime = troopsToBeDeployed;
                colorTransitionTime /= 50;

                foreach (GameObject region in selectedRegionsList)
                {
                    GameObject troopRing = Instantiate(troopRingPrefab, region.transform.position, Quaternion.identity);

                    troopRing.transform.DOMove(hit.collider.transform.position, colorTransitionTime).OnComplete(delegate
                    {
                        Destroy(troopRing);
                    });
                }

                for (int i = 0; i < selectedRegionsList.Count; i++)
                {
                    selectedRegionsList[i].GetComponent<StateDetails>().troopsStationed = 0;
                }

                hit.collider.gameObject.GetComponent<SpriteRenderer>().DOColor(playerColor, colorTransitionTime).OnComplete(delegate 
                {
                    selectedRegionsList.Clear();
                    troopsToBeDeployed = 0;
                    CheckVictory();
                });                
            }
        }
    }

    void CheckVictory()
    {
        if (gameObject.GetComponent<NpcAI>().enemyStates.Count == 0)
        {
            GameObject.Find("GoogleAdMan")?.GetComponent<GoogleAd>().ShowInterstialAD();
            GameObject.Find("GoogleAdMan")?.GetComponent<GoogleAd>().DestroyBannerAd();

            levMan.AddCoins(levelScore);
            levMan.IncrementLevel();            
            Time.timeScale = 0;
            resultUI.SetActive(true);
            pauseBtn.SetActive(false);
            gameObject.GetComponent<NpcAI>().enabled = false;
            statesHolder.SetActive(false);
            lvlTxt.SetActive(false);
            resultUI.transform.GetChild(0).gameObject.GetComponent<Text>().text = "You Won!!";
            resultUI.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Coins Earned: " + levelScore;
        }
    }

    void CheckGameOver()
    {
        if (playerRegions.Count == 0)
        {
            _isGameOver = true;

            GameObject.Find("GoogleAdMan")?.GetComponent<GoogleAd>().ShowInterstialAD();
            GameObject.Find("GoogleAdMan")?.GetComponent<GoogleAd>().DestroyBannerAd();

            levMan?.AddCoins(levelScore/10);           

            Time.timeScale = 0;
            resultUI.SetActive(true);
            pauseBtn.SetActive(false);
            gameObject.GetComponent<NpcAI>().enabled = false;
            statesHolder.SetActive(false);
            lvlTxt.SetActive(false);
            resultUI.transform.GetChild(0).gameObject.GetComponent<Text>().text = "You Lost!!";
            resultUI.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Coins Earned: " + (levelScore/10);
        }
    }
}
