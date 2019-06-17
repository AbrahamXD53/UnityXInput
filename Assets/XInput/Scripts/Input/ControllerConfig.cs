using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using XInputDotNetPure;

namespace XInput
{
    [CreateAssetMenu(fileName = "ControllerInfo", menuName = "Customs/Controllers", order = 1)]
    public class ControllerConfig : ScriptableObject
    {
        public bool allowVibration = true;
        [Tooltip("This is the maximun amount of gamepads to read, this does not mean maximun amount of players")]
        public int maxControllers = 4;

        [Header("Schema")]
        public ActionButton[] actionButtons;
        public ActionAxis[] actionAxis;
        
        protected PlayerIndex[] controllers = new PlayerIndex[] { PlayerIndex.One, PlayerIndex.Two, PlayerIndex.Three, PlayerIndex.Four };
        protected CachedGamepadState[] gamepadsState;
        protected GenericGamepad[] genericGamepads;

        public void Init()
        {
#if USE_GENERICINPUT
            genericGamepads = new GenericGamepad[maxControllers];
            foreach (var item in controllers)
            {
                genericGamepads[(int)item] = GenericGamePadInput.GetGamepad((int)item);
            }
#else
            gamepadsState = new CachedGamepadState[maxControllers];
            foreach (var item in controllers)
            {
                gamepadsState[(int)item] = new CachedGamepadState(item);
            }
#endif
        }

        public void Update()
        {
#if !USE_GENERICINPUT
            for (int i = 0; i < maxControllers; i++)
            {
                gamepadsState[i].Update();
            }
#endif
        }

        public CachedGamepadState GetGamepad(int index)
        {
            return gamepadsState[index];
        }

        public void Vibrate(PlayerIndex playerIndex, float intensity = 1.0f, float duration = 0.1f)
        {
            if (allowVibration)
                PartyManager.Instance.Vibrate(playerIndex, intensity, duration);
        }

        public bool IsConnected(int index)
        {
            return gamepadsState[index].IsConnected;
        }

        public float GetAxis(int actionIndex, InputDevice inputDevice, int playerIndex)
        {
            float res = 0;
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.KeyboardMouse))
                for (int i = 0; i < actionAxis[actionIndex].axisNames.Length; i++)
                    res += Input.GetAxis(actionAxis[actionIndex].axisNames[i]);
#if USE_GENERICINPUT
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.GenericGamepad))
                for (int i = 0; i < actionAxis[actionIndex].axes.Length; i++)
                    res += genericGamepads[playerIndex].GetAxis(actionAxis[actionIndex].axes[i]);
#else
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.Gamepad))
                for (int i = 0; i < actionAxis[actionIndex].axes.Length; i++)
                    res += gamepadsState[playerIndex].GetAxis(actionAxis[actionIndex].axes[i]);
#endif
#if MOBILE_INPUT
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.Touch))
                for (int i = 0; i < actionAxis[actionIndex].touchNames.Length; i++)
                    res += CrossPlatformInputManager.GetAxis(actionAxis[actionIndex].touchNames[i]);
#endif
            return res;
        }

        public bool ButtonDown(int actionIndex, InputDevice inputDevice, int playerIndex)
        {
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.KeyboardMouse))
                for (int i = 0; i < actionButtons[actionIndex].buttonKeyboard.Length; i++)
                    if (Input.GetKeyDown(actionButtons[actionIndex].buttonKeyboard[i]))
                        return true;
#if USE_GENERICINPUT
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.GenericGamepad))
                for (int i = 0; i < actionButtons[actionIndex].buttonGamepad.Length; i++)
                    if (genericGamepads[playerIndex].ButtonDown(actionButtons[actionIndex].buttonGamepad[i]))
                        return true;
#else
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.Gamepad))
                for (int i = 0; i < actionButtons[actionIndex].buttonGamepad.Length; i++)
                    if (gamepadsState[playerIndex].ButtonDown(actionButtons[actionIndex].buttonGamepad[i]))
                        return true;
#endif
#if MOBILE_INPUT
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.Touch))
                for (int i = 0; i < actionButtons[actionIndex].buttonNames.Length; i++)
                    if (CrossPlatformInputManager.GetButtonDown(actionButtons[actionIndex].buttonNames[i]))
                        return true;
#endif
            return false;

        }

        public bool Button(int actionIndex, InputDevice inputDevice, int playerIndex)
        {
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.KeyboardMouse))
                for (int i = 0; i < actionButtons[actionIndex].buttonKeyboard.Length; i++)
                    if (Input.GetKey(actionButtons[actionIndex].buttonKeyboard[i]))
                        return true;
#if USE_GENERICINPUT
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.GenericGamepad))
                for (int i = 0; i < actionButtons[actionIndex].buttonGamepad.Length; i++)
                    if (genericGamepads[playerIndex].Button(actionButtons[actionIndex].buttonGamepad[i]))
                        return true;
#else
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.Gamepad))
                for (int i = 0; i < actionButtons[actionIndex].buttonGamepad.Length; i++)
                    if (gamepadsState[playerIndex].Button(actionButtons[actionIndex].buttonGamepad[i]))
                        return true;
#endif
#if MOBILE_INPUT
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.Touch))
                for (int i = 0; i < actionButtons[actionIndex].buttonNames.Length; i++)
                    if (CrossPlatformInputManager.GetButtonDown(actionButtons[actionIndex].buttonNames[i]))
                        return true;
#endif
            return false;

        }

        public bool ButtonUp(int actionIndex, InputDevice inputDevice, int playerIndex)
        {
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.KeyboardMouse))
                for (int i = 0; i < actionButtons[actionIndex].buttonKeyboard.Length; i++)
                    if (Input.GetKeyUp(actionButtons[actionIndex].buttonKeyboard[i]))
                        return true;
#if USE_GENERICINPUT
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.GenericGamepad))
                for (int i = 0; i < actionButtons[actionIndex].buttonGamepad.Length; i++)
                    if (genericGamepads[playerIndex].ButtonUp(actionButtons[actionIndex].buttonGamepad[i]))
                        return true;
#else
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.Gamepad))
                for (int i = 0; i < actionButtons[actionIndex].buttonGamepad.Length; i++)
                    if (gamepadsState[playerIndex].ButtonUp(actionButtons[actionIndex].buttonGamepad[i]))
                        return true;
#endif
#if MOBILE_INPUT
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.Touch))
                for (int i = 0; i < actionButtons[actionIndex].buttonNames.Length; i++)
                    if (CrossPlatformInputManager.GetButtonDown(actionButtons[actionIndex].buttonNames[i]))
                        return true;
#endif
            return false;

        }
    }
}