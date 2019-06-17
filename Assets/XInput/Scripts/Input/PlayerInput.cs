﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XInputDotNetPure;

namespace XInput
{
    [Serializable]
    public class PlayerInput
    {
        public InputDevice inputDevice;
        public PlayerIndex controlIndex;
        public int realIndex = 0;
        public bool ready = false;

        public int selectedCharacter;


        public bool IsInput(InputDevice inputDevice)
        {
            return this.inputDevice == inputDevice;
        }

        public bool IsInput(InputDevice inputDevice, PlayerIndex playerIndex)
        {
            return this.inputDevice == inputDevice && this.controlIndex == playerIndex;
        }

        internal bool IsPlayer(InputDevice inputDevice, PlayerIndex playerIndex)
        {
            return this.inputDevice == InputDevice.Gamepad || this.inputDevice == InputDevice.GenericGamepad ? IsInput(inputDevice, playerIndex) : IsInput(inputDevice);
        }
    }
}