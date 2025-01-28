using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GrandManager : MonoBehaviour {
    
    public static GrandManager grandManager;

    [Header("Character Manager")]
    private GameObject characterInScene;
    [SerializeField] private List<GameObject> characterPrefab;
    [SerializeField] private List<int> characterPointerList = new();
    [SerializeField] private int characterPointer;

    [Header("Difficulty Manager")]
    public float levelMultiplier;
    [SerializeField] private float levelIncrement;
    private bool gameResult;
    [SerializeField] private AudioSource bgmAudio;

    [Header("Scoreboard")]
    public int customerCount;

    public enum GameState {CharacterArrive, CharacterServiced}
    public GameState gameState;

    private void Awake() {
        if (grandManager == null) {
            grandManager = this;
        }
        else {
            Destroy(gameObject);
        }
        gameState = GameState.CharacterArrive;
        customerCount = 0;

        SceneManager.sceneLoaded += OnSceneLoaded;

        DontDestroyOnLoad(grandManager);
    }

    private void Start() {

    }

    private void OnDisable() {
        // Unsubscribe all listeners from SceneManager.sceneLoaded
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Debug.Log($"Trigger Scene Loaded {scene.buildIndex}");
        int currentSceneIndex = scene.buildIndex;
        if (currentSceneIndex == 0) {
            Debug.Log("Title Screen");
            gameState = GameState.CharacterArrive;
        }
        else if (currentSceneIndex == 1) {
            //Enter Dialogue Scene
            Debug.Log("Found Dialogue Scene");
            if (gameState == GameState.CharacterArrive) {
                //Generate a new character and arrive
                GenerateNewCharacter();
                gameState = GameState.CharacterServiced;
            }
            else if (gameState == GameState.CharacterServiced) {
                //Check if the customer is satisfied
                //Continue or End game
                ReIntroduceCharacter();
            }
        }
        else if (currentSceneIndex == 2) {
            //Enter Minigame Scene
            Debug.Log("Found Minigame Scene");
        }
    }

    private int PopNextCharacter() {
        //Refill the PointerList
        if (characterPointerList.Count == 0) {
            characterPointerList = new();
            for (int i = 0; i < characterPrefab.Count; i++) {characterPointerList.Add(i);}

            for (int i = characterPointerList.Count - 1; i > 0; i--) {
                // Pick a random index from 0 to i
                int randomIndex = UnityEngine.Random.Range(0, i + 1);

                // Swap the current element with the randomly chosen one
                int temp = characterPointerList[i];
                characterPointerList[i] = characterPointerList[randomIndex];
                characterPointerList[randomIndex] = temp;
            }
        }
        int value = characterPointerList[0];
        characterPointerList.RemoveAt(0);
        return value;
    }

    private void GenerateNewCharacter() {
        bgmAudio.pitch = Mathf.Clamp(bgmAudio.pitch + levelIncrement / 2, 0.7f, 1.1f);
        characterPointer = PopNextCharacter();
        characterInScene = Instantiate(characterPrefab[characterPointer], new Vector3(11, 0, 0), quaternion.identity);
        characterInScene.GetComponent<CharacterControls>().PlayArrive();
    }

    private void ReIntroduceCharacter() {
        characterInScene = Instantiate(characterPrefab[characterPointer], new Vector3(0, 0, 0), quaternion.identity);
        characterInScene.GetComponent<CharacterControls>().PlayReIntroduce(gameResult);
    }

    public void GetMiniGameResult(bool result) {
        gameResult = result;

        levelMultiplier += levelIncrement;
        Debug.Log(result);
    }

    public void OnCharacterLeft() {
        if (gameResult == true) {
            customerCount += 1;
            GenerateNewCharacter();
        }
        else {
            customerCount = 0;
            SceneManager.LoadScene("TitleScene");
        }
    }
}
