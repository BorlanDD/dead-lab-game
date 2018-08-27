using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class Weapon : Item
{

    public static string SINGLE_FIRE = "Fire_Single";
    public static string BURST_FIRE = "Fire_Burst";
    public static string AUTOMATIC_FIRE = "Fire_Automatic";

    [SerializeField] private AudioClip soundShot;
    [SerializeField] private AudioClip soundClick;
    [SerializeField] private AudioSource audioSource;

    public enum ShootingMode
    {
        Locked,
        Single,
        Burst,
        Automatic
    }

    public enum Type
    {
        Pistol,
        Automat
    }

    public Rigidbody bulletPrefab;
    public Transform spawnPoint;

    public ShootingMode currentShootingMode { get; protected set; }
    protected IList<ShootingMode> availableShootingModes;

    protected Animator animator;

    public GameObject magazin;
    public Transform MagazinPos;

    public Type weaponType { get; protected set; }
    public int slot;
    public int MaxbulletCounts { get; set; }

    public int bulletCountsLeft { get; protected set; }
    public int bulletCounts { get; protected set; }
    public int damage { get; protected set; }

    public bool lockedShoot;
    private float lockedShootTimeLeft;

    protected int bulletSpeed;
    protected int burstBulletCount;
    protected int burstShootMade;
    protected float afterSingleDelay;
    public bool singleShootLock { get; protected set; }


    protected float afterAutomaticDelay;

    //Delay beetween bullets
    protected float burstModeDelay;
    public float afterBurstDelay;
    protected float burstModeTimeLeft;
    private bool makeBurstShoot;

    protected float afterAutomaticShotDelay;

    protected float dissipateAutomaticStartThrough;
    protected float dissipateAutomaticTimeLeft;


    public override void OnAwake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        type = ItemType.Weapon;
        availableShootingModes = new List<ShootingMode>();
        currentShootingMode = ShootingMode.Locked;
        availableShootingModes.Add(ShootingMode.Locked);
        currentMode = 0;
        lockedShoot = true;

        singleShootLock = false;

        burstBulletCount = 1;
        makeBurstShoot = false;

        afterAutomaticShotDelay = 0;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Player.GetInstance().usingWeapon != this)
        {
            return;
        }

        if (lockedShoot)
        {
            if (currentShootingMode == ShootingMode.Single)
            {
                if (lockedShootTimeLeft >= afterSingleDelay)
                {
                    lockedShoot = false;
                    lockedShootTimeLeft = 0;
                }
                else
                {
                    lockedShootTimeLeft += Time.deltaTime;
                }
            }
            else if (currentShootingMode == ShootingMode.Automatic)
            {
                if (lockedShootTimeLeft >= afterAutomaticDelay)
                {
                    lockedShoot = false;
                    lockedShootTimeLeft = 0;
                }
                else
                {
                    lockedShootTimeLeft += Time.deltaTime;
                }
            }
            else if (currentShootingMode == ShootingMode.Burst && !makeBurstShoot)
            {
                if (lockedShootTimeLeft >= afterBurstDelay)
                {
                    lockedShoot = false;
                    lockedShootTimeLeft = 0;
                }
                else
                {
                    lockedShootTimeLeft += Time.deltaTime;
                }
            }
        }
    }

    private void ResetBurstShoot()
    {
        makeBurstShoot = false;
        burstShootMade = 0;
        burstModeTimeLeft = 0;
    }

    private ShootingMode shotMode;
    public void Fire()
    {
        if (bulletCounts <= 0 || currentShootingMode == ShootingMode.Locked || lockedShoot
         || (currentShootingMode == ShootingMode.Single && singleShootLock))
        {

            if (currentShootingMode == ShootingMode.Locked)
            {
                WeaponUI.GetInstance().IsBlinked = true;
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(soundClick);
                }

            }
            else
            {
                WeaponUI.GetInstance().IsBlinked = false;
            }
            return;
        }

        if (animator != null)
        {
            lockedShoot = true;
            if (currentShootingMode == ShootingMode.Single && !singleShootLock)
            {
                animator.SetTrigger("SingleFire");
                singleShootLock = true;
            }
            else if (currentShootingMode == ShootingMode.Burst)
            {
                animator.SetTrigger("BurstFire");
            }
            else if (currentShootingMode == ShootingMode.Automatic)
            {
                MakeShoot();
                if (dissipateAutomaticTimeLeft >= dissipateAutomaticStartThrough)
                {
                    if (!Player.GetInstance().animator.GetBool(itemName + "_" + AUTOMATIC_FIRE))
                    {
                        Player.GetInstance().animator.SetBool(itemName + "_" + AUTOMATIC_FIRE, true);
                    }
                }
                else
                {
                    animator.SetTrigger("SingleFire");
                    dissipateAutomaticTimeLeft += Time.deltaTime;
                }
            }
        }
        shotMode = currentShootingMode;
    }

    public void StopShooting()
    {
        if (availableShootingModes.Contains(ShootingMode.Automatic) && Player.GetInstance().animator.GetBool(itemName + "_" + AUTOMATIC_FIRE))
        {
            Player.GetInstance().animator.SetBool(itemName + "_" + AUTOMATIC_FIRE, false);
        }

        if (singleShootLock)
        {
            singleShootLock = false;
        }

        if (dissipateAutomaticTimeLeft != 0)
        {
            dissipateAutomaticTimeLeft = 0;
        }
    }

    public void MakeSingleShot()
    {
        Player.GetInstance().animator.SetTrigger(itemName + "_" + SINGLE_FIRE);
    }

    private void MakeShoot()
    {

        if (bulletCounts <= 0)
        {
            return;
        }

        audioSource.PlayOneShot(soundShot);
        Bullet bullet = BulletsPull.GetInstnace().GetBullet();
        bullet.transform.SetPositionAndRotation(spawnPoint.transform.position, spawnPoint.transform.rotation);
        bullet.gameObject.SetActive(true);
        bullet.damage = damage;

        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
        bulletRB.velocity = spawnPoint.TransformDirection(new Vector3(0, 0, bulletSpeed));
        bulletCounts--;

        if (bulletCounts <= 0)
        {
            Player.GetInstance().ReloadWeapon();
            StopShooting();
        }
    }

    public void SetShootingMode(ShootingMode mode)
    {
        for (int i = 0; i < availableShootingModes.Count; i++)
        {
            if (mode == availableShootingModes[i])
            {
                currentShootingMode = mode;
                break;
            }
        }
    }

    private int currentMode;

    public void SetNextMode()
    {
        currentMode++;
        if (currentMode >= availableShootingModes.Count)
        {
            currentMode = 0;
        }
        currentShootingMode = availableShootingModes[currentMode];
    }
    public void SetPreviousMode()
    {
        currentMode--;
        if (currentMode < 0)
        {
            currentMode = availableShootingModes.Count - 1;
        }
        currentShootingMode = availableShootingModes[currentMode];
    }

    public void Reload()
    {
        bulletCounts = MaxbulletCounts;
        magazin.SetActive(false);
        magazin.transform.SetParent(MagazinPos);
        magazin.transform.localPosition = new Vector3(0f, 0f, 0f);
        magazin.transform.localScale = new Vector3(1f, 1f, 1f);
        magazin.SetActive(true);
        magazin.transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public bool NeedToReload()
    {
        if (bulletCounts < MaxbulletCounts)
        {
            return true;
        }
        return false;
    }

    public void Setted()
    {
        if (currentShootingMode != ShootingMode.Locked)
        {
            lockedShoot = false;
        }
    }

    public void UpdateBullets()
    {
        bulletCountsLeft = WeaponUtils.BUlletsLeft(this);
        if (Player.GetInstance().usingWeapon == this)
        {
            WeaponUI.GetInstance().UpdateSprites(this);
        }   
    }
}
