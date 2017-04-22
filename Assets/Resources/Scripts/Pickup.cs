
using UnityEngine;

public class Pickup : MonoBehaviour
{
    /// <summary>
    /// Handle player collision with pickup.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            PlayerInventoryManager.Instance.CollectedPickup();
            Destroy(this.gameObject);
        }
    }
}
