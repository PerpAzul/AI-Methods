using UnityEngine;

namespace DecisionTrees
{
    public class Item_Tut
    {
        public readonly RenderTexture Texture;
        public readonly bool IsRed;
        public readonly bool IsFruit;

        public Item_Tut(bool isRed, bool isFruit, RenderTexture texture)
        {
            IsRed = isRed;
            IsFruit = isFruit;
            Texture = texture;
        }

        public override string ToString()
        {
            return $"Red: {IsRed} Fruit: {IsFruit} Useful: {Useful()}";
        }

        public bool Useful()
        {
            return (IsFruit && !IsRed) || (!IsFruit && IsRed);
        }
    }
}