using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Instance;
    [SerializeField] private GameObject photo1Prefab;
    [SerializeField] private GameObject photo2Prefab;
    [SerializeField] private Transform photo1SpawnPoint;
    [SerializeField] private Transform[] photo2SpawnPoints;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameManager.GameState newState)
    {
        switch (newState)
        {
            case GameManager.GameState.Puzzle1:
                SpawnPhoto2();
                break;
            case GameManager.GameState.Puzzle2:
                break;
            default:
                break;
        }
    }
    public void DropPhoto1()
    {
        if (photo1Prefab == null || photo1SpawnPoint == null) return;

        GameObject photo = Instantiate(photo1Prefab, photo1SpawnPoint.position, photo1SpawnPoint.rotation);
        Rigidbody rb = photo.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(Vector3.down * 0.05f, ForceMode.Impulse); // hafifçe aşağıya it
            rb.AddTorque(new Vector3(0, 0, 1f), ForceMode.Impulse);

        }
    }

    private void SpawnPhoto2()
    {
        Debug.Log("Spawning Photo 2");
        if (photo2Prefab == null) return;
        Transform spawnPoint = photo2SpawnPoints[Random.Range(0, photo2SpawnPoints.Length)];
        Instantiate(photo2Prefab, spawnPoint.position, Quaternion.identity, spawnPoint);
    }

}
