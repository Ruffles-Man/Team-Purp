using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class PlayerFootstep : MonoBehaviour
{
    [SerializeField] UnityEvent onFootstep;

    PlayerSFX playerSFX;
    BoxCollider footCollider;

    void Awake()
    {
        playerSFX = GetComponentInParent<PlayerSFX>();
        footCollider = GetComponent<BoxCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Ground))
        {
            playerSFX.PlayMovementSFX();
            onFootstep.Invoke();
        }
    }
}
