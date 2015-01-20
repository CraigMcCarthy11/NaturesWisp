using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public static List<GameObject> MapAnchors = new List<GameObject>();

    public enum BiomeTypes
    {
        Woodland,
        Mountians,
        Tropical,
        Arctic
    };

    public TileMap tileMap;

    private int maxBiomeTypes = 8;

    void Start()
    {
        GenerateGame();
    }

    public void GenerateGame()
    {
        tileMap.BuildMesh();
        tileMap.BuildTexture();
        tileMap.CalclateGridCenterPoints();
        GenerateBiomePieces();
    }

    public void GenerateBiomePieces()
    {


        foreach (GameObject anchor in MapAnchors)
        {
            //anchor.AddComponent<Biome>();

        }
    }
}
