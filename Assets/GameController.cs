using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public Candy candy;

	public int columnNum = 10;

	public int rowNum = 7;

	public GameObject game;

	private ArrayList candyArr;
	// Use this for initialization
	void Start () {
		Debug.Log ("Game Start!");

		candyArr = new ArrayList ();

		for(int rowIndex = 0; rowIndex < rowNum; rowIndex++){
			ArrayList tmp = new ArrayList ();
			for (int columnIndex = 0; columnIndex < columnNum; columnIndex++) {
				Object o = Instantiate (candy); 
				Candy c = o as Candy;
				c.transform.parent = game.transform;
				c.columnIndex = columnIndex;
				c.rowIndex = rowIndex;
				c.UpdatePosition ();

				c.game = this;

				tmp.Add (c);
			}
			candyArr.Add (tmp);
		}
	}

	// Update is called once per frame
	void Update () {

	}

	//gets an element in a two-dimensional array
	private Candy GetCandy(int rowIndex, int columnIndex){
		ArrayList tmp = candyArr [rowIndex] as ArrayList;
		Candy c = tmp [columnIndex] as Candy;

		return c;
	}

	//sets an element in a two-dimensional array 
	private void SetCandy(int rowIndex, int columnIndex, Candy c){
		ArrayList tmp = candyArr [rowIndex] as ArrayList;
		tmp [columnIndex] = c;
	}

	//selected the first candy
	private Candy crt;
	//select candy
	public void Select(Candy c){
		Remove (c);return;

		if (crt == null) {	//first click
			crt = c;	//save 
			return;
		} else {		//second click 
			Exchange (crt, c);	//exchange two candy
			crt = null;	//set the first candy to null
		}
	}

	private void Exchange(Candy c1, Candy c2){
		int rowIndex = c1.rowIndex;
		c1.rowIndex = c2.rowIndex;
		c2.rowIndex = rowIndex;

		int columnIndex = c1.columnIndex;
		c1.columnIndex = c2.columnIndex;
		c2.columnIndex = columnIndex;

		c1.UpdatePosition ();
		c2.UpdatePosition (); 
	}

	//delete candy
	private void Remove(Candy c){
		int columnIndex = c.columnIndex;

		c.Dispose ();

		for(int rowIndex = c.rowIndex + 1; rowIndex < rowNum; rowIndex++){
			Candy c2 = GetCandy (rowIndex, columnIndex);
			c2.rowIndex--; 
			c2.UpdatePosition ();
			SetCandy (rowIndex - 1, columnIndex, c2);
		}
	}	
}
