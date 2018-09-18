using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CreateProfile : MonoBehaviour {


    private bool isLoop = true;

	public RectTransform[] introImages;

	private float wide;

	private float mousePositionStartX;
	private float mousePositionEndX;
    public float dragAmount;
    private float screenPosition;
    private float lastScreenPosition;
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
        }

		side = "left";

		//startWelcomeScreen = true;

	}
    
    void CheckTouch()
    {
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
            if (pageCount < (introImages.Length-1))
                OnSwipeComplete();
            else if (pageCount == (introImages.Length - 1) && dragAmount < 0)
                OnSwipeComplete();
            else if (pageCount == (introImages.Length - 1) && dragAmount > 0)
                OnSwipeComplete();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (Mathf.Abs(dragAmount) < swipeThrustHold)
            {
                if (EventSystem.current.currentSelectedGameObject != null)
                {
                    if (EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject.CompareTag("card"))
                    {
                        Debug.Log("validInput true " + EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex());
                        int index = EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();

                        if ((pageCount == 0 && index == introImages.Length - 1) ||
                            index == pageCount-1)
                        {
                            Debug.Log("trai");
                            dragAmount = 40;
                            screenPosition = lastScreenPosition + dragAmount;
                        }else if ((pageCount == introImages.Length - 1 && index == 0) ||
                            index == pageCount+1)
                        {
                            Debug.Log("phai");
                            dragAmount = -40;
                            screenPosition = lastScreenPosition + dragAmount;
                        }
                        else
                        {
                            lerpTimer = 0;
                        }
                    }
                    else if (EventSystem.current.IsPointerOverGameObject())
                        lerpTimer = 0;
                    else
                        lerpTimer = 0;
                }
                else
                {
                    lerpTimer = 0;
                }
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

            if (isLoop)
            {
                if(pageCount == 0)
                {
                    if( i == introImages.Length - 1)
                    {
                        introImages[i].anchoredPosition =
                            new Vector2(screenPosition + ((wide + spaceBetweenProfileImages) * -1), 0);
                    }else
                        introImages[i].anchoredPosition = new Vector2(screenPosition + ((wide + spaceBetweenProfileImages) * i), 0);
                }
                else if(pageCount == (introImages.Length - 1))
                {
                    if(i == 0)
                    {
                        introImages[i].anchoredPosition =
                        new Vector2(screenPosition + ((wide + spaceBetweenProfileImages) * introImages.Length), 0);

                    }else
                        introImages[i].anchoredPosition = new Vector2(screenPosition + ((wide + spaceBetweenProfileImages) * i), 0);
                }
                else
                     introImages[i].anchoredPosition = new Vector2(screenPosition + ((wide + spaceBetweenProfileImages) * i), 0);
            }
            else
                introImages[i].anchoredPosition = new Vector2(screenPosition + ((wide + spaceBetweenProfileImages) * i), 0);
            
            
			if(side == "right") {
				if(i == pageCount) {
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
            Debug.Log(" i trai");
            if ( Mathf.Abs(dragAmount) > (swipeThrustHold))
            {
                side = "left";
                pageCount -= 1;
                lerpTimer = 0;
                if (pageCount < 0)
                    pageCount = introImages.Length - 1;
                lerpPage = (wide + spaceBetweenProfileImages) * pageCount;

            }
            else
            {
                lerpTimer = 0;
                
            }
           

		} else if(dragAmount < 0) {
            Debug.Log("i phai");
            if ( Mathf.Abs(dragAmount) > (swipeThrustHold))
            {
                side = "right";
                lerpTimer = 0;
                pageCount++;
                if (pageCount > introImages.Length - 1)
                    pageCount = 0;
                lerpPage = (wide + spaceBetweenProfileImages) * pageCount;
            }
            else
            {
                lerpTimer = 0;
            }
        
		}
	}


}
