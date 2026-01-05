using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLogic : MonoBehaviour
{   [SerializeField] private GameObject CreditsWindow ;
    [SerializeField] private GameObject SavesWindow ;
    [SerializeField] private GameObject SoundWindow ;
    [SerializeField] private GameObject SettingsWindow ;

  void Start() //saves to zero
    {
        if (Static.size > Static.sizeMax)
        {
            Static.size = 0;
        }
    }
   
    public void StartGame() // Loading data from Static
    {
        if (Static.size == 0)
        {
            Static.entrySize = 0;
        }
        Static.sceneName = "CoreMain";
        SceneManager.LoadScene("Loading");
    }

    public void CloseWindows() //For the Back button in pop-up windows
    {
        GameObject[] windows = GameObject.FindGameObjectsWithTag("OverlayWindow");
        foreach (GameObject obj in windows)
        {
            obj.SetActive(false);
        }
        /*foreach (RectTransform entry in content)
        {
            Destroy(entry.gameObject);
        }*/
        //CommentData.SetActive(false);
        //+ add save data if settings window etc
    }
    public void CreditsShow()
    {
        CreditsWindow.SetActive(true);
    }
    
}
