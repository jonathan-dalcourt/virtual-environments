using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    public GameObject[] ingredients;

    void Start() {
        StartCoroutine(playAudioSequentially());
    }

    IEnumerator playAudioSequentially()
    {
    yield return new WaitForSeconds(1.5f);
    audioSource.Play();
    while (audioSource.isPlaying) yield return null;
    
        for (int i = 0; i < audioClips.Length; i++)
        {
            GameObject ingredient = ingredients[i];
            outlineObject(ingredient);

            audioSource.clip = audioClips[i];
            yield return new WaitForSeconds(1);
            audioSource.Play();
            while (audioSource.isPlaying) yield return null;
            yield return new WaitForSeconds(1);
            ingredient.GetComponent<Renderer>().enabled = false;
            yield return new WaitForSeconds(0.03f);

        }
    }

    void outlineObject(GameObject obj) {
        var outline = obj.AddComponent<Outline>();

        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineColor = Color.green;
        outline.OutlineWidth = 3f;
    }
}
