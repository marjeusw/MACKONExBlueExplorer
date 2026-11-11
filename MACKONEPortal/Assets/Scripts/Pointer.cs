
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using System.Collections;

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

    [Header("Components")]
    [SerializeField] private SplineAnimate splineAnimate;
    [SerializeField] private SplineExtrude splineExtrude;

    [Header("Initialisation")]
    [SerializeField] private float initialDelay = 1f;

    private Spline spline;
    private SplineContainer splineContainer;

    private Transform[] GetKnotTransforms()
    {
        return new Transform[]
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
    }

    private void Start()
    {
        // Spline Animate should be disabled in the Inspector.
        StartCoroutine(SetupSplineRoutine());
    }

    private IEnumerator SetupSplineRoutine()
    {
        // Wait for spatial anchors to initialise.
        yield return new WaitForSeconds(initialDelay);

        splineContainer = GetComponent<SplineContainer>();

        if (splineContainer == null)
        {
            Debug.LogError("Pointer: No SplineContainer found!");
            yield break;
        }

        spline = splineContainer.Spline;

        Transform[] transforms = GetKnotTransforms();

        // Validate all transforms before generating spline.
        foreach (Transform target in transforms)
        {
            if (target == null)
            {
                Debug.LogError(
                    "Pointer: One or more knot transforms are missing!"
                );
                yield break;
            }

            if (!IsFinite(target.position))
            {
                Debug.LogError(
                    "Pointer: Invalid position on " + target.name
                );
                yield break;
            }
        }

        // Clear existing knots.
        spline.Clear();

        // Add automatically smoothed knots.
        foreach (Transform target in transforms)
        {
            AddAutoKnot(target);
        }

        if (spline.Count < 2)
        {
            Debug.LogError("Pointer: Not enough spline knots!");
            yield break;
        }

        // Enable spline visualisation.
        if (splineExtrude != null)
        {
            splineExtrude.enabled = true;
        }

        // Enable animation after spline generation.
        if (splineAnimate != null)
        {
            splineAnimate.enabled = true;
        }

        Debug.Log("Pointer: Spline successfully generated!");
    }

    private void AddAutoKnot(Transform target)
    {
        Vector3 localPosition =
            splineContainer.transform.InverseTransformPoint(
                target.position
            );

        BezierKnot knot = new BezierKnot(
            (float3)localPosition
        );

        // Automatically calculate smooth tangents.
        spline.Add(knot, TangentMode.AutoSmooth);
    }

    private bool IsFinite(Vector3 p)
    {
        return
            !float.IsNaN(p.x) && !float.IsInfinity(p.x) &&
            !float.IsNaN(p.y) && !float.IsInfinity(p.y) &&
            !float.IsNaN(p.z) && !float.IsInfinity(p.z);
    }

    // Call after spatial anchor positions have been updated.
    public void FindSetupObject()
    {
        if (spline == null || splineContainer == null)
            return;

        Transform[] transforms = GetKnotTransforms();

        if (spline.Count != transforms.Length)
            return;

        for (int i = 0; i < transforms.Length; i++)
        {
            if (transforms[i] == null)
                return;

            Vector3 worldPosition = transforms[i].position;

            if (!IsFinite(worldPosition))
            {
                Debug.LogError(
                    "Pointer: Invalid anchor position: " +
                    transforms[i].name
                );
                return;
            }

            Vector3 localPosition =
                splineContainer.transform.InverseTransformPoint(
                    worldPosition
                );

            BezierKnot knot = spline[i];

            knot.Position = (float3)localPosition;

            spline.SetKnot(i, knot);
        }
    }
}

