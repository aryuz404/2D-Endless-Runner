using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSoundController : MonoBehaviour
{
    public AudioClip jump;
    public AudioClip scoreHighlight;
    public AudioClip gameLose;
    

    private AudioSource audioPlayer;

    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    public void PlayJump()
    {
        audioPlayer.PlayOneShot(jump);
    }

    public void PlayScoreHighlight()
    {
        audioPlayer.PlayOneShot(scoreHighlight);
    }

    public void GameOverSound()
    {
        audioPlayer.PlayOneShot(gameLose);
    }

   

}//class
