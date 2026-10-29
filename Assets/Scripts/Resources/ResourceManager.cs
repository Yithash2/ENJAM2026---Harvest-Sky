using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public int Resources;
    [SerializeField] private GameObject[] _spawnPoints;
    private List<int> _takenPoints = new List<int>();
    [SerializeField] private GameObject _resourceObject;

    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            NewResource();
        }
    }
    
    private void NewResource()
    {
        Vector3 foundPos = Vector3.zero;
        int currentId = 0;
        while (foundPos == Vector3.zero)
        {
            currentId = UnityEngine.Random.Range(0, _spawnPoints.Length);
            if (!_takenPoints.Contains(currentId))
            {
                _takenPoints.Add(currentId);
                foundPos = _spawnPoints[currentId].transform.position;
            }
        }
        GameObject newResource = Instantiate(_resourceObject, foundPos, Quaternion.identity);
        newResource.GetComponent<Resource>().ID = currentId;
    }

    public void AddResources(int amount, int id)
    {
        Resources += amount;
        _takenPoints.Remove(id);
        StartCoroutine(WaitForNewResource());
    }

    public void UseResources(int amount)
    {
        Resources -= amount;
    }

    public bool CompareResources(int comparison)
    {
        return Resources >= comparison;
    }

    private IEnumerator WaitForNewResource()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 15f));
        NewResource();
    }
}
