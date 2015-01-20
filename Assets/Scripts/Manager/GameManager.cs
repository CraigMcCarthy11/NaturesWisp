using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public Biome test;
    public static List<GameObject> MapAnchors = new List<GameObject>();

    private int MAX_STARTING_WISPS = 8;
    private int MIN_STARTING_WISPS = 4;

    private int MAX_STARTING_HEALTH = 110;
    private int MIN_STARTING_HEALTH = 80;
    

	// Use this for initialization
	void Start () {
        GenerateRandomBiomeType();
        GenerateRandomFactionType();
        //test = new Biome(Biome.BiomeTypes.Arctic, Faction.FactionTypes.ArcticFaction, );
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Biome GenerateBiomeData()
    {
        Biome biome = new Biome();

        //Set random Data
        Faction.FactionTypes BiomeType = GenerateRandomFactionType();
        Biome.BiomeTypes FactionType = GenerateRandomBiomeType();
        int numberOfStartingWisps = UnityEngine.Random.Range(MIN_STARTING_WISPS, MAX_STARTING_HEALTH);
        int health = UnityEngine.Random.Range(MIN_STARTING_HEALTH, MAX_STARTING_HEALTH);
        //biome.Init(BiomeType, FactionType, numberOfStartingWisps, health);
        return biome; 
    }

    public Biome.BiomeTypes GenerateRandomBiomeType()
    {
        Array values = Biome.BiomeTypes.GetValues(typeof(Biome.BiomeTypes));
        System.Random random = new System.Random();
        Biome.BiomeTypes randomBiome = (Biome.BiomeTypes)values.GetValue(random.Next(values.Length));
        Debug.Log(randomBiome);
        return randomBiome;
    }

    public Faction.FactionTypes GenerateRandomFactionType()
    {
        Array values = Biome.BiomeTypes.GetValues(typeof(Faction.FactionTypes));
        System.Random random = new System.Random();
        Faction.FactionTypes randomFaction = (Faction.FactionTypes)values.GetValue(random.Next(values.Length));
        Debug.Log(randomFaction);
        return randomFaction;
    }
}
