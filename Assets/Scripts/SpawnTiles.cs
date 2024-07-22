using UnityEngine;

public class SpawnTiles : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    void Start()
    {
        GameManager.Instance.onStart.AddListener(SpawnMapTiles);
    }

    private void SpawnMapTiles() {
        int row = 10;
        int col = 20;
        
        for (int i = 0; i < row; i++) {
            for (int j = 0; j < column; j++) {
                
            }
        }
    }
}
