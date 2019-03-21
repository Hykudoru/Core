using UnityEngine;
using System.Collections;

public class AutoDestruct : MonoBehaviour {
	
	public float lifetime = 0f;
	public bool destroyOnImpact = false;

	// Use this for initialization
	void Start() {
		Destroy(gameObject, lifetime);
	}

	void OnCollisionEnter()
	{
		if (destroyOnImpact) {
			GetComponent<Renderer>().enabled = false;
			Destroy(gameObject);
		}
	}
}