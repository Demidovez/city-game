using System;
using UnityEngine;

namespace GameControllerSpace
{
    public class GameController : MonoBehaviour
    {
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}

