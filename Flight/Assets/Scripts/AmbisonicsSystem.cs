
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System.Text;

/**  
 * CONTROL COMMANDS: 
 * enable 0/1
 * setpath filepath
 * setambient filename volume reverberation
 * setreverb roomsize shininess attenuation simplicity volume
 * speakers filename
 * view pos-x pos-y pos-z [quat-w quat-x quat-y quat-z]
 * shaker volume [decaytime] 
 * setsound id filename volume directionality 
 * move id pos-x pos-y pos-z [quat-w quat-x quat-y quat-z]
 * play id loop
 * stop id
 * stopall
 * 
 * TESTING COMMANDS: 
 * test channels
 * test pan
 * test sounds
 * test walking
 * test sub
 * test shakers
 * test none
 */

public class AmbisonicsSystem : MonoBehaviour
{

    private UdpClient client;
    private IPEndPoint endpoint;
    private bool systemOn;
    private bool correctlyConnected = true;

    private Dictionary<string, int> nameToID;
    private Dictionary<int, string> IDToFile;
    private Dictionary<int, bool> IDToStatus;
    private int nextOpenID;

    private bool initialized;

    /***** Public Functions *****/

    public bool SystemTurnedOn() { return systemOn; }

    public bool NameInSystem(string name) { return nameToID.ContainsKey(name); }

    public bool GetFile(string name, out string file)
    {
        int id;
        if (!nameToID.TryGetValue(name, out id))
        {
            file = "";
            return false;
        }
        if (!IDToFile.TryGetValue(id, out file)) return false;
        return true;
    }

    public bool GetStatus(string name, out bool status)
    {
        int id;
        if (!nameToID.TryGetValue(name, out id))
        {
            status = false;
            return false;
        }
        if (!IDToStatus.TryGetValue(id, out status)) return false;
        return true;
    }

    public bool Add(string name, string file, float volume)
    {
        if (NameInSystem(name)) return false;
        SetSound(nextOpenID, file, volume);
        nameToID.Add(name, nextOpenID);
        IDToFile.Add(nextOpenID, file);
        IDToStatus.Add(nextOpenID, false);
        nextOpenID += 1;
        return true;
    }

    public bool Remove(string name)
    {
        if (!NameInSystem(name)) return false;
        int id;
        nameToID.TryGetValue(name, out id);
        nameToID.Remove(name);
        IDToFile.Remove(id);
        IDToStatus.Remove(id);
        return true;
    }

    public bool StartSystem()
    {
        if (!initialized) initialize();
        if (!correctlyConnected)
        {
            print("No ambionic sound system");
            return false;
        }

        //if (systemOn) return;
        systemOn = true;

        SendCommand("enable 1");
        SendCommand("view 0 0 0 0 0 0 0");
        SendCommand("setpath /Users/worldviz/Desktop/sound-system/sounds/unity/");
        SendCommand("speakers /Users/worldviz/Desktop/sound-system/auralizer/speakers-link.txt");
        SendCommand("test none");
        SendCommand("stopall");
        return true;
    }

    public void StopSystem()
    {
        if (!systemOn) return;
        systemOn = false;

        SendCommand("test none");
        SendCommand("stopall");
        SendCommand("enable 0");
    }

    public bool StartSound(string name, bool loop)
    {
        if (!NameInSystem(name)) return false;
        int id = GetID(name);

        if (IDToStatus[id] == true) return true;
        if (loop) IDToStatus[id] = true;

        int loop_i;
        if (loop) loop_i = 1;
        loop_i = 0;

        string command = "play " + id.ToString() + " " + loop_i.ToString();
        SendCommand(command);
        return true;
    }

    public bool StopSound(string name)
    {
        if (!NameInSystem(name)) return false;
        int id = GetID(name);

        if (IDToStatus[id] == false) return true;
        IDToStatus[id] = false;

        string command = "stop " + id.ToString();
        SendCommand(command);
        return true;
    }

    public bool MoveSound(string name, Vector3 newLocation)
    {
        if (!NameInSystem(name)) return false;
        int id = GetID(name);

        string command = "move " + id.ToString() + " " + newLocation.x.ToString() + " " + newLocation.y.ToString() + " " + newLocation.z.ToString();
        SendCommand(command);
        return true;
    }

    public bool MoveSoundRot(string name, Vector3 newLocation, Quaternion rotation)
    {
        if (!NameInSystem(name)) return false;
        int id = GetID(name);

        string command = "move " + id.ToString() + " " + newLocation.x.ToString() + " " + newLocation.y.ToString() + " " + newLocation.z.ToString() + " " +
            rotation.w.ToString() + " " + rotation.x.ToString() + " " + rotation.y.ToString() + " " + rotation.z.ToString();
        SendCommand(command);
        return true;
    }

    public bool SetAmbient(string filename, float volume, float reverb)
    {
        string command = "setambient " + filename + " " + volume + " " + reverb;
        SendCommand(command);
        return true;
    }

    /*****  *****/

    void Start()
    {
        if (!initialized) initialize();
    }

    void OnApplicationQuit()
    {
        StopSystem();
        client.Close();
    }

    /***** Private Helpers*****/

    private void initialize()
    {
        client = new UdpClient();
        IPAddress ipAddress = IPAddress.Parse("171.64.33.148");
        try
        {
            
            client.Connect(ipAddress, 7400);
            correctlyConnected = true;
            print("Successfully connected");
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            correctlyConnected = false;
        }

        nameToID = new Dictionary<string, int>();
        IDToFile = new Dictionary<int, string>();
        IDToStatus = new Dictionary<int, bool>();
        nextOpenID = 0;

        systemOn = false;
        initialized = true;
        Debug.Log("Initialized ambisonics system");
    }

    private void SendCommand(string command)
    {
        Debug.Log("Command: " + command);
        Byte[] sendBytes = Encoding.ASCII.GetBytes(command);
        try
        {
            client.Send(sendBytes, sendBytes.Length);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private int GetID(string name)
    {
        int id;
        bool ok = nameToID.TryGetValue(name, out id);
        if (!ok) throw new System.ArgumentException("Name isn't in the system!");
        return id;
    }

    private void SetSound(int id, string file, float volume)
    {
        string command = "setsound " + id + " " + file + " " + volume + " 0";
        SendCommand(command);
    }

}