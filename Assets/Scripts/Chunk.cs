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
    GameObject model;
    List<MeshFilter> meshFilters = new List<MeshFilter>();
    List<GameObject> faces;

	public void GenChunk(int x , int z)
    {
        model = new GameObject();

        faces = new List<GameObject>();
        
        x = x * 15;
        z = z * 15;

        chunk = new float[width + 2, heigth, length + 2];

        for(var i = -1; i < width + 1; i++)
        {
            for(var j = 0; j < heigth; j++)
            {
                for(var k = -1; k < length + 1; k++)
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
        CombineInstance[] combine = new CombineInstance[meshFilters.Count];
        int i = 0;
        while (i < meshFilters.Count)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }
        model.GetComponent<MeshFilter>().mesh = new Mesh();
        model.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        model.gameObject.SetActive(true);
        rend.material.color = Color.grey;
        mesh.mesh.RecalculateNormals();
        collider.sharedMesh = mesh.sharedMesh;
        DelFaces();
    }

    public void DelFaces()
    {
        foreach(MeshFilter item in meshFilters)
        {
            Destroy(item.transform.gameObject);
        }
        faces.Clear();
        //filled = false;
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
                GameObject face = GameObject.CreatePrimitive(PrimitiveType.Quad);
                face.transform.position = new Vector3(posX - .5f + x, posY, posZ + z);
                face.transform.rotation = Quaternion.Euler(0, 90, 0);
                face.gameObject.SetActive(false);
                meshFilters.Add(face.GetComponent<MeshFilter>());
            }
            if (posX <= 15 && chunk[posX + 1, posY, posZ] < thresh)
            {
                GameObject face = GameObject.CreatePrimitive(PrimitiveType.Quad);
                face.transform.position = new Vector3(posX + .5f + x, posY, posZ + z);
                face.transform.rotation = Quaternion.Euler(0, -90, 0);
                face.gameObject.SetActive(false);
                meshFilters.Add(face.GetComponent<MeshFilter>());
            }
            if (posY > 0 && chunk[posX, posY - 1, posZ] < thresh)
            {
                GameObject face = GameObject.CreatePrimitive(PrimitiveType.Quad);
                face.transform.position = new Vector3(posX + x, posY - .5f, posZ + z);
                face.transform.rotation = Quaternion.Euler(-90, 0, 0);
                face.gameObject.SetActive(false);
                meshFilters.Add(face.GetComponent<MeshFilter>());
            }
            if (posY < 254 && chunk[posX, posY + 1, posZ] < thresh)
            {
                GameObject face = GameObject.CreatePrimitive(PrimitiveType.Quad);
                face.transform.position = new Vector3(posX + x, posY + .5f, posZ + z);
                face.transform.rotation = Quaternion.Euler(90, 0, 0);
                face.gameObject.SetActive(false);
                meshFilters.Add(face.GetComponent<MeshFilter>());
            }
            if (posZ > 0 && chunk[posX, posY, posZ - 1] < thresh)
            {
                GameObject face = GameObject.CreatePrimitive(PrimitiveType.Quad);
                face.transform.position = new Vector3(posX + x, posY, posZ - .5f + z);
                face.gameObject.SetActive(false);
                meshFilters.Add(face.GetComponent<MeshFilter>());
            }
            if (posZ <= 15 && chunk[posX, posY, posZ + 1] < thresh)
            {
                GameObject face = GameObject.CreatePrimitive(PrimitiveType.Quad);
                face.transform.position = new Vector3(posX + x, posY, posZ + .5f + z);
                face.transform.rotation = Quaternion.Euler(0, 180, 0);
                face.gameObject.SetActive(false);
                meshFilters.Add(face.GetComponent<MeshFilter>());
            }
            if (posY == 254)
            {
                GameObject face = GameObject.CreatePrimitive(PrimitiveType.Quad);
                face.transform.position = new Vector3(posX + x, posY + .5f, posZ + z);
                face.transform.rotation = Quaternion.Euler(90, 0, 0);
                face.gameObject.SetActive(false);
                meshFilters.Add(face.GetComponent<MeshFilter>());
            }
        }
    }

    private float genNoise(int x, int z, int posX, int posY, int posZ)
    {

        double total = 0;
        int frequency = 1;
        double amplitude = 3;
        double maxValue = 0;
        double persistence = 4;
        double octaves = 1;
        for(int i = 0; i< octaves; i++)
        {
            total += Simplex.Noise.CalcPixel3D((x + posX) * frequency,  (posY * frequency), (z + posZ) * frequency, scale);
            maxValue += amplitude;

            amplitude *= persistence;
            frequency *= 2;
        }
        return (float) (total/maxValue);
    }

    private void genType(int x, int z, int i, int j, int k)
    {
        float noise = genNoise(x, z, i, j, k);
        i += 1;
        k += 1;
        if (j < 1)
        {
            chunk[i, j, k] = 255;
        }
        if (j < 3)
        {
            chunk[i, j, k] = Random.Range(0, 160);
        }
        else if (noise < (30 - j / 4))
        {
            chunk[i, j, k] = noise + 50;
        }
        else
        {
            chunk[i, j, k] = (heigth - (j)) - noise;
        }
    }
}
