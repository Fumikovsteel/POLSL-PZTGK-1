using UnityEngine;
using System.Collections;
using System;

public class NPCManager : MonoBehaviour
{
	public enum ENPCName
	{
		NPCOne,
		NPCTwo,
		NPCThree
	}

	[SerializeField]
	private GameObject
		startTalkMessagePrefab;
	[SerializeField]
	private Vector3
		startTalkMessagePosition;

	/// <summary>
	/// If have some value we have message on screen, and we're in range of some NPC to talk
	/// </summary>
	private NPC npcInRange = null;
	private GameObject startTalkMessage;
	private bool gamePaused = false;
	private bool Locked
    { get { return gamePaused || (npcInRange == null); } }

	/// <summary>
	/// Parent of all NPCs na scene
	/// </summary>
	private const string npcsParentName = "NPCs";

	private void Awake ()
	{
		Zelda._Common._GameplayEvents._OnLevelWasLoaded += OnCurLevelWasLoaded;
		Zelda._Common._GameplayEvents._OnGamePaused += OnGamePaused;
		Zelda._Common._GameplayEvents._OnGameUnpaused += OnGameUnpaused;
		Zelda._Game._InputManager.RegisterOnInput (new InputManager.InputKeyTaker ()
            {
                _CanTakeInput = () => { return !Locked; },
                _OnInputUsed = (x) => StartTalk()
            }, new InputManager.KeyData ()
            {
                keyCode = KeyCode.E,
                keyType = InputManager.EKeyUseType.released
            });
	}

	private void OnDestroy ()
	{
		if (Zelda._Common != null) {
			Zelda._Common._GameplayEvents._OnLevelWasLoaded -= OnCurLevelWasLoaded;
			Zelda._Common._GameplayEvents._OnGamePaused -= OnGamePaused;
			Zelda._Common._GameplayEvents._OnGameUnpaused -= OnGameUnpaused;
		}
	}

	private void OnGamePaused ()
	{
		gamePaused = true;
	}

	private void OnGameUnpaused ()
	{
		gamePaused = false;
	}

	private void OnCurLevelWasLoaded ()
	{
		GameObject npcsParent = GameObject.Find (npcsParentName);
		if (npcsParent == null)
			Debug.LogError ("You need to have " + npcsParentName + " object on scene!");

		NPC[] allNPCs = npcsParent.GetComponentsInChildren<NPC> ();
		for (int i = allNPCs.Length - 1; i >= 0; i--)
			allNPCs [i]._Init (this);
	}

	private void StartTalk ()
	{
		Action onRotateFinished = () => {
			// TODO: to nie dziaua :-(;
		};
		Zelda._Game._DialogueManager.ShowDialogues (1);
		float playerZRotation = Zelda._Game._GameManager._Player.RotateToNPC (npcInRange.transform);
		playerZRotation = (playerZRotation + 180.0f) % 360.0f;
		npcInRange._ChangeLookDirection (playerZRotation);
	}

	public void _ShowCanTalkMessage (NPC curNPC)
	{
		npcInRange = curNPC;
		startTalkMessage = Instantiate (startTalkMessagePrefab) as GameObject;
		startTalkMessage.transform.SetParentResetLocal (transform);
		startTalkMessage.transform.position = new Vector3 (curNPC.transform.position.x + startTalkMessagePosition.x,
                                                          curNPC.transform.position.y + startTalkMessagePosition.y,
                                                          startTalkMessagePrefab.transform.position.z);
	}

	public void _HideCanTalkMessage ()
	{
		if (npcInRange == null)
			Debug.LogWarning ("There is no active NPC!");
		npcInRange = null;
		Destroy (startTalkMessage);
	}
}
