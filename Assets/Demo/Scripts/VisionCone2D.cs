using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VisionCone2D : MonoBehaviour
{
    [Range(0f, 360f)]
    public float viewAngle = 90f;
    public float viewRadius = 5f;
    public int resolution = 50; // Number of segments in the cone

    void Start()
    {
        GenerateMesh();
    }

    void Update()
    {
        GenerateMesh(); // Update every frame if rotating/moving
    }

    void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "VisionCone2D";

        Vector3[] vertices = new Vector3[resolution + 2];
        int[] triangles = new int[resolution * 3];

        vertices[0] = Vector3.zero; // Cone origin (center)

        float angleStep = viewAngle / resolution;
        float halfAngle = viewAngle / 2f;

        for (int i = 0; i <= resolution; i++)
        {
            float angle = -halfAngle + angleStep * i;
            float rad = Mathf.Deg2Rad * angle;
            Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0); // XY plane
            vertices[i + 1] = dir * viewRadius;
        }

        for (int i = 0; i < resolution; i++)
        {
            int triIndex = i * 3;
            triangles[triIndex] = 0;
            triangles[triIndex + 1] = i + 1;
            triangles[triIndex + 2] = i + 2;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}