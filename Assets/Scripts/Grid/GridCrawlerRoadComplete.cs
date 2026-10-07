using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

class RoadPieceGCRC : IEquatable<RoadPieceGCRC>
{
    public Vector3Int position;
    public GridCrawler2.RoadType type;
    public int yRotation;
    public GameObject road;

    public bool Equals(RoadPieceGCRC other)
    {
        return (position == other.position && type == other.type && yRotation == other.yRotation ||
            position == other.position && type == GridCrawler2.RoadType.STRAIGHT && other.type == GridCrawler2.RoadType.STRAIGHT
            && Mathf.Abs(yRotation - other.yRotation) == 180);
    }
}



public class GridCrawler2 : MonoBehaviour
{
    [SerializeField] private GameObject crawler;
    [SerializeField] private GameObject[] roads; // 0 = Straight, 1 = Crossroad, 2 = Corner, 3 = Bridge, 4 = T-Junction, 5 = Deadend (Long), 6 = Deadend (1x1)

    public enum RoadType { STRAIGHT, CROSS, CORNER, TJUNC };
    List<RoadPieceGCRC> roadPieces = new List<RoadPieceGCRC>();

    int width = 100;
    int depth = 100;
    Vector3Int crawlerPos;
    Vector3 dir = new Vector3(0, 0, 1);
    Vector3 neutral = new Vector3(0, 0, 1);

    void Crawl()
    {
        int randomTurn = UnityEngine.Random.Range(0, 3);
        float rot;
        GameObject go;
        RoadPieceGCRC newRoad;

        if (randomTurn == 0)
        {
            dir = Quaternion.Euler(0, -90, 0) * dir;
            rot = Vector3.SignedAngle(neutral, dir, this.transform.up) + 90;
            go = Instantiate(roads[2], crawlerPos, Quaternion.identity);
            go.transform.Rotate(0, rot, 0);

            newRoad = new RoadPieceGCRC
            { position = crawlerPos, type = RoadType.CORNER,
                                        yRotation = (int)Mathf.Round(go.transform.rotation.eulerAngles.y / 90) * 90,
                                        road = go};

        }
        else if (randomTurn == 1)
        {
            dir = Quaternion.Euler(0, 90, 0) * dir;
            rot = Vector3.SignedAngle(neutral, dir, this.transform.up) + 180;
            go = Instantiate(roads[2], crawlerPos, Quaternion.identity);
            go.transform.Rotate(0, rot, 0);

            newRoad = new RoadPieceGCRC
            {
                position = crawlerPos,
                type = RoadType.CORNER,
                yRotation = (int)Mathf.Round(go.transform.rotation.eulerAngles.y / 90) * 90,
                road = go
            };
        }
        else
        {
            rot = Vector3.SignedAngle(neutral, dir, this.transform.up);
            go = Instantiate(roads[0], crawlerPos, Quaternion.identity);
            go.transform.Rotate(0, rot, 0);

            newRoad = new RoadPieceGCRC
            {
                position = crawlerPos,
                type = RoadType.STRAIGHT,
                yRotation = (int)Mathf.Round(go.transform.rotation.eulerAngles.y / 90) * 90,
                road = go
            };
        }

        AddNoDuplications(newRoad);

        Vector3Int straightPos = crawlerPos + Vector3Int.RoundToInt(dir * 10);
        rot = Vector3.SignedAngle(neutral, dir, this.transform.up);
        go = Instantiate(roads[0], straightPos, Quaternion.identity);
        go.transform.Rotate(0, rot, 0);

        newRoad = new RoadPieceGCRC
        {
            position = straightPos,
            type = RoadType.STRAIGHT,
            yRotation = (int)Mathf.Round(go.transform.rotation.eulerAngles.y / 90) * 90,
            road = go
        };

        AddNoDuplications(newRoad);

        crawlerPos += Vector3Int.RoundToInt(dir * 20);

        crawler.transform.position = crawlerPos;
    }

    void AddNoDuplications(RoadPieceGCRC newPiece)
    {
        bool found = false;
        foreach (RoadPieceGCRC r in roadPieces)
        {
            if (r.Equals(newPiece))
            {
                found = true;
                break;
            }
        }
        if (!found)
            roadPieces.Add(newPiece);
    }

    void FixRoads()
    {
        Lookup<Vector3Int, RoadPieceGCRC> lookup = (Lookup<Vector3Int, RoadPieceGCRC>)roadPieces.ToLookup(p => p.position, p => p);

        foreach (IGrouping<Vector3Int, RoadPieceGCRC> roadGroup in lookup)
        {
            if (roadGroup.Count() > 1)
            {
                bool hasCorner0 = false;
                bool hasCorner90 = false;
                bool hasCorner180 = false;
                bool hasCorner270 = false;
                bool hasStraight0 = false;
                bool hasStraight90 = false;
                bool hasStraight180 = false;
                bool hasStraight270 = false;

                foreach(RoadPieceGCRC r in roadGroup)
                {
                    if (r.yRotation == 0 && r.type == RoadType.CORNER) hasCorner0 = true;
                    if (r.yRotation == 90 && r.type == RoadType.CORNER) hasCorner90 = true;
                    if (r.yRotation == 180 && r.type == RoadType.CORNER) hasCorner180 = true;
                    if (r.yRotation == 270 && r.type == RoadType.CORNER) hasCorner270 = true;
                    if (r.yRotation == 0 && r.type == RoadType.STRAIGHT) hasStraight0 = true;
                    if (r.yRotation == 90 && r.type == RoadType.STRAIGHT) hasStraight90 = true;
                    if (r.yRotation == 180 && r.type == RoadType.STRAIGHT) hasStraight180 = true;
                    if (r.yRotation == 270 && r.type == RoadType.STRAIGHT) hasStraight270 = true;
                    DestroyImmediate(r.road);
                }

                if (hasStraight0 && hasStraight90 ||
                    hasStraight90 && hasStraight180 ||
                    hasStraight180 && hasStraight270 ||
                    hasStraight270 && hasStraight0 ||
                    hasCorner0 && hasCorner180 ||
                    hasCorner90 && hasCorner270)
                    Instantiate(roads[1], roadGroup.Key, Quaternion.identity);
                else if (hasCorner0 && hasCorner90 ||
                    hasCorner0 && hasStraight0 ||
                    hasCorner0 && hasStraight180 ||
                    hasCorner90 && hasStraight0 ||
                    hasCorner90 && hasStraight180)
                    Instantiate(roads[4], roadGroup.Key, Quaternion.identity);
                else if (hasCorner0 && hasCorner270 ||
                    hasCorner0 && hasStraight90 ||
                    hasCorner0 && hasStraight270 ||
                    hasCorner270 && hasStraight90 ||
                    hasCorner270 && hasStraight270)
                {
                    GameObject go = Instantiate(roads[4], roadGroup.Key, Quaternion.identity);
                    go.transform.Rotate(0, -90, 0);
                }
                else if (hasCorner90 && hasCorner180 ||
                    hasCorner90 && hasStraight90 ||
                    hasCorner90 && hasStraight270 ||
                    hasCorner180 && hasStraight90 ||
                    hasCorner180 && hasStraight270)
                {
                    GameObject go = Instantiate(roads[4], roadGroup.Key, Quaternion.identity);
                    go.transform.Rotate(0, 90, 0);
                }
                else if (hasCorner180 && hasCorner270 ||
                    hasCorner180 && hasStraight0 ||
                    hasCorner180 && hasStraight180 ||
                    hasCorner270 && hasStraight0 ||
                    hasCorner270 && hasStraight180)
                {
                    GameObject go = Instantiate(roads[4], roadGroup.Key, Quaternion.identity);
                    go.transform.Rotate(0, 180, 0);
                }
            }
        }
    }

    int counter = 0;
    bool done = false;
    // Update is called once per frame
    void Update()
    {
        if (done) return;
        if (counter < 100)
        {
            Crawl();
            counter++;
        }
        else
        {
            FixRoads();
            done = true;
        }
    }
}
