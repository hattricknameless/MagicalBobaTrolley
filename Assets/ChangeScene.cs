using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
public void ChangeToScene() {
    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

   SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
