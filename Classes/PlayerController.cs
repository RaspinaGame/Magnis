using UnityEngine;
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
	
	private string DTouchWorldPosition;
	private string DFinalPosition;

	private Vector3 DesiredPosition;
    private Vector3 starttingScale;
    private Vector3 desiredScale;
    private int scaleState;
    private Quaternion starttingRotation;
    private float starttingMovmentSpeed;
    private bool bIsInContact;
    private float newMovmentSpeed;

    Vector2 collisionSurfaceNormal;
    Vector2 collisionpoint;
    MoverComponent colliderMoverComponent;

    ///////////////////////////////
    float maxSpeed;
    float lastVelocityAdjustmentTime;

	// Use this for initialization
	void Start () 
	{
        anchorCenter.transform.position = GetBasePosition();
        starttingScale = transform.localScale;
        starttingRotation = transform.rotation;
        desiredScale = starttingScale;
        starttingMovmentSpeed = MovmentSpeed;
        //StartCoroutine("UpdateScale");

        
	}

	Vector3 GetBasePosition()
	{
	
		//Camera MyCamera = FindObjectOfType<Camera> ();
		//return MyCamera.transform.position;
		//return transform.parent.position;
		return new Vector3(0,0,-2);
	}

    IEnumerator UpdateScale()
    {
        while (true)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, desiredScale, Time.deltaTime * 3f);
            yield return null;
        }
    }

	Vector3 GetInput()
	{
		if (Application.isEditor)
		{
			return Input.mousePosition ;
		}
		else if ( Input.touchCount > 0)
		{
			return Input.GetTouch(0).position ;
		}
		return new Vector3 (-1000, -1000, -1000);
	}

	void ProcessTouch(Vector3 InputPosition)
	{
		Vector3 FinalPosition, TouchWorldPosition, BasePosition;
		if ( InputPosition != new Vector3 (-1000, -1000, -1000) )
		{
            
			TouchWorldPosition = Camera.main.ScreenToWorldPoint(InputPosition);
            TouchWorldPosition.x = Mathf.Clamp(TouchWorldPosition.x, Xmin, Xmax);
            TouchWorldPosition.y = Mathf.Clamp(TouchWorldPosition.y, Ymin, Ymax);
            anchorLine.SetPosition(1, new Vector3(TouchWorldPosition.x, TouchWorldPosition.y, -1));
            fingerHead.transform.position = new Vector3(TouchWorldPosition.x, TouchWorldPosition.y, -1);
			BasePosition = GetBasePosition();
			TouchWorldPosition.z = -2;

			//FinalPosition = BasePosition -  TouchWorldPosition - transform.position;

			//FinalPosition = BasePosition + BasePosition - TouchWorldPosition;
            FinalPosition = -TouchWorldPosition;
		//	FinalPosition.x = Mathf.Clamp (FinalPosition.x, Xmin, Xmax);
		//	FinalPosition.y = Mathf.Clamp (FinalPosition.y, Ymin, Ymax);

			//transform.Translate(FinalPosition.x , FinalPosition.y , 0,Space.World);
          // if (!bIsInContact)
			DesiredPosition = FinalPosition;


			/// debug
			//		print (transform.parent.name + TouchWorldPosition  + BasePosition.ToString() + FinalPosition.ToString());
			DTouchWorldPosition = TouchWorldPosition.ToString();
			DFinalPosition = FinalPosition.ToString();
		}
		//
	}

    // Update is called once per frame
	void Update ()
	{
		if ( !GameInfo.IsGamePaused() )
		{
			
			//print ( transform.position + " " + DesiredPosition + " " + MovmentSpeed );
         //   rigidbody2D.position = Vector3.Lerp(rigidbody2D.position, DesiredPosition, 0.1f);
            ProcessTouch ( GetInput() );
            anchorLine.SetPosition(0, new Vector3(DesiredPosition.x, DesiredPosition.y, -1));
            anchorHead.transform.position = new Vector3(DesiredPosition.x, DesiredPosition.y, -1);
            

         //   if (desiredScale.x - transform.localScale.x > 0.05)
         //       transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, 0.15f);
          //  else
           
		}
		if (Input.GetKey (KeyCode.Escape)) 
			Application.LoadLevel (0);
	}

    void FixedUpdate()
    {
        if (!GameInfo.IsGamePaused())
        {
           // if (transform.localScale != desiredScale)
            //
            MoveToDesiredPosition(DesiredPosition);
           
        }
 
    }

    void LateUpdate()
    {
        if (!GameInfo.IsGamePaused())
        {
            
            // if (transform.localScale == desiredScale)
           // transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, 0.2f);
        }
    }


    Vector3 velocity;
    Vector3 lastVelocity;
    void MoveToDesiredPosition(Vector2 inDesiredPosition ) 
    {
        Vector2 newposition;
       // Ray2D traceline;
        RaycastHit2D traceinfo;

       
       
       // newposition = Vector2.MoveTowards(rigidbody2D.position, inDesiredPosition, MovmentSpeed);
       // newMovmentSpeed = Mathf.Lerp(newMovmentSpeed, MovmentSpeed, 0.8f);

        //Vector3 targetPosition = inDesiredPosition;
        
         
     //   if (Time.fixedTime - lastVelocityAdjustmentTime >= 0.03f)
    //    {
            velocity = rigidbody2D.velocity;
            newposition = Vector3.SmoothDamp(rigidbody2D.position, inDesiredPosition, ref velocity, 0.015f, 3000);
          //  lastVelocityAdjustmentTime = Time.fixedTime;
      //  }
        if (bIsInContact)
        {

            traceinfo = Physics2D.Raycast(rigidbody2D.position, velocity, 10f, LayerMask.GetMask("obstacle"));
            print(traceinfo.collider);
            if (traceinfo.collider != null)
            {
                Vector3 newVelocity;
               // float angle = Vector2.Angle(traceinfo.normal, velocity);
                //float angle = Vector2.Angle(collisionSurfaceNormal, velocity);
                

                Debug.DrawRay(rigidbody2D.position, lastVelocity, Color.yellow);
               
                
                //rigidbody2D.velocity = newVelocity;
               // velocity = newVelocity;




                velocity = (velocity + lastVelocity) / 2f;
                lastVelocity = velocity;
             //   velocity = newVelocity;
                newVelocity = Vector3.Project(velocity, Vector2.right);
                
                if (newVelocity.magnitude < 300f)
                {
                   
                 //   velocity = newVelocity;
                   // velocity = Vector3.zero;
                    velocity.x = Mathf.Lerp(0f, velocity.x, Mathf.Pow(newVelocity.magnitude/300f, 2f));
                }
                Debug.DrawRay(rigidbody2D.position, Vector2.right * 10f, Color.green);
                //if (angle >= 90 && angle <= 135)
                //{
                //    // print(Vector2.Angle(collisionSurfaceNormal, velocity));
                //    //velocity = Vector3.ClampMagnitude(velocity, 1f);
                //    // velocity = ( collisionSurfaceNormal.normalized) * velocity.magnitude;
                //}
                //else if (angle > 145)
                //{
                //    //  velocity = Vector3.ClampMagnitude(velocity, 0.1f);
                //}

                rigidbody2D.velocity = velocity;
               
            }
            else
            {
                rigidbody2D.velocity = velocity;
            }
           // Debug.DrawRay(rigidbody2D.position, collisionSurfaceNormal, Color.blue);
            //Debug.DrawRay(rigidbody2D.position, (Quaternion.AngleAxis(90, Vector3.forward) * collisionSurfaceNormal), Color.red);
        }
        else
        {
            rigidbody2D.velocity = velocity;
        }
          
            Debug.DrawRay(rigidbody2D.position, velocity);
            
            
        //rigidbody2D.velocity = velocity;
            //  newposition = inDesiredPosition - rigidbody2D.position ;
            //  if (newposition.sqrMagnitude > 2)
     //   newposition = Vector2.Lerp(rigidbody2D.position, inDesiredPosition, MovmentSpeed);
       // rigidbody2D.position = newposition;
      //  rigidbody2D.MovePosition(newposition);//= Vector3.Lerp(rigidbody2D.position, inDesiredPosition, 0.1f);

        //Vector3.SmoothDamp
    }

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.name == "LevelSplitter") 
		{
			GameInfo.LevelFinished();
		}
        else
        {
          // rigidbody2D.AddForce(collision.contacts[0].normal * 1000);
          //  collisionpoint = collision.contacts[0].point;
         //   DesiredPosition = new Vector3(temp.x,temp.y,-2);
         //   MovmentSpeed = 0.1f;
            
          //  newMovmentSpeed = 0.01f;
        //    colliderMoverComponent = collision.gameObject.GetComponentInParent<MoverComponent>();
            collisionSurfaceNormal = collision.contacts[0].normal;

         //   transform.parent = collision.gameObject.transform;
            bIsInContact = true;
            CancelInvoke("Scale");
			InvokeRepeating("Scale",Time.deltaTime,0.6f);
        }
		//foreach (ContactPoint2D contact in collision.contacts) 
		//{
		//	Debug.DrawRay(contact.point, contact.normal, Color.white);
		//}

	}

    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.name == "LevelSplitter")
        {
            //GameInfo.LevelFinished();
        }
        else
        {
            //collisionpoint = coll.contacts[0].point;
            collisionSurfaceNormal = coll.contacts[0].normal;
            //colliderMoverComponent = coll.gameObject.GetComponentInParent<MoverComponent>();
        }

    }



    void Scale()
    {
        if (scaleState < 7)
            scaleState++;
        anim.SetInteger("scaleState", scaleState);
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        bIsInContact = false;
        CancelInvoke("Scale");
        MovmentSpeed = starttingMovmentSpeed;
        transform.parent = null;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "BoundaryTirigger")
        {
            GameInfo.PauseGame();
            Invoke("GameOver", 1.5f);
        }
    }
    
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "LevelSplitter")
        {
            GameInfo.LevelFinished();
        }
        else
        {
            GameInfo.PauseGame();
            Invoke("GameOver", 1.5f);
        }

    }



	void GameOver()
	{
		GameInfo.GameOver();
        transform.localScale = starttingScale;
        transform.rotation = starttingRotation;
        desiredScale = starttingScale;
        MovmentSpeed = starttingMovmentSpeed;
       // anchorLine.SetPosition(0, GetBasePosition());
       // anchorLine.SetPosition(1, GetBasePosition());
       // anchorHead.transform.position = GetBasePosition();
        transform.position = DesiredPosition;
        rigidbody2D.velocity = Vector2.zero;
        scaleState = 0;
        anim.SetInteger("scaleState", scaleState);
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
        GUI.Label(new Rect(20, 20, 200, 200), "maxSpeed : " + maxSpeed);
        //print ("DTouchWorldPosition" + DTouchWorldPosition);
    //    _style.alignment = TextAnchor.MiddleCenter;
    //    GUI.Label(new Rect(0, 20, 200, 200), "DFinalPosition" + DFinalPosition, _style);
        //print ("DFinalPosition" + DFinalPosition);
        //_style.alignment = TextAnchor.MiddleCenter;
        //GUI.Label (new Rect (Screen.width - 320, 20, 200, 200), "LB(1):" + Input.touches.GetLowerBound (1) + " UB(0):" + Input.touches.GetUpperBound (1), _style);
    }
}
