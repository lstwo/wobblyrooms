using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Wobblyrooms.Level3
{
    public class NavMeshManager : MonoBehaviour
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