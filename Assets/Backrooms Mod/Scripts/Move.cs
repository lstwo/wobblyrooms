using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wobblyrooms
{
    public class Move : MonoBehaviour
    {
        [SerializeField] public Vector3 movementPerSecond;

        void Update()
        {
            transform.position = transform.position + movementPerSecond * Time.deltaTime;
        }
    }
}
