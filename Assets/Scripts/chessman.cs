using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chessman : MonoBehaviour {

	public enum ChessColor
	{
		black,
		white,
	}
	private static bool Chosen = false;
	public ChessColor ColorChess;

	

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(1))
		{
			Chosen = false;
		}
		
	}
	public void OnMouseEnter()
	{
		
		if (!Chosen)
		{
			if (Chess.instance.ChessTurn == Chess.turn.black&&ColorChess == ChessColor.black)
			{
				this.gameObject.GetComponent<Renderer>().material.color = Color.red;
			}else if (Chess.instance.ChessTurn == Chess.turn.white && ColorChess == ChessColor.white)
			{
				this.gameObject.GetComponent<Renderer>().material.color = Color.red;
			}



		}
	}
	public void OnMouseExit()
	{
		if (!Chosen)
		{
			if (ColorChess == ChessColor.black)
			{
				this.gameObject.GetComponent<Renderer>().material.color = Color.black;
			}
			if (ColorChess == ChessColor.white)
				this.gameObject.GetComponent<Renderer>().material.color = Color.white;
		}
	}
	public void OnMouseDown()
	{


		if (!Chosen&&(int)this.ColorChess==(int)Chess.instance.ChessTurn)
		{
			
				Chosen = true;
				Chess.instance.ChosenOne = this.gameObject;
			
		}
		else if (Chosen)
		{
			if (Chess.instance.ChosenOne == this.gameObject)
			{
				Chosen = false;
				Chess.instance.ChosenOne = null;
			}
		}
	}

	public void Reset()
	{
		Chosen = false;
		if (ColorChess == ChessColor.black)
		{
			this.gameObject.GetComponent<Renderer>().material.color = Color.black;
		}
		if (ColorChess == ChessColor.white)
			this.gameObject.GetComponent<Renderer>().material.color = Color.white;
	}

	
}
