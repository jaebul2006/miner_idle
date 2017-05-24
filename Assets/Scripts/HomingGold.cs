using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingGold : MonoBehaviour {

    public float _speed = 5f;
    public float _rotating_speed = 200f;
    public GameObject _target;

    Rigidbody2D _rigid;
    Vector3 _start_point;



	void Start () 
    {
        _target = GameObject.Find("HomingTarget");
        gameObject.transform.parent = _target.transform.parent;
        _rigid = gameObject.GetComponent<Rigidbody2D>();
        _start_point = new Vector3(Random.Range(-2.11f, -1.39f), -1.91f, 0f);
        transform.localPosition = _start_point;
	}
	
	void FixedUpdate () 
    {
        Vector2 point2target = (Vector2)transform.position - (Vector2)_target.transform.position;
        point2target.Normalize();
        float value = Vector3.Cross(point2target, transform.right).z;

        _rigid.angularVelocity = _rotating_speed * value;
        _rigid.velocity = transform.right * _speed;
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "HomingTarget")
        {
            Destroy(gameObject);
        }
    }

    
}
