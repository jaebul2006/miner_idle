using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMgr : MonoBehaviour 
{

    void Start()
    {
    }

	public void GoldGetEff(int get_gold)
    {
        GameObject homing_gold = Instantiate(Resources.Load("Prefabs/HomingGold")) as GameObject;
        homing_gold.GetComponent<HomingGold>().AddGold(get_gold);
    }

}
