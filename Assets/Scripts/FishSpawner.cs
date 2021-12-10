using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] fishPrefabs;

    private Camera mainCamera;

    private GameObject gameBound;

    private Vector3 RandTarget = Vector3.zero;

    [SerializeField] private float spawnRadius = 10f;

    private void Awake()
    {
        mainCamera = FindObjectOfType<Camera>();
    }

    private void Start()
    {
        PlaceFishInGameBounds();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(mainCamera.transform.position, spawnRadius);
    }

    private void PlaceFishInGameBounds()
    {
        for (int i = 0; i < fishPrefabs.Length; i++)
        {
            RandTarget = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

            RandTarget = RandTarget.normalized;

            RandTarget *= spawnRadius;

            Vector3 targetWorld = mainCamera.transform.TransformPoint(RandTarget);

            fishPrefabs[i].transform.position = targetWorld;
            Instantiate(fishPrefabs[i]);
        }
    }
}