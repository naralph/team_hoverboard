using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedObject : MonoBehaviour
{
    [SerializeField] int timeToWait = 50;
    int timeWaited = 0;
    bool isSelected = false;
    //object to update for reticle
    private reticle theReticle;

    //grabes the reticle object to show timer status
    public void selected(reticle grabbedReticle)
    {
        gameObject.GetComponent<Renderer>().material.color = Color.blue;
        isSelected = true;
        theReticle = grabbedReticle;
    }

    public void deSelected()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        isSelected = false;
        timeWaited = 0;
    }

    void FixedUpdate()
    {
        if (isSelected)
        {
            timeWaited++;
            if (timeWaited >= timeToWait)
            {
                gameObject.transform.localScale *= 0.5f;
            }
            float ratio = timeWaited / timeToWait;
            theReticle.updateReticle(ratio);
        }
    }
}
