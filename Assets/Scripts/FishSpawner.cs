using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] fishPrefabs;

    public Transform spawnerPoint;

    private Vector2 RandTarget = Vector2.zero;

    [SerializeField] private float spawnRadius = 10f;

    [Tooltip("Spawns in every fish on list X amount of times")]
    [SerializeField] private int spawnAmount = 1;

    private void Awake()
    {
        //mainCamera = FindObjectOfType<Camera>();
    }

    private void Start()
    {
        PlaceFishInGameBounds();
    }

    private void PlaceFishInGameBounds()
    {
        for (int j = 0; j < spawnAmount; j++)
        {
            for (int i = 0; i < fishPrefabs.Length; i++)
            {
                RandTarget = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

                RandTarget = RandTarget.normalized;

                RandTarget *= spawnRadius;

                Vector2 targetWorld = spawnerPoint.transform.TransformPoint(RandTarget);
                fishPrefabs[i].transform.position = targetWorld;
                fishPrefabs[i].transform.position = new Vector3(fishPrefabs[i].transform.position.x, fishPrefabs[i].transform.position.y, 0f);
                Instantiate(fishPrefabs[i]);

            }
        }
        
    }
}