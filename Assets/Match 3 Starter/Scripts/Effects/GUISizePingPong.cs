using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class GUISizePingPong : MonoBehaviour {

	public Vector3 minScale = new Vector3(.5f, .5f, .5f);
	public float changeDelay = .03f;
	RectTransform rectTransform;

	void Start() {
		rectTransform = GetComponent<RectTransform>();
		StartCoroutine(PingPongSize());
	}

	IEnumerator PingPongSize() {
		Vector3 startingScale = rectTransform.localScale;
		Vector3 currentScale = startingScale;
		
		Vector3 changeScale = new Vector3(.01f, .01f, .01f);
		bool shrink = true;

		while (true) {
			if (shrink) {
				currentScale -= changeScale;
				if (currentScale.x <= minScale.x) {
					shrink = false;
				}
			} else {
				currentScale += changeScale;
				if (currentScale.x >= 1) {
					shrink = true;
				}
            }
			rectTransform.localScale = currentScale;
			yield return new WaitForSeconds(changeDelay);
		}
	}
}
