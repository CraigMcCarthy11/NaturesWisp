﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Wisp : MonoBehaviour {

    public Faction.FactionTypes faction;
    public int health;
    public GameObject Home;
    public GameObject Target;
    public int RotationSpeed = 20;
    public int MovemnetSpeed = 15;
    public bool isMoving;
    public Attitude myAttitude;
    public Want myWant;
    public Action myAction;

    /// <summary>
    /// Sets init Data, This is called right after instatiation 
    /// </summary>
    public void BuildWispWithData(int health, Faction.FactionTypes faction, Action myAction, Attitude myAttitude, Want myWant, GameObject Home)
    {
        this.myAction = myAction;
        this.myAttitude = myAttitude;
        this.myWant = myWant;
        this.health = health;
        this.faction = faction;
        this.gameObject.name = "Wisp_" + this.faction.ToString() + "_" + System.Guid.NewGuid();
        this.Home = Home;

        //Set our home to our target(the first object to revole around)
        Target = Home;

        AnimateVisuals();
    }

    void Start()
    {
    }

    void Update()
    {
        //Check if we are moving
        if (this.transform.position == Target.transform.position)
        {
            //If not switch to a defualt state
            //TODO: this may not be modular, becasue what happens after start up
            myAction = Action.Idling;
            isMoving = false;
        }

        //If we are Idling, rotate around out home or target
        if(myAction == Action.Idling) 
            this.transform.RotateAround(Target.transform.position, Vector3.up, RotationSpeed * Time.deltaTime);

        if (myAction == Action.Moving && isMoving == false)
        {
            StartCoroutine(MoveToTarget(new Vector3(Target.transform.position.x + 3, Target.transform.position.y, Target.transform.position.z), 20)); //Offeting so we can call RotateAround()
        }

    }

    /// <summary>
    /// The Actions of the Wisp
    /// </summary>
    public enum Action
    {
        Moving, 
        Idling,
        Fighting,
        Spawning,
        Dieing,
        Nothing
    }

    /// <summary>
    /// Its Attitude of and how it reacts to other Factions
    /// </summary>
    public enum Attitude
    {
        Scared,
        Aggresive,
        Neutral,
        Hostile,
    };

    /// <summary>
    /// How it Interacts with Other Factions
    /// </summary>
    public enum Want
    {
        Nothing,
        Woodland,
        Desert,
        Tropical,
        Arctic,
        Everything
    };

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

    public IEnumerator MoveToTarget(Vector3 Target, int overTime)
    {
        isMoving = true;
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            float step = MovemnetSpeed * Time.deltaTime;
            this.transform.position = Vector3.MoveTowards(transform.position, Target, step);
            yield return null;
        }
    }
}
