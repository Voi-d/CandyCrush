using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public Candy candy;

	public int columnNum = 10;

	public int rowNum = 7;

	public GameObject game;

	public AudioClip swapClip;
	public AudioClip explodeClip;
	public AudioClip match3Clip;

	private ArrayList candyArr;
	private ArrayList matches;
	// Use this for initialization
	void Start () {
		Debug.Log ("Game Start!");

		candyArr = new ArrayList ();
		matches = new ArrayList ();

		for(int rowIndex = 0; rowIndex < rowNum; rowIndex++){
			ArrayList tmp = new ArrayList ();
			for (int columnIndex = 0; columnIndex < columnNum; columnIndex++) {
				Candy c = AddCandy (rowIndex, columnIndex);

				tmp.Add (c);
			}
			candyArr.Add (tmp);
		}

		//first check
		if (CheckMatches()) {
			RemoveMatches ();
		}
	}

	// Update is called once per frame
	void Update () {

	}

	private Candy AddCandy(int rowIndex, int columnIndex){
		Object o = Instantiate (candy); 
		Candy c = o as Candy;
		c.transform.parent = game.transform;
		c.columnIndex = columnIndex;
		c.rowIndex = rowIndex;
		c.UpdatePosition ();

		c.game = this;

		return c;
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

		if (crt == null) {	//first click
			crt = c;	//save 
			return;
		} else {		//second click 
			//exchange conditions
			if(1 == (Mathf.Abs(crt.rowIndex - c.rowIndex) + Mathf.Abs(crt.columnIndex - c.columnIndex))){
				StartCoroutine (ExchangeThread (crt, c)); 
			}

			crt = null;	//set the first candy to null
		}
	}

	IEnumerator ExchangeThread(Candy c1, Candy c2){
		Exchange (c1, c2);	//exchange two candy

		//wait for 0.4s
		yield return new WaitForSeconds(0.4f);

		if (CheckMatches ()) {
			RemoveMatches ();
		} else {
			Exchange (c1, c2);
		}
	}

	private void Exchange(Candy c1, Candy c2){
		StartCoroutine (PlaySoundThread(swapClip)); 

		SetCandy (c1.rowIndex, c1.columnIndex, c2);
		SetCandy (c2.rowIndex, c2.columnIndex, c1);

		int rowIndex = c1.rowIndex;
		c1.rowIndex = c2.rowIndex;
		c2.rowIndex = rowIndex;

		int columnIndex = c1.columnIndex;
		c1.columnIndex = c2.columnIndex;
		c2.columnIndex = columnIndex;

		//c1.UpdatePosition ();
		//c2.UpdatePosition (); 
		c1.TweenToPosition ();
		c2.TweenToPosition ();
	}

	//delete candy
	private void Remove(Candy c){
		int columnIndex = c.columnIndex;

		//remove
		c.Dispose ();
		//move up candy down
		for(int rowIndex = c.rowIndex + 1; rowIndex < rowNum; rowIndex++){
			Candy c2 = GetCandy (rowIndex, columnIndex);
			c2.rowIndex--; 
			c2.TweenToPosition ();
			SetCandy (rowIndex - 1, columnIndex, c2);
		}

		//add new candy to top 
		Candy newC = AddCandy(rowNum, columnIndex);
		newC.rowIndex--;
		newC.TweenToPosition ();
		SetCandy (rowNum - 1, columnIndex, newC);
	}

	//check the same candy
	private bool CheckMatches(){
		bool isHorizontalMatch = CheckHorizontalMatches ();
		bool iskVerticalMatch = CheckVerticalMatches ();
		bool isMatch = isHorizontalMatch || iskVerticalMatch;

		if(isMatch){
			StartCoroutine (PlaySoundThread(match3Clip)); 
		}

		return isMatch;
	}

	IEnumerator PlaySoundThread(AudioClip ac){
		yield return new WaitForSeconds(0.0f);
		this.GetComponent<AudioSource> ().PlayOneShot(ac);
	}


	//the same candy in the horizontal direction
	private bool CheckHorizontalMatches(){
		bool isMatch = false;

		for (int rowIndex = 0; rowIndex < rowNum; rowIndex++) {
			int columnIndex = 0, offset = 0, count = 1;
			while(columnIndex < columnNum - 1)  {
				offset = 0;
				count = 1;

				while ((columnIndex + offset < columnNum - 1) && (GetCandy (rowIndex, columnIndex + offset).type == GetCandy (rowIndex, columnIndex + offset + 1).type)) {
					count++;
					offset++;
				}
				if (count >= 3) {
					//Debug.Log ("matchNum:" + count);
					isMatch = true;
					for (int i = columnIndex; i < columnIndex + count; i++) {
						AddMatches (GetCandy(rowIndex, i));
					}
				}

				columnIndex += count;	//make sure not to go back
			}
		}
			
		return isMatch;
	}

	//the same candy in the vertical direction
	private bool CheckVerticalMatches(){
		bool isMatch = false;

		for (int columnIndex = 0; columnIndex < columnNum; columnIndex++) {
			int rowIndex = 0, offset = 0, count = 1;
			while(rowIndex < rowNum - 1)  {
				offset = 0;
				count = 1;

				while ((rowIndex + offset < rowNum - 1) && (GetCandy (rowIndex + offset, columnIndex).type == GetCandy (rowIndex + offset + 1, columnIndex).type)) {
					count++;
					offset++;
				}
				if (count >= 3) {
					//Debug.Log ("matchNum:" + count);
					isMatch = true;
					for (int i = rowIndex; i < rowIndex + count; i++) {
						AddMatches (GetCandy(i, columnIndex));
					}
				}

				rowIndex += count;	//make sure not to go back
			}
		}

		return isMatch;
	}

	private void AddMatches(Candy c){
		int index = matches.IndexOf (c);

		if(-1 == index){
			matches.Add (c);
		}
	}

	private void RemoveMatches(){
		Candy tmp;

		for(int i = 0; i < matches.Count; i++){
			tmp = matches [i] as Candy;
			Remove (tmp);
		}

		matches = new ArrayList ();

		//recursive call
		if (CheckMatches()) {
			RemoveMatches ();
		}
	}
}
