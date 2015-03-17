using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

    public void OnPlayPress()
    {
        Application.LoadLevel("Game");
    }
}
