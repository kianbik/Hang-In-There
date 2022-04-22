using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Buttons : MonoBehaviour
{
    public GameObject MainMenuCanvas;

    public GameObject CreditsCanvas;

    public AudioClip HoverClip;
    public AudioClip ClickClip;
    public AudioSource audioSource;
    public void OnPlayClicked()
    {
        SceneManager.LoadScene("MainLevel");
    }
    public void OnStartClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnCreditsClicked()
    {
        CreditsCanvas.SetActive(true);
        MainMenuCanvas.SetActive(false);
    }



    public void OnBackClicked()
    {

        CreditsCanvas.SetActive(false);

       
        MainMenuCanvas.SetActive(true);
    }

    public void OnExitClicked()
    {
        Application.Quit();
    }

    public void HoverSound()
    {
        audioSource.PlayOneShot(HoverClip);
    }
    public void ClickSound()
    {
        audioSource.PlayOneShot(ClickClip);
    }
}
