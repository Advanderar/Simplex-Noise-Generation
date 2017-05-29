using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {

    private int   width = 15;
    private int  length = 15;
    private int  heigth = 255;
    private float scale = 0.01f;
    private float[,,] chunk;
    private float thresh = 80;
    public bool filled = false;
    public int numverts = 0;
    public List<Vector3> verts;
    private List<int> tris;
    GameObject model;
    List<GameObject> faces;

	public void GenChunk(int x , int z)
    {
        model = new GameObject();
        verts = new List<Vector3>();
        tris = new List<int>();
        
        x = x * 15;
        z = z * 15;

        model.transform.position = new Vector3(x, 0, z);

        chunk = new float[width + 2, heigth, length + 2];

        for(var i = -1; i < width + 1; i++)
        {
            for(var k = -1; k < length; k++)
            {
                float ground = Noise.GenerateNoise(x + i,z + k, 1);
                chunk[i + 1,(int) ground, k + 1] = 255;

                for(var j = 0; j < (int) ground; j++)
                {
                    genType(x, z, i, j, k);
                }
            }
        }

        for (var posZ = 1; posZ < chunk.GetLength(0); posZ++)
        {
            for (var posY = 0; posY < chunk.GetLength(1); posY++)
            {
                for (var posX = 1; posX < chunk.GetLength(2); posX++)
                {
                    GenBlock(posX, posY, posZ, x, z);
                }
            }
        }
        filled = true;
        fuseMeshes();
    }

    private void fuseMeshes()
    {
        MeshFilter mesh = model.AddComponent<MeshFilter>();
        Renderer rend = model.AddComponent<MeshRenderer>();
        MeshCollider collider = model.AddComponent<MeshCollider>();

        model.GetComponent<MeshFilter>().mesh = new Mesh();
        mesh.mesh.vertices = verts.ToArray();
        mesh.mesh.triangles = tris.ToArray();

        model.gameObject.SetActive(true);
        rend.material.color = Color.grey;
        mesh.mesh.RecalculateNormals();
        collider.sharedMesh = mesh.sharedMesh;
    }

    public void DelChunk()
    {
        model.gameObject.SetActive(false);
        filled = false;
    }

    private void GenBlock(int posX, int posY, int posZ, int x, int z)
    {

        if (chunk[posX, posY, posZ] >= thresh)
        {
            if (posX > 0 && chunk[posX - 1, posY, posZ] < thresh)
            {
                verts.Add(new Vector3(-0.5f + posX, -0.5f + posY, -0.5f + posZ));
                verts.Add(new Vector3(-0.5f + posX, -0.5f + posY, 0.5f + posZ));
                verts.Add(new Vector3(-0.5f + posX, 0.5f + posY, -0.5f + posZ));
                verts.Add(new Vector3(-0.5f + posX, 0.5f + posY, 0.5f + posZ));

                numverts += 4;

                tris.AddRange(new int[6]{ numverts - 2, numverts - 4, numverts - 3, numverts - 2, numverts - 3, numverts - 1 });

            }
            if (posX <= 15 && chunk[posX + 1, posY, posZ] < thresh)
            {
                Vector3[] vertices = new Vector3[4];
                vertices[0] = new Vector3(0.5f + posX, -0.5f + posY, -0.5f + posZ);
                vertices[1] = new Vector3(0.5f + posX, -0.5f + posY, 0.5f + posZ);
                vertices[2] = new Vector3(0.5f + posX, 0.5f + posY, -0.5f + posZ);
                vertices[3] = new Vector3(0.5f + posX, 0.5f + posY, 0.5f + posZ);

                verts.AddRange(vertices);

                numverts += 4;

                tris.AddRange(new int[6] { numverts - 2, numverts - 3, numverts - 4, numverts - 2, numverts - 1, numverts - 3 });

            }
            if (posY > 0 && chunk[posX, posY - 1, posZ] < thresh)
            {
                Vector3[] vertices = new Vector3[4];
                vertices[0] = new Vector3(0.5f + posX, -0.5f + posY, -0.5f + posZ);
                vertices[1] = new Vector3(0.5f + posX, -0.5f + posY, 0.5f + posZ);
                vertices[2] = new Vector3(-0.5f + posX, -0.5f + posY, -0.5f + posZ);
                vertices[3] = new Vector3(-0.5f + posX, -0.5f + posY, 0.5f + posZ);

                verts.AddRange(vertices);

                numverts += 4;

                tris.AddRange(new int[6] { numverts - 2, numverts - 4, numverts - 3, numverts - 2, numverts - 3, numverts - 1 });

            }
            if (posY < 254 && chunk[posX, posY + 1, posZ] < thresh)
            {
                Vector3[] vertices = new Vector3[4];
                vertices = new Vector3[4];
                vertices[0] = new Vector3(0.5f + posX, 0.5f + posY, -0.5f + posZ);
                vertices[1] = new Vector3(0.5f + posX, 0.5f + posY, 0.5f + posZ);
                vertices[2] = new Vector3(-0.5f + posX, 0.5f + posY, -0.5f + posZ);
                vertices[3] = new Vector3(-0.5f + posX, 0.5f + posY, 0.5f + posZ);

                verts.AddRange(vertices);

                numverts += 4;

                tris.AddRange(new int[6] { numverts - 2, numverts - 3, numverts - 4, numverts - 2, numverts - 1, numverts - 3 });

            }
            if (posZ > 0 && chunk[posX, posY, posZ - 1] < thresh)
            {
                Vector3[] vertices = new Vector3[4];
                vertices = new Vector3[4];
                vertices[0] = new Vector3( 0.5f + posX, -0.5f + posY, -0.5f + posZ);
                vertices[1] = new Vector3(-0.5f + posX, -0.5f + posY, -0.5f + posZ);
                vertices[2] = new Vector3( 0.5f + posX, 0.5f + posY, -0.5f + posZ);
                vertices[3] = new Vector3(-0.5f + posX, 0.5f + posY, -0.5f + posZ);

                verts.AddRange(vertices);

                numverts += 4;

                tris.AddRange(new int[6] { numverts - 2, numverts - 4, numverts - 3, numverts - 2, numverts - 3, numverts - 1 });

            }
            if (posZ <= 15 && chunk[posX, posY, posZ + 1] < thresh)
            {
                Vector3[] vertices = new Vector3[4];
                vertices = new Vector3[4];
                vertices[0] = new Vector3(0.5f + posX, -0.5f + posY, 0.5f + posZ);
                vertices[1] = new Vector3(-0.5f + posX, -0.5f + posY, 0.5f + posZ);
                vertices[2] = new Vector3(0.5f + posX, 0.5f + posY, 0.5f + posZ);
                vertices[3] = new Vector3(-0.5f + posX, 0.5f + posY, 0.5f + posZ);

                verts.AddRange(vertices);

                numverts += 4;

                tris.AddRange(new int[6] { numverts - 2, numverts - 3, numverts - 4, numverts - 2, numverts - 1, numverts - 3 });

            }
            if (posY == 254)
            {
                Vector3[] vertices = new Vector3[4];
                vertices = new Vector3[4];
                vertices[0] = new Vector3(0.5f + posX, 0.5f + posY, -0.5f + posZ);
                vertices[1] = new Vector3(0.5f + posX, 0.5f + posY, 0.5f + posZ);
                vertices[2] = new Vector3(-0.5f + posX, 0.5f + posY, -0.5f + posZ);
                vertices[3] = new Vector3(-0.5f + posX, 0.5f + posY, 0.5f + posZ);

                verts.AddRange(vertices);

                numverts += 4;

                tris.AddRange(new int[6] { numverts - 2, numverts - 3, numverts - 4, numverts - 2, numverts - 1, numverts - 3 });

            }
        }
    }

    private float genNoise(int x, int z, int posX, int posY, int posZ)
    {
        double total = 0;
        int frequency = 1;
        double amplitude = 1;
        double maxValue = 0;
        double persistence = 4;
        double octaves = 2;
        for(int i = 0; i< octaves; i++)
        {
            total += Simplex.Noise.CalcPixel3D((x + posX) * frequency,  (posY * frequency), (z + posZ) * frequency, scale) / amplitude;
            maxValue += amplitude;

            amplitude *= persistence;
            frequency *= 2;
        }
        return (float) (total/maxValue);

    }

    private void genType(int x, int z, int i, int j, int k)
    {
        if(j < 3)
        {
            chunk[i + 1, j, k + 1] = 255;
        }
        else
        {
            chunk[i + 1, j, k + 1] = genNoise(x, z, i, j, k);
            if(chunk[i + 1, j, k + 1] > 100)
            {
                Debug.Log(chunk[i + 1, j, k + 1]);
            }
        }
    }
}
