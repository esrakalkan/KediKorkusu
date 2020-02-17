using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif




public class CharacterScript : MonoBehaviour
{
    
    public Animator anim;
    private float inputV;
   // private float counter;
    AudioSource audioSource;
    int readHead = 0;
    float[] myListener;
    private GameObject dialog;

    // Start is called before the first frame update
    void Start()
    {

        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
            dialog = new GameObject();
        }
        
        audioSource = GetComponent<AudioSource>();
        
        Debug.Log(Microphone.devices[0]);
        audioSource.clip = Microphone.Start(
            deviceName: Microphone.devices[0],
            loop: true,
            lengthSec: 1,
            frequency: 44100
        );
        //counter = 0;
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float[] myData = new float[10];
        audioSource.clip.GetData(myData, 20);
        if (myData[0]>=0.0015f)
        {
            Debug.Log(myData[0]);
            transform.Translate(0, 0, Time.deltaTime * -10f);
        }
       // Debug.Log("olmadi ");
        //if(myData[0] > 1.2f)
        //counter = counter + Time.deltaTime;

        inputV = Input.GetAxis("Vertical");

        anim.SetFloat("inputV", inputV);

        transform.Translate(0, 0, inputV * Time.deltaTime * 5f);

        
        
    }
    void FlushToListeners()
    {
        int writeHead = Microphone.GetPosition(Microphone.devices[0]);

        
        int nFloatsToGet = (audioSource.clip.samples + writeHead - readHead) % audioSource.clip.samples;

        float[] B = new float[nFloatsToGet];

       
        audioSource.clip.GetData(B, readHead);
        myListener = B;
        

        readHead = (readHead + nFloatsToGet) % audioSource.clip.samples;
    }
    void OnGUI()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
          
            dialog.AddComponent<PermissionsRationaleDialog>();
            return;
        }
        else if (dialog != null)
        {
            Destroy(dialog);

        }
#endif
    }
    public class PermissionsRationaleDialog : MonoBehaviour
    {
        const int kDialogWidth = 300;
        const int kDialogHeight = 100;
        private bool windowOpen = true;

        void DoMyWindow(int windowID)
        {
            GUI.Label(new Rect(10, 20, kDialogWidth - 20, kDialogHeight - 50), "Please let me use the microphone.");
            GUI.Button(new Rect(10, kDialogHeight - 30, 100, 20), "No");
            if (GUI.Button(new Rect(kDialogWidth - 110, kDialogHeight - 30, 100, 20), "Yes"))
            {
             #if PLATFORM_ANDROID
                Permission.RequestUserPermission(Permission.Microphone);
             #endif
                windowOpen = false;
            }
        }

        

        void OnGUI()
        {
            if (windowOpen)
            {
                Rect rect = new Rect((Screen.width / 2) - (kDialogWidth / 2), (Screen.height / 2) - (kDialogHeight / 2), kDialogWidth, kDialogHeight);
                GUI.ModalWindow(0, rect, DoMyWindow, "Permissions Request Dialog");
            }
        }
    }
    

}