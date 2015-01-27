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
    public GameManager gameManager;

    //Private
    public int numberOfStartingWisps;
    private System.Random rand = new System.Random();
    private int MAX_STARTING_WISPS = 8;
    private int MIN_STARTING_WISPS = 4;
    private int MAX_STARTING_HEALTH = 110;
    private int MIN_STARTING_HEALTH = 80;

    /// <summary>
    /// This fucntion generates data that will be asigned to this script when its instantiated 
    /// </summary>
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
    public void SpawnVisuals(Faction.FactionTypes factionToSpawn)
    {
        GameObject BiomePrefab =  null;
        try
        {
            foreach (GameObject biome in gameManager.BiomePrefabs)
            {
                if (biome.name == factionToSpawn.ToString())
                {
                    BiomePrefab = biome;
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Couldn't find prefab to spawn because: " + ex);
            return;
        }

        Vector3 posToSpawnBelow = new Vector3(this.transform.position.x,this.transform.position.y, this.transform.position.z);
        GameObject biomeObject = Instantiate(BiomePrefab, posToSpawnBelow, Quaternion.identity) as GameObject;
        //Set it as the child to this obj
        biomeObject.transform.parent = this.transform; 
    }

    /// <summary>
    /// Random enum generator, caution, don't use in quick sucsesstion. I.E: Update, foreach loop()...
    /// Unity Random generation is truely random, rather its baded of the system time, so if you call it alot there
    /// is a likly hood of getting the same value after multiple calls
    /// </summary>
    /// <typeparam name="T">Must be and enumoratior</typeparam>
    /// <returns>Random Enum</returns>
    public T RandomEnum<T>()
    {
        System.Array enumArray = System.Enum.GetValues(typeof(T));
        T theEnum = (T)enumArray.GetValue(UnityEngine.Random.Range(0, enumArray.Length));
        return theEnum;
    }
}
