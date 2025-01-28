using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayOrdersRemaining : MonoBehaviour
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
        innerText.text = gamMan.orderTimes.ToString() + " Orders Remaining";
    }
}
