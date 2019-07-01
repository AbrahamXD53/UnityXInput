using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace XInput
{
    public class ControllerUI : MonoBehaviour
    {
        public bool remaping;
        protected InputDevice remapingDevice;
        public int controlIndex;
        public int actionIndex;

        private float currentTime;
        public float waitTime;

        public GameObject playerPrefab;
        protected ControllerConfig controller;

        private Vector2 scrollPositionButtons, scrollPositionAxis;

        private void Start()
        {
            PartyManager.PlayerAdded += (player) =>
            {
                Debug.Log("Player " + player + " joinned");
            };

            PartyManager.PlayerRemoved += (player) =>
            {
                Debug.Log("Player " + player + " removed");
            };

            PartyManager.PlayerReady += (player) =>
            {
                Debug.Log("Player " + player + " ready");
            };

            PartyManager.PlayerCharacterChanged += (player) =>
            {
                Debug.Log("Player " + player + " changed character: " + PartyManager.Instance.Players[player].selectedCharacter);
            };

            PartyManager.PlayerNotReady += (player) =>
            {
                Debug.Log("Player " + player + " not ready");
            };

            PartyManager.AllPlayersReady += () =>
            {
                PartyManager.Instance.StopReadingControls();

                var players = PartyManager.Instance.Players;
                for (int i = 0; i < players.Count; i++)
                {
                    var go = Instantiate(playerPrefab, new Vector3(i * 2f, 1f, 0f), Quaternion.identity);
                    go.GetComponent<PlayerController>().Init(players[i]);
                }
                Debug.Log("All players are ready");
            };

            PartyManager.NotAllPlayersReady += () =>
            {
                Debug.Log("Not All players are ready");
            };

            controller = PartyManager.Instance.controllerConfig;
        }

        void Update()
        {
            if (remaping)
            {
                if (remapingDevice == InputDevice.Gamepad)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (controller.IsConnected(i))
                        {
                            var input = controller.GetGamepad(i).GetButtonPressed();
                            if (input != GamepadButton.None)
                            {
                                Debug.Log("Pressed Button " + input + ", on controller " + i);
                                controller.Remap(actionIndex, input, controlIndex);
                                remaping = false;
                                currentTime = 0;
                                Debug.Log("Done!");
                            }
                        }
                    }
                }
                else
                {
                    var input = controller.GetKeyPressed();
                    if (input != KeyCode.None)
                    {
                        Debug.Log("Pressed key " + input + ", on kb ");
                        controller.Remap(actionIndex, input, controlIndex);
                        remaping = false;
                        currentTime = 0;
                        Debug.Log("Done!");
                    }
                }
                Debug.Log("Mapping");
            }
        }

        void Remap(InputDevice inputDevice, int index, int actionIndex)
        {
            if (index < 0)
            {
                if (inputDevice == InputDevice.KeyboardMouse)
                {
                    controller.Remap(actionIndex, KeyCode.None, index);
                }
                else
                {
                    controller.Remap(actionIndex, GamepadButton.None, index);
                }
            }
            else
            {
                remapingDevice = inputDevice;
                controlIndex = index;
                this.actionIndex = actionIndex;
                remaping = true;
            }
        }

        /*TODO clean up this mess!*/
        private void OnGUI()
        {
            GUI.Box(new Rect(20, 20, 400, 400), "Players");
            var players = PartyManager.Instance.Players;
            if (players.Count > 0)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    GUI.RepeatButton(new Rect(40, 50 + i * 90, 360, 80), string.Format("Player {0}, ready? {1}, char: {2}, device: {3}", i + 1, players[i].ready, players[i].selectedCharacter, players[i].inputDevice));
                }
            }
            else
            {
                GUI.Label(new Rect(40, 100, 360, 80), "Press 'Ok Action' to join");
            }

            var schema = controller.GetSchema();
            GUI.Box(new Rect(430, 20, 400, 500), "Controllers: " + schema.name);

            if (remaping)
            {
                currentTime += Time.deltaTime;
                GUI.Label(new Rect(540, 80, 200, 100), string.Format("Remaping: Press any key in device: {0}\nOr Wait: {1}s", remapingDevice, (int)(waitTime - currentTime)));

                if ((waitTime - currentTime) < 0)
                {
                    remaping = false;
                    currentTime = 0;
                }
            }
            else
            {
                for (int i = 0; i < controller.schemas.Length; i++)
                {
                    if (GUI.Button(new Rect(450 + 110 * i, 50, 100, 40), controller.GetSchema(i).name))
                    {
                        controller.LoadSchema(i);
                    }
                }

                controller.allowVibration = GUI.Toggle(new Rect(450, 95, 150, 25), controller.allowVibration, "Use vibration");

                scrollPositionButtons = GUI.BeginScrollView(new Rect(440, 120, 360, 150), scrollPositionButtons, new Rect(0, 0, 340, schema.ActionButtons.Length * 85));
                for (int i = 0; i < schema.ActionButtons.Length; i++)
                {
                    GUI.Box(new Rect(5, 0 + i * 85, 330, 80), "");

                    var text = "Input (Id: " + i + "): " + schema.ActionButtons[i].name + "\nGp: [";
                    text += string.Join(", ", schema.ActionButtons[i].buttonGamepad);
                    for (int j = 0; j < schema.ActionButtons[i].buttonGamepad.Count; j++)
                    {
                        if (GUI.Button(new Rect(120 + 50 * j, 10 + i * 85, 40, 25), "G(" + j + ")"))
                        {
                            Remap(InputDevice.Gamepad, j, i);
                        }
                    }
                    if (GUI.Button(new Rect(120 + 50 * schema.ActionButtons[i].buttonGamepad.Count, 10 + i * 85, 40, 25), "+"))
                    {
                        Remap(InputDevice.Gamepad, schema.ActionButtons[i].buttonGamepad.Count, i);
                    }
                    if (schema.ActionButtons[i].buttonGamepad.Count > 0)
                    {
                        if (GUI.Button(new Rect(120 + 50 * (schema.ActionButtons[i].buttonGamepad.Count + 1), 10 + i * 85, 40, 25), "-"))
                        {
                            Remap(InputDevice.Gamepad, -1, i);
                        }
                    }

                    text += "]\nKb: [";
                    text += string.Join(", ", schema.ActionButtons[i].buttonKeyboard);
                    for (int j = 0; j < schema.ActionButtons[i].buttonKeyboard.Count; j++)
                    {
                        if (GUI.Button(new Rect(120 + 50 * j, 40 + i * 85, 40, 25), "K(" + j + ")"))
                        {
                            Remap(InputDevice.KeyboardMouse, j, i);
                        }
                    }
                    if (GUI.Button(new Rect(120 + 50 * schema.ActionButtons[i].buttonKeyboard.Count, 40 + i * 85, 40, 25), "+"))
                    {
                        Remap(InputDevice.KeyboardMouse, schema.ActionButtons[i].buttonKeyboard.Count, i);
                    }
                    if (schema.ActionButtons[i].buttonKeyboard.Count > 0)
                    {
                        if (GUI.Button(new Rect(120 + 50 * (schema.ActionButtons[i].buttonKeyboard.Count + 1), 40 + i * 85, 40, 25), "-"))
                        {
                            Remap(InputDevice.KeyboardMouse, -1, i);
                        }
                    }
                    text += "]\nCI: [";
                    text += string.Join(", ", schema.ActionButtons[i].buttonNames);
                    text += "]";

                    GUI.Label(new Rect(15, 0 + i * 85, 330, 80), text);
                }
                GUI.EndScrollView();

                scrollPositionAxis = GUI.BeginScrollView(new Rect(440, 300, 360, 150), scrollPositionAxis, new Rect(0, 0, 340, schema.ActionAxis.Length * 85));
                for (int i = 0; i < schema.ActionAxis.Length; i++)
                {
                    var text = "Input (Id: " + i + "): " + schema.ActionAxis[i].name + "\nGp: [";
                    text += string.Join(", ", schema.ActionAxis[i].axes);
                    text += "]\nKb: [";
                    text += string.Join(", ", schema.ActionAxis[i].axisNames);
                    text += "]\nCI: [";
                    text += string.Join(", ", schema.ActionAxis[i].touchNames);
                    text += "]";

                    GUI.Box(new Rect(5, 0 + i * 85, 330, 80), "");
                    GUI.Label(new Rect(15, 0 + i * 85, 330, 80), text);
                }
                GUI.EndScrollView();

                if (GUI.Button(new Rect(500, 475, 120, 25), "Defaults"))
                {
                    controller.RevertChanges();
                }

                if (GUI.Button(new Rect(630, 475, 120, 25), "Save"))
                {
                    controller.SaveChanges();
                }
            }
        }
    }
}