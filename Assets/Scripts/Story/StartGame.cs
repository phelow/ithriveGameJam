using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {

	public void startButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }

}
