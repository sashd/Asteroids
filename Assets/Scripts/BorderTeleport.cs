using UnityEngine;

public class BorderTeleport : MonoBehaviour
{
    private Vector2 screenBorders;

    private void Start()
    {
        screenBorders = ScreenBounds.Borders;
    }

    private void Update()
    {
        if (Mathf.Abs(transform.position.x) > screenBorders.x)
        {
            transform.position = new Vector2(Mathf.Sign(transform.position.x) * -screenBorders.x, transform.position.y);
        }

        if (Mathf.Abs(transform.position.y) > screenBorders.y)
        {
            transform.position = new Vector2(transform.position.x, Mathf.Sign(transform.position.y) * -screenBorders.y);
        }
    }
}
