using UnityEngine;

public class ElementScroller : MonoBehaviour
{
    #region Inspector Fields
    // Speed of movement
    [SerializeField] private float speed = 8.0f;
    // Left extreme (start position when resetting)
    [SerializeField] private float startX = 40.0f;
    // Right extreme (end position before resetting)
    [SerializeField] private float endX = -40.0f;      
    #endregion

    //Unity - Update
    private void Update()
    {
        // Get the current local position of the object
        Vector3 localPos = transform.localPosition;

        // Move towards the right (endX)
        localPos.x = Mathf.MoveTowards(localPos.x, endX, speed * Time.deltaTime);

        // If the object reaches endX, immediately reset to startX
        if (Mathf.Abs(endX - localPos.x) <= 0)
        {
            // Reset position to startX
            localPos.x = startX;  
        }

        // Apply the updated local position
        transform.localPosition = localPos;
    }
}
