using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private string nori = "Nori";
    private string fishCake = "Fish Cake";
    private string menma = "Menma";
    private string egg = "Egg";
    private string ramen = "Ramen";
    private string ramenBowl = "Ramen Bowl";
    private string ramenComponent = "Ramen Component";
    private int stageEgg = 0;
    private int stageFishCake = 1;
    private int stageMenma = 2;
    private int stageNori = 3;

    public Transform dest;
    public AudioSource eggAudio;
    public AudioSource menmaAudio;
    public AudioSource fishCakeAudio;
    public AudioSource noriAudio;

    private int checkpoint = 0;


    void OnMouseDown()
    {
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
        this.transform.position = dest.position;
        this.transform.parent = GameObject.Find("Destination").transform;

        if (gameObject.name == egg) {
                if (checkpoint != stageEgg) return;
                eggAudio.Play();
            } else if (gameObject.name == fishCake) {
                if (checkpoint != stageFishCake) return;
                fishCakeAudio.Play();
            } else if (gameObject.name == menma) {
                if (checkpoint != stageMenma) return;
                menmaAudio.Play();
            } else if (gameObject.name == nori) {
                if (checkpoint != stageNori) return;
                noriAudio.Play();
            } else {
                return;
            }
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
            // snap back to pos
            // egg 10.542/3.109/0.853
            // fish cake 10.022/3.103/1.026
            // menma 10.073/3.109/0.549
            // nori 9.567/3.104/0.821
            Vector3 newPosition;
            if (gameObject.name == egg) {
                newPosition = new Vector3(10.542f, 3.109f, 0.853f);
            } else if (gameObject.name == fishCake) {
                newPosition = new Vector3(10.022f, 3.103f, 1.026f);
            } else if (gameObject.name == menma) {
                newPosition = new Vector3(10.073f, 3.208f, 0.549f);
            } else if (gameObject.name == nori) {
                newPosition = new Vector3(9.567f, 3.104f, 0.821f);
            } else {
                return;
            }
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            transform.position = newPosition;
        } else {
            if (this.transform.parent != null) return;
            checkpoint++;
            Destroy(gameObject);
            return;

            //UnityEngine.Debug.log("Collided");
            // new pos on ramen
            // egg 11.233/3.156/1.021
            // fish cake 11.152/3.201/1.088
            // menma 11.167/3.208/1.028
            // nori 11.036/3.241/1.028
            // Vector3 newPosition;
            // if (gameObject.name == egg) {
            //     newPosition = new Vector3(11.233f, 3.156f, 1.021f);
            // } else if (gameObject.name == fishCake) {
            //     newPosition = new Vector3(11.152f, 3.201f, 1.088f);
            // } else if (gameObject.name == menma) {
            //     newPosition = new Vector3(11.167f, 3.208f, 1.028f);
            // } else if (gameObject.name == nori) {
            //     newPosition = new Vector3(11.036f, 3.241f, 1.028f);
            // } else {
            //     return;
            // }
            // transform.position = newPosition;
        }
    }

    bool isRamen(string name) {
        return (name == ramen || name == ramenBowl || name == ramenComponent);
    }

}
