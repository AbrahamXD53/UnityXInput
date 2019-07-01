using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

namespace XInput
{
    public class ControllerSupport : MonoBehaviour
    {
        protected static ControllerSupport instance;
        public static ControllerSupport Instance
        {
            get
            {
                if (ReferenceEquals(instance, null))
                {
                    var go = new GameObject("ControllerSupport");
                    instance = go.AddComponent<ControllerSupport>();
                }
                return instance;
            }
        }

        void Start()
        {
            if (!ReferenceEquals(instance, null))
            {
                if (!ReferenceEquals(instance, this))
                {
                    Destroy(gameObject);
                    return;
                }
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        protected IEnumerator VibrateCorutine(GamepadIndex index, float intensity, float duration)
        {
            GamePad.SetVibration((PlayerIndex)index, intensity, intensity);
            yield return new WaitForSeconds(duration);
            GamePad.SetVibration((PlayerIndex)index, 0, 0);
        }

        public void Vibrate(GamepadIndex playerIndex, float intensity = 1.0f, float duration = 0.1f)
        {
            StartCoroutine(VibrateCorutine(playerIndex, intensity, duration));
        }
    }
}