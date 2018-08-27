using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : InteractionObject
{

    #region SINGLETON PATTERN
    private static Generator instance;
    public static Generator GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public GameObject[] lights;
    public DoorScript[] doors;

    private int generatorBatteryId;
    private ItemType generatorBatteryType;
    public Transform batteryPos;

    [SerializeField] protected AudioClip generatorWorkingSound;
    [SerializeField] protected AudioClip generatorBrokenSound;

    [SerializeField] protected AudioClip horror;
    protected AudioSource _source;

    public override void OnStart()
    {
        base.OnStart();
        locked = true;
        _source = GetComponent<AudioSource>();


        generatorBatteryId = 1;
        generatorBatteryType = ItemType.Battery;
    }

    private bool isBroken;
    public void BrokeGenerator() {
        _source.PlayOneShot(generatorBrokenSound);
        isBroken = true;
    }

    void Update() {
        if (isBroken) {
            LockDoors();
            if (!_source.isPlaying) {
                SwitchOffLights();
                locked = false;
                enabled = false;
                _source.PlayOneShot(horror);
            }
        }
    }

    public void SwitchOnLights()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(true);
        }
    }

    public void SwitchOffLights()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(false);
        }
    }

    public void LockDoors()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].locked = true;
        }
    }

    public void UnlockDoors()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].locked = false;
        }
    }

    public override void Interract()
    {
        base.Interract();
        Inventory inventory = Player.GetInstance().inventory;
        Item battery = inventory.GetItem(generatorBatteryId, generatorBatteryType);
        if (battery != null)
        {
            SwitchOnLights();
            UnlockDoors();
            inventory.RemoveItem(battery);

            locked = true;
            battery.locked = true;

            battery.gameObject.transform.SetPositionAndRotation(batteryPos.transform.position, batteryPos.transform.rotation);
            battery.gameObject.transform.SetParent(batteryPos);
            battery.gameObject.SetActive(true);

            TasksManager.GetInstance().FinishTask(FindGeneratorTask.GetInstance());
            _source.loop = true;
            _source.clip = generatorWorkingSound;
            _source.Play();
        }
        else
        {
            FindGeneratorTask fgt = (FindGeneratorTask)FindGeneratorTask.GetInstance();
            if (fgt != null)
            {
                FindGeneratorKeyTask fgkt = GetComponent<FindGeneratorKeyTask>();
                if (!fgkt.started)
                {
                    fgkt.OnStart();
                    fgt.addSubTask(fgkt);
                    locked = true;
                }
            }
        }
    }
}
