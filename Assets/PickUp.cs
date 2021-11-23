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
    private string intro = "Intro";
    private int stageIntro = 0;
    private int stageEgg = 1;
    private int stageFishCake = 2;
    private int stageMenma = 3;
    private int stageNori = 4;

    public Transform dest;
    public AudioSource eggAudio;
    public AudioSource menmaAudio;
    public AudioSource fishCakeAudio;
    public AudioSource noriAudio;
    public AudioSource introAudio;
    public IngredientStatus?[] recipeStage;

    void Start() {
        recipeStage = new IngredientStatus?[5];
    }

    void OnTriggerEnter(Collider other) {
        if (recipeStage[stageIntro] != IngredientStatus.PlayedAndMixed) {
            playAudio(intro);
        }
    }

    void OnMouseDown()
    {
        int stage = getStageFromName(gameObject.name);
        if (stage != -1 && canInteract(stage)) {
            playAudio(gameObject.name);
            grabObject();
        }
    }

    void grabObject() {
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
            // if we're holding the object, don't do anything
            if (this.transform.parent != null) return;

            int stage = getStageFromName(gameObject.name);
            updateStatus(stage, IngredientStatus.Mixed);
            Destroy(gameObject);
        }
    }

    int getStageFromName(string name) {
        if (name == egg) return stageEgg;
        else if (name == fishCake) return stageFishCake;
        else if (name == menma) return stageMenma;
        else if (name == nori) return stageNori;
        else if (name == intro) return stageIntro;
        else return -1;
    }

    bool isRamen(string name) {
        return (name == ramen || name == ramenBowl || name == ramenComponent);
    }

    void playAudio(string name) {
        int stage = getStageFromName(name);

        // if (!canInteract(stage)) return;

        AudioSource audio;

        if (stage == stageIntro) {
            audio = introAudio;
        } else if (stage == stageEgg) {
            audio = eggAudio;
        } else if (stage == stageFishCake) {
            audio = fishCakeAudio;
        } else if (stage == stageMenma) {
            audio = menmaAudio;
        } else if (stage == stageNori) {
            audio = noriAudio;
        } else return;

        // TODO if audio ! playing?
        audio.Play();
        StartCoroutine(waitForAudioEnd(audio.clip.length));
        if (stage == stageIntro) {
            updateStatus(stage, IngredientStatus.PlayedAndMixed);
        } else {
            updateStatus(stage, IngredientStatus.Played);
        }
    }

    bool canInteract(int stage) {
        if (stage == -1) return false;
            if (recipeStage[stage - 1] != IngredientStatus.PlayedAndMixed) {
                int x = 0;
                int y = x / 2;
                return false;
            }
            if (stage != stageNori && recipeStage[stage + 1] != null) {
                int x = 0;
                int y = x / 2;
                return false;
            }

        return true;
    }

    IEnumerator waitForAudioEnd(float audioLength) {
        yield return new WaitForSeconds(audioLength);
    }
    void updateStatus(int stage, IngredientStatus newStatus) {
        if (stage == -1) return;

        IngredientStatus? oldStatus = recipeStage[stage];
        
        // if ingredient has not been added or fully played
        if (oldStatus == null) {
            recipeStage[stage] = newStatus;
            return;
        }

        if (newStatus == IngredientStatus.Played) {
            if (oldStatus != IngredientStatus.Mixed) return; // played, but not mixed
        } else if (newStatus == IngredientStatus.Mixed) {
            if (oldStatus != IngredientStatus.Played) return; // mixed, but not played
        }

        recipeStage[stage] = IngredientStatus.PlayedAndMixed;
    }

}
