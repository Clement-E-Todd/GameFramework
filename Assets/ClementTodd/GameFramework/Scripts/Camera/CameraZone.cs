﻿using Cinemachine;
using UnityEngine;

namespace ClementTodd.CameraControl
{
    public class CameraZone : MonoBehaviour
    {
        public CinemachineVirtualCameraBase virtualCamera;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                virtualCamera.gameObject.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                virtualCamera.gameObject.SetActive(false);
            }
        }
    }
}