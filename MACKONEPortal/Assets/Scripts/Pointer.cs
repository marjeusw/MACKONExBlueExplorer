using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class Pointer : MonoBehaviour
{
    public Transform rollercoasterTransform;
    public Transform anchorTransform1;
    public Transform anchorTransform2;
    public Transform anchorTransform3;
    public Transform anchorTransform4;
    public float pointerBendAmount;

    private Spline spline;
    private BezierKnot rollercoasterStartKnot;
    private BezierKnot anchorKnot1;
    private BezierKnot anchorKnot2;
    private BezierKnot anchorKnot3;
    private BezierKnot anchorKnot4;

    void Start()
    {
        //This should be put in a method that runs after we set up the spatial anchors instead of it being in start
        Invoke("SetupSpline", 1);
    }

    void SetupSpline()
    {
        spline = GetComponent<SplineContainer>().Spline;
        //adding the starting knot at index 0
        spline.Add(rollercoasterStartKnot);
        //inserting the anchor knot at index 1-4
        spline.Insert(1, anchorKnot1);
        spline.Insert(2, anchorKnot2);
        spline.Insert(3, anchorKnot3);
        spline.Insert(4, anchorKnot4);

        //putting this here for testing purposes
        FindSetupObject();
    }
    //this also needs to be run after spatial anchor setup
    public void FindSetupObject()
    {
        //here specify UnityEngine.Vector3 to avoid an error with the SystemNumerics
        rollercoasterStartKnot.Position = new UnityEngine.Vector3(rollercoasterTransform.position.x, rollercoasterTransform.position.y, rollercoasterTransform.position.z);
        anchorKnot1.Position = anchorTransform1.position;
        anchorKnot2.Position = anchorTransform2.position;
        anchorKnot3.Position = anchorTransform3.position;
        anchorKnot4.Position = anchorTransform4.position;

        //floats not final
        rollercoasterStartKnot.TangentOut = new float3(0f, pointerBendAmount, 1f);
        anchorKnot1.TangentIn = new float3(0f, pointerBendAmount, -1f);
        anchorKnot2.TangentIn = new float3(0f, pointerBendAmount, -1f);
        anchorKnot3.TangentIn = new float3(0f, pointerBendAmount, -1f);
        anchorKnot4.TangentOut = new float3(0f, pointerBendAmount, -1f);

        spline.SetKnot(0, rollercoasterStartKnot);
        spline.SetKnot(1, anchorKnot1);
        spline.SetKnot(2, anchorKnot2);
        spline.SetKnot(3, anchorKnot3);
        spline.SetKnot(4, anchorKnot4);

        //setting up how the spline curve should look
        //for the start and the end of the spline we want to use mirrored Bezier curves to make things look even
        //in the middle we mix it up with continuous for more interesting paths
        //WIP!!! TO FIX: the spline doesn't loop correctly
        spline.SetTangentMode(0, mode: TangentMode.Mirrored, BezierTangent.Out);
        spline.SetTangentMode(1, mode: TangentMode.Continuous, BezierTangent.In);
        spline.SetTangentMode(2, mode: TangentMode.Continuous, BezierTangent.In);
        spline.SetTangentMode(3, mode: TangentMode.Continuous, BezierTangent.In);
        spline.SetTangentMode(4, mode: TangentMode.Mirrored, BezierTangent.Out);

        //this is for rendering of the visuals for the spline, whether that is a mesh or something else
        GetComponent<SplineInstantiate>().enabled = true;
    }
}
