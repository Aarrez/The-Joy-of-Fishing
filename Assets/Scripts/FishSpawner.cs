using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] fishPrefabs;

    private Camera mainCamera;

    private GameObject gameBound;

    private Vector2 RandTarget = Vector2.zero;

    [SerializeField] private float spawnRadius = 10f;

    private void Awake()
    {
        mainCamera = FindObjectOfType<Camera>();
    }

    private void Start()
    {
        PlaceFishInGameBounds();
    }

    private void PlaceFishInGameBounds()
    {
        for (int i = 0; i < fishPrefabs.Length; i++)
        {
            RandTarget = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

            RandTarget = RandTarget.normalized;

            RandTarget *= spawnRadius;

            Vector2 targetWorld = mainCamera.transform.TransformPoint(RandTarget);
            fishPrefabs[i].transform.position = targetWorld;
            Instantiate(fishPrefabs[i]);
        }
    }
}