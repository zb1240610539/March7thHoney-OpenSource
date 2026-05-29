using March7thHoney.GameServer.Server;
using March7thHoney.GameServer.Server.Packet.Send.Item;
using March7thHoney.Util;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace March7thHoney.WebServer.Controllers;

[ApiController]
[Route("/")]
public class RechargeRoutes : ControllerBase
{
    [HttpPost("/recharge/success")]
    public async ValueTask<IActionResult> RechargeSuccess([FromBody] RechargeSuccessRequest req)
    {
        if (!IsAuthorizedServerExchangeRequest())
            return Unauthorized(new RechargeSuccessResponse(401, "Unauthorized"));

        if (req.Uid <= 0 || req.ItemId <= 0 || req.Num <= 0)
            return BadRequest(new RechargeSuccessResponse(400, "Invalid request"));

        var connection = Listener.GetActiveConnection(req.Uid);
        var player = connection?.Player;
        if (player?.InventoryManager == null || connection?.IsOnline != true)
            return NotFound(new RechargeSuccessResponse(404, "Player is not online"));

        var item = await player.InventoryManager.AddItem(req.ItemId, req.Num, notify: false);
        if (item == null)
            return BadRequest(new RechargeSuccessResponse(400, "Invalid item"));

        await player.SendPacket(new PacketRechargeSuccNotify(
            req.ProductId ?? "",
            req.PriceTier ?? "",
            req.MonthCardOutDateTime,
            item));

        return new JsonResult(new RechargeSuccessResponse(
            0,
            "OK",
            req.Uid,
            req.ProductId ?? "",
            req.PriceTier ?? "",
            req.ItemId,
            req.Num,
            req.MonthCardOutDateTime));
    }

    private bool IsAuthorizedServerExchangeRequest()
    {
        var expectedSecret = ConfigManager.Config.ServerOption.ServerConfig.ServerExchangeSecret;
        if (string.IsNullOrWhiteSpace(expectedSecret))
            return true;

        if (!Request.Headers.TryGetValue("X-March7thHoney-Server-Secret", out var providedValues))
            return false;

        var providedSecret = providedValues.ToString();
        if (string.IsNullOrWhiteSpace(providedSecret))
            return false;

        var expectedBytes = Encoding.UTF8.GetBytes(expectedSecret);
        var providedBytes = Encoding.UTF8.GetBytes(providedSecret);
        return expectedBytes.Length == providedBytes.Length &&
               CryptographicOperations.FixedTimeEquals(expectedBytes, providedBytes);
    }
}

public class RechargeSuccessRequest
{
    public int Uid { get; set; }
    public string? ProductId { get; set; }
    public string? PriceTier { get; set; }
    public int ItemId { get; set; } = 3;
    public int Num { get; set; }
    public ulong MonthCardOutDateTime { get; set; }
}

public record RechargeSuccessResponse(
    int Retcode,
    string Message,
    int Uid = 0,
    string ProductId = "",
    string PriceTier = "",
    int ItemId = 0,
    int Num = 0,
    ulong MonthCardOutDateTime = 0);
