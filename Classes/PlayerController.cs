using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public  float MovmentSpeed = 0.2f;
	public 	float Xmax= 7, Xmin = -7;
	public 	float Ymax = 1000, Ymin = -1000;
    public LineRenderer anchorLine;
    public GameObject fingerHead;
    public GameObject anchorCenter;
  //  public GameObject anchorHead;
    public Animator anim;
    public float scaleRate = 0.5f;
    public float regenDelay = 5f;
    public Vector3 Direction;
    MoverComponent anchorCenterMoverComponent;

    public GameObject Body;
    public GameObject EyeSocket;
    public GameObject EyeLight;
    public GameObject iris;
    public Animator eyeSocketAnim;

    public LineRenderer[] lightRenderer;
    float[] lightRendererTimeStamp;

    

    public FXController fXController;

	private Vector3 DesiredPosition;
    public int scaleState;
    int scaleReached;
    private Quaternion starttingRotation;
    private bool bIsInContact;
    private bool bIsContactWithWall;
    private float lastScaleUpTime;
    private float lastScaleDownTime;
    Vector3 BasePosition;

    public float timeToMoveBack;
    float timeElapsed;
    Vector3 AnchorCenterDiePosition;
    Vector3 MagnisDiePosition;
    /////////////////////////////// debug
    float maxSpeed;
    //float lastVelocityAdjustmentTime;
    //private string DTouchWorldPosition;
    //private string DFinalPosition;
    //Vector2 collisionSurfaceNormal;
    //Vector2 collisionpoint;
    //MoverComponent colliderMoverComponent;
    public Button pauseButton;

    Transform anchorCenterCashTransform;
    Camera mainCamera;
    Transform fingerHeadCashTransform;
    Transform bodyCashTransform;
    Transform cashTransform;
    Rigidbody2D cashRigidbody2D;

    //public Sprite solidStarSprite;
    //public Sprite emptyStarSprite;

    StarManager starManager;

	// Use this for initialization
	void Start () 
	{
        DisableLightRenderer();
        lightRendererTimeStamp = new float[lightRenderer.Length];
        cashTransform = transform;
        starttingRotation = cashTransform.rotation;

        anchorCenterMoverComponent = anchorCenter.GetComponent<MoverComponent>();
        anchorCenterCashTransform = anchorCenter.transform;
        fingerHeadCashTransform = fingerHead.transform;
        bodyCashTransform = Body.transform;
        cashRigidbody2D = rigidbody2D;
        mainCamera = Camera.main;

        starManager = FindObjectOfType<StarManager>();

        resertToInitial();
	}

	Vector3 GetBasePosition()
	{
        return anchorCenterCashTransform.position;
	}

	Vector3 GetInput()
	{
//        print("GetInput");
		if (Application.isEditor && Input.GetMouseButton(0))
		{
            if (Input.GetMouseButtonDown(0))
            {
          //      if (pauseButton.spriteState)
                    return new Vector3(-1000, -1000, -1000);
            }
			return Input.mousePosition ;
		}
		else if ( Input.touchCount > 0)
		{
            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                return new Vector3(-1000, -1000, -1000);
            }
			return Input.GetTouch(0).position ;
		}
		return new Vector3 (-1000, -1000, -1000);
	}

    Vector3 FinalPosition, TouchWorldPosition;//, BasePosition;
	void ProcessTouch(Vector3 InputPosition)
	{
        if (InputPosition != new Vector3(-1000, -1000, -1000))
        {
            Vector3 localTouchWorldPosition;

            TouchWorldPosition = mainCamera.ScreenToWorldPoint(InputPosition);
            TouchWorldPosition.x = Mathf.Clamp(TouchWorldPosition.x, Xmin, Xmax);
            localTouchWorldPosition = anchorCenterCashTransform.InverseTransformPoint(TouchWorldPosition);
            localTouchWorldPosition.y = Mathf.Clamp(localTouchWorldPosition.y, Ymin, Ymax);
            TouchWorldPosition = anchorCenterCashTransform.TransformPoint(localTouchWorldPosition);
            fingerHeadCashTransform.position = new Vector3(TouchWorldPosition.x, TouchWorldPosition.y, -2);
            BasePosition = GetBasePosition();
            TouchWorldPosition.z = -2;
            FinalPosition = BasePosition + BasePosition - TouchWorldPosition;
            DesiredPosition = FinalPosition;
        }
        else 
        {
            BasePosition = GetBasePosition();
            FinalPosition = BasePosition + BasePosition - fingerHeadCashTransform.position;
            DesiredPosition = FinalPosition;
        }
	}

    // Update is called once per frame
	void Update ()
	{
		if ( !GameInfo.IsGamePaused() )
		{
            anchorCenterMoverComponent.Move();
            ProcessTouch ( GetInput() );
            anchorLine.SetPosition(0, new Vector3(DesiredPosition.x, DesiredPosition.y, -2));
            anchorLine.SetPosition(1, new Vector3(fingerHeadCashTransform.position.x, fingerHeadCashTransform.position.y, -2));
           // anchorHead.transform.position = new Vector3(DesiredPosition.x, DesiredPosition.y, -2);


            if (cashRigidbody2D.velocity.x > 0)
                bodyCashTransform.rotation = Quaternion.Slerp(bodyCashTransform.rotation, Quaternion.AngleAxis(Vector3.Angle(Vector3.down, cashRigidbody2D.velocity.normalized - Vector2.up * 0.5f), Vector3.forward), 0.08f);
            else
                bodyCashTransform.rotation = Quaternion.Slerp(bodyCashTransform.rotation, Quaternion.AngleAxis(-Vector3.Angle(Vector3.down, cashRigidbody2D.velocity.normalized - Vector2.up * 0.5f), Vector3.forward), 0.08f);

          

            if (Time.time - lastScaleDownTime > regenDelay)
            {
                ScaleDown();
                lastScaleDownTime = Time.time;
            }
            
		}
        else if (GameInfo.IsRollingBack())
        {
            anchorCenterMoverComponent.MoveBack(GameInfo.GetLevelStartPostion(), timeToMoveBack, timeElapsed, AnchorCenterDiePosition);
            MagnisRollBack(GameInfo.GetLevelStartPostion(), timeToMoveBack, timeElapsed, MagnisDiePosition);
            timeElapsed += Time.deltaTime;
            if ((anchorCenterCashTransform.position - GameInfo.GetLevelStartPostion()).sqrMagnitude < 3f)
            {
                GameOver();
            }
        }

	//	if (Input.GetKey (KeyCode.Escape)) 
		//	Application.LoadLevel (0);

        ManageLightRenderers();
	}

    void MagnisRollBack(Vector3 StartPoint,float timeToMoveBack,float timeElapsed,Vector3 DiePosition)
    {
        Vector3 vAvg = ((StartPoint - DiePosition) / timeToMoveBack);
        cashTransform.position = ((-vAvg / timeToMoveBack * (timeElapsed * timeElapsed)) + (2f * vAvg * timeElapsed) + DiePosition);
    }

    void FixedUpdate()
    {
        if (!GameInfo.IsGamePaused())
        {
            MoveToDesiredPosition(DesiredPosition);
        }
        else if (GameInfo.IsRollingBack())
        {
        //    MoveToDesiredPosition(DesiredPosition);
        }
 
    }

    void LateUpdate()
    {
        if (!GameInfo.IsGamePaused())
        {

           // BasePosition += Direction * Time.smoothDeltaTime;
            // if (transform.localScale == desiredScale)
            // transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, 0.2f);
        }
    }


    Vector3 velocity;
    Vector3 lastVelocity;
    void MoveToDesiredPosition(Vector2 inDesiredPosition ) 
    {
        RaycastHit2D traceinfo;

        velocity = cashRigidbody2D.velocity;
        Vector3.SmoothDamp(cashRigidbody2D.position, inDesiredPosition, ref velocity, 0.015f, 3000f, Time.fixedDeltaTime);
       
        if (bIsInContact)
        {

            traceinfo = Physics2D.Raycast(cashRigidbody2D.position, velocity, scaleState + 1f, LayerMask.GetMask("obstacle"));
            if (traceinfo.collider != null)
            {
                Vector3 newVelocity;

                newVelocity = (velocity + lastVelocity) / 2f;
                lastVelocity = velocity;
                velocity = newVelocity;
                //velocity = Vector3.Lerp(velocity,newVelocity,0.01f);
                newVelocity = Vector3.Project(velocity, Vector2.right);
                
                if (newVelocity.magnitude < 600f)
                {
                   velocity.x = Mathf.Lerp(0f, velocity.x, Mathf.Max( Mathf.Pow(newVelocity.magnitude / 600f, 2f),0.1f));
                   velocity.y = Mathf.Lerp(0f, velocity.y, Mathf.Max(Mathf.Pow(newVelocity.magnitude / 600f, 2f), 0.1f));
                }
            }
         }
        cashRigidbody2D.velocity = velocity;
        
    }



	void OnCollisionEnter2D(Collision2D collision)
	{
       // print(collision.gameObject.name + "Enter");
		if (collision.gameObject.name == "LevelSplitter") 
		{
			//GameInfo.LevelFinished();
		}
        else if (collision.gameObject.tag == "wall")
        {
            if (collision.gameObject.name == "RightWall")
                SetInContactWithWall(true,false);
            else
                SetInContactWithWall(true, true);
        }
        else if(collision.gameObject.tag == "UpperBoundry")
        {
            if (bIsInContact)
            {
                Die("UpperBoundry");
            }
        }
        else if (collision.gameObject.tag == "obstacle")
        {
            SetInContact(true);
        //    CancelInvoke("Scale");
	    //	InvokeRepeating("Scale",Time.deltaTime,0.6f);
        }
        else if(collision.gameObject.tag == "InstantDeath")
        {
            Die("InstantDeath");
        }

		//foreach (ContactPoint2D contact in collision.contacts) 
		//{
		//	Debug.DrawRay(contact.point, contact.normal, Color.white);
		//}

	}

    void OnCollisionStay2D(Collision2D coll)
    {
       // print(coll.gameObject.name + "Stay");
        if (coll.gameObject.name == "LevelSplitter")
        {
            //GameInfo.LevelFinished();
        }
        else if (coll.gameObject.tag == "wall")
        {
            if (coll.gameObject.name == "RightWall")
                SetInContactWithWall(true, false);
            else
                SetInContactWithWall(true, true);
        }
        else if (coll.gameObject.tag == "UpperBoundry")
        {
            if (bIsInContact)
            {
                Die("UpperBoundry");
            }
        }
        else if (coll.gameObject.tag == "obstacle")
        {
            SetInContact(true);
        }

    }

    void SetInContact(bool inContact)
    {
        bIsInContact = inContact;
        eyeSocketAnim.SetBool("isInContact", inContact);
        if (inContact)
        {
            if (Time.time - lastScaleUpTime >= scaleRate)
            {
                ScaleUp();
                lastScaleUpTime = Time.time;
                lastScaleDownTime = Time.time; ;
            }
        }
    }

    void SetInContactWithWall(bool inContact, bool isLeft)
    {
        if (!isLeft)
        {
            bIsContactWithWall = inContact;
            eyeSocketAnim.SetBool("isInContactLeft", inContact);
        }
        else 
        {
            bIsContactWithWall = inContact;
            eyeSocketAnim.SetBool("isInContactRight", inContact);
        }
       
    }

    void ScaleUp()
    {
        if (scaleState < 7)
        {
            scaleState++;
            if (scaleState > 0)
            {
                fXController.ChageScale(scaleState);
                if (scaleReached < scaleState)
                {
                    scaleReached = scaleState;
                }
            }
        }
        else
        {
            Die("Scale");
        }
        anim.SetInteger("scaleState", scaleState);
    }

    void ScaleDown()
    {
        if (scaleState > 0)
        {
            scaleState = 0;
            anim.SetInteger("scaleState", scaleState);
            fXController.RegenerationFX();
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        
        if (coll.gameObject.tag == "wall")
        {
            if (coll.gameObject.name == "RightWall")
                SetInContactWithWall(false, false);
            else
                SetInContactWithWall(false, true);
        }
        else if (coll.gameObject.tag == "obstacle")
        {
            SetInContact(false);
        }
        //CancelInvoke("Scale");
        //MovmentSpeed = starttingMovmentSpeed;
        //transform.parent = null;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "BoundaryTirigger")
        {
            Die("BoundaryTirigger");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.gameObject.name == "LevelSplitter")
        {
            //other.
            //collider2D.
          //  GameInfo.LevelFinished();
        }
        else if (other.gameObject.tag == "obstacle")
        {
            StopCoroutine("lightRendererPointFinder");
        }
       
    }

    float lightRendererPointFindingTimeStamp;
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "obstacle" && !GameInfo.IsGamePaused() && Time.time - lightRendererPointFindingTimeStamp > 0.3f)// &&
        {
            StartCoroutine(lightRendererPointFinder(other));
        }
    }

    private IEnumerator lightRendererPointFinder(Collider2D other)
    {
        Collider2D[] colliders = other.gameObject.GetComponents<Collider2D>();

        // print(colliders.Length);

        for (int i = 0; i < colliders.Length; i++)
        {
            Vector2 nearestPoint = Vector2.zero;
            Vector2 nearestPoint1 = Vector2.zero;
            Vector2 nearestPoint2 = Vector2.zero;

            Vector2 direction = colliders[i].bounds.center;
            // nearestPoint = colliders[i].bounds.center;
            direction -= cashRigidbody2D.position;

            // 

            // Vector2.
            RaycastHit2D traceinfo;
            for (int j = -2; j < 2; j++)
            {
                traceinfo = Physics2D.Raycast(cashRigidbody2D.position, Quaternion.AngleAxis(j * 50f, Vector3.forward) * direction.normalized, 3f, LayerMask.GetMask("obstacle"));
               // Debug.DrawLine(cashRigidbody2D.position, Quaternion.AngleAxis(j * 50f, Vector3.forward) * direction.normalized,,);

                if ((cashRigidbody2D.position - traceinfo.point).sqrMagnitude < (cashRigidbody2D.position - nearestPoint).sqrMagnitude)
                {
                    //    nearestPoint2 = nearestPoint1;
                    nearestPoint1 = nearestPoint;
                    nearestPoint = traceinfo.point;
                }
                if (nearestPoint != Vector2.zero || nearestPoint1 != Vector2.zero || nearestPoint2 != Vector2.zero)
                {
                    lightRendererPointFindingTimeStamp = Time.time;
                }
                
                yield return new WaitForEndOfFrame();
            }

            if (nearestPoint != Vector2.zero)
                SetLightRenderer(nearestPoint);
            if (nearestPoint1 != Vector2.zero)
                SetLightRenderer(nearestPoint1);
            if (nearestPoint2 != Vector2.zero)
                SetLightRenderer(nearestPoint2);

            yield return new WaitForEndOfFrame();
        }
         
    }

    int rendererIndex;
    void SetLightRenderer(Vector2 inPosition)
    {
        rendererIndex %= lightRenderer.Length;
        lightRenderer[rendererIndex].enabled = true;
        fXController.LightningSFX();
        lightRenderer[rendererIndex].SetPosition(1, inPosition);
        lightRendererTimeStamp[rendererIndex] = Time.time;
        rendererIndex++;
    }


    void ManageLightRenderers()
    {
        for (int i = 0; i < lightRenderer.Length; i++)
        {
            lightRenderer[i].SetPosition(0, cashTransform.position);
            if (Time.time - lightRendererTimeStamp[i] > 0.35f)
            {
                lightRenderer[i].enabled = false;
            }
        }
    }

    void DisableLightRenderer()
    {
        for (int i = 0; i < lightRenderer.Length; i++)
        {
            lightRenderer[i].enabled = false;
        }
    }
    

	void GameOver()
	{
		GameInfo.GameOver();
        resertToInitial();
       
	}
    void resertToInitial()
    {
        cashTransform.rotation = starttingRotation;
        collider2D.enabled = true;
        renderer.enabled = true;
        Body.renderer.enabled = true;
        EyeSocket.renderer.enabled = true;
      //  EyeLight.renderer.enabled = true;
        EyeLight.SetActive(true);
      //  fingerHead.renderer.enabled = true;

        fingerHead.SetActive(true);
        iris.renderer.enabled = true;
        DesiredPosition = GetBasePosition();
      //  anchorHead.transform.position = GetBasePosition();
        fingerHeadCashTransform.position = GetBasePosition();
        anchorLine.SetPosition(0, GetBasePosition());
        anchorLine.SetPosition(1, GetBasePosition());
       // timeToMoveBack = 1f;
        timeElapsed = 0f;
        cashTransform.position = GetBasePosition();
        cashRigidbody2D.velocity = Vector2.zero;
        scaleState = -1;
        anim.SetInteger("scaleState", scaleState);
        scaleState = 0;
        scaleReached = 0;

        SetInContactWithWall(false, false);
        SetInContactWithWall(false, true);
        SetInContact(false);
    }


    public void Die(string reason)
    {
       

        if (!GameInfo.IsPlayerDead())
        {
            print(reason);
            GameInfo.PlayerDide();
            AnchorCenterDiePosition = anchorCenterCashTransform.position;
            MagnisDiePosition = cashTransform.position;
            cashRigidbody2D.drag = 1000000;
            cashRigidbody2D.isKinematic = true;
            fXController.PlayDyingFX();
            GameInfo.PauseGame();

            // StartRollOver();
            Invoke("HideBodyParts", 0.1f);
            Invoke("StartRollOver", 0.5f);
        }
    }

    void HideBodyParts()
    {
        renderer.enabled = false;
        Body.renderer.enabled = false;
        EyeSocket.renderer.enabled = false;
     //   EyeLight.renderer.enabled = false;
        EyeLight.SetActive(false);
        iris.renderer.enabled = false;
      //  scaleState = 0;
      //  anim.SetInteger("scaleState", scaleState);
    }

    void StartRollOver()
    {
        cashRigidbody2D.drag = 60;
        DisableLightRenderer();
        cashRigidbody2D.isKinematic = false;
        collider2D.enabled = false;

        
        anchorLine.SetPosition(0, GetBasePosition());
        anchorLine.SetPosition(1, GetBasePosition());
       // anchorHead.transform.position = GetBasePosition();
     //   fingerHead.renderer.enabled = false;
        fingerHead.SetActive(false);
        fingerHeadCashTransform.position = GetBasePosition();
        DesiredPosition = GetBasePosition();
        GameInfo.RollOver();
    }


    public void ShowStarsReached()
    {
        starManager.ShowStars(CalcStars());
    }

    public void LevelFinished()
    {
        starManager.ShowStars(CalcStars());
        Invoke("FinishLevel",4);
       // starManager.HideStars();
    }

    void FinishLevel()
    {
        starManager.HideStars();
        GameInfo.LevelFinished();
    }

    int CalcStars()
    {
        int stars = 3;

        //if (scaleReached > 5)
        //{
        //    stars = 0;
        //}
        //else 
        if (scaleReached > 4)
        {
            stars = 1;
        }
        else if (scaleReached > 1)
        {
            stars = 2;
        }

        SaveStars(stars);
        scaleReached = 0;
        return stars;
    }

    void SaveStars(int stars)
    {
        GameInfo.SaveStars(stars);
    }

	// Debug Part

    void OnGUI()
    {
        //GUI.color = Color.white;

        //GUIStyle _style = GUI.skin.GetStyle("Label");
        //_style.alignment = TextAnchor.UpperLeft;
        //_style.fontSize = 20;

        ////if (rigidbody2D.velocity.magnitude > maxSpeed)
        ////    maxSpeed = rigidbody2D.velocity.magnitude;
        //GUI.Label(new Rect(20, 40, 200, 200), "res : " + Screen.width +"*"+ Screen.height);
        //print ("DTouchWorldPosition" + DTouchWorldPosition);
        //_style.alignment = TextAnchor.MiddleCenter;
        //    GUI.Label(new Rect(20, 40, 200, 200), "Contact : " + bIsInContact + bIsContactWithWall);
        //  GUI.Label(new Rect(20, 60, 200, 200), "scaleState : " + scaleState);
        //print ("DFinalPosition" + DFinalPosition);
        //_style.alignment = TextAnchor.MiddleCenter;
        //GUI.Label (new Rect (Screen.width - 320, 20, 200, 200), "LB(1):" + Input.touches.GetLowerBound (1) + " UB(0):" + Input.touches.GetUpperBound (1), _style);
    }
}
