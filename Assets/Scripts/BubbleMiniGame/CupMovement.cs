using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupMovement : MonoBehaviour {
    
    public static CupMovement cupMovement;

    private Vector2 mousePosition;
    private Rigidbody2D rb;
    private bool isAnimating = false;

    private Vector2 forceThrow = new Vector2(-225f, 35f);

    private void Awake() {
        cupMovement = this;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            if (!isAnimating) {
                rb.MovePosition(mousePosition);
            } 
    }

    public void SpawnCup() {
        StartCoroutine(SpawnIn());
    }

    public void FireCup() {
        StartCoroutine(FireOff());
    }

    private IEnumerator SpawnIn() {
        isAnimating = true;
        rb.freezeRotation = false;
        rb.gravityScale = 2;

        transform.position = new Vector3(5, -10, 0);
        rb.velocity = new Vector2(-5, 25);
        yield return new WaitForSeconds(0.6f);

        isAnimating = false;
        transform.rotation = Quaternion.identity;
        rb.freezeRotation = true;
        rb.gravityScale = 0;
    }

    private IEnumerator FireOff () {
        isAnimating = true;
        rb.freezeRotation = false;
        rb.gravityScale = 2;

        rb.velocity = new Vector2(0,0);
        rb.AddForceAtPosition(forceThrow, new Vector2(0,2), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.6f);
        
        isAnimating = false;
        transform.rotation = Quaternion.identity;
        rb.freezeRotation = true;
        rb.gravityScale = 0;
    }
}


