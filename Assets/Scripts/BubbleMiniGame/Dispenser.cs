using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Dispenser : MonoBehaviour {

    public static Dispenser dispenser;
    private GrandManager grandManager;
    [SerializeField] public List<GameObject> bubblePrefabs;
    [SerializeField] private float dispenseInterval;
    [SerializeField] private float range;
    private Coroutine dispenserTimer;
    public float speedMultiplier = 1f;
    
    private void Awake() {
        dispenser = this;
    }

    private void Start() {
        grandManager = GrandManager.grandManager;
        
        StartDispenser();
    }

    private IEnumerator DispenseTimer() {
        while (true) {
            yield return new WaitForSeconds(dispenseInterval);
            Vector2 position = new Vector2(UnityEngine.Random.Range(transform.position.x - range, transform.position.x + range), transform.position.y);
            GameObject bubble = Instantiate(bubblePrefabs[UnityEngine.Random.Range(0, bubblePrefabs.Count)], position, quaternion.identity);
            bubble.GetComponent<Rigidbody2D>().drag = UnityEngine.Random.Range(2f, 6f) * speedMultiplier;
        }
    }

    public void StartDispenser() {
        speedMultiplier *= grandManager.levelMultiplier;
        dispenseInterval = Mathf.Clamp(dispenseInterval - 0.1f * grandManager.customerCount / 2f, 0.4f, 0.8f);
        dispenserTimer ??= StartCoroutine(DispenseTimer());
    }
}
