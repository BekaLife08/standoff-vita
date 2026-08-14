using Axlebolt.Standoff.Player.Movement.States;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerControl
{
	public class UI_PlayerDebug : MonoBehaviour
	{
		[Serializable]
		public class States
		{
			public Color colorActive;

			public Color colorInactive;

			public Image walkImg;

			public Image jumpImg;

			public Image idleImg;

			public Image crouchImg;
		}

		[Serializable]
		public class Parameters
		{
			public Slider standCoeffAct;

			public Slider standCoeffBld;

			public float standCoeffRange1;

			public float standCoeffRange2;

			public Slider jumpCoeffAct;

			public Slider jumpCoeffBld;

			public float jumpCoeffRange1;

			public float jumpCoeffRange2;
		}

		[Serializable]
		public class GeneralData
		{
			public List<UI_LogData> loggerList;
		}

		[Serializable]
		public class NetworkParameters
		{
			public float displacementSpeed;

			public RectTransform displacementPanel;

			public Text ping;

			public GameObject labelPrefab;

			[HideInInspector]
			public float panelOffset;
		}

		public GameObject statesPanel;

		public GameObject parametersPanel;

		public GameObject networkPanel;

		public GameObject logPanel;

		public States states;

		public Parameters parameters;

		public GeneralData generalData;

		public NetworkParameters networkParameters;

		private Queue<GameObject> garbageQueue = new Queue<GameObject>();

		public float standTypeAct
		{
			get
			{
				return 0f;
			}
			set
			{
				parameters.standCoeffAct.value = (value - parameters.standCoeffRange1) / (parameters.standCoeffRange2 - parameters.standCoeffRange1);
			}
		}

		public float standTypeBld
		{
			get
			{
				return 0f;
			}
			set
			{
				parameters.standCoeffBld.value = (value - parameters.standCoeffRange1) / (parameters.standCoeffRange2 - parameters.standCoeffRange1);
			}
		}

		public float jumpTypeAct
		{
			get
			{
				return 0f;
			}
			set
			{
				parameters.jumpCoeffAct.value = (value - parameters.jumpCoeffRange1) / (parameters.jumpCoeffRange2 - parameters.jumpCoeffRange1);
			}
		}

		public float jumpTypeBld
		{
			get
			{
				return 0f;
			}
			set
			{
				parameters.jumpCoeffBld.value = (value - parameters.jumpCoeffRange1) / (parameters.jumpCoeffRange2 - parameters.jumpCoeffRange1);
			}
		}

		public UI_LogData GetLogger()
		{
			foreach (UI_LogData logger in generalData.loggerList)
			{
				if (logger.isEmpty)
				{
					return logger;
				}
			}
			return generalData.loggerList[0];
		}

		public void SetState(TranslationStatesEnum state)
		{
			states.crouchImg.color = states.colorInactive;
			states.jumpImg.color = states.colorInactive;
			states.idleImg.color = states.colorInactive;
			states.walkImg.color = states.colorInactive;
			if (state == TranslationStatesEnum.Crouch)
			{
				states.crouchImg.color = states.colorActive;
			}
			if (state == TranslationStatesEnum.Jump)
			{
				states.jumpImg.color = states.colorActive;
			}
			if (state == TranslationStatesEnum.Idle)
			{
				states.idleImg.color = states.colorActive;
			}
			if (state == TranslationStatesEnum.Walk)
			{
				states.walkImg.color = states.colorActive;
			}
		}

		public void SetPing(int ping)
		{
			networkParameters.ping.text = string.Empty + ping;
		}

		public void OnPackageRecived()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(networkParameters.labelPrefab);
			gameObject.transform.SetParent(networkParameters.displacementPanel);
			float x = 0f - networkParameters.panelOffset;
			Vector3 localPosition = networkParameters.labelPrefab.transform.localPosition;
			Vector3 localPosition2 = new Vector3(x, localPosition.y, 0f);
			gameObject.transform.localPosition = localPosition2;
			gameObject.transform.localScale = networkParameters.labelPrefab.transform.localScale;
			garbageQueue.Enqueue(gameObject);
			if (garbageQueue.Count > 20)
			{
				gameObject = garbageQueue.Dequeue();
				UnityEngine.Object.Destroy(gameObject);
			}
		}

		private void Start()
		{
		}

		public void OnStatesPanelClick()
		{
			statesPanel.SetActive(!statesPanel.activeSelf);
		}

		public void OnParametersPanelClick()
		{
			parametersPanel.SetActive(!parametersPanel.activeSelf);
		}

		public void OnNetworkPanelClick()
		{
			networkPanel.SetActive(!networkPanel.activeSelf);
		}

		public void OnLogPanelClick()
		{
			logPanel.SetActive(!logPanel.activeSelf);
		}

		private void Update()
		{
			networkParameters.displacementPanel.localPosition += new Vector3((0f - networkParameters.displacementSpeed) * Time.deltaTime, 0f, 0f);
			NetworkParameters obj = networkParameters;
			Vector3 localPosition = networkParameters.displacementPanel.localPosition;
			obj.panelOffset = localPosition.x;
		}
	}
}
