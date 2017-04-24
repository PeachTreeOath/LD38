using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorRadar : MonoBehaviour
{

    public GameObject Meteor;


    private Camera currentCamera;
    public float yOffset;
    public GameObject TargetPrefab;

    [HideInInspector]
    public GameObject targetInstance;

    private float _timeSincePulseStarted = 0.0f;
    public float _pulseDuration;
    public Color _pulseStartColor;
    public Color _pulseEndColor;
    private bool _reverse;
    public int radarLevel;

    //Test change

    // Use this for initialization
    void Start()
    {
        currentCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        //RAD TIER 2 - Used for rotating the arrow to the direction of the meteor
        if (radarLevel > 1)
        {
            float angle = Mathf.Atan2(Meteor.GetComponent<Meteor>().moveDir.x,
                Meteor.GetComponent<Meteor>().moveDir.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle + 180.0f, Vector3.back);
        }

        //RAD TIER 3 - used to show move to location.
        if (radarLevel > 2)
        {
            targetInstance = Instantiate<GameObject>(TargetPrefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //RAD TIER 1
        if (radarLevel > 0 && Meteor != null && Camera.current != null)
        {
            Vector3 screenPos = currentCamera.WorldToScreenPoint(Meteor.transform.position);
            transform.position = currentCamera.ScreenToWorldPoint(new Vector3(screenPos.x, Screen.height - 50, 10.0f));
        }

        //RAD TIER 3
        if (radarLevel > 2 && Meteor != null)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(Meteor.transform.position, Meteor.GetComponent<Meteor>().moveDir);
            Vector2 position = new Vector2(0, -100);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.tag == "Ground")
                {
                    if (hit.point.y > position.y)
                    {
                        position = hit.point;

                    }
                }
            }
            targetInstance.transform.position = new Vector3(position.x, position.y, -1.0f);
            PulsateTarget();
        }
    }

    void PulsateTarget()
    {
        if (_reverse)
        {
            if (Time.time > _timeSincePulseStarted + _pulseDuration)
            {
                float normalizedTime = Time.time / (_timeSincePulseStarted + _pulseDuration);
                if (normalizedTime < 1.0f)
                {
                    Color color = Color.Lerp(_pulseEndColor, _pulseStartColor, normalizedTime);
                    targetInstance.GetComponent<SpriteRenderer>().color = color;
                }
                else
                {
                    targetInstance.GetComponent<SpriteRenderer>().color = _pulseStartColor;
                    _reverse = false;
                    _timeSincePulseStarted = Time.time;
                }
            }
        }
        else
        {
            if (Time.time > _timeSincePulseStarted + _pulseDuration)
            {
                float normalizedTime = Time.time / (_timeSincePulseStarted + _pulseDuration);
                if (normalizedTime < 1.0f)
                {
                    Color color = Color.Lerp(_pulseStartColor, _pulseEndColor, normalizedTime);
                    targetInstance.GetComponent<SpriteRenderer>().color = color;
                }
                else
                {
                    targetInstance.GetComponent<SpriteRenderer>().color = _pulseEndColor;
                    _reverse = true;
                    _timeSincePulseStarted = Time.time;
                }
            }
        }
    }

    public void DestroyRadar()
    {
        //RAD TIER 3
        if (radarLevel > 2)
            Destroy(targetInstance);

        //RAD TIER 1
        if (radarLevel > 0)
            Destroy(gameObject);
    }
}
