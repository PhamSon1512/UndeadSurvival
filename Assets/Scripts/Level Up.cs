using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }
    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
    }
    public void Select(int index)
    {
        items[index].OnClick();
    }

    void Next()
    {
        // 1, Vô hiệu hóa tất cả
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }
        // 2, Kích hoạt 3 vật phẩm ngẫu nhiên
        int[] rand = new int[3];
        while (true)
        {
            rand[0] = Random.Range(0, items.Length);
            rand[1] = Random.Range(0, items.Length);
            rand[2] = Random.Range(0, items.Length);

            if (rand[0] != rand[1] && rand[1] != rand[2] && rand[0] != rand[2]) break;
        }
        for(int i=0; i<rand.Length; i++)
        {
            Item randItem = items[rand[i]];

            // 3, Thay thế vật phẩm cấp max = vật phẩm khác
            if(randItem.level == randItem.data.damages.Length)
            {
                items[4].gameObject.SetActive(true);
            }
            else
            {
                randItem.gameObject.SetActive(true);
            }
            
        }
    }
}
