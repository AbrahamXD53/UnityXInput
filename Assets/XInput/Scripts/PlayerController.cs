using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XInput
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        protected PlayerInput playerInput;
        protected ControllerConfig controller;

        protected new Renderer renderer;
        protected new Rigidbody rigidbody;

        public int changeColorInput = 0;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            renderer = GetComponent<Renderer>();
        }

        public void Init(PlayerInput playerInput)
        {
            this.playerInput = playerInput;
            controller = PartyManager.Instance.GetConfig();
        }

        void LateUpdate()
        {
            if (controller.ButtonDown(changeColorInput, playerInput.inputDevice, (int)playerInput.controlIndex))
            {
                renderer.material.color = Random.ColorHSV();
            }

            rigidbody.velocity = new Vector3(controller.GetAxis(0, playerInput.inputDevice, (int)playerInput.controlIndex), 0,
                controller.GetAxis(1, playerInput.inputDevice, (int)playerInput.controlIndex));
        }
    }
}