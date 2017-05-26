using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour {

    MinerMgr _miner_mgr;
    Transform _popup_tm; // 공용팝업
    Transform _popup_trader; // 떠돌이 상인 전용팝업

    public UILabel _lb_title;
    public UILabel _lb_pop_level;
    public UILabel _lb_pop_content;
    public UILabel _lb_pop_price;
    public UILabel _lb_pop_price_head;
    public UILabel _lb_pop_price_lack;
    public UILabel _lb_pop_price_lack_head;

    private NumberRollingMgr _num_rolling_mgr;
    private int _level;
    private int _need_price;
    private int _cur_gold;
    private int _lack_gold;

    private int _cur_selected_miner_idx;

    public GameObject _first_panel;

    enum PopState
    {
        MINER_RECRUIT = 0,
        FOOD,
        MOUNTAIN,
        TRADER,
        TRADER_DETAIL,
        MINER_INFO,
    }
    private PopState _ePop_state = PopState.MINER_RECRUIT;

	public PopupMgr _popup_mgr;

    int ENERGY_DRINK_FIVE_PRICE = 5000;
    int ENERGY_DRINK_FIFTEEN_PRICE = 13000;
    int PARTY_TICKET = 8000;
    int PARTY_TICKET_THREE = 20000;
    int ESCALATOR = 10000000;
    int WARP = 30000000;
    int MAGIC_PAPER = 99999999;

    void Start ()
    {
        _miner_mgr = GameObject.Find("MinerMgr").GetComponent<MinerMgr>();
        _popup_tm = GameObject.Find("PopWnd").transform;
        _num_rolling_mgr = GameObject.Find("NumberRollingMgr").GetComponent<NumberRollingMgr>();
        _popup_trader = GameObject.Find("PopTrader").transform;
	}
	
	void Update ()
    {
        int total_gold = _num_rolling_mgr.GetTotalGold();
        switch(_ePop_state)
        {
            case PopState.MINER_RECRUIT:
                SetRecruitPrice();
                break;

            case PopState.FOOD:
                SetFoodPrice();
                break;

            case PopState.MOUNTAIN:
                SetMountainPrice();
                break;

            case PopState.TRADER:
                break;

            case PopState.TRADER_DETAIL:
                break;

            case PopState.MINER_INFO:
                SetMinerInfo();
                break;
        }
    }

    public void TouchFirstPopupOK()
    {
        _first_panel.SetActive(false);
    }

    public void PopupMinerRecruitOpen(string menu_name)
    {
        _ePop_state = PopState.MINER_RECRUIT;
        _popup_tm.localScale = Vector3.one;
        SetRecruitPrice();
    }

    public void PopupFoodOpen(string menu_name)
    {
        _ePop_state = PopState.FOOD;
        _popup_tm.localScale = Vector3.one;
        SetFoodPrice();
    }

    public void PopupMountainOpen(string menu_name)
    {
        _ePop_state = PopState.MOUNTAIN;
        _popup_tm.localScale = Vector3.one;
        SetMountainPrice();
    }

    public void PopupMinerInfoOpen(string name)
    {
        string[] temp = name.Split('r');
        _cur_selected_miner_idx = int.Parse(temp[1]);
        _ePop_state = PopState.MINER_INFO;
        _popup_tm.localScale = Vector3.one;
        SetMinerInfo();
    }

    private void SetRecruitPrice()
    {
        _lb_title.text = "<광부고용>";
        _level = _miner_mgr.GetRecruitLevel();
        _lb_pop_level.text = "+" + _level;

        if (!_miner_mgr.IsMaxMiner())
        {
            _lb_pop_content.text = "광부를 고용합니다.";
            _need_price = _miner_mgr.GetRecruitPrice(_level);
            _lb_pop_price.text = _need_price.ToString();
            _cur_gold = _miner_mgr.GetTotalGold();
            _lack_gold = _cur_gold - _need_price;
			_lb_pop_price_head.text = "가격: ";

            if (_lack_gold >= 0)
            {
                _lb_pop_price_lack.text = "";
                _lb_pop_price_lack_head.text = "";
            }
            else
            {
                _lb_pop_price_lack.text = _lack_gold.ToString();
                _lb_pop_price_lack_head.text = "부족 : ";
            }
        }
        else
        {
            _lb_pop_content.text = "최대치";
            _lb_pop_price.text = "";
            _lb_pop_price_head.text = "";
            _lb_pop_price_lack.text = "";
            _lb_pop_price_lack_head.text = "";
        }
    }

    private void SetFoodPrice()
    {
		_lb_title.text = "<급식>";
		_level = _miner_mgr.GetSpeedLevel ();
		_lb_pop_level.text = "+" + _level;

		if (!_miner_mgr.IsMaxFood ()) 
		{
			_lb_pop_content.text = "급식 레벨을 올려\n 광부들의 속도를 5% 높입니다.";
			_need_price = _miner_mgr.GetSpeedPrice (_level);
			_lb_pop_price.text = _need_price.ToString ();
			_cur_gold = _miner_mgr.GetTotalGold ();
			_lack_gold = _cur_gold - _need_price;
			_lb_pop_price_head.text = "가격: ";
				
			if (_lack_gold >= 0) 
			{
				_lb_pop_price_lack.text = "";
				_lb_pop_price_lack_head.text = "";
			} 
			else 
			{
				_lb_pop_price_lack.text = _lack_gold.ToString ();
				_lb_pop_price_lack_head.text = "부족 : ";
			}
		} 
		else 
		{
			_lb_pop_content.text = "최대치";
			_lb_pop_price.text = "";
			_lb_pop_price_head.text = "";
			_lb_pop_price_lack.text = "";
			_lb_pop_price_lack_head.text = "";
		}
    }

    private void SetMountainPrice()
    {
        _lb_title.text = "<광산>";
		_level = _miner_mgr.GetMountainLevel();
		_lb_pop_level.text = "+" + _level;

		if (!_miner_mgr.IsMaxMountain ()) 
		{
			_lb_pop_content.text = "광산 레벨을 올려\n 획득골드를 5% 높입니다.";
			_need_price = _miner_mgr.GetMountainPrice (_level);
			_lb_pop_price.text = _need_price.ToString ();
			_cur_gold = _miner_mgr.GetTotalGold ();
			_lack_gold = _cur_gold - _need_price;
			_lb_pop_price_head.text = "가격: ";

			if (_lack_gold >= 0) {
				_lb_pop_price_lack.text = "";
				_lb_pop_price_lack_head.text = "";
			} else {
				_lb_pop_price_lack.text = _lack_gold.ToString ();
				_lb_pop_price_lack_head.text = "부족 : ";
			}
		} 
		else 
		{
			_lb_pop_content.text = "최대치";
			_lb_pop_price.text = "";
			_lb_pop_price_head.text = "";
			_lb_pop_price_lack.text = "";
			_lb_pop_price_lack_head.text = "";
		}
    }

    private void SetMinerInfo()
    {
        _lb_title.text = "<광부 " + _cur_selected_miner_idx + ">";
		_level = _miner_mgr._miners[_cur_selected_miner_idx].GetLevel();
		_lb_pop_level.text = "+" + _level;

		if (!_miner_mgr._miners [_cur_selected_miner_idx].IsMaxLevel ()) 
		{
			_lb_pop_content.text = "광부를 성장시킵니다.";
			_lb_pop_content.text += "\n 다음 생산 골드: " + _miner_mgr.GetperGold (_level + 1);
			_need_price = _miner_mgr.GetMinerLevelupPrice (_level);
			_lb_pop_price.text = _need_price.ToString ();
			_cur_gold = _miner_mgr.GetTotalGold ();
			_lack_gold = _cur_gold - _need_price;
			_lb_pop_price_head.text = "가격: ";

			if (_lack_gold >= 0) {
				_lb_pop_price_lack.text = "";
				_lb_pop_price_lack_head.text = "";
			} else {
				_lb_pop_price_lack.text = _lack_gold.ToString ();
				_lb_pop_price_lack_head.text = "부족 : ";
			}
		} 
		else 
		{
			_lb_pop_content.text = "최대치";
			_lb_pop_price.text = "";
			_lb_pop_price_head.text = "";
			_lb_pop_price_lack.text = "";
			_lb_pop_price_lack_head.text = "";
		}
    }

    public void TouchPopupOK()
    {
        switch(_ePop_state)
        {
            case PopState.MINER_RECRUIT:
                if(_lack_gold >= 0)
                {
                    if (!_miner_mgr.IsMaxMiner())
                    {
                        _miner_mgr.AddMiner();
                        _cur_gold -= _need_price;
                        _num_rolling_mgr.AddGold(-_need_price);
                    }
                }
                break;

            case PopState.FOOD:
                if (_lack_gold >= 0)
                {
					if (!_miner_mgr.IsMaxFood ()) 
					{
						_miner_mgr.AddMinerSpeed ();
						_cur_gold -= _need_price;
						_num_rolling_mgr.AddGold (-_need_price);
					}
                }
                break;

            case PopState.MOUNTAIN:
                if (_lack_gold >= 0)
                {
					if (!_miner_mgr.IsMaxMountain ()) 
					{
						_miner_mgr.AddMountain ();
						_cur_gold -= _need_price;
						_num_rolling_mgr.AddGold (-_need_price);
					}
                }
                break;

            case PopState.MINER_INFO:
                if (_lack_gold >= 0)
                {
					if (!_miner_mgr._miners [_cur_selected_miner_idx].IsMaxLevel ()) 
					{
						_miner_mgr._miners [_cur_selected_miner_idx].LevelUp ();
						_cur_gold -= _need_price;
						_num_rolling_mgr.AddGold (-_need_price);
					}
                }
                break;
            
        }

    }

    public void TouchWithoutArea()
    {
        ClosePopup();
		_popup_mgr.PopupAniReady ();
    }

    private void ClosePopup()
    {
        if(_popup_tm.localScale == Vector3.one)
        {
            _popup_tm.localScale = Vector3.zero;
        }
    }

    public void TouchRecuritMiner(GameObject obj)
    {
        // 팝업
        PopupMinerRecruitOpen(obj.name);
		_popup_mgr.PopupAni ();
    }

    public void TouchFood(GameObject obj)
    {
        PopupFoodOpen(obj.name);
		_popup_mgr.PopupAni ();
    }

    public void TouchMountain(GameObject obj)
    {
        PopupMountainOpen(obj.name);
		_popup_mgr.PopupAni ();
    }

    public void TouchTrader(GameObject obj)
    {
        PopupTraderOpen(obj.name);
		_popup_mgr.PopupAni ();
    }

    private void PopupTraderOpen(string menu_name)
    {
        _ePop_state = PopState.TRADER;
        _popup_trader.localScale = Vector3.one;
    }

    public void TouchWithoutAreaTrader()
    {
        ClosePopupTrader();
		_popup_mgr.PopupAniReady ();
    }

    private void ClosePopupTrader()
    {
        if (_popup_trader.localScale == Vector3.one)
        {
            _popup_trader.localScale = Vector3.zero;
        }
    }

    public void TouchItem(GameObject obj)
    {
        _popup_trader.localScale = Vector3.zero;
        _popup_tm.localScale = Vector3.one;
        SetItemDetail(obj.name);
    }

    private void SetItemDetail(string item_name)
    {
        switch(item_name)
        {
            case "BtnPotion":
                _lb_title.text = "<에너지드링크5>";
                _lb_pop_content.text = "5분동안 광부들이 기절하지 않습니다.";
                _need_price = ENERGY_DRINK_FIVE_PRICE;
                break;

            case "BtnPotion15":
                _lb_title.text = "<에너지드링크15>";
                _lb_pop_content.text = "15분동안 광부들이 기절하지 않습니다.";
                _need_price = ENERGY_DRINK_FIFTEEN_PRICE;
                break;

            case "BtnPartyTicket":
                _lb_title.text = "<파티티켓1>";
                _lb_pop_content.text = "1분동안 파티를 열어 광부들의\n 이동속도를 2배로 늘립니다.";
                _need_price = PARTY_TICKET;
                break;

            case "BtnPartyTicket3":
                _lb_title.text = "<파티티켓3>";
                _lb_pop_content.text = "3분동안 파티를 열어 광부들의\n 이동속도를 2배로 늘립니다.";
                _need_price = PARTY_TICKET_THREE;
                break;

            case "BtnEscalator":
                _lb_title.text = "<에스컬레이터>";
                _lb_pop_content.text = "모든 광부들의 이동속도를 1증가 시킵니다.";
                _need_price = ESCALATOR;
                break;

            case "BtnWarp":
                _lb_title.text = "<순간이동장치>";
                _lb_pop_content.text = "일정확률로 광부를 순간이동시킵니다. 쿨타임이 존재합니다";
                _need_price = WARP;
                break;

            case "BtnMagicPaper":
                _lb_title.text = "<마법주문서>";
                _lb_pop_content.text = "일정확률로 광부의 골드획득량과 속도를 5%~15% 증가시킵니다. 재구매시 확률은 다시 지정됩니다";
                _need_price = MAGIC_PAPER;
                break;
        }
        _lb_pop_level.text = "";
        _lb_pop_price.text = _need_price.ToString();
        _cur_gold = _miner_mgr.GetTotalGold();
        _lack_gold = _cur_gold - _need_price;
        if (_lack_gold >= 0)
        {
            _lb_pop_price_lack.text = "";
            _lb_pop_price_lack_head.text = "";
        }
        else
        {
            _lb_pop_price_lack.text = _lack_gold.ToString();
            _lb_pop_price_lack_head.text = "부족 : ";
        }
    }

}
