using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XInputDotNetPure;

namespace XInput
{
    public class CachedGamepadState : IGamepad
    {
        protected PlayerIndex playerIndex;
        protected GamePadState prevState;
        protected GamePadState actualState;

        public uint PacketNumber { get { return actualState.PacketNumber; } }
        public bool IsConnected { get { return actualState.IsConnected; } }
        public GamePadButtons Buttons { get { return actualState.Buttons; } }
        public GamePadDPad DPad { get { return actualState.DPad; } }
        public GamePadTriggers Triggers { get { return actualState.Triggers; } }
        public GamePadThumbSticks ThumbSticks { get { return actualState.ThumbSticks; } }

        public float GetAxis(GamepadAxis gamepadAxis)
        {
            switch (gamepadAxis)
            {
                case GamepadAxis.LeftStickX:
                    return actualState.ThumbSticks.Left.X;
                case GamepadAxis.LeftStickY:
                    return actualState.ThumbSticks.Left.Y;
                case GamepadAxis.RightStickX:
                    return actualState.ThumbSticks.Right.X;
                case GamepadAxis.RightStickY:
                    return actualState.ThumbSticks.Right.Y;
                case GamepadAxis.LeftTrigger:
                    return actualState.Triggers.Left;
                case GamepadAxis.RightTrigger:
                    return actualState.Triggers.Right;
                case GamepadAxis.DpadY:
                    return (actualState.DPad.Up == ButtonState.Pressed ? 1 : 0) + (actualState.DPad.Down == ButtonState.Pressed ? -1 : 0);
                case GamepadAxis.DpadX:
                    return (actualState.DPad.Right == ButtonState.Pressed ? 1 : 0) + (actualState.DPad.Left == ButtonState.Pressed ? -1 : 0);
                default:
                    break;
            }
            return 0;
        }

        public bool ButtonDown(GamepadButton padButton)
        {
            switch (padButton)
            {
                case GamepadButton.A:
                    return prevState.Buttons.A == ButtonState.Released && actualState.Buttons.A == ButtonState.Pressed;
                case GamepadButton.B:
                    return prevState.Buttons.B == ButtonState.Released && actualState.Buttons.B == ButtonState.Pressed;
                case GamepadButton.X:
                    return prevState.Buttons.X == ButtonState.Released && actualState.Buttons.X == ButtonState.Pressed;
                case GamepadButton.Y:
                    return prevState.Buttons.Y == ButtonState.Released && actualState.Buttons.Y == ButtonState.Pressed;
                case GamepadButton.Up:
                    return prevState.DPad.Up == ButtonState.Released && actualState.DPad.Up == ButtonState.Pressed;
                case GamepadButton.Down:
                    return prevState.DPad.Down == ButtonState.Released && actualState.DPad.Down == ButtonState.Pressed;
                case GamepadButton.Left:
                    return prevState.DPad.Left == ButtonState.Released && actualState.DPad.Left == ButtonState.Pressed;
                case GamepadButton.Right:
                    return prevState.DPad.Right == ButtonState.Released && actualState.DPad.Right == ButtonState.Pressed;
                case GamepadButton.Start:
                    return prevState.Buttons.Start == ButtonState.Released && actualState.Buttons.Start == ButtonState.Pressed;
                case GamepadButton.Back:
                    return prevState.Buttons.Back == ButtonState.Released && actualState.Buttons.Back == ButtonState.Pressed;
                case GamepadButton.LeftStick:
                    return prevState.Buttons.LeftStick == ButtonState.Released && actualState.Buttons.LeftStick == ButtonState.Pressed;
                case GamepadButton.RightStick:
                    return prevState.Buttons.RightStick == ButtonState.Released && actualState.Buttons.RightStick == ButtonState.Pressed;
                case GamepadButton.LeftShoulder:
                    return prevState.Buttons.LeftShoulder == ButtonState.Released && actualState.Buttons.LeftShoulder == ButtonState.Pressed;
                case GamepadButton.RightShoulder:
                    return prevState.Buttons.RightShoulder == ButtonState.Released && actualState.Buttons.RightShoulder == ButtonState.Pressed;
                case GamepadButton.Guide:
                    return prevState.Buttons.Guide == ButtonState.Released && actualState.Buttons.Guide == ButtonState.Pressed;
                default:
                    return false;
            }
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

        public bool ButtonUp(GamepadButton padButton)
        {
            switch (padButton)
            {
                case GamepadButton.A:
                    return prevState.Buttons.A == ButtonState.Pressed && actualState.Buttons.A == ButtonState.Released;
                case GamepadButton.B:
                    return prevState.Buttons.B == ButtonState.Pressed && actualState.Buttons.B == ButtonState.Released;
                case GamepadButton.X:
                    return prevState.Buttons.X == ButtonState.Pressed && actualState.Buttons.X == ButtonState.Released;
                case GamepadButton.Y:
                    return prevState.Buttons.Y == ButtonState.Pressed && actualState.Buttons.Y == ButtonState.Released;
                case GamepadButton.Up:
                    return prevState.DPad.Up == ButtonState.Pressed && actualState.DPad.Up == ButtonState.Released;
                case GamepadButton.Down:
                    return prevState.DPad.Down == ButtonState.Pressed && actualState.DPad.Down == ButtonState.Released;
                case GamepadButton.Left:
                    return prevState.DPad.Left == ButtonState.Pressed && actualState.DPad.Left == ButtonState.Released;
                case GamepadButton.Right:
                    return prevState.DPad.Right == ButtonState.Pressed && actualState.DPad.Right == ButtonState.Released;
                case GamepadButton.Start:
                    return prevState.Buttons.Start == ButtonState.Pressed && actualState.Buttons.Start == ButtonState.Released;
                case GamepadButton.Back:
                    return prevState.Buttons.Back == ButtonState.Pressed && actualState.Buttons.Back == ButtonState.Released;
                case GamepadButton.LeftStick:
                    return prevState.Buttons.LeftStick == ButtonState.Pressed && actualState.Buttons.LeftStick == ButtonState.Released;
                case GamepadButton.RightStick:
                    return prevState.Buttons.RightStick == ButtonState.Pressed && actualState.Buttons.RightStick == ButtonState.Released;
                case GamepadButton.LeftShoulder:
                    return prevState.Buttons.LeftShoulder == ButtonState.Pressed && actualState.Buttons.LeftShoulder == ButtonState.Released;
                case GamepadButton.RightShoulder:
                    return prevState.Buttons.RightShoulder == ButtonState.Pressed && actualState.Buttons.RightShoulder == ButtonState.Released;
                case GamepadButton.Guide:
                    return prevState.Buttons.Guide == ButtonState.Pressed && actualState.Buttons.Guide == ButtonState.Released;
                default:
                    return false;
            }
        }

        public bool Button(GamepadButton padButton)
        {
            switch (padButton)
            {
                case GamepadButton.A:
                    return actualState.Buttons.A == ButtonState.Pressed;
                case GamepadButton.B:
                    return actualState.Buttons.B == ButtonState.Pressed;
                case GamepadButton.X:
                    return actualState.Buttons.X == ButtonState.Pressed;
                case GamepadButton.Y:
                    return actualState.Buttons.Y == ButtonState.Pressed;
                case GamepadButton.Up:
                    return actualState.DPad.Up == ButtonState.Pressed;
                case GamepadButton.Down:
                    return actualState.DPad.Down == ButtonState.Pressed;
                case GamepadButton.Left:
                    return actualState.DPad.Left == ButtonState.Pressed;
                case GamepadButton.Right:
                    return actualState.DPad.Right == ButtonState.Pressed;
                case GamepadButton.Start:
                    return actualState.Buttons.Start == ButtonState.Pressed;
                case GamepadButton.Back:
                    return actualState.Buttons.Back == ButtonState.Pressed;
                case GamepadButton.LeftStick:
                    return actualState.Buttons.LeftStick == ButtonState.Pressed;
                case GamepadButton.RightStick:
                    return actualState.Buttons.RightStick == ButtonState.Pressed;
                case GamepadButton.LeftShoulder:
                    return actualState.Buttons.LeftShoulder == ButtonState.Pressed;
                case GamepadButton.RightShoulder:
                    return actualState.Buttons.RightShoulder == ButtonState.Pressed;
                case GamepadButton.Guide:
                    return actualState.Buttons.Guide == ButtonState.Pressed;
                default:
                    return false;
            }
        }

        public CachedGamepadState(PlayerIndex playerIndex)
        {
            this.playerIndex = playerIndex;
            prevState = GamePad.GetState(playerIndex);
            actualState = GamePad.GetState(playerIndex);
        }

        public void Update()
        {
            prevState = actualState;
            actualState = GamePad.GetState(playerIndex);
        }
    }
}