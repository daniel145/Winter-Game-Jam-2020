using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Effect : MonoBehaviour
{
    public bool Visible { get; private set; }
    private Image image;
    
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        Color temp = image.color;
        temp.a = 0f;
        image.color = temp;
        Visible = false;
    }

    public void ChangeVisibility(bool visible)
    {
        if (visible == Visible)
            return;

        StopAllCoroutines();
        if (visible)
            StartCoroutine(InverseFlash());
        else
            StartCoroutine(Flash());

        Visible = visible;
    }

    // Makes the image flash twice in a duration of 0.5 sec
    // Turns off in the end
    private IEnumerator Flash()
    {
        Color imageColor = image.color;
        imageColor.a = 0f;
        image.color = imageColor;
        yield return new WaitForSeconds(0.25f);

        imageColor.a = 1f;
        image.color = imageColor;
        yield return new WaitForSeconds(0.25f);

        imageColor.a = 0f;
        image.color = imageColor;
        yield return 0;
    }

    // Makes the image flash twice in a duration of 0.5 sec
    // Turns on in the end
    private IEnumerator InverseFlash()
    {
        Color imageColor = image.color;
        imageColor.a = 1f;
        image.color = imageColor;
        yield return new WaitForSeconds(0.25f);

        imageColor.a = 0f;
        image.color = imageColor;
        yield return new WaitForSeconds(0.25f);

        imageColor.a = 1f;
        image.color = imageColor;
        yield return 0;
    }
}
