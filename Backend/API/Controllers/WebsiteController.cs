using Api.Controllers;
using Application.Websites;
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
}