using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlatBuffers;

public class FlatBufferTest : MonoBehaviour
{

    private void CreateLoginMsg()
    {
        string accountID = "TestAccountID";
        string deviceUUID = "Test Device UUID";
        string token = "Test Token";


        FlatBufferBuilder builder = new FlatBufferBuilder(1);
        StringOffset accountValue = builder.CreateString(accountID);
        StringOffset deviceUUIDValue = builder.CreateString(deviceUUID);
        StringOffset tokenValue = builder.CreateString(token);
        var offset = CSLogin.CreateCSLogin(builder, accountValue, deviceUUIDValue, tokenValue);
        builder.Finish(offset.Value);
        byte[] data = builder.SizedByteArray();

        // Send data to network
    }
}
