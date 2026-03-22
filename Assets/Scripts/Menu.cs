using System.Collections;
using UnityEditor.AdaptivePerformance.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    [SerializeField] GameObject Settings;

    IEnumerator Again()
    {
        yield return new WaitForSecondsRealtime(.5f);
        SceneManager.LoadScene("1stBuild");
    }

    public void LoadGame() {
        //pour le bouton play, lance la scene du jeu
        Time.timeScale=1;
        SceneManager.LoadScene("1stBuild");
        StartCoroutine(Again());
    }

    public void ExitGame(){
        //ferme le jeu correctement ?
        Application.Quit();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Settings.SetActive(false); //cacher le menu d'options
    }

  
}
