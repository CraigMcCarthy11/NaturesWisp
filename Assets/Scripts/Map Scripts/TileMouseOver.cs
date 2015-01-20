using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TileMap))]
public class TileMouseOver : MonoBehaviour
{
    TileMap tileMap;

    Vector3 currentTileCoordinate;

    public Transform testWisp;

    void Start()
    {
        tileMap = GetComponent<TileMap>();
    }
    // Update is called once per frame
    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (collider.Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            //Fancy math to find the point relative to the local matrix instead of world (thanks unity)
            transform.worldToLocalMatrix.MultiplyPoint3x4(hitInfo.point);
            //Sets the floats to ints
            int x = Mathf.FloorToInt(hitInfo.point.x / tileMap.tileSize);
            int z = Mathf.FloorToInt(hitInfo.point.z / tileMap.tileSize);
            //Debug.Log("Tile: " + x + " , " + z);

            currentTileCoordinate.x = x;
            currentTileCoordinate.z = z;

            testWisp.transform.position = currentTileCoordinate;
        }
        else
        {
            //whatever else
            //TODO: Make a MouseManager probably to handle mouse inputs?
        }

        if (Input.GetMouseButton(0))
        {
            //Event goes here
            Debug.Log("Clickarino");
        }

    }

}
