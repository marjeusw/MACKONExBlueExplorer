using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class Pointer : MonoBehaviour
{
    public Transform startPointTransform;

    public Transform staticTransform1;
    public Transform staticTransform2;
    public Transform staticTransform3;
    public Transform staticTransform4;
    public Transform staticTransform5;
    public Transform staticTransform6;

    public Transform anchorTransform1;
    public Transform anchorTransform2;
    public Transform anchorTransform3;
    public Transform anchorTransform4;

    public Transform staticTransform7;
    public Transform staticTransform8;
    public Transform staticTransform9;
    public Transform staticTransform10;
    public Transform staticTransform11;

    public Transform endPointTransform;

    public float pointerBendAmount;

    private Spline spline;

    void Start()
    {
        // Wait for spatial anchors to initialize
        Invoke(nameof(SetupSpline), 1f);
    }

    void SetupSpline()
    {
        spline = GetComponent<SplineContainer>().Spline;

        // Clear existing knots
        spline.Clear();

        // Add all knots in the correct order
        AddAutoKnot(startPointTransform);

        AddAutoKnot(staticTransform1);
        AddAutoKnot(staticTransform2);
        AddAutoKnot(staticTransform3);
        AddAutoKnot(staticTransform4);
        AddAutoKnot(staticTransform5);
        AddAutoKnot(staticTransform6);

        AddAutoKnot(anchorTransform1);
        AddAutoKnot(anchorTransform2);
        AddAutoKnot(anchorTransform3);
        AddAutoKnot(anchorTransform4);

        AddAutoKnot(staticTransform7);
        AddAutoKnot(staticTransform8);
        AddAutoKnot(staticTransform9);
        AddAutoKnot(staticTransform10);
        AddAutoKnot(staticTransform11);

        AddAutoKnot(endPointTransform);

        // Enable spline visualization
        GetComponent<SplineExtrude>().enabled = true;
    }

    void AddAutoKnot(Transform target)
    {
        BezierKnot knot = new BezierKnot(
            (float3)target.position
        );

        spline.Add(knot, TangentMode.AutoSmooth);
    }

    // Call this after spatial anchor positions have been updated
    public void FindSetupObject()
    {
        if (spline == null)
            return;

        Transform[] transforms =
        {
            startPointTransform,
            staticTransform1,
            staticTransform2,
            staticTransform3,
            staticTransform4,
            staticTransform5,
            staticTransform6,
            anchorTransform1,
            anchorTransform2,
            anchorTransform3,
            anchorTransform4,
            staticTransform7,
            staticTransform8,
            staticTransform9,
            staticTransform10,
            staticTransform11,
            endPointTransform
        };

        for (int i = 0; i < transforms.Length; i++)
        {
            BezierKnot knot = spline[i];

            knot.Position = (float3)transforms[i].position;

            spline.SetKnot(i, knot);
        }
    }
}
