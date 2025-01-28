using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {

    public enum Flavor {
        Mango,
        Tapioca,
        GrassJelly,
        Any,
        Toxin
    }
    public Flavor flavor;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Killzone")) {
        Destroy(gameObject);
        }
    }

    public void PopBubble() {
        StartCoroutine(DestroyBubble());
    }

    private IEnumerator DestroyBubble() {
        GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(true);
            
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    } 
}
