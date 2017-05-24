using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMgr : MonoBehaviour 
{

    void Start()
    {
    }

	public void GoldGetEff()
    {
        GameObject homing_gold = Instantiate(Resources.Load("Prefabs/HomingGold")) as GameObject;
    }

}
