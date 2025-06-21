using UnityEngine;
using TMPro;


public class FreeModeCanvasManager : MonoBehaviour
{
    public TextMeshProUGUI currentScoreText;

    void Start()
    {
        ScoreManager.Instance?.SetCurrentScoreTextReference(currentScoreText);
        ScoreManager.Instance?.ResetScore();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
