using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class VerificationPropertyStatusFilter : SinglePropertyFilter
{
    public VerificationPropertyStatusFilter(
        string propertyName,
        Condition verificationStatus)
    {
        Property = propertyName;
        VerificationStatus = verificationStatus;
    }

    /// <summary>
    /// Gets or sets the verification status condition.
    /// </summary>
    [JsonPropertyName("verification_status")]
    public Condition VerificationStatus { get; set; }

    public class Condition
    {
        public Condition(VerificationStatus status)
        {
            Status = status;
        }

        /// <summary>
        /// Gets or sets the verification status.
        /// </summary>
        [JsonPropertyName("status")]
        public VerificationStatus Status { get; set; }
    }
}
