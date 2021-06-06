using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour
{
    [SerializeField]
    private UIBar progressBar;

    // Start is called before the first frame update
    void Start()
    {
        // Start async operation
        StartCoroutine(LoadAsyncOperation());
    }

    IEnumerator LoadAsyncOperation()
	{
        // Create am async operation
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(2);

        // Sync progress bar to scene loading
        while(gameLevel.progress < 1)
		{
            progressBar.SetValue(gameLevel.progress);
            yield return new WaitForEndOfFrame();
        }
	}
}
