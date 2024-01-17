using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using Kogane;
using System.Linq;

public static class JsonDataManager
{
    /// �p�X���擾 & �Z�[�u�t�@�C�����L�^
    private static string getFilePath() { return Application.persistentDataPath + "/saveData"; }

    public static void Save<T>(T saveData, string key, bool pretty = false)
    {
        string jsonSerializedData = JsonUtility.ToJson(saveData, pretty);
        save(jsonSerializedData, key);

    }

    public static void SaveArray<T>(T[] array, string key)
    {
        string jsonSerializedData = JsonHelper.ToJson(array);
        save(jsonSerializedData, key);
    }

    public static void SaveDict<TKey, TValue>(Dictionary<TKey, TValue> dictionary, string key)
    {
        var jsonDictionary = new JsonDictionary<TKey, TValue>(dictionary);
        var jsonSerializedData = JsonUtility.ToJson(jsonDictionary, true);
        save(jsonSerializedData, key);
    }

    private static void save(string jsonSerializedData, string key)
    {
        using (var sw = new StreamWriter(getFilePath() + "_" + key + ".json", false))
        {
            try
            {
                sw.Write(jsonSerializedData);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }

    public static T Load<T>(string key)
    {
        T jsonDeserializedData;

        try
        {
            using (FileStream fs = new FileStream(getFilePath() + "_" + key + ".json", FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            {
                string result = sr.ReadToEnd();
                return jsonDeserializedData = JsonUtility.FromJson<T>(result);
            }
        }
        catch (Exception e)
        {
            Debug.Log("�f�[�^�����݂��܂���");
            Debug.LogWarning(e);
            return default(T);
        }
    }

    public static T[] LoadArray<T>(string key)
    {
        T[] jsonDeserializedData;

        try
        {
            using (FileStream fs = new FileStream(getFilePath() + "_" + key + ".json", FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            {
                string result = sr.ReadToEnd();
                return jsonDeserializedData = JsonHelper.FromJson<T>(result);
            }
        }
        catch (Exception e)
        {
            // Debug.Log("�Z�[�u�f�[�^�����݂��܂���");
            Debug.LogWarning(e);
            return default(T[]);
        }
    }

    public static Dictionary<TKey, TValue> LoadDict<TKey, TValue>(string key)
    {
        try
        {
            using (FileStream fs = new FileStream(getFilePath() + "_" + key + ".json", FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            {
                string result = sr.ReadToEnd();
                var jsonDictionary = JsonUtility.FromJson<JsonDictionary<TKey, TValue>>(result);
                var dictionary = jsonDictionary.Dictionary;
                return dictionary;
            }
        }
        catch (Exception e)
        {
            // Debug.Log("�Z�[�u�f�[�^�����݂��܂���");
            Debug.LogWarning(e);
            return null;
        }
    }

}

/// <summary>
/// <see cref="JsonUtility"/> �ɕs�����Ă���@�\��񋟂��܂��B
/// </summary>
public static class JsonHelper
{
    /// <summary>
    /// �w�肵�� string �� Root �I�u�W�F�N�g�������Ȃ� JSON �z��Ɖ��肵�ăf�V���A���C�Y���܂��B
    /// </summary>
    public static T[] FromJson<T>(string json)
    {
        // ���[�g�v�f������Εϊ��ł���̂�
        // ���͂��ꂽJSON�ɑ΂���(��)�̍s��ǉ�����
        //
        // e.g.
        // �� {
        // ��     "array":
        //        [
        //            ...
        //        ]
        // �� }
        //
        string dummy_json = $"{{\"{DummyNode<T>.ROOT_NAME}\": {json}}}";

        // �_�~�[�̃��[�g�Ƀf�V���A���C�Y���Ă��璆�g�̔z���Ԃ�
        var obj = JsonUtility.FromJson<DummyNode<T>>(dummy_json);
        return obj.array;
    }

    /// <summary>
    /// �w�肵���z��⃊�X�g�Ȃǂ̃R���N�V������ Root �I�u�W�F�N�g�������Ȃ� JSON �z��ɕϊ����܂��B
    /// </summary>
    /// <remarks>
    /// 'prettyPrint' �ɂ͔�Ή��B���`������������ʓr�ϊ����āB
    /// </remarks>
    public static string ToJson<T>(IEnumerable<T> collection)
    {
        string json = JsonUtility.ToJson(new DummyNode<T>(collection)); // �_�~�[���[�g���ƃV���A��������
        int start = DummyNode<T>.ROOT_NAME.Length + 4;
        int len = json.Length - start - 1;
        return json.Substring(start, len); // �ǉ����[�g�̕�������菜���ĕԂ�
    }

    // �����Ŏg�p����_�~�[�̃��[�g�v�f
    [Serializable]
    private struct DummyNode<T>
    {
        // �⑫:
        // �������Ɉꎞ�g�p�������J�N���X�̂��ߑ����݌v���ςł��C�ɂ��Ȃ�

        // JSON�ɕt�^����_�~�[���[�g�̖���
        public const string ROOT_NAME = nameof(array);
        // �^���I�Ȏq�v�f
        public T[] array;
        // �R���N�V�����v�f���w�肵�ăI�u�W�F�N�g���쐬����
        public DummyNode(IEnumerable<T> collection) => this.array = collection.ToArray();
    }
}