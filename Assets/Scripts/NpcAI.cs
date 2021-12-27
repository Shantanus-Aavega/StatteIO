using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NpcAI : MonoBehaviour
{
    [SerializeField] GameObject statesHolder;
    //[SerializeField] bool _raiding;
    public List <EnemyState> enemyStates;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RaidTimer());
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyStates.Count != 0) RemoveCapturedStateFromList();
    }

    void SelectStateToRaid()
    {
        if (enemyStates.Count == 0)
        {
            StopCoroutine(RaidTimer());
            return;
        }

        foreach (EnemyState enemyState in enemyStates)
        {           
            
            GameObject raider = enemyState.capturedStates[0];

            for (int i = 0; i < enemyState.capturedStates.Count; i++)
            {
                if (raider.GetComponent<StateDetails>().troopsStationed < enemyState.capturedStates[i].GetComponent<StateDetails>().troopsStationed) 
                    raider = enemyState.capturedStates[i];
            }

            if (!enemyState._isRaiding)
            {
                for (int i = 0; i < statesHolder.transform.childCount; i++)
                {
                    if (!enemyState.capturedStates.Contains(statesHolder.transform.GetChild(i).gameObject) 
                        && statesHolder.transform.GetChild(i).gameObject.GetComponent<StateDetails>().troopsStationed < raider.GetComponent<StateDetails>().troopsStationed)
                    {
                        AttemptRaid(raider, statesHolder.transform.GetChild(i).gameObject, enemyState);
                        break;
                    }
                }
            }
        }
    }

    void AttemptRaid(GameObject raider, GameObject target, EnemyState raiderState)
    {
        raiderState._isRaiding = true;
        raiderState.capturedStates.Add(target);
        if (gameObject.GetComponent<Gameplay>().playerRegions.Contains(target)) gameObject.GetComponent<Gameplay>().playerRegions.Remove(target);
        target.GetComponent<StateDetails>().currentRuler = raiderState.ruler;
        target.GetComponent<StateDetails>().troopsStationed = raider.GetComponent<StateDetails>().troopsStationed - target.GetComponent<StateDetails>().troopsStationed;
        target.GetComponent<SpriteRenderer>().DOColor(gameObject.GetComponent<Gameplay>().enemyColors[enemyStates.IndexOf(raiderState)], .5f).OnComplete(delegate 
        {
            raiderState._isRaiding = false;
            if (gameObject.GetComponent<Gameplay>().playerRegions.Contains(target)) gameObject.GetComponent<Gameplay>().playerRegions.Remove(target);
        });
    }

    void RemoveCapturedStateFromList()
    {
        if (enemyStates.Count == 0) return;

        for(int n = 0; n<enemyStates.Count;n++)
        {
            if (enemyStates.Count == 0) return;

            EnemyState state = enemyStates[n];

            for (int i = 0; i< state.capturedStates.Count; i++)
            {
                if (state.capturedStates[i].GetComponent<StateDetails>().currentRuler != state.ruler)
                {
                    state.capturedStates.Remove(state.capturedStates[i]);
                }
            }

            if (state.capturedStates.Count == 0)
            {
                enemyStates.Remove(state);
            }
        }
    }

    IEnumerator RaidTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            SelectStateToRaid();
        }
    }
}


[System.Serializable]
public class EnemyState
{
    public Ruler ruler;
    public List<GameObject> capturedStates;
    public bool _isRaiding;
}
