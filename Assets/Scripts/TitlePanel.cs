using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitlePanel : PanelBase {


	private Button startBtn;

	private Dropdown dropdown;

	public GameObject Five;

	public GameObject ChessObj;

	public GameObject CameraPoint;

	public override void Init(params object[] args)
	{
		base.Init(args);
		skinPath = "TitlePanel";
		layer = PanelLayer.Panel;
		Five = GameObject.Find("Five");
		ChessObj = GameObject.Find("Chess");
		CameraPoint = GameObject.Find("CameraPoint");

	}
	public override void OnShowing()
	{
		base.OnShowing();
		Transform skinTrans = skin.transform;
		startBtn = skinTrans.FindChild("StartBtn").GetComponent<Button>();
		dropdown = skinTrans.FindChild("Dropdown").GetComponent<Dropdown>();

		startBtn.onClick.AddListener(OnStartClick);
		
	}

	public void OnStartClick()
	{

		//Battle.instance.StartTwoCampBattle(2, 2);
		//Close();
		PanelMgr.instance.ClosePanel("TitlePanel");
		if(dropdown.value ==0)
		{
			five.instance.Ui = true;
			Chess.instance.Ui = false;
			Five.SetActive(true);
			ChessObj.SetActive(false);
			
			Camera.main.transform.position = CameraPoint.transform.position;
			Camera.main.transform.LookAt(Five.transform);

			five.instance.FiveInit();
		}else if(dropdown.value == 1)
		{
			five.instance.Ui = false;
			Chess.instance.Ui = true;
			ChessObj.SetActive(true);
			Five.SetActive(false);
			Camera.main.transform.position = CameraPoint.transform.position;
			Camera.main.transform.LookAt(ChessObj.transform);
			//Chess.instance.ChessInit();
		}
		
	}

	
}
