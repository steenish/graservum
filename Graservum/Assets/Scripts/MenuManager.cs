using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

#pragma warning disable
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Toggle soundEffectsToggle;
    [SerializeField]
    private Toggle musicToggle;
#pragma warning restore

    private void Start() {
        if (!PlayerPrefs.HasKey("PlaySoundEffects")) PlayerPrefs.SetInt("PlaySoundEffects", 1);
        if (!PlayerPrefs.HasKey("PlayMusic")) PlayerPrefs.SetInt("PlayMusic", 1);

        if (PlayerPrefs.GetInt("PlaySoundEffects") == 0) soundEffectsToggle.isOn = false;
        else soundEffectsToggle.isOn = true;

        if (PlayerPrefs.GetInt("PlayMusic") == 0) musicToggle.isOn = false;
        else musicToggle.isOn = true;
    }

    public void StartGame() {
        SceneManager.LoadSceneAsync("MainGameScene");
    }

    public void ShowMainMenu() {
        animator.SetTrigger("ShowMain");
    }

    public void ShowInstructionsMenu() {
        animator.SetTrigger("ShowInstructions");
    }

    public void ShowSettingsMenu() {
        animator.SetTrigger("ShowSettings");
    }

    public void OnSoundEffectsValueChange() {
        if (soundEffectsToggle.isOn) {
            PlayerPrefs.SetInt("PlaySoundEffects", 1);
        } else {
            PlayerPrefs.SetInt("PlaySoundEffects", 0);
            AudioManager.instance.StopSoundEffects();
        }
    }

    public void OnMusicValueChange() {
        if (musicToggle.isOn) {
            PlayerPrefs.SetInt("PlayMusic", 1);
			AudioManager.instance.Play("Theme");
        } else {
            PlayerPrefs.SetInt("PlayMusic", 0);
            AudioManager.instance.StopMusic();
        }
    }

	public void OnClickResetHighscore() {
		ScoreManager.ResetHighscore();
	}

	public void OnClickLogo() {
		Application.OpenURL("https://geodropstudios.github.io/");
	}
}
