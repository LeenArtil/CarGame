using UnityEngine;
using TMPro;

public class MenuPanelManager : MonoBehaviour
{
    public TextMeshProUGUI bestScoreText;
    void Start()
    {
       // ResetProgress();
        ScoreManager.Instance?.SetBestScoreTextReference(bestScoreText);
    }


    public void ResetProgress()
    {
        PlayerPrefs.SetInt("BestScore", 0);
        PlayerPrefs.Save();
        Debug.Log("✅ Progress reset. BestScore is now 0.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
