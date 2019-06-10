using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.UI;

public class RideScript : MonoBehaviour
{
    public WheelCollider Front;
    public WheelCollider Back;
    public WheelCollider Front2;
    public WheelCollider Back2;
    public GameObject Handles;
    public GameObject FrontWheelMesh;
    public GameObject BackWheelMesh;
    public GameObject Head;
    public GameObject bike;

    public InputField port;
    private string PortName = "COM9"; //default port is COM9

    public Slider SteerP;
    public Slider SpeedP;
    
    public float Steerforce;
    public float Speedforce;
    private float Forward;
    private float Sideways;
    private float Lean;
    private float Headheight;
    private float Headfront;
    private float Headside;
    private bool Cali = true;

    private float x = 0;
    private float y = 0;
    private float z = 0;
    private float r = 0;
    public float precision = 0.01f;
    private float lean;
    public float leanforce;

    private SerialPort seri = new SerialPort("COM9", 9600);//define our port

     private void Awake()
     {
         seri.Open();//open our port
         StartCoroutine(ReadDataFromSerialPort());//start loop
     }

     IEnumerator ReadDataFromSerialPort()
     {
         while (true)//loop
         {

            string[] values = seri.ReadLine().Split(',');//we split our string value by , because we write string as carspeed,cartotation in our ardunio codes
            // Debug.Log(seri.ReadLine().Split(','));
            Forward = (float.Parse(values[0]));// * Powerforce;
            Sideways = (float.Parse(values[1]));// * Steerforce;

             yield return null;//waiting seconds to read data. It should be same as ardunio code loop delay
         }
     }
 
     public void Portchange() //Change users Arduino port or reset connection in-game 
    {
        Debug.Log("Port Entered Is " + port.text);
        PortName = port.text;
        r = 1;
    }

    public void ForceChange()
    {
        Debug.Log("Steerforce Entered Is " + SteerP.value);
        Debug.Log("Speedforce Entered Is " + SpeedP.value);
        Steerforce = SteerP.value;
        Speedforce = SpeedP.value;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Forward = Input.GetAxis("Vertical");
        //Sideways = Input.GetAxis("Horizontal");
        
        Front.steerAngle = Steerforce * Sideways;
        Back.motorTorque = Speedforce * Forward;
        Front2.steerAngle = Steerforce * Sideways;
        Back2.motorTorque = Speedforce * Forward;

        FrontWheelMesh.transform.Rotate(Speedforce/20 * Forward, 0, 0, Space.Self);
        BackWheelMesh.transform.Rotate(Speedforce/20 * Forward, 0, 0, Space.Self);


        Handles.transform.localEulerAngles = new Vector3(-15, Sideways*Steerforce, 0);
        //lean = Input.GetAxis("Horizontal");
        lean = (-Steerforce);
        bike.transform.Rotate(0,0,2, Space.Self); //lean*leanforce //bike.transform.localrotation = new Vector3(0, 0,0);
        

        if (r == 1) //if reset has been enabled 
        {
        seri.Close();
        seri = new SerialPort(PortName, 9600);//define our port
        seri.Open();//open our port
        StartCoroutine(ReadDataFromSerialPort());//start loop
            r = 0;
        }

        if (Input.GetKey(KeyCode.M))
        {
            Debug.Log("portname = " + PortName);
        }


        if (Input.GetKey(KeyCode.F))
            {
            x = x + 1f * precision;
            Head.transform.localPosition = new Vector3(x, y, z);
        }
        if (Input.GetKey(KeyCode.H))
        {
            x = x - 1f * precision;
            Head.transform.localPosition = new Vector3(x, y, z);
        }
        
        if (Input.GetKey(KeyCode.C))
        {
            r = r + 1f * precision;
            Head.transform.localRotation = Quaternion.Euler(0, r, 0);
        }
        if (Input.GetKey(KeyCode.B))
        {
            r = r - 1f * precision;
            Head.transform.localRotation = Quaternion.Euler(0, r, 0);
        }

    }

    private void OnApplicationQuit()
    {
        seri.Close();
    }
}



