using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAudio : MonoBehaviour
{
    private string ramen = "Ramen";
    private string ramenBowl = "Ramen Bowl";
    private string ramenComponent = "Ramen Component";
    public bool stageCompleted;
    public AudioSource audioSource;

    void OnTriggerEnter(Collider other) {
        if (!audioSource.isPlaying && !stageCompleted) {
            //audioSource.Play();
            //StartCoroutine(WaitForAudioEnd(audioSource.clip.length));
        }
    }

    void PlayAudio() {
        // TODO if audio ! playing?
        if (!audioSource.isPlaying) {
            audioSource.Play();
            StartCoroutine(WaitForAudioEnd(audioSource.clip.length));
        }
    }

    IEnumerator WaitForAudioEnd(float audioLength) {
        yield return new WaitForSeconds(audioLength);
        stageCompleted = true;
    }
}
