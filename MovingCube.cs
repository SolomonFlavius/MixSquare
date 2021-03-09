using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingCube : MonoBehaviour
{
	private float hsvColor;
    private AudioSource audio;
    public static MovingCube CurrentCube { get; private set; }
    public static MovingCube LastCube { get; private set; }
    public MoveDirection MoveDirection { get; set; }
    [SerializeField]
    public float moveSpeed = 10f;
    private Renderer renderer;
    private void OnEnable() 
    {
    	renderer = GetComponent<Renderer>();
        if(LastCube == null)
            {
            	LastCube = GameObject.Find("Start").GetComponent<MovingCube>();
            	hsvColor=Random.Range(0.0f, 1.0f);
        	}
        CurrentCube = this;
        hsvColor=GetHSVColor(LastCube.hsvColor);

        renderer.material.color = Color.HSVToRGB(hsvColor,1f,1f);

        transform.localScale = new Vector3(LastCube.transform.localScale.x, transform.localScale.y, LastCube.transform.localScale.z);
    }
    //culaore
    private float GetHSVColor(float number)
    {
    	if(number>=1f)
    		number=0f;
    	else
    		number+=0.025f;
    	return number;
    }

    //efect glow
    IEnumerator Glow()
    {
    	for(float f = 1f; f>= - 0.05f; f-=0.05f)
    	{
    		renderer.material.color = Color.HSVToRGB(hsvColor,f,1f);
    		yield return new WaitForSeconds(0.01f);
    	}

    	for(float f = 0.05f; f<= 1f; f+=0.05f)
    	{
    		renderer.material.color = Color.HSVToRGB(hsvColor,f,1f);
    		yield return new WaitForSeconds(0.01f);
    	}
    	
    }
    //start efect glow
    private void StartGlowing()
    {
    	StartCoroutine("Glow");
    }

  /*  private void ColorChanger()
    {
        renderer.material.color = Color.Lerp(Color.green, Color.red, t);
        if (t < 1)
        	{ 
                t += Time.deltaTime/duration;
            }
             
    }*/

    public static void ResetCubes()
    {
        
        LastCube = null;
        CurrentCube = null;
    }
    //play sound
    private void PlaySound()
    {
        if (audio)
            audio.Play();
    }
    //false = lose  true = continua
    internal bool Stop()
    {

        moveSpeed = 0;
        float hangover = GetHangover();
        float max = MoveDirection == MoveDirection.Z ? LastCube.transform.localScale.z : LastCube.transform.localScale.x;
        if(Mathf.Abs(hangover) >= max)
        {
           // Lose();
           Destroy(this, 1f);
           transform.position = new Vector3(0,-100,0);
           // LastCube = this;
            return false;
        }
        else
        {
        	PlaySound();
        

        	float direction = hangover > 0 ? 1f : -1f;

        	if(hangover > 0.05f || hangover < -0.05f)
        	{
        		if(MoveDirection == MoveDirection.Z)
            		SplitCubeOnZ(hangover, direction);
        		if(MoveDirection == MoveDirection.X)
            		SplitCubeOnX(hangover, direction);
        	}
        	else //Perfect case
        	{
        		if(CurrentCube != GameObject.Find("Start").GetComponent<MovingCube>())
        			StartGlowing();
       			transform.position = new Vector3(LastCube.transform.position.x, transform.position.y, LastCube.transform.position.z);
    		}

   		}

        LastCube = this;
        return true;

    }
    private float GetHangover()
    {
        if(MoveDirection == MoveDirection.Z)
            return transform.position.z - LastCube.transform.position.z;
        else
            return transform.position.x - LastCube.transform.position.x;
    }

    private void SplitCubeOnX(float hangover, float direction)
    {
        float newXSize = LastCube.transform.localScale.x - Mathf.Abs(hangover);
        float fallingBlockSize = transform.localScale.x - newXSize;

        float newXPosition = LastCube.transform.position.x + (hangover / 2);
        transform.localScale = new Vector3(newXSize, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newXPosition,transform.position.y,  transform.position.z );

        float cubeEdge = transform.position.x + (newXSize / 2f * direction);
        float fallingBlockXPosition = cubeEdge + fallingBlockSize / 2f * direction;

        if(CurrentCube != GameObject.Find("Start").GetComponent<MovingCube>())
            SpawnDropCube(fallingBlockXPosition, fallingBlockSize);
    }

    private void SplitCubeOnZ(float hangover, float direction)
    {
        float newZSize = LastCube.transform.localScale.z - Mathf.Abs(hangover);
        float fallingBlockSize = transform.localScale.z - newZSize;

        float newZPosition = LastCube.transform.position.z + (hangover / 2);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZSize);
        transform.position = new Vector3(transform.position.x,  transform.position.y, newZPosition);

        float cubeEdge = transform.position.z + (newZSize / 2f * direction);
        float fallingBlockZPosition = cubeEdge + fallingBlockSize / 2f * direction;

        if(CurrentCube != GameObject.Find("Start").GetComponent<MovingCube>())
            SpawnDropCube(fallingBlockZPosition, fallingBlockSize);
    }

    private void SpawnDropCube(float fallingBlockPosition, float fallingBlockSize)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        if(MoveDirection == MoveDirection.Z)
        {
        cube.transform.position = new Vector3(transform.position.x, transform.position.y, fallingBlockPosition);
        cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSize);
        }
        else
        {
            cube.transform.position = new Vector3(fallingBlockPosition, transform.position.y, transform.position.z );
            cube.transform.localScale = new Vector3(fallingBlockSize , transform.localScale.y, transform.localScale.z );
        }

        cube.AddComponent<Rigidbody>();
        cube.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
        Destroy(cube.gameObject, 1f);
    }

    private void Start() 
    {
        audio = GetComponent<AudioSource>();
    }
    void Update()
    {
    	//Debug.Log(MoveDirection);
        if(MoveDirection == MoveDirection.Z)
        {
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
            if(transform.position.z < -1.45f || transform.position.z > 1.45f)
            	moveSpeed *= -1;
        }
        else
        {
            transform.position += transform.right * Time.deltaTime * moveSpeed;
            if(transform.position.x < -1.45f || transform.position.x > 1.45f)
            	moveSpeed *= -1;
        }
    }
}
