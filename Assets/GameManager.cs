using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] allCars;
    public Transform spawnPoint;

    void Start()
    {
        int selected = PlayerPrefs.GetInt("SelectedCarIndex", 0);

        for (int i = 0; i < allCars.Length; i++)
        {
            allCars[i].SetActive(i == selected);

            // Move selected car to spawn point
            if (i == selected && spawnPoint != null)
            {
                allCars[i].transform.position = spawnPoint.position;
                allCars[i].transform.rotation = spawnPoint.rotation;
            }
        }
    }
}
