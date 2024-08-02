using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Wobblyrooms
{
    public class NavMeshBuilder : MonoBehaviour
    {
        public NavMeshSurface surface;

        void Start()
        {
            surface.BuildNavMesh();
        }
    }
}