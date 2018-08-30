using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Arms arms;

    #region Borland's code
    public enum Status
    {
        Stand,
        Walk,
        Run
    }


    [SerializeField] private AudioClip soundFirstLeg;
    [SerializeField] private AudioClip soundSecondLeg;
    [SerializeField] private AudioClip fartSound;

    [SerializeField] private AudioClip breath;
    [SerializeField] private AudioClip breathRun;
    [SerializeField] private AudioClip exhalation;
    [SerializeField] private AudioClip exhalationRun;
    private AudioSource audioSource;

    public Status CurrentStatus { get; set; }

    private Vector3 prevPositionPlayer;

    public float stepLenght = 1.6f;
    private bool isFirstLeg = false;
    private float currentDistance;


    public float staminaUse = 20f;
    public float staminaIdleRestore = 25f;
    public float staminaWalkRestore = 30f;

    private float prevDistance;

    public bool IsInMotion { get; set; }

    private float eps = 0.0001f;

    private bool farted = false;

    public bool Reloading;

    private FieldOfView eyes;

    void FixedUpdate()
    {
        
        //Debug.Log(GameObject.FindGameObjectWithTag("NavMesh").GetComponent<UnityEngine.AI.NavMeshSurface>().)
        if (!Generator.GetInstance().enabled && !farted)
        {
            audioSource.PlayOneShot(fartSound);
            farted = true;
        }

        currentDistance += Vector3.Distance(prevPositionPlayer, transform.position);
        if (currentDistance > prevDistance + eps)
        {
            IsInMotion = true;
        }
        else
        {
            IsInMotion = false;
        }

        prevDistance = currentDistance;
        prevPositionPlayer = transform.position;

        if (!animator.GetBool("Walk") && IsInMotion)
        {
            animator.SetBool("Walk", true);
        }
        else if (CurrentStatus == Status.Stand)
        {
            animator.SetBool("Walk", false);
        }


        if (tired)
        {
            if (!_audioBreath.isPlaying)
            {
                if (_audioBreath.clip == breath)
                {
                    _audioBreath.clip = exhalation;
                    _audioBreath.Play();
                }
                else
                {
                    _audioBreath.clip = breath;
                    _audioBreath.Play();
                }
            }

        }


        if (IsInMotion && CurrentStatus == Status.Run && !tired)
        {
            player.Sprinting(Time.deltaTime / staminaUse);
        }
        else
        {
            if (stamina < 1f)
            {
                float staminaRestore = 0;
                if (CurrentStatus == Status.Stand || !IsInMotion)
                {
                    staminaRestore = staminaIdleRestore;
                }
                else if (CurrentStatus == Status.Walk)
                {
                    staminaRestore = staminaWalkRestore;
                }
                else if (CurrentStatus == Status.Run && tired)
                {
                    staminaRestore = staminaWalkRestore;
                }
                if (staminaRestore != 0)
                {
                    player.Resting(Time.deltaTime / staminaRestore);
                }
            }
        }



        if (currentDistance >= stepLenght)
        {
            if (isFirstLeg)
            {
                audioSource.PlayOneShot(soundFirstLeg);
                isFirstLeg = false;
            }
            else
            {
                audioSource.PlayOneShot(soundSecondLeg);
                isFirstLeg = true;
            }
            prevDistance = currentDistance = 0f;
        }

        List<Kangaroo> visibleEnemies = eyes.FindVisibleTargets();
        foreach (Kangaroo kangaroo in visibleEnemies) {
            kangaroo.playerSee = true;
        }
    }



    #endregion

    public static string EQUIP_ANIMATION = "_Equip";
    public static string UNEQUIP_ANIMATION = "_Unequip";

    public Animator animator;
    public float stamina { get; set; }
    public float staminaCriticalLevel { get; private set; }
    public bool tired { get; private set; }

    public Weapon usingWeapon { get; private set; }

    public Weapon beforeHealerWeapon;

    private static Player player;

    public float Health;
    public Inventory inventory;

    public Transform automatHandPosition;

    public Transform pistolHandPosition;

    public Healer healer;

    public GameObject breathGameObj;
    private AudioSource _audioBreath;

    public bool IsHealing { get; set; }

    void Awake()
    {
        // animator = GetComponent<Animator>();
        eyes = GetComponentInChildren<FieldOfView>();
        usingWeapon = null;
        player = this;
        Reloading = false;
        Health = 1f;
    }
    public static Player GetInstance()
    {
        return player;
    }


    // Use this for initialization
    void Start()
    {
        audioSource = GetComponentInParent<AudioSource>();
        _audioBreath = breathGameObj.GetComponent<AudioSource>();
        prevPositionPlayer = transform.position;
        stamina = 1f;
        staminaCriticalLevel = 0.3f;
        tired = false;
        settingWeapon = false;
    }

    public void Sprinting(float staminaNeed)
    {
        stamina -= staminaNeed;
        if (stamina <= 0)
        {
            stamina = 0;
            tired = true;
        }
    }

    public void Resting(float staminaRest)
    {
        stamina += staminaRest;
        if (stamina > 1f)
        {
            stamina = 1f;
        }
        if (stamina >= staminaCriticalLevel)
        {
            tired = false;
        }
    }

    public bool settingWeapon;

    public void UnequipWeapon()
    {
        if (usingWeapon != null)
        {
            lastWeapon = usingWeapon;
        }
        else if (lastWeapon == null)
        {
            return;
        }
        StopShooting();
        usingWeapon = null;
        animator.SetTrigger(lastWeapon.itemName + UNEQUIP_ANIMATION);
        WeaponUI.GetInstance().UpdateSprites(usingWeapon);
    }

    private Weapon lastWeapon;
    public void TakeOffWeapon()
    {
        if (lastWeapon == null || (settingWeapon && lastWeapon == usingWeapon))
        {
            return;
        }

        lastWeapon.gameObject.SetActive(false);
        inventory.AddItem(lastWeapon);
    }

    public void SetWeapon()
    {
        if (usingWeapon == null)
        {
            return;
        }

        usingWeapon.gameObject.SetActive(true);
        settingWeapon = false;

        usingWeapon.UpdateBullets();
        WeaponUI.GetInstance().UpdateSprites(usingWeapon);
    }

    public void EquipWeapon(Weapon weapon)
    {
        if (settingWeapon || Reloading)
        {
            return;
        }

        if (weapon == null)
        {
            return;
        }

        settingWeapon = true;
        if (usingWeapon != null)
        {
            lastWeapon = usingWeapon;
            lastWeapon.StopShooting();
            UnequipWeapon();
        }


        Transform handPos = null;
        if (weapon.weaponType == Weapon.Type.Pistol)
        {
            handPos = pistolHandPosition;
        }
        else if (weapon.weaponType == Weapon.Type.Automat)
        {
            handPos = automatHandPosition;
        }

        weapon.transform.SetParent(handPos);
        weapon.transform.position = handPos.transform.position;
        weapon.transform.rotation = new Quaternion(0, 0, 0, 0);
        usingWeapon = weapon;
        WeaponUI.GetInstance().UpdateSprites(weapon);
        animator.SetTrigger(weapon.itemName + EQUIP_ANIMATION);
    }

    public void EquipPreviousWeapon()
    {
        EquipWeapon(lastWeapon);
    }

    public void Fire()
    {
        if (usingWeapon != null && !Reloading)
        {
            usingWeapon.Fire();
            WeaponUI.GetInstance().UpdateSprites(usingWeapon);
        }
    }

    public void StopShooting()
    {
        if (usingWeapon != null)
        {
            usingWeapon.StopShooting();
            WeaponUI.GetInstance().UpdateSprites(usingWeapon);
        }
    }

    public void ChangeFireMode(int direction)
    {
        if (usingWeapon == null)
        {
            return;
        }

        StopShooting();

        if (direction == 1)
        {
            usingWeapon.SetNextMode();
        }
        else if (direction == -1)
        {
            usingWeapon.SetPreviousMode();
        }
    }

    public void ReloadWeapon()
    {
        if (usingWeapon == null || !usingWeapon.NeedToReload() || Reloading)
        {
            return;
        }
        StopShooting();
        Reloading = true;
        usingWeapon.audioSource.PlayOneShot(usingWeapon.soundReloading);
        animator.SetTrigger(usingWeapon.itemName + "_Reload");
    }

    public void ReloadedWeapon()
    {
        usingWeapon.Reload();
        Reloading = false;
        WeaponUI.GetInstance().UpdateSprites(usingWeapon);
    }

    public void UseHealer(Healer healer)
    {
        if (healer == null)
        {
            return;
        }
        if (usingWeapon != null)
        {
            beforeHealerWeapon = usingWeapon;
            Debug.Log("setted");
            UnequipWeapon();
        }
        this.healer = healer;

        animator.SetTrigger("Use_Healer");
    }

    public void GetDamage(float damage) {
        if (Health > 0f) {
            Health -= damage;
        }
        else {
            Die();
        }
    }

    private void Die() {
        Debug.Log("Dude, you died!");
        Debug.Break();
    }
}
