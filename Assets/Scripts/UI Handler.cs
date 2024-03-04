using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;
    public UnityEngine.UI.Image[] lives;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public UnityEngine.UI.Slider beamSlider;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Singleton violation");
        }
        instance = this;
    }

    public void SetScoreText(int score)
    {
        scoreText.text = score.ToString();
    }

    public void SetHighScoreText(int score)
    {
        highScoreText.text = score.ToString();
    }

    public void RemoveLifeIcon(int index)
    {
        if (index < 0 || index >= lives.Length || !lives[index].enabled)
        {
            Debug.Log("Invalid index for life icon");
            return;
        }
        lives[index].enabled = false;
    }

    public void SetBeamSliderValue(float value)
    {
        beamSlider.value = value;
    }
}
