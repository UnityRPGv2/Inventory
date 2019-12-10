using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryExample.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target = null;

        void LateUpdate()
        {
            transform.position = target.position;
        }
    }
}