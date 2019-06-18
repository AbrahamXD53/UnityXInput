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

        [Header("Controller Schemas")]
        public ControllerSchema[] schemas;
        public int loadedSchema = 0;

        protected ControllerSchema currentSchema;

        protected CachedGamepadState[] gamepadsState;
        protected GenericGamepad[] genericGamepads;


        protected KeyCode[] keyboardSupported = new KeyCode[]
        {
            KeyCode.Backspace,
            KeyCode.Delete,
            KeyCode.Tab,
            KeyCode.Clear,
            KeyCode.Return,
            KeyCode.Pause,
            KeyCode.Escape,
            KeyCode.Space,
            KeyCode.Keypad0,
            KeyCode.Keypad1,
            KeyCode.Keypad2,
            KeyCode.Keypad3,
            KeyCode.Keypad4,
            KeyCode.Keypad5,
            KeyCode.Keypad6,
            KeyCode.Keypad7,
            KeyCode.Keypad8,
            KeyCode.Keypad9,
            KeyCode.KeypadPeriod,
            KeyCode.KeypadDivide,
            KeyCode.KeypadMultiply,
            KeyCode.KeypadMinus,
            KeyCode.KeypadPlus,
            KeyCode.KeypadEnter,
            KeyCode.KeypadEquals,
            KeyCode.UpArrow,
            KeyCode.DownArrow,
            KeyCode.RightArrow,
            KeyCode.LeftArrow,
            KeyCode.Insert,
            KeyCode.Home,
            KeyCode.End,
            KeyCode.PageUp,
            KeyCode.PageDown,
            KeyCode.F1,
            KeyCode.F2,
            KeyCode.F3,
            KeyCode.F4,
            KeyCode.F5,
            KeyCode.F6,
            KeyCode.F7,
            KeyCode.F8,
            KeyCode.F9,
            KeyCode.F10,
            KeyCode.F11,
            KeyCode.F12,
            KeyCode.F13,
            KeyCode.F14,
            KeyCode.F15,
            KeyCode.Alpha0,
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
            KeyCode.Alpha9,
            KeyCode.Exclaim,
            KeyCode.DoubleQuote,
            KeyCode.Hash,
            KeyCode.Dollar,
            KeyCode.Percent,
            KeyCode.Ampersand,
            KeyCode.Quote,
            KeyCode.LeftParen,
            KeyCode.RightParen,
            KeyCode.Asterisk,
            KeyCode.Plus,
            KeyCode.Comma,
            KeyCode.Minus,
            KeyCode.Period,
            KeyCode.Slash,
            KeyCode.Colon,
            KeyCode.Semicolon,
            KeyCode.Less,
            KeyCode.Equals,
            KeyCode.Greater,
            KeyCode.Question,
            KeyCode.At,
            KeyCode.LeftBracket,
            KeyCode.Backslash,
            KeyCode.RightBracket,
            KeyCode.Caret,
            KeyCode.Underscore,
            KeyCode.BackQuote,
            KeyCode.A,
            KeyCode.B,
            KeyCode.C,
            KeyCode.D,
            KeyCode.E,
            KeyCode.F,
            KeyCode.G,
            KeyCode.H,
            KeyCode.I,
            KeyCode.J,
            KeyCode.K,
            KeyCode.L,
            KeyCode.M,
            KeyCode.N,
            KeyCode.O,
            KeyCode.P,
            KeyCode.Q,
            KeyCode.R,
            KeyCode.S,
            KeyCode.T,
            KeyCode.U,
            KeyCode.V,
            KeyCode.W,
            KeyCode.X,
            KeyCode.Y,
            KeyCode.Z,
            KeyCode.LeftCurlyBracket,
            KeyCode.Pipe,
            KeyCode.RightCurlyBracket,
            KeyCode.Tilde,
            KeyCode.Numlock,
            KeyCode.CapsLock,
            KeyCode.ScrollLock,
            KeyCode.RightShift,
            KeyCode.LeftShift,
            KeyCode.RightControl,
            KeyCode.LeftControl,
            KeyCode.RightAlt,
            KeyCode.LeftAlt,
            KeyCode.LeftCommand,
            KeyCode.LeftApple,
            KeyCode.LeftWindows,
            KeyCode.RightCommand,
            KeyCode.RightApple,
            KeyCode.RightWindows,
            KeyCode.AltGr,
            KeyCode.Help,
            KeyCode.Print,
            KeyCode.SysReq,
            KeyCode.Break,
            KeyCode.Menu,
            KeyCode.Mouse0,
            KeyCode.Mouse1,
            KeyCode.Mouse2,
            KeyCode.Mouse3,
            KeyCode.Mouse4,
            KeyCode.Mouse5,
            KeyCode.Mouse6,
        };

        public void Init()
        {
            currentSchema = schemas[loadedSchema];
#if USE_GENERICINPUT
            genericGamepads = new GenericGamepad[maxControllers];
            for (int i = 0; i < maxControllers; i++)
            {
                genericGamepads[i] = GenericGamePadInput.GetGamepad(i);
            }
#else
            gamepadsState = new CachedGamepadState[maxControllers];
            for (int i = 0; i < maxControllers; i++)
            {
                gamepadsState[i] = new CachedGamepadState((PlayerIndex)i);
            }
#endif
        }

        public void LoadSchema(int id)
        {
            loadedSchema = id;
            currentSchema = schemas[loadedSchema];
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


#if USE_GENERICINPUT

        public GenericGamepad GetGamepad(int index)
        {
            return genericGamepads[index];
        }
#else
        public CachedGamepadState GetGamepad(int index)
        {
            return gamepadsState[index];
        }

#endif

        public KeyCode GetKeyPressed()
        {
            for (int i = 0; i < keyboardSupported.Length; i++)
            {
                if (Input.GetKeyDown(keyboardSupported[i]))
                {
                    return keyboardSupported[i];
                }
            }
            return KeyCode.None;
        }

        public ControllerSchema GetSchema(int index = -1)
        {
            if (index < 0)
                return currentSchema;
            else
                return schemas[index];
        }

        public void Vibrate(PlayerIndex playerIndex, float intensity = 1.0f, float duration = 0.1f)
        {
            if (allowVibration)
                ControllerSupport.Instance.Vibrate(playerIndex, intensity, duration);
        }

        public bool IsConnected(int index)
        {
#if USE_GENERICINPUT
            return true;
#else
            return gamepadsState[index].IsConnected;
#endif
        }

        public float GetAxis(int actionIndex, InputDevice inputDevice, int playerIndex)
        {
            float res = 0;
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.KeyboardMouse))
                for (int i = 0; i < currentSchema.actionAxis[actionIndex].axisNames.Length; i++)
                    res += Input.GetAxis(currentSchema.actionAxis[actionIndex].axisNames[i]);
#if USE_GENERICINPUT
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.GenericGamepad))
                for (int i = 0; i < currentSchema.actionAxis[actionIndex].axes.Length; i++)
                    res += genericGamepads[playerIndex].GetAxis(currentSchema.actionAxis[actionIndex].axes[i]);
#else
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.Gamepad))
                for (int i = 0; i < currentSchema.actionAxis[actionIndex].axes.Length; i++)
                    res += gamepadsState[playerIndex].GetAxis(currentSchema.actionAxis[actionIndex].axes[i]);
#endif
#if MOBILE_INPUT
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.Touch))
                for (int i = 0; i < currentSchema.actionAxis[actionIndex].touchNames.Length; i++)
                    res += CrossPlatformInputManager.GetAxis(currentSchema.actionAxis[actionIndex].touchNames[i]);
#endif
            return res;
        }

        public bool ButtonDown(int actionIndex, InputDevice inputDevice, int playerIndex)
        {
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.KeyboardMouse))
                for (int i = 0; i < currentSchema.actionButtons[actionIndex].buttonKeyboard.Count; i++)
                    if (Input.GetKeyDown(currentSchema.actionButtons[actionIndex].buttonKeyboard[i]))
                        return true;
#if USE_GENERICINPUT
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.GenericGamepad))
                for (int i = 0; i < currentSchema.actionButtons[actionIndex].buttonGamepad.Count; i++)
                    if (genericGamepads[playerIndex].ButtonDown(currentSchema.actionButtons[actionIndex].buttonGamepad[i]))
                        return true;
#else
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.Gamepad))
                for (int i = 0; i < currentSchema.actionButtons[actionIndex].buttonGamepad.Count; i++)
                    if (gamepadsState[playerIndex].ButtonDown(currentSchema.actionButtons[actionIndex].buttonGamepad[i]))
                        return true;
#endif
#if MOBILE_INPUT
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.Touch))
                for (int i = 0; i < currentSchema.actionButtons[actionIndex].buttonNames.Count; i++)
                    if (CrossPlatformInputManager.GetButtonDown(currentSchema.actionButtons[actionIndex].buttonNames[i]))
                        return true;
#endif
            return false;

        }

        public bool Button(int actionIndex, InputDevice inputDevice, int playerIndex)
        {
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.KeyboardMouse))
                for (int i = 0; i < currentSchema.actionButtons[actionIndex].buttonKeyboard.Count; i++)
                    if (Input.GetKey(currentSchema.actionButtons[actionIndex].buttonKeyboard[i]))
                        return true;
#if USE_GENERICINPUT
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.GenericGamepad))
                for (int i = 0; i < currentSchema.actionButtons[actionIndex].buttonGamepad.Count; i++)
                    if (genericGamepads[playerIndex].Button(currentSchema.actionButtons[actionIndex].buttonGamepad[i]))
                        return true;
#else
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.Gamepad))
                for (int i = 0; i < currentSchema.actionButtons[actionIndex].buttonGamepad.Count; i++)
                    if (gamepadsState[playerIndex].Button(currentSchema.actionButtons[actionIndex].buttonGamepad[i]))
                        return true;
#endif
#if MOBILE_INPUT
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.Touch))
                for (int i = 0; i < currentSchema.actionButtons[actionIndex].buttonNames.Count; i++)
                    if (CrossPlatformInputManager.GetButtonDown(currentSchema.actionButtons[actionIndex].buttonNames[i]))
                        return true;
#endif
            return false;

        }

        public bool ButtonUp(int actionIndex, InputDevice inputDevice, int playerIndex)
        {
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.KeyboardMouse))
                for (int i = 0; i < currentSchema.actionButtons[actionIndex].buttonKeyboard.Count; i++)
                    if (Input.GetKeyUp(currentSchema.actionButtons[actionIndex].buttonKeyboard[i]))
                        return true;
#if USE_GENERICINPUT
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.GenericGamepad))
                for (int i = 0; i < currentSchema.actionButtons[actionIndex].buttonGamepad.Count; i++)
                    if (genericGamepads[playerIndex].ButtonUp(currentSchema.actionButtons[actionIndex].buttonGamepad[i]))
                        return true;
#else
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.Gamepad))
                for (int i = 0; i < currentSchema.actionButtons[actionIndex].buttonGamepad.Count; i++)
                    if (gamepadsState[playerIndex].ButtonUp(currentSchema.actionButtons[actionIndex].buttonGamepad[i]))
                        return true;
#endif
#if MOBILE_INPUT
            if (inputDevice == InputDevice.Any || inputDevice.HasFlag(InputDevice.Touch))
                for (int i = 0; i < currentSchema.actionButtons[actionIndex].buttonNames.Count; i++)
                    if (CrossPlatformInputManager.GetButtonDown(currentSchema.actionButtons[actionIndex].buttonNames[i]))
                        return true;
#endif
            return false;

        }

        public void Remap(int action, string button, int index)
        {
            if (index >= currentSchema.actionButtons[action].buttonNames.Count)
                currentSchema.actionButtons[action].buttonNames.Add(button);
            else
                currentSchema.actionButtons[action].buttonNames[index] = button;
        }

        public void Remap(int action, KeyCode key, int index)
        {
            if (index < 0)
            {
                currentSchema.actionButtons[action].buttonKeyboard.RemoveAt(currentSchema.actionButtons[action].buttonKeyboard.Count - 1);
            }
            else
            {
                if (index >= currentSchema.actionButtons[action].buttonKeyboard.Count)
                    currentSchema.actionButtons[action].buttonKeyboard.Add(key);
                else
                    currentSchema.actionButtons[action].buttonKeyboard[index] = key;
            }
        }

        public void Remap(int action, GamepadButton button, int index)
        {
            if (index < 0)
            {
                currentSchema.actionButtons[action].buttonGamepad.RemoveAt(currentSchema.actionButtons[action].buttonGamepad.Count - 1);
            }
            else
            {
                if (index >= currentSchema.actionButtons[action].buttonGamepad.Count)
                    currentSchema.actionButtons[action].buttonGamepad.Add(button);
                else
                    currentSchema.actionButtons[action].buttonGamepad[index] = button;
            }
        }
    }
}