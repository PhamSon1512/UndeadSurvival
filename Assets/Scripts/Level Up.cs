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

            // Check if any selected item is the Heal item (Item 4) and it's already been used 5 times
            bool rerollNeeded = false;
            for (int i = 0; i < rand.Length; i++)
            {
                if (rand[i] == 4 && Item.healItemUsageCount >= 5)
                {
                    rerollNeeded = true;
                    break;
                }
            }

            // Reroll if needed or if we have duplicates
            if (rerollNeeded)
                continue;

            if (rand[0] != rand[1] && rand[1] != rand[2] && rand[0] != rand[2])
                break;
        }

        for (int i = 0; i < rand.Length; i++)
        {
            Item randItem = items[rand[i]];

            // Skip heal item if it's already been used 5 times
            if (rand[i] == 4 && Item.healItemUsageCount >= 5)
                continue;

            // 3, Thay thế vật phẩm cấp max = vật phẩm khác
            if (randItem.level == randItem.data.damages.Length)
            {
                // Don't activate item 4 if it's already been used 5 times
                if (items[4].data.itemType != ItemData.ItemType.Heal || Item.healItemUsageCount < 5)
                {
                    items[4].gameObject.SetActive(true);
                }
            }
            else
            {
                randItem.gameObject.SetActive(true);
            }
        }
    }
}
