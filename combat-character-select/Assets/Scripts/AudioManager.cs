using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Inspector Fields
    [Header("Audio Sources")]
    [SerializeField] private AudioSource p1AudioSource;
    [SerializeField] private AudioSource p2AudioSource;
    [Header("Audio Clips")]
    [SerializeField] private AudioClip playerJoin;
    [SerializeField] private AudioClip cursorChange;
    #endregion

    //Plays the player joined audio clip
    public void PlayPlayerJoin(int playerIndex)
    {
        //Get target audio source based on player index
        var targetSource = playerIndex == 0 ? p1AudioSource : p2AudioSource;

        if (targetSource.isPlaying)
            targetSource.Stop();

        //Play cursor change in one shot
        targetSource.PlayOneShot(playerJoin);
    }

    //Plays the cursor change audio clip
    public void PlayCursorChange(int playerIndex)    
    {
        //Get target audio source based on player index
        var targetSource = playerIndex == 0 ? p1AudioSource : p2AudioSource;

        if(targetSource.isPlaying)
            targetSource.Stop();

        //Play cursor change in one shot
        targetSource.PlayOneShot(cursorChange);    
    }
}
