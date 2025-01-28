using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupContainer : MonoBehaviour {

    public static CupContainer cupContainer;
    private BubbleGameManager gameManager;
    [SerializeField] public List<Bubble.Flavor> content = new();
    private List<GameObject> bubbles = new();

    private AudioSource source;
    public AudioClip ohnoClip;

    private void Awake() {
        cupContainer = this;
    }

    private void Start() {
        gameManager = BubbleGameManager.gameManager;
        source = GetComponent<AudioSource>();
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Bubble>() != null) {
            if (other.GetComponent<Bubble>().flavor == Bubble.Flavor.Toxin) {
                source.PlayOneShot(ohnoClip);

                RemoveBubbles();
                other.gameObject.GetComponent<Bubble>().PopBubble();
                content.Clear();
                return;
            }
            bubbles.Add(other.gameObject);
            content.Add(other.GetComponent<Bubble>().flavor);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.GetComponent<Bubble>() != null) {
            bubbles.Remove(other.gameObject);
            content.Remove(other.GetComponent<Bubble>().flavor);
        }
    }

    public void RemoveBubbles() {
        for (int i = bubbles.Count-1; i >= 0; i--) {
            bubbles[i].GetComponent<Bubble>().PopBubble();
        }
        content.Clear();
    }
}
