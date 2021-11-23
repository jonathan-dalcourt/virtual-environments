using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IngredientStatus { Uninitialized, AudioPlayed, Mixed, PlayedAndMixed }
public enum RecipeStage { Intro, Egg, FishCake, Menma, Nori }
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


    public Transform dest;
    public AudioSource eggAudio;
    public AudioSource menmaAudio;
    public AudioSource fishCakeAudio;
    public AudioSource noriAudio;
    public AudioSource introAudio;
    public List<IngredientStatus> recipeStage;

    void OnTriggerEnter(Collider other) {
        int introIndex = (int) RecipeStage.Intro;
        if (recipeStage.Count == (int) introIndex) {
            PlayAudio(intro);
        }
    }

    void OnMouseDown()
    {
        RecipeStage stage = GetStageFromName(gameObject.name);
        //if (CanInteract(stage)) {
            PlayAudio(gameObject.name);
            GrabObject();
        //}
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

            RecipeStage stage = GetStageFromName(gameObject.name);
            UpdateStatus(stage, IngredientStatus.Mixed);
            Destroy(gameObject);
        }
    }

    RecipeStage GetStageFromName(string name) {
        if (name == egg) return RecipeStage.Egg;
        else if (name == fishCake) return RecipeStage.FishCake;
        else if (name == menma) return RecipeStage.Menma;
        else if (name == nori) return RecipeStage.Nori;
        else if (name == intro) return RecipeStage.Intro;
        else return RecipeStage.Intro; // TODO fix
    }

    bool IsRamen(string name) {
        return (name == ramen || name == ramenBowl || name == ramenComponent);
    }

    void PlayAudio(string name) {
        RecipeStage stage = GetStageFromName(name);

        // if (!canInteract(stage)) return;

        AudioSource audio;

        if (stage == RecipeStage.Intro) {
            audio = introAudio;
        } else if (stage == RecipeStage.Egg) {
            audio = eggAudio;
        } else if (stage == RecipeStage.FishCake) {
            audio = fishCakeAudio;
        } else if (stage == RecipeStage.Menma) {
            audio = menmaAudio;
        } else if (stage == RecipeStage.Nori) {
            audio = noriAudio;
        } else return;

        // TODO if audio ! playing?
        audio.Play();
        StartCoroutine(WaitForAudioEnd(stage, audio.clip.length));
    }

    // void audioHelper(RecipeStage stage, AudioSource audio) {
    //     audio.Play();
    //     StartCoroutine(WaitForAudioEnd(audio.clip.length));
    // }

    IEnumerator WaitForAudioEnd(RecipeStage stage, float audioLength) {
        print("beginning");
        yield return new WaitForSeconds(audioLength);
        print("ended");
        if (stage == RecipeStage.Intro) {
            UpdateStatus(stage, IngredientStatus.PlayedAndMixed);
        } else {
            UpdateStatus(stage, IngredientStatus.AudioPlayed);
        }
    }

    bool CanInteract(RecipeStage stage) {
        int stageIndex = (int) stage;
        if (recipeStage.Count < stageIndex) return false;
        if (recipeStage[stageIndex] == IngredientStatus.PlayedAndMixed) return false;
            // if (recipeStage[stageIndex] != IngredientStatus.PlayedAndMixed) {
            //     return false;
            // }
            // if (stage != stageNori && recipeStage[stage + 1] != null) {
            //     return false;
            // }
        return true;
    }

    void UpdateStatus(RecipeStage stage, IngredientStatus newStatus) {
        int stageIndex = (int) stage;
        // if ingredient has not been added or fully played
        if (recipeStage.Count <= stageIndex) {
            recipeStage.Add(newStatus);
        } else {
            IngredientStatus oldStatus = recipeStage[stageIndex];
            if (newStatus == IngredientStatus.AudioPlayed) {
                if (oldStatus != IngredientStatus.Mixed) return; // played, but not mixed
            } else if (newStatus == IngredientStatus.Mixed) {
                if (oldStatus != IngredientStatus.AudioPlayed) return; // mixed, but not played
            }
            recipeStage[stageIndex] = IngredientStatus.PlayedAndMixed;
        }
    }

}
