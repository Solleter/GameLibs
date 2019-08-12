using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using FlatBuffers;

public class ReadFlatBufferData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //string filePath = Application.dataPath + "/Config/fbmonster.bytes";
        //byte[] buffer = File.ReadAllBytes(filePath);

        //ByteBuffer bb = new ByteBuffer(buffer);
        //FBMonster fbMonster = FBMonster.GetRootAsFBMonster(bb);
        //string name = fbMonster.Name;
        //int hp = fbMonster.Hp;
        //int attack = fbMonster.Attack;

        //Debug.LogFormat("Name: {0}, HP: {1}, Attack: {2}", name, hp, attack);

        //string filePath = Application.dataPath + "/Config/music.bytes";
        //byte[] buffer = File.ReadAllBytes(filePath);

        //ByteBuffer bb = new ByteBuffer(buffer);
        //TShowMusicsConfig config = TShowMusicsConfig.GetRootAsTShowMusicsConfig(bb);
        //for (int i = 0; i < config.DatalistLength; ++i)
        //{
        //    TShowMusicsConfigRowData data = config.Datalist(i).Value;
        //    Debug.LogFormat("ID: {0}  Name: {1}  BGM: {2}  Time: {3}", data.ID, data.NoteName, data.BGMName, data.TotalTime);
        //}
        int num = DanceMusicConfigSets.Inst.GetNum();
        Debug.Log(num);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
