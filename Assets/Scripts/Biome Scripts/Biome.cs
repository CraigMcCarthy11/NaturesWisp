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
    [SerializeField]
    private int numberOfStartingWisps;
    private System.Random rand = new System.Random();
    private int MAX_STARTING_WISPS = 8;
    private int MIN_STARTING_WISPS = 4;
    private int MAX_STARTING_HEALTH = 110;
    private int MIN_STARTING_HEALTH = 80;

    public void GenerateBiomeData()
    {
        myBiome = RandomEnum<BiomeTypes>();
        myFaction = RandomEnum<Faction.FactionTypes>();
        numberOfStartingWisps = UnityEngine.Random.Range(MIN_STARTING_WISPS, MAX_STARTING_HEALTH);
        health = UnityEngine.Random.Range(MIN_STARTING_HEALTH, MAX_STARTING_HEALTH);
    }

    /// <summary>
    /// This method will actually create the game object in the screen. 
    /// It only gets called in the constructor after we have generated all the data to build
    /// the biome
    /// </summary>
    private void BuildWithData()
    {

    }

    public T RandomEnum<T>()
    {
        T[] values = (T[])Enum.GetValues(typeof(T));
        return values[rand.Next(0, values.Length)];
    }
}
