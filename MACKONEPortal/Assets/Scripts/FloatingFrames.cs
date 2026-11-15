using UnityEngine;

public class FloatingFrames : MonoBehaviour
{
    private bool IsRising = false;
    public float plusHeight = 0.07f;
    public float minusHeight = -0.03f;
    private float originalYPos;
    private float currentYPos;
    public float movementSpeed = 0.04f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalYPos = this.transform.position.y;
        currentYPos = originalYPos + Random.Range(minusHeight, plusHeight);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsRising)
        {
            if (currentYPos < originalYPos + plusHeight)
            {
                currentYPos += Time.deltaTime * movementSpeed;
            }
            else
            {
                IsRising = false;
            }
        }
        else
        {
            if (currentYPos > originalYPos + minusHeight)
            {
                currentYPos -= Time.deltaTime * movementSpeed;
            }
            else
            {
                IsRising = true;
            }
        }

        this.transform.position = new Vector3(this.transform.position.x, currentYPos, this.transform.position.z);
    }


}
