using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Biome : MonoBehaviour
{
    public enum BiomeTypes
    {
        Woodland,
        Desert,
        Tropical,
        Arctic
    };

    //Public
    public BiomeTypes myBiome;
    public Faction.FactionTypes myFaction;
    public int health;

    //Private
    public int numberOfStartingWisps;
    private System.Random rand = new System.Random();
    private int MAX_STARTING_WISPS = 8;
    private int MIN_STARTING_WISPS = 4;
    private int MAX_STARTING_HEALTH = 110;
    private int MIN_STARTING_HEALTH = 80;

    public void GenerateBiomeData()
    {
        myBiome = RandomEnum<BiomeTypes>();
        myFaction = (Faction.FactionTypes)((int)myBiome); //As long as the enums are the same, type cast to an int, then set it as a enum of that type, should be the same!
        numberOfStartingWisps = UnityEngine.Random.Range(MIN_STARTING_WISPS, MAX_STARTING_WISPS);
        health = UnityEngine.Random.Range(MIN_STARTING_HEALTH, MAX_STARTING_HEALTH);
    }

    /// <summary>
    /// This method will actually create the game object in the screen. 
    /// It only gets called in the constructor after we have generated all the data to build
    /// the biome
    /// </summary>
    private void SpawnVisuals()
    {

    }

    public T RandomEnum<T>()
    {
        System.Array enumArray = System.Enum.GetValues(typeof(T));
        T theEnum = (T)enumArray.GetValue(UnityEngine.Random.Range(0, enumArray.Length));
        return theEnum;
    }
}
