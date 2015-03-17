using UnityEngine;
using System.Collections;

public class UsefulPrefabs : MonoBehaviour
{
    private static UsefulPrefabs gInstance;

    public GameObject RageHalo;

    /// <summary>
    /// Constructor that handles getting ang setting the instaance
    /// this is using the singleton pattern
    /// </summary>
    public static UsefulPrefabs instance
    {
        get
        {
            if (gInstance == null)
            {
                gInstance = GameObject.FindObjectOfType<UsefulPrefabs>();

                //Tell unity not to destroy this object when loading a new scene!
                DontDestroyOnLoad(gInstance.gameObject);
            }
            return gInstance;
        }
    }

    void Awake()
    {

        if (gInstance == null)
        {
            //If I am the first instance, make me the Singleton
            gInstance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            //If a Singleton already exists and you find
            //another reference in scene, destroy it!
            if (this != gInstance)
                Destroy(this.gameObject);
        }
    }
}
