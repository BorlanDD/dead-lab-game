using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    #region SINGLETON PATTERN
    private static AudioManager instance;
    public static AudioManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    #endregion

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip battleLoop;

    [SerializeField]
    private AudioClip battleEnd;

    public bool BattleLoopPlayed { get; set; }

    public bool BattleLoopPlay { get; set; }
    public bool BattleEndPlay { get; set; }




    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (BattleLoopPlay)
        {
            Play(battleLoop);

        }
        else if (BattleEndPlay)
        {
			if (Play(battleEnd)) {
				BattleEndPlay = false;
			}
			
        }
    }

	private bool Play(AudioClip clip) {
		if (!audioSource.isPlaying)
        {
            audioSource.clip = clip;
            audioSource.Play();
			return true;
        }
		return false;
	}


    public void PlayBattleLoopSound()
    {
        /* if (!audioSource.isPlaying)
        {
            audioSource.clip = battleLoop;
            audioSource.Play();
			BattleEndPlay = false;
            BattleLoopPlay = false;
            BattleLoopPlayed = true;
        } */
		BattleEndPlay = false;
		BattleLoopPlay = true;
    }

    public void PlayBattleEndSound()
    {
        /* if (!audioSource.isPlaying)
        {
            audioSource.clip = battleEnd;
            audioSource.Play();
            BattleLoopPlayed = false;
            BattleEndPlay = false;
        } */
		BattleLoopPlay = false;
		BattleEndPlay = true;
    }
}
