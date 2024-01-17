using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// アイテムバッグ
public class ItemBuggage<T>
{
    private List<T>     m_itemList;
    private int[]       m_possession;
    public  List<int>   buggage;

    public ItemBuggage(List<T> itemList)
    {
        m_itemList = itemList;
        m_possession = new int[itemList.Count];
        buggage = new List<int>();
    }

    public void Init() {
        if (m_possession != null) {
            for (int i = 0; i < m_possession.Length; i++)
            {
                m_possession[i] = 0;
            }
        }
        buggage.Clear();
    }

    public void Load(string key)
    {
        int[] l = JsonDataManager.LoadArray<int>(key);
        if (l == null) {

            // Debug.Log("セーブデータが存在しません");
            return; 
        }

        for (int i = 0; i < l.Length; i++)
        {
            if (m_possession.Length < i + 1)
            {
                Debug.LogWarning("このセーブデータは現在のアイテムリストとはバージョンが異なります。多い。");
                break;
            }
            m_possession[i] = l[i];
        }
        setBuggage();
    }

    private void sortBug() {
        buggage.Sort();
    }

    public void Save(string key)
    {
        JsonDataManager.SaveArray(m_possession, key);
    }

    public int this[int index]
    {
        get
        {
            return m_possession[index];
        }

        set
        {
            if (value < 0) Debug.LogWarning("Item Value is invalid");

            m_possession[index] = value;
            adjustBuggage(index);
        }
    }

    private void setBuggage()
    {
        buggage.Clear();
        for (int i = 0; i < m_possession.Length; i++)
        {
            adjustBuggage(i);
        }
    }

    private bool checkIndex(int index)
    {
        if (index > m_possession.Length - 1 || index < 0)
        {
            Debug.LogWarning("This index is Invalid"); return false;
        }
        return true;
    }

    private void _cal(int index, int num)
    {
        if (!checkIndex(index)) return;

        m_possession[index] += num;
        if (m_possession[index] < 0) m_possession[index] = 0;
    }

    // 変更があればtrueを返す
    private bool adjustBuggage(int index)
    {
        if (!checkIndex(index)) return false;

        if (m_possession[index] <= 0)
        {
            if (buggage.Contains(index))
            {
                buggage.Remove(index);

                sortBug();
                
                return true;
            }
        }
        else
        {
            if (!buggage.Contains(index))
            {
                buggage.Add(index);

                sortBug();

                return true;
            }
        }

 

        return false;
    }

    private bool set(int index, int num)
    {
        if (!checkIndex(index)) return false;
        m_possession[index] = num;
        return adjustBuggage(index);
    }

    public bool cal(int index, int num)
    {
        if (!checkIndex(index)) return false;
        _cal(index, num);
        return adjustBuggage(index);
    }

    public int getItemNum(int index)
    {
        if (!checkIndex(index)) return -1;
        return m_possession[index];
    }

    public bool add(int index)
    {
        if (!checkIndex(index)) return false;
        return cal(index, 1);
    }

    public bool sub(int index)
    {
        if (!checkIndex(index)) return false;
        return cal(index, -1);
    }

    // debug用
    public void setMaxItem(int num = 99)
    {
        for (int i = 0; i < m_possession.Length; i++)
        {
            set(i, num);
        }
    }

}


// アイテムショップ
public class ItemShop<ItemListParam>
{
    public List<ShopList.Param> m_shopItemList;

    public ItemShop(List<ShopList.Param> itemList)
    {
        m_shopItemList = itemList;
    }

    int buyItem(int index, int money, ItemBuggage<ItemListParam> bug)
    {
        if (m_shopItemList[index].num <= 0)
        {
            emptyItem();
            return -1;
        }

        if (money < m_shopItemList[index].buyPrice)
        {
            shortOfMoney();
            return -1;
        }

        money -= m_shopItemList[index].buyPrice;
        bug.add(index);

        // 減らす処理
        m_shopItemList[index].num -= 1;

        return money;
    }

    int sellItem(int index, int money, ItemBuggage<ItemListParam> bug)
    {
        if (bug.getItemNum(index) <= 0)
        {
            Debug.Log("このアイテム持ってないぞ");
        }

        money += m_shopItemList[index].sellPrice;
        bug.sub(index);
        return money;
    }

    void emptyItem()
    {
        Debug.Log("品切れ");
    }

    void shortOfMoney()
    {
        Debug.Log("金足りんぞ");
    }
}
