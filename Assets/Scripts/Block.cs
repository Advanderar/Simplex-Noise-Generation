using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    private float thresh = 128;
    private float scale = 0.05f;

    public List<GameObject> CreateBlock(float[,,] chunk, int posX, int posY, int posZ, int x, int z)
    {
        List<GameObject> faces = new List<GameObject>();
        if (chunk[posX, posY, posZ] >= thresh)
        {
            if (posX == 0)
            {
                float value = Simplex.Noise.CalcPixel3D(posX + x - 1, posY, posZ + z, scale);
                if (value < thresh)
                {
                    GameObject face = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    face.transform.position = new Vector3(posX - .5f + x, posY, posZ + z);
                    face.transform.rotation = Quaternion.Euler(0, 90, 0);
                    faces.Add(face);
                }
            }
            if (posX == 14)
            {
                float value = Simplex.Noise.CalcPixel3D(posX + x + 1, posY, posZ + z, scale);
                if (value < thresh)
                {
                    GameObject face = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    face.transform.position = new Vector3(posX + .5f + x, posY, posZ + z);
                    face.transform.rotation = Quaternion.Euler(0, -90, 0);
                    faces.Add(face);
                }
            }
            if (posY == 0)
            {
                float value = Simplex.Noise.CalcPixel3D(posX + x, posY - 1, posZ + z, scale);
                if (value < thresh)
                {
                    GameObject face = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    face.transform.position = new Vector3(posX + x, posY - .5f, posZ + z);
                    face.transform.rotation = Quaternion.Euler(-90, 0, 0);
                    faces.Add(face);
                }
            }
            if (posZ == 0)
            {
                float value = Simplex.Noise.CalcPixel3D(posX + x, posY, posZ + z - 1, scale);
                if (value < thresh)
                {
                    GameObject face = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    face.transform.position = new Vector3(posX + x, posY, posZ - .5f + z);
                    faces.Add(face);
                }
            }
            if (posZ == 14)
            {
                float value = Simplex.Noise.CalcPixel3D(posX + x, posY, posZ + z + 1, scale);
                if (value < thresh)
                {
                    GameObject face = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    face.transform.position = new Vector3(posX + x, posY, posZ + .5f + z);
                    face.transform.rotation = Quaternion.Euler(0, 180, 0);
                    faces.Add(face);
                }
            }

            if (posX > 0 && chunk[posX - 1, posY, posZ] < thresh)
            {

                GameObject face = GameObject.CreatePrimitive(PrimitiveType.Quad);
                face.transform.position = new Vector3(posX - .5f + x, posY, posZ + z);
                face.transform.rotation = Quaternion.Euler(0, 90, 0);
                faces.Add(face);
            }
            if (posX < 14 && chunk[posX + 1, posY, posZ] < thresh)
            {
                GameObject face = GameObject.CreatePrimitive(PrimitiveType.Quad);
                face.transform.position = new Vector3(posX + .5f + x, posY, posZ + z);
                face.transform.rotation = Quaternion.Euler(0, -90, 0);
                faces.Add(face);
            }
            if (posY > 0 && chunk[posX, posY - 1, posZ] < thresh)
            {
                GameObject face = GameObject.CreatePrimitive(PrimitiveType.Quad);
                face.transform.position = new Vector3(posX + x, posY - .5f, posZ + z);
                face.transform.rotation = Quaternion.Euler(-90, 0, 0);
                faces.Add(face);
            }
            if (posY < 254 && chunk[posX, posY + 1, posZ] < thresh)
            {
                GameObject face = GameObject.CreatePrimitive(PrimitiveType.Quad);
                face.transform.position = new Vector3(posX + x, posY + .5f, posZ + z);
                face.transform.rotation = Quaternion.Euler(90, 0, 0);
                faces.Add(face);
            }
            if (posZ > 0 && chunk[posX, posY, posZ - 1] < thresh)
            {
                GameObject face = GameObject.CreatePrimitive(PrimitiveType.Quad);
                face.transform.position = new Vector3(posX + x, posY, posZ - .5f + z);
                faces.Add(face);
            }
            if (posZ < 14 && chunk[posX, posY, posZ + 1] < thresh)
            {
                GameObject face = GameObject.CreatePrimitive(PrimitiveType.Quad);
                face.transform.position = new Vector3(posX + x, posY, posZ + .5f + z);
                face.transform.rotation = Quaternion.Euler(0, 180, 0);
                faces.Add(face);
            }
            if (posY == 254)
            {
                GameObject face = GameObject.CreatePrimitive(PrimitiveType.Quad);
                face.transform.position = new Vector3(posX + x, posY + .5f, posZ + z);
                face.transform.rotation = Quaternion.Euler(90, 0, 0);
                faces.Add(face);
            }
        }
        return faces;
    }
	
}
