using UnityEngine;
using System.Collections;

public class Biome : MonoBehaviour
{
    public enum BiomeTypes
    {
        Woodland,
        Mountians,
        Tropical,
        Arctic
    };

    //Public
    public BiomeTypes myBiome;
    public Faction.FactionTypes myFaction;
    public int health;

    //Private
    private int numberOfStartingWisps;
    
    //Constructor
    public void Init(BiomeTypes setBiome, Faction.FactionTypes setFaction, int setStartingWisps, int setHealth)
    {
        //Set all of our data here
        health = setHealth;
        myBiome = setBiome;
        myFaction = setFaction;
        numberOfStartingWisps = setStartingWisps;

        BuildWithData();
    }

    /// <summary>
    /// This method will actually create the game object in the screen. 
    /// It only gets called in the constructor after we have generated all the data to build
    /// the biome
    /// </summary>
    private void BuildWithData()
    {

    }
}
