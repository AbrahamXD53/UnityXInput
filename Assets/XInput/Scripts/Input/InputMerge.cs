using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XInput
{
    public enum InputDevice
    {
        Any = 0,
        KeyboardMouse = 1,
        Gamepad = 2,
        GenericGamepad = 4,
        Touch = 8,
    }

    [System.Serializable]
    public class ActionAxis
    {
        public string name;
        [Header("Gamepad")]
        public GamepadAxis[] axes;
        [Header("Unity Input Manager")]
        public string[] axisNames;
        [Header("Crossplatform Input Manager")]
        public string[] touchNames;
    }

    [System.Serializable]
    public class ActionButton
    {
        public string name;
        [Header("Gamepad")]
        public GamepadButton[] buttonGamepad;
        [Header("Unity Input Manager")]
        public KeyCode[] buttonKeyboard;
        [Header("Crossplatform Input Manager")]
        public string[] buttonNames;
    }
}
