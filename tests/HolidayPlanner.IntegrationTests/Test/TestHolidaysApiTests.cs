using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using HolidayPlanner.Application.Test.Commands.CreateTestHoliday;
using HolidayPlanner.Domain.Test;
using Microsoft.AspNetCore.Mvc.Testing;

namespace HolidayPlanner.IntegrationTests.Test;

public sealed class TestHolidaysApiTests : IClassFixture<IntegrationTestFactory>
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    private readonly HttpClient _client;

    public TestHolidaysApiTests(IntegrationTestFactory factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),
        });
    }

    // ── GET /api/v1/test/holidays ────────────────────────────────────────────

    [Fact]
    public async Task GetAll_Always_Returns200WithJsonArray()
    {
        var response = await _client.GetAsync("/api/v1/test/holidays");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().StartWith("["); // confirms a JSON array
    }

    // ── POST /api/v1/test/holidays ────────────────────────────────────────────

    [Fact]
    public async Task Post_ValidRequest_Returns201WithLocationHeader()
    {
        var command = new CreateTestHolidayCommand(
            Name: "Integration Test Holiday",
            Destination: TestDestination.Japan,
            StartDate: new DateOnly(2026, 8, 1),
            EndDate: new DateOnly(2026, 8, 14));

        var response = await _client.PostAsJsonAsync("/api/v1/test/holidays", command);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location!.ToString().Should().Contain("/api/v1/test/holidays/");
    }

    [Fact]
    public async Task Post_InvalidRequest_Returns422()
    {
        var invalidCommand = new CreateTestHolidayCommand(
            Name: "",                          // invalid: empty name
            Destination: TestDestination.Mexico,
            StartDate: new DateOnly(2026, 8, 14),
            EndDate: new DateOnly(2026, 8, 1)); // invalid: end before start

        var response = await _client.PostAsJsonAsync("/api/v1/test/holidays", invalidCommand);

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    // ── GET /api/v1/test/holidays/{id} ──────────────────────────────────────────

    [Fact]
    public async Task GetById_WhenHolidayExists_Returns200WithHoliday()
    {
        var id = await CreateHolidayAsync("GetById Test", TestDestination.Iceland);

        var response = await _client.GetAsync($"/api/v1/test/holidays/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.Content.ReadFromJsonAsync<TestHolidayResponse>(JsonOptions);
        dto.Should().NotBeNull();
        dto!.Name.Should().Be("GetById Test");
        dto.Destination.Should().Be(TestDestination.Iceland);
    }

    [Fact]
    public async Task GetById_WhenHolidayDoesNotExist_Returns404()
    {
        var response = await _client.GetAsync($"/api/v1/test/holidays/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ── PUT /api/v1/test/holidays/{id} ──────────────────────────────────────────

    [Fact]
    public async Task Put_WhenHolidayExists_Returns204AndUpdatesData()
    {
        var id = await CreateHolidayAsync("Before Update", TestDestination.Mexico);
        var updateBody = new UpdateHolidayBody(
            Name: "After Update",
            Destination: TestDestination.Scotland,
            StartDate: new DateOnly(2027, 1, 5),
            EndDate: new DateOnly(2027, 1, 12));

        var putResponse = await _client.PutAsJsonAsync($"/api/v1/test/holidays/{id}", updateBody);

        putResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"/api/v1/test/holidays/{id}");
        var dto = await getResponse.Content.ReadFromJsonAsync<TestHolidayResponse>(JsonOptions);
        dto!.Name.Should().Be("After Update");
        dto.Destination.Should().Be(TestDestination.Scotland);
    }

    [Fact]
    public async Task Put_WhenHolidayDoesNotExist_Returns404()
    {
        var updateBody = new UpdateHolidayBody(
            Name: "Ghost Holiday",
            Destination: TestDestination.NewZealand,
            StartDate: new DateOnly(2027, 3, 1),
            EndDate: new DateOnly(2027, 3, 14));

        var response = await _client.PutAsJsonAsync(
            $"/api/v1/test/holidays/{Guid.NewGuid()}", updateBody);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ── DELETE /api/v1/test/holidays/{id} ────────────────────────────────────────────

    [Fact]
    public async Task Delete_WhenHolidayExists_Returns204AndHolidayIsGone()
    {
        var id = await CreateHolidayAsync("To Delete", TestDestination.Japan);

        var deleteResponse = await _client.DeleteAsync($"/api/v1/test/holidays/{id}");

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"/api/v1/test/holidays/{id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_WhenHolidayDoesNotExist_Returns404()
    {
        var response = await _client.DeleteAsync($"/api/v1/test/holidays/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ── Private helpers ─────────────────────────────────────────────────────

    private async Task<Guid> CreateHolidayAsync(string name, string destination)
    {
        var command = new CreateTestHolidayCommand(
            Name: name,
            Destination: destination,
            StartDate: new DateOnly(2026, 6, 1),
            EndDate: new DateOnly(2026, 6, 14));

        var response = await _client.PostAsJsonAsync("/api/v1/test/holidays", command);
        response.EnsureSuccessStatusCode();

        var location = response.Headers.Location!.ToString();
        var idString = location.Split('/').Last();
        return Guid.Parse(idString);
    }

    private sealed record TestHolidayResponse(
        Guid Id,
        string Name,
        string Destination,
        string StartDate,
        string EndDate,
        DateTimeOffset CreatedOn,
        DateTimeOffset ModifiedOn);

    private sealed record UpdateHolidayBody(
        string Name,
        string Destination,
        DateOnly StartDate,
        DateOnly EndDate);
}
