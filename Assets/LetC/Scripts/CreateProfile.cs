using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CreateProfile : MonoBehaviour {
    

    public bool isLoop;
    private float tempPos;

	public RectTransform[] introImages;

	private float wide;

	private float mousePositionStartX;
	private float mousePositionEndX;
    public float dragAmount;
    public float screenPosition;
    public float lastScreenPosition;
	private float lerpTimer;
	private float lerpPage;

	public int pageCount = 0;
	public string side = "";

	public int swipeThrustHold = 30;
	public int spaceBetweenProfileImages = 30;
	private bool canSwipe;

	public GameObject cartoonWindow;
    

	#region mono functions

	void Start() {

		wide = cartoonWindow.GetComponent<RectTransform>().rect.width;

		for(int i = 0; i < introImages.Length; i++){

			introImages[i].anchoredPosition = new Vector2(((wide+spaceBetweenProfileImages)*i),0);
            if (introImages[i].gameObject.GetComponent<Button>() != null)
            {
                Debug.Log(":v: " + introImages[i].gameObject.GetComponent<Button>().name + i);
                var j = i;
                introImages[i].gameObject.GetComponent<Button>().onClick.AddListener(delegate { OnSelectCard(j); });
            }

        }

		side = "left";

		//startWelcomeScreen = true;

	}
    
    void OnSelectCard(int cardIndex)
    {
        Debug.Log("just push button: " + cardIndex);
        if (cardIndex > pageCount) //On the right
        {
            dragAmount = -35;
            Debug.Log("ben phai: " + pageCount);
            
            screenPosition -= 130; 
        }
        else if(cardIndex < pageCount-1) // On the Left
        {
            dragAmount = 35;
            Debug.Log("ben trai: " + pageCount);
            screenPosition += 130;
        }
        
        OnSwipeComplete();
    }

    void CheckTouch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                if (EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject.CompareTag("card"))
                    Debug.Log("validInput true ");
                else if (EventSystem.current.IsPointerOverGameObject())
                    Debug.Log("validInput false ");
                else
                    Debug.Log("validInput true ");
            }
        }
        //TODO: Xác định vị trí Screen height
        if (Input.GetMouseButtonDown(0) && Input.mousePosition.y > (Screen.height * 0.6173f) && Input.mousePosition.y < (Screen.height * 0.91f))
        {
            canSwipe = true;
            mousePositionStartX = Input.mousePosition.x;
        }


        if (Input.GetMouseButton(0))
        {
            if (canSwipe)
            {
                mousePositionEndX = Input.mousePosition.x;
                dragAmount = mousePositionEndX - mousePositionStartX;
                screenPosition = lastScreenPosition + dragAmount;
            }
        }

        if (Mathf.Abs(dragAmount) > swipeThrustHold && canSwipe)
        {
            canSwipe = false;
            lastScreenPosition = screenPosition;
            if (pageCount < introImages.Length)
                OnSwipeComplete();
            else if (pageCount == introImages.Length && dragAmount < 0)
                lerpTimer = 0;
            else if (pageCount == introImages.Length && dragAmount > 0)
                OnSwipeComplete();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (Mathf.Abs(dragAmount) < swipeThrustHold)
            {
                lerpTimer = 0;
            }
        }
    }


    void Update() {
        
        lerpTimer =lerpTimer+Time.deltaTime;
		if(lerpTimer<.333){
			screenPosition = Mathf.Lerp(lastScreenPosition ,lerpPage*-1 , lerpTimer*3);
			lastScreenPosition=screenPosition;
		}
        
        CheckTouch();

        for (int i = 0; i < introImages.Length; i++){
          
            introImages[i].anchoredPosition = new Vector2(screenPosition + ((wide + spaceBetweenProfileImages) * i), 0);
            
            
			if(side == "right") {
				if(i == pageCount-1) {
                    //TODO:
					introImages[i].localScale = Vector3.Lerp(introImages[i].localScale,new Vector3(1.2f,1.2f,1.2f),Time.deltaTime*5);
					Color temp = introImages[i].GetComponent<Image>().color;
					introImages[i].GetComponent<Image>().color = new Color(temp.r,temp.g,temp.b,1);
				} else {
					introImages[i].localScale = Vector3.Lerp(introImages[i].localScale,new Vector3(0.7f,0.7f,0.7f),Time.deltaTime*5);
					Color temp = introImages[i].GetComponent<Image>().color;
                    //Make tranparent for img not select
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

        Debug.Log("in");
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
            Debug.Log("phai");
            if (Mathf.Abs(dragAmount) > (swipeThrustHold)){

				if(pageCount == introImages.Length){
					lerpTimer=0;
					lerpPage=(wide+spaceBetweenProfileImages)*introImages.Length-1;
				}else {
                    //if (side == "left")
                    //    pageCount++;
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
