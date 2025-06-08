using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardUtils
{
    public static void SetCardSize(RectTransform rt, float scale)
    {
        float widthCard = 178f;
        float heightCard = 254f;
        rt.sizeDelta = new Vector2(widthCard * scale, heightCard * scale);
    }
}
