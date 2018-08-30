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

    [SerializeField] private AudioClip battleLoopSound;

    [SerializeField] private AudioClip battleEndSound;
    [SerializeField] private AudioClip deathSound;
	
    public bool BattleLoopPlay { get; set; }
    public bool BattleEndPlay { get; set; }
	public bool DeathSoundPlay {get; set;}




    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
		if (DeathSoundPlay) {
			audioSource.Stop();
			if (Play(deathSound)) {
				DeathSoundPlay = false;
				BattleEndPlay = false;
				BattleLoopPlay = false;
			}
		}

        if (BattleLoopPlay)
        {
            Play(battleLoopSound);

        }
        else if (BattleEndPlay)
        {
            if (Play(battleEndSound))
            {
                BattleEndPlay = false;
            }

        }
    }

    private bool Play(AudioClip clip)
    {
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
        BattleEndPlay = false;
        BattleLoopPlay = true;
    }

    public void PlayBattleEndSound()
    {
        BattleLoopPlay = false;
        BattleEndPlay = true;
    }

    public void PlayDeathSound()
    {
		DeathSoundPlay = true;
    }
}
