using System;
using System.Collections.Generic;
using UnityEngine;

namespace Card
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Card")]
    public class CardSO : ScriptableObject
    {
        public enum CardType { Default, Rare, Epic };

        public string title;
        public Sprite icon;
        public Perks[] perks;
        public int weight;
        public CardType type;

        public Color GetOutlineColor()
        {
            Color color = Color.white;
            switch (type)
            {
                case CardType.Default:
                    color = Color.white;
                    break;
                case CardType.Rare:
                    color = Color.white;
                    break;
                case CardType.Epic:
                    color = Color.white;
                    break;
            }

            return color;
        }
    }
}