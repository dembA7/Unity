using TMPro;
using UnityEngine;

public class BulletCountUI : MonoBehaviour
{
    public TextMeshProUGUI bulletCountText;

    public void UpdateBulletCount(int bulletCount)
    {
        bulletCountText.text = $"{bulletCount.ToString("000")}";
    }
}
