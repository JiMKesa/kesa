using KSP.Game;
using UnityEngine;

namespace kesa.Utils;

public static class KMessages
{
    public static void KAlertLow(string titre,string mesg)
    {
        var notification = GameManager.Instance.Game.Notifications;
        var texico = SpaceWarp.API.Assets.AssetManager.GetAsset<Texture2D>($"kesa/images/icon.png");
        var sprite = Sprite.Create(texico, new Rect(0, 0, texico.width, texico.height), new Vector2(0.5f, 0.5f));
        var notificationData = new NotificationData()
        {
            Importance = NotificationImportance.Low,
            Tier = NotificationTier.Alert,
            AlertTitle = new NotificationLineItemData()
            {
                Icon = sprite,
                LocKey = titre
            },
            FirstLine = new NotificationLineItemData()
            {
                LocKey = mesg
            }
        };
        notification.ProcessNotification(notificationData);
    }
}
