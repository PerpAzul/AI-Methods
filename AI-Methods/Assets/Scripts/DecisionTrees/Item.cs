using UnityEngine;

namespace DecisionTrees
{
    public class Item
    {
        public readonly RenderTexture Texture;
        public readonly bool IsMetal;
        public readonly bool IsDangerous;
        public readonly bool HasBlueEnergy;

        public Item(bool isMetal, bool isDangerous, bool hasBlueEnergy, RenderTexture texture)
        {
            IsMetal = isMetal;
            IsDangerous = isDangerous;
            HasBlueEnergy = hasBlueEnergy;
            Texture = texture;
        }

        public override string ToString()
        {
            return $"Metal: {IsMetal} Danger: {IsDangerous}  HasBlueEnergy: {HasBlueEnergy} Useful: {Useful()}";
        }

        public bool Useful()
        {
            return HasBlueEnergy || (IsDangerous && IsMetal);
        }
    }
}