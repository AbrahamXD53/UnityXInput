using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XInput
{
    public enum GamepadButton
    {
        A, B, X, Y,
        Back,
        Start,
        LeftStick,
        RightStick,
        LeftShoulder,
        RightShoulder,
        Up, Down, Left, Right,
        Guide,
        None
    };

    public enum GamepadAxis {
        LeftStickX,
        LeftStickY,
        RightStickX,
        RightStickY,
        LeftTrigger,
        RightTrigger,
        DpadX,
        DpadY
    }

    public enum GamepadTriggers { Left, Right }
    public enum GamepadThumbSticks { Left, Right }


    public interface IGamepad
    {
        bool ButtonDown(GamepadButton padButton);

        bool Button(GamepadButton padButton);

        bool ButtonUp(GamepadButton padButton);

        float GetAxis(GamepadAxis gamepadAxis);
    }

}