using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BubbleGameManager : MonoBehaviour {
    
    public float orderTimeout;
    public float originalOrderTimeout = 30;
    public bool isStarted = false;
    public int orderTimes = 2;

    private AudioSource audioSource;
    public AudioClip nextCustomer;
    public AudioClip[] nextRound;
    public AudioClip fail;
    public GameObject winBubble;
    public GameObject failBubble;


    public static BubbleGameManager gameManager;
    private GrandManager grandManager;
    private CupMovement cupMovement;

    private Dispenser dispenser;
    private CupContainer cupContainer;
    public List<Bubble.Flavor> currentOrder;

    [SerializeField] private List<GameObject> orderDisplayer;
    [SerializeField] private List<Sprite> bubbleIcon;
    [SerializeField] private float countDown;
    private Coroutine countdownCoroutine;

    private void Awake() {
        gameManager = this;
    }

    private void Start() {
        dispenser = Dispenser.dispenser;
        cupContainer = CupContainer.cupContainer;
        cupMovement = CupMovement.cupMovement;
        audioSource = transform.GetComponent<AudioSource>();
        grandManager = GrandManager.grandManager;

        originalOrderTimeout = Mathf.Clamp(originalOrderTimeout - grandManager.levelMultiplier * 1.5f, 12f, 40f);
        orderTimeout = originalOrderTimeout;
        
        GenerateOrder();
        StartCoroutine(StartGame());
    }

    private void Update() {
        if (CompareOrder()) {
            if (countdownCoroutine == null) {
                countdownCoroutine = StartCoroutine(OrderCountdown());
            }
        }

        if (orderTimeout < 0) {
            if (isStarted) StartCoroutine(Fail());
        }
        else {
            if (isStarted) orderTimeout -= Time.deltaTime;
        }
    }

    private void GenerateOrder() {
        cupMovement.SpawnCup();
        List<Bubble.Flavor> randomOrder = new();
        int count = UnityEngine.Random.Range(3, 6);
        for (int i = 0; i < count; i++) {
            randomOrder.Add((Bubble.Flavor)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Bubble.Flavor)).Length-1));
        }
        currentOrder = randomOrder;

        orderTimeout = originalOrderTimeout;

        DisplayOrder();
    }

    private void DisplayOrder() {
        for (int i = 0; i < orderDisplayer.Count; i++) {
            try {
                orderDisplayer[i].GetComponent<Image>().sprite = bubbleIcon[(int)currentOrder[i]];
            }
            catch {
                orderDisplayer[i].GetComponent<Image>().sprite = bubbleIcon[bubbleIcon.Count-1];
            }
        }
    }

    public bool CompareOrder() {
        int[] containerCounter = new int[Enum.GetValues(typeof(Bubble.Flavor)).Length-1];
        foreach (Bubble.Flavor flavor in cupContainer.content) {
            containerCounter[(int)flavor] += 1;
        }
        // Debug.Log($"{containerCounter[0]} {containerCounter[1]} {containerCounter[2]} {containerCounter[3]}");
        foreach (Bubble.Flavor flavor in currentOrder) {
            containerCounter[(int)flavor] -= 1;
        }
        // Debug.Log($"{containerCounter[0]} {containerCounter[1]} {containerCounter[2]} {containerCounter[3]}");
        for (int i = 0; i < containerCounter.Count()-1; i++) {
            if (containerCounter[i] < 0) {return false;}
        }
        if (containerCounter.Sum() != 0) {return false;}
        return true;
    }

    private IEnumerator StartGame() {
        yield return new WaitForSeconds(1);
        isStarted = true;
    }

    private IEnumerator OrderCountdown() {

        print("Start count down");
        float timer = countDown;
        while (timer > 0) {
            if (!CompareOrder()) {
                yield break;
            }
            timer -= Time.deltaTime;
            yield return null;
        }
        print("Contained");
        
        //Detection for win-lose
        //Order completed
        cupContainer.RemoveBubbles();
        cupMovement.FireCup();

        if (orderTimes > 1) {
            orderTimes -= 1; 
        }
        else {
            if (isStarted) StartCoroutine(Win());
            yield break;
        }

        audioSource.PlayOneShot(nextRound[UnityEngine.Random.Range(0, nextRound.Length-1)]);
        GenerateOrder();
        yield break;
    }

    private IEnumerator Win() {

        isStarted = false;

        // Play Next Customer Sound
        audioSource.PlayOneShot(nextCustomer, 1f);
        // Show Bubble
        winBubble.SetActive(true);
        yield return new WaitForSeconds(1f);
        grandManager.GetMiniGameResult(true);

        SceneManager.LoadScene("DialogueScene", LoadSceneMode.Single);
    }
 
    private IEnumerator Fail() {

        Debug.Log("Hello :)");

        isStarted = false;

        // Play Fail Sound
        audioSource.PlayOneShot(fail, 1f);
        // Game Reset
        // Show Bubble
        failBubble.SetActive(true);

        yield return new WaitForSeconds(1f);
        grandManager.GetMiniGameResult(false);

        SceneManager.LoadScene("DialogueScene", LoadSceneMode.Single);
    }
}
