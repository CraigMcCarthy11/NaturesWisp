using UnityEngine;
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
