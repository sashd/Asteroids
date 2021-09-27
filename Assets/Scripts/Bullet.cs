using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private bool playerBullet = true;
    [SerializeField] private LayerMask destroyLayer;

    public void Init(Vector2 velocity) 
    {
        GetComponent<Rigidbody2D>().velocity = velocity;
        StartCoroutine(DestroyBullet(velocity.magnitude));  
    }

    private IEnumerator DestroyBullet(float speed) 
    {
        float destroyTime = 2 * ScreenBounds.Borders.x / speed; 
        yield return new WaitForSeconds(destroyTime);

        DestroyBullet();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (((1 << other.gameObject.layer) & destroyLayer) != 0)
        {
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        if (playerBullet)
        {
            ObjectPooler.instance.Return(gameObject, "Bullet");
        }
        else
        {
            ObjectPooler.instance.Return(gameObject, "UFOBullet");
        }
    }
}
