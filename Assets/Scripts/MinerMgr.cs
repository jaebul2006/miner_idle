using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    int MAX_MINER = 20;
	int MAX_FOOD = 15;
	int MAX_MOUNTAIN = 15;

	int MAX_MINER_LEVEL = 20; // 광부의 다음레벨의 생산량을 확인하기 위해 필요하다.

	public float MAX_PROTECT_STUN_POTION_TIME_5 = 300f;
	public float MAX_PROTECT_STUN_POTION_TIME_15 = 900f;
	public UILabel _lb_protect_stun_potion_time;
	bool _is_protect_stun_potion = false;
	float _protect_stun_potion_time = 0f;

	public float MAX_PARTY_TIME1 = 60f;
	public float MAX_PARTY_TIME3 = 180f;
	public UILabel _lb_party_ticket_time;
	bool _is_party_ticket = false;
	float _party_ticket_time = 0f;

	private bool _isget_escalator = false;

	private bool _isget_warp = false;

	int ADDITIONAL_SPEED = 0;
	int ADDITIONAL_GOLD_PERCENT = 0;


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
	
	void FixedUpdate () 
    {
		if (_is_protect_stun_potion) 
		{
			int minuete = (int)(_protect_stun_potion_time / 60);
			int seconds = (int)(_protect_stun_potion_time % 60);
			_lb_protect_stun_potion_time.text = minuete.ToString ("00") + " : " + seconds.ToString ("00"); 
			_protect_stun_potion_time -= Time.fixedDeltaTime;
			if (_protect_stun_potion_time < 0f) 
			{
				EndProtectStunPotions ();
			}
		}

		if (_is_party_ticket) 
		{
			int minuete = (int)(_party_ticket_time / 60);
			int seconds = (int)(_party_ticket_time % 60);
			_lb_party_ticket_time.text = minuete.ToString ("00") + " : " + seconds.ToString ("00"); 
			_party_ticket_time -= Time.fixedDeltaTime;
			if (_party_ticket_time < 0f) 
			{
				EndPartyTickets();
			}
		}
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
			_game_mgr._popup_mgr.PopupAni ();
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

    // 지금 광부가 다음레벨이 됐을 때 한번의 생산량
    public int GetperGold(int levelplusone)
    {
        if(levelplusone <= MAX_MINER_LEVEL)
        {
            return _per_gold_by_miner_level_table[levelplusone];
        }
        return 0;
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

    // 광부가 20명이 다찼으면 true를 리턴
    public bool IsMaxMiner()
    {
        if(_miners.Count >= MAX_MINER)
        {
            return true;
        }
        return false;
    }

	//  급식 레벨이 15가 되면 true를 리턴.
	public bool IsMaxFood()
	{
		if (_miners_speed_level >= MAX_FOOD) 
		{
			return true;
		}
		return false;
	}

	// 광산 레벨이 15가 되면 true를 리턴.
	public bool IsMaxMountain()
	{
		if (_mountain_level >= MAX_MOUNTAIN) 
		{
			return true;
		}
		return false;
	}

	// 전체 광부들은 n 분동안 스턴 방지.
	public void UseProtectStunPotions(float ntime)
	{
		_is_protect_stun_potion = true;
		_protect_stun_potion_time = ntime;

		foreach(KeyValuePair<int, Miner>kv in _miners)
		{
			kv.Value.UseProtectStunPotion ();
		}
	}

	private void EndProtectStunPotions()
	{
		_lb_protect_stun_potion_time.text = "";
		_is_protect_stun_potion = false;
		foreach(KeyValuePair<int, Miner>kv in _miners)
		{
			kv.Value.EndProtectStunPotion ();
		}
	}

	public void UsePartyTickets(float ntime)
	{
		_is_party_ticket = true;
		_party_ticket_time = ntime;

		foreach(KeyValuePair<int, Miner>kv in _miners)
		{
			kv.Value.UsePartyTicket ();
		}
	}

	private void EndPartyTickets()
	{
		_lb_party_ticket_time.text = "";
		_is_party_ticket = false;
		foreach(KeyValuePair<int, Miner>kv in _miners)
		{
			kv.Value.EndPartyTicket ();
		}
	}

	public void UseEscalators()
	{
		_isget_escalator = true;

		foreach(KeyValuePair<int, Miner>kv in _miners)
		{
			kv.Value.UseEscalator ();
		}
	}

	public bool IsGetEscalator()
	{
		return _isget_escalator;
	}

	// 모든 광부들은 일정확률로 워프기능을 가진다.
	public void UseWarps()
	{
		_isget_warp = true;
	}

	public bool IsGetWarp()
	{
		return _isget_warp;
	}

	public void UseMagicPapers()
	{
		int add_v = UnityEngine.Random.Range (5, 16);
		ADDITIONAL_SPEED = add_v;
		ADDITIONAL_GOLD_PERCENT = add_v;
	}

	public float GetAdditionalSpeed(float move_spd)
	{
		float t = ((float)ADDITIONAL_SPEED) * 0.01f;
		float add_speed = t * move_spd;
		return add_speed;
	}

	public int GetAdditionalGold(int get_gold)
	{
		float t = ((float)ADDITIONAL_GOLD_PERCENT) * 0.01f;
		float t2 = t * (float)get_gold;
		int add_gold = (int)t2;
		return add_gold;
	}

	//  Auto Save Test
	void OnApplicationQuit()
	{
		string json_data = "";
		foreach (KeyValuePair<int, Miner>kv in _miners) 
		{
			MinerInfo m = new MinerInfo ();
			m.id = kv.Value._idx;
			m.level = kv.Value._level;
			m.position = kv.Value.transform.localPosition;
			m.state = kv.Value._state.ToString ();
			json_data += (JsonUtility.ToJson(m) + "\n");
		}
		Debug.Log (json_data);
	}

}
	
[Serializable]
public class MinerInfo
{
	public int id;
	public int level;
	public Vector3 position;
	public string state;
}