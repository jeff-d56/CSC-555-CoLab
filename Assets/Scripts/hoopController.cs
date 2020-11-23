using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hoopController : MonoBehaviour
{
    public int scoreMultiplyer = 1;
    public Text scoreOfTarget;

    private void Start()
    {
        scoreOfTarget.text = scoreMultiplyer.ToString();
    }
}
