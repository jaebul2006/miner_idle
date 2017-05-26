using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMgr : MonoBehaviour 
{
	Animator _popwnd_animator;
	Animator _poptrader_animator;

	void Start()
	{
		_popwnd_animator = GameObject.Find ("PopWnd").GetComponent<Animator> ();
		_poptrader_animator = GameObject.Find ("PopTrader").GetComponent<Animator> ();
	}

	public void PopupAni()
	{
		_popwnd_animator.SetBool ("DoPopup", true);
		_poptrader_animator.SetBool ("DoPopup", true);
	}

	public void PopupAniReady()
	{
		_popwnd_animator.SetBool ("DoPopup", false);
		_poptrader_animator.SetBool ("DoPopup", false);
	}
}
