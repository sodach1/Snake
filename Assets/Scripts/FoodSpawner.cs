using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] Transform rightBound;
    [SerializeField] Transform leftBound;
    [SerializeField] Transform upperBound;
    [SerializeField] Transform lowerBound;

    [Space(10f)]

    [SerializeField] GameObject foodPrefab;

    public static FoodSpawner instance { get; private set; }

    private void Awake()
    {
        if (instance == null) // сделал чисто ради одного свойства GetRandomPositionInBounds, идея не очень, то так как у нас не сущности поля, щито поделать десу
            instance = this;

        else
            Destroy(gameObject);

        Snake.OnFoodGrabbed += SpawnFood;        
    }

    private void OnDestroy()
    {
        Snake.OnFoodGrabbed -= SpawnFood;
    }

    private void OnEnable()
    {
        SpawnFood();
    }

    private void SpawnFood()
    {
        GameObject food = Instantiate(foodPrefab, GetRandomPositionInBounds, Quaternion.identity);
        food.transform.SetParent(transform);
    }

    public Vector3 GetRandomPositionInBounds => new Vector3((int)Random.Range(rightBound.position.x, leftBound.position.x),
            (int)Random.Range(upperBound.position.y, lowerBound.position.y), 0f);
}
