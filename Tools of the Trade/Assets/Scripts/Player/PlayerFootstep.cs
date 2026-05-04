using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class PlayerFootstep : MonoBehaviour
{
    [SerializeField] LayerMask groundMask;
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            playerSFX.PlayMovementSFX();
            onFootstep.Invoke();
        }
    }
}
