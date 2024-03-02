using Google.Protobuf;

namespace OTelDemo.OtplDebugger
{
    public static class FormattingExtensions
    {
        public static string ToHexString(this ByteString bytes)
            => bytes.ToByteArray().ToHexString();

        public static string ToHexString(this byte[] bytes)
            => BitConverter.ToString(bytes).Replace("-", "");
    }
}
