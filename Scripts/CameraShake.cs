using System.Collections;
using UnityEngine;
using Gemukodo.Events.V2;

namespace Gemukodo
{
    public static partial class EventType
    {
        public const string OnCameraShake = "OnCameraShake";
    }

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

    public class CameraShake : MonoBehaviour
    {
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
            EventManager.AddListener(EventType.OnCameraShake, OnShake);
            EventManager<CameraShakeEventArgs>.AddListener(EventType.OnCameraShake, OnShake);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener(EventType.OnCameraShake, OnShake);
            EventManager<CameraShakeEventArgs>.RemoveListener(EventType.OnCameraShake, OnShake);
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