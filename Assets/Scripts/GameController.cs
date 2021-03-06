﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour {

    bool gameHasEnded = false;
    private float restartDelay = 2.5f;
    private float nextLevelDelay = 3.5f;
    public static int numLives = 3;
    public int hpCount;
    public TextMeshProUGUI lives, gameState, items, hp, time;
    AudioSource audioSrc;
    private int finalBuildIndex = 3;
    float currTime = 0;
    public static float firstLevelTime = 0, secondLevelTime = 0, thirdLevelTime = 0;

    void Start() {
        Cursor.visible = false;
        audioSrc = GetComponent<AudioSource>();
        lives.text = "Lives: " + numLives;
        gameState.text = "";
        items.text = "Items: None";
        hp.text = "HP: " + hpCount;
        Time.timeScale = 1;
        audioSrc.pitch = 1;
    }

    void Update() {
        if (!gameHasEnded) {
            updateTime();
        }
        if (hpCount == 0) {
            EndGame();
        }
        Inputs();
    }

    void Inputs() {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q)) {
            returnToTitle();
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (!gameHasEnded && Input.GetKeyDown(KeyCode.P)) {
            Time.timeScale = (Time.timeScale > 0 ? 0 : 1);
        }
        if (Input.GetKeyDown(KeyCode.Z)) {
            Time.timeScale = (Time.timeScale == 0.75f ? 1f : 0.75f);
            audioSrc.pitch = (audioSrc.pitch == 0.75f ? 1f : 0.75f);
        }
        if (Input.GetKeyDown(KeyCode.X)) {
            Time.timeScale = (Time.timeScale == 1.5f ? 1f : 1.5f);
            audioSrc.pitch = (audioSrc.pitch == 1.5f ? 1f : 1.5f);
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            EndGame();                      // Super secret button: Goodbye cruel world
        }
    }

    public void EndGame() {
        if (gameHasEnded == false) {
            Time.timeScale = 1;
            audioSrc.pitch = 1;
            audioSrc.Stop();
            hpCount = 0;
            hp.text = "HP: " + hpCount;
            lives.text = "Lives: " + --numLives;
            gameHasEnded = true;
            if (numLives >= 0) {
                gameState.text = "You died!";
                Invoke("Restart", restartDelay);
            }
            else {
                lives.text = "Lives: " + ++numLives;
                gameState.text = "You lost all your lives! Game Over!";
                Invoke("returnToTitle", restartDelay);
            }
        }
    }

    public void levelComplete() {
        recordTime();
        audioSrc.Stop();
        gameHasEnded = true;
        lives.text = "Lives: " + ++numLives;
        if (SceneManager.GetActiveScene().buildIndex != finalBuildIndex) {
            gameState.text = "You completed the level!";
            Invoke("NextLevel", nextLevelDelay);
        }
        else {
            float totalTime = firstLevelTime + secondLevelTime + thirdLevelTime;
            string minutes = Mathf.Floor(totalTime / 60).ToString("0");
            string seconds = (totalTime % 60).ToString("00");
            gameState.text = "You beat the game! Congrats!\nTotal time: " + minutes + ":" + seconds;
            Invoke("returnToTitle", nextLevelDelay + 3);
        }
    }

    void recordTime() {
        if (SceneManager.GetActiveScene().buildIndex == 1) {
            firstLevelTime = currTime;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2) {
            secondLevelTime = currTime;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3) {
            thirdLevelTime = currTime;
        }
    }

    public void resetLevelTimes() {
        firstLevelTime = 0;
        secondLevelTime = 0;
        thirdLevelTime = 0;
    }

    void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void NextLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void updateItemsText(GameObject[] itemsList, int itemCount) {
        items.text = "Items: ";
        for (int i = 0; i < itemCount; i++) {
            if (i == 0)
                items.text += itemsList[i].tag;
            else
                items.text += ", " + itemsList[i].tag;
        }
    }

    public int getHp() {
        return hpCount;
    }

    public void updateHp(int hp) {
        hpCount = hp;
        this.hp.text = "HP: " + hpCount;
    }

    void returnToTitle() {
        SceneManager.LoadScene(0);
    }

    public void setLives(int lives) {
        numLives = lives;
    }

    void updateTime() {
        currTime += Time.deltaTime;

        string minutes = Mathf.Floor(currTime / 60).ToString("0");
        string seconds = (currTime % 60).ToString("00");
       
        time.text = minutes + ":" + seconds;
    }

}
