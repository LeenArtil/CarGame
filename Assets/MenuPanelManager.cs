using UnityEngine;
using TMPro;

public class MenuPanelManager : MonoBehaviour
{
    public TextMeshProUGUI bestScoreText;
    void Start()
    {
        ScoreManager.Instance?.SetBestScoreTextReference(bestScoreText);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
