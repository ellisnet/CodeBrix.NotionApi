using Xunit;

namespace CodeBrix.NotionApi.Tests.Integration;

/// <summary>
/// Every integration test class creates real content under the SAME parent page in the target
/// workspace. Run in parallel, those writes make Notion answer HTTP 409 "Conflict occurred while
/// saving" at random, which shows up as flaky failures unrelated to the code under test. Putting
/// them all in one non-parallel collection keeps the suite deterministic.
/// </summary>
[CollectionDefinition(Name, DisableParallelization = true)]
public class NotionIntegrationCollection
{
    public const string Name = "Notion API integration";
}
