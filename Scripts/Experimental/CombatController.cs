using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HitBox
{
    [SerializeField] private Collider collider;
    [SerializeField] private LayerMask layer;
    [SerializeField] private int maxHitDamage;
    [SerializeField] private int maxHitPoints;

    [SerializeField] private KeyCode reactKey;

    public Collider Collider
    {
        get { return collider; }
        set { collider = value; }
    }

    public LayerMask Layer
    {
        get { return layer; }
        set { layer = value; }
    }

    public int MaxHitDamage
    {
        get { return maxHitDamage; }
        set { maxHitDamage = value; }
    }

    public int MaxHitPoints
    {
        get { return maxHitPoints; }
        set { maxHitPoints = value; }
    }

    public KeyCode ReactKey
    {
        get { return reactKey; }
        set { reactKey = value; }
    }

    public virtual Collider[] Overlap()
    {
        return Physics.OverlapBox(
            collider.bounds.center, 
            collider.bounds.extents, 
            collider.transform.rotation, 
            layer);
    }
}

public class CombatController : MonoBehaviour
{
    [SerializeField] private HitBox[] hitBoxes;
    private bool isPerformingMove = false;

    // Use this for initialization
    void Start ()
    {
        if (hitBoxes == null)
        {
            this.enabled = false;
        }
	}

    // Update is called once per frame
    void Update ()
    {
        if (!isPerformingMove && Input.anyKeyDown)
        {
            foreach (HitBox hitbox in hitBoxes)
            {
                try
                {
                    if (Input.GetKeyDown(hitbox.ReactKey))
                    {
                        StartCoroutine(HitBox(hitbox));
                    }
                    break;
                }
                catch (Exception)
                {
                    return;
                }
            }
        }
	}

    private IEnumerator HitBox(HitBox hitBox)
    {
        isPerformingMove = true;

        Collider[] cols = hitBox.Overlap();

        foreach (Collider c in cols)
        {
            if (c.transform.root != transform)
            {
                int damage = 0;

                switch (c.name.ToLower())
                {
                    case "head":
                        damage = hitBox.MaxHitDamage;
                        break;
                    case "torso":
                        damage = hitBox.MaxHitDamage / 2;
                        break;
                    default:
                        break;
                }
                
                Debug.Log(c.name);
                //Limit to one collider
                break;
            }
        }

        isPerformingMove = false;
        yield return null;
    }

    private void OnHit()
    {
        StopAllCoroutines();
    }
}
