using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteGameObj : MonoBehaviour
{
public float timeTillDestroy = 1;
private Animator anim;


    void Start()
    {
            StartCoroutine(PlayClose());
            anim = GetComponent<Animator>();
    }

   IEnumerator PlayClose()
    {
        yield return new WaitForSeconds(timeTillDestroy);
        anim.Play("BubbleClose");
    }

    public void RemoveBubbles () {
GameObject.Destroy(gameObject);
    }
}
