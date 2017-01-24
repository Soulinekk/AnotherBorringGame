using UnityEngine;
using System.Collections;

public class CardInfo : MonoBehaviour {

        public GameObject imageFrame;
        public GameObject nameFrame;

        public int _id_shopCard;
        public string _name;    //probably not needed
        public int _price;      //probably not needed

    void Awake ()
    {
        imageFrame = transform.FindChild("Sprite").gameObject;
        nameFrame = transform.FindChild("Title").gameObject;
    }

    public void OnCardClick() {
        GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>().LoadSelectedCardShop(_id_shopCard);
    }
}
