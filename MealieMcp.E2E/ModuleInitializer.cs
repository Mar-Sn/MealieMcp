using System.Runtime.CompilerServices;
using VerifyTests;
using VerifyXunit;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        // Initialize Verify check if needed, but Verifier.Initialize() is deprecated/removed.
        
        // Customize verification if needed
        VerifierSettings.IgnoreMember("id"); // Ignore dynamic IDs in JSON-RPC
    }
}
