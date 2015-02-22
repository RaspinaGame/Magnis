using UnityEngine;
using System.Collections;

public class InstantDeath : MonoBehaviour
{
    PlayerController playerController;
    // Use this for initialization
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("obstacle") ||other.gameObject.CompareTag("wall") )
        {
            playerController.Die("InstantDeath");
        }
    }


}
