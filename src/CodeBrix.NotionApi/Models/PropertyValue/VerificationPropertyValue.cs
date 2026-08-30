using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
///     Verification property value.
/// </summary>
public class VerificationPropertyValue : PropertyValue
{
    public override PropertyValueType Type => PropertyValueType.Verification;

    /// <summary>
    ///     Object containing verification type-specific data.
    /// </summary>
    [JsonPropertyName("verification")]
    public Info Verification { get; set; }

    public class Info
    {
        /// <summary>
        ///     The state of verification.
        ///     Read values: <see cref="VerificationStatus.Verified"/>, <see cref="VerificationStatus.Expired"/>,
        ///     <see cref="VerificationStatus.None"/>.
        ///     Write values: <see cref="VerificationStatus.Verified"/>, <see cref="VerificationStatus.Unverified"/>.
        /// </summary>
        [JsonPropertyName("state")]
        public VerificationStatus State { get; set; }

        /// <summary>
        ///     Describes the user who verified this page.
        /// </summary>
        [JsonPropertyName("verified_by")]
        public User VerifiedBy { get; set; }

        /// <summary>
        ///     Date verification property values contain a date property value.
        /// </summary>
        [JsonPropertyName("date")]
        public Date Date { get; set; }
    }
}
