using UnityEngine;
using Hykudoru.Events.V2;

namespace Hykudoru
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform parent;
        [SerializeField] Transform target;
        public Transform Target { get { return target; } set { target = value; } }
        [SerializeField] Vector3 offset;
        public Vector3 Offset { get { return offset; } set { offset = value; } }
        [SerializeField] float smoothSpeed = .25f;
        public float SmoothSpeed { get { return smoothSpeed; } set { smoothSpeed = value; } }

        private void Start()
        {
            if (parent == null)
            {
                parent = transform;
            }

            OnChangeTarget(target);
        }

        private void OnEnable()
        {
            EventManager<Transform>.AddListener("OnCameraChangeTarget", OnChangeTarget);
            OnChangeTarget(target);
        }

        private void OnDisable()
        {
            EventManager<Transform>.RemoveListener("OnCameraChangeTarget", OnChangeTarget);
        }

        void LateUpdate()
        {
            if (target != null)
            {
                parent.position += ((target.position + offset) - parent.position) / smoothSpeed;//parent.position = Vector3.Lerp(parent.position, target.position + offset, smoothSpeed);
                parent.LookAt(target);
            }
        }

        void OnChangeTarget(Transform newTarget)
        {
            if (newTarget != null)
            {
                target = newTarget;
                parent.position = target.position + offset;
                parent.LookAt(target);
            }
        }
    }
}