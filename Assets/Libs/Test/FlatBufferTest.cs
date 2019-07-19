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

    /// <summary>
    /// 假设这里收到了服务器的数据
    /// </summary>
    /// <param name="data"></param>
    private void ProcessLoginMsg(byte[] data)
    {
        ByteBuffer bb = new ByteBuffer(data);
        SCLogin resp = SCLogin.GetRootAsSCLogin(bb);
        if(resp.ErrorCode == CommonErrorCode.Success)
        {
            string uid = resp.UID;
        }
        else
        {
            Debug.LogErrorFormat("Login Error: {0}", resp.ErrorCode);
        }
    }
}
