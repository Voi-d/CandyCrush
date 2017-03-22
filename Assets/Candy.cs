using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour {

	public int rowIndex = 0;
	public int columnIndex = 0;
	public float xoff = -4.5f;
	public float yoff = -3f;

	public GameObject[] bgs;
	private GameObject bg;
	public int candyTypeNum = 6;	//control the game difficult
	public int type;

	public GameController game;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void AddRandomBG(){
		if (bg != null) {
			return;
		}
		type = Random.Range (0, Mathf.Min(candyTypeNum, bgs.Length));
		bg = (GameObject)Instantiate (bgs[type]);
		bg.transform.parent = this.transform;
	}

	void OnMouseDown(){
		//Debug.Log ("type:" + type);
		game.Select(this);
	}

	public void UpdatePosition(){
		
		AddRandomBG ();

		transform.position = new Vector3 (columnIndex + xoff, rowIndex + yoff, 0f);
	}

	//move slowly to the appropriate location
	public void TweenToPosition(){
		AddRandomBG ();
		iTween.MoveTo (this.gameObject, iTween.Hash("x", columnIndex + xoff, "y", rowIndex + yoff, "time", 0.3f));
	}

	public void Dispose(){
		game = null;

		Destroy (bg.gameObject);

		Destroy (this.gameObject);
	}
}
