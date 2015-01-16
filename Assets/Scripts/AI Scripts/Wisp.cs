using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wisp : MonoBehaviour {

    public Faction.FactionTypes faction;
    public int health;
    public List<Faction.FactionTypes> Enemies = new List<Faction.FactionTypes>(); 
    public List<Faction.FactionTypes> Allies = new List<Faction.FactionTypes>();

    public enum Attitude
    {
        Scared,
        Aggresive,
        Neutral,
        Hostile,
    };

    public Attitude currentAttitude;

    public enum Want
    {
        Nothing,
        Woodland,
        Mountians,
        Tropical,
        Arctic,
        Everything
    };

    public Want currentWant;

    public void Initalize()
    {

    }

	void Start () {
	}
	
	void Update () {
	
	}

    public void SpawnSelf()
    {

    }

    private Attitude HandleAttitude()
    {
        Attitude newAttitude = Attitude.Neutral; 
        return newAttitude;
    }

    private Want HandleWant()
    {
        Want newWant = Want.Nothing;
        return newWant;
    }
}
