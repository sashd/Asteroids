using System.Collections;
using UnityEngine;

public class AsteroidsManager : MonoBehaviour
{
    [SerializeField] private float newWaveTime = 2f;
    [SerializeField] private int startAsteroidCount = 2;
    private int currentAsteroidCount = 0;

    private void Awake()
    {
        Asteroid.OnAsteroidDestroy += OnAsteroidDestroy;
        Asteroid.OnAsteroidCreate += OnAsteroidCreate;
    }   

    private void Start() 
    {
        StartCoroutine(NewWave(startAsteroidCount));
    }

    private IEnumerator NewWave(int count)
    {
        yield return  new WaitForSeconds(newWaveTime);

        for (int i = 0; i < count; i++)
        {
            GameObject asteroid = ObjectPooler.instance.Get("LargeAsteroid");
            asteroid.transform.position = new Vector2(Random.Range(-ScreenBounds.Borders.x, ScreenBounds.Borders.x), Random.Range(-ScreenBounds.Borders.y, ScreenBounds.Borders.y));
            asteroid.GetComponent<Asteroid>().Init();
        }
    }
    private void OnAsteroidDestroy(Asteroid.AsteroidType type)
    {
        currentAsteroidCount--;

        if (currentAsteroidCount < 1)
        {
            startAsteroidCount++;
            StartCoroutine(NewWave(startAsteroidCount));
        }
    }
    private void OnAsteroidCreate()
    {
        currentAsteroidCount++;
    }

    private void OnDestroy()
    {
        Asteroid.OnAsteroidDestroy -= OnAsteroidDestroy;
        Asteroid.OnAsteroidCreate  -= OnAsteroidCreate;
    }
}
