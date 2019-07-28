using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class five : MonoBehaviour {

	//四个锚点位置 计算落点位置
	public Transform LeftTop;
	public Transform RightTop;
	public Transform LeftBottom;
	public Transform RightBottm;
	


	Vector3 PointPos;//当前选择的位置

	private Vector3 LTPos;
	private Vector3 RTPos;
	private Vector3 LBPos;
	private Vector3 RBPos;

	//棋子的长宽
	private float GridWidth = 1;
	private float GridHeight = 1;

	//黑白棋子
	public Texture2D BlackChess;

	public Texture2D WhiteChess;

	public static five instance;//单例模式

	private float GridDis;//判断属于哪个棋子的落点

	int[,] ChessState;//棋盘上的落子状态 0没棋子 1 黑子 -1 白子

	private Vector2 [,] ChessPos;//棋子落点的位置

	bool isPlaying = false;//检测是否是下棋状态 
    public	bool Ui = false;
	public turn ChessTurn = turn.black;//落子顺序
	public GameObject CameraPoint;

	public enum turn {
		black,
		white,
	};
	// Use this for initialization
	void Start () {
		instance = this;
		//FiveInit();
		CameraPoint = GameObject.Find("CameraPoint");
	}
	
	// Update is called once per frame
	void Update () {
		FiveUpdate();
	}

	private void OnGUI()//绘制棋子
	{

		if (Ui)
		{
			if (GUILayout.Button("End", GUILayout.Width(200), GUILayout.Height(50)))
			{
				Clear();
			}

			if (ChessTurn == five.turn.black)
			{
				GUILayout.Label("Black", GUILayout.Width(200), GUILayout.Height(50));
			}
			if (ChessTurn == five.turn.white)
			{
				GUILayout.Label("White", GUILayout.Width(200), GUILayout.Height(50));
			}
			for (int i = 0; i < 19; i++)
			{
				for (int j = 0; j < 19; j++)
				{
					if (ChessState[i, j] == 1)
					{
						GUI.DrawTexture(new Rect(ChessPos[i, j].x - GridWidth / 2, Screen.height - ChessPos[i, j].y - GridHeight / 2, GridWidth, GridHeight), BlackChess);
					}
					if (ChessState[i, j] == -1)
					{
						GUI.DrawTexture(new Rect(ChessPos[i, j].x - GridWidth / 2, Screen.height - ChessPos[i, j].y - GridHeight / 2, GridWidth, GridHeight), WhiteChess);
					}


				}
			}
		}
	}

	public void FiveInit()//初始化棋盘
	{
		ChessPos = new Vector2[19, 19];
		ChessState = new int[19, 19];
		

		isPlaying = true;

		LeftTop = transform.FindChild("LeftTop");
		RightTop = transform.FindChild("RightTop");
		LeftBottom = transform.FindChild("LeftBottom");
		RightBottm = transform.FindChild("RightBottom");

		LTPos = Camera.main.WorldToScreenPoint(LeftTop.position);
		RTPos = Camera.main.WorldToScreenPoint(RightTop.position);
		LBPos = Camera.main.WorldToScreenPoint(LeftBottom.position);
		RBPos = Camera.main.WorldToScreenPoint(RightBottm.position);

		GridWidth = (RTPos.x - LTPos.x) / 18;
		GridHeight = (LTPos.y - LBPos.y) / 18;

		GridDis = GridWidth < GridHeight ? GridWidth : GridHeight;
		//Debug.Log("DIS:" + GridDis);

		for (int i =0;i<19;i++)//将棋子的位置保存起来
		{
			for(int j =0;j<19;j++)
			{
				ChessPos[i, j] = new Vector2(LBPos.x + GridWidth * j,  LBPos.y + GridHeight * i );//两者坐标系不同 需要转换
				ChessState[i, j] = 0;
				//Debug.Log(ChessPos[i, j]);
			}
		}


	}

	public void FiveUpdate()//每一帧的逻辑
	{


		if (isPlaying && Input.GetMouseButtonDown(0))
		{
			Debug.Log("here");
			Vector3 pos = Input.mousePosition;
			//Debug.Log(pos);
			for (int i = 0; i < 19; i++)
			{
				for (int j = 0; j < 19; j++)
				{
					//找到最接近鼠标点击位置的落子点，如果空则落子
						
					if (Dis(pos, ChessPos[i, j]) < GridDis/2  && ChessState[i, j] == 0)
					{
						//Debug.Log(i + "行");
						//Debug.Log(j + "列");
						int color = 0;
						//Debug.Log("here1");
						//根据下棋顺序确定落子颜色
						if(ChessTurn == turn.black)
						{
							ChessState[i, j] = 1;
							color = 1;
						}else
						{
							ChessState[i, j] = -1;
							color = -1;
						}
						//List<int> Cur = FindSameColor(i, j, color);
						//bool test = Eat(ref Cur,color);
						//Debug.Log(test);

						DoEat(i, j);
						//落子成功，更换下棋顺序
						ChessTurn = ChessTurn == turn.black ? turn.white : turn.black;

					}
				}
			}
		}

	}

	public float Dis(Vector3 pos1,Vector2 gridpos)//距离
	{
		return Mathf.Sqrt(Mathf.Pow(pos1.x - gridpos.x, 2) + Mathf.Pow(pos1.y - gridpos.y, 2));
	}

	public int BlackisWin()//判断胜负 五子棋的废弃不用
	{
		int flag = 0;//0代表没下完 1代表黑胜 -1代表白胜 
		if(ChessTurn == turn.black)
		{
			for(int i = 0;i < 11;i++)
			{
				for(int j =0;j<15;j++)
				{
					if (j < 4)
					{
						if (ChessState[i, j] == -1 && ChessState[i, j + 1] == -1 && ChessState[i, j + 2] == -1 && ChessState[i, j + 3] == -1 && ChessState[i, j + 4] == -1)
						{//横向
							flag = -1;
							return flag;
						}

						if (ChessState[i, j] == -1 && ChessState[i + 1, j] == -1 && ChessState[i + 2, j] == -1 && ChessState[i + 3, j] == -1 && ChessState[i + 4, j] == -1)
						{//纵向
							flag = -1;
							return flag;
						}

						if (ChessState[i, j] == -1 && ChessState[i + 1, j + 1] == -1 && ChessState[i + 2, j + 2] == -1 && ChessState[i + 3, j + 3] == -1 && ChessState[i + 4, j + 4] == -1)
						{//右上
							flag = -1;
							return flag;
						}
					}else if(j>=4&&j<11)
					{
						if (ChessState[i, j] == -1 && ChessState[i, j + 1] == -1 && ChessState[i, j + 2] == -1 && ChessState[i, j + 3] == -1 && ChessState[i, j + 4] == -1)
						{//横向
							flag = -1;
							return flag;
						}

						if (ChessState[i, j] == -1 && ChessState[i + 1, j] == -1 && ChessState[i + 2, j] == -1 && ChessState[i + 3, j] == -1 && ChessState[i + 4, j] == -1)
						{//纵向
							flag = -1;
							return flag;
						}

						if (ChessState[i, j] == -1 && ChessState[i + 1, j + 1] == -1 && ChessState[i + 2, j + 2] == -1 && ChessState[i + 3, j + 3] == -1 && ChessState[i + 4, j + 4] == -1)
						{//右上
							flag = -1;
							return flag;
						}
						if (ChessState[i, j] == -1 && ChessState[i + 1, j - 1] == -1 && ChessState[i + 2, j - 2] == -1 && ChessState[i + 3, j - 3] == -1 && ChessState[i + 4, j - 4] == -1)
						{//右上
							flag = -1;
							return flag;
						}
					}else
					{

					}


				}
			}

		}else if(ChessTurn == turn.white)
		{
			
		}


		return flag;
		
	}

	/*public void Eat(int i,int j)//判断是否有吃子
	{
		List<int> neghStatck = new List<int>();//存储落下棋子附近不同颜色的棋子
		Queue<int> neghQueue = new Queue<int>();
		int index;//index = i*19+j
		int CheckState = 0;//下的是黑子就先判断是否有白棋被吃 下白子则相反
		if(ChessTurn == turn.black)
		{
			CheckState = -1;

		}
		else if(ChessTurn == turn.white)
		{
			CheckState = 1;
		}
		int left = j - 1;
		int right = j + 1;
		int top = i - 1;
		int bottom = i + 1;
		if(left>=0&&ChessState[i,left]==CheckState)
		{
			List<int> LeftNegh = new List<int>();
			LeftNegh = FindSameColor(i, left, CheckState);
		}
		if(right<=18&& ChessState[i, right] == CheckState)
		{
			List<int> RightNegh = new List<int>();
			RightNegh = FindSameColor(i, right, CheckState);
		}
		if(top>=0&& ChessState[top, j] == CheckState)
		{
			List<int> TopNegh = new List<int>();
			TopNegh = FindSameColor(top, j, CheckState);
		}
		if(bottom<=18 && ChessState[bottom, j] == CheckState)
		{
			List<int> BottomNegh = new List<int>();
			BottomNegh = FindSameColor(bottom, j,CheckState);
		}

	}*/

	public bool Eat(ref List<int> listPos,int color)//判断这一部分是否被吃掉
	{
		List<int> listCur = new List<int>();
		if (listPos == null)
			return false;
		color = -color;//颜色变化
		foreach(int num in listPos)
		{
			int row = num / 19 - 1;
			int col = num % 19 - 1;
			int left = col - 1;
			int right = col + 1;
			int top = row + 1;
			int bottom = row - 1;
			if (left >= 0 )
			{
				//Debug.Log("left");
				//Debug.Log(ChessState[i, left]);
				//Debug.Log(row+"行");
				//Debug.Log(col + "列");
				int newNum = (row + 1) * 19 + left + 1;
				if (!listPos.Contains(newNum))
				{

					listCur.Add(newNum);
				}

			}
			if (right <= 18 )
			{
				//Debug.Log("right");
				//Debug.Log(ChessState[i, right]);
				//Debug.Log(row + "行");
				//Debug.Log(col + "列");
				int newNum = (row + 1) * 19 + right + 1;
				
				if (!listPos.Contains(newNum))
				{
					//Debug.Log("right+");
					//int newNum = (i + 1) * 19 + right + 1;
					listCur.Add(newNum);
				}

			}
			if (top >= 0 )
			{

				int newNum = (top + 1) * 19 + col + 1;
				if (!listPos.Contains(newNum))
				{
					//Debug.Log("right+");
					//int newNum = (i + 1) * 19 + right + 1;
					listCur.Add(newNum);
				}
			}
			if (bottom <= 18)
			{

				int newNum = (bottom + 1) * 19 + col + 1;
				if (!listPos.Contains(newNum))
				{
					//Debug.Log("right+");
					//int newNum = (i + 1) * 19 + right + 1;
					listCur.Add(newNum);
				}
			}

		}

		foreach(int num in listCur)
		{
			int row = num / 19 - 1;
			int col = num % 19 - 1;
			if(ChessState[row,col]!= color)
			{
				return false;
			}
		}
		return true;
	}

	public void DoEat(int row,int col)
	{

		int left = col - 1;
		int right = col + 1;
		int top = row + 1;
		int bottom = row - 1;

		if (left >= 0)
		{
			int color1 = ChessState[row, left];
			List<int> l1 = FindSameColor(row, left, color1);
			if (Eat(ref l1, color1))
			{
				foreach (int num in l1)
				{
					int i = num / 19 - 1;
					int j = num % 19 - 1;
					ChessState[i, j] = 0;
				}
			}
		}
		if (right <= 18)
		{
			int color2 = ChessState[row, right];
			List<int> l2 = FindSameColor(row, right, color2);

			if (Eat(ref l2, color2))
			{
				foreach (int num in l2)
				{
					int i = num / 19 - 1;
					int j = num % 19 - 1;
					ChessState[i, j] = 0;
				}
			}
		}
		if (top <= 18)
		{
			int color3 = ChessState[top, col];
			List<int> l3 = FindSameColor(top, col, color3);

			if (Eat(ref l3, color3))
			{
				foreach (int num in l3)
				{
					int i = num / 19 - 1;
					int j = num % 19 - 1;
					ChessState[i, j] = 0;
				}
			}

		}
		if (bottom >= 0)
		{
			int color4 = ChessState[bottom, col];
			List<int> l4 = FindSameColor(bottom, col, color4);
			if (Eat(ref l4, color4))
			{
				foreach (int num in l4)
				{
					int i = num / 19 - 1;
					int j = num % 19 - 1;
					ChessState[i, j] = 0;
				}
			}
		}


		int color5 = ChessState[row, col];
		List<int> l5 = FindSameColor(row, col, color5);

		
	
		
		
		if (Eat(ref l5, color5))
		{
			foreach (int num in l5)
			{
				int i = num / 19 - 1;
				int j = num % 19 - 1;
				ChessState[i, j] = 0;
			}
		}


	}

	public List<int> FindSameColor(int i,int j,int chessState)//寻找同样颜色的棋子
	{
		//Debug.Log("i:" + i);
		//Debug.Log("j:" + j);
		if (chessState == 0)
			return null;
		List<int> NeghS = new List<int>();
		NeghS.Clear();
		Queue<int> NeghQ = new Queue<int>();
		NeghQ.Clear();
		int index = (i+1) * 19 + j + 1;
		
		int row = i;
		int col = j;
		NeghS.Add(index);
		NeghQ.Enqueue(index);
		while(NeghQ.Count!=0)
		{
			int num = NeghQ.Dequeue();
			row = num / 19 - 1;
			col = num % 19 - 1;
			//Debug.Log("row:" + row);
			//Debug.Log("col:" + col);
			int left = col - 1;
			int right = col + 1;
			int top = row + 1;
			int bottom = row - 1;
			if (left >= 0 && ChessState[i, left] == chessState)
			{
				//Debug.Log("left");
				//Debug.Log(ChessState[i, left]);
				//Debug.Log(row+"行");
				//Debug.Log(col + "列");
				int newNum = (i + 1) * 19 + left + 1;
				if (!NeghS.Contains(newNum))
				{
					Debug.Log("left+");
					NeghS.Add(newNum);
					NeghQ.Enqueue(newNum);
				}
				
			}
			if (right <= 18 && ChessState[i, right] == chessState)
			{
				//Debug.Log("right");
				//Debug.Log(ChessState[i, right]);
				//Debug.Log(row + "行");
				//Debug.Log(col + "列");
				int newNum = (i + 1) * 19 + right + 1;
				Debug.Log(newNum);
				if (!NeghS.Contains(newNum))
				{
					//Debug.Log("right+");
					//int newNum = (i + 1) * 19 + right + 1;
					NeghS.Add(newNum);
					NeghQ.Enqueue(newNum);
				}

			}
			if (top <=18 && ChessState[top, j] == chessState)
			{
				
				int newNum = (top + 1) * 19 + j + 1;
				if (!NeghS.Contains(newNum))
				{
					//Debug.Log("top+");
					//int newNum = (top + 1) * 19 + j + 1;

					NeghS.Add(newNum);
					NeghQ.Enqueue(newNum);
				}
			}
			if (bottom >=0 && ChessState[bottom, j] == chessState)
			{
				
				int newNum = (bottom + 1) * 19 + j + 1;
				if (!NeghS.Contains(newNum))
				{
					//Debug.Log("bottom+");

					//int newNum = (bottom + 1) * 19 + j + 1;
					NeghS.Add(newNum);
					NeghQ.Enqueue(newNum);
				}
			}
		
		}
		Purge(ref NeghS);//去重
		 //NeghS.Sort();

		Debug.Log(NeghS.Count);
		return NeghS;
	}
	
	public void Purge(ref List<int>needToPurge)//去掉重复的数字
	{
		for(int i=0;i<needToPurge.Count-1;i++)
		{
			int num1 = needToPurge[i];
			for(int j=i+1;j<needToPurge.Count-1;j++)
			{
				int num2 = needToPurge[j];
				if (num1 == num2)
				{
					needToPurge.RemoveAt(j);
					j--;
					continue;
				}

			}
		}
	}

	public void Clear()
	{
		ChessPos = new Vector2[19, 19];
		ChessState = new int[19, 19];
		isPlaying = false;
		Ui = false;
		Camera.main.transform.position = CameraPoint.transform.position;
		Camera.main.transform.rotation = CameraPoint.transform.rotation;
		PanelMgr.instance.OpenPanel<TitlePanel>("");
	}
}
