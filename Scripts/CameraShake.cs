using System.Collections;
using UnityEngine;
using Hykudoru.Events.V2;

namespace Hykudoru
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

    public class CameraShake : MonoBehaviour
    {
        private WaitForEndOfFrame cache_waitForEndOfFrame;
        private const string cache_ShakeMethodName = "Shake";

        private void Start()
        {
            if (transform.parent == null)
            {
                Debug.LogWarning("CamerShake: Missing parent transform. The camera shakes with respect to its parent transform.");
            }

            cache_waitForEndOfFrame = new WaitForEndOfFrame();
        }

        private void OnEnable()
        {
            EventManager.AddListener(StringConst.Event.OnCameraShake, OnShake);
            EventManager<CameraShakeEventArgs>.AddListener(StringConst.Event.OnCameraShake, OnShake);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener(StringConst.Event.OnCameraShake, OnShake);
            EventManager<CameraShakeEventArgs>.RemoveListener(StringConst.Event.OnCameraShake, OnShake);
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
                StartCoroutine(cache_ShakeMethodName, args);
            }
        }

        private IEnumerator Shake(CameraShakeEventArgs args)
        {
            Vector3 initLocalPos = transform.localPosition;
            float startTime = Time.timeSinceLevelLoad;

            while (Time.timeSinceLevelLoad - startTime < args.Seconds)
            {
                transform.localPosition = new Vector3(Random.Range(-1f, 1f) * args.Intensity, Random.Range(-1f, 1f) * args.Intensity, transform.localPosition.z);

                yield return cache_waitForEndOfFrame;
            }

            transform.localPosition = initLocalPos;
        }
    }
}