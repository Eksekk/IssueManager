namespace IssueManager.Static_classes
{
    public class Constants
    {
        public enum BootstrapMsgType
        {
            Success,
            Info,
            Warning,
            Danger
        }

        public static string GetBootstrapAlertClass(BootstrapMsgType messageType)
        {
            return messageType switch
            {
                BootstrapMsgType.Success => "alert-success",
                BootstrapMsgType.Info => "alert-info",
                BootstrapMsgType.Warning => "alert-warning",
                BootstrapMsgType.Danger => "alert-danger",
                _ => "alert-info"
            };
        }
    }
}
