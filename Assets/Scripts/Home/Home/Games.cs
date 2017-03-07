using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Games : MonoBehaviour { 
	void Start ()
	{
	    GridLayoutGroup group = GetComponent<GridLayoutGroup>();
	    int size = (Screen.width - 30)/3;
        group.cellSize= new Vector2(size, size);
	}
}
