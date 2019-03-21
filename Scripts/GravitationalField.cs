using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class GravitationalField : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    [SerializeField] private float g = -9.81f; //Acceleration of gravity on Earth at sea level
    [SerializeField] private Rigidbody rb;
    private Vector3 gravField;
    private Vector3 gravFieldStrength = Vector3.zero;
    private Vector3 gravForce = Vector3.zero;
    public static float GravitationalConstant = 6.67f; //0.0000000000667f;

    private void Start()
    {
        if (!rb)
        {
            rb = gameObject.GetComponent<Rigidbody>();
        }
        rb.useGravity = false;
        rb.isKinematic = true;
        gameObject.layer = layer;
        GetComponent<SphereCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != gameObject && 
            other.GetComponent<GravitationalField>() == null)
        {
            GravitationalMass massComponent = other.gameObject.GetComponent<GravitationalMass>();
            if (massComponent == null)
            {
                massComponent = other.gameObject.AddComponent<GravitationalMass>();
            }
            massComponent.CurrentGravitationalField = this;
            massComponent.GetComponent<Rigidbody>().useGravity = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject != gameObject &&
            other.GetComponent<GravitationalField>() == null)
        {
            other.gameObject.GetComponent<GravitationalMass>().CurrentGravitationalField = null;
        }
    }

    public void Realistic(Rigidbody otherBody)
    {
        gravField = rb.worldCenterOfMass - otherBody.worldCenterOfMass;
        gravFieldStrength = gravField.normalized * GravitationalConstant * rb.mass / gravField.sqrMagnitude;
        gravForce = gravFieldStrength * otherBody.mass;

        // Adjust orientation
        otherBody.rotation = Quaternion.FromToRotation(-otherBody.transform.up, gravField.normalized) * otherBody.rotation;

        // Pull towards center of mass
        otherBody.velocity += gravForce;
    }

    public void Unrealistic(Rigidbody otherBody)
    {
        gravField = rb.worldCenterOfMass - otherBody.worldCenterOfMass;

        // Adjust orientation
        otherBody.rotation = Quaternion.FromToRotation(-otherBody.transform.up, gravField.normalized) * otherBody.rotation;

        // Pull towards center
        otherBody.velocity += gravField.normalized * otherBody.mass;
    }
}