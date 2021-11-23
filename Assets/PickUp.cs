using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IngredientStatus { Played, Mixed, PlayedAndMixed }
public class PickUp : MonoBehaviour
{
    private string nori = "Nori";
    private string fishCake = "Fish Cake";
    private string menma = "Menma";
    private string egg = "Egg";
    private string ramen = "Ramen";
    private string ramenBowl = "Ramen Bowl";
    private string ramenComponent = "Ramen Component";
    private int stageIntro = 0;
    private int stageEgg = 1;
    private int stageFishCake = 2;
    private int stageMenma = 3;
    private int stageNori = 4;
    private int played = 1;
    private int mixed = 2;
    private int playedAndMixed = 3;

    public Transform dest;
    public AudioSource eggAudio;
    public AudioSource menmaAudio;
    public AudioSource fishCakeAudio;
    public AudioSource noriAudio;
    private IEnumerator audioRoutine;
    private int checkpoint = 0;
    public IngredientStatus[] recipeStage;


    void OnMouseDown()
    {

        if (gameObject.name == egg) {
            if (recipeStage[stageIntro] == IngredientStatus.PlayedAndMixed) {
                // bool introPlaying = GameObject.Find("Intro Trigger").GetComponent<PlayAudio>().introAudio.isPlaying;
                // bool introPlayed = GameObject.Find("Intro Trigger").GetComponent<PlayAudio>().played;
                // if (introPlaying || !introPlayed) return;

                eggAudio.Play();
                StartCoroutine(markPlayed(eggAudio.clip.length));
                checkpoint = stageFishCake;
            } else if (gameObject.name == fishCake) {
                if (recipeStage[stageEgg] == mixed) {
                    fishCakeAudio.Play();
                }
            } else if (gameObject.name == menma) {
                if (checkpoint != stageMenma) return;
                menmaAudio.Play();
            } else if (gameObject.name == nori) {
                if (checkpoint != stageNori) return;
                noriAudio.Play();
            } else {
                noriAudio.Play();
                return;
            }
    }
    }

    void pickUp() {
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
        this.transform.position = dest.position;
        this.transform.parent = GameObject.Find("Destination").transform;
    }

    IEnumerator markPlayed(float audioLength) {
        yield return new WaitForSeconds(audioLength);
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
        if (!isRamen(col.gameObject.name)) {
            Vector3 newPosition;
            if (gameObject.name == egg) {
                newPosition = new Vector3(10.542f, 3.109f, 0.853f);
            } else if (gameObject.name == fishCake) {
                newPosition = new Vector3(10.022f, 3.103f, 1.026f);
            } else if (gameObject.name == menma) {
                newPosition = new Vector3(10.073f, 3.109f, 0.549f);
            } else if (gameObject.name == nori) {
                newPosition = new Vector3(9.567f, 3.104f, 0.821f);
            } else {
                return;
            }
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            transform.position = newPosition;
        } else {
            if (this.transform.parent != null) return;

            int stage = getStageFromName(gameObject.name);
            if (stage == -1) return;

            if (recipeStage[stage] == IngredientStatus.Played) {
                recipeStage[stage] = IngredientStatus.PlayedAndMixed;
            } else {
                recipeStage[stage] = IngredientStatus.Mixed;
            }
            
            Destroy(gameObject);
        }
    }

    int getStageFromName(string name) {
        if (gameObject.name == egg) return stageEgg;
        else if (gameObject.name == fishCake) return stageFishCake;
        else if (gameObject.name == menma) return stageMenma;
        else if (gameObject.name == nori) return stageNori;
        else return -1;
    }

    bool isRamen(string name) {
        return (name == ramen || name == ramenBowl || name == ramenComponent);
    }

}
