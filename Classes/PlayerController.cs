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
    public GameObject anchorHead;
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

    public GameObject mainCamera;

    public FXController fXController;

	private Vector3 DesiredPosition;
    public int scaleState;
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
	// Use this for initialization
	void Start () 
	{
        DisableLightRenderer();
        lightRendererTimeStamp = new float[lightRenderer.Length];
        //anchorCenter.transform.position = GetBasePosition();
        starttingRotation = transform.rotation;

        anchorCenterMoverComponent = anchorCenter.GetComponent<MoverComponent>();

        resertToInitial();
	}

	Vector3 GetBasePosition()
	{
		//Camera MyCamera = FindObjectOfType<Camera> ();
		//return MyCamera.transform.position;
		//return transform.parent.position;
       // return mainCamera.transform.position;
        return anchorCenter.transform.position;
        //return new Vector3(mainCamera.transform.position.x,mainCamera.transform.position.y,-2);
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

            TouchWorldPosition = Camera.main.ScreenToWorldPoint(InputPosition);
            //TouchWorldPosition = Vector3.Lerp(TouchWorldPosition, Camera.main.ScreenToWorldPoint(InputPosition),0.9f);
            TouchWorldPosition.x = Mathf.Clamp(TouchWorldPosition.x, Xmin, Xmax);
            localTouchWorldPosition = anchorCenter.transform.InverseTransformPoint(TouchWorldPosition);
            localTouchWorldPosition.y = Mathf.Clamp(localTouchWorldPosition.y, Ymin, Ymax);
            TouchWorldPosition = anchorCenter.transform.TransformPoint(localTouchWorldPosition);
          //  anchorLine.SetPosition(1, new Vector3(TouchWorldPosition.x, TouchWorldPosition.y, -1));
            fingerHead.transform.position = new Vector3(TouchWorldPosition.x, TouchWorldPosition.y, -2);
            BasePosition = GetBasePosition();
            TouchWorldPosition.z = -2;
            FinalPosition = BasePosition + BasePosition - TouchWorldPosition;
            //FinalPosition = - TouchWorldPosition;
           // DesiredPosition = FinalPosition;
            DesiredPosition = FinalPosition;
            

            /// debug
            //		print (transform.parent.name + TouchWorldPosition  + BasePosition.ToString() + FinalPosition.ToString());
            //DTouchWorldPosition = TouchWorldPosition.ToString();
            //DFinalPosition = FinalPosition.ToString();
        }
        else 
        {
            //anchorLine.SetPosition(1, new Vector3(TouchWorldPosition.x, TouchWorldPosition.y, -1));
            //fingerHead.transform.localPosition = new Vector3(TouchWorldPosition.x, TouchWorldPosition.y, -1);
           // BasePosition = GetBasePosition();
          //  TouchWorldPosition.z = -2;
            BasePosition = GetBasePosition();
            FinalPosition = BasePosition + BasePosition - fingerHead.transform.position;
            DesiredPosition = FinalPosition;
            //FinalPosition = - TouchWorldPosition;
          //  DesiredPosition = FinalPosition;
        }
        // Vector3.Lerp(DesiredPosition, FinalPosition, 0.9f);
	}

    // Update is called once per frame
	void Update ()
	{
		if ( !GameInfo.IsGamePaused() )
		{
            anchorCenterMoverComponent.Move();
           // BasePosition += Direction * Time.smoothDeltaTime;
           // anchorCenter.transform.position = GetBasePosition();
			
			//print ( transform.position + " " + DesiredPosition + " " + MovmentSpeed );
            //rigidbody2D.position = Vector3.Lerp(rigidbody2D.position, DesiredPosition, 0.1f);
            ProcessTouch ( GetInput() );
            anchorLine.SetPosition(0, new Vector3(DesiredPosition.x, DesiredPosition.y, -2));
            anchorLine.SetPosition(1, new Vector3(fingerHead.transform.position.x, fingerHead.transform.position.y, -2));
            anchorHead.transform.position = new Vector3(DesiredPosition.x, DesiredPosition.y, -2);

           // EyeSocket.transform.localPosition = rigidbody2D.velocity/5f;
          //  float y = Mathf.Lerp(EyeSocket.transform.localPosition.y, rigidbody2D.velocity.normalized.y ,0.05f);
           // EyeSocket.transform.localPosition = new Vector3(EyeSocket.transform.localPosition.x, y, EyeSocket.transform.localPosition.z);

         //   EyeSocket.transform.localPosition = Vector3.Lerp(EyeSocket.transform.localPosition, rigidbody2D.velocity.normalized - Vector2.up, 0.03f);


            if (rigidbody2D.velocity.x > 0)
                Body.transform.rotation = Quaternion.Slerp(Body.transform.rotation, Quaternion.AngleAxis(Vector3.Angle(Vector3.down, rigidbody2D.velocity.normalized - Vector2.up * 0.5f), Vector3.forward), 0.08f);
            else
                Body.transform.rotation = Quaternion.Slerp(Body.transform.rotation, Quaternion.AngleAxis(-Vector3.Angle(Vector3.down, rigidbody2D.velocity.normalized - Vector2.up*0.5f), Vector3.forward), 0.08f);

             //   Quaternion.Slerp(Body.transform.rotation, Quaternion.FromToRotation(Vector3.down, rigidbody2D.velocity.normalized), 0.1f);
            //Body.transform.rotation = Quaternion.Lerp(Body.transform.rotation, Quaternion.FromToRotation(Vector3.down, rigidbody2D.velocity.normalized), 0.1f);

         //   Debug.DrawRay(Body.transform.position, rigidbody2D.velocity.normalized - Vector2.up );

          //  EyeSocket.transform.rotation = Quaternion.Lerp(EyeSocket.transform.rotation, Quaternion.FromToRotation(Vector3.down, rigidbody2D.velocity - Vector2.up), 0.008f);
           // EyeLight.transform.rotation = Quaternion.Lerp(EyeLight.transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.05f);
          //  EyeLight.transform.rotation = Quaternion.Lerp(EyeLight.transform.rotation, Quaternion.FromToRotation(rigidbody2D.velocity,Vector3.down) , 0.01f);

            if (Time.time - lastScaleDownTime > regenDelay)
            {
                ScaleDown();
                lastScaleDownTime = Time.time;
            }
            
		}
        else if (GameInfo.IsRollingBack())
        {
           // (GameInfo.GetLevelStartPostion() - anchorCenter.transform.position)/1.1f
            anchorCenterMoverComponent.MoveBack(GameInfo.GetLevelStartPostion(), timeToMoveBack, timeElapsed, AnchorCenterDiePosition);
            MagnisRollBack(GameInfo.GetLevelStartPostion(), timeToMoveBack, timeElapsed, MagnisDiePosition);
            //ProcessTouch(GetBasePosition());
           // DesiredPosition = GetBasePosition();
           // transform.Translate((GameInfo.GetLevelStartPostion() - transform.position) * Time.deltaTime );
          //  Vector3 vAvg = ((GameInfo.GetLevelStartPostion() - AnchorCenterDiePosition) / timeToMoveBack);
          //  transform.position = ((-vAvg / timeToMoveBack * (timeElapsed * timeElapsed)) + (2f * vAvg * timeElapsed) + DiePosition);
          //  transform.Translate((GameInfo.GetLevelStartPostion() - transform.position) / timeToMoveBack * Time.deltaTime);
          //  timeToMoveBack -= Time.deltaTime;
            timeElapsed += Time.deltaTime;
            if ((anchorCenter.transform.position- GameInfo.GetLevelStartPostion()).sqrMagnitude < 3f)
            {
                GameOver();
            }
        }

		if (Input.GetKey (KeyCode.Escape)) 
			Application.LoadLevel (0);

        ManageLightRenderers();
	}

    void MagnisRollBack(Vector3 StartPoint,float timeToMoveBack,float timeElapsed,Vector3 DiePosition)
    {
        Vector3 vAvg = ((StartPoint - DiePosition) / timeToMoveBack);
        transform.position = ((-vAvg / timeToMoveBack * (timeElapsed * timeElapsed)) + (2f * vAvg * timeElapsed) + DiePosition);
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

        velocity = rigidbody2D.velocity;
        Vector3.SmoothDamp(rigidbody2D.position, inDesiredPosition, ref velocity, 0.015f, 3000f, Time.fixedDeltaTime);
       // Vector3.SmoothDamp(rigidbody2D.position, inDesiredPosition, ref velocity, 0.015f, 3000f, Time.smoothDeltaTime);
       
        if (bIsInContact)
        {

            traceinfo = Physics2D.Raycast(rigidbody2D.position, velocity, scaleState + 1f, LayerMask.GetMask("obstacle"));
           // Debug.DrawRay(rigidbody2D.position, traceinfo.normal, Color.red);
           // print(traceinfo.collider);
            if (traceinfo.collider != null)
            {
                Vector3 newVelocity;

             //   Debug.DrawRay(rigidbody2D.position, lastVelocity, Color.yellow);
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
            //Debug.DrawRay(rigidbody2D.position, Vector2.right * 10f, Color.green);
         }
        //velocity -=new Vector3(0,100,0);

        rigidbody2D.velocity = Vector3.Lerp(rigidbody2D.velocity, velocity, 0.8f);

        

         //rigidbody2D.AddRelativeForce(new Vector2(0,100));
         //Debug.DrawRay(rigidbody2D.position, velocity);
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
            scaleState++;
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
       
    }

    float lightRendererPointFindingTimeStamp;
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "obstacle" && Time.time - lightRendererPointFindingTimeStamp > 0.3f && !GameInfo.IsGamePaused())
        {
            // Die("");
            Collider2D[] colliders = other.gameObject.GetComponents<Collider2D>();

            // print(colliders.Length);

            for (int i = 0; i < colliders.Length; i++)
            {
                Vector2 nearestPoint = Vector2.zero;
                Vector2 nearestPoint1 = Vector2.zero;
                Vector2 nearestPoint2 = Vector2.zero;

                Vector2 direction = colliders[i].bounds.center;
                // nearestPoint = colliders[i].bounds.center;
                direction -= rigidbody2D.position;

                // 

                // Vector2.
                RaycastHit2D traceinfo;
                for (int j = -2; j < 2; j++)
                {
                    traceinfo = Physics2D.Raycast(rigidbody2D.position, Quaternion.AngleAxis(j * 35f, Vector3.forward) * direction.normalized, 3f, LayerMask.GetMask("obstacle"));

                    if ((rigidbody2D.position - traceinfo.point).sqrMagnitude < (rigidbody2D.position - nearestPoint).sqrMagnitude)
                    {
                        //    nearestPoint2 = nearestPoint1;
                        nearestPoint1 = nearestPoint;
                        nearestPoint = traceinfo.point;
                    }
                    if (nearestPoint != Vector2.zero || nearestPoint1 != Vector2.zero || nearestPoint2 != Vector2.zero)
                    {
                        lightRendererPointFindingTimeStamp = Time.time;
                    }
                    // Debug.DrawLine(rigidbody2D.position, traceinfo.point,Color.yellow);
                }

                if (nearestPoint != Vector2.zero)
                    SetLightRenderer(nearestPoint);
                // Debug.DrawLine(rigidbody2D.position, nearestPoint,Color.red);
                if (nearestPoint1 != Vector2.zero)
                    SetLightRenderer(nearestPoint1);
                //Debug.DrawLine(rigidbody2D.position, nearestPoint1, Color.red);
                if (nearestPoint2 != Vector2.zero)
                    SetLightRenderer(nearestPoint2);
                //Debug.DrawLine(rigidbody2D.position, nearestPoint2, Color.red);


            }

            // traceinfo.point;
        }
    }
    int rendererIndex;
    void SetLightRenderer(Vector2 inPosition)
    {
        rendererIndex %= lightRenderer.Length;
        lightRenderer[rendererIndex].enabled = true;
        lightRenderer[rendererIndex].SetPosition(1, inPosition);
        lightRendererTimeStamp[rendererIndex] = Time.time;
        rendererIndex++;
    }


    void ManageLightRenderers()
    {
        for (int i = 0; i < lightRenderer.Length; i++)
        {
            lightRenderer[i].SetPosition(0, transform.position);
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
        transform.rotation = starttingRotation;
        collider2D.enabled = true;
        renderer.enabled = true;
        Body.renderer.enabled = true;
        EyeSocket.renderer.enabled = true;
        EyeLight.renderer.enabled = true;
        fingerHead.renderer.enabled = true;
        iris.renderer.enabled = true;
        DesiredPosition = GetBasePosition();
        anchorHead.transform.position = GetBasePosition();
        fingerHead.transform.position = GetBasePosition();
        anchorLine.SetPosition(0, GetBasePosition());
        anchorLine.SetPosition(1, GetBasePosition());
       // timeToMoveBack = 1f;
        timeElapsed = 0f;
        transform.position = GetBasePosition();
        rigidbody2D.velocity = Vector2.zero;
        scaleState = -1;
        anim.SetInteger("scaleState", scaleState);
        scaleState = 0;

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
            AnchorCenterDiePosition = anchorCenter.transform.position;
            MagnisDiePosition = transform.position;
            rigidbody2D.drag = 1000000;
            rigidbody2D.isKinematic = true;
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
        EyeLight.renderer.enabled = false;
        iris.renderer.enabled = false;
      //  scaleState = 0;
      //  anim.SetInteger("scaleState", scaleState);
    }

    void StartRollOver()
    {
        rigidbody2D.drag = 60;
        DisableLightRenderer();
        rigidbody2D.isKinematic = false;
        collider2D.enabled = false;

        
        anchorLine.SetPosition(0, GetBasePosition());
        anchorLine.SetPosition(1, GetBasePosition());
        anchorHead.transform.position = GetBasePosition();
        fingerHead.renderer.enabled = false;
        fingerHead.transform.position = GetBasePosition();
        DesiredPosition = GetBasePosition();
        GameInfo.RollOver();
    }
	// Debug Part

    void OnGUI()
    {
        GUI.color = Color.white;

        GUIStyle _style = GUI.skin.GetStyle("Label");
        _style.alignment = TextAnchor.UpperLeft;
        _style.fontSize = 20;

        if (rigidbody2D.velocity.magnitude > maxSpeed)
            maxSpeed = rigidbody2D.velocity.magnitude;
      //  GUI.Label(new Rect(20, 20, 200, 200), "maxSpeed : " + maxSpeed);
        //print ("DTouchWorldPosition" + DTouchWorldPosition);
        //_style.alignment = TextAnchor.MiddleCenter;
    //    GUI.Label(new Rect(20, 40, 200, 200), "Contact : " + bIsInContact + bIsContactWithWall);
      //  GUI.Label(new Rect(20, 60, 200, 200), "scaleState : " + scaleState);
        //print ("DFinalPosition" + DFinalPosition);
        //_style.alignment = TextAnchor.MiddleCenter;
        //GUI.Label (new Rect (Screen.width - 320, 20, 200, 200), "LB(1):" + Input.touches.GetLowerBound (1) + " UB(0):" + Input.touches.GetUpperBound (1), _style);
    }
}
