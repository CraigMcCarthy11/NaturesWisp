using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    public static List<GameObject> MapAnchors = new List<GameObject>();
    public TileMap tilemap;
    public UIManager uiManager;
    public Dictionary<Faction.FactionTypes, Faction> FactionsMasterDic = new Dictionary<Faction.FactionTypes, Faction>();
    public GameObject[] WispHaloPrefabs;
    public AudioManager audioMan;
    //GameObjects
    public GameObject WispPrefab;
    public List<GameObject> BiomePrefabs = new List<GameObject>();
    public GameObject DustPrefab;
    public GameObject SwapBiome1;
    public GameObject SwapBiome2;

    //bools for what skill is about to be used 
    public bool bombBool = false;
    public bool swapBool = false;
    public bool wispAddBool = false;

    public Material materialHighlight;
    private List<Material> materialOriginals = new List<Material>();
    public Shader shader1;
    private Shader shader2;
    

    void Start()
    {
        SetFactions();
        Debug.Log("Factions Built");
        tilemap.BuildMesh();
        Debug.Log("Mesh Built");
        //tilemap.BuildTexture();
        Debug.Log("Texture Built");
        tilemap.CalclateGridCenterPoints();
        Debug.Log("Anchors Spawned");
        BuildBiomes();
        Debug.Log("Created Biome");
        audioMan.PlayAudio(audioMan.SoundClips[0], Camera.main.transform.position);
        shader2 = Shader.Find("Diffuse");
    }

    public void SetFactions()
    {
        #region Arctic
        //Allies
        List<Faction.FactionTypes> ArcticsAllies = new List<Faction.FactionTypes>();
        ArcticsAllies.Add(Faction.FactionTypes.Woodland);

        //Enemies
        List<Faction.FactionTypes> ArcticEnemies = new List<Faction.FactionTypes>();
        ArcticEnemies.Add(Faction.FactionTypes.Desert);
        ArcticEnemies.Add(Faction.FactionTypes.Tropical);

        //Create and add to master list
        Faction Arctic = new Faction();
        Arctic.InitFaction(Faction.FactionTypes.Arctic, ArcticEnemies, ArcticsAllies);
        FactionsMasterDic.Add(Faction.FactionTypes.Arctic, Arctic);
        #endregion

        #region Desert
        //Allies
        List<Faction.FactionTypes> DesertAllies = new List<Faction.FactionTypes>();
        DesertAllies.Add(Faction.FactionTypes.Tropical);

        //Enemies
        List<Faction.FactionTypes> DesertEnemies = new List<Faction.FactionTypes>();
        DesertEnemies.Add(Faction.FactionTypes.Woodland);
        DesertEnemies.Add(Faction.FactionTypes.Arctic);

        //Create and add to master list
        Faction Desert = new Faction();
        Desert.InitFaction(Faction.FactionTypes.Desert, DesertAllies, DesertEnemies);
        FactionsMasterDic.Add(Faction.FactionTypes.Desert, Desert);
        #endregion

        #region Woodland
        //Allies
        List<Faction.FactionTypes> WoodlandAllies = new List<Faction.FactionTypes>();
        WoodlandAllies.Add(Faction.FactionTypes.Arctic);

        //Enemies
        List<Faction.FactionTypes> WoodlandEnemies = new List<Faction.FactionTypes>();
        WoodlandEnemies.Add(Faction.FactionTypes.Desert);
        WoodlandEnemies.Add(Faction.FactionTypes.Tropical);

        //Create and add to master list
        Faction Woodland = new Faction();
        Woodland.InitFaction(Faction.FactionTypes.Woodland, ArcticEnemies, ArcticsAllies);
        FactionsMasterDic.Add(Faction.FactionTypes.Woodland, Woodland);
        #endregion

        #region Tropical
        //Allies
        List<Faction.FactionTypes> TropicalAllies = new List<Faction.FactionTypes>();
        TropicalAllies.Add(Faction.FactionTypes.Desert);

        //Enemies
        List<Faction.FactionTypes> TropicalEnemies = new List<Faction.FactionTypes>();
        TropicalEnemies.Add(Faction.FactionTypes.Arctic);
        TropicalEnemies.Add(Faction.FactionTypes.Woodland);

        //Create and add to master list
        Faction Tropical = new Faction();
        Tropical.InitFaction(Faction.FactionTypes.Tropical, TropicalEnemies, TropicalAllies);
        FactionsMasterDic.Add(Faction.FactionTypes.Tropical, Tropical);
        #endregion
    }
    public void wispButtonPressed()
    {
        wispAddBool = true;
        Debug.Log("Wisp Button");   
    }
    public void swapButtonPressed()
    {
        swapBool = true;
        Debug.Log("Swap Button");  
    }
    public void bombButtonPressed()
    {
        bombBool = true;
        Debug.Log("Destroy Button");
    }

    void Update()
    {
        //StartCoroutine(FindEnemy());
        if (Input.GetMouseButtonDown(0) && bombBool == true && uiManager.bombsCount > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit; if (Physics.Raycast(ray, out hit))
            {
                Destroy(hit.transform.gameObject);
            }
            uiManager.bombsCount--;
            bombBool = false;
        }
        if (Input.GetMouseButtonDown(0) && wispAddBool == true && uiManager.extraWispsCount > 0)
        {
            /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit; if (Physics.Raycast(ray, out hit))
            {
                Destroy(hit.transform.gameObject);
            }*/
            uiManager.extraWispsCount--;
            wispAddBool = false;
        }
        if (Input.GetMouseButtonDown(0) && swapBool == true && uiManager.swapCount > 0)
        {
            // if left button pressed... 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit; if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.name != "TileMap")
                {
                    if (SwapBiome1 == null)
                    {
                        SwapBiome1 = hit.transform.gameObject;
                        Debug.Log("You got biome 1");
                         foreach(Renderer r in SwapBiome1.GetComponentsInChildren<Renderer>()){
                             materialOriginals.Add(r.material);
                             r.material = materialHighlight;
                           }
                        return;
                    }
                    if (SwapBiome2 == null && SwapBiome1 != hit.transform.gameObject)
                    {
                        SwapBiome2 = hit.transform.gameObject;
                        Debug.Log("You got biome 2");
                    }
                    if (SwapBiome1 != null && SwapBiome2 != null)
                    {
                        int i = 0;
                        foreach (Renderer r in SwapBiome1.GetComponentsInChildren<Renderer>())
                        {
                            r.material = materialOriginals[i];
                            i++;
                        }
                        Vector3 temp = Vector3.zero;
                        temp = SwapBiome1.transform.localPosition;
                        SwapBiome1.transform.localPosition = SwapBiome2.transform.localPosition;
                        SwapBiome2.transform.localPosition = temp;
                        temp = Vector3.zero;
                        SwapBiome1 = null;
                        SwapBiome2 = null;
                    }
                }
            }
            uiManager.swapCount--;
            swapBool = false;
        } 

    }

    void BuildBiomes()
    {
        //Setting init data and spawning wisps with it
        foreach (GameObject square in MapAnchors)
        {
            //Add and Access the biome
            Biome thisBiome = square.GetComponent<Biome>();
            //Give it an Instance of GameManager
            thisBiome.gameManager = this;
            //Generate its data
            thisBiome.GenerateBiomeData(uiManager);

            //Add the faction class based on what biome they are
            switch (thisBiome.myFaction)
            {
                case Faction.FactionTypes.Arctic:
                    square.AddComponent<Faction>().InitFaction(Faction.FactionTypes.Arctic, FactionsMasterDic[Faction.FactionTypes.Arctic].Enemies, FactionsMasterDic[Faction.FactionTypes.Arctic].Allies);
                    break;
                case Faction.FactionTypes.Desert:
                    square.AddComponent<Faction>().InitFaction(Faction.FactionTypes.Desert, FactionsMasterDic[Faction.FactionTypes.Desert].Enemies, FactionsMasterDic[Faction.FactionTypes.Desert].Allies);
                    break;
                case Faction.FactionTypes.Tropical:
                    square.AddComponent<Faction>().InitFaction(Faction.FactionTypes.Tropical, FactionsMasterDic[Faction.FactionTypes.Tropical].Enemies, FactionsMasterDic[Faction.FactionTypes.Tropical].Allies);
                    break;
                case Faction.FactionTypes.Woodland:
                    square.AddComponent<Faction>().InitFaction(Faction.FactionTypes.Woodland, FactionsMasterDic[Faction.FactionTypes.Woodland].Enemies, FactionsMasterDic[Faction.FactionTypes.Woodland].Allies);
                    break;
            }

            //Spawn it and make a pretty effect
            StartCoroutine(thisBiome.SpawnVisuals(thisBiome.myFaction));

            //Spawn Wisps based on random number
            for (int i = 0; i < thisBiome.numberOfStartingWisps; i++)
            {
                //yield return new WaitForSeconds(0.5f);
                SpawnWisp(thisBiome.myFaction, square);
            }

        }
        Debug.Log("Created Biomes");
        Debug.Log("Spawned Wisps");
        SetNeighbors();
    }

    public void SetNeighbors()
    {
        foreach(GameObject biome in MapAnchors)
            biome.GetComponent<Biome>().FindAndSetNeighbors();
        
    }

    public void SpawnWisp(Faction.FactionTypes setFaction, GameObject Home)
    {
        //Spawn in Scene, on the at a random point in the sky
        float z = UnityEngine.Random.Range(0f, 250f);
        float y = Camera.main.transform.position.y - 20; //Spawn a little below the player
        float x = UnityEngine.Random.Range(22f, 250f);
        Vector3 RandomSpawnPos = new Vector3(x, y, z);

        GameObject wispObj = Instantiate(WispPrefab, RandomSpawnPos, Quaternion.identity) as GameObject;

        //Instantiate a Wisp Class on the newly created wispObj, 
        Wisp wispScript = wispObj.AddComponent<Wisp>().GetComponent<Wisp>();
        //Creating this struct so we can pass it in below

        /*
         * TODO: Setting these values? or maybe random? But they would also have to be the same for
         * all wisps in that Biome
         */
        Wisp.Attitude myAttitude = Wisp.Attitude.Neutral;
        Wisp.Want myWant = Wisp.Want.Nothing;
        Wisp.Action myAction = Wisp.Action.Initialization;

        //Set Data to that Instance on that Obj
        wispScript.BuildWispWithData(
            UnityEngine.Random.Range(80, 110), //Its health with a bit of Randomness 
            setFaction,                        //Its Faction (Same as the Biome's)
            myAction,
            myAttitude,
            myWant,
            Home,                               //Its Home GameObject (a MapAnchor)
            this
            );

        //Add to our master list 
        Home.GetComponent<Biome>().myWisps.Add(wispObj);
    }

    public void UpdateFaction(Faction.FactionTypes factionToUpdate, List<Faction.FactionTypes> newAllies, List<Faction.FactionTypes> newEniemes)
    {
        //Find out which faction to update and grab it
        Faction newFaction = FactionsMasterDic[factionToUpdate];
        //Update Eneimes
        newFaction.Enemies.Clear();
        newFaction.Enemies = newEniemes;

        //Update Allies
        newFaction.Allies.Clear();
        newFaction.Allies = newAllies;

        //Reset our dictionary
        FactionsMasterDic.Remove(factionToUpdate);
        FactionsMasterDic.Add(factionToUpdate, newFaction); //Re-Add
    }

    public IEnumerator FindEnemy()
    {
        //Wait a certain amount of time
        int timeToWait = UnityEngine.Random.Range(30, 60);
        yield return new WaitForSeconds(timeToWait);
        Debug.Log("Waited: " + timeToWait);
        //After we wait, pick a random biome (Mapanchor) that will be the Attacker, than grab all its scripts for use
        GameObject Attacker = MapAnchors[UnityEngine.Random.Range(0, MapAnchors.Count)]; 
        Biome AttackerBiome = Attacker.GetComponent<Biome>();
        Faction AttackerFaction = Attacker.GetComponent<Faction>();
        /*
        bool foundValidEnemy = false;
        while(foundValidEnemy == false) //While we haven't found a valid enemy
        {*/
            try
            {
                //Pick a random Neighbor from the Attackers list 
                GameObject randomNeigbor = AttackerBiome.Neighbors[UnityEngine.Random.Range(0, AttackerBiome.Neighbors.Count)];
                //Check if the Attackers first enemie is the same as the Neighbor
                if (AttackerFaction.Enemies[0] == randomNeigbor.GetComponent<Biome>().myFaction)
                {
                    //foundValidEnemy = true;
                    Debug.Log(Attacker.name + " is going to start attacking " + randomNeigbor.name);
                    AttackerBiome.AttackTarget(randomNeigbor);
                }
                //Then check the secound
                if (AttackerFaction.Enemies[1] == randomNeigbor.GetComponent<Biome>().myFaction)
                {
                    //foundValidEnemy = true;
                    Debug.Log(Attacker.name + " is going to start attacking " + randomNeigbor.name);
                    AttackerBiome.AttackTarget(randomNeigbor);
                }
            }
            catch (SystemException ex)
            {
                Debug.LogError("[SELF] Attacker could find valid target, Does it have eniems? " + ex);
            }
        //}
    }
}
