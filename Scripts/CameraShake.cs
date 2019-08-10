using System.Collections;
using UnityEngine;
//using Gemukodo.Events.v2;
using Gemukodo.Events.v4;

namespace Gemukodo
{
    public static partial class EventType
    {
        public const string OnCameraShake = "OnCameraShake";
    }

    public class CameraShake : MonoBehaviour
    {
        public struct CameraShakeEventArgs
        {
            public float Seconds { get; set; }
            public float Intensity { get; set; }

            public CameraShakeEventArgs(float seconds = .5f, float intensity = .25f)
            {
                Intensity = intensity;
                Seconds = seconds;
            }
        }

        private WaitForEndOfFrame waitForEndOfFrame;

        private void Start()
        {
            if (transform.parent == null)
            {
                Debug.LogWarning("CamerShake: Missing parent transform. The camera shakes with respect to its parent transform.");
            }

            waitForEndOfFrame = new WaitForEndOfFrame();
        }

        private void OnEnable()
        {
            EventManager.Register(EventType.OnCameraShake, OnShake);
            EventManager.Register<CameraShakeEventArgs>(EventType.OnCameraShake, OnShake);
        }

        private void OnDisable()
        {
            EventManager.Unregister(EventType.OnCameraShake, OnShake);
            EventManager.Unregister<CameraShakeEventArgs>(EventType.OnCameraShake, OnShake);
        }

        private void OnShake()
        {
            OnShake(new CameraShakeEventArgs());
        }

        private void OnShake(CameraShakeEventArgs args)
        {
            if (transform.parent != null)
            {
                StopAllCoroutines();
                StartCoroutine(EventType.OnCameraShake, args);
            }
        }

        private IEnumerator Shake(CameraShakeEventArgs args)
        {
            Vector3 initLocalPos = transform.localPosition;
            float startTime = Time.timeSinceLevelLoad;

            while (Time.timeSinceLevelLoad - startTime < args.Seconds)
            {
                transform.localPosition = new Vector3(Random.Range(-1f, 1f) * args.Intensity, Random.Range(-1f, 1f) * args.Intensity, transform.localPosition.z);

                yield return waitForEndOfFrame;
            }

            transform.localPosition = initLocalPos;
        }
    }
}