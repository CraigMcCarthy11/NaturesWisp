using System.Collections.Generic;
using UnityEngine;

public class Faction : MonoBehaviour {

	//Public
    public enum FactionTypes
    {
        Woodland,
        Desert,
        Tropical,
        Arctic
    };

    public FactionTypes myFaction;
    public List<FactionTypes> Enemies;
    public List<FactionTypes> Allies;

    //Private
    
    
    //Construct
    public void InitFaction(FactionTypes myFaction, List<Faction.FactionTypes> Enemies, List<Faction.FactionTypes> Allies)
    {
        this.Enemies = Enemies;
        this.Allies = Allies;
        this.myFaction = myFaction;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="potentialEnemy"></param>
    /// <returns>if the gameobject is an emeny returns true</returns>
    public bool isEnemyOrFoe(GameObject potentialEnemy)
    {
        if (Enemies[0] == potentialEnemy.GetComponent<Biome>().myFaction)
            return true;
        //Then check the secound
        if (Enemies[1] == potentialEnemy.GetComponent<Biome>().myFaction)
            return true;
        else
        return false;
    }
}
