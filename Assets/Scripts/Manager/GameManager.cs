using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public Biome test;
    

	// Use this for initialization
	void Start () {

        test = new Biome(Biome.BiomeTypes.Mountians);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
