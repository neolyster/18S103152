using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chess : MonoBehaviour {

	//四个锚点位置 计算落点位置
	public Transform LeftTop;
	public Transform RightTop;
	public Transform LeftBottom;
	public Transform RightBottm;

	private Vector3 LTPos;
	private Vector3 RTPos;
	private Vector3 LBPos;
	private Vector3 RBPos;
	private Vector3[,] ChessPos;
	private int[,] ChessState;//棋子的状态 0没棋子 1表示黑棋 -1表示白棋
	public GameObject ChosenOne;
	public GameObject CameraPoint;

	private Dictionary<GameObject,int> Location;
	private bool blackwin = false;
	private bool whitewin = false;

	public GameObject testCube;
	//棋子
	public GameObject blackQueen;
	public GameObject blackKing;
	public GameObject blackBishop;
	public GameObject blackKnight;
	public GameObject blackPawn;
	public GameObject blackRook;

	public GameObject whiteQueen;
	public GameObject whiteKing;
	public GameObject whiteBishop;
	public GameObject whiteKnight;
	public GameObject whitePawn;
	public GameObject whiteRook;

	//黑棋
	private GameObject blackleftrook;
	private GameObject blackleftknight;
	private GameObject blackleftbishop;
	private GameObject blackking;
	private GameObject blackqueen;
	private GameObject blackrightbishop;
	private GameObject blackrightknight;
	private GameObject blackrightrook;
	private GameObject blackpawn1;
	private GameObject blackpawn2;
	private GameObject blackpawn3;
	private GameObject blackpawn4;
	private GameObject blackpawn5;
	private GameObject blackpawn6;
	private GameObject blackpawn7;
	private GameObject blackpawn8;
	//白棋

	private GameObject whiteleftrook;
	private GameObject whiteleftknight;
	private GameObject whiteleftbishop;
	private GameObject whiteking;
	private GameObject whitequeen;
	private GameObject whiterightbishop;
	private GameObject whiterightknight;
	private GameObject whiterightrook;
	private GameObject whitepawn1;
	private GameObject whitepawn2;
	private GameObject whitepawn3;
	private GameObject whitepawn4;
	private GameObject whitepawn5;
	private GameObject whitepawn6;
	private GameObject whitepawn7;
	private GameObject whitepawn8;


	//棋子的长宽
	private float GridWidth = 1;
	private float GridHeight = 1;
	private float GridDis;

	public bool isPlaying = false;
	public bool Ui = false;

	private int[,] ChessStates;//棋子的位置

	public turn ChessTurn = turn.black;//落子顺序


	public enum turn
	{
		black,
		white,
	};

	public static Chess instance;
	
	// Use this for initialization
	void Start () {
		instance = this;
		ChessInit();
		CameraPoint = GameObject.Find("CameraPoint");
	}
	
	// Update is called once per frame
	void Update () {
		ChessUpdate();
		WinCheck();
	}

	public void ChessInit()
	{
		Location = new Dictionary<GameObject, int>();
		Location.Clear();

		isPlaying = true;

		LeftTop = transform.FindChild("LeftTop");

		RightTop = transform.FindChild("RightTop");
		LeftBottom = transform.FindChild("LeftBottom");
		RightBottm = transform.FindChild("RightBottom");

		ChessStates = new int[8, 8];
		ChessPos = new Vector3[8, 8];
		GridWidth = (RightTop.localPosition.x - LeftTop.localPosition.x) / 7;
		GridHeight = (LeftTop.localPosition.z - LeftBottom.localPosition.z) / 7;

		GridDis = GridWidth < GridHeight
			? GridWidth : GridHeight;
		for (int i = 0; i < 8; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				Vector3 pos = new Vector3();
				pos.x = LeftTop.localPosition.x + GridWidth * j;
				pos.y = 0;
				pos.z = LeftBottom.localPosition.z + GridHeight * i;
				//Debug.Log(pos);
				ChessPos[i, j] = pos;
			}
		}
		#region 分配
		blackleftrook = (GameObject)Instantiate(blackRook, this.transform);
		blackleftrook.transform.localPosition = ChessPos[0, 0];

		Location.Add(blackleftrook, 0);
		blackleftknight = (GameObject)Instantiate(blackKnight, this.transform);
		blackleftknight.transform.localPosition = ChessPos[0, 1];
		Location.Add(blackleftknight, 1);
		blackleftbishop = (GameObject)Instantiate(blackBishop, this.transform);
		blackleftbishop.transform.localPosition = ChessPos[0, 2];
		Location.Add(blackleftbishop, 2);
		blackking = (GameObject)Instantiate(blackKing, this.transform);
		blackking.transform.localPosition = ChessPos[0, 3];
		Location.Add(blackking, 3);
		blackqueen = (GameObject)Instantiate(blackQueen, this.transform);
		blackqueen.transform.localPosition = ChessPos[0, 4];
		Location.Add(blackqueen, 4);
		blackrightbishop = (GameObject)Instantiate(blackBishop, this.transform);
		blackrightbishop.transform.localPosition = ChessPos[0, 5];
		Location.Add(blackrightbishop, 5);
		blackrightknight = (GameObject)Instantiate(blackKnight, this.transform);
		blackrightknight.transform.localPosition = ChessPos[0, 6];
		Location.Add(blackrightknight, 6);
		blackrightrook = (GameObject)Instantiate(blackRook, this.transform);
		blackrightrook.transform.localPosition = ChessPos[0, 7];
		Location.Add(blackrightrook, 7);

		//白色
		whiteleftrook = (GameObject)Instantiate(whiteRook, this.transform);
		whiteleftrook.transform.localPosition = ChessPos[7, 0];
		Location.Add(whiteleftrook, 56);

		whiteleftknight = (GameObject)Instantiate(whiteKnight, this.transform);
		whiteleftknight.transform.localPosition = ChessPos[7, 1];
		Location.Add(whiteleftknight, 57);
		whiteleftknight.transform.eulerAngles = new Vector3(0, 180, 0);//转过头来
		whiteleftbishop = (GameObject)Instantiate(whiteBishop, this.transform);
		whiteleftbishop.transform.localPosition = ChessPos[7, 2];
		Location.Add(whiteleftbishop, 58);

		whiteking = (GameObject)Instantiate(whiteKing, this.transform);
		whiteking.transform.localPosition = ChessPos[7, 3];
		Location.Add(whiteking, 59);

		whitequeen = (GameObject)Instantiate(whiteQueen, this.transform);
		whitequeen.transform.localPosition = ChessPos[7, 4];
		Location.Add(whitequeen, 60);

		whiterightbishop = (GameObject)Instantiate(whiteBishop, this.transform);
		whiterightbishop.transform.localPosition = ChessPos[7, 5];
		Location.Add(whiterightbishop, 61);

		whiterightknight = (GameObject)Instantiate(whiteKnight, this.transform);
		whiterightknight.transform.localPosition = ChessPos[7, 6];
		whiterightknight.transform.eulerAngles = new Vector3(0, 180, 0);//转过头来
		Location.Add(whiterightknight, 62);

		whiterightrook = (GameObject)Instantiate(whiteRook, this.transform);
		whiterightrook.transform.localPosition = ChessPos[7, 7];
		Location.Add(whiterightrook, 63);

		blackpawn1 = (GameObject)Instantiate(blackPawn, this.transform);
		blackpawn1.transform.localPosition = ChessPos[1, 0];
		Location.Add(blackpawn1, 8);
		blackpawn2 = (GameObject)Instantiate(blackPawn, this.transform);
		blackpawn2.transform.localPosition = ChessPos[1, 1];
		Location.Add(blackpawn2, 9);
		blackpawn3 = (GameObject)Instantiate(blackPawn, this.transform);
		blackpawn3.transform.localPosition = ChessPos[1, 2];
		Location.Add(blackpawn3, 10);
		blackpawn4 = (GameObject)Instantiate(blackPawn, this.transform);
		blackpawn4.transform.localPosition = ChessPos[1, 3];
		Location.Add(blackpawn4, 11);
		blackpawn5 = (GameObject)Instantiate(blackPawn, this.transform);
		blackpawn5.transform.localPosition = ChessPos[1, 4];
		Location.Add(blackpawn5, 12);
		blackpawn6 = (GameObject)Instantiate(blackPawn, this.transform);
		blackpawn6.transform.localPosition = ChessPos[1, 5];
		Location.Add(blackpawn6, 13);
		blackpawn7 = (GameObject)Instantiate(blackPawn, this.transform);
		blackpawn7.transform.localPosition = ChessPos[1, 6];
		Location.Add(blackpawn7, 14);
		blackpawn8 = (GameObject)Instantiate(blackPawn, this.transform);
		blackpawn8.transform.localPosition = ChessPos[1, 7];
		Location.Add(blackpawn8, 15);

		whitepawn1 = (GameObject)Instantiate(whitePawn, this.transform);
		whitepawn1.transform.localPosition = ChessPos[6, 0];
		Location.Add(whitepawn1, 48);
		whitepawn2 = (GameObject)Instantiate(whitePawn, this.transform);
		whitepawn2.transform.localPosition = ChessPos[6, 1];
		Location.Add(whitepawn2, 49);
		whitepawn3 = (GameObject)Instantiate(whitePawn, this.transform);
		whitepawn3.transform.localPosition = ChessPos[6, 2];
		Location.Add(whitepawn3, 50);
		whitepawn4 = (GameObject)Instantiate(whitePawn, this.transform);
		whitepawn4.transform.localPosition = ChessPos[6, 3];
		Location.Add(whitepawn4, 51);
		whitepawn5 = (GameObject)Instantiate(whitePawn, this.transform);
		whitepawn5.transform.localPosition = ChessPos[6, 4];
		Location.Add(whitepawn5, 52);
		whitepawn6 = (GameObject)Instantiate(whitePawn, this.transform);
		whitepawn6.transform.localPosition = ChessPos[6, 5];
		Location.Add(whitepawn6, 53);
		whitepawn7 = (GameObject)Instantiate(whitePawn, this.transform);
		whitepawn7.transform.localPosition = ChessPos[6, 6];
		Location.Add(whitepawn7, 54);
		whitepawn8 = (GameObject)Instantiate(whitePawn, this.transform);
		whitepawn8.transform.localPosition = ChessPos[6, 7];
		Location.Add(whitepawn8, 55);

		#endregion
		#region 设置States状态

		for (int i = 0;i<2;i++)
		{
			for (int j = 0;j<8;j++)
			{
				ChessStates[i, j] = 1;
			}
		}

		for(int i =2;i<6;i++)
		{
			for (int j = 0; j < 8; j++)
			{
				ChessStates[i, j] = 0;
			}
		}

		for(int i = 6;i<8;i++)
		{
			for (int j = 0; j < 8; j++)
			{
				ChessStates[i, j] = -1;
			}
		}
		#endregion


	}

	public void ChessUpdate()
	{
		Move();
	}


	public void Move()//棋子的逻辑
	{
		if (ChosenOne == null)
			return;
		
		MouseClick();
			
		
			
	}
	public void MouseClick()
	{
		
		Vector3 hitPoint = Vector3.zero;
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Input.GetMouseButtonDown(0))
		{
			if(Physics.Raycast(ray,out hit,400f, 1 << LayerMask.NameToLayer("chess")))
			{
				if (hit.transform.gameObject.tag
					!= "chess")
					return;
				testCube.transform.SetParent(this.transform);
				testCube.transform.position = hit.point;
				//Debug.Log(testCube.transform.localPosition);
				for (int i = 0; i < 8; i++)
				{
					for (int j = 0; j < 8; j++)
					{
						if (Distance(testCube.transform.localPosition, ChessPos[i, j]) < GridDis / 2)
						{
							int index = Location[ChosenOne];
							int count = i * 8 + j;
							Dictionary<GameObject, int>.KeyCollection keyColl = Location.Keys;
							#region 处理逻辑



							if (ChosenOne.tag == "pawn")//小兵的逻辑
							{
								#region 小兵逻辑
								if (((count == Location[ChosenOne] + 9 || count == Location[ChosenOne] + 7) && ChessTurn == turn.black) || ((count == Location[ChosenOne] - 9 || count == Location[ChosenOne] - 7) && ChessTurn == turn.white))
								{
									Debug.Log(count);
									foreach (GameObject obj in keyColl)
									{
										if (Location[obj] == count)
										{
											Debug.Log(Location[obj]);
											if ((int)obj.GetComponent<chessman>().ColorChess != (int)ChessTurn)
											{
												if (count / 8 == 0 || count / 8 == 9)
													ChosenOne.tag = "queen";
												Location.Remove(obj);
												Destroy(obj);
												ChosenOne.transform.localPosition = ChessPos[i, j];
												Location[ChosenOne] = count;

												ChosenOne.GetComponent<chessman>().Reset();
												ChosenOne = null;

												ChessTurn = ChessTurn == turn.black ? turn.white : turn.black;
												return;
											}
											else
											{
												return;
											}


										}

									}





								}
								else
								if ((count == Location[ChosenOne] + 8 && ChessTurn == turn.black) || (count == Location[ChosenOne] - 8 && ChessTurn == turn.white))
								{

									foreach (GameObject obj in keyColl)
									{
										if (Location[obj] == count)
										{


											return;

										}

									}

									ChosenOne.transform.localPosition = ChessPos[i, j];
									Location[ChosenOne] = count;

									if (count / 8 == 0 || count / 8 == 9)
										ChosenOne.tag = "queen";

									ChessTurn = ChessTurn == turn.black ? turn.white : turn.black;




									ChosenOne.GetComponent<chessman>().Reset();
									ChosenOne = null;



								}


								#endregion




							}

							else if (ChosenOne.tag == "knight")//骑士逻辑
							{
								#region
								if (count == Location[ChosenOne] + 10 || count == Location[ChosenOne] + 6 || count == Location[ChosenOne] + 17 || count == Location[ChosenOne] + 15 || count == Location[ChosenOne] - 6 || count == Location[ChosenOne] - 10 || count == Location[ChosenOne] - 15 || count == Location[ChosenOne] - 17)
								{
									Debug.Log(count);
									foreach (GameObject obj in keyColl)
									{
										if (Location[obj] == count)
										{

											if ((int)obj.GetComponent<chessman>().ColorChess != (int)ChessTurn)
											{
												Location.Remove(obj);
												Destroy(obj);
												ChosenOne.transform.localPosition = ChessPos[i, j];
												Location[ChosenOne] = count;

												ChosenOne.GetComponent<chessman>().Reset();
												ChosenOne = null;

												ChessTurn = ChessTurn == turn.black ? turn.white : turn.black;
												return;
											}
											else
											{
												return;
											}


										}

									}
									ChosenOne.transform.localPosition = ChessPos[i, j];
									Location[ChosenOne] = count;

									ChosenOne.GetComponent<chessman>().Reset();
									ChosenOne = null;

									ChessTurn = ChessTurn == turn.black ? turn.white : turn.black;
									return;







								}
								#endregion


							}
							else if (ChosenOne.tag == "queen")
							{
								#region  王后逻辑
								int num = count - Location[ChosenOne];
								if (num != 0)
								{
									if (num < 0)
										num = -num;
									if((count % 8 == Location[ChosenOne] % 8) || (count / 8 == Location[ChosenOne] / 8)|| (num % 9 == 0 )||( num % 7 == 0))
									{
										foreach (GameObject obj in keyColl)
										{
											if (Location[obj] == count)
											{

												if ((int)obj.GetComponent<chessman>().ColorChess != (int)ChessTurn)
												{
													Location.Remove(obj);
													Destroy(obj);
													ChosenOne.transform.localPosition = ChessPos[i, j];
													Location[ChosenOne] = count;

													ChosenOne.GetComponent<chessman>().Reset();
													ChosenOne = null;

													ChessTurn = ChessTurn == turn.black ? turn.white : turn.black;
													return;
												}
												else
												{
													return;
												}


											}
										}
										ChosenOne.transform.localPosition = ChessPos[i, j];
										Location[ChosenOne] = count;

										ChosenOne.GetComponent<chessman>().Reset();
										ChosenOne = null;

										ChessTurn = ChessTurn == turn.black ? turn.white : turn.black;
									}

								}

								#endregion



							}
							else if (ChosenOne.tag == "king")
							{
								#region 国王逻辑

								if (count == Location[ChosenOne] + 1 || count == Location[ChosenOne] + 8 || count == Location[ChosenOne] - 1 || count == Location[ChosenOne] - 8 || count == Location[ChosenOne] + 7 || count == Location[ChosenOne] + 9 || count == Location[ChosenOne] - 7 || count == Location[ChosenOne] - 9)
								{
									Debug.Log(count);
									foreach (GameObject obj in keyColl)
									{
										if (Location[obj] == count)
										{

											if ((int)obj.GetComponent<chessman>().ColorChess != (int)ChessTurn)
											{
												Location.Remove(obj);
												Destroy(obj);
												ChosenOne.transform.localPosition = ChessPos[i, j];
												Location[ChosenOne] = count;

												ChosenOne.GetComponent<chessman>().Reset();
												ChosenOne = null;

												ChessTurn = ChessTurn == turn.black ? turn.white : turn.black;
												return;
											}
											else
											{
												return;
											}


										}

									}
									ChosenOne.transform.localPosition = ChessPos[i, j];
									Location[ChosenOne] = count;

									ChosenOne.GetComponent<chessman>().Reset();
									ChosenOne = null;

									ChessTurn = ChessTurn == turn.black ? turn.white : turn.black;
									return;

								}
								#endregion
							}
							else if (ChosenOne.tag == "rook")
							{
								#region 车逻辑
								int num = count - Location[ChosenOne];
								if (num != 0)
								{
									if ((count % 8 == Location[ChosenOne] % 8) || (count / 8 == Location[ChosenOne] / 8))
									{
										foreach (GameObject obj in keyColl)
										{
											if (Location[obj] == count)
											{

												if ((int)obj.GetComponent<chessman>().ColorChess != (int)ChessTurn)
												{
													Location.Remove(obj);
													Destroy(obj);
													ChosenOne.transform.localPosition = ChessPos[i, j];
													Location[ChosenOne] = count;

													ChosenOne.GetComponent<chessman>().Reset();
													ChosenOne = null;

													ChessTurn = ChessTurn == turn.black ? turn.white : turn.black;
													return;
												}
												else
												{
													return;
												}


											}
										}
										ChosenOne.transform.localPosition = ChessPos[i, j];
										Location[ChosenOne] = count;

										ChosenOne.GetComponent<chessman>().Reset();
										ChosenOne = null;

										ChessTurn = ChessTurn == turn.black ? turn.white : turn.black;
										return;



									}
								}
								#endregion

							}
							else if (ChosenOne.tag == "bishop")
							{
								#region 主教逻辑
								int num = count - Location[ChosenOne];
								if (num != 0)
								{
									if (num < 0)
										num = -num;
									if(num%9==0||num%7==0)
									{
										Debug.Log("here");
										foreach (GameObject obj in keyColl)
										{
											if (Location[obj] == count)
											{

												if ((int)obj.GetComponent<chessman>().ColorChess != (int)ChessTurn)
												{
													Location.Remove(obj);
													Destroy(obj);
													ChosenOne.transform.localPosition = ChessPos[i, j];
													Location[ChosenOne] = count;

													ChosenOne.GetComponent<chessman>().Reset();
													ChosenOne = null;

													ChessTurn = ChessTurn == turn.black ? turn.white : turn.black;
													return;
												}
												else
												{
													return;
												}


											}
										}
										ChosenOne.transform.localPosition = ChessPos[i, j];
										Location[ChosenOne] = count;

										ChosenOne.GetComponent<chessman>().Reset();
										ChosenOne = null;

										ChessTurn = ChessTurn == turn.black ? turn.white : turn.black;
									}
								}


								#endregion

							}
							#endregion
						}

					}
				}
			}
		}
	}
	


	public float Distance(Vector3 pos1,Vector3 pos2)//计算距离
	{
		return Mathf.Sqrt(Mathf.Pow(pos1.x - pos2.x, 2) + Mathf.Pow(pos1.z - pos2.z, 2));
	}

	
	

	public void QueenCtrl()
	{

	}

	public void RookCtrl()
	{

	}

	public void OnGUI()
	{
		if(Ui)
		{
			if (GUILayout.Button("End", GUILayout.Width(200), GUILayout.Height(50)))
			{
				////PanelMgr.instance.OpenPanel<TitlePanel>("");
				//Camera.main.transform.position = CameraPoint.transform.position;
				//Camera.main.transform.rotation = CameraPoint.transform.rotation;
				Application.Quit();
			}
			if (ChessTurn == Chess.turn.black)
			{
				GUILayout.Label("Black", GUILayout.Width(200), GUILayout.Height(50));
			}
			if (ChessTurn == Chess.turn.white)
			{
				GUILayout.Label("White", GUILayout.Width(200), GUILayout.Height(50));
			}
			if(blackwin)
				GUILayout.Label("BlackWin", GUILayout.Width(200), GUILayout.Height(50));
			if(whitewin)
				GUILayout.Label("BlackWin", GUILayout.Width(200), GUILayout.Height(50));

		}
	}
	public void WinCheck()
	{
		
		int count = 0;
		Dictionary<GameObject, int>.KeyCollection keyColl = Location.Keys;
		foreach(GameObject obj in keyColl)
		{
			if(obj.tag=="king")
			{
				count++;
				return;
			}
		}
		if (count == 1)
		{
			foreach (GameObject obj in keyColl)
			{
				if (obj.tag == "king")
				{
					if (obj.GetComponent<chessman>().ColorChess == chessman.ChessColor.black)
						blackwin = true;
					if (obj.GetComponent<chessman>().ColorChess == chessman.ChessColor.white)
						whitewin = true;
				}
			}
		}
	}
	


	//public void KnightCtrl(int row,int col)
	//{

	//	if (((count == Location[ChosenOne] + 9 || count == Location[ChosenOne] + 7) && ChessTurn == turn.black) || ((count == Location[ChosenOne] - 9 || count == Location[ChosenOne] - 7) && ChessTurn == turn.white))
	//	{
	//		Debug.Log(count);
	//		foreach (GameObject obj in keyColl)
	//		{
	//			if (Location[obj] == count)
	//			{
	//				Debug.Log(Location[obj]);
	//				if ((int)obj.GetComponent<chessman>().ColorChess != (int)ChessTurn)
	//				{
	//					if (count / 8 == 0 || count / 8 == 9)
	//						ChosenOne.tag = "queen";
	//					Location.Remove(obj);
	//					Destroy(obj);
	//					ChosenOne.transform.localPosition = ChessPos[i, j];
	//					Location[ChosenOne] = count;

	//					ChosenOne.GetComponent<chessman>().Reset();
	//					ChosenOne = null;

	//					ChessTurn = ChessTurn == turn.black ? turn.white : turn.black;
	//					return;
	//				}


	//			}

	//		}





	//	}
	//	else
	//	if ((count == Location[ChosenOne] + 8 && ChessTurn == turn.black) || (count == Location[ChosenOne] - 8 && ChessTurn == turn.white))
	//	{

	//		foreach (GameObject obj in keyColl)
	//		{
	//			if (Location[obj] == count)
	//			{


	//				return;

	//			}

	//		}

	//		ChosenOne.transform.localPosition = ChessPos[i, j];
	//		Location[ChosenOne] = count;

	//		if (count / 8 == 0 || count / 8 == 9)
	//			ChosenOne.tag = "queen";

	//		ChessTurn = ChessTurn == turn.black ? turn.white : turn.black;




	//		ChosenOne.GetComponent<chessman>().Reset();
	//		ChosenOne = null;



	//	}

	//}

}
