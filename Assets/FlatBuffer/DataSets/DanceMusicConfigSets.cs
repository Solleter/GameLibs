using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using FlatBuffers;

public class DanceMusicConfigSets : CommonSingleton<DanceMusicConfigSets>
{

    protected override void OnInit()
    {
        string path = Path.Combine(Application.dataPath, "Config/DanceMusicConfig.bytes");
        byte[] data = File.ReadAllBytes(path);
        ParseData(data);
    }

    private void ParseData(byte[] bArray)
    {
        ByteBuffer bb = new ByteBuffer(bArray);
        DanceMusicConfig config = DanceMusicConfig.GetRootAsDanceMusicConfig(bb);
        for(int i = 0; i < config.DatalistLength; ++i)
        {
            DanceMusicConfigRowData? data = config.Datalist(i).Value;
            Debug.LogFormat("ID: {0}  NoteName: {1}  BGMName: {2}  TotalTime: {3}  ToTalTime2: {4}",
                data.Value.ID,
                data.Value.NoteName,
                data.Value.BGMName,
                data.Value.TotalTime,
                data.Value.TotalTime2);
        }
    }

    public int GetNum()
    {
        return 100;
    }
}
