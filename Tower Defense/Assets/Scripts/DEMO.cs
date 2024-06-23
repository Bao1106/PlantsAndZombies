using UnityEngine;

public class Demo : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform spawnPos;

    [SerializeField] private int spawnNum = 5;

    private void Start()
    {
        for (int i = 0; i < spawnNum; i++)
        {
            Instantiate(prefab, spawnPos.position, Quaternion.identity);
        }
    }
}
