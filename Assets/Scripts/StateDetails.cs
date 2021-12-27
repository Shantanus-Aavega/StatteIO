using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Ruler
{
    None,
    Player,
    Enemy1,
    Enemy2,
    Enemy3,
    Enemy4,
    Enemy5,
    Enemy6,
    Enemy7,
    Enemy8,
    Enemy9,
    Enemy10
}

public class StateDetails : MonoBehaviour
{
    public Ruler currentRuler;
    public int troopsStationed;

    [SerializeField] UnityEngine.UI.Text troopTxt;

    void Start()
    {
        troopTxt.text = troopsStationed.ToString();
        StartCoroutine(IncrementTroops());
    }

    // Update is called once per frame
    void Update()
    {
        troopTxt.text = troopsStationed.ToString();
    }

    IEnumerator IncrementTroops()
    {
        while (troopsStationed != 100)
        {            
            if (currentRuler != Ruler.None && currentRuler != Ruler.Player)
            {
                yield return new WaitForSeconds(2.5f);
                troopsStationed += 1;
            }

            else if (currentRuler == Ruler.Player)
            {
                yield return new WaitForSeconds(1f);
                troopsStationed += 1;
            }

            else
            {
                yield return new WaitForSeconds(1f);
            }
        }        
    }
}
