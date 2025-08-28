using UnityEngine;

public class ItemSpwaner : MonoBehaviour
{
    //public GameObject AmmoPrefab;
    //public GameObject HealPrefab;
    //public GameObject CoinPrefab;

    public GameObject[] itemPrefabs;

    public Vector3 spawnMin = new Vector3(-3, 0, -3);
    public Vector3 spawnMax = new Vector3(3, 0, 3);

    public float interval = 5f;
    private float timer;

    private void Awake()
    {
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        //if (timer >= interval)
        //{
        //    Vector3 spawnPos1 = new Vector3(
        //        Random.Range(spawnMin.x, spawnMax.x),
        //        Random.Range(spawnMin.y, spawnMax.y),
        //        Random.Range(spawnMin.z, spawnMax.z)
        //    );
        //    Vector3 spawnPos2 = new Vector3(
        //        Random.Range(spawnMin.x, spawnMax.x),
        //        Random.Range(spawnMin.y, spawnMax.y),
        //        Random.Range(spawnMin.z, spawnMax.z)
        //    );
        //    Vector3 spawnPos3 = new Vector3(
        //        Random.Range(spawnMin.x, spawnMax.x),
        //        Random.Range(spawnMin.y, spawnMax.y),
        //        Random.Range(spawnMin.z, spawnMax.z)
        //    );
        //    Instantiate(AmmoPrefab, spawnPos1, Quaternion.identity);
        //    Instantiate(HealPrefab, spawnPos2, Quaternion.identity);
        //    Instantiate(CoinPrefab, spawnPos3, Quaternion.identity);

        if(timer > interval)
        {
            //foreach (var itemPrefab in itemPrefabs)
            //{
            //    Vector3 spawnPos = new Vector3(
            //        Random.Range(spawnMin.x, spawnMax.x),
            //        Random.Range(spawnMin.y, spawnMax.y),
            //        Random.Range(spawnMin.z, spawnMax.z)
            //    );
            //    Instantiate(itemPrefab, spawnPos, Quaternion.identity);
            //}
            int randomIndex = Random.Range(0, itemPrefabs.Length);
            Vector3 spawnPos = new Vector3(
                Random.Range(spawnMin.x, spawnMax.x),
                Random.Range(spawnMin.y, spawnMax.y),
                Random.Range(spawnMin.z, spawnMax.z)
            );
            Instantiate(itemPrefabs[randomIndex], spawnPos, Quaternion.identity);
            timer = 0f;
        }
    }
}
