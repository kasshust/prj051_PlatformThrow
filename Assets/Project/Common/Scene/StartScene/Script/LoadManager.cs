using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour
{

    public Button[] loadButtons;
    GameMainSystem gms;


    void Start()
    {

        gms = GameMainSystem.Instance;


        foreach (var button in loadButtons)
        {
            if (button == null) continue;
            Transform t = button.transform.Find("Text");

            if (t == null) {
                Debug.LogAssertion("This Button don't have Text Component!!!");
                continue;
            }

            Text text = t.GetComponent<Text>();
            text.text = "fuck";
        }
    }

    public void loadData(int slot) {
        if (gms == null) {
            Debug.LogWarning("GameMainSystemが生成されていません");
            return;
        }
        // gms.data.load(slot);
        changeScene();
    }

    public void  changeScene() {
        Debug.LogWarning("遷移先が未記入です");
        // シーン遷移処理
    }

}
