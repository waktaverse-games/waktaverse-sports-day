namespace GameHeaven.PunctureGame
{
    public enum ItemType
    {
        GoldCoin,
        SilverCoin,
        BronzeCoin
    }

    public class ItemPool : GenericPoolManager<Item>
    {
    }
}