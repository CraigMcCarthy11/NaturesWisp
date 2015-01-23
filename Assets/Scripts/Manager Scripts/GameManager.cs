using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    [SerializeField]
    public static List<GameObject> MapAnchors = new List<GameObject>();
    [SerializeField]
    public static List<GameObject> SpawnedWisps = new List<GameObject>();
    public TileMap tilemap;
    public List<Faction> Factions = new List<Faction>();

    //GameObjects
    public GameObject WispPrefab;

    void Start()
    {
        SetFactions();
        Debug.Log("Factions Built");
        tilemap.BuildMesh();
        Debug.Log("Mesh Built");
        tilemap.BuildTexture();
        Debug.Log("Texture Built");
        tilemap.CalclateGridCenterPoints();
        Debug.Log("Anchors Spawned");
        BuildBiomes();
        Debug.Log("Created Biome");
    }

    public void SetFactions()
    {
        //============= Arctics ===============
        //Allies
        List<Faction.FactionTypes> ArcticsAllies = new List<Faction.FactionTypes>();
        ArcticsAllies.Add(Faction.FactionTypes.Woodland);
        
        //Enemies
        List<Faction.FactionTypes> ArcticEnemies = new List<Faction.FactionTypes>();
        ArcticEnemies.Add(Faction.FactionTypes.Desert);
        ArcticEnemies.Add(Faction.FactionTypes.Tropical);

        //Create and add to master list
        Faction Arctic = new Faction(Faction.FactionTypes.Arctic, ArcticEnemies, ArcticsAllies);
        Factions.Add(Arctic);

        //============= Desert ===============
        //Allies
        List<Faction.FactionTypes> DesertAllies = new List<Faction.FactionTypes>();
        DesertAllies.Add(Faction.FactionTypes.Tropical);

        //Enemies
        List<Faction.FactionTypes> DesertEnemies = new List<Faction.FactionTypes>();
        DesertEnemies.Add(Faction.FactionTypes.Woodland);
        DesertEnemies.Add(Faction.FactionTypes.Arctic);

        //Create and add to master list
        Faction Desert = new Faction(Faction.FactionTypes.Desert, ArcticEnemies, ArcticsAllies);
        Factions.Add(Desert);


        //============= Woodland ===============
        //Allies
        List<Faction.FactionTypes> WoodlandAllies = new List<Faction.FactionTypes>();
        WoodlandAllies.Add(Faction.FactionTypes.Arctic);

        //Enemies
        List<Faction.FactionTypes> WoodlandEnemies = new List<Faction.FactionTypes>();
        WoodlandEnemies.Add(Faction.FactionTypes.Desert);
        WoodlandEnemies.Add(Faction.FactionTypes.Tropical);

        //Create and add to master list
        Faction Woodland = new Faction(Faction.FactionTypes.Woodland, ArcticEnemies, ArcticsAllies);
        Factions.Add(Woodland);

        //============= Tropical ===============
        //Allies
        List<Faction.FactionTypes> TropicalAllies = new List<Faction.FactionTypes>();
        TropicalAllies.Add(Faction.FactionTypes.Desert);

        //Enemies
        List<Faction.FactionTypes> TropicalEnemies = new List<Faction.FactionTypes>();
        TropicalEnemies.Add(Faction.FactionTypes.Arctic);
        TropicalEnemies.Add(Faction.FactionTypes.Woodland);

        //Create and add to master list
        Faction Tropical = new Faction(Faction.FactionTypes.Woodland, ArcticEnemies, ArcticsAllies);
        Factions.Add(Tropical);
    }

    public void BuildBiomes()
    {
        if (MapAnchors == null)
        {
            Debug.LogError("MapAnchors List is empty, something be goofed");
            return;
        }

        foreach (GameObject square in MapAnchors)
        {
            //Add and Access the biome
            Biome thisBiome = square.AddComponent<Biome>().GetComponent<Biome>();
            //Generate its data
            thisBiome.GenerateBiomeData();
            //Spawn Wisps based on random number
            for (int i = 0; i < thisBiome.numberOfStartingWisps; i++)
            {
                SpawnWisp(square.transform.position, thisBiome.myFaction);
            }
        }
        Debug.Log("Created Biomes");
        Debug.Log("Spawned Wisp");
    }

    public void SpawnWisp(Vector3 biomePos, Faction.FactionTypes setFaction)
    {
        //Spawn in Scene, on the biome pos
        GameObject wispObj = Instantiate(WispPrefab, biomePos, Quaternion.identity) as GameObject;

        //Instantiate a Wisp Class on the newly created wispObj, 
        Wisp wispScript = wispObj.AddComponent<Wisp>().GetComponent<Wisp>();

        //Creating this struct so we can pass it in below
        Wisp.currentState wispsStartingState = new Wisp.currentState();
        /*
         * TODO: Setting these values? or maybe random? But they would also have to be the same for
         * all wisps in that Biome
         */ 
        wispsStartingState.myAttitude = Wisp.Attitude.Neutral; 
        wispsStartingState.myWant = Wisp.Want.Nothing;

        //Set Data to that Instance on that Obj
        wispScript.BuildWispWithData(
            UnityEngine.Random.Range(80, 110), //Its health with a bit of Randomness 
            setFaction,                        //Its Faction (Same as the Biome's)
            wispsStartingState
            );

        //Add to our master list 
        SpawnedWisps.Add(wispObj);
    }
}
