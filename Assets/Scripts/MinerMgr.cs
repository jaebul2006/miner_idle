using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerMgr : MonoBehaviour 
{

    private int _miner_cnt;
    public Dictionary<int, Miner> _miners = new Dictionary<int, Miner>();
    Dictionary<int, int> _miner_recruit_price_table = new Dictionary<int, int>();
    Dictionary<int, int> _per_gold_by_miner_level_table = new Dictionary<int, int>();
    Dictionary<int, int> _miner_levelup_price_table = new Dictionary<int, int>();
    Dictionary<int, float> _miner_speed_table = new Dictionary<int, float>();
    Dictionary<int, int> _miners_speed_upgrade_price_table = new Dictionary<int, int>();
    Dictionary<int, int> _mountain_upgrade_price_table = new Dictionary<int, int>();

    NumberRollingMgr _num_rolling_mgr;

    private GameMgr _game_mgr;
    private float _cur_miners_speed = 1f;

    private int _miners_speed_level = 1;
    private int _mountain_level = 1;

    void Start () 
    {
        _game_mgr = GameObject.Find("GameMgr").GetComponent<GameMgr>();
        _num_rolling_mgr = GameObject.Find("NumberRollingMgr").GetComponent<NumberRollingMgr>();

        _miner_recruit_price_table.Add(0, 0);
        _miner_recruit_price_table.Add(1, 25);
        _miner_recruit_price_table.Add(2, 50);
        _miner_recruit_price_table.Add(3, 100);
        _miner_recruit_price_table.Add(4, 300);
        _miner_recruit_price_table.Add(5, 500);
        _miner_recruit_price_table.Add(6, 700);
        _miner_recruit_price_table.Add(7, 1000);
        _miner_recruit_price_table.Add(8, 1500);
        _miner_recruit_price_table.Add(9, 2500);
        _miner_recruit_price_table.Add(10, 3000);
        _miner_recruit_price_table.Add(11, 4500);
        _miner_recruit_price_table.Add(12, 6000);
        _miner_recruit_price_table.Add(13, 8000);
        _miner_recruit_price_table.Add(14, 10000);
        _miner_recruit_price_table.Add(15, 30000);
        _miner_recruit_price_table.Add(16, 50000);
        _miner_recruit_price_table.Add(17, 100000);
        _miner_recruit_price_table.Add(18, 300000);
        _miner_recruit_price_table.Add(19, 600000);

        _per_gold_by_miner_level_table.Add(1, 15);
        _per_gold_by_miner_level_table.Add(2, 25);
        _per_gold_by_miner_level_table.Add(3, 50);
        _per_gold_by_miner_level_table.Add(4, 75);
        _per_gold_by_miner_level_table.Add(5, 100);
        _per_gold_by_miner_level_table.Add(6, 150);
        _per_gold_by_miner_level_table.Add(7, 200);
        _per_gold_by_miner_level_table.Add(8, 300);
        _per_gold_by_miner_level_table.Add(9, 800);
        _per_gold_by_miner_level_table.Add(10, 1000);
        _per_gold_by_miner_level_table.Add(11, 1500);
        _per_gold_by_miner_level_table.Add(12, 2000);
        _per_gold_by_miner_level_table.Add(13, 2500);
        _per_gold_by_miner_level_table.Add(14, 3500);
        _per_gold_by_miner_level_table.Add(15, 4000);
        _per_gold_by_miner_level_table.Add(16, 4500);
        _per_gold_by_miner_level_table.Add(17, 5000);
        _per_gold_by_miner_level_table.Add(18, 5500);
        _per_gold_by_miner_level_table.Add(19, 6000);
        _per_gold_by_miner_level_table.Add(20, 7000);

        int default_miner_levelup_price = 50;
        int miner_levelup_price = 0;
        for (int i = 0; i < 20; i++)
        {
            miner_levelup_price = default_miner_levelup_price + (i * 50);
            _miner_levelup_price_table.Add(i + 1, miner_levelup_price);
        }

        float default_miner_speed = 1f;
        float miner_speed = 0f;
        for (int i = 0; i < 20; i++ )
        {
            miner_speed = default_miner_speed + (i * 0.05f);
            _miner_speed_table.Add(i + 1, miner_speed);
        }
        _cur_miners_speed = _miner_speed_table[1];
        _miners_speed_level = 1;

        int miners_speed_upgrade_price = 0;
        int default_miners_speed_upgrade_price = 300;
        for(int i=0; i<20; i++)
        {
            miners_speed_upgrade_price = default_miners_speed_upgrade_price + (default_miners_speed_upgrade_price * i);
            _miners_speed_upgrade_price_table.Add(i + 1, miners_speed_upgrade_price);
        }

        int mountain_upgrade_price = 0;
        int default_mountain_upgrade_price = 400;
        for(int i=0; i<20; i++)
        {
            mountain_upgrade_price = default_mountain_upgrade_price + (default_mountain_upgrade_price * i);
            _mountain_upgrade_price_table.Add(i + 1, mountain_upgrade_price);
        }
	}
	
	void Update () 
    {
	}

    public void AddMiner()
    {
        GameObject miner = Instantiate(Resources.Load("Prefabs/miner")) as GameObject;
        miner.name = "miner" + _miner_cnt;
        miner.transform.parent = GameObject.Find("Miners").transform;
        miner.transform.localPosition = new Vector3(3.58f, -1.93f, 0f);
        miner.transform.localScale = Vector3.one;
        _miners.Add(_miner_cnt, miner.GetComponent<Miner>());
        miner.GetComponent<Miner>().SetMyIdx(_miner_cnt);
        _miner_cnt++;
        UpdateMinerSpeed();
    }

    public void AddMinerSpeed()
    {
        _miners_speed_level++;
    }

    public void AddMountain()
    {
        _mountain_level++;
    }

    public int GetPerGold(int level)
    {
        return _per_gold_by_miner_level_table[level];
    }

    public void TouchMinerLabel(GameObject obj)
    {
        string[] temp = obj.name.Split('_');
        string[] temp2 = temp[1].Split('r');
        int idx = int.Parse(temp2[1]);
        if (_miners.ContainsKey(idx))
        {
            _game_mgr.PopupMinerInfoOpen(obj.name);
        }
        else 
        {
            Debug.Log("생성안되어있음");
        }
    }

    public int GetRecruitLevel()
    {
        return _miner_cnt;
    }

    public int GetSpeedLevel()
    {
        return _miners_speed_level;
    }

    public int GetMountainLevel()
    {
        return _mountain_level;
    }

    public int GetRecruitPrice(int key)
    {
        return _miner_recruit_price_table[key];
    }

    public int GetMinerLevelupPrice(int level)
    {
        return _miner_levelup_price_table[level];
    }

    public int GetTotalGold()
    {
        return _num_rolling_mgr.GetTotalGold();
    }

    public int GetSpeedPrice(int key)
    {
        return _miners_speed_upgrade_price_table[key];
    }

    public void UpdateMinerSpeed()
    {
        foreach(KeyValuePair<int, Miner>kv in _miners)
        {
            kv.Value.SetSpeed(_cur_miners_speed);
        }
    }

    public int GetMountainPrice(int key)
    {
        return _mountain_upgrade_price_table[key];
    }

}
