using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSpawner : MonoBehaviour
{
    // Array that holds all shapes
    public GameObject[] shapes;

    // Array that holds all display shape models
    public GameObject[] displays;

    GameObject upNextObject = null;

    int shapeIndex = 0;
    int nextShapeIndex = 0;

    public void SpawnShape()
    {
        shapeIndex = nextShapeIndex;


        Instantiate(shapes[shapeIndex],
                    transform.position,
                     Quaternion.identity);

        nextShapeIndex = Random.Range(0, 6);

        Vector3 nextShapePos = new Vector3(-8.5f, 17f);

        if (upNextObject != null)
        {
            Destroy(upNextObject);
        }

        upNextObject = Instantiate(displays[nextShapeIndex],
                                            nextShapePos,
                                            Quaternion.identity);
    }

    // Use this for initialization
    void Start()
    {
        nextShapeIndex = Random.Range(0, 6);

        SpawnShape();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
