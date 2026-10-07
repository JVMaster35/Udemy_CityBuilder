using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] roads; // 0 = Straight, 1 = Crossroad, 2 = Corner, 3 = Bridge, 4 = T-Junction, 5 = Deadend (Long), 6 = Deadend (1x1)
    [SerializeField] private int width = 100;
    [SerializeField] private int depth = 100;

    void Start()
    {
        Destroy(this);
        CreateGrid();
    }

    void CreateGrid()
    {
        for (int z = 0; z < depth; z += 20)
        {
            for (int x = 0; x < width; x += 20)
            {
                Vector3 position = new Vector3(x, 0, z);
                GameObject r;
                if (Random.Range(0, 100) < 20)
                {
                    r = Instantiate(roads[2], position, Quaternion.identity);
                }
                else
                {
                    r = Instantiate(roads[1], position, Quaternion.identity);
                }

                position.x = x + 10;
                position.z = z; 
                if (Random.Range(0, 100) < 20)
                {
                    r = Instantiate(roads[5], position, Quaternion.identity);
                    r.transform.Rotate(0, 90, 0);
                }
                else
                {
                    r = Instantiate(roads[0], position, Quaternion.identity);
                }

                position.x = x;
                position.z = z + 10;
                if (Random.Range(0, 100) < 20)
                {
                    r = Instantiate(roads[5], position, Quaternion.identity);
                }
                else
                {
                    r = Instantiate(roads[0], position, Quaternion.identity);
                    r.transform.Rotate(0, 90, 0);
                }
            }
        }
    }
}
