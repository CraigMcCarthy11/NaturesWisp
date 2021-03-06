﻿using UnityEngine;
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

    //Static
    private static float timeToSpawnMod = 0;
    private static int initDepth = 10;
    private static int gracePeriod = 10;

    //Public
    public BiomeTypes myBiome;
    public UIManager uiManager;
    public Faction.FactionTypes myFaction;
    public int health;
    public GameManager gameManager;
    public List<GameObject> Neighbors = new List<GameObject>();
    public List<GameObject> myWisps = new List<GameObject>();
    public int numberOfStartingWisps;
    public int angerValue = 0;
    public float angerRefreshRate = 2f;

    //Private
    private int numberOfNeighborsToHave = 8;
    private int MAX_STARTING_WISPS = 8;
    private int MIN_STARTING_WISPS = 4;
    private int MAX_STARTING_HEALTH = 110;
    private int MIN_STARTING_HEALTH = 80;
    private float timer = 1f;
    



    void Update()
    {
        //Every one secound check to, increase rage
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = angerRefreshRate;
            foreach(GameObject biome in Neighbors)
            {
                try
                {
                    //if we are surrounded by an enemy
                    if (GetComponent<Faction>().isEnemyOrFoe(biome))
                    {
                        //increase anger
                        angerValue++;
                    }
                }
                catch (System.Exception ex)
                {
                }
            }
        }

        if(angerValue >= 100)
        {
            StartWar();
            angerValue = -400;
        }
    }

    public void StartWar()
    {
        //We do this so everyone doesn't rage at once
        if(GameManager.CurrentRagers.Count >= GameManager.MAX_NUM_OF_RAGERS)
        {
            angerValue = -200;
            return;
        }

        GameManager.CurrentRagers.Add(this.gameObject);
            
        //Show we are starting war
        Instantiate(UsefulPrefabs.instance.RageHalo, this.transform.position, Quaternion.identity);
        //Prepare to store all neibors that are enemies
        List<GameObject> potentialEneimes = new List<GameObject>();
        //loop
        foreach (GameObject biome in Neighbors)
        {
            try
            {
                //if they are an enemy
                if (GetComponent<Faction>().isEnemyOrFoe(biome))
                {
                    //add
                    potentialEneimes.Add(biome);
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        //Grab a random enemy
        if (potentialEneimes.Count != 0) //We do this check because we might be surrounded by no enemies.
        {
            GameObject EnemyToAttack = potentialEneimes[UnityEngine.Random.Range(1, potentialEneimes.Count)];

            foreach(GameObject wisp in myWisps)
            {
                wisp.GetComponent<Wisp>().Target = EnemyToAttack;
                wisp.GetComponent<Wisp>().myAction = Wisp.Action.Fighting;
            }
        }
    }

    /// <summary>
    /// This fucntion generates data that will be asigned to this script when its instantiated 
    /// </summary>
    public void GenerateBiomeData(UIManager uiManager)
    {
        myBiome = RandomEnum<BiomeTypes>();
        myFaction = (Faction.FactionTypes)((int)myBiome); //As long as the enums are the same, type cast to an int, then set it as a enum of that type, should be the same!
        this.uiManager = uiManager;
        uiManager.GetBiomesInit(myFaction);
        numberOfStartingWisps = UnityEngine.Random.Range(MIN_STARTING_WISPS, MAX_STARTING_WISPS);
        health = UnityEngine.Random.Range(MIN_STARTING_HEALTH, MAX_STARTING_HEALTH);
        //this.angerValue = UnityEngine.Random.Range(-50, -100);
        this.angerValue = 1;
    }

    /// <summary>
    /// This method will actually create the game object in the screen. 
    /// It only gets called in the constructor after we have generated all the data to build
    /// the biome
    /// </summary>
    public IEnumerator SpawnVisuals(Faction.FactionTypes factionToSpawn)
    {
        timeToSpawnMod += 0.5f;
        yield return new WaitForSeconds(timeToSpawnMod);
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
            Debug.LogError("Couldn't find prefab in List, probably doesn't exsist: " + ex);
        }

        //Create the Dust effect
        //NOTE: DustDestoryScript, handles destorying
        GameObject dust = Instantiate(gameManager.DustPrefab, this.transform.position, Quaternion.identity) as GameObject;

        Vector3 posToSpawnBelow = new Vector3(this.transform.position.x,this.transform.position.y - initDepth, this.transform.position.z);
        GameObject biomeObject = Instantiate(BiomePrefab, posToSpawnBelow, Quaternion.identity) as GameObject;

        //Set it as the child to this obj
        biomeObject.transform.parent = this.transform;
 
        //Animate it through code and wait to finish
        posToSpawnBelow.y += 10;
        StartCoroutine(PlayStartUpEffect(biomeObject,posToSpawnBelow, 50f));
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

    /// <summary>
    /// This has a basic timer that Lerps a gameObject to a target over a certian amount of time
    /// </summary>
    /// <param name="objToMove">The GameObject to move</param>
    /// <param name="target">Its desiered pos</param>
    /// <param name="overTime">the Time it takes to do that</param>
    /// <returns></returns>
    private IEnumerator PlayStartUpEffect(GameObject objToMove, Vector3 target, float overTime)
    {
        float startTime = Time.time;
        while(Time.time < startTime + overTime)
        {
            objToMove.transform.position = Vector3.Lerp(objToMove.transform.position, target, (Time.time - startTime) / overTime);
            transform.position = target;
            yield return null;
        }
    }

    public void FindAndSetNeighbors()
    {
        //Find neighbors based on my distance
        try
        {
            List<GameObject> TempNeighbors = new List<GameObject>(GameManager.MapAnchors);
            TempNeighbors.Sort(SortByDistance);
            //Sort the top 5 (closest) in the list as Neighbors NOTE: Skipping the first one in the list because it myself
            for (int i = 0; i < numberOfNeighborsToHave; i++)
            {
                Neighbors.Add(TempNeighbors[i]);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("[SELF] Error finding and/or setting Neighbors becasue: " + ex);
        }
    }

    public int SortByDistance(GameObject a, GameObject b)
    {
        var dstToA = Vector3.Distance(transform.position, a.transform.position);
        var dstToB = Vector3.Distance(transform.position, b.transform.position);
        return dstToA.CompareTo(dstToB);
    }

    /*void OnMouseDown()
    {
        Debug.Log("You've Clicked " + this.gameObject.name);
    }*/

    /// <summary>
    /// If the wisp are assigned a target via this biome, loop through all of yours them tell'em to move
    /// </summary>
    /// <param name="Target">The GameObject to move towards</param>
    public void AttackTarget(GameObject Target)
    {
        //Skip me if my wisps haven't spawend yet or......dead
        if (myWisps.Count != 0)
        {
            foreach (GameObject wisp in myWisps)
            {
                StartCoroutine(wisp.GetComponent<Wisp>().MoveToTarget(Target.transform.position, 10));
            }
        }
    }

}
