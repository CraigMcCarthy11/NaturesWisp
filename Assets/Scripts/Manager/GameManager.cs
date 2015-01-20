using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    [SerializeField]
    public static List<GameObject> MapAnchors = new List<GameObject>();
    private System.Random rand = new System.Random();

    public T RandomEnum<T>()
    {
        T[] values = (T[])Enum.GetValues(typeof(T));
        return values[rand.Next(0, values.Length)];
    }

    public void BuildBiomes()
    {
        if (MapAnchors == null)
        {
            Debug.LogError("MapAnchors is empty");
            return;
        }

        foreach (GameObject square in MapAnchors)
        {
            //Add and Access the biome
            Biome thisBiome = square.AddComponent<Biome>().GetComponent<Biome>();
            //Generate its data
            thisBiome.GenerateBiomeData();
        }
    }
}
