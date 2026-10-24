using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Minigame : MonoBehaviour
{
    [SerializeField] private Renderer planeRenderer;
    [SerializeField] private GameObject CubePrefab, CirclePrefab;
    [SerializeField] private float heightOffset = 0.5f;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float objLifetime = 10f;
    [SerializeField] private float hideDelayAfterClick = 0.3f;
    [SerializeField] private string cubeTag, sphereTag;

    private float spawnTimer = 0f;
    private int i = 0;

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0;
            SpawnObj();
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                string clickedTag = hit.collider.tag;
                if (clickedTag == cubeTag || clickedTag == sphereTag)
                    StartCoroutine(HideAllWithTag(clickedTag));
            }
        }
    }

    IEnumerator HideAllWithTag(string tag)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject e in objs)
        {
            e.GetComponent<MeshRenderer>().material.color = Color.red;
        }

        yield return new WaitForSeconds(hideDelayAfterClick);

        foreach (GameObject e in objs)
        {
            if (e != null)
                e.GetComponent<MeshRenderer>().enabled = false;
        }
    }


    public void SpawnObj()
    {
        Vector3 spawnPos = GetRandomPointInBounds(planeRenderer.bounds);
        spawnPos.y += heightOffset;

        GameObject prefab = (i % 2 == 0) ? CubePrefab : CirclePrefab;
        i++;
        GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);
        Destroy(obj, objLifetime);

    }

    public static Vector3 GetRandomPointInBounds(Bounds b)
    {
        return new Vector3(
            Random.Range(b.min.x, b.max.x),
            b.max.y,
            Random.Range(b.min.z, b.max.z));
    }
}