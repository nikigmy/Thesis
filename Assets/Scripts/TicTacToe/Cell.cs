using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Cell : MonoBehaviour {


    public Sprite PlayerOneImage;
    public Sprite PlayerTwoImage;
    private Button button;
    private Image image;

    private bool interactable;

    public Main.Player taken;

    void Start ()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        interactable = true;
        taken = Main.Player.None;
    }

    public bool PlaceTurn(Main.Player player)
    {
        if (interactable && taken == Main.Player.None)
        {
            if (player == Main.Player.PlayerOne)
            {
                image.sprite = PlayerOneImage;
                taken = Main.Player.PlayerOne;
                interactable = button.interactable = false;
            }
            else
            {
                image.sprite = PlayerTwoImage;
                taken = Main.Player.PlayerTwo;
                interactable = button.interactable = false;
            }
            return true;
        }
        else return false;
    }

	// Update is called once per frame
	void Update () {
	    
	}
}
