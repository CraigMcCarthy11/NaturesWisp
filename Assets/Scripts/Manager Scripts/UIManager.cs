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
    public GameObject funFactText;

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
    private List<string> funFacts = new List<string>();

	public int bombsCount = 2;
	public int extraWispsCount = 5;
	public int swapCount = 3;

    // Use this for initialization
    void Start()
    {
        biomeCountsText = biomeCounts.GetComponentInChildren<Text>();
        funFactSetting();
        funFactText.GetComponentInChildren<Text>().text = funFacts[UnityEngine.Random.Range(0, funFacts.Capacity)];
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
            funFactText.GetComponentInChildren<Text>().text = funFacts[UnityEngine.Random.Range(0, funFacts.Capacity)];
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
    void funFactSetting()
    {
        funFacts.Add("When lightning strikes it can reach up to 30,000 degrees celsius (54,000 degrees fahrenheit)");
        funFacts.Add("The only continent with no active volcanoes is Australia");
        funFacts.Add("The Amazon rainforest produces half the world's oxygen supply");
        funFacts.Add("The Great Barrier Reef in Australia is the world’s largest reef system.");
        funFacts.Add("Around 75% of the volcanoes on Earth are found in the Pacific Ring of Fire, an area around the Pacific Ocean where tectonic plates meet.");
        funFacts.Add("About 75% of increase in CO2 levels from humans in the last 20 years is from the burning of fuels. The rest is largely deforestation.");
        funFacts.Add("85% of plant life is found in the ocean and");
        funFacts.Add("Our earth is moving around the sun at 67,000 miles (107,826 km) per hour.");
    }
}
