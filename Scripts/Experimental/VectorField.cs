using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Force
{
    attract,
    repel,
}

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class VectorField : MonoBehaviour
{
    [SerializeField] float fieldStrength = 9.81f;
    public float FieldStrength
    {
        get { return fieldStrength; }
        set { fieldStrength = value; }
    }
    [SerializeField] private bool adjustOrientations = false;
    [SerializeField] private bool visible = true;
    [SerializeField] private Rigidbody sourceBody;
    private Vector3 g;
    private List<Rigidbody> bodies = new List<Rigidbody>();
    private Vector3 relativePosition;
    private WaitForFixedUpdate waitfixedUpdate;

    // Use this for initialization
	void Start ()
    {
        if (!sourceBody)
        {
            sourceBody = gameObject.GetComponent<Rigidbody>();
        }
        sourceBody.isKinematic = true;
        gameObject.GetComponent<Collider>().isTrigger = true;
        gameObject.GetComponent<Renderer>().enabled = visible;
        waitfixedUpdate = new WaitForFixedUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != gameObject 
            && !other.GetComponent<VectorField>()
            && other.attachedRigidbody)
        {
            other.attachedRigidbody.useGravity = false;
            bodies.Add(other.attachedRigidbody);

            if (bodies.Count == 1)
            {
                StartCoroutine("Influence");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody && bodies.Contains(other.attachedRigidbody))
        {
            bodies.Remove(other.attachedRigidbody);
        }
    }

    private IEnumerator Influence()
    {
        while (bodies.Count > 0)
        {
            foreach (var body in bodies)
            {
                relativePosition = sourceBody.position - body.position;
                g = relativePosition.normalized * (sourceBody.mass / relativePosition.sqrMagnitude);

                if (adjustOrientations)
                {
                    body.rotation = Quaternion.FromToRotation(-body.transform.up, relativePosition.normalized) * body.rotation;
                }

                body.velocity += relativePosition.normalized * fieldStrength * Time.deltaTime;
                //body.velocity += g;
            }

            yield return waitfixedUpdate;
        }
    }
}
