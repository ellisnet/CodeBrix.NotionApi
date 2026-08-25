================================================================================
EXTRAS-README: CodeBrix.NotionApi
Samples, tools and other content in this repository that is not part of a NuGet
package
================================================================================

This repository contains NO sample applications, demo apps or build tools. It
is a single library project plus its test project:

    src/CodeBrix.NotionApi/          the library that becomes the
                                     CodeBrix.NotionApi.MitLicenseForever
                                     NuGet package (documented for consumers in
                                     AGENT-README.txt)
    tests/CodeBrix.NotionApi.Tests/  the only non-package content

TESTS AS WORKED EXAMPLES
========================
Path:  tests/CodeBrix.NotionApi.Tests
Run:   dotnet test CodeBrix.NotionApi.slnx

The test project is the repository's de facto example set, and consumers are
pointed at it from AGENT-README.txt ("WORKING EXAMPLES ON GITHUB"). It is not
packaged and is not published.

Two groups live inside it:

  * Unit tests (the project root and Models/, Authoring/) run offline. They
    drive the clients through two in-repo fakes rather than a network: the
    FakeServer harness (a WireMock.Net-shaped stub: Given / Request.Create() /
    Response.Create() / scenario states / LogEntries) and RecordingRestClient
    (an IRestClient fake that records calls and returns canned objects).
    Notable files: PagesClientTests.cs, BlocksClientTests.cs,
    DataSourcesClientTests.cs, FilterTests.cs, PropertyTests.cs,
    SearchClientTests.cs, UserClientTests.cs, FileUploadsClientTests.cs,
    AuthenticationClientTests.cs, RetryPolicyTests.cs,
    NotionClientFactoryTests.cs, DateCustomConverterTests.cs, and the two
    UpdatePropertyConfigurationRequest serialization test files.

  * Integration tests (tests/CodeBrix.NotionApi.Tests/Integration) hit the REAL
    Notion API and are OPT-IN. Every class in that folder skips unless the
    NOTION_AUTH_TOKEN, NOTION_PARENT_PAGE_ID and NOTION_PARENT_DATABASE_ID
    environment variables are set. They CREATE REAL CONTENT in the target
    workspace, so point them at a scratch page you do not mind polluting. See
    MAINTAINER-README.txt for the details.

OPTIONAL TEST DATA
==================
Path:  tests/CodeBrix.NotionApi.Tests/data
JSON response fixtures used by the offline unit tests, grouped by endpoint
(blocks, databases, pages, search, users). They are copied to the test output
directory by the test csproj. These fixtures came from the upstream
notion-sdk-net test suite; see THIRD-PARTY-NOTICES.txt.

Path:  tests/CodeBrix.NotionApi.Tests/Integration/assets
notion-logo.png — an image the upstream test suite bundles as file-upload test
data, used by the opt-in integration tests. The Notion logo itself is a brand
asset of Notion Labs, Inc.; it is present only as test input and is not
redistributed in any package.

STALE BUILD OUTPUT
==================
A working copy that predates the removal of the in-repo polymorphism library
may still hold src/CodeBrix.JsonPolymorphism/ and
tests/CodeBrix.JsonPolymorphism.Tests/ folders containing only bin/ and obj/.
Nothing there is tracked or built; that functionality now comes from the
CodeBrix.Json.Extensions.MitLicenseForever NuGet package. The folders can be
deleted.
