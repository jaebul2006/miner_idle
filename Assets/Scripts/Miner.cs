using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : MonoBehaviour
{
    float _move_speed = 1.0f;
    public tk2dSprite _tkspr;
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
        Stun,
    }

    private State _state = State.ToCart;

    void Start()
    {
        _tkspr.FlipX = false;
        _miner_mgr = GameObject.Find("MinerMgr").GetComponent<MinerMgr>();
        _homing_mgr = GameObject.Find("HomingMgr").GetComponent<HomingMgr>();
    }

    void Update()
    {
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.name == "cart")
        {
            _state = State.ToMine;
            _tkspr.FlipX = true;
            int get_gold = _miner_mgr.GetPerGold(_level);
            _homing_mgr.GoldGetEff(get_gold);
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
        _level++;
    }

    public void SetSpeed(float spd)
    {
        _move_speed = spd;
    }

}
