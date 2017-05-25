using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberRollingMgr : MonoBehaviour
{
    public UILabel[] _lb_rolling_gold;
    public float _rolling_time = 0.5f;

    private float[] _rolling_value;
    private bool _is_rolling = false;
    private List<int> _list_gold = new List<int>();
    private int _gold_value;

    void Awake()
    {
        int len = _lb_rolling_gold.Length;
        if (len != 0)
        {
            _rolling_value = new float[len];
        }
        _list_gold.Clear();
    }

    void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            
        }
    }

    public int GetTotalGold()
    {
        return _gold_value;
    }

    public void InitValue(int pos, int v)
    {
        _lb_rolling_gold[pos].text = v.ToString();
    }

    public void AddGold(int add_v)
    {
        if (!_is_rolling)
        {
            MoveToTime(0, _gold_value + add_v);
        }
        else
        {
            _list_gold.Add(add_v);
        }
    }

    public void MoveToTime(int pos, int v)
    {
        _gold_value = v;
        _is_rolling = true;
        _rolling_value[pos] = v;
        StartCoroutine("UpdateMoveToTime", pos);
    }

    private IEnumerator UpdateMoveToTime(int pos)
    {
        float value = _rolling_value[pos];    // 최종 값
        float prevValue = float.Parse(_lb_rolling_gold[pos].text);  // 현재 표시 값(변경 이전 값)
        float distance = Abs(prevValue - value);           // 두 값 사이의 거리
        float limitTime = _rolling_time;                         // 해당 시간 동안 롤링

        while (true)
        {
            yield return null;

            // deltaTime동안 이동 해야 할 거리를 계산
            float dis = distance * (Time.deltaTime / _rolling_time);

            // 라벨에 쓰여진 숫자(prevValue)에서 목표 값(value)까지 dis 만큼 이동한 값을 얻는다
            prevValue = GetMovedValue(prevValue, value, dis);

            // 얻은 값을 라벨에 입력
            _lb_rolling_gold[pos].text = ((int)prevValue).ToString();

            // 지정 된 시간이 지나면 롤링 종료
            if (limitTime < 0.0f)
            {
                _is_rolling = false;
                if(_list_gold.Count > 0)
                {
                    MoveToTime(0, _gold_value + _list_gold[0]);
                    _list_gold.RemoveAt(0);
                }
                break;
            }

            limitTime -= Time.deltaTime;
        }

        // 예외처리
        // 값이 잘못 입력되었을 때를 대비한다
		_lb_rolling_gold[pos].text = value.ToString();
    }

    // 절대값.
    private float Abs(float v)
    {
        return (v > 0) ? v : -(v);
    }

    // _distance 만큼 이동 된 결과를 return.
    private float GetMovedValue(float s, float e, float dis)
    {
        // 값이 동일하면 목표값을 return
        if (s == e)
            return e;

        // 시작점과 도착점의 거리 차이를 구함
        float value = Abs(s - e);

        // 두 지점 차이의 거리가 이동 해야 할 거리보다 적으면 목표 값 return
        if (value < dis)
            return e;

        // 시작 값이 목표 값보다 작을 경우 시작값에 이동거리를 더하여 return
        if (s < e)
            return s + dis;

        // 시작 값이 목표 값보다 클 경우 시작값에 이동거리를 빼고 return
        return s - dis;
    }
}
