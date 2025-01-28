using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayTimeRemaining : MonoBehaviour
{
    public BubbleGameManager gamMan;
    private TextMeshProUGUI innerText;

  void Awake()
    {
        innerText = gameObject.GetComponent<TextMeshProUGUI>();   
    }

    // Update is called once per frame
    void Update()
    {
        innerText.text = (Mathf.Round(gamMan.orderTimeout * 10) / 10).ToString() + "s";
    }
}
