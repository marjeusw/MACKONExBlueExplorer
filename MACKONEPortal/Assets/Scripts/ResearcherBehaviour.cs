using UnityEngine;
using UnityEngine.Splines; 

public class ResearcherBehaviour : MonoBehaviour
{
    public bool hasHitBottom;
    public bool hasBeenTeleported;
    public bool isInStartPosition;
    public Transform researcherIdleStart;
    public Transform crackBottom;
    public Transform portalCrackOnPath;
    public SplineContainer jumpSpline;
    public SplineContainer pathSpline;
    public SplineAnimate splineAnimate;
    private int IdleStateHash;
    private int JumpStateHash;
    private int WalkStateHash;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.GetComponent<Animator>();

        IdleStateHash = Animator.StringToHash("Researcher_Idle");
        JumpStateHash = Animator.StringToHash("Researcher_Jump");
        WalkStateHash = Animator.StringToHash("Researcher_Walk");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.name == "pathcube1")
        {
            isInStartPosition = true;
            splineAnimate.Container = jumpSpline;
            isInStartPosition = false;
        }
        else if(other.name == "pathcube2")
        {
            hasHitBottom = true;
            splineAnimate.Container = pathSpline;
            hasHitBottom = false;
        }
        else if(other.name == "pathcube3")
        {
            hasBeenTeleported = true;
        }
    }
}
