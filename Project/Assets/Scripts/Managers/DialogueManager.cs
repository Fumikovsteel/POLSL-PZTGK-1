using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using System;

public class DialogueManager : MonoBehaviour
{
	
	private GameObject messageBox;

	private Text messageText;
		
	const string file = "Assets//XML//dialogues.xml";

	const string spacer = "<<Press SPACE to continue.>>";

	private List<Dialogue> dialogues;

	private int currentStatement;
	
	private int currentDialog;

	public bool Locked;

	public void setMessageDialog (GameObject mb, Text mt)
	{
		messageBox = mb;
		messageText = mt;
		
		showMessagebox (false);
	}

	void Awake ()
	{
		RegisterInput ();

		dialogues = new List<Dialogue> ();
		XmlDocument doc = new XmlDocument ();
		doc.Load (file);
		foreach (XmlNode node in doc.DocumentElement.ChildNodes) {
			int id = int.Parse (node.Attributes ["id"].InnerText);
			List<Dialogue.Statement> statements = new List<Dialogue.Statement> ();
			foreach (XmlNode childNode in node.ChildNodes) {
				Dialogue.Statement statement = new Dialogue.Statement ();
				statement.person = childNode.Attributes ["person"].InnerText;
				statement.text = childNode.Attributes ["text"].InnerText;
				statements.Add (statement);
			}
			dialogues.Add (new Dialogue (id, statements));
		}
	}

	private void RegisterInput ()
	{
		Zelda._Game._InputManager.RegisterOnInput (new InputManager.InputKeyTaker ()
		                                          {
			_CanTakeInput = () => { return !Locked; },
			_OnInputUsed = (x) => showMessage()
		}, new InputManager.KeyData ()
		{
			keyCode = KeyCode.Space, keyType = InputManager.EKeyUseType.released
		});
	}

	private void showMessagebox (bool value)
	{
		messageBox.SetActive (value);
	}

	public void ShowDialogues (int id)
	{
		Locked = false;
		showMessagebox (true);
		currentDialog = id;
		if (currentDialog != -1)
			showMessage ();
	}

    public void CloseDialogues()
    {
        closeDialog();
    }

	private void closeDialog ()
	{
		Locked = true;
		showMessagebox (false);
		currentDialog = -1;
		currentStatement = 0;
	}

	private void showMessage ()
	{
		if (dialogues [currentDialog].dialogueList.Count > currentStatement) {
			Dialogue.Statement current = dialogues [currentDialog].dialogueList [currentStatement++];
			string message = string.Format ("{0}: {1}{2}{1}{1}{3}", 
			                                current.person, 
			                                Environment.NewLine, 
			                                current.text,
			                                spacer);
			messageText.text = message;
			messageText.enabled = true;
			showMessagebox (true);
		} else {
			closeDialog ();
		}

	}
}
