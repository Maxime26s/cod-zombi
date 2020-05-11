using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
	float deltaTime = 0.0f;
	TextMeshProUGUI tmpro;

	private void Start()
	{
		tmpro = GetComponent<TextMeshProUGUI>();
	}

	void Update()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
		Refresh();
	}

	void Refresh()
	{
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
		tmpro.text = text;
	}
}