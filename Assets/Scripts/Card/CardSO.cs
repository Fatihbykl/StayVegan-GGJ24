using UnityEngine;
using UnityEngine.UIElements;

namespace Card
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Card")]
    public class CardSO : ScriptableObject
    {
        public string title;
        public Sprite icon;
        public Perks[] perks;
        public int weight;
        public Color outlineColor;
    }
}