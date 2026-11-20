using UnityEngine;
using UnityEngine.Splines;
using System.Collections;

[SelectionBase]
public class ResearcherBehaviour : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    public float idleDuration = 3f;
    public Animator researcherAnimator;
    public Transform researcherIdleStart;
    public Transform researcherJumpStart;
    public Transform crackBottom;
    public Transform portalCrackOnPath;
    public SplineContainer pathSpline;
    public SplineAnimate splineAnimate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPosition = this.transform.position;
        originalRotation = this.transform.rotation;
        researcherAnimator.SetBool("isInStartPosition", true);
    }

    public void IdleBehaviour()
    {
        // TODO: Needs a blocker to not trigger the first time when loading into the scene. Because the researcher already starts by jumping and this method cancels that mid-jump
        splineAnimate.Container = null;
        researcherAnimator.SetBool("hasBeenTeleported", false);
        StartCoroutine(IdleCoroutine());
    }

    public void JumpBehaviour()
    {
        this.transform.position = researcherJumpStart.position;
        researcherAnimator.SetBool("isInStartPosition", true);
        StartCoroutine(JumpCoroutine());
    }

    public void TeleportBehaviour()
    {
        researcherAnimator.SetBool("isInStartPosition", false);
        researcherAnimator.SetBool("hasHitBottom", true);
        //this.transform.position = originalPosition;
        splineAnimate.Container = pathSpline;
        splineAnimate.Restart(true);
        splineAnimate.Play();
    }

    public void WalkBehaviour()
    {
        researcherAnimator.SetBool("hasHitBottom", false);
        researcherAnimator.SetBool("hasBeenTeleported", true);
    }

    IEnumerator IdleCoroutine()
    {
        // Transition from Walking (which ends at the location where researcher comes in contact with cube1) to Idle. During this position, they are moved from that beginning location to the target location
        Vector3 contactLocation = this.transform.position;
        Quaternion contactRotation = this.transform.rotation;
        Vector3 targetLocation = originalPosition;
        Quaternion targetRotation = originalRotation;
        float timer = 0f;
        float transitionDuration = idleDuration;
        while (timer < transitionDuration)
        {
            this.transform.position = Vector3.Lerp(contactLocation, targetLocation, timer / transitionDuration);
            this.transform.rotation = Quaternion.Lerp(contactRotation, targetRotation, timer);
            timer += Time.deltaTime;
            yield return null;
        }

        JumpBehaviour();
    }

    IEnumerator JumpCoroutine()
    {
        //yield on a new YieldInstruction that waits for jumping animation.
        yield return new WaitForSeconds(1.4f);

        // move the rest of the distance towards the collider
        Vector3 researcherRootOffsetDuringJump = new Vector3(0f,0.094f,3.12f);
        Vector3 contactLocation = this.transform.position;
        Vector3 targetLocation = crackBottom.position/* + researcherRootOffsetDuringJump*/; // TODO: Fix his offset during jump so that he actually targets the collider
        float timer = 0f;
        float transitionDuration = 0.5f; //<- This is where you make him fall faster / slower
        while (timer < transitionDuration)
        {
            this.transform.position = Vector3.Lerp(contactLocation, targetLocation, timer / transitionDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        // transform.position = Vector3.MoveTowards(transform.position, targetLocation, 0.2f); // Movement-Speed based alternative to the while loop
    }

    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawSphere(crackBottom.position, 0.5f);
    // }
}
