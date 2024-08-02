using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Wobblyrooms.MainMenu
{
    public class AssignLists : MonoBehaviour
    {
        public Dictionary<int, Sprite> spriteList;
        public List<int> keys;
        public List<Sprite> sprites;

        public void Awake()
        {
            spriteList = new Dictionary<int, Sprite>();
            for (int i = 0; i < keys.Count; i++)
            {
                spriteList.Add(keys[i], sprites[i]);
            }
        }
    }
}