using UnityEngine;
using System.Collections;

public class Tut_MagnisController : MonoBehaviour {

    public bool isFreeTransform = true;
    public float MovmentSpeed = 0.2f;
    public float Xmax = 7, Xmin = -7;
    public float Ymax = 1000, Ymin = -1000;
    public LineRenderer anchorLine;
    public GameObject fingerHead;
    public GameObject anchorCenter;
    public GameObject anchorHead;
    public GameObject Body;
    private Vector3 DesiredPosition;

    MoverComponent anchorCenterMoverComponent;

    public LineRenderer[] lightRenderer;
    float[] lightRendererTimeStamp;

    FXController fXController;
    Vector3 BasePosition;

    public AudioSource bgMusic;
   // public 
    // Use this for initialization
    void Start()
    {
//        DisableLightRenderer();
        lightRendererTimeStamp = new float[lightRenderer.Length];
        anchorCenterMoverComponent = anchorCenter.GetComponent<MoverComponent>();
        resertToInitial();
      //  fXController = GetComponent<FXController>();

        //if (PlayerPrefs.GetInt("sfx", 1) == 1)
        //{
        //    AudioListener.volume = 1;
        //}
        //else
        //{
        //    AudioListener.volume = 0;
        //}

       // bgMusic = GetComponent<AudioSource>();
        bgMusic.ignoreListenerVolume = true;
        if (PlayerPrefs.GetInt("music", 1) == 1)
        {
            bgMusic.mute = false;
        }
        else
        {
            bgMusic.mute = true;
        }
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


    Vector3 FinalPosition;
    void ProcessTouch()
    {
            BasePosition = GetBasePosition();
            FinalPosition = BasePosition + BasePosition - fingerHead.transform.position;
            DesiredPosition = FinalPosition;
    }

    // Update is called once per frame
    void Update()
    {

        anchorCenterMoverComponent.Move();
        ProcessTouch();
        anchorLine.SetPosition(0, new Vector3(DesiredPosition.x, DesiredPosition.y, -2));
        anchorLine.SetPosition(1, new Vector3(fingerHead.transform.position.x, fingerHead.transform.position.y, -2));
        anchorHead.transform.position = new Vector3(DesiredPosition.x, DesiredPosition.y, -2);
        if (!isFreeTransform)
        {
            if (rigidbody2D.velocity.x > 0)
                Body.transform.rotation = Quaternion.Slerp(Body.transform.rotation, Quaternion.AngleAxis(Vector3.Angle(Vector3.down, rigidbody2D.velocity.normalized - Vector2.up * 0.5f), Vector3.forward), 0.08f);
            else
                Body.transform.rotation = Quaternion.Slerp(Body.transform.rotation, Quaternion.AngleAxis(-Vector3.Angle(Vector3.down, rigidbody2D.velocity.normalized - Vector2.up * 0.5f), Vector3.forward), 0.08f);
        }


     //   ManageLightRenderers();
    }

   

    void FixedUpdate()
    {
        if (!isFreeTransform)
            MoveToDesiredPosition(DesiredPosition);
    }

    void LateUpdate()
    {
       
    }


    Vector3 velocity;
    Vector3 lastVelocity;
    void MoveToDesiredPosition(Vector2 inDesiredPosition)
    {
        velocity = rigidbody2D.velocity;
        Vector3.SmoothDamp(rigidbody2D.position, inDesiredPosition, ref velocity, 0.015f, 3000f, Time.fixedDeltaTime);
        rigidbody2D.velocity = velocity;
    }

    void resertToInitial()
    {
        //transform.rotation = starttingRotation;
        //collider2D.enabled = true;
        //renderer.enabled = true;
        //Body.renderer.enabled = true;
        //EyeSocket.renderer.enabled = true;
        ////  EyeLight.renderer.enabled = true;
        //EyeLight.SetActive(true);
        ////  fingerHead.renderer.enabled = true;

        //fingerHead.SetActive(true);
        //iris.renderer.enabled = true;
        //DesiredPosition = GetBasePosition();
        //anchorHead.transform.position = GetBasePosition();
        //fingerHead.transform.position = GetBasePosition();
        //anchorLine.SetPosition(0, GetBasePosition());
        //anchorLine.SetPosition(1, GetBasePosition());
        //// timeToMoveBack = 1f;
        //timeElapsed = 0f;
        //transform.position = GetBasePosition();
        //rigidbody2D.velocity = Vector2.zero;
        //scaleState = -1;
        //anim.SetInteger("scaleState", scaleState);
        //scaleState = 0;

    }

//    void OnTriggerEnter2D(Collider2D other)
//    {

//       if (other.gameObject.tag == "obstacle")
//        {
//            StopCoroutine("lightRendererPointFinder");
//        }

//    }

//    void OnTriggerStay2D(Collider2D other)
//    {
//        if (other.gameObject.tag == "obstacle" && Time.time - lightRendererPointFindingTimeStamp > 0.3f)// &&
//        {
//            StartCoroutine(lightRendererPointFinder(other));
//        }
//    }

//    float lightRendererPointFindingTimeStamp;

//    private IEnumerator lightRendererPointFinder(Collider2D other)
//    {
//        Collider2D[] colliders = other.gameObject.GetComponents<Collider2D>();

//        // print(colliders.Length);

//        for (int i = 0; i < colliders.Length; i++)
//        {
//            Vector2 nearestPoint = Vector2.zero;
//            Vector2 nearestPoint1 = Vector2.zero;
//            Vector2 nearestPoint2 = Vector2.zero;

//            Vector2 direction = colliders[i].bounds.center;
//            // nearestPoint = colliders[i].bounds.center;
//            direction -= rigidbody2D.position;

//            // 

//            // Vector2.
//            RaycastHit2D traceinfo;
//            for (int j = -2; j < 2; j++)
//            {
//                traceinfo = Physics2D.Raycast(rigidbody2D.position, Quaternion.AngleAxis(j * 50f, Vector3.forward) * direction.normalized, 3f, LayerMask.GetMask("obstacle"));

//                if ((rigidbody2D.position - traceinfo.point).sqrMagnitude < (rigidbody2D.position - nearestPoint).sqrMagnitude)
//                {
//                    //    nearestPoint2 = nearestPoint1;
//                    nearestPoint1 = nearestPoint;
//                    nearestPoint = traceinfo.point;
//                }
//                if (nearestPoint != Vector2.zero || nearestPoint1 != Vector2.zero || nearestPoint2 != Vector2.zero)
//                {
//                    lightRendererPointFindingTimeStamp = Time.time;
//                }

//                yield return new WaitForEndOfFrame();
//                // Debug.DrawLine(rigidbody2D.position, traceinfo.point,Color.yellow);
//            }

//            if (nearestPoint != Vector2.zero)
//                SetLightRenderer(nearestPoint);
//            // Debug.DrawLine(rigidbody2D.position, nearestPoint,Color.red);
//            if (nearestPoint1 != Vector2.zero)
//                SetLightRenderer(nearestPoint1);
//            //Debug.DrawLine(rigidbody2D.position, nearestPoint1, Color.red);
//            if (nearestPoint2 != Vector2.zero)
//                SetLightRenderer(nearestPoint2);
//            //Debug.DrawLine(rigidbody2D.position, nearestPoint2, Color.red);

//            yield return new WaitForEndOfFrame();
//        }

//    }

//    int rendererIndex;
//    void SetLightRenderer(Vector2 inPosition)
//    {
//        rendererIndex %= lightRenderer.Length;
//        lightRenderer[rendererIndex].enabled = true;
////        fXController.LightningSFX();
//        lightRenderer[rendererIndex].SetPosition(1, inPosition);
//        lightRendererTimeStamp[rendererIndex] = Time.time;
//        rendererIndex++;
//    }


//    void ManageLightRenderers()
//    {
//        for (int i = 0; i < lightRenderer.Length; i++)
//        {
//            lightRenderer[i].SetPosition(0, transform.position);
//            if (Time.time - lightRendererTimeStamp[i] > 0.35f)
//            {
//                lightRenderer[i].enabled = false;
//            }
//        }
//    }

//    void DisableLightRenderer()
//    {
//        for (int i = 0; i < lightRenderer.Length; i++)
//        {
//            lightRenderer[i].enabled = false;
//        }
//    }

}
