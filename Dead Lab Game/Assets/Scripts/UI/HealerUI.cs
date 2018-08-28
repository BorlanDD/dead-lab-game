using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealerUI : MonoBehaviour {

	#region SINGLETON PATTERN
    private static HealerUI instance;
    public static HealerUI GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    #endregion

	public Image healerLevel;
	public Text healerCounts;
	public GameObject healer;

    private Player _player;

	// Use this for initialization
	void Start () {
		_player = Player.GetInstance();
	}
	
	// Update is called once per frame
	void Update () {
		int count = _player.inventory.GetItemByType(ItemType.Healer).Count;
		if (count > 0) {
			healer.SetActive(true);
			healerCounts.text = count.ToString();
			healerCounts.enabled = true;
			if (_player.IsHealing) {
				healerLevel.color = Color.red;
			} else {
				healerLevel.fillAmount = 1f;
				healerLevel.color = Color.white;
			}
		}
		else 
		{
			if (_player.IsHealing) {
				healer.SetActive(true);
				healerLevel.color = Color.red;
			} else {
				healer.SetActive(false);
				healerLevel.color = Color.white;
			}
			
			healerCounts.enabled = false;
		}
	}
}
