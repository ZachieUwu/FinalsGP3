using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneNextScene : MonoBehaviour
{
    [SerializeField] string levelName;
    void OnEnable()
    {
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }
}
