﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Wisp : MonoBehaviour {

    public Faction.FactionTypes faction;
    public int health;
    public GameObject Home;
    public GameObject Target;
    public int RotationSpeed = 90;
    public int MovemnetSpeed = 15;
    public bool isMoving;
    public float speed = 2;
    public Attitude myAttitude;
    public Want myWant;
    public Action myAction;
    public float idleRotX,idleRotY,idleRotZ;
    public GameManager gameManager;

    //All the Enum's used in this class
    #region Wisp Enumorations

    /// <summary>
    /// The Actions of the Wisp
    /// </summary>
    public enum Action
    {
        Initialization,
        Moving,
        Idling,
        Fighting,
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

    #endregion

    /// <summary>
    /// Sets init Data, This is called right after instatiation 
    /// </summary>
    public void BuildWispWithData(int health, Faction.FactionTypes faction, Action myAction, Attitude myAttitude, Want myWant, GameObject Home, GameManager gameManager)
    {
        this.gameManager = gameManager;
        this.myAction = myAction;
        this.myAttitude = myAttitude;
        this.myWant = myWant;
        this.health = health;
        this.faction = faction;
        this.gameObject.name = "Wisp_" + this.faction.ToString() + "_" + Home.name;
        this.Home = Home;
        this.idleRotX = UnityEngine.Random.Range(0, 20000);
        this.idleRotY = UnityEngine.Random.Range(0, 20000);
        this.idleRotZ = UnityEngine.Random.Range(0, 20000);

        //Set our home to our target(the first object to revole around)
        Target = Home;

        AnimateVisuals();
    }

    void Update()
    {
        HandleAction(myAction);
    }

    private void HandleAction(Action action)
    {
        switch(action)
        {
            case Action.Dieing:
                Dieing();
                break;
            case Action.Fighting:
                Fighting();
                break;
            case Action.Idling:
                Idling();
                break;
            case Action.Initialization:
                Initialization();
                break;
            case Action.Moving:
                Moving();
                break;
            case Action.Nothing:
                Debug.Log("Current Action: " + action.ToString());
                break;
        }
    }

    #region Actions Functions

    private void Initialization()
    {
        StartCoroutine("MoveToHome",new Vector3(Home.transform.position.x, Home.transform.position.y, Home.transform.position.z)); 
        if (this.transform.position == Home.transform.position)
        {
            //Offeting so we can call RotateAround()
            this.transform.position = new Vector3(transform.position.x + 12, transform.position.y, transform.position.z);
            myAction = Action.Idling;
            StopCoroutine("MoveToHome");
        }
    }

    private void Moving()
    {
        
        try
        {
            if (myAction == Action.Moving && isMoving == false)
            {
                StartCoroutine(MoveToTarget(new Vector3(Target.transform.position.x + 3, Target.transform.position.y, Target.transform.position.z), 20)); //Offeting so we can call RotateAround()
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("[SELF] Error moving to target. Target null? : " + ex);
        }
    }

    private void Idling()
    {
        try
        {
            this.transform.RotateAround(Target.transform.position, new Vector3(idleRotX,idleRotY,idleRotZ), RotationSpeed * Time.deltaTime);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("[SELF] Error rotating around object. Target null? : " + ex);
        }
    }

    private void Fighting()
    {
        if(transform.position == Target.transform.position)
        {
            this.transform.RotateAround(Target.transform.position, new Vector3(idleRotX + UnityEngine.Random.Range(0, 360), idleRotY + UnityEngine.Random.Range(0, 360), idleRotZ + UnityEngine.Random.Range(0, 360)), RotationSpeed * Time.deltaTime);
        }

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(Target.transform.position.x + 3, Target.transform.position.y, Target.transform.position.z), step);
    }

    private void Dieing()
    {

    }

    #endregion

    /// <summary>
    /// Loopes throught the ParticleAnimator setting its colors based on a hex code 
    /// </summary>
    private void AnimateVisuals()
    {
        //Assigning a color
        switch (faction.ToString())
        {
            case "Arctic":
                GameObject obj = Instantiate(gameManager.WispHaloPrefabs[0], this.transform.position, Quaternion.identity) as GameObject;
                obj.transform.parent = this.gameObject.transform;
                this.gameObject.GetComponent<Renderer>().material.color = HexToColor("0AF3FF");
                break;
            case "Desert":
                GameObject obj2 = Instantiate(gameManager.WispHaloPrefabs[1], this.transform.position, Quaternion.identity) as GameObject;
                obj2.transform.parent = this.gameObject.transform;
                this.gameObject.GetComponent<Renderer>().material.color = HexToColor("FFD900");
                break;
            case "Tropical":
                GameObject obj3 = Instantiate(gameManager.WispHaloPrefabs[2], this.transform.position, Quaternion.identity) as GameObject;
                obj3.transform.parent = this.gameObject.transform;
                this.gameObject.GetComponent<Renderer>().material.color = HexToColor("2BFF00");
                break;
            case "Woodland":
                GameObject obj4 = Instantiate(gameManager.WispHaloPrefabs[3], this.transform.position, Quaternion.identity) as GameObject;
                obj4.transform.parent = this.gameObject.transform;
                this.gameObject.GetComponent<Renderer>().material.color = HexToColor("826042");
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

    public IEnumerator MoveToHome(Vector3 myHome)
    {
        isMoving = true;
        int overTime = 20;
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            float step = MovemnetSpeed * Time.deltaTime;
            this.transform.position = Vector3.MoveTowards(transform.position, myHome, step);
            yield return null;
        }
    }
}
