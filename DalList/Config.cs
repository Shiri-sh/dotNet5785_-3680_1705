namespace Dal
{
    internal static class Config
    {
        //count call id auto
        internal const int startCallId = 1;
        private static int nextCallId = startCallId;
        internal static int NextCallId { get=> nextCallId++; }

        ///count auto
        internal const int startAssignmentId = 1;
        private static int nextAssignmentId = startAssignmentId;
        internal static int NextAssignmentId { get => nextAssignmentId++; }

        internal static void Reset()
        {
            nextCallId = 0; 
            nextAssignmentId = 0;
        }
    }
}
