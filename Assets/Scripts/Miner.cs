using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : MonoBehaviour
{
    float _move_speed = 1.0f;
	float DEFAULT_SPEED = 1.0f;
	float GET_ESCALATOR_SPEED = 1.3f;

    public tk2dSprite _tkspr;
	public tk2dSpriteAnimator _tkanim;

	int _level = 1;
    GameObject _myobj_menu;
    int _idx;
    private tk2dSprite _tk_label_spr;
    MinerMgr _miner_mgr;
    HomingMgr _homing_mgr;

    enum State
    {
        ToCart = 0,
        ToMine,
		Stunned,
    }

    private State _state = State.ToCart;
	private State _prev_state;
	NumberRollingMgr _num_rolling_mgr;

	int MAX_LEVEL = 20;
	float _life_time = 0f;
	float MAX_LIFE_TIME = 60f; // 1분간 활동하면 기절상태에 빠지고 터치하면 다시 일어난다. 

	bool _is_protect_stun = false; // 스턴방지용 약을 복용하였는가.
	bool _is_party_ticket = false; // 파티티켓 스피드 상승 효과.

	float WARP_COOL_TIME = 5f;
	float _cur_warp_cool_time = 0f;

	float CLOSEST_CART_X = -0.76f;
	float CLOSEST_MINE_X = 5.17f;


    void Start()
    {
        _tkspr.FlipX = false;
        _miner_mgr = GameObject.Find("MinerMgr").GetComponent<MinerMgr>();
        _homing_mgr = GameObject.Find("HomingMgr").GetComponent<HomingMgr>();
		_num_rolling_mgr = GameObject.Find ("NumberRollingMgr").GetComponent<NumberRollingMgr> ();
		_life_time = MAX_LIFE_TIME;
		_prev_state = _state;
		if (_miner_mgr.IsGetEscalator()) 
		{
			UseEscalator ();
		}

    }

    void Update()
    {
		/*
		for (var i = 0; i < Input.touchCount; ++i) {
			if (Input.GetTouch(i).phase == TouchPhase.Began) {
				RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position), Vector2.zero);
				// RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
				if(hitInfo)
				{
					Debug.Log( hitInfo.transform.gameObject.name );
					// Here you can check hitInfo to see which collider has been hit, and act appropriately.
				}
			}
		}*/

		if (Input.GetMouseButtonDown (0) && _state == State.Stunned) 
		{
			Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			RaycastHit2D hitInfo = Physics2D.Raycast(GameObject.Find("GameCamera").GetComponent<Camera>().ScreenToWorldPoint(pos), Vector2.zero);
			if(hitInfo)
			{
				if (hitInfo.transform.gameObject.name.StartsWith ("miner")) 
				{
					StopStun ();
				}
			}
		}


    }
		
	void StopStun()
	{
		_state = _prev_state;
		_prev_state = _state;
		_tkanim.Play ();
		_life_time = MAX_LIFE_TIME;
	}

	void FixedUpdate()
	{
		if (_life_time < 0 && (_state != State.Stunned)) 
		{
			_prev_state = _state;
			_state = State.Stunned;
			_tkanim.Stop ();
		} 
		else 
		{
			if(!_is_protect_stun)
				_life_time -= Time.fixedDeltaTime;
		}

		if (_is_party_ticket) 
		{
			_move_speed = (DEFAULT_SPEED * 2f);
		} 
		else 
		{
			_move_speed = DEFAULT_SPEED;
		}

		if (_state == State.Stunned)
			return;

		TryWarp ();

		float add_speed = _miner_mgr.GetAdditionalSpeed (_move_speed);
		_move_speed += add_speed;

		switch(_state)
		{
		case State.ToCart:
			gameObject.transform.localPosition -= new Vector3(Time.deltaTime * _move_speed, 0f, 0f);
			break;
		case State.ToMine:
			gameObject.transform.localPosition += new Vector3(Time.deltaTime * _move_speed, 0f, 0f);
			break;
		}

	}

    public void SetMyIdx(int id)
    {
        _idx = id;
        SetMyLabel();
    }

    public void SetMyLabel()
    {
        string label_name = (_idx + 5).ToString();
        label_name = label_name + "_miner" + _idx;
        _myobj_menu = GameObject.Find(label_name);
        _myobj_menu.GetComponent<UILabel>().text = "광부" + _idx;
        _tk_label_spr = _myobj_menu.transform.GetChild(0).GetComponent<tk2dSprite>();
        _tk_label_spr.transform.localScale = Vector3.one;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.name == "cart")
        {
            _state = State.ToMine;
            _tkspr.FlipX = true;
            int get_gold = _miner_mgr.GetPerGold(_level);
			int add_gold = _miner_mgr.GetAdditionalGold (get_gold);
			get_gold += add_gold;
			Debug.Log ("획득골드:" + get_gold);
            _homing_mgr.GoldGetEff();
			_num_rolling_mgr.AddGold(get_gold);
        }

        if(collision.collider.name == "mine")
        {
            _state = State.ToCart;
            _tkspr.FlipX = false;
        }
    }

    public int GetLevel()
    {
        return _level;
    }

    public void LevelUp()
    {
		if (_level < MAX_LEVEL) 
		{
			_level++;
		} 
    }

    public void SetSpeed(float spd)
    {
        _move_speed = spd;
    }

	public bool IsMaxLevel()
	{
		if (_level >= MAX_LEVEL) 
		{
			return true;
		}
		return false;
	}

	public void UseProtectStunPotion()
	{
		_is_protect_stun = true;
	}

	public void EndProtectStunPotion()
	{
		_is_protect_stun = false;
	}

	public void UsePartyTicket()
	{
		_is_party_ticket = true;
	}

	public void EndPartyTicket()
	{
		_is_party_ticket = false;
	}

	// 영구적으로 기본 스피드가 30% 증가. 
	public void UseEscalator()
	{
		DEFAULT_SPEED = GET_ESCALATOR_SPEED;
	}

	private void TryWarp()
	{
		if (_miner_mgr.IsGetWarp ()) 
		{
			_cur_warp_cool_time += Time.fixedDeltaTime;
			/*if (_cur_warp_cool_time > WARP_COOL_TIME) 
			{
				_cur_warp_cool_time = 0f;
				int warp_chance = Random.Range (0, 10);
				if (warp_chance < 4) 
				{
					if (_state == State.ToCart) 
					{
					} 
					else if (_state == State.ToMine) 
					{
					}
				}
			}*/
			if (_cur_warp_cool_time > WARP_COOL_TIME) 
			{
				if (_state == State.ToCart) 
				{
					float c_v = gameObject.transform.localPosition.x;
					float test_v = c_v - 1f;
					if (test_v < CLOSEST_CART_X)
						test_v = CLOSEST_CART_X;
					gameObject.transform.localPosition = new Vector3(test_v, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
				} 
				else if (_state == State.ToMine) 
				{
					float c_v = gameObject.transform.localPosition.x;
					float test_v = c_v + 1f;
					if (test_v > CLOSEST_MINE_X) 
						test_v = CLOSEST_MINE_X;
					gameObject.transform.localPosition = new Vector3 (test_v, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
				}
				_cur_warp_cool_time = 0f;
			}
		}
	}

}
