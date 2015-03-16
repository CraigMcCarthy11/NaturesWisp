using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{

    public GameObject biomeCounts;
    public GameManager gameManager;
	public GameObject timerText;
	public GameObject extraWispsLeft;
	public GameObject bombsLeft;
	public GameObject swapsLeft;
    public GameObject powerupTextTimers;

    //Button Prefabs
    public GameObject WispButtonPrefab;
    public GameObject SwapButtonPrefab;
    public GameObject BombButtonPrefab;

    private Text biomeCountsText;
    private int arcticCount;
    private int desertCount;
    private int woodlandCount;
    private int tropicalCount;
    private bool triggerUpdate = false;

    //for getting powerups
	private float wispTimer;
    private float swapTimer;
    private float bombTimer;
    private float overallTimer;

	public int bombsCount = 2;
	public int extraWispsCount = 5;
	public int swapCount = 3;

    // Use this for initialization
    void Start()
    {
        biomeCountsText = biomeCounts.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        biomeCountsText.text = "Arctic: " + arcticCount + " Desert: " + desertCount + " Woodland: " + woodlandCount + " Tropical " + tropicalCount;

        overallTimer += Time.deltaTime;
        wispTimer += Time.deltaTime;
        swapTimer += Time.deltaTime;
        bombTimer += Time.deltaTime;
        if (wispTimer >= 30 || swapTimer >= 45 || bombTimer >= 250)
        {
            addPowerups();
        }
        timerText.GetComponentInChildren<Text>().text = "Timer: " + (int)overallTimer;
        extraWispsLeft.GetComponentInChildren<Text>().text = "ExtraWisps: " + extraWispsCount;
        bombsLeft.GetComponentInChildren<Text>().text = "Bombs Left: " + bombsCount;
        swapsLeft.GetComponentInChildren<Text>().text = "Swaps: " + swapCount;
        powerupTextTimers.GetComponentInChildren<Text>().text = "Wisp +1 = " + (int)wispTimer + "/30" + " || Swap +1 = " + (int)swapTimer + "/45" + " || Bomb +1 = " + (int)bombTimer + "/250";
    }
    void addPowerups()
    {
        if (wispTimer >= 30)
        {
            extraWispsCount++;
            wispTimer = 0;
        }
        if (swapTimer >= 45)
        {
            swapCount++;
            swapTimer = 0;
        }
        if (bombTimer >= 250)
        {
            bombsCount++;
            bombTimer = 0;
        }
    }
    public void GetBiomesInit(Faction.FactionTypes type)
    {
        switch (type)
        {
            case Faction.FactionTypes.Arctic:
                arcticCount++;
                break;
            case Faction.FactionTypes.Desert:
                desertCount++;
                break;
            case Faction.FactionTypes.Tropical:
                tropicalCount++;
                break;
            case Faction.FactionTypes.Woodland:
                woodlandCount++;
                break;
        }
    }
}
