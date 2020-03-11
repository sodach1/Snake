using UnityEngine;

public class Food : MonoBehaviour
{
    private void OnDisable() => Destroy(gameObject);

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
            transform.position = FoodSpawner.instance.GetRandomPositionInBounds;
    }

}
