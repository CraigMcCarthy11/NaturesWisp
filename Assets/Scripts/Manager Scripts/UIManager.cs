using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class UIManager : MonoBehaviour {

    public GameObject biomeCounts;
    public GameManager gameManager;

    private Text biomeCountsText;
    private int arcticCount;
    private int desertCount;
    private int woodlandCount;
    private int tropicalCount;
    private bool triggerUpdate = false;

	// Use this for initialization
	void Start () {
        biomeCountsText = biomeCounts.GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if (triggerUpdate = true)
        {
            biomeCountsText.text = "Arctic: " + arcticCount + " Desert: " + desertCount + " Woodland: " + woodlandCount + " Tropical " + tropicalCount;
            triggerUpdate = false;
        }
	}

    public void GetBiomesInit(String type)
    {
        triggerUpdate = true;
        if (type == "Woodland")
            woodlandCount++;
        if (type == "Desert")
            desertCount++;
        if (type == "Tropical")
            tropicalCount++;
        if (type == "Arctic")
            arcticCount++;
    }
}
