using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour {

	#region SINGLETON PATTERN
    private static HealthUI instance;
    public static HealthUI GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    #endregion


	public Image healthLevel;

    private Player _player;

	// Use this for initialization
	void Start()
    {
        _player = Player.GetInstance();
    }
	
	// Update is called once per frame
	void Update()
    {
        healthLevel.fillAmount = _player.Health;
		if (_player.IsHealing) {
			healthLevel.color = Color.green;
		} 
		else {
			healthLevel.color = Color.white;
		}
    }
}
