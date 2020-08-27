using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextArchitect
{
	/// <summary>A dictionary keeping tabs on all architects present in a scene. Prevents multiple architects from influencing the same text object simultaneously.</summary>
	private static Dictionary<TextMeshProUGUI, TextArchitect> activeArchitects = new Dictionary<TextMeshProUGUI, TextArchitect>();

	private string preText;
	private string targetText;

	[HideInInspector] public int charactersPerFrame = 1;
	[HideInInspector] public float speed = 1f;

	public bool skip = false;

	public bool isConstructing {get{return buildProcess != null;}}
	Coroutine buildProcess = null;

	TextMeshProUGUI tmpro;

	public TextArchitect(TextMeshProUGUI tmpro, string targetText, string preText = "", int charactersPerFrame = 1, float speed = 1f)
	{
		this.tmpro = tmpro;
		this.targetText = targetText;
		this.preText = preText;
		this.charactersPerFrame = charactersPerFrame;
		this.speed = Mathf.Clamp(speed, 1f, 300f);

		Initiate();
	}

	public void Stop()
	{
		if (isConstructing)
		{
			DialogueSystem.instance.StopCoroutine(buildProcess);
		}
		buildProcess = null;
	}

	IEnumerator Construction()
	{
		int runsThisFrame = 0;

		tmpro.text = "";
		tmpro.text += preText;

		tmpro.ForceMeshUpdate();
		TMP_TextInfo inf = tmpro.textInfo;
		int vis = inf.characterCount;

		tmpro.text += targetText;

		tmpro.ForceMeshUpdate();
		inf = tmpro.textInfo;
		int max = inf.characterCount;

		tmpro.maxVisibleCharacters = vis;

		while(vis < max)
		{

			if (skip)
			{
				speed = 1;
				charactersPerFrame = charactersPerFrame < 5 ? 5 : charactersPerFrame + 3;
			}


			while(runsThisFrame < charactersPerFrame)
			{
				vis++;
				tmpro.maxVisibleCharacters = vis;
				runsThisFrame++;
			}


			runsThisFrame = 0;
			yield return new WaitForSeconds(0.01f * speed);
		}


		Terminate();
	}

	void Initiate()
	{

		TextArchitect existingArchitect = null;
		if (activeArchitects.TryGetValue(tmpro, out existingArchitect))
			existingArchitect.Terminate();

		buildProcess = DialogueSystem.instance.StartCoroutine(Construction());
		activeArchitects.Add(tmpro, this);
	}

	public void Terminate()
	{
		activeArchitects.Remove(tmpro);
		if (isConstructing){
			DialogueSystem.instance.StopCoroutine(buildProcess);
        }
		buildProcess = null;
	}

	public void ForceFinish(){
		tmpro.maxVisibleCharacters = tmpro.text.Length;
		Terminate();
	}

	public void Renew(string targ, string pre){
		targetText = targ;
		preText = pre;

		if(isConstructing){
			DialogueSystem.instance.StopCoroutine(buildProcess);
		}
		buildProcess = DialogueSystem.instance.StartCoroutine(Construction());
	}

	public void ShowText(string text){
		if(isConstructing){
			DialogueSystem.instance.StopCoroutine(buildProcess);
		}
		targetText = text;
		tmpro.text = text;

		tmpro.maxVisibleCharacters = tmpro.text.Length;

		if(tmpro == DialogueSystem.instance.speechText){
			DialogueSystem.instance.targetSpeech = text;
		}
	}
}