using System;
using System.Collections.Generic;
using UnityEngine;

namespace Card
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Card")]
    public class CardSO : ScriptableObject
    {
        public enum CardType { Common, Rare, Epic, Legendary };

        public string title;
        public Sprite icon;
        public Perks[] perks;
        public int weight;
        public CardType type;
    //new Color(61, 151, 255)
    //new Color(216, 93, 255)
    //new Color(255, 255, 50)
        public Color GetOutlineColor()
        {
            Color color = type switch
            {
                CardType.Common => Color.white,
                CardType.Rare => Color.blue,
                CardType.Epic => Color.magenta,
                CardType.Legendary => Color.yellow,
                _ => Color.white
            };

            return color;
        }
    }
}