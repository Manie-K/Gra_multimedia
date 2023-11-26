using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedPlatforms : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;
    const int PLATFORMS_NUM = 2;
    GameObject[] platforms;
    Vector3[] positions;
    Vector3 center;
    Vector3 destination;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float radius = 20.0f;
    private float angle = 30f;

    void Update()
    {
        angle += moveSpeed * Time.deltaTime;

        for (int i = 0; i < PLATFORMS_NUM; i++)
        {
            float x = center.x + Mathf.Cos(angle + Mathf.PI * i) * radius;
            float y = center.y + Mathf.Sin(angle + Mathf.PI * i) * radius;
            float z = center.z;

            platforms[i].transform.position = new Vector3(x, y, z);
        }
    }

    void Awake()
    {
        platforms = new GameObject[PLATFORMS_NUM];
        positions = new Vector3[PLATFORMS_NUM];

        float x = this.transform.position.x;
        float y = this.transform.position.y;
        float z = this.transform.position.z;

        center = new Vector3(x, y, z);

        for (int i = 0; i < PLATFORMS_NUM; i++)
        {
            x = center.x + Mathf.Cos(Mathf.PI * i) * radius;
            y = center.y + Mathf.Sin(Mathf.PI * i) * radius;
            z = center.z;

            positions[i] = new Vector3(x, y, z);

            platforms[i] = Instantiate(platformPrefab, positions[i], Quaternion.identity);
        }
    }
}
