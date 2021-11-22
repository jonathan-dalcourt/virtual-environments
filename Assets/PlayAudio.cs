using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public AudioSource introAudio;
    private bool played;


    void OnTriggerEnter(Collider other) {
        if (!played) {
            introAudio.Play();
            played = true;
        }
    }
}
