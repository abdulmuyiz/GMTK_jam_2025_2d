using System.Collections.Generic;
using UnityEngine;

public class AutoGeneration : MonoBehaviour
{
    public float radius = 10;
    public Vector2 regionSize = Vector2.one;
    public int rejectionSamples = 30;
    public float displayRadius = 1;
    public int maxCount;
    private int count = 0;
    [SerializeField] GameObject chest;

    List<Vector2> points;

    void OnValidate()
    {
        points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(regionSize / 2, regionSize);
        if (points != null)
        {
            foreach (Vector2 point in points)
            {
                Gizmos.DrawSphere(point, displayRadius);
            }
        }

    }

    private void FixedUpdate()
    {
        if (points != null)
        {
            while (count <= maxCount)
            {
                count++;
                Vector2 selPoint = points[Random.Range(0, points.Count)];
                Instantiate(chest, selPoint, Quaternion.identity);
            }
        }
    }

    private void OnApplicationQuit()
    {
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Chest");
        foreach (GameObject obj in allObjects)
        {
            Destroy(obj);
        }
    }
}
