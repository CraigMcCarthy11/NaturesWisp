using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Wisp : MonoBehaviour {

    public Faction.FactionTypes faction;
    public int health;
    public GameObject Home;
    public GameObject Target;
    public int MovementSpeed = 20;
    public struct currentState
    {
        public Attitude myAttitude;
        public Want myWant;
        public Action myAction;
    }

    public currentState myState;

    public void BuildWispWithData(int health, Faction.FactionTypes faction, currentState setCurrentState, GameObject Home)
    {
        this.health = health;
        this.faction = faction;
        this.gameObject.name = "Wisp_" + this.faction.ToString() + "_" + System.Guid.NewGuid();
        this.Home = Home;

        //Set our home to our target(the first object to revole around
        Target = Home;

        AnimateVisuals();
    }

    void Update()
    {
        this.transform.RotateAround(Target.transform.position, Vector3.up, MovementSpeed * Time.deltaTime);
    }

    public enum Action
    {
        Moving, 
        Idling,
        Fighting,
        Spawning,
        Dieing
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

    /// <summary>
    /// Loopes throught the ParticleAnimator setting its colors based on a hex code 
    /// </summary>
    private void AnimateVisuals()
    {
        //Assigning a color
        ParticleAnimator particleAnimatior = this.gameObject.GetComponent<ParticleAnimator>();
        switch (faction.ToString())
        {
            case "Arctic":
                particleAnimatior.colorAnimation = HexToColorArray("0AF3FF",5);
                break;
            case "Desert":
                particleAnimatior.colorAnimation = HexToColorArray("FFD900",5);
                break;
            case "Tropical":
                particleAnimatior.colorAnimation = HexToColorArray("2BFF00",5);
                break;
            case "Woodland":
                particleAnimatior.colorAnimation = HexToColorArray("826042",5);
                break;
            default:
                Debug.LogError("Could not set color, no valid faction type set");
                break;
        }
    }

    /// <summary>
    /// Takes a string as a hex code and converts it to a Unity Color Object
    /// for hex codes go to: http://www.colorpicker.com/ 
    /// </summary>
    /// <param name="hex">The Hex code, NO HASH TAG</param>
    /// <returns></returns>
    private Color HexToColor(string hex)
    {
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r, g, b, 255);
    }

    private Color[] HexToColorArray(string hex, int arraySize)
    {
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        Color[] colorArray = new Color[arraySize];
        for (int i = 0; i < arraySize; i++)
        {
            colorArray[i] = new Color32(r, g, b, 255);
        }
        return colorArray;
    }
}
