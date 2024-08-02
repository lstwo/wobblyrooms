using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace OldWobblyrooms.Level3
{
    public class NavMeshBuilder : MonoBehaviour
    {
        public NavMeshSurface surface;

        // Start is called before the first frame update
        void Start()
        {
            surface.BuildNavMesh();
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}