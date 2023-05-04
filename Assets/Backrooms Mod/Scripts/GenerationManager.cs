using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ModWobblyLife;
using ModWobblyLife.Network;

public enum GenerationState
{
    Idle,
    Rooms,
    Lights
}

public class GenerationManager : MonoBehaviour
{
    public int seed;

    [Header("Config")]
    public int mapSize = 16; // amount of rooms
    public float roomSize = 7; // size of every room (room size x = 7 AND room size z = 7)
    public Transform worldGrid; // parent of the rooms
    public int mapEmptyness; // chance of empty room spawning
    public int mapBrightness; // chance of light spawning
    public bool enableRareRooms; // whether to allow for rare rooms to spawn
    public int rareness = 25; // how rare they are (higher = rarer)
    public int rareAmount = 10; // how many to spawn in one area
    public GenerationState currentState; // current state of generation

    [Header("Rooms")]
    public List<GameObject> roomTypes; // prefab of the rooms
    public GameObject emptyRoom; // empty room (generated more often)
    public List<GameObject> lightTypes; // prefab of the lights
    public List<GameObject> rareRooms; // rare variant of rooms


    private int mapSizeSqr; // amount of rooms in one row
    private Vector3 currentPos; // position of currently generating room

    private float currentPosX, currentPosZ; // position of the currently generating room
    private int currentPosTracker; // current room in the row (4 = 4th room in that row)

    // called when gameObject gets activated (first frame)
    private void Awake()
    {
        mapSizeSqr = (int)Mathf.Sqrt(mapSize); // get the square root of the mapSize (amount of rooms in one row)
    }

    // generating all the rooms
    public void GenerateWorld()
    {
        Random.InitState(42);
        for (int i = 0; i < mapEmptyness; i++)
            roomTypes.Add(emptyRoom); // adds empty rooms to the roomtypes array

        int _rareAmount = rareAmount;
        for (int i = 0; i < mapSize; i++) // generate each room
        {
            if (currentPosTracker == mapSizeSqr) // go to new room if row is 'full'
            {
                currentPosX = 0; // resets the current x position
                currentPosZ += roomSize; // moves z position to new row

                currentPosTracker = 0; // reset currentPosTracker for new row
            }
            currentPos = new Vector3(currentPosX, 0, currentPosZ); // set the position for currently generating room

            // whether to spawn the rare rooms or the normal rooms
            if (enableRareRooms)
            {
                if (Random.Range(0, rareness) == rareness-1 || _rareAmount != rareAmount)
                {
                    int rot = Random.Range(0, 4) * 90;
                    Instantiate(rareRooms[Random.Range(0, rareRooms.Count)], currentPos, new Quaternion(0, rot, 0, 0),
                        worldGrid); // generating the rare room for rareAmount times

                    _rareAmount--; // counting down to indicate that one has been generated

                    if (_rareAmount == 0)
                        _rareAmount = rareAmount; // reset rareAmount once all of them have been generated
                }
                else
                {
                    int rot = Random.Range(0, 4) * 90;
                    Instantiate(roomTypes[Random.Range(0, roomTypes.Count)], currentPos, new Quaternion(0, rot, 0, 0),
                        worldGrid); // create the room
                }
            }
            else
            {
                int rot = Random.Range(0, 4) * 90;
                Instantiate(roomTypes[Random.Range(0, roomTypes.Count)], currentPos, new Quaternion(0, rot, 0, 0),
                    worldGrid); // create the room
                Debug.Log(rot);
            }

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
