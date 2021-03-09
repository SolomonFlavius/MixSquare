using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    public GameManager GameManager;
    public Image image;
	public float time = 10f;
    public float time2d;
	private Text text;
    public bool ok = false;
    // Start is called before the first frame update
    void Awake()
    {
        time = 10f;
        image = GetComponent<Image> ();
    }

    // Update is called once per frame
    void Update()
    {
        if(ok)
        {
            if (time > 0f)
            {
            	time -= Time.deltaTime;
                time2d = Mathf.Round(time * 100f) / 100f;
                image.fillAmount = time2d/10;
            	//text.text = time2d.ToString();
                //Debug.Log(time2d/10);
            }
            else
            {
                time = 10f;
                GameManager.Reset();
            }
        }
    }
}
