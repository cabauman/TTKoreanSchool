namespace TTKS.Core.Config
{
    public static class FirebaseStorageHelper
    {
        public static string AUDIOBOOK_IMAGES = "audiobook-images";
        public static string AUDIOBOOK_AUDIO = "audiobook-audio";

        public static string AudiobookImagePath(string filename)
        {
            return $"{AUDIOBOOK_IMAGES}/{filename}";
        }

        public static string AudiobookAudioPath(string filename)
        {
            return $"{AUDIOBOOK_AUDIO}/{filename}";
        }
    }
}
