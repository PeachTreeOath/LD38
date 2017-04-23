
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
            
            AudioManager.Instance.PlaySound("Item_Pickup_Sound", 8.0f);
            PlayerInventoryManager.Instance.CollectedPickup();
            Destroy(this.gameObject);
        }
    }
}
