using UnityEngine;
using System.Collections;

public class CollisionController : MonoBehaviour 
{

    Collider2D[] colliders;
    void Awake()
    {
        colliders = GetComponents<Collider2D>();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
    }


	// Use this for initialization
	void Start () 
    {
	   //     gameObject.AddComponent<Rigidbody2D>().isKinematic = true;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnBecameInvisible()
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
    }

    void OnBecameVisible()
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = true;
        }
    }


}
