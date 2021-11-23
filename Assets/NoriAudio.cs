using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoriAudio : MonoBehaviour
{
    private string ramen = "Ramen";
    private string ramenBowl = "Ramen Bowl";
    private string ramenComponent = "Ramen Component";

    public Transform dest;
    public AudioSource audioSource;
    private bool audioFinished;
    private bool ingredientMixed;
    public bool stageCompleted;

    void OnMouseDown()
    {
        bool eggComplete = GameObject.Find("Menma").GetComponent<MenmaAudio>().stageCompleted;
        if (eggComplete) {
            PlayAudio(gameObject.name);
            GrabObject();
        }
    }

    void GrabObject() {
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
        this.transform.position = dest.position;
        this.transform.parent = GameObject.Find("Destination").transform;
    }

    void OnMouseUp()
    {
        this.transform.parent = null;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    void OnCollisionEnter(Collision col)
    {
        // if collided with something other than ramen, snap back to original position
        if (!IsRamen(col.gameObject.name)) {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            Vector3 newPosition = new Vector3(9.567f, 3.104f, 0.821f);
            transform.position = newPosition;
        } else {
            // if we're holding the object, don't do anything
            if (this.transform.parent != null) return;
            ingredientMixed = true;
            GetComponent<Renderer>().enabled = false;
            if (audioSource.isPlaying && audioFinished) audioSource.Stop();
            checkStageComplete();
        }
    }

    bool IsRamen(string name) {
        return (name == ramen || name == ramenBowl || name == ramenComponent);
    }

    void PlayAudio(string name) {
        if (!audioSource.isPlaying) {
            audioSource.Play();
            audioFinished = false;
            StartCoroutine(WaitForAudioEnd(audioSource.clip.length));
        }
    }

    IEnumerator WaitForAudioEnd(float audioLength) {
        yield return new WaitForSeconds(audioLength);
        audioFinished = true;
        checkStageComplete();
    }

    void checkStageComplete() {
        if (ingredientMixed && audioFinished) {
            stageCompleted = true;
        };
    }
}
