﻿using UnityEngine;
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

    private Text biomeCountsText;
    private int arcticCount;
    private int desertCount;
    private int woodlandCount;
    private int tropicalCount;
    private bool triggerUpdate = false;

	private float timer;
	private int bombsCount = 2;
	private int extraWispsCount = 5;
	private int swapCount = 3;

    // Use this for initialization
    void Start()
    {
        biomeCountsText = biomeCounts.GetComponentInChildren<Text>();
		extraWispsLeft.GetComponentInChildren<Text>().text = "ExtraWisps: " + extraWispsCount;
		bombsLeft.GetComponentInChildren<Text> ().text = "Bombs Left: " + bombsCount;
		swapsLeft.GetComponentInChildren<Text> ().text = "Swaps: " + swapCount;
    }

    // Update is called once per frame
    void Update()
    {
        biomeCountsText.text = "Arctic: " + arcticCount + " Desert: " + desertCount + " Woodland: " + woodlandCount + " Tropical " + tropicalCount;
		timer += Time.deltaTime;
		timerText.GetComponentInChildren<Text>().text = "Timer: " + (int)timer;
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