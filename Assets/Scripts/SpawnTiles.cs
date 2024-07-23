using UnityEngine;

public class SpawnTiles : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Transform camera;

    void Start()
    {
        GameManager.Instance.onStart.AddListener(SpawnMapTiles);
    }

    private void SpawnMapTiles() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
            }
        }
    }
}
