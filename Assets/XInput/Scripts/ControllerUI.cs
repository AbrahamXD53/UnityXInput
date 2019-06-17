using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XInput
{
    public class ControllerUI : MonoBehaviour
    {
        public GameObject playerPrefab;

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
        }

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
                GUI.Label(new Rect(40, 100, 360, 80), "Press A Button o Space to join");
            }
        }
    }
}