using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour {

	public int rowIndex = 0;
	public int columnIndex = 0;
	public float xOff = -4.5f;
	public float yoff = -3f;

	public GameObject[] bgs;
	private GameObject bg;

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
		type = Random.Range (0, bgs.Length);
		bg = (GameObject)Instantiate (bgs[type]);
		bg.transform.parent = this.transform;
	}

	void OnMouseDown(){
		//Debug.Log ("type:" + type);
		game.Select(this);
	}

	public void UpdatePosition(){
		
		AddRandomBG ();

		transform.position = new Vector3 (columnIndex + xOff, rowIndex + yoff, 0f);
	}

	public void Dispose(){
		game = null;

		Destroy (bg.gameObject);

		Destroy (this.gameObject);
	}
}
