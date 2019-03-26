using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float blastMagnitude = 0.0f;
    [SerializeField] private float blastRadius = 0.0f;
    [SerializeField] private float upwardsModifier = 0.0f;
    [SerializeField] private ForceMode forceMode = ForceMode.VelocityChange;
    [SerializeField] private LayerMask layersDetect;
    [SerializeField] private bool explodeOnStart = false;
    [SerializeField] private bool explodeOnce = true;

    // Use this for initialization
    private void Start()
    {
        if (explodeOnStart)
        {
            Explode();

            if (explodeOnce)
            {
                Destroy(this);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (blastRadius <= 0f)
        {
            OriginExplosion(collision.rigidbody, blastMagnitude, forceMode);
        }
        else
        {
            Explode();
        }


        if (explodeOnce)
        {
            Destroy(this);
        }
    }

    public void Explode()
    {
        Collider[] detected = Physics.OverlapSphere(transform.position, blastRadius, layersDetect);
        PointRadiusExplosion(detected, transform.position, blastMagnitude, blastRadius);
    }

    //All methods/overloaded methods verify rigidbodies are present before applying physics.

    public static void OriginExplosion(Rigidbody rb, float blastForce, ForceMode mode = ForceMode.Impulse)
    {
        if (rb != null)
        {
            rb.AddExplosionForce(blastForce, rb.position, 0, 1, mode);
        }
    }

    public static void OriginExplosion(Rigidbody[] rbs, float force, ForceMode mode = ForceMode.Impulse)
    {
        foreach (Rigidbody rb in rbs)
        {
            if (rb != null)
            {
                rb.AddExplosionForce(force, rb.position, 0, 1, mode);
            }
        }
    }

    public static void OriginExplosion(Collider collider, float force, ForceMode mode = ForceMode.Impulse)
    {
        if (collider.attachedRigidbody != null)
        {
            collider.attachedRigidbody.AddExplosionForce(force, collider.transform.position, 0, 1, mode);
        }
    }

    public static void OriginExplosion(Collider[] colliders, float force, ForceMode mode = ForceMode.Impulse)
    {
        foreach (Collider collider in colliders)
        {
            if (collider.attachedRigidbody != null)
            {
                collider.attachedRigidbody.AddExplosionForce(force, collider.transform.position, 1, 1, mode);
            }
        }
    }

    //All methods/overloaded methods verify rigidbodies are present before applying physics.

    public static void PointRadiusExplosion(Rigidbody rb, Vector3 explosionPosition, float force, float radius, ForceMode mode = ForceMode.Impulse)
    {
        if (rb != null)
        {
            rb.AddExplosionForce(force, explosionPosition, radius, 1, mode);
        }
    }

    public static void PointRadiusExplosion(Collider collider, Vector3 explosionPosition, float force, float radius, ForceMode mode = ForceMode.Impulse)
    {
        if (collider.attachedRigidbody != null)
        {
            collider.attachedRigidbody.AddExplosionForce(force, explosionPosition, radius, 1, mode);
        }
    }

    public static void PointRadiusExplosion(Rigidbody[] rbs, Vector3 explosionPosition, float force, float radius, ForceMode mode = ForceMode.Impulse)
    {
        foreach (Rigidbody rb in rbs)
        {
            if (rb != null)
            {
                rb.AddExplosionForce(force, explosionPosition, radius, 1, mode);
            }
        }
    }

    public static void PointRadiusExplosion(Collider[] colliders, Vector3 explosionPosition, float force, float radius, ForceMode mode = ForceMode.Impulse)
    {
        foreach (Collider collider in colliders)
        {
            if (collider.attachedRigidbody != null)
            {
                collider.attachedRigidbody.AddExplosionForce(force, explosionPosition, radius, 1, mode);
            }
        }
    }
}