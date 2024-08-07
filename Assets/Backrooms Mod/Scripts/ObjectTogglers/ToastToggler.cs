using UnityEngine;
using Wobblyrooms.MainMenu;

namespace Wobblyrooms
{
    public class ToastToggler : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(Settings.toasts);
        }
    }
}
