using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterManager : MonoBehaviour {
    
    private GrandManager grandManager;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start() {
        grandManager = GrandManager.grandManager;
    }

    private void Update() {
        scoreText.text = $"Customer Served: {grandManager.customerCount}";
    }
}
