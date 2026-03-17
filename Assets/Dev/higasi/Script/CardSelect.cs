using UnityEngine;

public class CardSelect : MonoBehaviour
{
    bool click = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && !click)
        {
            click = true;
            Vector3 mousePos = Input.mousePosition;
            Ray selectRay = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            
            if (Physics.Raycast(selectRay, out hit, 100.0f))
            {
                Debug.Log("hit");
                GameObject hitObj = hit.collider.gameObject;
                if (hitObj != null)
                {
                    Debug.Log("null");
                }
                if (hitObj.CompareTag("Card"))
                {
                    Debug.Log("Card");
                }
            }
        }
        else if (!Input.GetMouseButton(0) && click)
        {
            click = false;
        }
    }
}
