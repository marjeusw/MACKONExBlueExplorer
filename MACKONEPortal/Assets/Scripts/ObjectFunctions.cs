using UnityEngine;

//handles all the functions that happen when each door is used

public class ObjectFunctions : MonoBehaviour
{

    public GameObject Studio1;
    public GameObject Studio2;
    public GameObject Studio3;
    public GameObject Studio4;


    public void DoorOne()
    {
        Debug.Log("Door1 doin its thing");
        Studio1.SetActive(true);

        Studio2.SetActive(false);
        Studio3.SetActive(false);
        Studio4.SetActive(false);
    }

    public void DoorTwo()
    {
        Debug.Log("Door2 doin its thing");
        Studio2.SetActive(true);

        Studio1.SetActive(false);
        Studio3.SetActive(false);
        Studio4.SetActive(false);
    }

    public void DoorThree()
    {
        Debug.Log("Door3 doin its thing");
        Studio3.SetActive(true);

        Studio1.SetActive(false);
        Studio2.SetActive(false);
        Studio4.SetActive(false);
    }

    public void DoorFour()
    {
        Debug.Log("Door4 doin its thing");
        Studio4.SetActive(true);

        Studio1.SetActive(false);
        Studio2.SetActive(false);
        Studio3.SetActive(false);

    }
}
