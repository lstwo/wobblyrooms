using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ModWobblyLife;
using ModWobblyLifeEditor;

public class GenerationManager : MonoBehaviour
{
    public Transform worldGrid; // parent of the rooms
    public GameObject roomType; // prefab of the room

    public int mapSize = 16; // amount of rooms

    private int mapSizeSqr; // amount of rooms in one row
    private Vector3 currentPos; // position of currently generating room

    private float roomSize = 7; // size of every room (room size x = 7 AND room size z = 7)

    private float currentPosX, currentPosZ; // position of the currently generating room
    private int currentPosTracker; // current room in the row (4 = 4th room in that row)

        // called when gameObject gets activated (first frame)
    private void Awake()
    {
        GenerateWorld(); // generate the world on start
    }

        // runs every frame
    private void Update()
    {
        mapSizeSqr = (int)Mathf.Sqrt(mapSize); // get the square root of the mapSize (amount of rooms in one row)
    }

        // generating all the rooms
    public void GenerateWorld()
    {
        for(int i = 0; i < mapSize; i++) // generate each room
        {
            if(currentPosTracker == mapSizeSqr) // go to new room if row is 'full'
            {
                currentPosX = 0; // resets the current x position
                currentPosZ += roomSize; // moves z position to new row

                currentPosTracker = 0; // reset currentPosTracker for new row
            }

            currentPos = new Vector3(currentPosX, 0, currentPosZ); // set the position for currently generating room
            Instantiate(roomType, currentPos, Quaternion.identity, worldGrid); // create the room

            currentPosTracker++; // set the tracker to the next room
            currentPosX += roomSize; // move to current x position to next to the last room
        }
    }

        // reload the scene to generate new
    public void ReloadWorld()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // reloading the scene
    }
}
