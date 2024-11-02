using System;
using UnityEngine;

namespace ET.Client
{
    [RequireComponent(typeof (MeshRenderer), typeof (MeshFilter))]
    public class CircleMesh: MonoBehaviour
    {
        [SerializeField]
        private MeshFilter meshFilter;

        [SerializeField]
        private float radius = 5;

        [SerializeField]
        private float innerRadius = 4;

        [Range(15, 360), SerializeField]
        private float angleDegree = 360;

        [Range(30, 360), SerializeField]
        private int segments = 60;

        private Mesh meshGen;

        private void OnValidate()
        {
            meshFilter = GetComponent<MeshFilter>();
        }

        private void Awake()
        {
            GenMesh();
        }

        private void OnDestroy()
        {
            Destroy(this.meshGen);
        }

        [ContextMenu("CreateMesh")]
        private void GenMesh()
        {
            if (this.meshGen == null)
            {
                this.meshGen = new Mesh();
                meshFilter.mesh = this.meshGen;
            }

            GenerateMesh(this.meshGen, this.radius, this.innerRadius, this.angleDegree, this.segments);
        }

        public void GenCircle(float r, float innerR)
        {
            if (this.meshGen != null && Math.Abs(this.radius - r) < 0.001f && Math.Abs(this.innerRadius - innerR) < 0.001f &&
                Math.Abs(this.angleDegree - 360) < 0.001f)
            {
                return;
            }

            this.radius = r;
            this.innerRadius = innerR;
            this.angleDegree = 360;
            GenMesh();
        }

        public void GenSector(float r, float innerR, float angleDeg)
        {
            if (this.meshGen != null && Math.Abs(this.radius - r) < 0.001f && Math.Abs(this.innerRadius - innerR) < 0.001f &&
                Math.Abs(this.angleDegree - angleDeg) < 0.001f)
            {
                return;
            }

            this.radius = r;
            this.innerRadius = innerR;
            this.angleDegree = angleDeg;
            this.GenMesh();
        }

        private static void GenerateMesh(Mesh mesh, float radius, float innerradius, float angledegree, int segments)
        {
            //因为vertices(顶点)的个数与triangles（索引三角形顶点数）必须匹配
            int vertices_count = segments * 2 + 2;
            int triangle_count = segments * 6;
            Vector3[] vertices = null;
            Vector2[] uvs = null;
            int[] triangles = new int[triangle_count];
            if (vertices_count != mesh.vertexCount)
            {
                vertices = new Vector3[vertices_count];
                uvs = new Vector2[vertices_count];
                triangles = new int[triangle_count];
            }
            else
            {
                vertices = mesh.vertices;
                uvs = mesh.uv;
                triangles = mesh.triangles;
            }

            //vertices(顶点):
            float angleRad = Mathf.Deg2Rad * angledegree;
            float angleCur = angleRad;
            float angledelta = angleRad / segments;
            for (int i = 0; i < vertices_count; i += 2)
            {
                float cosA = Mathf.Cos(angleCur);
                float sinA = Mathf.Sin(angleCur);

                vertices[i] = new Vector3(radius * cosA, 0, radius * sinA);
                vertices[i + 1] = new Vector3(innerradius * cosA, 0, innerradius * sinA);
                angleCur -= angledelta;
            }

            //triangles:
            for (int i = 0, vi = 0; i < triangle_count; i += 6, vi += 2)
            {
                triangles[i] = vi;
                triangles[i + 1] = vi + 3;
                triangles[i + 2] = vi + 1;
                triangles[i + 3] = vi + 2;
                triangles[i + 4] = vi + 3;
                triangles[i + 5] = vi;
            }

            //uv:
            //for (int i = 0; i < vertices_count; i++) {
            //    uvs[i] = new Vector2(vertices[i].x / radius / 2 + 0.5f, vertices[i].z / radius / 2 + 0.5f);
            //}
            for (int i = 0; i < vertices_count / 2; i++)
            {
                float p = i / (float)segments;
                uvs[i * 2] = new Vector2(p, 1);
                uvs[i * 2 + 1] = new Vector2(p, 0);
            }

            // 设置数据
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
        }
    }
}