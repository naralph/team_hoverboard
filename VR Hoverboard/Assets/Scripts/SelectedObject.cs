using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectedObject : MonoBehaviour
{
    [SerializeField] int timeToWait = 50;
    int timeWaited = 0;
    bool isSelected = false;
    //object to update for reticle
    private reticle theReticle;

    //grabes the reticle object to show timer status
    public void selected(reticle grabbedReticle)
    {
        selectedFuntion();
        isSelected = true;
        theReticle = grabbedReticle;
    }

    //What the class actually does with the object while selected(if applicable), inherited class must fill this out
    public abstract void selectedFuntion();

    //deals with leftovers from selecting the object when you look away
    public void deSelected()
    {
        deSelectedFunction();
        theReticle.updateReticle(0);
        isSelected = false;
        timeWaited = 0;
    }

    //Cleans up what the class actually does(if applicable), inherited class must fill this out
    public abstract void deSelectedFunction();

    //what the class actually does when select is successful, inherited class must fill this out
    public abstract void selectSuccessFunction();

    void FixedUpdate()
    {
        if (isSelected)
        {
            timeWaited++;
            if (timeWaited >= timeToWait)
            {
                selectSuccessFunction();
                theReticle.updateReticle(0);
                timeWaited = 0;
            }
            float ratio = (float)timeWaited / (float)timeToWait;
            theReticle.updateReticle(ratio);
        }
    }
}
