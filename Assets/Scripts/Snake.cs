using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Snake : MonoBehaviour
{
    [SerializeField] float defaultMoveRate = .3f;
    private float moveRate = .3f;
    private Vector3 currentDirection = Vector3.right;
    private float timeSinceLastMove = 0f;
    private bool canSetDirection = true;

    [SerializeField] GameObject tailPrefab;
    private List<GameObject> snakeList = new List<GameObject>();

    public static UnityAction OnFoodGrabbed = null; // не люблю безосновательно ликовать компоненты во-первых, а во вторых этим еще и скор-метр пользуется
    public static UnityAction OnLoose = null;
    public static UnityAction OnSnakeSpawned = null;

    private void Awake()
    {
        GameManager.OnSetDirection += SetDirection;
    }

    private void OnDestroy()
    {
        GameManager.OnSetDirection -= SetDirection;
    }

    private void OnEnable()
    {
        snakeList = new List<GameObject>
        {
            gameObject
        };

        transform.position = Vector3.zero;
        moveRate = defaultMoveRate;
        currentDirection = Vector3.right;
        timeSinceLastMove = 0f;

        OnSnakeSpawned?.Invoke();
    }

    private void FixedUpdate()
    {
        CheckForInput();

        timeSinceLastMove += Time.fixedDeltaTime;

        if (timeSinceLastMove >= moveRate)
        {
            timeSinceLastMove = 0f;
            Move();
        }

    }

    /// <summary>
    /// это для кампутера
    /// </summary>
    private void CheckForInput()
    {
        if (Input.GetKey(KeyCode.A))
            SetDirection(Vector3.left);
        else if (Input.GetKey(KeyCode.D))
            SetDirection(Vector3.right);
        else if (Input.GetKey(KeyCode.W))
            SetDirection(Vector3.up);
        else if (Input.GetKey(KeyCode.S))
            SetDirection(Vector3.down);

    }

    private void SetDirection(Vector3 direction)
    {
        if (direction != currentDirection && direction != -currentDirection && canSetDirection)
        {
            currentDirection = direction; // )
            canSetDirection = false;
        }
    }

    private void Move()
    {
        UpdateTail();
        transform.position += currentDirection;
        canSetDirection = true;
    }

    private void CreateTailElement()
    {
        GameObject tail = Instantiate(tailPrefab,
            transform.position - currentDirection, Quaternion.identity);
        tail.name = string.Format("Tail {0}", Random.Range(0, 1000));
        tail.transform.SetParent(transform.parent);

        snakeList.Add(tail);
    }

    private void UpdateTail()
    {
        for (int i = snakeList.Count - 1; i > 0; i--)
        {
            snakeList[i].transform.position = snakeList[i - 1].transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            OnLoose?.Invoke();

            foreach (var part in snakeList)
            {
                if (part != gameObject)
                    Destroy(part);
            }

            snakeList = new List<GameObject>();

        }
        else if (other.CompareTag("Food"))
        {
            OnFoodGrabbed?.Invoke();
            Destroy(other.gameObject);
            CreateTailElement();

            if (moveRate >= .06f)
                moveRate -= .015f;
        }

    }

}
