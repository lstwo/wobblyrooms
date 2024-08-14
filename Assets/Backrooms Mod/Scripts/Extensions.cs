using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Wobblyrooms
{
    public static class Extensions
    {
        public static T GetComponentElseChildren<T>(this GameObject instance, bool includeInactive = false)
        {
            if (instance.TryGetComponent<T>(out var component))
            {
                return component;
            }
            else
            {
                return instance.GetComponentInChildren<T>(includeInactive);
            }
        }

        public static T GetComponentElseChildren<T>(this Component instance, bool includeInactive = false)
        {
            if (instance.TryGetComponent<T>(out var component))
            {
                return component;
            }
            else
            {
                return instance.GetComponentInChildren<T>(includeInactive);
            }
        }
    }
}
