using AnswerNow.Domain.Enums;

namespace AnswerNow.Utilities.Extensions
{
    public static class FlagReasonExtensions
    {
        public static string ToDisplayString(this FlagReason reason )
        {
            return reason switch
            {
                FlagReason.Spam => "Spam or advertising",
                FlagReason.Harassment => "Harassment or bullying",
                FlagReason.HateSpeech => "Hate speech",
                FlagReason.Profanity => "Profanity or bad language",
                FlagReason.Misinformation => "Misinformation",
                FlagReason.OffTopic => "Off-topic",
                FlagReason.LowQuality => "Low-quality content",
                FlagReason.CopyrightViolation => "Copyright violation",
                FlagReason.PersonalInformation => "Personal information",
                FlagReason.Other => "Other",
                _ => "Unknown"
            };
        }
    }
}
