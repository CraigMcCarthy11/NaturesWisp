using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Wisp : MonoBehaviour {

    public Faction.FactionTypes faction;
    public int health;
    public struct currentState
    {
        public Attitude myAttitude;
        public Want myWant;
    }

    public currentState myState;

    public void BuildWispWithData(int health, Faction.FactionTypes faction, currentState setCurrentState)
    {
        this.health = health;
        this.faction = faction;
        this.gameObject.name = "Wisp_" + this.faction.ToString() + "_" + System.Guid.NewGuid();
    }

    public enum Attitude
    {
        Scared,
        Aggresive,
        Neutral,
        Hostile,
    };

    public enum Want
    {
        Nothing,
        Woodland,
        Desert,
        Tropical,
        Arctic,
        Everything
    };

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
