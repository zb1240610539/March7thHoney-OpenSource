using March7thHoney.Enums.Avatar;
using March7thHoney.GameServer.Server.Packet.Send.Player;
using March7thHoney.GameServer.Server.Packet.Send.PlayerSync;
using March7thHoney.Kcp;
using March7thHoney.Proto;

namespace March7thHoney.GameServer.Server.Packet.Recv.Player;

[Opcode(CmdIds.SetGenderCsReq)]
public class HandlerSetGenderCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var player = connection.Player!;
        var req = SetGenderCsReq.Parser.ParseFrom(data);
        if (req == null) return;

        if (req.Gender == Gender.None)
        {
            await connection.SendPacket(new PacketSetGenderScRsp(player.Data.CurBasicType));
            return;
        }

        player.Data.CurrentGender = req.Gender == Gender.Woman ? Gender.Woman : Gender.Man;

        await connection.SendPacket(new PacketSetGenderScRsp(player.Data.CurBasicType));
        await connection.SendPacket(new PacketPlayerSyncScNotify(player.ToProto()));
    }
}
