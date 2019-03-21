using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravitationalMass : MonoBehaviour
{
    public GravitationalField CurrentGravitationalField { private get; set; }
    private Rigidbody rb;

    // Use this for initialization
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (CurrentGravitationalField != null)
        {
            CurrentGravitationalField.Unrealistic(rb);
        }
    }
}