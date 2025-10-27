using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour {
    public GameObject[] tilePrefabs;
    private List<GameObject> activeTiles = new List<GameObject>();
    private float spawnPos = -35;
    private float tileLength = 60;


    [SerializeField] private Transform player;
    private int startTiles = 5;

    // Start is called before the first frame update
    void Start() {
        for (int i = 0; i < startTiles; i++) {
            if (i == 0)
                SpawnTile(2);
            SpawnTile(Random.Range(0, tilePrefabs.Length));
        }
    }

    // Update is called once per frame
    void Update() {
        if (player.position.z - 100 > spawnPos - (startTiles * tileLength)) {
            SpawnTile(Random.Range(0, tilePrefabs.Length));
            DeleteTile();
        }
    }

    private void SpawnTile(int tileIndex) {
        GameObject nextTile = Instantiate(tilePrefabs[tileIndex], new Vector3(0, 0, spawnPos), Quaternion.identity);
        activeTiles.Add(nextTile);
        spawnPos += tileLength;
    }
    private void DeleteTile() {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}