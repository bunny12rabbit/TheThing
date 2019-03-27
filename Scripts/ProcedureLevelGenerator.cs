using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcedureLevelGenerator : MonoBehaviour {

    [System.Serializable]
    public struct PickPlatform
    {
        public string Name;
        public GameObject Item;
        public int Chance;
    }

    public GameObject FirstPlatform;
    public GameObject FinishPlatform;
    public List<PickPlatform> Platforms = new List<PickPlatform>();
    public float LastPointGeneration = 445f;
    [SerializeField] bool proceedLevel = false;
    [SerializeField] GameObject startPlatformIfProceedLevel;
    [SerializeField] float _maxSpawnHeight = 3.7f;
    [SerializeField] float _minSpawnHeight = -3.84f;
    List<Vector3> _spawnPoints = new List<Vector3>();
    Vector3 _nextSpawnPoint;
    int _platformSelector;
    int _spawnPointSelector;
    private int lastPrefabIndex = -1;
    GameObject nextPlatform;

    private void Start()
    {
        if (!proceedLevel)
        {
            nextPlatform = Instantiate(FirstPlatform, transform.position, transform.rotation);
            GetSpawnPoints(nextPlatform);
            GenerateNextPlatform();
        }
    }

    void GenerateNextPlatform()
    {
        //Генерируем платформы до конца уровня, заданного по X LastPointGeneration
        do
        {
            _platformSelector =  RandomPlatformIndex();
            InstantiatePlatform(_platformSelector);
            GetSpawnPoints(nextPlatform);

        } while (nextPlatform.transform.position.x <= LastPointGeneration);

        GetSpawnPoints(nextPlatform);
        Vector3 _finishPoint = new Vector3(nextPlatform.transform.position.x + 10, -3.5f, 1);
        Instantiate(FinishPlatform, _finishPoint, Quaternion.identity);
    }

    void InstantiatePlatform(int value)
    {
        _nextSpawnPoint = _spawnPoints[RandomSpawnPointIndex()];
        nextPlatform = Instantiate(Platforms[value].Item, _nextSpawnPoint, Quaternion.identity);
        if (nextPlatform.transform.position.y <= _minSpawnHeight || nextPlatform.transform.position.y >= _maxSpawnHeight)
        {
            Destroy(nextPlatform);
            InstantiatePlatform(value);
        }
    }

    void GetSpawnPoints(GameObject platform)
    {
        if(_spawnPoints.Count != 0)
        _spawnPoints.Clear();
        for (int i = 0; i < platform.transform.childCount; i++)
        {
            if (platform.transform.GetChild(i).CompareTag("SpawnPoint"))
                _spawnPoints.Add(platform.transform.GetChild(i).position); 
        }
    }

    int RandomPlatformIndex()
    {
        int itemWeight = 0;
        for (int i = 0; i < Platforms.Count; i++)
        {
            itemWeight += Platforms[i].Chance;
        }

        int randomIndex = Random.Range(0, itemWeight);

        for (int j = 0; j < Platforms.Count; j++)
        {
            if (randomIndex <= Platforms[j].Chance)
            {
                randomIndex = j;
                return randomIndex;
            }
            randomIndex -= Platforms[j].Chance;
        }
                return randomIndex;
    }

    int RandomSpawnPointIndex()
        {
            if (_spawnPoints.Count <= 1)
                return 0;

            int randomIndex = lastPrefabIndex;
            while (randomIndex == lastPrefabIndex || randomIndex == -1)
            {
                randomIndex = Random.Range(-1, _spawnPoints.Count);
            }
            lastPrefabIndex = randomIndex;
            return randomIndex;
        }
}
