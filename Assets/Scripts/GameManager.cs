using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action<string> HasChanged;

    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }

    [NonSerialized]
    public string gameResult;

    [SerializeField]
    List<GameObject> objsWaitingToRun;
    private void Start()
    {
        Application.targetFrameRate = 60;
        thresold = 1;
        weaponAmount = 100;
        charHealth = 100;
        zomHealth = 100;
        secondStageOn = false;
        foreach (GameObject obj in objsWaitingToRun)
        {
            obj.GetComponent<IOrderOfRunningStart>()?.Init();
        }
    }

    public int score { get; private set; }
    public void SetScore()
    {
        score++;
        HasChanged?.Invoke("score");
    }

    int minusAmount = 1;
    [NonSerialized]
    public int plusAmount = 20;
    public int weaponAmount { get; private set; }
    public void AddValidDamage()
    {
        weaponAmount += plusAmount;
        HasChanged?.Invoke("weaponAmount");
    }
    public void DecreaseValidDamage()
    {
        weaponAmount -= minusAmount;
        HasChanged?.Invoke("weaponAmount");
    }

    [NonSerialized]
    public int plusValue = 1;

    public int specialEnergy { get; private set; }
    public void SetSpecialEnergy()
    {
        if (specialEnergy < thresold)
        {
            specialEnergy += plusValue;
            HasChanged?.Invoke("specialEnergy");
        }
        if (specialEnergy >= thresold) HasChanged?.Invoke("canTurnOnSkill");
    }

    public int thresold { get; private set; }
    public void SetThresold()
    {
        thresold = 50;
        HasChanged?.Invoke("thresold");
    }

    int minusValue = 5;
    public int charHealth { get; private set; }
    public void SetCharHealth()
    {
        if (charHealth <= 0) CommunicateManager.instance.CanDieThing("Character")?.Die();
        charHealth -= minusValue * 5;
        HasChanged?.Invoke("charHealth");
    }

    public int zomHealth { get; private set; }
    public void SetZomHealth()
    {
        if (zomHealth <= 0) CommunicateManager.instance.CanDieThing("Zombie")?.Die();
        zomHealth -= minusValue;
        HasChanged?.Invoke("zomHealth");
    }

    public bool secondStageOn {  get; private set; }
    public void SetBoolSecondStageOn()
    {
        secondStageOn = true;
        HasChanged?.Invoke("secondStageOn");
    }

    //[NonSerialized]
    //public bool hasRunOnDestroy = false;

    [NonSerialized]
    public bool oneTimeUse = true;

    [NonSerialized]
    public bool hasClick = false;

    [NonSerialized]
    public bool hasClickSpcButton = false;

    bool turnOnSkill = false;
    bool turnOnCoroutine = false;

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) && thresold <= specialEnergy) || hasClickSpcButton)
        {
            if (!secondStageOn) Time.timeScale = 0;
            else
            {
                turnOnSkill = true;
                turnOnCoroutine = true;
            }
            specialEnergy = 0;
            hasClickSpcButton = false;
            HasChanged?.Invoke("turnOffButton");
            HasChanged?.Invoke("specialEnergy");
        }
        if (Time.timeScale == 0 && gameResult == null)
        {
            CommunicateManager.instance.SpecialSkill("Zombie1")?.OnSpecialSkill();
            CommunicateManager.instance.SpecialSkill("TheWall")?.OnSpecialSkill();
        }
        if (turnOnSkill)
        {
            if (turnOnCoroutine)
            {
                StartCoroutine(Duration());
                turnOnCoroutine = false;
            }    
            CommunicateManager.instance.SpecialSkill("TheKnife")?.OnSpecialSkill();
        }
    }

    IEnumerator Duration()
    {
        yield return new WaitForSecondsRealtime(5);
        zomHealth -= 20;
        HasChanged?.Invoke("zomHealth");
        turnOnSkill = false;
        CommunicateManager.instance.Adjust()?.AdjustTransform();
    }
}
