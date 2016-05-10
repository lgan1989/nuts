using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Cover : MonoBehaviour
{

    public float fadeSpeed = 10.0f;
    public float fadeTime = 0.01f;
    public bool fadeIn = true;
    public float fade = 0;
    public Color color;
    public float transparency = 0.3f;
    public static int zIndex = 4;

    // Use this for initialization
    void Start()
    {
        fade = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeIn && transparency - fade > 0.00001f)
        {
            fade = Mathf.SmoothDamp(fade, transparency, ref fadeSpeed, fadeTime);
            color.a = fade;
            gameObject.GetComponent<Renderer>().material.SetColor("_TintColor", color);
        }

    }

    void OnMouseOver()
    {

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }


        if (Logic.selectedPawn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject selector = GameObject.Find("Selection");

                Logic.selectedPawn.FindPathAndMoveTo(Selector.gridPosition);

                Logic.control.CurrentStatus = Control.ControlStatus.Moving;

            }
            else if (Input.GetMouseButtonDown(1))
            {
                Logic.DeselectPawn();
            }
        }

    }


}
