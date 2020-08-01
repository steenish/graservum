using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

#pragma warning disable
    [SerializeField]
    private Animator animator;
#pragma warning restore

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
}
