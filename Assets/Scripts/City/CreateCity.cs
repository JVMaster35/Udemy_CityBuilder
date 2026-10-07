using UnityEngine;
using UnityEngine.Rendering;

public class CreateCity : MonoBehaviour
{
    private int cityWidth = 300;
    private int cityDepth = 300;

    [SerializeField]
    private Material[] buildingMats; //0 - Residential, 1 = Commerical, 2 = Industrial, 3 = Pleasure

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MeshUtils.GenerateVoronoi(20, cityWidth, cityDepth);

        for (int z = 0; z < cityDepth; z++)
        {
            for (int x = 0; x < cityWidth; x++)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.transform.position = new Vector3(x, 0, z);

                Renderer rend = go.GetComponent<Renderer>();
                if (MeshUtils.voronoiMap[x,z] < 10) rend.material = buildingMats[0];
                else if (MeshUtils.voronoiMap[x, z] < 14) rend.material = buildingMats[1];
                else if (MeshUtils.voronoiMap[x, z] < 18) rend.material = buildingMats[2];
                else rend.material = buildingMats[3];

                float perlin = MeshUtils.fBM(x * 0.005f, z * 0.005f, 5);

                int h = 1;

                if (perlin < 0.354f) h = 1;
                else if (perlin < 0.417f) h = 2;
                else if (perlin < 0.509f) h = 3;
                else if (perlin < 0.623f) h = 5;
                else  h = 10;

                go.transform.localScale = new Vector3(1, h, 1);
                go.transform.Translate(0, h/2.0f, 0);
            }
        }
    }
}
