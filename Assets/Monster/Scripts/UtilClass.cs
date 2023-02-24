using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Litkey.Utility
{
    public static class UtilClass
    {
        /// <summary>
        /// Gets the float Angle with the direction to the target
        /// </summary>
        /// <param name="direction">direction to the target</param>
        public static float GetAngleFromDirection(Vector3 direction)
        {
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Gets the Rotation Angle with the direction to the target
        /// </summary>
        /// <param name="direction">direction to the target</param>
        public static Quaternion GetRotationFromDirection(Vector3 direction)
        {
            return Quaternion.AngleAxis(GetAngleFromDirection(direction), Vector3.forward);
        }

    }
}
