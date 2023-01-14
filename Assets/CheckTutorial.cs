using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckTutorial : MonoBehaviour
{
    private void Awake()
    {
        // Tutorial done?           0 = No           1 = Yes
        if (PlayerPrefs.GetInt("DONE_TUTORIAL", 0) == 0)
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        else
            SceneManager.LoadScene(2, LoadSceneMode.Single);
    }
}
