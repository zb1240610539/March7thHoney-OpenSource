using March7thHoney.Database.Inventory;
using March7thHoney.Kcp;
using March7thHoney.Proto;

namespace March7thHoney.GameServer.Server.Packet.Send.Item;

public class PacketRechargeSuccNotify : BasePacket
{
    public PacketRechargeSuccNotify(string productId, string priceTier, ulong monthCardOutDateTime, ItemData item)
        : base(CmdIds.RechargeSuccNotify)
    {
        var proto = new RechargeSuccNotify
        {
            ProductId = productId,
            PriceTier = priceTier,
            MonthCardOutDateTime = monthCardOutDateTime,
            ItemList = new ItemList
            {
                ItemList_ = { item.ToProto() }
            }
        };

        SetData(proto);
    }
}
