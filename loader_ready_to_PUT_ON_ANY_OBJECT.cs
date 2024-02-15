using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loader_ready_to_PUT_ON_ANY_OBJECT : MonoBehaviour
{
    public string Bank0 = "common";
    public string Bank1 = "common.strings";
    public string Bank2 = "GT3_car_8";

    string[] Banks = new string[3];
    public string GUID_To_Load = "9ace6d49-2a52-44e2-b6f6-f923ec43bb09";
    [Range(0,1)]
    public float throttle = 0;
    [Range(0, 9000)]
    public float rpm = 2000;
    public FMODUnity.StudioEventEmitter emitter;

    private GameObject engineEmitterObject;
    private void Start()
    {
        Banks[0] = Bank0; Banks[1] = Bank1; Banks[2] = Bank2;

        RuntimeUtils.EnforceLibraryOrder();
        foreach (var bankRef in Banks)
        {
            try
            { RuntimeManager.LoadBank(bankRef, true); }
            catch (BankLoadException e)
            { RuntimeUtils.DebugLogException(e); }
        }
        RuntimeManager.WaitForAllSampleLoading();
        
    }
    void OnEnable()
    {
        FMOD.GUID guid = FMOD.GUID.Parse(GUID_To_Load);

        emitter = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
        emitter.EventReference.Guid = guid;
        emitter.PlayEvent = FMODUnity.EmitterGameEvent.ObjectStart;
        emitter.StopEvent = FMODUnity.EmitterGameEvent.ObjectDestroy;
    }

    void Update()
    {
        if (emitter == null)
            return;

        emitter.SetParameter("throttle", throttle);
        emitter.SetParameter("rpms", rpm);
    }
}