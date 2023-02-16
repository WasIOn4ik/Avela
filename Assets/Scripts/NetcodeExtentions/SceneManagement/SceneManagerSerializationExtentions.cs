using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
public static class SceneManagerSerializationExtentions
{
    public static void ReadValueSafe(this FastBufferReader reader, out string[] stringArr)
    {
        reader.ReadValueSafe(out string readed);
        stringArr = String.IsNullOrEmpty(readed) ? new string[0] : readed.Split('\n');
    }

    public static void WriteValueSafe(this FastBufferWriter writer, in string[] stringArr)
    {
        string res = "";
        for (int i = 0; i < stringArr.Length;)
        {
            res += stringArr[i];
            i++;
            if (i != stringArr.Length)
            {
                res += '\n';
            }
        }
        writer.WriteValueSafe(res);
    }

    public static void SerializeValue<TReaderWriter>(this BufferSerializer<TReaderWriter> serializer, ref string[] stringArr) where TReaderWriter : IReaderWriter
    {
        if (serializer.IsReader)
        {
            int count = 0;

            serializer.SerializeValue(ref count);

            string[] vs = new string[count];

            string tempStr = "";
            for (int i = 0; i < count; i++)
            {
                serializer.SerializeValue(ref tempStr);
                vs[i] = tempStr;
            }
            stringArr = vs;
        }
        else
        {
            int count = stringArr.Length;
            serializer.SerializeValue(ref count);

            string tempStr = "";

            foreach (var s in stringArr)
            {
                tempStr = s;
                serializer.SerializeValue(ref tempStr);
            }
        }
    }
}
