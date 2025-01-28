using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterControls : MonoBehaviour {
    
    private GrandManager grandManager;
    private Animator animator;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject textFrame;
    [SerializeField] private Sprite satisfiedSprite;
    [SerializeField] private List<string> pregameDialogue;
    [SerializeField] private List<string> postgameDialogue;
    [SerializeField] private List<string> lostgameDialogue;

    private const string WALKIN_ANIM = "CharacterWalkinAnim";
    private const string SATISFIED_ANIM = "CharacterSatisfiedAnim";
    private const string WALKOUT_ANIM = "CharacterWalkoutAnim";

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        grandManager = GrandManager.grandManager;

        textFrame.SetActive(false);
    }

    public void OnCharacterArrive() {
        Debug.Log("Arrive");
        textFrame.SetActive(true);
        StartCoroutine(EntryDialogue());
    }

    public void OnCharacterSatisfied() {
        Debug.Log("Satisfied");
        textFrame.SetActive(true);
    }

    public void OnCharacterLeave() {
        Debug.Log("Left");
        //Call the next one in.
        grandManager.OnCharacterLeft();
        Destroy(gameObject);
    }

    private IEnumerator EntryDialogue() {
        Debug.Log("Entry dialogue");
        foreach (string dialogue in pregameDialogue) {
            dialogueText.text = dialogue;
            yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
            yield return null;
        }
        SceneManager.LoadSceneAsync("BubbleTeaMiniGame");
    }

    public void PlayArrive() {
        Debug.Log("Start walk in");
        animator.Play(WALKIN_ANIM);
    }

    public void PlayReIntroduce(bool result) {
        if (result) {
            Debug.Log("PlayerWon");
            StartCoroutine(SatisfiedDialogue());
        }
        else {
            Debug.Log("PlayerLost");
            StartCoroutine(NotSatisfiedDialogue());
        }
        
    }

    private IEnumerator SatisfiedDialogue() {
        Debug.Log("Exit Dialogue");
        GetComponentInChildren<SpriteRenderer>().sprite = satisfiedSprite;
        animator.Play(SATISFIED_ANIM);
        foreach (string dialogue in postgameDialogue) {
            dialogueText.text = dialogue;
            yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
            yield return null;
        }
        textFrame.SetActive(false);
        animator.Play(WALKOUT_ANIM);
    }

    private IEnumerator NotSatisfiedDialogue() {
        Debug.Log("Lost Dialogue");
        yield return null;
        textFrame.SetActive(true);
        foreach (string dialogue in lostgameDialogue) {
            dialogueText.text = dialogue;
            yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
            yield return null;
        }
        textFrame.SetActive(false);
        animator.Play(WALKOUT_ANIM);
    }
}
