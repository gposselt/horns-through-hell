using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour
{
    public void GoTo()
    {
        SceneManager.LoadScene("CurrentPlatformScene");
    }
}
