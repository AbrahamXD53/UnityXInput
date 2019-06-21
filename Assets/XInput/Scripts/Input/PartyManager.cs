using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using System;
using System.Linq;
using UnityStandardAssets.CrossPlatformInput;

namespace XInput
{
    public class PartyManager : MonoBehaviour
    {
        #region ATTRIBUTES
        public delegate void PlayerChanged(int playerIndex);
        public delegate void PlayersReady();

        public static event PlayersReady AllPlayersReady;
        public static event PlayersReady NotAllPlayersReady;
        public static event PlayerChanged PlayerAdded;
        public static event PlayerChanged PlayerReady;
        public static event PlayerChanged PlayerNotReady;
        public static event PlayerChanged PlayerRemoved;
        public static event PlayerChanged PlayerCharacterChanged;

        public static PartyManager Instance;

        public ControllerConfig controllerConfig;
        protected List<PlayerInput> players;
        public List<PlayerInput> Players { get { return players; } }

        public bool readingControls = false;
        public int maxCharacters = 4;
        public int maxPlayers = 4;
        public bool allowRepeatedChars = true;
        public bool allowVibration = true;

        public int cancelButton = 1;
        public int joinButton = 0;
        public int horizontalAxis = 0;

        protected bool[] coldDowns = new bool[] { true, true, true, true };
        protected bool[] usedCharacters;
        protected int currentControllerIndex = 0;
        protected bool allPlayersReady = false;

        public bool remaping;
        protected InputDevice remapingDevice;

        #endregion

        #region UNITY_EVENTS
        private void Start()
        {
            if (Instance)
            {
                Destroy(Instance.gameObject);
                return;
            }

            players = new List<PlayerInput>();
            controllerConfig.Init();
            Instance = this;
            DontDestroyOnLoad(gameObject);
            readingControls = true;
            usedCharacters = new bool[maxCharacters];
        }

        private void Update()
        {
            if (readingControls)
            {
                ReadControls();
            }
            else
            {
                controllerConfig.Update();
            }


            if (remaping)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (controllerConfig.IsConnected(i))
                    {
                        var input = controllerConfig.GetGamepad(i).GetButtonPressed();
                        if (input != GamepadButton.None)
                        {
                            Debug.Log("Pressed Button " + input + ", on controller " + i);
                            controllerConfig.Remap(0, input, 0);
                            remaping = false;
                            Debug.Log("Done!");
                        }
                    }
                }
                Debug.Log("Mapping");
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    remaping = true;
                }
            }
        }

        private void LateUpdate()
        {
            SendAllPlayerEvents();
        }

        #endregion


        protected IEnumerator StarColdDown(int control)
        {
            coldDowns[control] = false;
            yield return new WaitForSeconds(.3f);
            coldDowns[control] = true;
        }

        protected IEnumerator DetectControllers()
        {
            yield return new WaitForSeconds(1);
            readingControls = true;
        }

        public ControllerConfig GetConfig()
        {
            return controllerConfig;
        }

        /*Check if controller has joinned*/
        public bool IsControllIn(InputDevice inputDevice, PlayerIndex k)
        {
            if (players == null || players.Count == 0)
                return false;
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].IsPlayer(inputDevice, k))
                {
                    return true;
                }
            }
            return false;
        }

        protected void ChangeCharacter(InputDevice inputDevice, PlayerIndex playerIndex, Vector2 direction)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (!coldDowns[i])
                {
                    continue;
                }
                if (!players[i].ready && players[i].IsPlayer(inputDevice, playerIndex))
                {
                    StartCoroutine(StarColdDown(i));
                    players[i].selectedCharacter += (int)direction.x;
                    if (players[i].selectedCharacter >= maxCharacters)
                        players[i].selectedCharacter = 0;
                    if (players[i].selectedCharacter < 0)
                        players[i].selectedCharacter = maxCharacters - 1;

                    while (usedCharacters[players[i].selectedCharacter])
                    {
                        players[i].selectedCharacter += (int)direction.x;
                        if (players[i].selectedCharacter >= maxCharacters)
                            players[i].selectedCharacter = 0;
                        if (players[i].selectedCharacter < 0)
                            players[i].selectedCharacter = maxCharacters - 1;
                    }
                    if (PlayerCharacterChanged != null)
                    {
                        PlayerCharacterChanged(players[i].realIndex);
                    }
                }
            }
        }

        public bool CheckAllPlayers()
        {
            if (players != null)
            {
                var res = players.Count > 0;

                for (int i = 0; i < players.Count; i++)
                {
                    res &= players[i].ready;
                }
                return res;
            }
            return false;
        }

        protected void SendAllPlayerEvents()
        {
            var prevPlayers = allPlayersReady;
            allPlayersReady = CheckAllPlayers();
            if (!prevPlayers && allPlayersReady)
            {
                if (AllPlayersReady != null)
                {
                    AllPlayersReady.Invoke();
                }
            }
            if (prevPlayers && !allPlayersReady)
            {
                if (NotAllPlayersReady != null)
                {
                    NotAllPlayersReady.Invoke();
                }
            }
        }

        public void StopReadingControls()
        {
            readingControls = false;
        }

        protected void SetPlayerReady(InputDevice inputDevice, int player, PlayerIndex playerIndex)
        {
            if (PlayerReady != null)
                PlayerReady(player);

            if (inputDevice == InputDevice.Gamepad)
            {
                controllerConfig.Vibrate(playerIndex, 0.3f, 0.3f);
            }
            usedCharacters[players[player].selectedCharacter] = true;

            if (!allowRepeatedChars)
            {
                for (int j = 0; j < players.Count; j++)
                {
                    if (j != player && players[player].selectedCharacter == players[j].selectedCharacter)
                        ChangeCharacter(players[j].inputDevice, players[player].controlIndex, Vector2.right);
                }
            }
            players[player].ready = true;
        }

        protected void FixPlayerCount(InputDevice inputDevice, PlayerIndex index)
        {
            PlayerInput cont;
            if (inputDevice != InputDevice.Gamepad && inputDevice != InputDevice.GenericGamepad)
            {
                cont = (from p in players where p.IsInput(inputDevice) select p).First();
            }
            else
            {
                currentControllerIndex--;
                cont = (from p in players where p.IsInput(inputDevice, index) select p).First();
            }
            for (int i = cont.realIndex + 1; i < players.Count; i++)
            {
                players[i].realIndex--;
            }
            players.Remove(cont);
            if (PlayerRemoved != null)
                PlayerRemoved(cont.realIndex);
        }

        protected void RemovePlayer(InputDevice inputDevice, PlayerIndex index)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].ready)
                {
                    if (players[i].IsPlayer(inputDevice, index))
                    {
                        players[i].ready = false;
                        usedCharacters[players[i].selectedCharacter] = false;
                        if (PlayerNotReady != null)
                            PlayerNotReady(i);

                        return;
                    }
                }
            }
            FixPlayerCount(inputDevice, index);
        }



        private void AddPlayer(InputDevice inputDevice, PlayerIndex index)
        {
            if (players == null)
                players = new List<PlayerInput>();

            if (players.Count >= maxPlayers)
                return;

            if (inputDevice == InputDevice.Gamepad || inputDevice == InputDevice.GenericGamepad)
            {
                currentControllerIndex++;
            }
            if (inputDevice == InputDevice.Gamepad)
            {
                controllerConfig.Vibrate(index, 0.3f, 0.3f);
            }

            var cont = new PlayerInput();
            cont.controlIndex = index;
            cont.realIndex = players.Count;
            cont.inputDevice = inputDevice;
            cont.ready = false;

            players.Add(cont);
            if (PlayerAdded != null)
                PlayerAdded(cont.realIndex);
            if (PlayerNotReady != null)
                PlayerNotReady(cont.realIndex);
            if (PlayerCharacterChanged != null)
                PlayerCharacterChanged(cont.realIndex);
        }

        protected void CheckByDevice(InputDevice inputDevice, PlayerIndex playerIndex = PlayerIndex.One)
        {
            var axisVal = controllerConfig.GetAxis(horizontalAxis, inputDevice, (int)playerIndex);
            if (axisVal > 0)
            {
                ChangeCharacter(inputDevice, playerIndex, Vector2.right);
            }
            if (axisVal < 0)
            {
                ChangeCharacter(inputDevice, playerIndex, Vector2.left);
            }

            if (controllerConfig.ButtonDown(cancelButton, inputDevice, (int)playerIndex))
            {
                if (IsControllIn(inputDevice, playerIndex))
                {
                    RemovePlayer(inputDevice, playerIndex);
                }
            }

            if (controllerConfig.ButtonDown(joinButton, inputDevice, (int)playerIndex))
            {
                if (!IsControllIn(inputDevice, playerIndex))
                {
                    AddPlayer(inputDevice, playerIndex);
                }
                else
                {
                    for (int i = 0; i < players.Count; i++)
                    {
                        if (players[i].IsPlayer(inputDevice, playerIndex) && !players[i].ready)
                        {
                            SetPlayerReady(inputDevice, i, playerIndex);
                        }
                    }
                }
            }
        }

        protected void ReadControls()
        {
            controllerConfig.Update();


#if USE_GENERICINPUT
            for (int i = 0; i < Input.GetJoystickNames().Length; i++)
            {
                CheckByDevice(InputDevice.GenericGamepad, (PlayerIndex)i);
            }
#else
            for (int i = 0; i < controllerConfig.maxControllers; i++)
            {
                if (controllerConfig.IsConnected(i))
                {
                    CheckByDevice(InputDevice.Gamepad, (PlayerIndex)i);
                }
            }
#endif

            CheckByDevice(InputDevice.KeyboardMouse);
#if MOBILE_INPUT
            CheckByDevice(InputDevice.Touch);
#endif
        }
    }
}