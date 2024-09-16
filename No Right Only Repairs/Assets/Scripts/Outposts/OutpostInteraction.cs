using UnityEngine;

public class OutpostInteraction : MonoBehaviour
{
    public UpgradeStore upgradeStore;
    private bool isInteracting = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInteracting = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInteracting = false;
        }
    }

}
