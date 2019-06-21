using UnityEngine;
using System.Collections;
using System;
using System.Linq;

namespace XInput
{
    [System.Serializable]
    public class GenericGamepad : IGamepad
    {
        public GenericGamePadInput.Index index;

        public KeyCode[] buttonKeyCodes;
        public string[] axis;

        public GenericGamepad(int index, KeyCode[] buttons, string[] axisNames)
        {
            this.index = (GenericGamePadInput.Index)index;
            buttonKeyCodes = buttons;
            axis = axisNames;
        }

        public bool ButtonDown(GamepadButton padButton)
        {
            return Input.GetKeyDown(buttonKeyCodes[(int)padButton]);
        }

        public bool Button(GamepadButton padButton)
        {
            return Input.GetKey(buttonKeyCodes[(int)padButton]);
        }

        public bool ButtonUp(GamepadButton padButton)
        {
            return Input.GetKeyUp(buttonKeyCodes[(int)padButton]);
        }

        public float GetAxis(GamepadAxis gamepadAxis)
        {
            return Input.GetAxis(axis[(int)gamepadAxis]);
        }

        public GamepadButton GetButtonPressed()
        {
            foreach (var item in Enum.GetValues(typeof(GamepadButton)).Cast<GamepadButton>())
            {
                if (ButtonDown(item))
                    return item;
            }
            return GamepadButton.None;
        }
    }

    public static class GenericGamePadInput
    {
        public enum Index { One, Two, Three, Four, Any }

        private static readonly string[][] TriggerNames = new string[][]
        {
            new string[]
            {
                    "TriggersL_1",
                    "TriggersL_2",
                    "TriggersL_3",
                    "TriggersL_4",
                    "TriggersL_0"
            },
            new string[]
            {
                    "TriggersR_1",
                    "TriggersR_2",
                    "TriggersR_3",
                    "TriggersR_4",
                    "TriggersR_0"
            }
        };

        private static readonly string[][][] AxisNames = new string[][][]
        {
            new string[][] {
                new string[] {
                    "L_XAxis_1",
                    "L_XAxis_2",
                    "L_XAxis_3",
                    "L_XAxis_4",
                    "L_XAxis_0"
                },
                new string[] {
                    "L_YAxis_1",
                    "L_YAxis_2",
                    "L_YAxis_3",
                    "L_YAxis_4",
                    "L_YAxis_0"
                }
            },
            new string[][] {
                new string[] {
                    "R_XAxis_1",
                    "R_XAxis_2",
                    "R_XAxis_3",
                    "R_XAxis_4",
                    "R_XAxis_0"
                },
                new string[] {
                    "R_YAxis_1",
                    "R_YAxis_2",
                    "R_YAxis_3",
                    "R_YAxis_4",
                    "R_YAxis_0"
                }
            },
            new string[][] {
                new string[] {
                   "DPad_XAxis_1",
                   "DPad_XAxis_2",
                   "DPad_XAxis_3",
                   "DPad_XAxis_4",
                   "DPad_XAxis_0"
                },
                new string[] {
                    "DPad_YAxis_1",
                    "DPad_YAxis_2",
                    "DPad_YAxis_3",
                    "DPad_YAxis_4",
                    "DPad_YAxis_0"
                }
            }
        };

        public static GenericGamepad GetGamepad(int controller)
        {
            return new GenericGamepad(controller, GetControllerButtons(controller), GetControllerAxis(controller));
        }

        private static string[] GetControllerAxis(int controller)
        {
            return new string[] {
                AxisNames[0][0][controller],
                AxisNames[0][1][controller],
                AxisNames[1][0][controller],
                AxisNames[1][1][controller],
                TriggerNames[0][controller],
                TriggerNames[1][controller],
                AxisNames[2][0][controller],
                AxisNames[2][1][controller]
            };
        }

        private static KeyCode[] GetControllerButtons(int controller)
        {
            switch (controller)
            {
                case 0:
                    return new KeyCode[] {
                            KeyCode.Joystick1Button0,
                            KeyCode.Joystick1Button1,
                            KeyCode.Joystick1Button2,
                            KeyCode.Joystick1Button3,
                            KeyCode.Joystick1Button6,
                            KeyCode.Joystick1Button7,
                            KeyCode.Joystick1Button8,
                            KeyCode.Joystick1Button9,
                            KeyCode.Joystick1Button4,
                            KeyCode.Joystick1Button5, //GamepadButton.RightShoulder
                            KeyCode.Joystick1Button10,
                            KeyCode.Joystick1Button11,
                            KeyCode.Joystick1Button12,
                            KeyCode.Joystick1Button13,
                            KeyCode.Joystick1Button14,
                            KeyCode.Joystick1Button15
                        };
                case 1:
                    return new KeyCode[] {
                            KeyCode.Joystick2Button0,
                            KeyCode.Joystick2Button1,
                            KeyCode.Joystick2Button2,
                            KeyCode.Joystick2Button3,
                            KeyCode.Joystick2Button6,
                            KeyCode.Joystick2Button7,
                            KeyCode.Joystick2Button8,
                            KeyCode.Joystick2Button9,
                            KeyCode.Joystick2Button4,
                            KeyCode.Joystick2Button5, //GamepadButton.RightShoulder
                            KeyCode.Joystick2Button10,
                            KeyCode.Joystick2Button11,
                            KeyCode.Joystick2Button12,
                            KeyCode.Joystick2Button13,
                            KeyCode.Joystick2Button14,
                            KeyCode.Joystick2Button15
                        };
                case 2:
                    return new KeyCode[] {
                            KeyCode.Joystick3Button0,
                            KeyCode.Joystick3Button1,
                            KeyCode.Joystick3Button2,
                            KeyCode.Joystick3Button3,
                            KeyCode.Joystick3Button6,
                            KeyCode.Joystick3Button7,
                            KeyCode.Joystick3Button8,
                            KeyCode.Joystick3Button9,
                            KeyCode.Joystick3Button4,
                            KeyCode.Joystick3Button5, //GamepadButton.RightShoulder
                            KeyCode.Joystick3Button10,
                            KeyCode.Joystick3Button11,
                            KeyCode.Joystick3Button12,
                            KeyCode.Joystick3Button13,
                            KeyCode.Joystick3Button14,
                            KeyCode.Joystick3Button15
                        };
                case 3:
                    return new KeyCode[] {
                            KeyCode.Joystick4Button0,
                            KeyCode.Joystick4Button1,
                            KeyCode.Joystick4Button2,
                            KeyCode.Joystick4Button3,
                            KeyCode.Joystick4Button6,
                            KeyCode.Joystick4Button7,
                            KeyCode.Joystick4Button8,
                            KeyCode.Joystick4Button9,
                            KeyCode.Joystick4Button4,
                            KeyCode.Joystick4Button5, //GamepadButton.RightShoulder
                            KeyCode.Joystick4Button10,
                            KeyCode.Joystick4Button11,
                            KeyCode.Joystick4Button12,
                            KeyCode.Joystick4Button13,
                            KeyCode.Joystick4Button14,
                            KeyCode.Joystick4Button15
                        };
                default:
                    return new KeyCode[] {
                            KeyCode.JoystickButton0,
                            KeyCode.JoystickButton1,
                            KeyCode.JoystickButton2,
                            KeyCode.JoystickButton3,
                            KeyCode.JoystickButton6,
                            KeyCode.JoystickButton7,
                            KeyCode.JoystickButton8,
                            KeyCode.JoystickButton9,
                            KeyCode.JoystickButton4,
                            KeyCode.JoystickButton5, //GamepadButton.RightShoulder
                            KeyCode.JoystickButton10,
                            KeyCode.JoystickButton11,
                            KeyCode.JoystickButton12,
                            KeyCode.JoystickButton13,
                            KeyCode.JoystickButton14,
                            KeyCode.JoystickButton15
                        };
            }
        }
    }
}