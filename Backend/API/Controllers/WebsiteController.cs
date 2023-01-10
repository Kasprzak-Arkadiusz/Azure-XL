using Api.Controllers;
using Application.Websites;
using Application.Websites.Commands;
using Application.Websites.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class WebsiteController : BaseController
{
    /// <summary>
    /// Get website by search string
    /// </summary>
    /// <param name="searchString">String by which website is searched</param> 
    /// <returns>Website details (url and title)</returns>
    /// <response code="200">Successfully returned website details</response>
    /// <response code="404">Not found matching results</response>
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WebsiteSearchResult>> GetSearchResult(string searchString)
    {
        var result = await Mediator.Send(new SearchBySearchStringQuery(searchString));
        return Ok(result);
    }

    /// <summary>
    /// Add new websites
    /// </summary>
    /// <param name="websites">List of objects containing website url and title</param> 
    /// <response code="200">Successfully created websites</response>
    [HttpPost("website")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> AddKeyPhrasesToWebsites([FromBody] List<AddWebsiteRequest> websites)
    {
        await Mediator.Send(new AddWebsitesCommand(websites));
        return StatusCode(StatusCodes.Status201Created);
    }
}