using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CarousalView : MonoBehaviour {


	public RectTransform window;
	public bool startWelcomeScreen;

	public RectTransform[] introImages;

	private float wide;

	private float mousePositionStartX;
	private float mousePositionEndX;
	private float dragAmount;
	private float screenPosition;
	private float lastScreenPosition;
	private float lerpTimer;
	private float lerpPage;

	public int pageCount = 1;
	public string side = "";

	public int swipeThrustHold = 30;
	public int spaceBetweenProfileImages = 30;
	private bool canSwipe;

	public GameObject cartoonWindow;

	public Texture2D userPic;

	#region mono functions

	void Start() {

		wide = cartoonWindow.GetComponent<RectTransform>().rect.width;

		for(int i = 1; i < introImages.Length; i++){

			introImages[i].anchoredPosition = new Vector2(((wide+spaceBetweenProfileImages)*i),0);

		}

		side = "right";

		startWelcomeScreen = true;

	}

	public int GetSelectedItemIndex() {
		if (side.Equals ("right")) {
			return (pageCount - 1);
		} else {
			return (pageCount);
		}
	}

	void Update() {

		if(!startWelcomeScreen)
			return;

		lerpTimer=lerpTimer+Time.deltaTime;
		if(lerpTimer<.333){
			screenPosition = Mathf.Lerp(lastScreenPosition ,lerpPage*-1 , lerpTimer*3);
			lastScreenPosition=screenPosition;
		}

		if(Input.GetMouseButtonDown(0) && Input.mousePosition.y > (Screen.height*0.3f) && Input.mousePosition.y < (Screen.height*0.91f) ) {
			canSwipe = true;
			mousePositionStartX = Input.mousePosition.x;
		}


		if(Input.GetMouseButton(0)) {
			if(canSwipe){
				mousePositionEndX = Input.mousePosition.x;
				dragAmount=mousePositionEndX-mousePositionStartX;
				screenPosition=lastScreenPosition+dragAmount;
			}
		}

		if(Mathf.Abs(dragAmount) > swipeThrustHold && canSwipe){
			canSwipe = false;
			lastScreenPosition=screenPosition;
			if(pageCount < introImages.Length )
				OnSwipeComplete () ;
			else if(pageCount == introImages.Length && dragAmount < 0)
				lerpTimer=0;
			else if(pageCount == introImages.Length && dragAmount > 0)
				OnSwipeComplete () ;
		}

		if(Input.GetMouseButtonUp(0)) {

			if(Mathf.Abs(dragAmount) < swipeThrustHold) {
				lerpTimer = 0;
			}
		}

		for(int i = 0; i < introImages.Length; i++){

			introImages[i].anchoredPosition = new Vector2(screenPosition+((wide+spaceBetweenProfileImages)*i),0);

			if(side == "right") {
				if(i == pageCount-1) {
					introImages[i].localScale = Vector3.Lerp(introImages[i].localScale,new Vector3(1.2f,1.2f,1.2f),Time.deltaTime*5);
					Color temp = introImages[i].GetComponent<Image>().color;
					introImages[i].GetComponent<Image>().color = new Color(temp.r,temp.g,temp.b,1);
				} else {
					introImages[i].localScale = Vector3.Lerp(introImages[i].localScale,new Vector3(0.7f,0.7f,0.7f),Time.deltaTime*5);
					Color temp = introImages[i].GetComponent<Image>().color;
					introImages[i].GetComponent<Image>().color = new Color(temp.r,temp.g,temp.b,0.5f);
				}
			} else {
				if(i == pageCount) {
					introImages[i].localScale = Vector3.Lerp(introImages[i].localScale,new Vector3(1.2f,1.2f,1.2f),Time.deltaTime*5);
					Color temp = introImages[i].GetComponent<Image>().color;
					introImages[i].GetComponent<Image>().color = new Color(temp.r,temp.g,temp.b,1);
				} else {
					introImages[i].localScale = Vector3.Lerp(introImages[i].localScale,new Vector3(0.7f,0.7f,0.7f),Time.deltaTime*5);
					Color temp = introImages[i].GetComponent<Image>().color;
					introImages[i].GetComponent<Image>().color = new Color(temp.r,temp.g,temp.b,0.5f);
				}
			}
		}


	}

	#endregion



	private void OnSwipeComplete () {

		lastScreenPosition=screenPosition;

		if(dragAmount > 0){

			if(Mathf.Abs(dragAmount) > (swipeThrustHold)){

				if(pageCount == 0){
					lerpTimer=0;
					lerpPage=0;
				}else {
					if(side == "right")
						pageCount--;
					side = "left";
					pageCount-=1;
					lerpTimer=0;
					if(pageCount < 0)
						pageCount = 0;
					lerpPage = (wide+spaceBetweenProfileImages)*pageCount;
					//introimage[pagecount] is the current picture
				}

			} else {
				lerpTimer=0;
			}

		} else if(dragAmount < 0) {

			if(Mathf.Abs(dragAmount) > (swipeThrustHold)){

				if(pageCount == introImages.Length){
					lerpTimer=0;
					lerpPage=(wide+spaceBetweenProfileImages)*introImages.Length-1;
				}else {
					if(side == "left")
						pageCount++;
					side = "right";
					lerpTimer=0;
					lerpPage = (wide+spaceBetweenProfileImages)*pageCount;
					pageCount++;
					//introimage[pagecount] is the current picture
				}

			} else {
				lerpTimer=0;
			}
		}
	}
}
